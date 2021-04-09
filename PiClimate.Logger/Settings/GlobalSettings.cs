// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System.Collections.Generic;
using PiClimate.Common.Settings;
using PiClimate.Logger.Limiters;
using PiClimate.Logger.Loggers;
using PiClimate.Logger.Providers;

namespace PiClimate.Logger.Settings
{
  /// <summary>
  ///   The global settings of the program.
  /// </summary>
  public partial class GlobalSettings : SettingsSection
  {
    /// <summary>
    ///   Defines the default used connection string key.
    /// </summary>
    public const string DefaultConnectionStringKey = "Default";

    /// <summary>
    ///   Defines the default used connection string value.
    /// </summary>
    public const string DefaultConnectionStringValue =
      "Server=localhost; Port=3306; UserId=root; Password=; Database=PiClimate";

    /// <summary>
    ///   The default measurement loop delay in seconds.
    /// </summary>
    public const int DefaultMeasurementLoopDelay = 1;

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
    private static readonly string[] DefaultMeasurementLimiterClassNames = { };

    /// <summary>
    ///   Gets or sets the name of the measurement provider to use.
    /// </summary>
    [Comment("Sets the name of the measurement provider to use.")]
    [Comment("The value cannot be empty and must be one of these values: " +
      "'" + nameof(RandomDataProvider) + "' (random data generation), " +
      "'" + nameof(BmeReaderProvider) + "' (reading data from the BME280 sensor using the BMEReader adapter), " +
      "'" + nameof(Bme280Provider) + "' (reading data directly from the BME280 sensor).")]
    public string UseMeasurementProvider { get; set; } = DefaultMeasurementProviderClassName;

    /// <summary>
    ///   Gets or sets a comma-separated list of names of the measurement loggers to use.
    /// </summary>
    [Comment("Sets a comma-separated list of names of the measurement loggers to use.")]
    [Comment("Can be a combination of these values: " +
      "'" + nameof(ConsoleLogger) + "' (logging data to standard output stream), " +
      "'" + nameof(MySqlLogger) + "' (logging data to a MySQL database).")]
    public string UseMeasurementLoggers { get; set; } = string.Join(", ", DefaultMeasurementLoggerClassNames);

    /// <summary>
    ///   Gets or sets a comma-separated list of names of the measurement limiters to use.
    /// </summary>
    [Comment("Sets a comma-separated list of names of the measurement limiters to use.")]
    [Comment("The order of the used limiters in the list defines the limiter appliance order, so it may matter.")]
    [Comment("Can be a combination of these values: " +
      "'" + nameof(MySqlCountLimiter) + "' (total data rows count limiting in a MySQL database), " +
      "'" + nameof(MySqlPeriodLimiter) + "' (time period limiting in a MySQL database).")]
    public string UseMeasurementLimiters { get; set; } = string.Join(", ", DefaultMeasurementLimiterClassNames);

    /// <summary>
    ///   Gets or sets the measurement loop delay value in seconds.
    /// </summary>
    [Comment("Sets the measurement loop delay value in seconds.")]
    public int MeasurementLoopDelay { get; set; } = DefaultMeasurementLoopDelay;

    /// <summary>
    ///   Gets or sets the dictionary of connection strings providing the database connection parameters.
    /// </summary>
    [Comment("Defines a section of key-value pairs containing the connection strings.")]
    [Comment("Connection strings provide specific options for establishing database connections.")]
    public Dictionary<string, string> ConnectionStrings { get; set; } = new()
    {
      {DefaultConnectionStringKey, DefaultConnectionStringValue}
    };
  }
}
