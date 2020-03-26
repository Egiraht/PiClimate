// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Logger.ConfigurationLayout
{
  /// <summary>
  ///   The configuration's section for data row count limiters.
  /// </summary>
  public static class CountLimiterOptions
  {
    /// <summary>
    ///   Defines the total count of data rows to keep in the database table.
    /// </summary>
    public const string CountLimit =
      nameof(CountLimiterOptions) + ":" + nameof(CountLimit);
  }
}
