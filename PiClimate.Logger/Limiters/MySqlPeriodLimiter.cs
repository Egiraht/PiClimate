// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using PiClimate.Logger.Configuration;
using PiClimate.Logger.Models;

namespace PiClimate.Logger.Limiters
{
  /// <summary>
  ///   The MySQL data row limiter based on the maximal time period.
  /// </summary>
  class MySqlPeriodLimiter : IMeasurementLimiter
  {
    /// <summary>
    ///   A class for storing the query parameters.
    /// </summary>
    private class QueryParameters
    {
      /// <summary>
      ///   Gets or sets the last timestamp that limits the oldest data rows to be kept in the database table.
      /// </summary>
      public DateTime LastTimestamp { get; set; }
    }

    /// <summary>
    ///   The connection string used for MySQL database connection.
    /// </summary>
    private string? _connectionString;

    /// <summary>
    ///   The database table name for data limiting.
    /// </summary>
    private string _measurementsTableName = MySqlOptions.DefaultMeasurementsTableName;

    /// <summary>
    ///   The time period limiting value in seconds.
    /// </summary>
    private int _periodLimit = PeriodLimiterOptions.DefaultPeriodLimit;

    /// <inheritdoc />
    public bool IsConfigured { get; private set; }

    /// <summary>
    ///   Gets the SQL query used for data row deletion.
    /// </summary>
    private string DeleteSqlTemplate => $@"
      DELETE FROM {_measurementsTableName}
      WHERE `{nameof(Measurement.Timestamp)}` < @{nameof(QueryParameters.LastTimestamp)};
    ";

    /// <inheritdoc />
    public void Configure(GlobalSettings settings)
    {
      _connectionString =
        settings.ConnectionStrings.TryGetValue(settings.MySqlOptions.UseConnectionStringKey, out var connectionString)
          ? connectionString
          : GlobalSettings.DefaultConnectionStringValue;
      _measurementsTableName = settings.MySqlOptions.MeasurementsTableName;
      _periodLimit = settings.PeriodLimiterOptions.PeriodLimit;

      using var connection = new MySqlConnection(_connectionString);
      connection.Open();
      connection.Close();

      IsConfigured = true;
    }

    /// <inheritdoc />
    public async Task ConfigureAsync(GlobalSettings settings)
    {
      _connectionString =
        settings.ConnectionStrings.TryGetValue(settings.MySqlOptions.UseConnectionStringKey, out var connectionString)
          ? connectionString
          : GlobalSettings.DefaultConnectionStringValue;
      _measurementsTableName = settings.MySqlOptions.MeasurementsTableName;
      _periodLimit = settings.PeriodLimiterOptions.PeriodLimit;

      await using var connection = new MySqlConnection(_connectionString);
      await connection.OpenAsync();
      await connection.CloseAsync();

      IsConfigured = true;
    }

    /// <inheritdoc />
    public void Apply()
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlPeriodLimiter)} is not configured.");

      using var connection = new MySqlConnection(_connectionString);
      connection.Execute(DeleteSqlTemplate,
        new QueryParameters {LastTimestamp = DateTime.Now - TimeSpan.FromSeconds(_periodLimit)});
    }

    /// <inheritdoc />
    public async Task ApplyAsync()
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlPeriodLimiter)} is not configured.");

      await using var connection = new MySqlConnection(_connectionString);
      await connection.ExecuteAsync(DeleteSqlTemplate,
        new QueryParameters {LastTimestamp = DateTime.Now - TimeSpan.FromSeconds(_periodLimit)});
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
