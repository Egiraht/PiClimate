using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Models;

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
    ///   Configures the measurement provider using the provided configuration.
    /// </summary>
    /// <param name="configuration">
    ///   The <see cref="IConfiguration" /> object containing the provider's configuration data.
    /// </param>
    void Configure(IConfiguration configuration);

    /// <inheritdoc cref="Configure" />
    Task ConfigureAsync(IConfiguration configuration);

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
