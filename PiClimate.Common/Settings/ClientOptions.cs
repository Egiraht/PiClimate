// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using PiClimate.Common.Components;

namespace PiClimate.Common.Settings
{
  /// <summary>
  ///   The section of the global settings that is shared with the WebAssembly browser client.
  /// </summary>
  public class ClientOptions : SettingsSection
  {
    /// <summary>
    ///   Defines the default status page time scale in seconds.
    /// </summary>
    public const int DefaultStatusPageTimeScale = TimePeriods.Day;

    /// <summary>
    ///   Defines the default time period in seconds after that the latest data expires.
    /// </summary>
    public const int DefaultLatestDataExpirationPeriod = 10 * TimePeriods.Minute;

    /// <summary>
    ///   Gets or sets the time scale period in seconds to be used for data visualization on the status page.
    /// </summary>
    [Comment("Sets the time scale period in seconds to be used for data visualization on the status page.")]
    public int StatusPageTimeScale { get; set; } = DefaultStatusPageTimeScale;

    /// <summary>
    ///   Gets or sets the time period in seconds from the latest logged measurement after that the latest data expires,
    ///   so the corresponding warning will be displayed. This feature helps to detect communication failures on the
    ///   measurement logger side.
    /// </summary>
    [Comment("Sets the time period in seconds from the latest logged measurement after that the latest data expires.")]
    [Comment("On expiration the corresponding warning will be displayed.")]
    [Comment("This feature helps to detect communication failures on the measurement logger side.")]
    public int LatestDataExpirationPeriod { get; set; } = DefaultLatestDataExpirationPeriod;
  }
}
