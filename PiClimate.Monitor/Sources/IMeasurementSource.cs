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
    Task<IEnumerable<Measurement>> GetMeasurementsAsync(MeasurementFilter filter);
  }
}
