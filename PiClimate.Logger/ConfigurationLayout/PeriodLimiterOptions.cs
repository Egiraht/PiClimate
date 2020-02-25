namespace PiClimate.Logger.ConfigurationLayout
{
  /// <summary>
  ///   The configuration's section for period limiters.
  /// </summary>
  public static class PeriodLimiterOptions
  {
    /// <summary>
    ///   Defines the time period in seconds that limits the data row lifetime.
    /// </summary>
    public const string PeriodLimit =
      nameof(PeriodLimiterOptions) + ":" + nameof(PeriodLimit);
  }
}
