// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using PiClimate.Logger.Loggers;
using PiClimate.Logger.Providers;

namespace PiClimate.Logger.Configuration
{
  /// <summary>
  ///   The global settings of the program.
  /// </summary>
  public partial class GlobalSettings
  {
    /// <summary>
    ///   Defines the default used connection string key.
    /// </summary>
    public const string DefaultConnectionStringKey = "Default";

    /// <summary>
    ///   Defines the default used connection string value.
    /// </summary>
    public const string DefaultConnectionStringValue = "";

    /// <summary>
    ///   The default measurement loop delay in seconds.
    /// </summary>
    public const int DefaultMeasurementLoopDelay = 60;

    /// <summary>
    ///   The default measurement provider class name.
    /// </summary>
    private const string DefaultMeasurementProviderClassName = nameof(RandomDataProvider);

    /// <summary>
    ///   The list of class names of the default measurement loggers.
    /// </summary>
    private static readonly string[] DefaultMeasurementLoggerClassNames = {nameof(ConsoleLogger)};

    /// <summary>
    ///   The list of class names of the default measurement limiters.
    /// </summary>
    private static readonly string[] DefaultMeasurementLimiterClassNames = Array.Empty<string>();

    /// <summary>
    ///   Gets or sets the name of the measurement provider to use.
    /// </summary>
    public string UseMeasurementProvider { get; set; } = DefaultMeasurementProviderClassName;

    /// <summary>
    ///   Gets or sets a comma-separated list of names of the measurement loggers to use.
    /// </summary>
    public string UseMeasurementLoggers { get; set; } = string.Join(", ", DefaultMeasurementLoggerClassNames);

    /// <summary>
    ///   Gets or sets a comma-separated list of names of the measurement limiters to use.
    /// </summary>
    public string UseMeasurementLimiters { get; set; } = string.Join(", ", DefaultMeasurementLimiterClassNames);

    /// <summary>
    ///   Gets or sets the measurement loop delay value in seconds.
    /// </summary>
    public int MeasurementLoopDelay { get; set; } = DefaultMeasurementLoopDelay;

    /// <summary>
    ///   Gets or sets the named list of connection strings providing the database connection parameters.
    /// </summary>
    public Dictionary<string, string> ConnectionStrings { get; set; } = new()
    {
      {DefaultConnectionStringKey, DefaultConnectionStringValue}
    };
  }
}
