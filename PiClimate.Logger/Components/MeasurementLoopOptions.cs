// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using PiClimate.Logger.Configuration;

namespace PiClimate.Logger.Components
{
  /// <summary>
  ///   The measurement loop options.
  /// </summary>
  public class MeasurementLoopOptions
  {
    private int _measurementLoopDelay = GlobalSettings.DefaultMeasurementLoopDelay;

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
