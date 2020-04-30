// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PiClimate.Monitor.Configuration;
using PiClimate.Monitor.Models;

namespace PiClimate.Monitor.Pages
{
  /// <summary>
  ///   The user authentication page code-behind class.
  /// </summary>
  public class Auth : PageModel
  {
    /// <summary>
    ///   Defines the default authentication scheme name.
    /// </summary>
    public const string SchemeName = CookieAuthenticationDefaults.AuthenticationScheme;

    /// <summary>
    ///   Defines the query parameter name that contains the redirection path for returning.
    /// </summary>
    public const string ReturnQueryParameterName = nameof(Return);

    /// <summary>
    ///   Defines the authentication cookie name.
    /// </summary>
    public const string CookieName = "AuthToken";

    /// <summary>
    ///   Defines the default return redirection path.
    /// </summary>
    public const string DefaultReturnPath = "/";

    /// <summary>
    ///   Gets the name of the page's login handler.
    /// </summary>
    public static string LoginHandler => ExtractHandlerName(nameof(OnPostLogin));

    /// <summary>
    ///   Gets the name of the page's logout handler.
    /// </summary>
    public static string LogoutHandler => ExtractHandlerName(nameof(OnPostLogout));

    /// <summary>
    ///   Gets the path to the page's login handler.
    /// </summary>
    public static string LoginPath => $"/{nameof(Auth)}/{LoginHandler}";

    /// <summary>
    ///   Gets the path to the page's logout handler.
    /// </summary>
    public static string LogoutPath => $"/{nameof(Auth)}/{LogoutHandler}";

    /// <summary>
    ///   The authentication cookie expiration period taken from the configuration.
    /// </summary>
    private readonly TimeSpan _cookieExpirationPeriod;

    /// <summary>
    ///   The collection of login name-password pairs taken from the configuration.
    /// </summary>
    private readonly IDictionary<string, string> _loginPairs;

    /// <summary>
    ///   Gets the login form bound from the HTTP request.
    /// </summary>
    [BindProperty]
    public LoginForm LoginForm { get; set; } = new LoginForm();

    /// <summary>
    ///   Gets the return redirection path bound from the HTTP request.
    /// </summary>
    [BindProperty]
    public string? Return { get; set; }

    /// <summary>
    ///   Initializes the new instance of the page.
    /// </summary>
    /// <param name="settings">
    ///   The global settings used for configuring the MySQL connection.
    ///   Provided via dependency injection.
    /// </param>
    public Auth(GlobalSettings settings)
    {
      _cookieExpirationPeriod = TimeSpan.FromDays(settings.AuthenticationOptions.CookieExpirationPeriod);
      _loginPairs = settings.AuthenticationOptions.LoginPairs;
    }

    /// <summary>
    ///   The login callback handler for GET HTTP requests.
    /// </summary>
    /// <returns>
    ///   The processed page view result.
    /// </returns>
    public IActionResult OnGetLogin() => Page();

    /// <summary>
    ///   The login callback handler for POST HTTP requests.
    /// </summary>
    /// <returns>
    ///   The return redirection result on successful user authentication or the processed page view result with
    ///   the form validation errors otherwise.
    /// </returns>
    public IActionResult OnPostLogin()
    {
      if (!ModelState.IsValid)
        return Page();

      var user = FindUser(LoginForm);
      if (user == null)
      {
        ModelState.AddModelError("", "There is no user matching the provided name and password.");
        return Page();
      }

      var properties = new AuthenticationProperties
      {
        AllowRefresh = true,
        IssuedUtc = DateTimeOffset.Now,
        ExpiresUtc = DateTimeOffset.Now + _cookieExpirationPeriod,
        IsPersistent = LoginForm.Remember,
        RedirectUri = Return ?? DefaultReturnPath
      };
      return SignIn(user, properties, SchemeName);
    }

    /// <summary>
    ///   The login callback handler for POST HTTP requests.
    /// </summary>
    /// <returns>
    ///   The return redirection result after user's logout.
    /// </returns>
    public IActionResult OnPostLogout()
    {
      var properties = new AuthenticationProperties {RedirectUri = Return ?? DefaultReturnPath};
      return SignOut(properties, SchemeName);
    }

    /// <summary>
    ///   Finds the user in the collection of available login name-password pairs.
    /// </summary>
    /// <param name="loginForm">
    ///   The login form containing the login name-password pair to find.
    /// </param>
    /// <returns>
    ///   The <see cref="ClaimsPrincipal" /> object representing the authenticated user if the user was found in
    ///   the collection, otherwise returns <c>null</c>.
    /// </returns>
    private ClaimsPrincipal? FindUser(LoginForm loginForm)
    {
      if (!_loginPairs.ContainsKey(loginForm.Name) || _loginPairs[loginForm.Name] != loginForm.Password)
        return null;

      var claims = new[]
      {
        new Claim(ClaimTypes.Name, "PiClimate"),
        new Claim(ClaimTypes.Role, "User")
      };

      return new ClaimsPrincipal(new ClaimsIdentity(claims, SchemeName, ClaimTypes.Name, ClaimTypes.Role));
    }

    /// <summary>
    ///   Extracts the handler name from the provided name of the Razor Page handler method.
    /// </summary>
    /// <param name="methodName">
    ///   The name of the Razor Page handler method.
    /// </param>
    /// <returns>
    ///   The extracted handler name on success, otherwise returns an empty string.
    /// </returns>
    private static string ExtractHandlerName(string methodName)
    {
      const string pageHandlerRegExp = @"^On(?:Get|Post|Patch|Put|Delete)(\w*)(?:Async)?$";

      var match = Regex.Match(methodName, pageHandlerRegExp, RegexOptions.IgnoreCase);
      return match.Success && match.Groups.Count == 2 ? match.Groups[1].Value : "";
    }
  }
}
