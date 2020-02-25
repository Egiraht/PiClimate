using System;
using System.Threading;
using System.Threading.Tasks;

namespace PiClimate.Logger.Components
{
  /// <summary>
  ///   A class representing an action that runs asynchronously and periodically with a constant delay.
  /// </summary>
  public class PeriodicLoop : IDisposable
  {
    /// <summary>
    ///   The default loop delay value awaited between the two consequent cycles of the running loop.
    /// </summary>
    public static readonly TimeSpan DefaultLoopDelay = TimeSpan.FromMinutes(1);

    /// <summary>
    ///   The object's disposal flag.
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    ///   The loop's cancellation token source.
    /// </summary>
    private CancellationTokenSource? _cancellationTokenSource;

    /// <summary>
    ///   The <see cref="Task" /> encapsulating the periodic asynchronous action.
    /// </summary>
    private Task? _loopTask;

    /// <summary>
    ///   The loop delay value awaited between the two consequent cycles of the running loop.
    /// </summary>
    public TimeSpan LoopDelay { get; set; } = DefaultLoopDelay;

    /// <summary>
    ///   Checks if the periodic loop is running.
    /// </summary>
    public bool IsRunning => _loopTask != null;

    /// <summary>
    ///   The asynchronous loop action delegate to be run periodically.
    /// </summary>
    /// <remarks>
    ///   As a parameter the delegate takes a cancellation token for checking if the loop was stopped.
    ///   As a result the delegate must return an asynchronous <see cref="Task" /> encapsulating the action.
    /// </remarks>
    public Func<CancellationToken, Task>? Loop { get; set; }

    /// <summary>
    ///   The event fired when an exception is thrown from the running loop.
    /// </summary>
    /// <remarks>
    ///   The thrown exception does not stop the loop, so the action will continue get periodically called until it
    ///   will be explicitly stopped using the <see cref="StopLoop" /> method or the object will be disposed.
    /// </remarks>
    public event ThreadExceptionEventHandler? LoopException;

    /// <summary>
    ///   Asynchronously runs the provided action delegate.
    /// </summary>
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

    /// <summary>
    ///   Starts the periodic loop.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   The object was disposed.
    /// </exception>
    public void StartLoop()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(PeriodicLoop));

      if (_loopTask != null)
        return;

      _cancellationTokenSource = new CancellationTokenSource();
      _loopTask = Task.Run(LoopCycleAsync, _cancellationTokenSource.Token);
    }

    /// <summary>
    ///   Stops the periodic loop.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   The object was disposed.
    /// </exception>
    public void StopLoop()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(PeriodicLoop));

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

    /// <inheritdoc />
    public void Dispose()
    {
      if (_disposed)
        return;

      StopLoop();

      GC.SuppressFinalize(this);
      _disposed = true;
    }

    /// <inheritdoc />
    ~PeriodicLoop()
    {
      Dispose();
    }
  }
}
