// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using PiClimate.Common.Models;
using PiClimate.Logger.Settings;

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
    ///   Gets or sets the connection string used for MySQL database connection.
    /// </summary>
    protected string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    ///   Gets or sets the database table name for data logging.
    /// </summary>
    protected string MeasurementsTableName { get; set; }  = MySqlOptions.DefaultMeasurementsTableName;

    /// <summary>
    ///   Gets or sets the time period limiting value in seconds.
    /// </summary>
    protected int PeriodLimit { get; set; } = PeriodLimiterOptions.DefaultPeriodLimit;

    /// <inheritdoc />
    public bool IsConfigured { get; private set; }

    /// <summary>
    ///   Gets the SQL query used for data row deletion.
    /// </summary>
    private string DeleteSqlTemplate => $@"
      DELETE FROM {MeasurementsTableName}
      WHERE `{nameof(Measurement.Timestamp)}` < @{nameof(QueryParameters.LastTimestamp)};
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
      PeriodLimit = settings.PeriodLimiterOptions.PeriodLimit;

      await using var connection = new MySqlConnection(ConnectionString);
      await connection.OpenAsync();
      await connection.CloseAsync();

      IsConfigured = true;
    }

    /// <inheritdoc />
    public void Apply() => ApplyAsync().Wait();

    /// <inheritdoc />
    public async Task ApplyAsync()
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlPeriodLimiter)} is not configured.");

      await using var connection = new MySqlConnection(ConnectionString);
      await connection.ExecuteAsync(DeleteSqlTemplate,
        new QueryParameters {LastTimestamp = DateTime.Now - TimeSpan.FromSeconds(PeriodLimit)});
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
