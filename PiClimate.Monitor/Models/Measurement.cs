// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Text.Json.Serialization;

namespace PiClimate.Monitor.Models
{
  /// <summary>
  ///   A model class representing a single climatic data measurement.
  /// </summary>
  public class Measurement
  {
    /// <summary>
    ///   The measurement timestamp.
    /// </summary>
    [JsonPropertyName("d")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    ///   The measured pressure value expressed in mmHg.
    /// </summary>
    [JsonPropertyName("p")]
    public double Pressure { get; set; }

    /// <summary>
    ///   The measured temperature value expressed in degrees Celsius.
    /// </summary>
    [JsonPropertyName("t")]
    public double Temperature { get; set; }

    /// <summary>
    ///   The measured humidity value expressed in percents.
    /// </summary>
    [JsonPropertyName("h")]
    public double Humidity { get; set; }
  }
}
