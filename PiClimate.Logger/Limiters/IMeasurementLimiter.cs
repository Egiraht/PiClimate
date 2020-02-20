using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PiClimate.Logger.Limiters
{
  public interface IMeasurementLimiter : IDisposable
  {
    bool IsConfigured { get; }

    void Configure(IConfiguration configuration);

    Task ConfigureAsync(IConfiguration configuration);

    void Apply();

    Task ApplyAsync();
  }
}
