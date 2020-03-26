// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;

namespace PiClimate.Logger.Components
{
  /// <summary>
  ///   The measurement loop options.
  /// </summary>
  public class MeasurementLoopOptions
  {
    private int _measurementLoopDelay = 60;

    /// <summary>
    ///   Gets or sets the measurement loop delay value in seconds.
    /// </summary>
    public int MeasurementLoopDelay
    {
      get => _measurementLoopDelay;
      set => _measurementLoopDelay = Math.Max(value, 1);
    }
  }
}
