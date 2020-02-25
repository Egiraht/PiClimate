using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Models;

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
    ///   Configures the measurement logger using the provided configuration.
    /// </summary>
    /// <param name="configuration">
    ///   The <see cref="IConfiguration" /> object containing the provider's configuration data.
    /// </param>
    void Configure(IConfiguration configuration);

    /// <inheritdoc cref="Configure" />
    Task ConfigureAsync(IConfiguration configuration);

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
