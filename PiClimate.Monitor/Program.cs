using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PiClimate.Monitor.Sources;

namespace PiClimate.Monitor
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
    ///   Gets the program's name.
    /// </summary>
    private static readonly string ProgramName = Assembly.GetExecutingAssembly()
      .GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? $"{nameof(PiClimate)}.{nameof(Monitor)}";

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
    private static int Main(string[] args)
    {
      try
      {
        // Building the web host.
        var host = CreateHostBuilder(args).Build();

        // Checking the service configuration.
        using (var scope = host.Services.CreateScope())
        {
          var loggerService = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
          if (scope.ServiceProvider.GetService<IMeasurementSource>() == null)
          {
            loggerService.LogError("Cannot start a web host as no measurement source is set.");
            return -1;
          }
        }

        // Starting the web host.
        host.Run();

        return 0;
      }
      catch (Exception e)
      {
        Console.Error.WriteLine($"The fatal error encountered: {e.Message}");
        return -1;
      }
    }

    /// <summary>
    ///   Creates a new web host builder.
    /// </summary>
    /// <param name="args">
    ///   An array of program's command line arguments.
    /// </param>
    /// <returns>
    ///   The configured web host builder instance.
    /// </returns>
    private static IHostBuilder CreateHostBuilder(string[] args) => Host
      .CreateDefaultBuilder(args)
      .ConfigureWebHostDefaults(builder => builder
        .UseStartup<Startup>()
        .UseWebRoot("Static"))
      .ConfigureAppConfiguration(builder => builder.AddJsonFile(ConfigurationJsonFileName));
  }
}
