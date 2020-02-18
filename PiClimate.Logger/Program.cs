using System;
using System.Globalization;
using System.IO;
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
          measurementLoop.LoopException += OnLoopException;
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
      var measurementLoopBuilder = new MeasurementLoopBuilder()
        .UseConfiguration(configuration)
        .UseMeasurementProvider(new Bme280Provider())
        .AddMeasurementLogger(new ConsoleMeasurementLogger())
        .AddMeasurementLogger(new MySqlLogger())
        .SetMeasurementLoopDelay(int.TryParse(configuration[MeasurementOptions.MeasurementLoopDelay], out var value)
          ? value
          : DefaultMeasurementLoopDelay);

      if (configuration[MeasurementOptions.UseRandomData]?.ToLower() == true.ToString().ToLower())
        measurementLoopBuilder.UseMeasurementProvider(new RandomMeasurementProvider());

      return measurementLoopBuilder;
    }

    private static void OnLoopException(object sender, ThreadExceptionEventArgs eventArgs)
    {
      ConsoleWriter.WriteWarning(
        $"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] Measurement warning: {eventArgs.Exception.Message}");
    }

    private static void WaitForExitRequested()
    {
      Console.TreatControlCAsInput = true;

      while (true)
      {
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.C && key.Modifiers == ConsoleModifiers.Control)
          break;
      }
    }
  }
}
