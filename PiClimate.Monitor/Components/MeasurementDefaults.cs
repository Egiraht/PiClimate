// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Monitor.Components
{
  /// <summary>
  ///   The static class containing the measurement-related default constants.
  /// </summary>
  public static class MeasurementDefaults
  {
    /// <summary>
    ///   Defines the default measurement units for pressure.
    /// </summary>
    public const string DefaultPressureUnits = "mmHg";

    /// <summary>
    ///   Defines the default measurement units for temperature.
    /// </summary>
    public const string DefaultTemperatureUnits = "°C";

    /// <summary>
    ///   Defines the default measurement units for humidity.
    /// </summary>
    public const string DefaultHumidityUnits = "%";
  }
}
