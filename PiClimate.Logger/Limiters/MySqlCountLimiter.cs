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
    public const int DefaultCountLimit = 1440;

    private string? _connectionString;

    private string _measurementsTableName = MySqlLogger.DefaultMeasurementsTableName;

    private int _countLimit = DefaultCountLimit;

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
      _countLimit = int.TryParse(configuration[CountLimiterOptions.CountLimit], out var value)
        ? value
        : DefaultCountLimit;

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
      _countLimit = int.TryParse(configuration[CountLimiterOptions.CountLimit], out var value)
        ? value
        : DefaultCountLimit;

      await using var connection = new MySqlConnection(_connectionString);
      await connection.OpenAsync();
      await connection.CloseAsync();

      IsConfigured = true;
    }

    public void Apply()
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlCountLimiter)} is not configured.");

      using var connection = new MySqlConnection(_connectionString);
      var count = (long) connection.ExecuteScalar(CountSqlTemplate);
      if (count > _countLimit)
        connection.Execute(DeleteSqlTemplate, new {Count = count - _countLimit});
    }

    public async Task ApplyAsync()
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlCountLimiter)} is not configured.");

      await using var connection = new MySqlConnection(_connectionString);
      var count = (long) await connection.ExecuteScalarAsync(CountSqlTemplate);
      if (count > _countLimit)
        await connection.ExecuteAsync(DeleteSqlTemplate, new {Count = count - _countLimit});
    }

    public void Dispose()
    {
    }
  }
}
