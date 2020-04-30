// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Threading.Tasks;
using PiClimate.Logger.Configuration;
using PiClimate.Logger.Models;

namespace PiClimate.Logger.Providers
{
  /// <summary>
  ///   Generates the random climatic data for testing purposes.
  /// </summary>
  class RandomDataProvider : IMeasurementProvider
  {
    /// <inheritdoc />
    public bool IsConfigured { get; } = true;

    /// <inheritdoc />
    public void Configure(GlobalSettings settings)
    {
    }

    /// <inheritdoc />
    public Task ConfigureAsync(GlobalSettings settings) => Task.Run(() => Configure(settings));

    /// <inheritdoc />
    public Measurement Measure()
    {
      var random = new Random();

      return new Measurement
      {
        Timestamp = DateTime.Now,
        Pressure = 700.0 + 100.0 * random.NextDouble(),
        Temperature = 40.0 * random.NextDouble(),
        Humidity = 100.0 * random.NextDouble()
      };
    }

    /// <inheritdoc />
    public Task<Measurement> MeasureAsync() => Task.Run(Measure);

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
