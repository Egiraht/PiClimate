using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Components;
using PiClimate.Logger.Models;

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
    private readonly ConsoleWriter _consoleWriter = new ConsoleWriter();

    /// <inheritdoc />
    public bool IsConfigured { get; } = true;

    /// <inheritdoc />
    public void Configure(IConfiguration configuration)
    {
    }

    /// <inheritdoc />
    public Task ConfigureAsync(IConfiguration configuration) => Task.Run(() => Configure(configuration));

    /// <inheritdoc />
    public void LogMeasurement(Measurement measurement) => _consoleWriter.WriteData(measurement.ToString());

    /// <inheritdoc />
    public Task LogMeasurementAsync(Measurement measurement) => Task.Run(() => LogMeasurement(measurement));

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
