using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using PiClimate.Logger.ConfigurationLayout;
using PiClimate.Logger.Loggers;

namespace PiClimate.Logger.Limiters
{
  /// <summary>
  ///   The MySQL data row limiter based on the maximal time period.
  /// </summary>
  class MySqlPeriodLimiter : IMeasurementLimiter
  {
    /// <summary>
    ///   The default time period limiting value in seconds.
    /// </summary>
    public const int DefaultPeriodLimit = 86400;

    /// <summary>
    ///   The connection string used for MySQL database connection.
    /// </summary>
    private string? _connectionString;

    /// <summary>
    ///   The database table name for data limiting.
    /// </summary>
    private string _measurementsTableName = MySqlLogger.DefaultMeasurementsTableName;

    /// <summary>
    ///   The time period limiting value in seconds.
    /// </summary>
    private int _periodLimit = DefaultPeriodLimit;

    /// <inheritdoc />
    public bool IsConfigured { get; private set; }

    /// <summary>
    ///   Gets the SQL query used for data row deletion.
    /// </summary>
    private string DeleteSqlTemplate => $@"
      DELETE FROM {_measurementsTableName}
      WHERE `Timestamp` < @Timestamp;
    ";

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void Apply()
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlPeriodLimiter)} is not configured.");

      using var connection = new MySqlConnection(_connectionString);
      connection.Execute(DeleteSqlTemplate,
        new {Timestamp = DateTime.Now - TimeSpan.FromSeconds(_periodLimit)});
    }

    /// <inheritdoc />
    public async Task ApplyAsync()
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlPeriodLimiter)} is not configured.");

      await using var connection = new MySqlConnection(_connectionString);
      await connection.ExecuteAsync(DeleteSqlTemplate,
        new {Timestamp = DateTime.Now - TimeSpan.FromSeconds(_periodLimit)});
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
