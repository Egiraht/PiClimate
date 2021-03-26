// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using PiClimate.Common.Components;
using PiClimate.Common.Models;
using PiClimate.Monitor.WebAssembly.Components;

namespace PiClimate.Monitor.WebAssembly.Services
{
  /// <summary>
  ///   The JWT-based authentication state provider service class.
  /// </summary>
  public class JwtUserAuthenticator : AuthenticationStateProvider, IUserAuthenticator, IAccessTokenProvider
  {
    private AuthTokens _authTokens = new();

    /// <summary>
    ///   Defines the key name in the storage locating the refresh token.
    /// </summary>
    private const string AuthTokensStorageKeyName = "AuthTokens";

    /// <summary>
    ///   The <see cref="HttpClient" /> service instance used for server communication.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    ///   The local storage provider service instance used for authentication tokens storage.
    /// </summary>
    private readonly IStorageProvider _storageProvider;

    /// <summary>
    ///   Defines the server's authentication login endpoint path.
    /// </summary>
    public string LoginEndpointPath { get; init; } = "";

    /// <summary>
    ///   Defines the server's authentication refresh endpoint path.
    /// </summary>
    public string RefreshEndpointPath { get; init; } = "";

    /// <inheritdoc />
    public string AuthenticationSchemeName => "Bearer";

    /// <inheritdoc />
    public bool IsAuthenticating { get; private set; } = false;

    /// <inheritdoc />
    public ClaimsPrincipal User { get; private set; } = new(new ClaimsIdentity());

    /// <summary>
    ///   Gets or sets a pair of JWT authentication tokens.
    /// </summary>
    private AuthTokens AuthTokens
    {
      get => _authTokens;
      set
      {
        _authTokens = value;

        // Updating the user data.
        User = ValidateToken(_authTokens.AccessToken)
          ? new ClaimsPrincipal(new ClaimsIdentity(new JwtSecurityToken(AuthTokens.AccessToken).Claims,
            AuthenticationSchemeName, ClaimTypes.Name, ClaimTypes.Role))
          : new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
      }
    }

    /// <summary>
    ///   Creates a new instance of JWT user authenticator.
    /// </summary>
    /// <param name="httpClient">
    ///   The <see cref="HttpClient" /> service instance used for server communication.
    /// </param>
    /// <param name="storageProvider">
    ///   The local storage provider service instance used for authentication tokens storage.
    /// </param>
    public JwtUserAuthenticator(HttpClient httpClient, IStorageProvider storageProvider)
    {
      _httpClient = httpClient;
      _storageProvider = storageProvider;
    }

    /// <summary>
    ///   Gets the authentication state taken from the JWT token.
    /// </summary>
    /// <returns>
    ///   The <see cref="AuthenticationState" /> object containing the user authentication data.
    /// </returns>
    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
      Task.FromResult(new AuthenticationState(User));

    /// <summary>
    ///   Encodes the authentication tokens into the single string representation.
    /// </summary>
    /// <param name="authTokens">
    ///   The <see cref="AuthTokens" /> instance to encode.
    /// </param>
    /// <returns>
    ///   The string representation of the authentication tokens.
    /// </returns>
    private static string EncodeTokens(AuthTokens authTokens) => $"{authTokens.AccessToken};{authTokens.RefreshToken}";

    /// <summary>
    ///   Decodes the authentication tokens from the single string representation.
    /// </summary>
    /// <param name="authTokensString">
    ///   The single string representation of the authentication tokens.
    /// </param>
    /// <returns>
    ///   The decoded <see cref="AuthTokens" /> instance.
    /// </returns>
    private static AuthTokens DecodeTokens(string? authTokensString)
    {
      var authTokens = authTokensString?.Split(';', 2) ?? Array.Empty<string>();
      if (authTokens.Length != 2)
        return new AuthTokens();

      return new AuthTokens
      {
        AccessToken = authTokens[0],
        RefreshToken = authTokens[1]
      };
    }

