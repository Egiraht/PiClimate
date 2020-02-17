using System;
using System.Device.I2c;
using System.Threading.Tasks;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using Microsoft.Extensions.Configuration;
using static PiClimate.Logger.ConfigurationLayout;

// ReSharper disable InconsistentNaming
namespace PiClimate.Logger.Hardware
{
  public class Bme280Provider : IMeasurementProvider
  {
    private bool _disposed = false;

    private readonly Bme280 _device;

    public StandbyTime StandbyTime { get; set; } = StandbyTime.Ms62_5;

    public FilteringMode FilteringMode { get; set; } = FilteringMode.Off;

    public Sampling PressureSampling { get; set; } = Sampling.UltraHighResolution;

    public Sampling TemperatureSampling { get; set; } = Sampling.UltraHighResolution;

    public Sampling HumiditySampling { get; set; } = Sampling.UltraHighResolution;

    public Bmx280PowerMode PowerMode { get; set; } = Bmx280PowerMode.Normal;

    public bool IsConfigured { get; private set; }

    public Bme280Provider(IConfiguration configuration)
    {
      var busId = int.TryParse(configuration[HardwareOptions.BusId], out var value) ? value : 1;

      byte i2cAddress;
      if (I2cUtils.TryConnect(busId, Bmx280Base.DefaultI2cAddress))
        i2cAddress = Bmx280Base.DefaultI2cAddress;
      else if (I2cUtils.TryConnect(busId, Bmx280Base.SecondaryI2cAddress))
        i2cAddress = Bmx280Base.SecondaryI2cAddress;
      else
        throw new Exception($"No BME280 devices found on the I2C bus 0x{busId:X}. " +
          $"Checked I2C addresses: 0x{Bmx280Base.DefaultI2cAddress:X}, 0x{Bmx280Base.SecondaryI2cAddress:X}.");

      _device = new Bme280(I2cDevice.Create(new I2cConnectionSettings(busId, i2cAddress)));
    }

    public virtual void Initialize()
    {
      _device.Reset();
      _device.SetStandbyTime(StandbyTime);
      _device.SetFilterMode(FilteringMode);
      _device.SetPressureSampling(PressureSampling);
      _device.SetTemperatureSampling(TemperatureSampling);
      _device.SetHumiditySampling(HumiditySampling);
      _device.SetPowerMode(PowerMode);
      IsConfigured = true;
    }

    public virtual Task InitializeAsync() => Task.Run(Initialize);

    private double ConvertPressureToMmHg(double pressureInPa) => pressureInPa * 0.00750062;

    public virtual Measurement Measure()
    {
      var measurementTask = MeasureAsync();
      measurementTask.Wait();
      return measurementTask.Result;
    }

    public virtual async Task<Measurement> MeasureAsync() =>
      new Measurement
      {
        Timestamp = DateTime.Now,
        Pressure = ConvertPressureToMmHg(await _device.ReadPressureAsync()),
        Temperature = (await _device.ReadTemperatureAsync()).Celsius,
        Humidity = await _device.ReadHumidityAsync()
      };

    public void Dispose()
    {
      if (_disposed)
        return;

      // The _device field may appear uninitialized if an exception was thrown in the constructor.
      // ReSharper disable once ConstantConditionalAccessQualifier
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
