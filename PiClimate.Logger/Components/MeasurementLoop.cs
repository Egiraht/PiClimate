using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Loggers;
using PiClimate.Logger.Providers;

namespace PiClimate.Logger.Components
{
  public class MeasurementLoop : IDisposable
  {
    private readonly IConfiguration _configuration;

    private readonly IMeasurementProvider _measurementProvider;

    private readonly List<IMeasurementLogger> _measurementLoggers = new List<IMeasurementLogger>();

    private readonly PeriodicLoop _pollingLoop;

    private bool _disposed = false;

    public event ThreadExceptionEventHandler LoopException
    {
      add => _pollingLoop.LoopException += value;
      remove => _pollingLoop.LoopException -= value;
    }

    internal MeasurementLoop(IConfiguration configuration, IMeasurementProvider measurementProvider,
      IEnumerable<IMeasurementLogger> measurementLoggers, MeasurementLoopOptions? options = null)
    {
      if (options == null)
        options = new MeasurementLoopOptions();

      _configuration = configuration;
      _measurementProvider = measurementProvider;
      _measurementLoggers.AddRange(measurementLoggers);
      _pollingLoop = new PeriodicLoop
      {
        LoopDelay = TimeSpan.FromSeconds(options.MeasurementLoopDelay),
        Loop = MeasureAndLogAsync
      };
    }

    public void StartLoop()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(MeasurementLoop));

      _measurementProvider.Configure(_configuration);
      foreach (var logger in _measurementLoggers)
        logger.Configure(_configuration);
      _pollingLoop.StartLoop();
    }

    public async Task StartLoopAsync()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(MeasurementLoop));

      await _measurementProvider.ConfigureAsync(_configuration);
      foreach (var logger in _measurementLoggers)
        await logger.ConfigureAsync(_configuration);
      _pollingLoop.StartLoop();
    }

    public void StopLoop()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(MeasurementLoop));

      _pollingLoop.StopLoop();
    }

    private async Task MeasureAndLogAsync(CancellationToken cancellationToken)
    {
      var measurement = await _measurementProvider.MeasureAsync();

      foreach (var logger in _measurementLoggers)
      {
        if (cancellationToken.IsCancellationRequested)
          break;

        await logger.LogMeasurementAsync(measurement);
      }
    }

    public void Dispose()
    {
      if (_disposed)
        return;

      _pollingLoop.Dispose();
      _measurementProvider.Dispose();
      foreach (var logger in _measurementLoggers)
        logger.Dispose();

      GC.SuppressFinalize(this);
      _disposed = true;
    }

    ~MeasurementLoop()
    {
      Dispose();
    }
  }
}
