// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Linq;

namespace PiClimate.Monitor.Models
{
  /// <summary>
  ///   A model class representing a collection of climatic data measurements.
  /// </summary>
  public class MeasurementsCollection
  {
    /// <summary>
    ///   Gets the minimal timestamp available in the collection.
    /// </summary>
    public DateTime MinTimestamp => Count > 0 ? Measurements.Min(m => m.Timestamp) : DateTime.Now;

    /// <summary>
    ///   Gets the maximal timestamp available in the collection.
    /// </summary>
    public DateTime MaxTimestamp => Count > 0 ? Measurements.Max(m => m.Timestamp) : DateTime.Now;

    /// <summary>
    ///   Gets the minimal pressure value available in the collection.
    /// </summary>
    public double MinPressure => Count > 0 ? Measurements.Min(m => m.Pressure) : default;

    /// <summary>
    ///   Gets the maximal pressure value available in the collection.
    /// </summary>
    public double MaxPressure => Count > 0 ? Measurements.Max(m => m.Pressure) : default;

    /// <summary>
    ///   Gets the timestamp corresponding to the <see cref="MinPressure" /> value.
    /// </summary>
    public DateTime MinPressureTimestamp =>
      Count > 0 ? Measurements.First(m => m.Pressure.Equals(MinPressure)).Timestamp : DateTime.Now;

    /// <summary>
    ///   Gets the timestamp corresponding to the <see cref="MaxPressure" /> value.
    /// </summary>
    public DateTime MaxPressureTimestamp =>
      Count > 0 ? Measurements.First(m => m.Pressure.Equals(MaxPressure)).Timestamp : DateTime.Now;

    /// <summary>
    ///   Gets the minimal temperature value available in the collection.
    /// </summary>
    public double MinTemperature => Count > 0 ? Measurements.Min(m => m.Temperature) : default;

    /// <summary>
    ///   Gets the maximal temperature value available in the collection.
    /// </summary>
    public double MaxTemperature => Count > 0 ? Measurements.Max(m => m.Temperature) : default;

    /// <summary>
    ///   Gets the timestamp corresponding to the <see cref="MinTemperature" /> value.
    /// </summary>
    public DateTime MinTemperatureTimestamp =>
      Count > 0 ? Measurements.First(m => m.Temperature.Equals(MinTemperature)).Timestamp : DateTime.Now;

    /// <summary>
    ///   Gets the timestamp corresponding to the <see cref="MaxTemperature" /> value.
    /// </summary>
    public DateTime MaxTemperatureTimestamp =>
      Count > 0 ? Measurements.First(m => m.Temperature.Equals(MaxTemperature)).Timestamp : DateTime.Now;

    /// <summary>
    ///   Gets the minimal humidity value available in the collection.
    /// </summary>
    public double MinHumidity => Count > 0 ? Measurements.Min(m => m.Humidity) : default;

    /// <summary>
    ///   Gets the maximal humidity value available in the collection.
    /// </summary>
    public double MaxHumidity => Count > 0 ? Measurements.Max(m => m.Humidity) : default;

    /// <summary>
    ///   Gets the timestamp corresponding to the <see cref="MinHumidity" /> value.
    /// </summary>
    public DateTime MinHumidityTimestamp =>
      Count > 0 ? Measurements.First(m => m.Humidity.Equals(MinHumidity)).Timestamp : DateTime.Now;

    /// <summary>
    ///   Gets the timestamp corresponding to the <see cref="MaxHumidity" /> value.
    /// </summary>
    public DateTime MaxHumidityTimestamp =>
      Count > 0 ? Measurements.First(m => m.Humidity.Equals(MaxHumidity)).Timestamp : DateTime.Now;

    /// <summary>
    ///   Gets the count of data entries in the collection.
    /// </summary>
    public int Count => Measurements.Count();

    /// <summary>
    ///   Gets the measurement data items stored in the collection.
    /// </summary>
    public IEnumerable<Measurement> Measurements { get; }

    /// <summary>
    ///   Initializes a new climatic data measurements collection.
    /// </summary>
    /// <param name="measurements">
    ///   The measurement data items to be stored in the collection.
    /// </param>
    public MeasurementsCollection(IEnumerable<Measurement> measurements)
    {
      Measurements = measurements;
    }
  }
}
