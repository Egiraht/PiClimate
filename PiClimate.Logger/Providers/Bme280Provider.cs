// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using PiClimate.Logger.Configuration;
using PiClimate.Logger.Models;

// ReSharper disable InconsistentNaming
namespace PiClimate.Logger.Providers
{
  /// <summary>
  ///   A measurement provider that uses the BME280 climatic sensor connected via I2C bus.
  /// </summary>
  public class Bme280Provider : IMeasurementProvider
  {
    /// <summary>
    ///   The object's disposal flag.
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    ///   The BME280 device descriptor.
    /// </summary>
    private Bme280? _device;

    /// <summary>
    ///   Gets or sets the BME280 standby time.
    /// </summary>
    public StandbyTime StandbyTime { get; set; } = StandbyTime.Ms62_5;

    /// <summary>
    ///   Gets or sets the BME280 measurement filtering.
    /// </summary>
    public FilteringMode FilteringMode { get; set; } = FilteringMode.Off;

    /// <summary>
    ///   Gets or sets the BME280 pressure oversampling rate.
    /// </summary>
    public Sampling PressureSampling { get; set; } = Sampling.UltraHighResolution;

    /// <summary>
    ///   Gets or sets the BME280 temperature oversampling rate.
    /// </summary>
    public Sampling TemperatureSampling { get; set; } = Sampling.UltraHighResolution;

    /// <summary>
    ///   Gets or sets the BME280 humidity oversampling rate.
    /// </summary>
    public Sampling HumiditySampling { get; set; } = Sampling.UltraHighResolution;

    /// <summary>
    ///   Gets or sets the BME280 active power mode.
    /// </summary>
    public Bmx280PowerMode PowerMode { get; set; } = Bmx280PowerMode.Normal;

    /// <inheritdoc />
    public bool IsConfigured { get; private set; }

    /// <summary>
    ///   Converts the decimal or hexadecimal string value to integer value.
    /// </summary>
    /// <param name="value">
    ///   The decimal or hexadecimal string value to be converted.
    /// </param>
    /// <returns>
    ///   The converted integer value on success or <c>null</c> otherwise.
    /// </returns>
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

    /// <summary>
    ///   Tries to connect to a BME280 device using the provided I2C bus ID and I2C device address.
    /// </summary>
    /// <param name="i2cBusId">
    ///   The I2C bus ID for connection.
    /// </param>
    /// <param name="i2cDeviceAddress">
    ///   The I2C device address for connection.
    /// </param>
    /// <returns>
    ///   <c>true</c> on successful BME280 device connection, otherwise <c>false</c>.
    /// </returns>
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

    /// <inheritdoc />
    public void Configure(GlobalSettings settings)
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(Bme280Provider));

      // Reading I2C bus ID from configuration.
      var i2cBusId = settings.Bme280Options.I2cBusId;

      // Building a list of I2C addresses to check.
      var i2cAddresses = new List<int>
      {
        Bmx280Base.DefaultI2cAddress,
        Bmx280Base.SecondaryI2cAddress
      };
      var customI2cAddress = settings.Bme280Options.CustomI2cAddress;
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

    /// <inheritdoc />
    public Task ConfigureAsync(GlobalSettings settings) => Task.Run(() => Configure(settings));

    /// <summary>
    ///   Converts the pressure in Pa to mmHg.
    /// </summary>
    /// <param name="pressureInPa">
    ///   The pressure value in Pa.
    /// </param>
    /// <returns>
    ///   The pressure value in mmHg.
    /// </returns>
    private double ConvertPressureToMmHg(double pressureInPa) => pressureInPa * 0.00750062;

    /// <inheritdoc />
    public Measurement Measure()
    {
      var measurementTask = MeasureAsync();
      measurementTask.Wait();
      return measurementTask.Result;
    }

    /// <inheritdoc />
    public async Task<Measurement> MeasureAsync()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(Bme280Provider));

      if (!IsConfigured || _device == null)
        throw new InvalidOperationException($"{nameof(Bme280Provider)} is not configured.");

      return new Measurement
      {
        Timestamp = DateTime.Now,
        Pressure = ConvertPressureToMmHg(await _device.ReadPressureAsync()),
        Temperature = (await _device.ReadTemperatureAsync()).Celsius,
        Humidity = await _device.ReadHumidityAsync()
      };
    }

    /// <inheritdoc />
    public void Dispose()
    {
      if (_disposed)
        return;

      _device?.SetPowerMode(Bmx280PowerMode.Sleep);
      _device?.Dispose();

      GC.SuppressFinalize(this);
      _disposed = true;
    }

    /// <inheritdoc />
    ~Bme280Provider()
    {
      Dispose();
    }
  }
}
