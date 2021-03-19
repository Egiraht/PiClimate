// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using PiClimate.Logger.Configuration;
using PiClimate.Logger.Models;

namespace PiClimate.Logger.Limiters
{
  /// <summary>
  ///   The MySQL data row limiter based on the maximal data row count.
  /// </summary>
  class MySqlCountLimiter : IMeasurementLimiter
  {
    /// <summary>
    ///   A class for storing the query parameters.
    /// </summary>
    private class QueryParameters
    {
      /// <summary>
      ///   Gets or sets the count of data rows to be deleted from the database table.
      /// </summary>
      public long CountToDelete { get; set; }
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
    ///   Gets or sets the total data row count limit.
    /// </summary>
    protected int CountLimit { get; set; } = CountLimiterOptions.DefaultCountLimit;

    /// <inheritdoc />
    public bool IsConfigured { get; private set; }

    /// <summary>
    ///   Gets the SQL query used for data row counting.
    /// </summary>
    private string CountSqlTemplate => $@"SELECT COUNT(*) FROM {MeasurementsTableName}";

    /// <summary>
    ///   Gets the SQL query used for data row deletion.
    /// </summary>
    private string DeleteSqlTemplate => $@"
      DELETE FROM {MeasurementsTableName}
      ORDER BY `{nameof(Measurement.Timestamp)}`
      LIMIT @{nameof(QueryParameters.CountToDelete)};
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
      CountLimit = settings.CountLimiterOptions.CountLimit;

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
        throw new InvalidOperationException($"{nameof(MySqlCountLimiter)} is not configured.");

      await using var connection = new MySqlConnection(ConnectionString);
      var count = (long) await connection.ExecuteScalarAsync(CountSqlTemplate);
      if (count > CountLimit)
        await connection.ExecuteAsync(DeleteSqlTemplate, new QueryParameters {CountToDelete = count - CountLimit});
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
