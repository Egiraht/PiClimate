using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.ConfigurationLayout;
using PiClimate.Logger.Models;

// ReSharper disable InconsistentNaming
namespace PiClimate.Logger.Providers
{
  public class Bme280Provider : IMeasurementProvider
  {
    public const int DefaultI2cBusId = 1;

    private bool _disposed = false;

    private Bme280? _device;

    public StandbyTime StandbyTime { get; set; } = StandbyTime.Ms62_5;

    public FilteringMode FilteringMode { get; set; } = FilteringMode.Off;

    public Sampling PressureSampling { get; set; } = Sampling.UltraHighResolution;

    public Sampling TemperatureSampling { get; set; } = Sampling.UltraHighResolution;

    public Sampling HumiditySampling { get; set; } = Sampling.UltraHighResolution;

    public Bmx280PowerMode PowerMode { get; set; } = Bmx280PowerMode.Normal;

    public bool IsConfigured { get; private set; }

    private int? ConvertToInt(string? value)
    {
      if (value == null)
        return null;

      value = value.Trim();

      if (value.StartsWith("0x"))
        return int.TryParse(value.Substring(2), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo,
          out var hexConvertedValue)
          ? hexConvertedValue as int?
          : null;

      return int.TryParse(value, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var decConvertedValue)
        ? decConvertedValue as int?
        : null;
    }

    public bool TryConnect(int i2cBusId, int i2cDeviceAddress)
    {
      try
      {
        using var device = I2cDevice.Create(new I2cConnectionSettings(i2cBusId, i2cDeviceAddress));
        var response = new Span<byte>(new byte[1]);
        device.WriteRead(new ReadOnlySpan<byte>(new byte[] {0xD0}), response);
        return response[0] == 0x60;
      }
      catch
      {
        return false;
      }
    }

    public void Configure(IConfiguration configuration)
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(Bme280Provider));

      // Reading I2C bus ID from configuration.
      var i2cBusId = ConvertToInt(configuration[Bme280Options.I2cBusId]) ?? DefaultI2cBusId;

      // Building a list of I2C addresses to check.
      var i2cAddresses = new List<int>
      {
        Bmx280Base.DefaultI2cAddress,
        Bmx280Base.SecondaryI2cAddress
      };
      var customI2cAddress = ConvertToInt(configuration[Bme280Options.CustomI2cAddress]);
      if (customI2cAddress != null)
        i2cAddresses.Insert(0, customI2cAddress.Value);

      // Checking the I2C addresses for the device.
      var i2cAddress = i2cAddresses.FirstOrDefault(address => TryConnect(i2cBusId, address));
      if (i2cAddress == default)
      {
        var checkedAddresses = string.Join(", ", i2cAddresses.Select(address => $"0x{address:X2}"));
        throw new Exception(
          $"No BME280 devices found on the I2C bus 0x{i2cBusId:X2}. Checked I2C addresses: {checkedAddresses}.");
      }

      // Configuring the device.
      _device = new Bme280(I2cDevice.Create(new I2cConnectionSettings(i2cBusId, i2cAddress)));
      _device.Reset();
      _device.SetStandbyTime(StandbyTime);
      _device.SetFilterMode(FilteringMode);
      _device.SetPressureSampling(PressureSampling);
      _device.SetTemperatureSampling(TemperatureSampling);
      _device.SetHumiditySampling(HumiditySampling);
      _device.SetPowerMode(PowerMode);
      Task.Delay(120).Wait();
      IsConfigured = true;
    }

    public Task ConfigureAsync(IConfiguration configuration) => Task.Run(() => Configure(configuration));

    private double ConvertPressureToMmHg(double pressureInPa) => pressureInPa * 0.00750062;

    public Measurement Measure()
    {
      var measurementTask = MeasureAsync();
      measurementTask.Wait();
      return measurementTask.Result;
    }

    public async Task<Measurement> MeasureAsync()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(Bme280Provider));

      if (!IsConfigured || _device == null)
        throw new InvalidOperationException("The BME280 device is not configured.");

      return new Measurement
      {
        Timestamp = DateTime.Now,
        Pressure = ConvertPressureToMmHg(await _device.ReadPressureAsync()),
        Temperature = (await _device.ReadTemperatureAsync()).Celsius,
        Humidity = await _device.ReadHumidityAsync()
      };
    }

    public void Dispose()
    {
      if (_disposed)
        return;

      _device?.SetPowerMode(Bmx280PowerMode.Sleep);
      _device?.Dispose();

      GC.SuppressFinalize(this);
      _disposed = true;
    }

    ~Bme280Provider()
    {
      Dispose();
    }
  }
}
