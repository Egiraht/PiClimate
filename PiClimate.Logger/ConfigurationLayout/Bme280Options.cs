// ReSharper disable InconsistentNaming
namespace PiClimate.Logger.ConfigurationLayout
{
  public static class Bme280Options
  {
    public const string I2cBusId =
      nameof(Bme280Options) + ":" + nameof(I2cBusId);

    public const string CustomI2cAddress =
      nameof(Bme280Options) + ":" + nameof(CustomI2cAddress);
  }
}
