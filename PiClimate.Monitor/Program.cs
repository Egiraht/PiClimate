// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
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
    ///   The settings JSON file path.
    /// </summary>
    public static readonly string SettingsFilePath =
      $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Settings.json";

    /// <summary>
    ///   Gets the program's name.
    /// </summary>
    public static readonly string ProgramName = Assembly.GetExecutingAssembly()
      .GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? $"{nameof(PiClimate)}.{nameof(Monitor)}";

    /// <summary>
    ///   Gets the program's version.
    /// </summary>
    public static readonly string ProgramVersion = Assembly.GetExecutingAssembly()
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
        Console.Out.WriteLine($"{ProgramName} v{ProgramVersion}\n");

        // Building the web host.
        var host = CreateHostBuilder(args).Build();

        // Printing the settings.
        PrintSettings(host.Services);

        // Starting the web host.
        host.Run();

        return 0;
      }
      catch (Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine($"The fatal error encountered: {e.Message}");
        Console.ResetColor();
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
      .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>());

    /// <summary>
    ///   Prints the essential settings information read from the configuration file.
    /// </summary>
    /// <param name="provider">
    ///   The service provider containing the services to use for printing.
    /// </param>
    private static void PrintSettings(IServiceProvider provider)
    {
      var measurementSourceName = provider.GetRequiredService<IMeasurementSource>().GetType().Name;
      var logger = provider.GetRequiredService<ILogger<Startup>>();
      logger.LogInformation($"Using measurement source: {measurementSourceName}.");
    }
  }
}
