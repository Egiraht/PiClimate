using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using PiClimate.Logger.Hardware;
using static PiClimate.Logger.ConfigurationLayout;

namespace PiClimate.Logger.Database
{
  class MySqlLogger : IMeasurementLogger
  {
    private string? _measurementsTableName = DefaultMeasurementsTableName;

    private const string DefaultMeasurementsTableName = "Measurements";

    public string ConnectionString { get; }

    public string? MeasurementsTableName
    {
      get => _measurementsTableName;
      set => _measurementsTableName = !string.IsNullOrEmpty(value) ? value : DefaultMeasurementsTableName;
    }

    public MySqlLogger(IConfiguration configuration)
    {
      ConnectionString = configuration.GetSection(DatabaseOptions.ConnectionStrings)[
        configuration[DatabaseOptions.SelectedConnectionStringKey] ?? "0"] ?? "";
      MeasurementsTableName = configuration[DatabaseOptions.MeasurementsTableName];
    }

    public void Initialize()
    {
      using (var connection = new MySqlConnection(ConnectionString))
        connection.Execute(InitializeSql);
    }

    public async Task InitializeAsync()
    {
      await using (var connection = new MySqlConnection(ConnectionString))
        await connection.ExecuteAsync(InitializeSql);
    }

    public void LogMeasurement(Measurement measurement)
    {
      using (var connection = new MySqlConnection(ConnectionString))
        connection.Execute(InsertSql, measurement);
    }

    public async Task LogMeasurementAsync(Measurement measurement)
    {
      await using (var connection = new MySqlConnection(ConnectionString))
        await connection.ExecuteAsync(InsertSql, measurement);
    }

    private string InitializeSql => string.Format(@"
      CREATE TABLE IF NOT EXISTS `{0}`
      (
        `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP PRIMARY KEY,
        `Pressure` DOUBLE NULL,
        `Temperature` DOUBLE NULL,
        `Humidity` DOUBLE NULL
      );
    ", MeasurementsTableName ?? DefaultMeasurementsTableName);

    private string InsertSql => string.Format(@"
      # noinspection SqlResolve
      INSERT INTO `{0}`
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
    ", MeasurementsTableName ?? DefaultMeasurementsTableName);

    public void Dispose()
    {
    }
  }
}
