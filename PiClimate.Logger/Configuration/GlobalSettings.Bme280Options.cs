// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

// ReSharper disable InconsistentNaming
namespace PiClimate.Logger.Configuration
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
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings for BME280 sensor.
    /// </summary>
    public Bme280Options Bme280Options { get; set; } = new Bme280Options();
  }
}
