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
  ///   The MySQL data row limiter based on the maximal data row count.
  /// </summary>
  class MySqlCountLimiter : IMeasurementLimiter
  {
    /// <summary>
    ///   The default total data row count limit.
    /// </summary>
    public const int DefaultCountLimit = 1440;

    /// <summary>
    ///   The connection string used for MySQL database connection.
    /// </summary>
    private string? _connectionString;

    /// <summary>
    ///   The database table name for data limiting.
    /// </summary>
    private string _measurementsTableName = MySqlLogger.DefaultMeasurementsTableName;

    /// <summary>
    ///   The total data row count limit.
    /// </summary>
    private int _countLimit = DefaultCountLimit;

    /// <inheritdoc />
    public bool IsConfigured { get; private set; }

    /// <summary>
    ///   Gets the SQL query used for data row counting.
    /// </summary>
    private string CountSqlTemplate => $@"SELECT COUNT(*) FROM {_measurementsTableName}";

    /// <summary>
    ///   Gets the SQL query used for data row deletion.
    /// </summary>
    private string DeleteSqlTemplate => $@"
      DELETE FROM {_measurementsTableName}
      ORDER BY Timestamp
      LIMIT @Count;
    ";

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void Apply()
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlCountLimiter)} is not configured.");

      using var connection = new MySqlConnection(_connectionString);
      var count = (long) connection.ExecuteScalar(CountSqlTemplate);
      if (count > _countLimit)
        connection.Execute(DeleteSqlTemplate, new {Count = count - _countLimit});
    }

    /// <inheritdoc />
    public async Task ApplyAsync()
    {
      if (!IsConfigured)
        throw new InvalidOperationException($"{nameof(MySqlCountLimiter)} is not configured.");

      await using var connection = new MySqlConnection(_connectionString);
      var count = (long) await connection.ExecuteScalarAsync(CountSqlTemplate);
      if (count > _countLimit)
        await connection.ExecuteAsync(DeleteSqlTemplate, new {Count = count - _countLimit});
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
  }
}
