// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using PiClimate.Common.Settings;

namespace PiClimate.Logger.Settings
{
  /// <summary>
  ///   The section of the global settings for data row count limiters.
  /// </summary>
  public class CountLimiterOptions : SettingsSection
  {
    /// <summary>
    ///   Defines the default total data row count limit.
    /// </summary>
    public const int DefaultCountLimit = 1440;

    /// <summary>
    ///   Gets or sets the total count of data rows to keep in the database table.
    /// </summary>
    [Comment("Sets the total count of data rows to keep in the database table.")]
    public int CountLimit { get; set; } = DefaultCountLimit;
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings object for data row count limiters.
    /// </summary>
    [Comment("The settings section dedicated for data row count limiters.")]
    public CountLimiterOptions CountLimiterOptions { get; set; } = new();
  }
}
