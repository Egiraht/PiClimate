// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Components;
using PiClimate.Logger.ConfigurationLayout;
using PiClimate.Logger.Loggers;
using PiClimate.Logger.Providers;

namespace PiClimate.Logger
{
  /// <summary>
  ///   The main program's class.
  /// </summary>
  internal static class Program
  {
    /// <summary>
    ///   The configuration JSON file name.
    /// </summary>
    private const string ConfigurationJsonFileName = "Configuration.json";

    /// <summary>
    ///   The default measurement loop delay in seconds.
    /// </summary>
    private const int DefaultMeasurementLoopDelay = 60;

    /// <summary>
    ///   The default measurement provider class name.
    /// </summary>
    private const string DefaultMeasurementProviderClassName = nameof(RandomDataProvider);

    /// <summary>
    ///   The list of class names of the default measurement loggers.
    /// </summary>
    private static readonly List<string> DefaultMeasurementLoggerClassNames = new List<string> {nameof(ConsoleLogger)};

    /// <summary>
    ///   The list of class names of the default measurement limiters.
    /// </summary>
    private static readonly List<string> DefaultMeasurementLimiterClassNames = new List<string>();

    /// <summary>
    ///   The console writer instance used for console output message formatting.
    /// </summary>
    private static readonly ConsoleWriter ConsoleWriter = new ConsoleWriter();

    /// <summary>
    ///   Gets the program's name.
    /// </summary>
    private static readonly string ProgramName = Assembly.GetExecutingAssembly()
      .GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? $"{nameof(PiClimate)}.{nameof(Logger)}";

    /// <summary>
    ///   Gets the program's version.
    /// </summary>
    private static readonly string ProgramVersion = Assembly.GetExecutingAssembly()
      .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "???";

    /// <summary>
    ///   The program's entry point.
    /// </summary>
    /// <param name="args">
    ///   An array of program's command line arguments.
    /// </param>
    /// <returns>
    ///   The program's exit code.
    ///   Returns code <c>0</c> when the program finishes successfully.
    /// </returns>
    private static async Task<int> Main(string[] args)
    {
      try
      {
        // Printing the header.
        ConsoleWriter.WriteNotice($"{ProgramName} v{ProgramVersion}\n");
        ConsoleWriter.WriteNotice("Starting the measurement loop...");

        // Collecting the program's configuration.
        var configuration = ConfigureConfigurationBuilder(args).Build();

        // Building and starting the measurement loop.
        using (var measurementLoop = ConfigureMeasurementLoopBuilder(configuration).Build())
        {
          measurementLoop.MeasurementException += OnMeasurementException;
          measurementLoop.LoggerException += OnLoggerException;
          measurementLoop.LimiterException += OnLimiterException;

          await measurementLoop.StartLoopAsync();
          ConsoleWriter.WriteNotice("The measurement loop has started.");

          // Halting the main thread until the shutdown is requested.
          await WaitForShutdownAsync();
        }

        // Shutting down the program.
        ConsoleWriter.WriteNotice("Shutting down...");

        return 0;
      }
      catch (Exception e)
      {
        ConsoleWriter.WriteError($"The fatal error encountered: {e.Message}");
        return -1;
      }
    }

    /// <summary>
    ///   Configures the program's <see cref="ConfigurationBuilder" /> instance.
    ///   The program's configuration is collected from the JSON configuration file available at the
    ///   <see cref="ConfigurationJsonFileName" /> path and program's command line arguments.
    /// </summary>
    /// <param name="commandLineArguments">
    ///   An array of program's command line arguments.
    /// </param>
    /// <returns>
    ///   The configured program's <see cref="ConfigurationBuilder" /> instance.
    /// </returns>
    private static ConfigurationBuilder ConfigureConfigurationBuilder(string[]? commandLineArguments = null)
    {
      var configurationBuilder = new ConfigurationBuilder();
      var jsonFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".",
        ConfigurationJsonFileName);

