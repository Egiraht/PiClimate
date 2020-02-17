using System;
using System.Threading.Tasks;

namespace PiClimate.Logger.Hardware
{
  class RandomMeasurementProvider : IMeasurementProvider
  {
    public bool IsConfigured { get; private set; }

    public void Initialize()
    {
      IsConfigured = true;
    }

    public Task InitializeAsync() => Task.Run(Initialize);

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
