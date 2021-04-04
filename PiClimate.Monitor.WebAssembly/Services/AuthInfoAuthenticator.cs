// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using PiClimate.Common;
using PiClimate.Common.Components;
using PiClimate.Common.Models;
using PiClimate.Monitor.WebAssembly.Components;

namespace PiClimate.Monitor.WebAssembly.Services
{
  /// <summary>
  ///   The authenticator service class that uses a login form for authentication on the server.
  ///   The acquired authentication information is stored locally.
  /// </summary>
  public class AuthInfoAuthenticator : AuthenticationStateProvider, IUserAuthenticator<LoginForm, AuthInfo>
  {
    /// <summary>
    ///   The storage provider service instance.
    /// </summary>
    private readonly IStorageProvider _storageProvider;

    /// <summary>
    ///   The HTTP client factory service instance.
    /// </summary>
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    ///   Creates a new authentication service instance.
    /// </summary>
    /// <param name="storageProvider">
    ///   The storage provider service instance.
    ///   Provided via dependency injection.
    /// </param>
    /// <param name="httpClientFactory">
    ///   The HTTP client factory service instance.
    ///   Provided via dependency injection.
    /// </param>
    public AuthInfoAuthenticator(IStorageProvider storageProvider, IHttpClientFactory httpClientFactory)
    {
      _storageProvider = storageProvider;
      _httpClientFactory = httpClientFactory;
    }

    /// <inheritdoc />
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
      var authStateTask = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

      try
      {
        var authInfoString = await _storageProvider.GetItemAsync(nameof(AuthInfo));
        if (string.IsNullOrEmpty(authInfoString))
          return await authStateTask;

        var authInfo = JsonSerializer.Deserialize<AuthInfo>(authInfoString);
        if (authInfo == null)
        {
          await _storageProvider.RemoveItemAsync(nameof(AuthInfo));
          return await authStateTask;
        }

        authStateTask = Task.FromResult(
          new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[]
            {
              new(ClaimTypes.Name, authInfo.Name),
              new(ClaimTypes.Role, authInfo.Role)
            },
            nameof(AuthInfo),
            ClaimTypes.Name,
            ClaimTypes.Role))));

        return await authStateTask;
      }
      catch
      {
        await _storageProvider.RemoveItemAsync(nameof(AuthInfo));
        return await authStateTask;
      }
      finally
      {
        NotifyAuthenticationStateChanged(authStateTask);
      }
    }

    /// <inheritdoc />
    /// <exception cref="HttpRequestException">
    ///   Unable to send the HTTP request because of WebAPI endpoint connection failure.
    /// </exception>
    /// <exception cref="HttpResponseException">
    ///   The HTTP status code of the response is not a successful one (200-299) or no authentication information has
    ///   been received from the server.
    /// </exception>
    public async Task<AuthInfo> SignInAsync(LoginForm loginForm)
    {
      try
      {
        // Authenticating on the server by requesting a HTTP-only cookie (server-side signing in).
        using var httpClient = _httpClientFactory.CreateClient();
        var response =
          await httpClient.PostJsonAsync<LoginForm, JsonPayload<AuthInfo>>(ApiEndpoints.UserSignInEndpoint, loginForm);
        if (response?.Data == null)
          throw new HttpResponseException("No authentication information has been received.");

        // Storing the authentication information (client-side signing in).
        await _storageProvider.SetItemAsync(nameof(AuthInfo), JsonSerializer.Serialize(response.Data));

        return response.Data;
      }
      finally
      {
        // Wiping the password for better security.
        loginForm.Password = string.Empty;

        // Validating and updating the authentication state.
        await GetAuthenticationStateAsync();
      }
    }

    /// <inheritdoc />
    /// <exception cref="HttpRequestException">
    ///   Unable to send the HTTP request because of WebAPI endpoint connection failure.
    /// </exception>
    /// <exception cref="HttpResponseException">
    ///   The HTTP status code of the response is not a successful one (200-299).
    /// </exception>
    public async Task SignOutAsync()
    {
      try
      {
        // Trying to remove the HTTP-only cookie using a server request (server-side signing out).
        using var httpClient = _httpClientFactory.CreateClient();
        await httpClient.PostJsonAsync<object, JsonPayload<object>>(ApiEndpoints.UserSignOutEndpoint, new { });
      }
      catch
      {
        // Ignore exceptions.
      }
      finally
      {
        // Removing the authentication information from the storage (client-side signing out).
        await _storageProvider.RemoveItemAsync(nameof(AuthInfo));

        // Validating and updating the authentication state.
        await GetAuthenticationStateAsync();
      }
    }
  }
}
