namespace PiClimate.Logger.ConfigurationLayout
{
  public static class MySqlOptions
  {
    public const string UseConnectionStringKey =
      nameof(MySqlOptions) + ":" + nameof(UseConnectionStringKey);

    public const string MeasurementsTableName =
      nameof(MySqlOptions) + ":" + nameof(MeasurementsTableName);
  }
}
