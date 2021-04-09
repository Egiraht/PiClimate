// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Collections.Generic;
using PiClimate.Common.Settings;
using PiClimate.Monitor.Components;

namespace PiClimate.Monitor.Settings
{
  /// <summary>
  ///   The section of the global settings for user authentication options.
  /// </summary>
  public class AuthenticationOptions : SettingsSection
  {
    /// <summary>
    ///   Defines the default authentication cookie expiration period in seconds.
    /// </summary>
    public const int DefaultCookieExpirationPeriod = TimePeriods.Week;

    /// <summary>
    ///   Gets or sets the authentication cookie expiration period in seconds.
    ///   This value is used when the "Remember" option is selected in the login form.
    /// </summary>
    [Comment("Sets the authentication cookie expiration period in seconds.")]
    [Comment("This value is used when the 'Remember' option is selected in the login form.")]
    [Comment("Can be a floating point numeric value.")]
    public int CookieExpirationPeriod { get; set; } = DefaultCookieExpirationPeriod;

    /// <summary>
    ///   Gets or sets the list of key-value pairs representing user names and corresponding passwords used for
    ///   signing in.
    /// </summary>
    [Comment("Defines the section of key-value pairs representing user names and corresponding passwords used for " +
      "signing in.")]
    [Comment("Both user names and passwords are case-sensitive.")]
    [Comment("If no login pais are defined, any user can sign in using a custom name and any password.")]
    public Dictionary<string, string> LoginPairs { get; set; } = new();
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings object for user authentication options.
    /// </summary>
    [Comment("The settings section related to the user authentication options.")]
    public AuthenticationOptions AuthenticationOptions { get; set; } = new();
  }
}
