using System;
using System.Threading;
using System.Threading.Tasks;

namespace PiClimate.Logger.Measurements
{
  public class PeriodicLoop : IDisposable
  {
    private bool _disposed = false;

    private CancellationTokenSource? _cancellationTokenSource;

    private Task? _loopTask;

    public TimeSpan LoopDelay { get; set; } = TimeSpan.FromMinutes(1);

    public bool IsRunning => _loopTask != null;

    public Func<CancellationToken, Task>? Loop { get; set; }

    public event ThreadExceptionEventHandler? LoopException;

    private async Task LoopCycleAsync()
    {
      if (_cancellationTokenSource == null || Loop == null)
        return;

      while (!_cancellationTokenSource.IsCancellationRequested)
      {
        try
        {
          await Loop(_cancellationTokenSource.Token);
        }
        catch (Exception e)
        {
          LoopException?.Invoke(this, new ThreadExceptionEventArgs(e));
        }

        await Task.Delay(LoopDelay, _cancellationTokenSource.Token);
      }
    }

    public void StartLoop()
    {
      if (_loopTask != null)
        return;

      _cancellationTokenSource = new CancellationTokenSource();
      _loopTask = Task.Run(LoopCycleAsync, _cancellationTokenSource.Token);
    }

    public void StopLoop()
    {
      if (_loopTask == null || _cancellationTokenSource == null)
        return;

      // Cancel the task, wait for its cancellation and suppress the succeeding task cancellation exception.
      try
      {
        _cancellationTokenSource.Cancel();
        _loopTask.Wait();
      }
      catch (AggregateException)
      {
      }

      _loopTask.Dispose();
      _loopTask = null;
      _cancellationTokenSource.Dispose();
      _cancellationTokenSource = null;
    }

    public void Dispose()
    {
      if (_disposed)
        return;

      StopLoop();

      GC.SuppressFinalize(this);
      _disposed = true;
    }

    ~PeriodicLoop()
    {
      Dispose();
    }
  }
}
