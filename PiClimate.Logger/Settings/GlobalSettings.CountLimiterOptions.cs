// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Logger.Settings
{
  /// <summary>
  ///   The section of the global settings for data row count limiters.
  /// </summary>
  public class CountLimiterOptions
  {
    /// <summary>
    ///   Defines the default total data row count limit.
    /// </summary>
    public const int DefaultCountLimit = 1440;

    /// <summary>
    ///   Gets or sets the total count of data rows to keep in the database table.
    /// </summary>
    public int CountLimit { get; set; } = DefaultCountLimit;
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings for data row count limiters.
    /// </summary>
    public CountLimiterOptions CountLimiterOptions { get; set; } = new();
  }
}
