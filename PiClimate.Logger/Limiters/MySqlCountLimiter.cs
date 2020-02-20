using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using PiClimate.Logger.ConfigurationLayout;
using PiClimate.Logger.Loggers;

namespace PiClimate.Logger.Limiters
{
  class MySqlCountLimiter : IMeasurementLimiter
  {
    public const int DefaultMaxMeasurementsCount = 1440;

    private string? _connectionString;

    private string _measurementsTableName = MySqlLogger.DefaultMeasurementsTableName;

    private int _maxMeasurementsCount = DefaultMaxMeasurementsCount;

    public bool IsConfigured { get; private set; }

    private string CountSqlTemplate => $@"SELECT COUNT(*) FROM {_measurementsTableName}";

    private string DeleteSqlTemplate => $@"
      DELETE FROM {_measurementsTableName}
      ORDER BY Timestamp
      LIMIT @Count;
    ";

    public void Configure(IConfiguration configuration)
    {
      _connectionString =
        configuration.GetSection(Root.ConnectionStrings)[configuration[MySqlOptions.UseConnectionStringKey]] ?? "";
      _measurementsTableName =
        configuration[MySqlOptions.MeasurementsTableName] ?? MySqlLogger.DefaultMeasurementsTableName;
      _maxMeasurementsCount = int.TryParse(configuration[CountLimiterOptions.MaxMeasurementsCount], out var value)
        ? value
        : DefaultMaxMeasurementsCount;

      using var connection = new MySqlConnection(_connectionString);
      connection.Open();
      connection.Close();

      IsConfigured = true;
    }

    public async Task ConfigureAsync(IConfiguration configuration)
    {
      _connectionString =
        configuration.GetSection(Root.ConnectionStrings)[configuration[MySqlOptions.UseConnectionStringKey]] ?? "";
      _measurementsTableName =
        configuration[MySqlOptions.MeasurementsTableName] ?? MySqlLogger.DefaultMeasurementsTableName;
      _maxMeasurementsCount = int.TryParse(configuration[CountLimiterOptions.MaxMeasurementsCount], out var value)
        ? value
        : DefaultMaxMeasurementsCount;

      await using var connection = new MySqlConnection(_connectionString);
      await connection.OpenAsync();
      await connection.CloseAsync();

      IsConfigured = true;
    }

    public void Apply()
    {
      if (!IsConfigured)
        throw new InvalidOperationException("The MySQL measurement count limiter is not configured.");

      using var connection = new MySqlConnection(_connectionString);
      var measurementsCount = (long) connection.ExecuteScalar(CountSqlTemplate);
      if (measurementsCount > _maxMeasurementsCount)
        connection.Execute(DeleteSqlTemplate, new {Count = measurementsCount - _maxMeasurementsCount});
    }

    public async Task ApplyAsync()
    {
      if (!IsConfigured)
        throw new InvalidOperationException("The MySQL measurement count limiter is not configured.");

      await using var connection = new MySqlConnection(_connectionString);
      var measurementsCount = (long) await connection.ExecuteScalarAsync(CountSqlTemplate);
      if (measurementsCount > _maxMeasurementsCount)
        await connection.ExecuteAsync(DeleteSqlTemplate, new {Count = measurementsCount - _maxMeasurementsCount});
    }

    public void Dispose()
    {
    }
  }
}
