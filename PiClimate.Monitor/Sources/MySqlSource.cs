// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using PiClimate.Monitor.Configuration;
using PiClimate.Monitor.Models;

namespace PiClimate.Monitor.Sources
{
  /// <summary>
  ///   The MySQL database measurement data source.
  /// </summary>
  class MySqlSource : IMeasurementSource
  {
    /// <summary>
    ///   A class for storing the query parameters.
    /// </summary>
    private class QueryParameters
    {
      /// <summary>
      ///   Gets or sets the beginning timestamp of the time period.
      /// </summary>
      public DateTime FromTime { get; set; }

      /// <summary>
      ///   Gets or sets the ending timestamp of the time period.
      /// </summary>
      public DateTime ToTime { get; set; }

      /// <summary>
      ///   Gets or sets the time step in seconds to be used within the time period.
      /// </summary>
      public int TimeStep { get; set; }
    }

    /// <summary>
    ///   The connection string used for MySQL database connection.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    ///   The database table name where the measurement data are stored.
    /// </summary>
    private readonly string _measurementsTableName;

    /// <summary>
    ///   Gets the SQL query for measurement filtering.
    /// </summary>
    private string FilterMeasurementsSqlTemplate => $@"
      SELECT
        FROM_UNIXTIME(MIN(UNIX_TIMESTAMP(`{nameof(Measurement.Timestamp)}`) DIV @{nameof(QueryParameters.TimeStep)} *
          @{nameof(QueryParameters.TimeStep)}))
          AS `{nameof(Measurement.Timestamp)}`,
        ROUND(AVG(`{nameof(Measurement.Pressure)}`), 3)
          AS `{nameof(Measurement.Pressure)}`,
        ROUND(AVG(`{nameof(Measurement.Temperature)}`), 3)
          AS `{nameof(Measurement.Temperature)}`,
        ROUND(AVG(`{nameof(Measurement.Humidity)}`), 3)
          AS `{nameof(Measurement.Humidity)}`
      FROM `{_measurementsTableName}`
      WHERE `{nameof(Measurement.Timestamp)}` BETWEEN @{nameof(QueryParameters.FromTime)} AND
        @{nameof(QueryParameters.ToTime)}
      GROUP BY UNIX_TIMESTAMP(`{nameof(Measurement.Timestamp)}`) DIV @{nameof(QueryParameters.TimeStep)}
      ORDER BY `{nameof(Measurement.Timestamp)}` ASC;
    ";

    /// <summary>
    ///   Gets the SQL query for selecting the latest measurement.
    /// </summary>
    private string LatestMeasurementSqlTemplate => $@"
      SELECT
        `{nameof(Measurement.Timestamp)}`,
        ROUND(`{nameof(Measurement.Pressure)}`, 3) 
          AS `{nameof(Measurement.Pressure)}`,
        ROUND(`{nameof(Measurement.Temperature)}`, 3)
          AS `{nameof(Measurement.Temperature)}`,
        ROUND(`{nameof(Measurement.Humidity)}`, 3)
          AS `{nameof(Measurement.Humidity)}`
      FROM `{_measurementsTableName}`
      ORDER BY `{nameof(Measurement.Timestamp)}` DESC
      LIMIT 1;
    ";

    /// <summary>
    ///   Creates a new MySQL measurement data source instance.
    /// </summary>
    /// <param name="settings">
    ///   The global settings used for configuring the MySQL connection.
    ///   Provided via dependency injection.
    /// </param>
    public MySqlSource(GlobalSettings settings)
    {
      _connectionString =
        settings.ConnectionStrings.TryGetValue(settings.MySqlOptions.UseConnectionStringKey, out var connectionString)
          ? connectionString
          : GlobalSettings.DefaultConnectionStringValue;
      _measurementsTableName = settings.MySqlOptions.MeasurementsTableName;
    }

    /// <inheritdoc />
    public IEnumerable<Measurement> GetMeasurements(MeasurementFilter filter)
    {
      var fromTime = filter.FromTime!.Value <= filter.ToTime ? filter.FromTime!.Value : filter.ToTime;
      var toTime = filter.FromTime!.Value <= filter.ToTime ? filter.ToTime : filter.FromTime!.Value;

      using var connection = new MySqlConnection(_connectionString);
      return connection.Query<Measurement>(FilterMeasurementsSqlTemplate, new QueryParameters
      {
        FromTime = fromTime,
        ToTime = toTime,
        TimeStep = Math.Max((int) ((toTime - fromTime).TotalSeconds / filter.Resolution), 1)
      });
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Measurement>> GetMeasurementsAsync(MeasurementFilter filter)
    {
      var fromTime = filter.FromTime!.Value <= filter.ToTime ? filter.FromTime!.Value : filter.ToTime;
      var toTime = filter.FromTime!.Value <= filter.ToTime ? filter.ToTime : filter.FromTime!.Value;

      await using var connection = new MySqlConnection(_connectionString);
      return await connection.QueryAsync<Measurement>(FilterMeasurementsSqlTemplate, new QueryParameters
      {
        FromTime = fromTime,
        ToTime = toTime,
        TimeStep = Math.Max((int) ((toTime - fromTime).TotalSeconds / filter.Resolution), 1)
      });
    }

    /// <inheritdoc />
    public Measurement? GetLatestMeasurement()
    {
      using var connection = new MySqlConnection(_connectionString);
      return connection.QueryFirstOrDefault<Measurement>(LatestMeasurementSqlTemplate);
    }

    /// <inheritdoc />
    public async Task<Measurement?> GetLatestMeasurementAsync()
    {
      await using var connection = new MySqlConnection(_connectionString);
      return await connection.QueryFirstOrDefaultAsync<Measurement>(LatestMeasurementSqlTemplate);
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
