// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PiClimate.Common.Models;

namespace PiClimate.Monitor.Sources
{
  /// <summary>
  ///   A measurement source class that generates random data measurements.
  /// </summary>
  public class RandomDataSource : IMeasurementSource
  {
    /// <inheritdoc />
    public IEnumerable<Measurement> GetMeasurements(MeasurementFilter filter)
    {
      var random = new Random();
      var fromTime = filter.FromTime!.Value;
      var timeStep = (filter.ToTime - fromTime).Duration() / filter.Resolution;
      var measurements = new List<Measurement>();

      for (var counter = 0; counter < filter.Resolution; counter++)
        measurements.Add(new Measurement
        {
          Timestamp = (fromTime + counter * timeStep).ToLocalTime(),
          Pressure = Math.Round(700.0 + 10.0 * random.NextDouble() + 100.0 * counter / filter.Resolution, 3),
          Temperature = Math.Round(40.0 + 1.0 * random.NextDouble() - 10.0 * counter / filter.Resolution, 3),
          Humidity =
            Math.Round(80.0 + 6.0 * random.NextDouble() - 60.0 * Math.Sin(Math.PI * counter / filter.Resolution), 3)
        });

      return measurements;
    }

    /// <inheritdoc />
    public Task<IEnumerable<Measurement>> GetMeasurementsAsync(MeasurementFilter filter) =>
      Task.Run(() => GetMeasurements(filter));

    /// <inheritdoc />
    public IEnumerable<Measurement> GetLatestMeasurements(LatestDataRequest request)
    {
      var random = new Random();
      var fromTime = DateTime.Now - TimeSpan.FromSeconds(request.MaxRows);
      var measurements = new List<Measurement>();

      for (var counter = 0; counter < request.MaxRows; counter++)
        measurements.Add(new Measurement
        {
          Timestamp = (fromTime + TimeSpan.FromSeconds(counter)).ToLocalTime(),
          Pressure = Math.Round(700.0 + 100.0 * random.NextDouble(), 3),
          Temperature = Math.Round(40.0 * random.NextDouble(), 3),
          Humidity = Math.Round(100.0 * random.NextDouble(), 3)
        });

      return measurements;
    }

    /// <inheritdoc />
    public Task<IEnumerable<Measurement>> GetLatestMeasurementsAsync(LatestDataRequest request) =>
      Task.Run(() => GetLatestMeasurements(request));

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
