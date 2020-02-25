using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Models;

namespace PiClimate.Logger.Providers
{
  /// <summary>
  ///   Generates the random climatic data for testing purposes.
  /// </summary>
  class RandomDataProvider : IMeasurementProvider
  {
    /// <inheritdoc />
    public bool IsConfigured { get; } = true;

    /// <inheritdoc />
    public void Configure(IConfiguration configuration)
    {
    }

    /// <inheritdoc />
    public Task ConfigureAsync(IConfiguration configuration) => Task.Run(() => Configure(configuration));

    /// <inheritdoc />
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

    /// <inheritdoc />
    public Task<Measurement> MeasureAsync() => Task.Run(Measure);

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
