// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PiClimate.Logger.Configuration;
using PiClimate.Logger.Limiters;
using PiClimate.Logger.Loggers;
using PiClimate.Logger.Providers;

namespace PiClimate.Logger.Components
{
  /// <summary>
  ///   A class representing the periodic measurement loop.
  /// </summary>
  public class MeasurementLoop : IDisposable
  {
    /// <summary>
    ///   The object's disposal flag.
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    ///   The global settings used for configuring the measurement loop.
    /// </summary>
    private GlobalSettings Settings { get; }

    /// <summary>
    ///   The measurement provider associated with the current measurement loop.
    /// </summary>
    private IMeasurementProvider MeasurementProvider { get; }

    /// <summary>
    ///   The list of measurement loggers associated with the current measurement loop.
    /// </summary>
    private List<IMeasurementLogger> MeasurementLoggers { get; }

    /// <summary>
    ///   The list of measurement limiters associated with the current measurement loop.
    /// </summary>
    private List<IMeasurementLimiter> MeasurementLimiters { get; }

    /// <summary>
    ///   The periodic loop allowing to run the regular measurement cycles asynchronously.
    /// </summary>
    private PeriodicLoop Loop { get; }

    /// <summary>
    ///   The event fired when an exception is thrown during the measurement provider processing.
    /// </summary>
    /// <remarks>
    ///   The thrown exception does not stop the measurement loop but prevents the loggers and limiters from running
    ///   in the current loop cycle.
    /// </remarks>
    public event ThreadExceptionEventHandler? MeasurementException;

    /// <summary>
    ///   The event fired when an exception is thrown during the measurement logger processing.
    /// </summary>
    /// <remarks>
    ///   The thrown exception does not stop the measurement loop and does not affect any other measurement loggers
    ///   or limiters.
    /// </remarks>
    public event ThreadExceptionEventHandler? LoggerException;

    /// <summary>
    ///   The event fired when an exception is thrown during the measurement limiter processing.
    /// </summary>
    /// <remarks>
    ///   The thrown exception does not stop the measurement loop and does not affect any other measurement loggers
    ///   or limiters.
    /// </remarks>
    public event ThreadExceptionEventHandler? LimiterException;

    /// <summary>
    ///   Creates a new measurement loop object.
    /// </summary>
    /// <param name="settings">
    ///   The global settings used for configuring the measurement loop.
    /// </param>
    public MeasurementLoop(GlobalSettings settings)
    {
      Settings = settings;
      MeasurementProvider = ClassFactory.CreateMeasurementProvider(settings.UseMeasurementProvider);
      MeasurementLoggers =
        new List<IMeasurementLogger>(ClassFactory.CreateMeasurementLoggers(settings.UseMeasurementLoggers));
      MeasurementLimiters =
        new List<IMeasurementLimiter>(ClassFactory.CreateMeasurementLimiters(settings.UseMeasurementLimiters));
      Loop = new PeriodicLoop
      {
        LoopDelay = TimeSpan.FromSeconds(settings.MeasurementLoopDelay),
        Loop = MeasureAndLogAsync
      };
    }

    /// <summary>
    ///   Starts the measurement loop.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   The object was disposed.
    /// </exception>
    public void StartLoop()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(MeasurementLoop));

      MeasurementProvider.Configure(Settings);
      foreach (var logger in MeasurementLoggers)
        logger.Configure(Settings);
      foreach (var limiter in MeasurementLimiters)
        limiter.Configure(Settings);
      Loop.StartLoop();
    }

    /// <inheritdoc cref="StartLoop" />
    public async Task StartLoopAsync()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(MeasurementLoop));

      await MeasurementProvider.ConfigureAsync(Settings);
      foreach (var logger in MeasurementLoggers)
        await logger.ConfigureAsync(Settings);
      foreach (var limiter in MeasurementLimiters)
        await limiter.ConfigureAsync(Settings);
      Loop.StartLoop();
    }

    /// <summary>
    ///   Stops the measurement loop.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   The object was disposed.
    /// </exception>
    public void StopLoop()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(MeasurementLoop));

      Loop.StopLoop();
    }

    /// <summary>
    ///   The asynchronous measurement loop callback used for data measuring, logging, and limiting.
    /// </summary>
    /// <param name="cancellationToken">
    ///   The cancellation token used for proper stopping of the measurement loop.
    /// </param>
    private async Task MeasureAndLogAsync(CancellationToken cancellationToken)
    {
      try
      {
        // Measuring the data.
        if (cancellationToken.IsCancellationRequested)
          return;

        var measurement = await MeasurementProvider.MeasureAsync();

        // Logging the data.
        foreach (var logger in MeasurementLoggers)
        {
          if (cancellationToken.IsCancellationRequested)
            return;

          try
          {
            await logger.LogMeasurementAsync(measurement);
          }
          catch (Exception e)
          {
            LoggerException?.Invoke(logger, new ThreadExceptionEventArgs(e));
          }
        }

        // Limiting the data.
        foreach (var limiter in MeasurementLimiters)
        {
          if (cancellationToken.IsCancellationRequested)
            return;

          try
          {
            await limiter.ApplyAsync();
          }
          catch (Exception e)
          {
            LimiterException?.Invoke(limiter, new ThreadExceptionEventArgs(e));
          }
        }
      }
      catch (Exception e)
      {
        MeasurementException?.Invoke(MeasurementProvider, new ThreadExceptionEventArgs(e));
      }
    }

    /// <inheritdoc />
    public void Dispose()
    {
      if (_disposed)
        return;

      Loop.Dispose();
      MeasurementProvider.Dispose();
      foreach (var logger in MeasurementLoggers)
        logger.Dispose();
      foreach (var limiter in MeasurementLimiters)
        limiter.Dispose();

      GC.SuppressFinalize(this);
      _disposed = true;
    }

    /// <inheritdoc />
    ~MeasurementLoop()
    {
      Dispose();
    }
  }
}
