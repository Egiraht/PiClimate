// ReSharper disable InconsistentNaming
namespace PiClimate.Logger.ConfigurationLayout
{
  /// <summary>
  ///   The configuration's section related to BME280 sensor options.
  /// </summary>
  public static class Bme280Options
  {
    /// <summary>
    ///   Defines the I2C bus ID.
    ///   Can be a decimal or hexadecimal numeric value.
    /// </summary>
    public const string I2cBusId =
      nameof(Bme280Options) + ":" + nameof(I2cBusId);

    /// <summary>
    ///   Defines the I2C bus ID.
    ///   Can be a decimal or hexadecimal numeric value, or <c>null</c> if not used.
    /// </summary>
    public const string CustomI2cAddress =
      nameof(Bme280Options) + ":" + nameof(CustomI2cAddress);
  }
}
