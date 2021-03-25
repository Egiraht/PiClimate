// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Iot.Device.Bmxx80.PowerMode;

// ReSharper disable InconsistentNaming
namespace PiClimate.Logger.Settings
{
  /// <summary>
  ///   The section of the global settings for BME280 sensor.
  /// </summary>
  public class Bme280Options
  {
    /// <summary>
    ///   Defines the default I2C bus ID.
    /// </summary>
    public const int DefaultI2cBusId = 1;

    /// <summary>
    ///   Gets or sets the I2C bus ID.
    /// </summary>
    public int I2cBusId { get; set; } = DefaultI2cBusId;

    /// <summary>
    ///   Gets or sets the custom I2C bus address.
    ///   Can be <c>null</c> if not used.
    /// </summary>
    public int? CustomI2cAddress { get; set; } = null;

    /// <summary>
    ///   Gets or sets the BME280 pressure sampling mode.
    /// </summary>
    public Sampling PressureSampling { get; set; } = Sampling.UltraHighResolution;

    /// <summary>
    ///   Gets or sets the BME280 temperature sampling mode.
    /// </summary>
    public Sampling TemperatureSampling { get; set; } = Sampling.UltraHighResolution;

    /// <summary>
    ///   Gets or sets the BME280 humidity sampling mode.
    /// </summary>
    public Sampling HumiditySampling { get; set; } = Sampling.UltraHighResolution;

    /// <summary>
    ///   Gets or sets the BME280 measurement filtering mode.
    /// </summary>
    public Bmx280FilteringMode FilteringMode { get; set; } = Bmx280FilteringMode.Off;

    /// <summary>
    ///   Gets or sets the BME280 power mode to be used for measuring: normal (continuous measurements) or forced
    ///   (single measurement on request).
    /// </summary>
    public Bmx280PowerMode PowerMode { get; set; } = Bmx280PowerMode.Normal;

    /// <summary>
    ///   Gets or sets the BME280 standby time. Applicable only when the power mode is set to normal.
    /// </summary>
    public StandbyTime StandbyTime { get; set; } = StandbyTime.Ms62_5;
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings for BME280 sensor.
    /// </summary>
    public Bme280Options Bme280Options { get; set; } = new();
  }
}
