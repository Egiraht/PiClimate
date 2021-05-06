// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using PiClimate.Common.Settings;

namespace PiClimate.Logger.Settings
{
  /// <summary>
  ///   The section of the global settings for time period limiters.
  /// </summary>
  public class PeriodLimiterOptions : SettingsSection
  {
    /// <summary>
    ///   Defines the default time period limiting value in seconds.
    /// </summary>
    public const int DefaultPeriodLimit = 86400;

    /// <summary>
    ///   Gets or sets the time period in seconds that limits the data row lifetime.
    /// </summary>
    [Comment("Sets the time period in seconds that limits the data row lifetime.")]
    public int PeriodLimit { get; set; } = DefaultPeriodLimit;
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings object for time period limiters.
    /// </summary>
    [Comment("The settings section dedicated for time period limiters.")]
    public PeriodLimiterOptions PeriodLimiterOptions { get; set; } = new();
  }
}
