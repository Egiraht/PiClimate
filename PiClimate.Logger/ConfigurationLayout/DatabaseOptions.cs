namespace PiClimate.Logger.ConfigurationLayout
{
  public static class DatabaseOptions
  {
    public const string ConnectionStrings = nameof(DatabaseOptions) + ":" + nameof(ConnectionStrings);

    public const string SelectedConnectionStringKey =
      nameof(DatabaseOptions) + ":" + nameof(SelectedConnectionStringKey);

    public const string MeasurementsTableName = nameof(DatabaseOptions) + ":" + nameof(MeasurementsTableName);
  }
}
