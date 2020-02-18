using System.Device.I2c;

// ReSharper disable InconsistentNaming
namespace PiClimate.Logger.Components
{
  public static class I2cUtils
  {
    public static bool TryConnect(int i2cBusId, int i2cDeviceAddress)
    {
      try
      {
        I2cDevice.Create(new I2cConnectionSettings(i2cBusId, i2cDeviceAddress)).ReadByte();
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
