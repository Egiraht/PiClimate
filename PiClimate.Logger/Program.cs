// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Components;
using PiClimate.Logger.Configuration;

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
    ///   The console writer instance used for console output message formatting.
    /// </summary>
    private static readonly ConsoleWriter ConsoleWriter = new();

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
        ConsoleWriter.WriteNotice($"{ProgramName} v{ProgramVersion}\n", false);
        ConsoleWriter.WriteNotice("Starting the measurement loop...");

        // Collecting the program's configuration.
        var configuration = ConfigureConfigurationBuilder(args).Build();
        var settings = configuration.Get<GlobalSettings>();
        PrintSettings(settings);

        // Building and starting the measurement loop.
        using (var measurementLoop = new MeasurementLoop(settings))
        {
          measurementLoop.MeasurementException += OnMeasurementException;
          measurementLoop.LoggerException += OnLoggerException;
          measurementLoop.LimiterException += OnLimiterException;

          await measurementLoop.StartLoopAsync();
          ConsoleWriter.WriteNotice("The measurement loop has started.");
          ConsoleWriter.WriteNotice("Press Ctrl+C to shut down the loop.");

          // Halting the main thread until the shutdown is requested.
          await WaitForShutdownAsync();
        }

        // Shutting down the program.
        ConsoleWriter.WriteNotice("Shutting down...");

        return 0;
      }
      catch (Exception e)
      {
        ConsoleWriter.WriteError($"[{e.GetType().Name}] The fatal error encountered: {e.Message}");
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
    ///   Prints the essential settings information read from the configuration file.
    /// </summary>
    /// <param name="settings">
    ///   The program's global settings.
    /// </param>
    private static void PrintSettings(GlobalSettings settings)
    {
      var providerTypeName = ClassFactory.GetMeasurementProviderType(settings.UseMeasurementProvider).Name;
      var loggerTypeNames = string.Join(", ",
        ClassFactory.GetMeasurementLoggerTypes(settings.UseMeasurementLoggers).Select(type => type.Name));
      var limiterTypeNames = string.Join(", ",
        ClassFactory.GetMeasurementLimiterTypes(settings.UseMeasurementLimiters).Select(type => type.Name));
      ConsoleWriter.WriteNotice($"Using measurement provider: {providerTypeName}.");
      ConsoleWriter.WriteNotice($"Using measurement loggers: {(!string.IsNullOrEmpty(loggerTypeNames) ? loggerTypeNames : "none")}.");
      ConsoleWriter.WriteNotice($"Using measurement limiters: {(!string.IsNullOrEmpty(limiterTypeNames) ? limiterTypeNames : "none")}.");
      ConsoleWriter.WriteNotice($"Using measurement loop delay: {settings.MeasurementLoopDelay} second(s).");
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
      ConsoleWriter.WriteWarning($"[{sender.GetType().Name}] Measuring failed: {eventArgs.Exception.Message}");
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
      ConsoleWriter.WriteWarning($"[{sender.GetType().Name}] Logging failed: {eventArgs.Exception.Message}");
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
      ConsoleWriter.WriteWarning($"[{sender.GetType().Name}] Limiting failed: {eventArgs.Exception.Message}");
    }

    /// <summary>
    ///   Halts the main thread until the program's shutdown is requested.
    /// </summary>
    private static async Task WaitForShutdownAsync()
    {
      var cancellationTokenSource = new CancellationTokenSource();

      Console.CancelKeyPress += (_, args) =>
      {
        args.Cancel = true;
        cancellationTokenSource.Cancel();
      };

      try
      {
        await Task.Delay(-1, cancellationTokenSource.Token);
      }
      catch (OperationCanceledException)
      {
        // Ignore cancellation exceptions.
      }
    }
  }
}
