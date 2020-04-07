// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using PiClimate.Monitor.ConfigurationLayout;
using PiClimate.Monitor.Models;

namespace PiClimate.Monitor.Pages
{
  public class Auth : PageModel
  {
    public const string SchemeName = CookieAuthenticationDefaults.AuthenticationScheme;

    public const string ReturnQueryParameterName = nameof(Return);

    public const string CookieName = "AuthToken";

    public const string DefaultReturnPath = "/";

    private static readonly TimeSpan DefaultCookieExpirationPeriod = TimeSpan.FromDays(7);

    public static string LoginHandler => ExtractHandlerName(nameof(OnPostLogin));

    public static string LogoutHandler => ExtractHandlerName(nameof(OnPostLogout));

    public static string LoginPath => $"/{nameof(Auth)}/{LoginHandler}";

    public static string LogoutPath => $"/{nameof(Auth)}/{LogoutHandler}";

    private readonly TimeSpan _cookieExpirationPeriod;

    private readonly IDictionary<string, string> _loginPairs;

    [BindProperty]
    public LoginForm LoginForm { get; set; } = new LoginForm();

    [BindProperty]
    public string? Return { get; set; }

    public Auth(IConfiguration configuration)
    {
      _cookieExpirationPeriod = double.TryParse(configuration[Authentication.CookieExpirationPeriod], NumberStyles.Any,
        NumberFormatInfo.InvariantInfo, out var value)
        ? TimeSpan.FromDays(value)
        : DefaultCookieExpirationPeriod;

      _loginPairs = configuration.GetSection(Authentication.LoginPairs)
        .GetChildren()
        .ToDictionary(element => element.Key, element => element.Value);
    }

    public IActionResult OnGetLogin() => Page();

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

    public IActionResult OnPostLogout()
    {
      var properties = new AuthenticationProperties {RedirectUri = Return ?? DefaultReturnPath};
      return SignOut(properties, SchemeName);
    }

    private ClaimsPrincipal? FindUser(LoginForm loginForm)
    {
      if (!_loginPairs.ContainsKey(loginForm.Name) || _loginPairs[loginForm.Name] != loginForm.Password)
        return null;

      var claims = new Claim[]
      {
        new Claim(ClaimTypes.Name, "PiClimate"),
        new Claim(ClaimTypes.Role, "User")
      };

      return new ClaimsPrincipal(new ClaimsIdentity(claims, SchemeName, ClaimTypes.Name, ClaimTypes.Role));
    }

    private static string ExtractHandlerName(string methodName)
    {
      const string pageHandlerRegExp = @"^On(?:Get|Post|Patch|Put|Delete)(\w*)(?:Async)?$";

      var match = Regex.Match(methodName, pageHandlerRegExp, RegexOptions.IgnoreCase);
      return match.Success && match.Groups.Count == 2 ? match.Groups[1].Value : "";
    }
  }
}
