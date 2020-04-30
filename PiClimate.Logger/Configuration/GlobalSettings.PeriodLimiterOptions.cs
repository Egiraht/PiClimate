// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Logger.Configuration
{
  /// <summary>
  ///   The section of the global settings for time period limiters.
  /// </summary>
  public class PeriodLimiterOptions
  {
    /// <summary>
    ///   Defines the default time period limiting value in seconds.
    /// </summary>
    public const int DefaultPeriodLimit = 86400;

    /// <summary>
    ///   Gets or sets the time period in seconds that limits the data row lifetime.
    /// </summary>
    public int PeriodLimit { get; set; } = DefaultPeriodLimit;
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings for time period limiters.
    /// </summary>
    public PeriodLimiterOptions PeriodLimiterOptions { get; set; } = new PeriodLimiterOptions();
  }
}
