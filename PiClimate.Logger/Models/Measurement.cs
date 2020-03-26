// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Globalization;

namespace PiClimate.Logger.Models
{
  /// <summary>
  ///   The climatic data measurement model.
  /// </summary>
  public class Measurement
  {
    /// <summary>
    ///   The timestamp of the measurement.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    ///   The measured pressure value.
    /// </summary>
    public double Pressure { get; set; }

    /// <summary>
    ///   The measured temperature value.
    /// </summary>
    public double Temperature { get; set; }

    /// <summary>
    ///   The measured humidity value.
    /// </summary>
    public double Humidity { get; set; }

    /// <summary>
    ///   Gets the string representation of the measurement.
    /// </summary>
    /// <returns>
    ///   The formatted measurement's <see cref="Timestamp" />, <see cref="Pressure" />, <see cref="Temperature" />,
    ///   and <see cref="Humidity" /> values.
    /// </returns>
    public override string ToString() =>
      $"[{Timestamp.ToString(CultureInfo.InvariantCulture)}] P = {Pressure:F2} mmHg, T = {Temperature:F2} °C, H = {Humidity:F2}%";
  }
}
