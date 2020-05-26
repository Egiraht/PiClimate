// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;

namespace PiClimate.Monitor.Models
{
  /// <summary>
  ///   A model class for requesting the latest data measurements.
  /// </summary>
  public class LatestDataRequest
  {
    private int _maxRows = DefaultLatestRowsCount;

    /// <summary>
    ///   Defines the default number of the latest data rows to take.
    /// </summary>
    public const int DefaultLatestRowsCount = 10;

    /// <summary>
    ///   Defines the maximal number of the latest data rows to take.
    /// </summary>
    public const int MaxLatestRowsCount = 50;

    /// <summary>
    ///   Gets or sets the maximal number of the latest data rows to take.
    ///   The value defaults to <see cref="DefaultLatestRowsCount" /> and must be in range between 1 and
    ///   <see cref="MaxLatestRowsCount" />.
    /// </summary>
    public int MaxRows
    {
      get => _maxRows;
      set => _maxRows = Math.Clamp(value, 1, MaxLatestRowsCount);
    }
  }
}
