// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Threading.Tasks;
using PiClimate.Logger.Components;
using PiClimate.Logger.Models;
using PiClimate.Logger.Settings;

namespace PiClimate.Logger.Loggers
{
  /// <summary>
  ///   The console measurement data logger.
  /// </summary>
  class ConsoleLogger : IMeasurementLogger
  {
    /// <summary>
    ///   The console writer instance used for console output message formatting.
    /// </summary>
    private readonly ConsoleWriter _consoleWriter = new();

    /// <inheritdoc />
    public bool IsConfigured { get; } = true;

    /// <inheritdoc />
    public void Configure(GlobalSettings settings)
    {
    }

    /// <inheritdoc />
    public Task ConfigureAsync(GlobalSettings settings) => Task.Run(() => Configure(settings));

    /// <inheritdoc />
    public void LogMeasurement(Measurement measurement) => _consoleWriter.WriteData(measurement.ToString(), false);

    /// <inheritdoc />
    public Task LogMeasurementAsync(Measurement measurement) => Task.Run(() => LogMeasurement(measurement));

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
