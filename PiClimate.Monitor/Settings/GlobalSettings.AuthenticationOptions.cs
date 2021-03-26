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
    ///   Defines the default access token expiration time in seconds.
    /// </summary>
    public const int DefaultAccessTokenExpirationTime = 15 * TimePeriods.Minute;

    /// <summary>
    ///   Defines the default refresh token expiration time in seconds.
    /// </summary>
    public const int DefaultRefreshTokenExpirationTime = TimePeriods.Week;

    /// <summary>
    ///   Defines the default clock skew period in seconds.
    /// </summary>
    public const int DefaultClockSkewPeriod = TimePeriods.Minute;

    /// <summary>
    ///   Gets or sets the signing key used for issuing tokens.
    /// </summary>
    public string TokenSigningKey { get; set; } = $"{nameof(PiClimate)}.{nameof(TokenSigningKey)}";

    /// <summary>
    ///   Gets or sets the signing key used for hash generation.
    /// </summary>
    public string HashSigningKey { get; set; } = $"{nameof(PiClimate)}.{nameof(HashSigningKey)}";

    /// <summary>
    ///   Gets or sets the access token expiration time in seconds.
    /// </summary>
    public int AccessTokenExpirationTime { get; set; } = DefaultAccessTokenExpirationTime;

    /// <summary>
    ///   Gets or sets the refresh token expiration time in seconds.
    /// </summary>
    public int RefreshTokenExpirationTime { get; set; } = DefaultRefreshTokenExpirationTime;

    /// <summary>
    ///   Gets or sets the clock skew period in seconds used to compensate the client-server clock time difference.
    /// </summary>
    public int ClockSkewPeriod { get; set; } = DefaultClockSkewPeriod;

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
