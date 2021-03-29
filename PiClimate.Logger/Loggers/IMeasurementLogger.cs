// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Threading.Tasks;
using PiClimate.Common.Models;
using PiClimate.Logger.Settings;

namespace PiClimate.Logger.Loggers
{
  /// <summary>
  ///   The measurement data logger interface.
  /// </summary>
  public interface IMeasurementLogger : IDisposable
  {
    /// <summary>
    ///   Checks if the measurement logger is configured.
    /// </summary>
    bool IsConfigured { get; }

    /// <summary>
    ///   Configures the measurement logger using the provided settings.
    /// </summary>
    /// <param name="settings">
    ///   The global settings object containing the necessary options.
    /// </param>
    void Configure(GlobalSettings settings);

    /// <inheritdoc cref="Configure" />
    Task ConfigureAsync(GlobalSettings settings);

    /// <summary>
    ///   Logs the provided measurement data.
    /// </summary>
    /// <returns>
    ///   The <see cref="Measurement" /> instance with the collected climatic data.
    /// </returns>
    void LogMeasurement(Measurement measurement);

    /// <inheritdoc cref="LogMeasurement" />
    Task LogMeasurementAsync(Measurement measurement);
  }
}
