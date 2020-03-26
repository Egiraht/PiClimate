// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Logger.ConfigurationLayout
{
  /// <summary>
  ///   The configuration's section for period limiters.
  /// </summary>
  public static class PeriodLimiterOptions
  {
    /// <summary>
    ///   Defines the time period in seconds that limits the data row lifetime.
    /// </summary>
    public const string PeriodLimit =
      nameof(PeriodLimiterOptions) + ":" + nameof(PeriodLimit);
  }
}
