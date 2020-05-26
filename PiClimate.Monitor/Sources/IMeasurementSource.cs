// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PiClimate.Monitor.Models;

namespace PiClimate.Monitor.Sources
{
  /// <summary>
  ///   An interface for the measurement source services.
  /// </summary>
  public interface IMeasurementSource : IDisposable
  {
    /// <summary>
    ///   Gets the stored measurement data filtered using the provided measurement filter.
    /// </summary>
    /// <param name="filter">
    ///   The measurement filter used for measurement data filtering.
    /// </param>
    /// <returns>
    ///   A collection of data measurements.
    /// </returns>
    IEnumerable<Measurement> GetMeasurements(MeasurementFilter filter);

    /// <inheritdoc cref="GetMeasurements" />
    /// <summary>
    ///   Asynchronously gets the stored measurement data filtered using the provided measurement filter.
    /// </summary>
    Task<IEnumerable<Measurement>> GetMeasurementsAsync(MeasurementFilter filter);

    /// <summary>
    ///   Gets the latest logged measurements.
    /// </summary>
    /// <param name="request">
    ///   The latest data request object.
    /// </param>
    /// <returns>
    ///   A collection of data measurements.
    /// </returns>
    IEnumerable<Measurement> GetLatestMeasurements(LatestDataRequest request);

    /// <inheritdoc cref="GetLatestMeasurements" />
    /// <summary>
    ///   Asynchronously gets the latest logged measurements.
    /// </summary>
    Task<IEnumerable<Measurement>> GetLatestMeasurementsAsync(LatestDataRequest request);
  }
}