      // Command line argument values have priority over the configuration file values.
      configurationBuilder.AddJsonFile(jsonFilePath);
      configurationBuilder.AddCommandLine(commandLineArguments ?? Array.Empty<string>());

      return configurationBuilder;
    }

    /// <summary>
    ///   Configures the program's <see cref="MeasurementLoopBuilder" /> instance.
    /// </summary>
    /// <param name="configuration">
    ///   The program's configuration.
    /// </param>
    /// <returns>
    ///   The configured program's <see cref="MeasurementLoopBuilder" /> instance.
    /// </returns>
    private static MeasurementLoopBuilder ConfigureMeasurementLoopBuilder(IConfiguration configuration)
    {
      var measurementProvider = configuration[Root.UseMeasurementProvider];
      if (string.IsNullOrEmpty(measurementProvider))
        measurementProvider = DefaultMeasurementProviderClassName;

      var measurementLoggers = (configuration[Root.UseMeasurementLoggers] ?? "")
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(className => className.Trim())
        .ToList();
      if (!measurementLoggers.Any())
        measurementLoggers = DefaultMeasurementLoggerClassNames;

      var measurementLimiters = (configuration[Root.UseMeasurementLimiters] ?? "")
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(className => className.Trim())
        .ToList();
      if (!measurementLimiters.Any())
        measurementLimiters = DefaultMeasurementLimiterClassNames;

      var measurementLoopBuilder = new MeasurementLoopBuilder()
        .UseConfiguration(configuration)
        .UseMeasurementProvider(measurementProvider)
        .AddMeasurementLoggers(measurementLoggers)
        .AddMeasurementLimiters(measurementLimiters)
        .SetMeasurementLoopDelay(int.TryParse(configuration[Root.MeasurementLoopDelay], out var value)
          ? value
          : DefaultMeasurementLoopDelay);

      return measurementLoopBuilder;
    }

    /// <summary>
    ///   The event callback called on measurement provider exception.
    /// </summary>
    /// <param name="sender">
    ///   The object that threw the exception.
    /// </param>
    /// <param name="eventArgs">
    ///   The event arguments object containing the thrown exception information.
    /// </param>
    private static void OnMeasurementException(object sender, ThreadExceptionEventArgs eventArgs)
    {
      ConsoleWriter.WriteWarning($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] " +
        $"[{sender.GetType().Name}] Measuring failed: {eventArgs.Exception.Message}");
    }

    /// <summary>
    ///   The event callback called on measurement logger exception.
    /// </summary>
    /// <param name="sender">
    ///   The object that threw the exception.
    /// </param>
    /// <param name="eventArgs">
    ///   The event arguments object containing the thrown exception information.
    /// </param>
    private static void OnLoggerException(object sender, ThreadExceptionEventArgs eventArgs)
    {
      ConsoleWriter.WriteWarning($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] " +
        $"[{sender.GetType().Name}] Logging failed: {eventArgs.Exception.Message}");
    }

    /// <summary>
    ///   The event callback called on measurement limiter exception.
    /// </summary>
    /// <param name="sender">
    ///   The object that threw the exception.
    /// </param>
    /// <param name="eventArgs">
    ///   The event arguments object containing the thrown exception information.
    /// </param>
    private static void OnLimiterException(object sender, ThreadExceptionEventArgs eventArgs)
    {
      ConsoleWriter.WriteWarning($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] " +
        $"[{sender.GetType().Name}] Limiting failed: {eventArgs.Exception.Message}");
    }

    /// <summary>
    ///   Halts the main thread until the program's shutdown is requested.
    /// </summary>
    private static async Task WaitForShutdownAsync()
    {
      var cancellationTokenSource = new CancellationTokenSource();

      Console.CancelKeyPress += (sender, args) =>
      {
        args.Cancel = true;
        cancellationTokenSource.Cancel();
      };

      try
      {
        await Task.Delay(-1, cancellationTokenSource.Token);
      }
      catch
      {
        // Ignore the task cancellation exception.
      }
    }
  }
}
