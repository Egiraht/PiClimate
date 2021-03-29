// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using PiClimate.Common.Models;
using PiClimate.Monitor.Settings;

namespace PiClimate.Monitor.Sources
{
  /// <summary>
  ///   The MySQL database measurement data source.
  /// </summary>
  class MySqlSource : IMeasurementSource
  {
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
        FROM_UNIXTIME(MIN(UNIX_TIMESTAMP(`{nameof(Measurement.Timestamp)}`) DIV @{nameof(MeasurementFilter.TimeStep)} *
          @{nameof(MeasurementFilter.TimeStep)}))
          AS `{nameof(Measurement.Timestamp)}`,
        ROUND(AVG(`{nameof(Measurement.Pressure)}`), 3)
          AS `{nameof(Measurement.PressureInMmHg)}`,
        ROUND(AVG(`{nameof(Measurement.Temperature)}`), 3)
          AS `{nameof(Measurement.TemperatureInDegC)}`,
        ROUND(AVG(`{nameof(Measurement.Humidity)}`), 3)
          AS `{nameof(Measurement.HumidityInPercent)}`
      FROM `{_measurementsTableName}`
      WHERE `{nameof(Measurement.Timestamp)}` BETWEEN @{nameof(MeasurementFilter.PeriodStart)} AND
        @{nameof(MeasurementFilter.PeriodEnd)}
      GROUP BY UNIX_TIMESTAMP(`{nameof(Measurement.Timestamp)}`) DIV @{nameof(MeasurementFilter.TimeStep)}
      ORDER BY `{nameof(Measurement.Timestamp)}` ASC;
    ";

    /// <summary>
    ///   Gets the SQL query for selecting the latest measurement.
    /// </summary>
    private string LatestMeasurementSqlTemplate => $@"
      SELECT
        `{nameof(Measurement.Timestamp)}`,
        ROUND(`{nameof(Measurement.Pressure)}`, 3) 
          AS `{nameof(Measurement.PressureInMmHg)}`,
        ROUND(`{nameof(Measurement.Temperature)}`, 3)
          AS `{nameof(Measurement.TemperatureInDegC)}`,
        ROUND(`{nameof(Measurement.Humidity)}`, 3)
          AS `{nameof(Measurement.HumidityInPercent)}`
      FROM `{_measurementsTableName}`
      ORDER BY `{nameof(Measurement.Timestamp)}` DESC
      LIMIT @{nameof(LatestDataRequest.MaxRows)};
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
    public IEnumerable<Measurement> GetMeasurements(MeasurementFilter filter) =>
      GetMeasurementsAsync(filter).GetAwaiter().GetResult();

    /// <inheritdoc />
    public async Task<IEnumerable<Measurement>> GetMeasurementsAsync(MeasurementFilter filter)
    {
      filter.PeriodStart = filter.PeriodStart.ToLocalTime();
      filter.PeriodEnd = filter.PeriodEnd.ToLocalTime();
      await using var connection = new MySqlConnection(_connectionString);
      return await connection.QueryAsync<Measurement>(FilterMeasurementsSqlTemplate, filter);
    }

    /// <inheritdoc />
    public IEnumerable<Measurement> GetLatestMeasurements(LatestDataRequest request) =>
      GetLatestMeasurementsAsync(request).GetAwaiter().GetResult();

    /// <inheritdoc />
    public async Task<IEnumerable<Measurement>> GetLatestMeasurementsAsync(LatestDataRequest request)
    {
      await using var connection = new MySqlConnection(_connectionString);
      return await connection.QueryAsync<Measurement>(LatestMeasurementSqlTemplate, request);
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
