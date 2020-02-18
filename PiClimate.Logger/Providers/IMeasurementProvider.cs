using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Models;

namespace PiClimate.Logger.Providers
{
  public interface IMeasurementProvider : IDisposable
  {
    bool IsConfigured { get; }

    void Configure(IConfiguration configuration);

    Task ConfigureAsync(IConfiguration configuration);

    Measurement Measure();

    Task<Measurement> MeasureAsync();
  }
}
