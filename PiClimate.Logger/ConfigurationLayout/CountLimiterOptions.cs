namespace PiClimate.Logger.ConfigurationLayout
{
  /// <summary>
  ///   The configuration's section for data row count limiters.
  /// </summary>
  public static class CountLimiterOptions
  {
    /// <summary>
    ///   Defines the total count of data rows to keep in the database table.
    /// </summary>
    public const string CountLimit =
      nameof(CountLimiterOptions) + ":" + nameof(CountLimit);
  }
}
