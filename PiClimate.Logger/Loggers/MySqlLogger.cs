using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using PiClimate.Logger.ConfigurationLayout;
using PiClimate.Logger.Models;

namespace PiClimate.Logger.Loggers
{
  /// <summary>
  ///   The MySQL database measurement data logger.
  /// </summary>
  class MySqlLogger : IMeasurementLogger
  {
    /// <summary>
    ///   The default database table name for data logging.
    /// </summary>
    public const string DefaultMeasurementsTableName = "Measurements";

    /// <summary>
    ///   The connection string used for MySQL database connection.
    /// </summary>
    private string? _connectionString;

    /// <summary>
    ///   The database table name for data logging.
    /// </summary>
    private string _measurementsTableName = DefaultMeasurementsTableName;

    /// <inheritdoc />
    public bool IsConfigured { get; private set; }

    /// <summary>
    ///   Gets the SQL query for database table initialization.
    /// </summary>
    private string InitializeSqlTemplate => $@"
      CREATE TABLE IF NOT EXISTS `{_measurementsTableName}`
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
      INSERT INTO `{_measurementsTableName}`
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
    public void Configure(IConfiguration configuration)
    {
      _connectionString =
        configuration.GetSection(Root.ConnectionStrings)[configuration[MySqlOptions.UseConnectionStringKey]] ?? "";
      _measurementsTableName = configuration[MySqlOptions.MeasurementsTableName] ?? DefaultMeasurementsTableName;

      using (var connection = new MySqlConnection(_connectionString))
        connection.Execute(InitializeSqlTemplate);

      IsConfigured = true;
    }

    /// <inheritdoc />
    public async Task ConfigureAsync(IConfiguration configuration)
    {
      _connectionString =
        configuration.GetSection(Root.ConnectionStrings)[configuration[MySqlOptions.UseConnectionStringKey]] ?? "";
      _measurementsTableName = configuration[MySqlOptions.MeasurementsTableName] ?? DefaultMeasurementsTableName;

      await using (var connection = new MySqlConnection(_connectionString))
        await connection.ExecuteAsync(InitializeSqlTemplate);

      IsConfigured = true;
    }

    /// <inheritdoc />
    public void LogMeasurement(Measurement measurement)
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlLogger)} is not configured.");

      using var connection = new MySqlConnection(_connectionString);
      connection.Execute(InsertSqlTemplate, measurement);
    }

    /// <inheritdoc />
    public async Task LogMeasurementAsync(Measurement measurement)
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlLogger)} is not configured.");

      await using var connection = new MySqlConnection(_connectionString);
      await connection.ExecuteAsync(InsertSqlTemplate, measurement);
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
