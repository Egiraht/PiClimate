// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PiClimate.Common;
using PiClimate.Common.Models;
using PiClimate.Monitor.Components;
using PiClimate.Monitor.Settings;
using AuthenticationOptions = PiClimate.Monitor.Settings.AuthenticationOptions;

namespace PiClimate.Monitor.Controllers
{
  /// <summary>
  ///   The user authentication API controller class.
  /// </summary>
  [ApiController]
  public class Auth : Controller
  {
    /// <summary>
    ///   Defines the default authentication scheme name.
    /// </summary>
    public const string SchemeName = CookieAuthenticationDefaults.AuthenticationScheme;

    /// <summary>
    ///   Defines the authentication cookie name.
    /// </summary>
    public const string CookieName = "AuthToken";

    /// <summary>
    ///   Defines the default user role.
    /// </summary>
    public const string DefaultUserRole = "User";

    /// <summary>
    ///   The authentication cookie expiration period taken from the configuration.
    /// </summary>
    private readonly TimeSpan _cookieExpirationPeriod;

    /// <summary>
    ///   The collection of user names and corresponding hashed passwords taken from the configuration.
    /// </summary>
    private readonly IDictionary<string, string> _signInCredentials;

    /// <summary>
    ///   Initializes the new instance of the controller.
    /// </summary>
    /// <param name="settings">
    ///   The global settings object.
    ///   Provided via dependency injection.
    /// </param>
    public Auth(GlobalSettings settings)
    {
      _cookieExpirationPeriod = TimeSpan.FromSeconds(settings.AuthenticationOptions.CookieExpirationPeriod);
      _signInCredentials = settings.AuthenticationOptions.SignInCredentials;
    }

    /// <summary>
    ///   Finds the user in the collection of available sign-in credentials.
    ///   If no sign-in credentials are defined, return a default user with the provided user name.
    /// </summary>
    /// <param name="loginForm">
    ///   The login form containing the sign-in credentials.
    /// </param>
    /// <returns>
    ///   The <see cref="ClaimsPrincipal" /> object representing the authenticated user if the user was found in
    ///   the collection, otherwise returns <c>null</c>.
    /// </returns>
    [NonAction]
    private ClaimsPrincipal? FindUser(LoginForm loginForm)
    {
      if (_signInCredentials.Any() && (!_signInCredentials.ContainsKey(loginForm.Name) ||
        !_signInCredentials[loginForm.Name].ToHashedString().ValidateOriginalValue(loginForm.Password,
          AuthenticationOptions.HashingKey)))
        return null;

      var claims = new Claim[]
      {
        new(ClaimTypes.Name, loginForm.Name),
        new(ClaimTypes.Role, DefaultUserRole)
      };

      return new ClaimsPrincipal(new ClaimsIdentity(claims, SchemeName, ClaimTypes.Name, ClaimTypes.Role));
    }

    /// <summary>
    ///   Signs the user in using a POST request.
    /// </summary>
    /// <param name="loginForm">
    ///   The login form object.
    /// </param>
    /// <returns>
    ///   A response JSON object containing the operation result.
    /// </returns>
    [HttpPost(ApiEndpoints.UserSignInEndpoint)]
    public async Task<IActionResult> OnPostSignInAsync([FromBody] LoginForm loginForm)
    {
      var user = FindUser(loginForm);
      if (user == null)
        return new EmptyJsonResponse(StatusCodes.Status406NotAcceptable,
          "There is no user matching the provided name and password.");

      var properties = new AuthenticationProperties
      {
        AllowRefresh = true,
        IssuedUtc = DateTimeOffset.Now,
        ExpiresUtc = DateTimeOffset.Now + _cookieExpirationPeriod,
        IsPersistent = loginForm.Remember
      };

      await SignIn(user, properties, SchemeName).ExecuteResultAsync(ControllerContext);
      return new JsonResponse<AuthInfo>(new()
      {
        Name = user.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
        Role = user.FindFirstValue(ClaimTypes.Role) ?? string.Empty
      });
    }

    /// <summary>
    ///   Signs the user out using GET and POST requests.
    /// </summary>
    /// <returns>
    ///   An empty response JSON object.
    /// </returns>
    [AcceptVerbs("GET", "POST", Route = ApiEndpoints.UserSignOutEndpoint)]
    public async Task<IActionResult> OnSignOutAsync()
    {
      await SignOut(SchemeName).ExecuteResultAsync(ControllerContext);
      return new EmptyJsonResponse();
    }
  }
}
