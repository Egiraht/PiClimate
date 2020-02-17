using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PiClimate.Logger.Database;
using PiClimate.Logger.Hardware;

namespace PiClimate.Logger.Measurements
{
  public class MeasurementLoop : IDisposable
  {
    private readonly IMeasurementProvider _measurementProvider;

    private readonly List<IMeasurementLogger> _measurementLoggers = new List<IMeasurementLogger>();

    private readonly PeriodicLoop _pollingLoop;

    private bool _disposed = false;

    public event ThreadExceptionEventHandler LoopException
    {
      add => _pollingLoop.LoopException += value;
      remove => _pollingLoop.LoopException -= value;
    }

    internal MeasurementLoop(IMeasurementProvider measurementProvider,
      IEnumerable<IMeasurementLogger> measurementLoggers, MeasurementLoopOptions? options = null)
    {
      if (options == null)
        options = new MeasurementLoopOptions();

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
      _measurementProvider.Initialize();
      foreach (var logger in _measurementLoggers)
        logger.Initialize();
      _pollingLoop.StartLoop();
    }

    public async Task StartLoopAsync()
    {
      await _measurementProvider.InitializeAsync();
      foreach (var logger in _measurementLoggers)
        await logger.InitializeAsync();
      _pollingLoop.StartLoop();
    }

    public void StopLoop() => _pollingLoop.StopLoop();

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
