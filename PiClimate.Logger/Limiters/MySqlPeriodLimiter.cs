using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using PiClimate.Logger.ConfigurationLayout;
using PiClimate.Logger.Loggers;

namespace PiClimate.Logger.Limiters
{
  class MySqlPeriodLimiter : IMeasurementLimiter
  {
    public const int DefaultPeriodLimit = 86400;

    private string? _connectionString;

    private string _measurementsTableName = MySqlLogger.DefaultMeasurementsTableName;

    private int _periodLimit = DefaultPeriodLimit;

    public bool IsConfigured { get; private set; }

    private string DeleteSqlTemplate => $@"
      DELETE FROM {_measurementsTableName}
      WHERE `Timestamp` < @Timestamp;
    ";

    public void Configure(IConfiguration configuration)
    {
      _connectionString =
        configuration.GetSection(Root.ConnectionStrings)[configuration[MySqlOptions.UseConnectionStringKey]] ?? "";
      _measurementsTableName =
        configuration[MySqlOptions.MeasurementsTableName] ?? MySqlLogger.DefaultMeasurementsTableName;
      _periodLimit = int.TryParse(configuration[PeriodLimiterOptions.PeriodLimit], out var value)
        ? value
        : DefaultPeriodLimit;

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
      _periodLimit = int.TryParse(configuration[PeriodLimiterOptions.PeriodLimit], out var value)
        ? value
        : DefaultPeriodLimit;

      await using var connection = new MySqlConnection(_connectionString);
      await connection.OpenAsync();
      await connection.CloseAsync();

      IsConfigured = true;
    }

    public void Apply()
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlPeriodLimiter)} is not configured.");

      using var connection = new MySqlConnection(_connectionString);
      connection.Execute(DeleteSqlTemplate,
        new {Timestamp = DateTime.Now - TimeSpan.FromSeconds(_periodLimit)});
    }

    public async Task ApplyAsync()
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlPeriodLimiter)} is not configured.");

      await using var connection = new MySqlConnection(_connectionString);
      await connection.ExecuteAsync(DeleteSqlTemplate,
        new {Timestamp = DateTime.Now - TimeSpan.FromSeconds(_periodLimit)});
    }

    public void Dispose()
    {
    }
  }
}
