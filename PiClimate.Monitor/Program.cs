using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
    private static void Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();
      host.Run();
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
