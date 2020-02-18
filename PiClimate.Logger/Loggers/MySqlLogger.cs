using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using PiClimate.Logger.ConfigurationLayout;
using PiClimate.Logger.Models;

namespace PiClimate.Logger.Loggers
{
  class MySqlLogger : IMeasurementLogger
  {
    private const string DefaultMeasurementsTableName = "Measurements";

    private string? _connectionString;

    private string _measurementsTableName = DefaultMeasurementsTableName;

    public bool IsConfigured { get; private set; }

    private string InitializeSqlTemplate => $@"
      CREATE TABLE IF NOT EXISTS `{_measurementsTableName}`
      (
        `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP PRIMARY KEY,
        `Pressure` DOUBLE NULL,
        `Temperature` DOUBLE NULL,
        `Humidity` DOUBLE NULL
      );
    ";

    private string InsertSqlTemplate => $@"
      INSERT INTO `{_measurementsTableName}`
      (
        `Timestamp`,
        `Pressure`,
        `Temperature`,
        `Humidity`
      )
      VALUES
      (
        @Timestamp,
        @Pressure,
        @Temperature,
        @Humidity
      );
    ";

    public void Configure(IConfiguration configuration)
    {
      _connectionString = configuration.GetSection(DatabaseOptions.ConnectionStrings)[
        configuration[DatabaseOptions.SelectedConnectionStringKey] ?? "0"] ?? "";
      _measurementsTableName = configuration[DatabaseOptions.MeasurementsTableName] ?? DefaultMeasurementsTableName;

      using (var connection = new MySqlConnection(_connectionString))
        connection.Execute(InitializeSqlTemplate);

      IsConfigured = true;
    }

    public async Task ConfigureAsync(IConfiguration configuration)
    {
      _connectionString = configuration.GetSection(DatabaseOptions.ConnectionStrings)[
        configuration[DatabaseOptions.SelectedConnectionStringKey] ?? "0"] ?? "";
      _measurementsTableName = configuration[DatabaseOptions.MeasurementsTableName] ?? DefaultMeasurementsTableName;

      await using (var connection = new MySqlConnection(_connectionString))
        await connection.ExecuteAsync(InitializeSqlTemplate);

      IsConfigured = true;
    }

    public void LogMeasurement(Measurement measurement)
    {
      if (!IsConfigured)
        throw new InvalidOperationException("The MySQL logger is not configured.");

      using (var connection = new MySqlConnection(_connectionString))
        connection.Execute(InsertSqlTemplate, measurement);
    }

    public async Task LogMeasurementAsync(Measurement measurement)
    {
      if (!IsConfigured)
        throw new InvalidOperationException("The MySQL logger is not configured.");

      await using (var connection = new MySqlConnection(_connectionString))
        await connection.ExecuteAsync(InsertSqlTemplate, measurement);
    }

    public void Dispose()
    {
    }
  }
}
