namespace PiClimate.Monitor.ConfigurationLayout
{
  /// <summary>
  ///   The configuration's root section.
  /// </summary>
  public static class Root
  {
    /// <summary>
    ///   Defines the used measurement source.
    /// </summary>
    public const string UseMeasurementSource = nameof(UseMeasurementSource);

    /// <summary>
    ///   Defines the named list of connection strings providing the database connection parameters.
    /// </summary>
    public const string ConnectionStrings = nameof(ConnectionStrings);
  }
}
