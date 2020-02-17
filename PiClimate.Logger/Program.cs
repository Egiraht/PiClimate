using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Database;
using PiClimate.Logger.Hardware;
using PiClimate.Logger.Measurements;
using static PiClimate.Logger.ConfigurationLayout;

namespace PiClimate.Logger
{
  internal static class Program
  {
    private const string ConfigurationJsonFileName = "Settings.json";

    private const int DefaultMeasurementLoopDelay = 60;

    private static readonly ConsoleWriter ConsoleWriter = new ConsoleWriter();

    private static async Task<int> Main(string[] args)
    {
      try
      {
        ConsoleWriter.WriteNotice("Configuring the measurement loop...");

        var configuration = ConfigureConfigurationBuilder(args).Build();
        using (var measurementLoop = ConfigureMeasurementLoopBuilder(configuration).Build())
        {
          measurementLoop.LoopException += OnLoopException;
          ConsoleWriter.WriteNotice("The measurement loop is configured.");

          await measurementLoop.StartLoopAsync();

          WaitForExitRequested();
        }

        ConsoleWriter.WriteNotice("Shutting down...");

        return 0;
      }
      catch (Exception e)
      {
        ConsoleWriter.WriteError($"Failed to start the measurement loop: {e.Message}");
        return -1;
      }
    }

    private static ConfigurationBuilder ConfigureConfigurationBuilder(string[]? commandLineArguments = null)
    {
      var configurationBuilder = new ConfigurationBuilder();
      var jsonFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".",
        ConfigurationJsonFileName);

      configurationBuilder.AddJsonFile(jsonFilePath);
      configurationBuilder.AddCommandLine(commandLineArguments ?? Array.Empty<string>());

      return configurationBuilder;
    }

    private static MeasurementLoopBuilder ConfigureMeasurementLoopBuilder(IConfiguration configuration)
    {
      var measurementLoopBuilder = new MeasurementLoopBuilder();

      if (configuration[MeasurementOptions.UseRandomData]?.ToLower() == true.ToString().ToLower())
        measurementLoopBuilder.UseMeasurementProvider(new RandomMeasurementProvider());
      else
        measurementLoopBuilder.UseMeasurementProvider(new Bme280Provider(configuration));

      measurementLoopBuilder.AddMeasurementLogger(new ConsoleMeasurementLogger(ConsoleWriter));
      measurementLoopBuilder.AddMeasurementLogger(new MySqlLogger(configuration));

      measurementLoopBuilder.SetMeasurementLoopDelay(
        int.TryParse(configuration[MeasurementOptions.MeasurementLoopDelay], out var value)
          ? value
          : DefaultMeasurementLoopDelay);

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
