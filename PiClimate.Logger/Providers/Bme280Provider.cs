// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using PiClimate.Logger.Models;
using PiClimate.Logger.Settings;
using UnitsNet;

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
    ///   Gets or sets the BME280 device descriptor.
    /// </summary>
    protected Bme280? Device { get; set; }

    /// <summary>
    ///   Gets or sets the BME280 options object.
    /// </summary>
    protected Bme280Options Options { get; set; } = new();

    /// <inheritdoc />
    public bool IsConfigured { get; protected set; }

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
    ///   <c>true</c> on successful BME280 device connection and identification, otherwise <c>false</c>.
    /// </returns>
    protected static bool TryConnect(int i2cBusId, int i2cDeviceAddress)
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
    public virtual void Configure(GlobalSettings settings)
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(Bme280Provider));

      Options = settings.Bme280Options;

      // Building a list of I2C addresses to check.
      var i2cAddresses = new List<int>
      {
        Bmx280Base.DefaultI2cAddress,
        Bmx280Base.SecondaryI2cAddress
      };
      if (Options.CustomI2cAddress != null)
        i2cAddresses.Insert(0, Options.CustomI2cAddress.Value);

      // Checking the I2C addresses for the device.
      var i2cAddress = i2cAddresses.FirstOrDefault(address => TryConnect(Options.I2cBusId, address));
      if (i2cAddress == default)
      {
        var checkedAddresses = string.Join(", ", i2cAddresses.Select(address => $"0x{address:X2}"));
        throw new IOException(
          $"No BME280 devices found on the I2C bus 0x{Options.I2cBusId:X2}. Checked I2C addresses: {checkedAddresses}.");
      }

      // Configuring the device.
      Device = new Bme280(I2cDevice.Create(new I2cConnectionSettings(Options.I2cBusId, i2cAddress)));
      Device.Reset();
      Device.StandbyTime = Options.StandbyTime;
      Device.FilterMode = Options.FilteringMode;
      Device.PressureSampling = Options.PressureSampling;
      Device.TemperatureSampling = Options.TemperatureSampling;
      Device.HumiditySampling = Options.HumiditySampling;
      Device.SetPowerMode(Options.PowerMode);
      Task.Delay(120).Wait();
      IsConfigured = true;
    }

    /// <inheritdoc />
    public virtual Task ConfigureAsync(GlobalSettings settings) => Task.Run(() => Configure(settings));

    /// <inheritdoc />
    public virtual Measurement Measure() => MeasureAsync().GetAwaiter().GetResult();

    /// <inheritdoc />
    public virtual async Task<Measurement> MeasureAsync()
    {
      if (_disposed)
        throw new ObjectDisposedException(nameof(Bme280Provider));

      if (!IsConfigured || Device == null)
        throw new InvalidOperationException($"{nameof(Bme280Provider)} is not configured.");

      var result = await Device.ReadAsync();
      return new Measurement
      {
        Timestamp = DateTime.Now,
        Pressure = result.Pressure ?? new Pressure(),
        Temperature = result.Temperature ?? new Temperature(),
        Humidity = result.Humidity ?? new RelativeHumidity()
      };
    }

    /// <inheritdoc />
    public virtual void Dispose()
    {
      if (_disposed)
        return;

      Device?.SetPowerMode(Bmx280PowerMode.Sleep);
      Device?.Dispose();

      GC.SuppressFinalize(this);
      _disposed = true;
    }

    /// <inheritdoc />
    ~Bme280Provider() => Dispose();
  }
}
