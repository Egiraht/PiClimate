// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Collections.Generic;
using System.Linq;
using PiClimate.Common.Components;
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
    ///   Defines the hashing key.
    /// </summary>
    public const string HashingKey = nameof(PiClimate) + "." + nameof(Monitor);

    /// <summary>
    ///   Defines the default authentication cookie expiration period in seconds.
    /// </summary>
    public const int DefaultCookieExpirationPeriod = TimePeriods.Week;

    /// <summary>
    ///   The backing field for the <see cref="SignInCredentials" /> property.
    /// </summary>
    private readonly Dictionary<string, string> _signInCredentials = new();

    /// <summary>
    ///   Gets or sets the authentication cookie expiration period in seconds.
    ///   This value is used when the "Remember" option is selected in the login form.
    /// </summary>
    [Comment("Sets the authentication cookie expiration period in seconds.")]
    [Comment("This value is used when the 'Remember' option is selected in the login form.")]
    public int CookieExpirationPeriod { get; set; } = DefaultCookieExpirationPeriod;

    /// <summary>
    ///   Gets or sets the list of sign-in credentials representing user names and corresponding passwords used for
    ///   user authentication.
    /// </summary>
    [Comment("Defines the section of key-value string pairs representing user names and corresponding passwords used " +
      "for user authentication.")]
    [Comment("Both user names and passwords are case-sensitive.")]
    [Comment("The passwords are automatically encrypted on server startup. New passwords should be defined in their " +
      "original form.")]
    [Comment("If no key-value pairs are defined is this section, any user can sign in using a custom name and any " +
      "password.")]
    public Dictionary<string, string> SignInCredentials
    {
      get => _signInCredentials.ToDictionary(pair => pair.Key,
        pair => pair.Value.ToHashedString(HashingKey).ToString());
      set
      {
        foreach (var (name, password) in value)
          _signInCredentials[name] = password.ToHashedString(HashingKey).ToString();
      }
    }
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
