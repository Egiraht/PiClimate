using System;
using System.Threading.Tasks;

namespace PiClimate.Logger.Hardware
{
  public interface IMeasurementProvider : IDisposable
  {
    bool IsConfigured { get; }

    void Initialize();

    Task InitializeAsync();

    Measurement Measure();

    Task<Measurement> MeasureAsync();
  }
}
