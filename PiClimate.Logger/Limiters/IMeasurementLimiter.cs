// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Threading.Tasks;
using PiClimate.Logger.Settings;

namespace PiClimate.Logger.Limiters
{
  /// <summary>
  ///   The measurement limiter interface.
  /// </summary>
  public interface IMeasurementLimiter : IDisposable
  {
    /// <summary>
    ///   Checks if the measurement limiter is configured.
    /// </summary>
    bool IsConfigured { get; }

    /// <summary>
    ///   Configures the measurement limiter using the provided settings.
    /// </summary>
    /// <param name="settings">
    ///   The global settings object containing the necessary options.
    /// </param>
    void Configure(GlobalSettings settings);

    /// <inheritdoc cref="Configure" />
    Task ConfigureAsync(GlobalSettings settings);

    /// <summary>
    ///   Applies the limiter.
    /// </summary>
    void Apply();

    /// <inheritdoc cref="Apply" />
    Task ApplyAsync();
  }
}
