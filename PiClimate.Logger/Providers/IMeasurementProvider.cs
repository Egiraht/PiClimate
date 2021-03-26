// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Threading.Tasks;
using PiClimate.Logger.Models;
using PiClimate.Logger.Settings;

namespace PiClimate.Logger.Providers
{
  /// <summary>
  ///   The measurement provider interface.
  /// </summary>
  public interface IMeasurementProvider : IDisposable
  {
    /// <summary>
    ///   Checks if the measurement provider is configured.
    /// </summary>
    bool IsConfigured { get; }

    /// <summary>
    ///   Configures the measurement provider using the provided settings.
    /// </summary>
    /// <param name="settings">
    ///   The global settings object containing the necessary options.
    /// </param>
    void Configure(GlobalSettings settings);

    /// <inheritdoc cref="Configure" />
    Task ConfigureAsync(GlobalSettings settings);

    /// <summary>
    ///   Measures the current climatic data.
    /// </summary>
    /// <returns>
    ///   The <see cref="Measurement" /> instance with the collected climatic data.
    /// </returns>
    Measurement Measure();

    /// <inheritdoc cref="Measure" />
    Task<Measurement> MeasureAsync();
  }
}
