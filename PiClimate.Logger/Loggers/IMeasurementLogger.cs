using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Models;

namespace PiClimate.Logger.Loggers
{
  public interface IMeasurementLogger : IDisposable
  {
    bool IsConfigured { get; }

    void Configure(IConfiguration configuration);

    Task ConfigureAsync(IConfiguration configuration);

    void LogMeasurement(Measurement measurement);

    Task LogMeasurementAsync(Measurement measurement);
  }
}
