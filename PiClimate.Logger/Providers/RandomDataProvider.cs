using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Models;

namespace PiClimate.Logger.Providers
{
  class RandomDataProvider : IMeasurementProvider
  {
    public bool IsConfigured { get; } = true;

    public void Configure(IConfiguration configuration)
    {
    }

    public Task ConfigureAsync(IConfiguration configuration) => Task.Run(() => Configure(configuration));

    public Measurement Measure()
    {
      var random = new Random();

      return new Measurement
      {
        Timestamp = DateTime.Now,
        Pressure = 700.0 + 100.0 * random.NextDouble(),
        Temperature = 40.0 * random.NextDouble(),
        Humidity = 100.0 * random.NextDouble()
      };
    }

    public Task<Measurement> MeasureAsync() => Task.Run(Measure);

    public void Dispose()
    {
    }
  }
}
