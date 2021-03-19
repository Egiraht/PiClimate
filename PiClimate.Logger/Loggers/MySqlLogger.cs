// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using PiClimate.Logger.Configuration;
using PiClimate.Logger.Models;

namespace PiClimate.Logger.Loggers
{
  /// <summary>
  ///   The MySQL database measurement data logger.
  /// </summary>
  class MySqlLogger : IMeasurementLogger
  {
    /// <summary>
    ///   Gets or sets the connection string used for MySQL database connection.
    /// </summary>
    protected string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    ///   Gets or sets the database table name for data logging.
    /// </summary>
    protected string MeasurementsTableName { get; set; }  = MySqlOptions.DefaultMeasurementsTableName;

    /// <inheritdoc />
    public bool IsConfigured { get; private set; }

    /// <summary>
    ///   Gets the SQL query for database table initialization.
    /// </summary>
    private string InitializeSqlTemplate => $@"
      CREATE TABLE IF NOT EXISTS `{MeasurementsTableName}`
      (
        `{nameof(Measurement.Timestamp)}` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP PRIMARY KEY,
        `{nameof(Measurement.Pressure)}` DOUBLE NULL,
        `{nameof(Measurement.Temperature)}` DOUBLE NULL,
        `{nameof(Measurement.Humidity)}` DOUBLE NULL
      );
    ";

    /// <summary>
    ///   Gets the SQL query for new data row insertion.
    /// </summary>
    private string InsertSqlTemplate => $@"
      INSERT INTO `{MeasurementsTableName}`
      (
        `{nameof(Measurement.Timestamp)}`,
        `{nameof(Measurement.Pressure)}`,
        `{nameof(Measurement.Temperature)}`,
        `{nameof(Measurement.Humidity)}`
      )
      VALUES
      (
        @{nameof(Measurement.Timestamp)},
        @{nameof(Measurement.Pressure)},
        @{nameof(Measurement.Temperature)},
        @{nameof(Measurement.Humidity)}
      );
    ";

    /// <inheritdoc />
    public void Configure(GlobalSettings settings) => ConfigureAsync(settings).Wait();

    /// <inheritdoc />
    public async Task ConfigureAsync(GlobalSettings settings)
    {
      ConnectionString =
        settings.ConnectionStrings.TryGetValue(settings.MySqlOptions.UseConnectionStringKey, out var connectionString)
          ? connectionString
          : GlobalSettings.DefaultConnectionStringValue;
      MeasurementsTableName = settings.MySqlOptions.MeasurementsTableName;

      await using (var connection = new MySqlConnection(ConnectionString))
        await connection.ExecuteAsync(InitializeSqlTemplate);

      IsConfigured = true;
    }

    /// <inheritdoc />
    public void LogMeasurement(Measurement measurement) => LogMeasurementAsync(measurement).Wait();

    /// <inheritdoc />
    public async Task LogMeasurementAsync(Measurement measurement)
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlLogger)} is not configured.");

      await using var connection = new MySqlConnection(ConnectionString);
      await connection.ExecuteAsync(InsertSqlTemplate, new Dictionary<string, object>
      {
        {nameof(Measurement.Timestamp), measurement.Timestamp},
        {nameof(Measurement.Pressure), measurement.Pressure.MillimetersOfMercury},
        {nameof(Measurement.Temperature), measurement.Temperature.DegreesCelsius},
        {nameof(Measurement.Humidity), measurement.Humidity.Percent}
      });
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
