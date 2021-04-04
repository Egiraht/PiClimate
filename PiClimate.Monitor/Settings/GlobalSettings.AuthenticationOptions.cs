// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Collections.Generic;
using PiClimate.Common.Components;

namespace PiClimate.Monitor.Settings
{
  /// <summary>
  ///   The section of the global settings for authentication options.
  /// </summary>
  public class AuthenticationOptions
  {
    /// <summary>
    ///   Defines the default authentication cookie expiration period in seconds.
    /// </summary>
    public const int DefaultCookieExpirationPeriod = TimePeriods.Week;

    /// <summary>
    ///   Defines the authentication cookie expiration period in seconds.
    ///   This value is used when the "Remember" option is selected in the login form.
    /// </summary>
    public int CookieExpirationPeriod { get; set; } = DefaultCookieExpirationPeriod;

    /// <summary>
    ///   Defines the list of key-value pairs representing user names and corresponding passwords used for signing in.
    /// </summary>
    public Dictionary<string, string> LoginPairs { get; set; } = new();
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings for time period limiters.
    /// </summary>
    public AuthenticationOptions AuthenticationOptions { get; set; } = new();
  }
}
