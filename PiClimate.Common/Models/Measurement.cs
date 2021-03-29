// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Globalization;
using System.Text.Json.Serialization;
using UnitsNet;

namespace PiClimate.Common.Models
{
  /// <summary>
  ///   A record containing a single climatic data measurement.
  /// </summary>
  public record Measurement
  {
    /// <summary>
    ///   The timestamp of the measurement.
    /// </summary>
    [JsonPropertyName("d")]
    public DateTime Timestamp { get; init; }

    /// <summary>
    ///   The convertible measured pressure magnitude.
    /// </summary>
    [JsonIgnore]
    public Pressure Pressure { get; init; }

    /// <summary>
    ///   The measured pressure value expressed in millimeters of mercury.
    /// </summary>
    [JsonPropertyName("p")]
    public double PressureInMmHg
    {
      get => Pressure.MillimetersOfMercury;
      init => Pressure = Pressure.FromMillimetersOfMercury(value);
    }

    /// <summary>
    ///   The convertible measured temperature magnitude.
    /// </summary>
    [JsonIgnore]
    public Temperature Temperature { get; init; }

    /// <summary>
    ///   The measured temperature value expressed in degrees Celsius.
    /// </summary>
    [JsonPropertyName("t")]
    public double TemperatureInDegC
    {
      get => Temperature.DegreesCelsius;
      init => Temperature = Temperature.FromDegreesCelsius(value);
    }

    /// <summary>
    ///   The convertible measured humidity magnitude.
    /// </summary>
    [JsonIgnore]
    public RelativeHumidity Humidity { get; init; }

    /// <summary>
    ///   The measured humidity value expressed in percent.
    /// </summary>
    [JsonPropertyName("h")]
    public double HumidityInPercent
    {
      get => Humidity.Percent;
      init => Humidity = RelativeHumidity.FromPercent(value);
    }

    /// <summary>
    ///   Gets the string representation of the measurement using the provided format provider.
    /// </summary>
    /// <param name="formatProvider">
    ///   The format provider object used for formatting the value.
    /// </param>
    /// <returns>
    ///   The formatted measurement's <see cref="Timestamp" />, <see cref="Pressure" />, <see cref="Temperature" />,
    ///   and <see cref="Humidity" /> values.
    /// </returns>
    public string ToString(IFormatProvider formatProvider) =>
      $"[{Timestamp.ToString(formatProvider)}] " +
      $"P = {Pressure.ToString(formatProvider)}, " +
      $"T = {Temperature.ToString(formatProvider)}, " +
      $"H = {Humidity.ToString(formatProvider)}";

    /// <summary>
    ///   Gets the string representation of the measurement using the culture-invariant formatting.
    /// </summary>
    /// <returns>
    ///   The formatted measurement's <see cref="Timestamp" />, <see cref="Pressure" />, <see cref="Temperature" />,
    ///   and <see cref="Humidity" /> values.
    /// </returns>
    public override string ToString() => ToString(CultureInfo.InvariantCulture);
  }
}
