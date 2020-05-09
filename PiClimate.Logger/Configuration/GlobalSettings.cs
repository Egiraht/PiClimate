// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

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
    ///   Gets or sets the name of the used measurement provider.
    /// </summary>
    public string UseMeasurementProvider { get; set; } = nameof(RandomDataProvider);

    /// <summary>
    ///   Gets or sets a comma-separated list of names of the used measurement loggers.
    /// </summary>
    public string UseMeasurementLoggers { get; set; } = nameof(ConsoleLogger);

    /// <summary>
    ///   Gets or sets a comma-separated list of names of the used measurement limiters.
    /// </summary>
    public string UseMeasurementLimiters { get; set; } = "";

    /// <summary>
    ///   Gets or sets the measurement loop delay value in seconds.
    /// </summary>
    public int MeasurementLoopDelay { get; set; } = DefaultMeasurementLoopDelay;

    /// <summary>
    ///   Gets or sets the named list of connection strings providing the database connection parameters.
    /// </summary>
    public Dictionary<string, string> ConnectionStrings { get; set; } = new Dictionary<string, string>
    {
      {DefaultConnectionStringKey, DefaultConnectionStringValue}
    };
  }
}
