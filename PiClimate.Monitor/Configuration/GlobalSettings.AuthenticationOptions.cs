// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System.Collections.Generic;

namespace PiClimate.Monitor.Configuration
{
  /// <summary>
  ///   The section of the global settings for authentication options.
  /// </summary>
  public class AuthenticationOptions
  {
    /// <summary>
    ///   Defines the default authentication cookie expiration period in days.
    /// </summary>
    public const int DefaultCookieExpirationPeriod = 7;

    /// <summary>
    ///   Defines the authentication cookie expiration period in days used when the "Remember me" option is active.
    /// </summary>
    public int CookieExpirationPeriod { get; set; } = DefaultCookieExpirationPeriod;

    /// <summary>
    ///   Defines the list of key-value pairs representing user names and corresponding passwords used for signing in.
    /// </summary>
    public Dictionary<string, string> LoginPairs { get; set; } = new Dictionary<string, string>();
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings for time period limiters.
    /// </summary>
    public AuthenticationOptions AuthenticationOptions { get; set; } = new AuthenticationOptions();
  }
}
