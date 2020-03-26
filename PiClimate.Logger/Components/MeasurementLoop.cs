// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
    ///   The configuration used for configuring the measurement loop.
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    ///   The measurement provider associated with the current measurement loop.
    /// </summary>
    private readonly IMeasurementProvider _measurementProvider;

    /// <summary>
    ///   The list of measurement loggers associated with the current measurement loop.
    /// </summary>
    private readonly List<IMeasurementLogger> _measurementLoggers = new List<IMeasurementLogger>();

    /// <summary>
    ///   The list of measurement limiters associated with the current measurement loop.
    /// </summary>
    private readonly List<IMeasurementLimiter> _measurementLimiters = new List<IMeasurementLimiter>();

    /// <summary>
    ///   The periodic loop allowing to run the regular measurement cycles asynchronously.
    /// </summary>
    private readonly PeriodicLoop _periodicLoop;

    /// <summary>
    ///   The object's disposal flag.
    /// </summary>
    private bool _disposed = false;

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
    /// <param name="configuration">
    ///   The configuration used for configuring the measurement loop.
    /// </param>
    /// <param name="measurementProvider">
    ///   The measurement provider associated with the current measurement loop.
    /// </param>
    /// <param name="measurementLoggers">
    ///   The list of measurement loggers associated with the current measurement loop.
    /// </param>
    /// <param name="measurementLimiters">
    ///   The list of measurement limiters associated with the current measurement loop.
    /// </param>
    /// <param name="options">
    ///   The <see cref="MeasurementLoopOptions" /> instance containing the additional measurement loop options.
    /// </param>
    /// <remarks>
    ///   It is not recommended to use this constructor explicitly.
    ///   Use the <see cref="MeasurementLoopBuilder" /> class instead.
    /// </remarks>
    public MeasurementLoop(IConfiguration configuration, IMeasurementProvider measurementProvider,
      IEnumerable<IMeasurementLogger> measurementLoggers, IEnumerable<IMeasurementLimiter> measurementLimiters,
      MeasurementLoopOptions options)
    {
      _configuration = configuration;
      _measurementProvider = measurementProvider;
      _measurementLoggers.AddRange(measurementLoggers);
      _measurementLimiters.AddRange(measurementLimiters);
      _periodicLoop = new PeriodicLoop
      {
        LoopDelay = TimeSpan.FromSeconds(options.MeasurementLoopDelay),
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

      _measurementProvider.Configure(_configuration);
      foreach (var logger in _measurementLoggers)
        logger.Configure(_configuration);
      foreach (var limiter in _measurementLimiters)
        limiter.Configure(_configuration);
      _periodicLoop.StartLoop();
    }

    /// <inheritdoc cref="StartLoop" />
    public async Task StartLoopAsync()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(MeasurementLoop));

      await _measurementProvider.ConfigureAsync(_configuration);
      foreach (var logger in _measurementLoggers)
        await logger.ConfigureAsync(_configuration);
      foreach (var limiter in _measurementLimiters)
        await limiter.ConfigureAsync(_configuration);
      _periodicLoop.StartLoop();
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

      _periodicLoop.StopLoop();
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

        var measurement = await _measurementProvider.MeasureAsync();

        // Logging the data.
        foreach (var logger in _measurementLoggers)
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
        foreach (var limiter in _measurementLimiters)
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
        MeasurementException?.Invoke(_measurementProvider, new ThreadExceptionEventArgs(e));
      }
    }

    /// <inheritdoc />
    public void Dispose()
    {
      if (_disposed)
        return;

      _periodicLoop.Dispose();
      _measurementProvider.Dispose();
      foreach (var logger in _measurementLoggers)
        logger.Dispose();
      foreach (var limiter in _measurementLimiters)
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
