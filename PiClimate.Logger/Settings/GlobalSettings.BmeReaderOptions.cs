// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Logger.Settings
{
  /// <summary>
  ///   The section of the global settings for the BMEReader adapter device.
  /// </summary>
  public class BmeReaderOptions
  {
    /// <summary>
    ///   Defines the default serial port name.
    /// </summary>
    public const string DefaultSerialPortName = "COM1";

    /// <summary>
    ///   Gets or sets the serial port name where the BMEReader device is connected.
    /// </summary>
    public string SerialPortName { get; set; } = DefaultSerialPortName;
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings for the BMEReader adapter device.
    /// </summary>
    public BmeReaderOptions BmeReaderOptions { get; set; } = new();
  }
}
