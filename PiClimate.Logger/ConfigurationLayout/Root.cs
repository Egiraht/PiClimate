namespace PiClimate.Logger.ConfigurationLayout
{
  /// <summary>
  ///   The configuration's root section.
  /// </summary>
  public static class Root
  {
    /// <summary>
    ///   Defines the used measurement provider.
    /// </summary>
    public const string UseMeasurementProvider = nameof(UseMeasurementProvider);

    /// <summary>
    ///   Defines a comma-separated list of the used measurement loggers.
    /// </summary>
    public const string UseMeasurementLoggers = nameof(UseMeasurementLoggers);

    /// <summary>
    ///   Defines a comma-separated list of the used measurement limiters.
    /// </summary>
    public const string UseMeasurementLimiters = nameof(UseMeasurementLimiters);

    /// <summary>
    ///   Defines the measurement loop delay value in seconds.
    /// </summary>
    public const string MeasurementLoopDelay = nameof(MeasurementLoopDelay);

    /// <summary>
    ///   Defines the named list of connection strings providing the database connection parameters.
    /// </summary>
    public const string ConnectionStrings = nameof(ConnectionStrings);
  }
}
