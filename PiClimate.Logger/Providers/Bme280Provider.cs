using System;
using System.Device.I2c;
using System.Linq;
using System.Threading.Tasks;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Components;
using PiClimate.Logger.ConfigurationLayout;
using PiClimate.Logger.Models;

// ReSharper disable InconsistentNaming
namespace PiClimate.Logger.Providers
{
  public class Bme280Provider : IMeasurementProvider
  {
    private bool _disposed = false;

    private Bme280? _device;

    public StandbyTime StandbyTime { get; set; } = StandbyTime.Ms62_5;

    public FilteringMode FilteringMode { get; set; } = FilteringMode.Off;

    public Sampling PressureSampling { get; set; } = Sampling.UltraHighResolution;

    public Sampling TemperatureSampling { get; set; } = Sampling.UltraHighResolution;

    public Sampling HumiditySampling { get; set; } = Sampling.UltraHighResolution;

    public Bmx280PowerMode PowerMode { get; set; } = Bmx280PowerMode.Normal;

    public bool IsConfigured { get; private set; }

    public void Configure(IConfiguration configuration)
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(Bme280Provider));

      var busId = int.TryParse(configuration[HardwareOptions.I2cBusId], out var value) ? value : 1;

      byte[] i2cAddresses =
      {
        Bmx280Base.DefaultI2cAddress,
        Bmx280Base.SecondaryI2cAddress
      };
      var i2cAddress = i2cAddresses.FirstOrDefault(address => I2cUtils.TryConnect(busId, address));
      if (i2cAddress == default)
      {
        var checkedAddresses = string.Join(", ", i2cAddresses.Select(address => $"0x{address:X}"));
        throw new Exception(
          $"No BME280 devices found on the I2C bus 0x{busId:X}. Checked I2C addresses: {checkedAddresses}.");
      }

      _device = new Bme280(I2cDevice.Create(new I2cConnectionSettings(busId, i2cAddress)));
      _device.Reset();
      _device.SetStandbyTime(StandbyTime);
      _device.SetFilterMode(FilteringMode);
      _device.SetPressureSampling(PressureSampling);
      _device.SetTemperatureSampling(TemperatureSampling);
      _device.SetHumiditySampling(HumiditySampling);
      _device.SetPowerMode(PowerMode);

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
