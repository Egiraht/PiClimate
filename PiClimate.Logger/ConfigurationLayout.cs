namespace PiClimate.Logger
{
  internal static class ConfigurationLayout
  {
    public static class HardwareOptions
    {
      public const string BusId = nameof(HardwareOptions) + ":" + nameof(BusId);
    }

    public static class DatabaseOptions
    {
      public const string ConnectionStrings = nameof(DatabaseOptions) + ":" + nameof(ConnectionStrings);

      public const string SelectedConnectionStringKey =
        nameof(DatabaseOptions) + ":" + nameof(SelectedConnectionStringKey);

      public const string MeasurementsTableName = nameof(DatabaseOptions) + ":" + nameof(MeasurementsTableName);
    }

    public static class MeasurementOptions
    {
      public const string UseRandomData = nameof(MeasurementOptions) + ":" + nameof(UseRandomData);

      public const string MeasurementLoopDelay = nameof(MeasurementOptions) + ":" + nameof(MeasurementLoopDelay);
    }
  }
}
