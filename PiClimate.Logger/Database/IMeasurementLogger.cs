using System;
using System.Threading.Tasks;
using PiClimate.Logger.Hardware;

namespace PiClimate.Logger.Database
{
  public interface IMeasurementLogger : IDisposable
  {
    void Initialize();

    Task InitializeAsync();

    void LogMeasurement(Measurement measurement);

    Task LogMeasurementAsync(Measurement measurement);
  }
}
