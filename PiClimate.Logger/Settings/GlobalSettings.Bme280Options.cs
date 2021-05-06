// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Iot.Device.Bmxx80.PowerMode;
using PiClimate.Common.Settings;

// ReSharper disable InconsistentNaming
namespace PiClimate.Logger.Settings
{
  /// <summary>
  ///   The section of the global settings for BME280 sensor.
  /// </summary>
  public class Bme280Options : SettingsSection
  {
    /// <summary>
    ///   Defines the default I2C bus ID.
    /// </summary>
    public const int DefaultI2cBusId = 1;

    /// <summary>
    ///   Gets or sets the I2C bus ID.
    /// </summary>
    [Comment("Sets the I2C bus ID.")]
    [Comment("Can be a decimal numeric or hexadecimal string value.")]
    public int I2cBusId { get; set; } = DefaultI2cBusId;

    /// <summary>
    ///   Gets or sets the custom I2C bus address.
    ///   Can be set to <c>null</c> if not used.
    /// </summary>
    [Comment("Sets the custom I2C bus address.")]
    [Comment("Can be a decimal numeric or hexadecimal string value, or 'null' if not used.")]
    public int? CustomI2cAddress { get; set; } = null;

    /// <summary>
    ///   Gets or sets the BME280 pressure sampling mode.
    /// </summary>
    [Comment("Sets the BME280 pressure sampling mode.")]
    [Comment("Can be one of these values: " +
      "'" + nameof(Sampling.Skipped) + "' (no measuring is performed), " +
      "'" + nameof(Sampling.UltraLowPower) + "', " +
      "'" + nameof(Sampling.LowPower) + "', " +
      "'" + nameof(Sampling.Standard) + "', " +
      "'" + nameof(Sampling.HighResolution) + "', " +
      "'" + nameof(Sampling.UltraHighResolution) + "'.")]
    public Sampling PressureSampling { get; set; } = Sampling.UltraHighResolution;

    /// <summary>
    ///   Gets or sets the BME280 temperature sampling mode.
    /// </summary>
    [Comment("Sets the BME280 temperature sampling mode.")]
    [Comment("Can be one of these values: " +
      "'" + nameof(Sampling.Skipped) + "' (no measuring is performed), " +
      "'" + nameof(Sampling.UltraLowPower) + "', " +
      "'" + nameof(Sampling.LowPower) + "', " +
      "'" + nameof(Sampling.Standard) + "', " +
      "'" + nameof(Sampling.HighResolution) + "', " +
      "'" + nameof(Sampling.UltraHighResolution) + "'.")]
    public Sampling TemperatureSampling { get; set; } = Sampling.UltraHighResolution;

    /// <summary>
    ///   Gets or sets the BME280 humidity sampling mode.
    /// </summary>
    [Comment("Sets the BME280 humidity sampling mode.")]
    [Comment("Can be one of these values: " +
      "'" + nameof(Sampling.Skipped) + "' (no measuring is performed), " +
      "'" + nameof(Sampling.UltraLowPower) + "', " +
      "'" + nameof(Sampling.LowPower) + "', " +
      "'" + nameof(Sampling.Standard) + "', " +
      "'" + nameof(Sampling.HighResolution) + "', " +
      "'" + nameof(Sampling.UltraHighResolution) + "'.")]
    public Sampling HumiditySampling { get; set; } = Sampling.UltraHighResolution;

    /// <summary>
    ///   Gets or sets the BME280 measurement filtering mode.
    /// </summary>
    [Comment("Sets the BME280 measurement filtering mode.")]
    [Comment("Can be one of these values: " +
      "'" + nameof(Bmx280FilteringMode.Off) + "' (no measurement value filtering), " +
      "'" + nameof(Bmx280FilteringMode.X2) + "', " +
      "'" + nameof(Bmx280FilteringMode.X4) + "', " +
      "'" + nameof(Bmx280FilteringMode.X8) + "', " +
      "'" + nameof(Bmx280FilteringMode.X16) + "'.")]
    public Bmx280FilteringMode FilteringMode { get; set; } = Bmx280FilteringMode.Off;

    /// <summary>
    ///   Gets or sets the BME280 power mode to be used for measuring: normal (continuous measurements) or forced
    ///   (single measurement on request).
    /// </summary>
    [Comment("Sets the BME280 power mode to be used for measuring.")]
    [Comment("Can be one of these values: " +
      "'" + nameof(Bmx280PowerMode.Normal) + "' (continuous measurements), " +
      "'" + nameof(Bmx280PowerMode.Forced) + "' (single measurement on request).")]
    public Bmx280PowerMode PowerMode { get; set; } = Bmx280PowerMode.Normal;

    /// <summary>
    ///   Gets or sets the BME280 standby time. Applicable only when the power mode is set to normal.
    /// </summary>
    [Comment("Sets the BME280 standby time (delay between measurements).")]
    [Comment("Applicable only when the power mode is set to '" + nameof(Bmx280PowerMode.Normal) + "'.")]
    [Comment("Can be one of these values: " +
      "'" + nameof(StandbyTime.Ms0_5) + "' (0.5 ms), " +
      "'" + nameof(StandbyTime.Ms10) + "' (10 ms), " +
      "'" + nameof(StandbyTime.Ms20) + "' (20 ms), " +
      "'" + nameof(StandbyTime.Ms62_5) + "' (62.5 ms), " +
      "'" + nameof(StandbyTime.Ms125) + "' (125 ms), " +
      "'" + nameof(StandbyTime.Ms250) + "' (250 ms), " +
      "'" + nameof(StandbyTime.Ms500) + "' (500 ms), " +
      "'" + nameof(StandbyTime.Ms1000) + "' (1 s).")]
    public StandbyTime StandbyTime { get; set; } = StandbyTime.Ms62_5;
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings object for BME280 sensor.
    /// </summary>
    [Comment("The settings section related to the BME280 sensor options.")]
    public Bme280Options Bme280Options { get; set; } = new();
  }
}
