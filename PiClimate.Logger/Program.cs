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
  internal static class Program
  {
    private const string ConfigurationJsonFileName = "Configuration.json";

    private const int DefaultMeasurementLoopDelay = 60;

    private const string DefaultMeasurementProviderClassName = nameof(RandomDataProvider);

    private static readonly List<string> DefaultMeasurementLoggerClassNames = new List<string> {nameof(ConsoleLogger)};

    private static readonly List<string> DefaultMeasurementLimiterClassNames = new List<string>();

    private static readonly ConsoleWriter ConsoleWriter = new ConsoleWriter();

    private static readonly string ProgramName = Assembly.GetExecutingAssembly()
      .GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? $"{nameof(PiClimate)}.{nameof(Logger)}";

    private static readonly string ProgramVersion = Assembly.GetExecutingAssembly()
      .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "???";

    private static async Task<int> Main(string[] args)
    {
      try
      {
        ConsoleWriter.WriteNotice($"{ProgramName} v{ProgramVersion}\n");
        ConsoleWriter.WriteNotice("Starting the measurement loop...");

        var configuration = ConfigureConfigurationBuilder(args).Build();
        using (var measurementLoop = ConfigureMeasurementLoopBuilder(configuration).Build())
        {
          measurementLoop.MeasurementException += OnMeasurementException;
          measurementLoop.LoggerException += OnLoggerException;
          measurementLoop.LimiterException += OnLimiterException;

          await measurementLoop.StartLoopAsync();
          ConsoleWriter.WriteNotice("The measurement loop has started.");

          WaitForExitRequested();
        }

        ConsoleWriter.WriteNotice("Shutting down...");

        return 0;
      }
      catch (Exception e)
      {
        ConsoleWriter.WriteError($"The fatal error encountered: {e.Message}");
        return -1;
      }
    }

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

    private static void OnMeasurementException(object sender, ThreadExceptionEventArgs eventArgs)
    {
      ConsoleWriter.WriteWarning($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] " +
        $"[{sender.GetType().Name}] Measuring failed: {eventArgs.Exception.Message}");
    }

    private static void OnLoggerException(object sender, ThreadExceptionEventArgs eventArgs)
    {
      ConsoleWriter.WriteWarning($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] " +
        $"[{sender.GetType().Name}] Logging failed: {eventArgs.Exception.Message}");
    }

    private static void OnLimiterException(object sender, ThreadExceptionEventArgs eventArgs)
    {
      ConsoleWriter.WriteWarning($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] " +
        $"[{sender.GetType().Name}] Limiting failed: {eventArgs.Exception.Message}");
    }

    private static void WaitForExitRequested()
    {
      var exit = false;

      Console.CancelKeyPress += (sender, args) =>
      {
        args.Cancel = true;
        exit = true;
      };

      while (!exit)
        Task.Delay(100).Wait();
    }
  }
}
