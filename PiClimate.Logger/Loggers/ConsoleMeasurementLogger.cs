using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Components;
using PiClimate.Logger.Models;

namespace PiClimate.Logger.Loggers
{
  class ConsoleMeasurementLogger : IMeasurementLogger
  {
    private readonly ConsoleWriter _consoleWriter = new ConsoleWriter();

    public bool IsConfigured { get; } = true;

    public void Configure(IConfiguration configuration)
    {
    }

    public Task ConfigureAsync(IConfiguration configuration) => Task.Run(() => Configure(configuration));

    public void LogMeasurement(Measurement measurement) => _consoleWriter.WriteData(measurement.ToString());

    public Task LogMeasurementAsync(Measurement measurement) => Task.Run(() => LogMeasurement(measurement));

    public void Dispose()
    {
    }
  }
}
