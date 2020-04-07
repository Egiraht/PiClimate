// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Monitor.ConfigurationLayout
{
  /// <summary>
  ///   The configuration's section for authentication options.
  /// </summary>
  public static class Authentication
  {
    /// <summary>
    ///   Defines the authentication cookie expiration period in days used when the "Remember me" option is active.
    /// </summary>
    public const string CookieExpirationPeriod =
      nameof(Authentication) + ":" + nameof(CookieExpirationPeriod);

    /// <summary>
    ///   Defines the list of key-value pairs representing user names and corresponding passwords used for signing in.
    /// </summary>
    public const string LoginPairs =
      nameof(Authentication) + ":" + nameof(LoginPairs);
  }
}
