using System.Threading.Tasks;
using PiClimate.Logger.Hardware;

namespace PiClimate.Logger.Database
{
  class ConsoleMeasurementLogger : IMeasurementLogger
  {
    private readonly ConsoleWriter _consoleWriter;

    public ConsoleMeasurementLogger(ConsoleWriter consoleWriter)
    {
      _consoleWriter = consoleWriter;
    }

    public void Initialize()
    {
    }

    public Task InitializeAsync() => Task.Run(Initialize);

    public void LogMeasurement(Measurement measurement) => _consoleWriter.WriteData(measurement.ToString());

    public Task LogMeasurementAsync(Measurement measurement) => Task.Run(() => LogMeasurement(measurement));

    public void Dispose()
    {
    }
  }
}