    /// <summary>
    ///   Asynchronously signs the user in using the provided login credentials.
    ///   The method throws an exception on operation failure.
    /// </summary>
    /// <param name="loginForm">
    ///   The user's login credentials.
    /// </param>
    /// <exception cref="Exception">
    ///   Failed to get the authentication tokens from the server.
    ///   More detailed information is provided in the <see cref="Exception.InnerException" /> property.
    /// </exception>
    public async Task SignInAsync(LoginForm loginForm)
    {
      if (IsAuthenticating)
        return;

      try
      {
        IsAuthenticating = true;

        // Signing in the user using the POST request to the server.
        AuthTokens =
          (await _httpClient.PostJsonAsync<LoginForm, JsonPayload<AuthTokens>>(LoginEndpointPath, loginForm))
          ?.Data ?? new AuthTokens();

        // Saving the tokens in the storage if necessary.
        if (loginForm.Remember)
          await _storageProvider.SetItemAsync(AuthTokensStorageKeyName, EncodeTokens(AuthTokens));
        else
          await _storageProvider.RemoveItemAsync(AuthTokensStorageKeyName);
      }
      catch (Exception e)
      {
        // Signing the user out on exception.
        await SignOutAsync();

        // Throwing the authentication exception with the status message from the HTTP response if it is available,
        // or with the caught exception's message otherwise.
        var message = e is HttpResponseException httpResponseException && httpResponseException.Response != null &&
          await httpResponseException.Response.Content.ReadFromJsonAsync<JsonPayload<AuthTokens>>() is { } payload &&
          !string.IsNullOrWhiteSpace(payload.Description)
            ? payload.Description
            : e.Message;
        throw new Exception($"Failed to sign in: {message}", e);
      }
      finally
      {
        IsAuthenticating = false;
      }
    }

    /// <inheritdoc />
    public async Task SignOutAsync()
    {
      AuthTokens = new AuthTokens();
      await _storageProvider.RemoveItemAsync(AuthTokensStorageKeyName);
    }

    /// <inheritdoc />
    public async Task RestoreSessionAsync()
    {
      try
      {
        // Loading the authentication tokens from the storage if they are available.
        AuthTokens = DecodeTokens(await _storageProvider.GetItemAsync(AuthTokensStorageKeyName));

        // Refreshing the access token if it is no more valid while the refresh token is.
        // Signing out if both tokens are no more valid or if refreshing fails.
        if (ValidateToken(AuthTokens.RefreshToken))
        {
          if (!ValidateToken(AuthTokens.AccessToken))
            await RefreshTokensAsync();
        }
        else
          await SignOutAsync();
      }
      catch
      {
        await SignOutAsync();
      }
    }

    /// <summary>
    ///   Checks if the provided JWT token is valid.
    /// </summary>
    /// <param name="token">
    ///   The string representation of the JWT token to check.
    /// </param>
    /// <returns>
    ///   <c>true</c> if the <paramref name="token" /> is still valid, otherwise <c>false</c>.
    /// </returns>
    private static bool ValidateToken(string? token)
    {
      try
      {
        return DateTime.Now.ToUniversalTime() < new JwtSecurityToken(token).ValidTo;
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    ///   Asynchronously refreshes the authentication tokens.
    ///   The method throws an exception on operation failure.
    /// </summary>
    /// <exception cref="Exception">
    ///   Failed to get the authentication tokens from the server.
    ///   More detailed information is provided in the <see cref="Exception.InnerException" /> property.
    /// </exception>
    private async Task RefreshTokensAsync()
    {
      if (IsAuthenticating)
        return;

      try
      {
        IsAuthenticating = true;

        // Refreshing the authentication tokens using the POST request to the server.
        AuthTokens =
          (await _httpClient.PostJsonAsync<AuthTokens, JsonPayload<AuthTokens>>(RefreshEndpointPath, AuthTokens))
          ?.Data ?? new AuthTokens();

        // Updating the authentication tokens in the storage if they exist.
        if (await _storageProvider.GetItemAsync(AuthTokensStorageKeyName) != null)
          await _storageProvider.SetItemAsync(AuthTokensStorageKeyName, EncodeTokens(AuthTokens));
      }
      catch (Exception e)
      {
        // Signing the user out on exception.
        await SignOutAsync();

        // Throwing the authentication exception with the status message from the HTTP response if it is available,
        // or with the caught exception's message otherwise.
        var message = e is HttpResponseException httpResponseException && httpResponseException.Response != null &&
          await httpResponseException.Response.Content.ReadFromJsonAsync<JsonPayload<AuthTokens>>() is { } payload &&
          !string.IsNullOrWhiteSpace(payload.Description)
            ? payload.Description
            : e.Message;
        throw new Exception($"Failed to authorize the request: {message}", e);
      }
      finally
      {
        IsAuthenticating = false;
      }
    }

    /// <inheritdoc />
    public async Task<string?> GetAccessTokenValueAsync()
    {
      // Refresh the authentication tokens if the access token is invalid.
      if (!ValidateToken(AuthTokens.AccessToken))
        await RefreshTokensAsync();

      return AuthTokens.AccessToken;
    }
  }
}
