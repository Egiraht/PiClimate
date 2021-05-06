// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using PiClimate.Common.Settings;

namespace PiClimate.Logger.Settings
{
  /// <summary>
  ///   The section of the global settings for the BMEReader adapter device.
  /// </summary>
  public class BmeReaderOptions : SettingsSection
  {
    /// <summary>
    ///   Defines the default serial port name.
    /// </summary>
    public const string DefaultSerialPortName = "COM1";

    /// <summary>
    ///   Defines the default serial port operations timeout in milliseconds.
    /// </summary>
    public const int DefaultSerialPortTimeout = 1000;

    /// <summary>
    ///   Gets or sets the serial port name where the BMEReader device is connected.
    /// </summary>
    [Comment("Sets the serial port name where the BMEReader device is connected.")]
    public string SerialPortName { get; set; } = DefaultSerialPortName;

    /// <summary>
    ///   Gets or sets the serial port operations timeout in milliseconds.
    /// </summary>
    [Comment("Sets the serial port operations timeout in milliseconds.")]
    [Comment("Increase this value if timeout errors appear frequently.")]
    public int SerialPortTimeout { get; set; } = DefaultSerialPortTimeout;
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings object for the BMEReader adapter device.
    /// </summary>
    [Comment("The settings section related to the BMEReader adapter options.")]
    public BmeReaderOptions BmeReaderOptions { get; set; } = new();
  }
}
