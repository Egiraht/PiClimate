// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Logger.Settings
{
  /// <summary>
  ///   The section of the global settings for MySQL connections.
  /// </summary>
  public class MySqlOptions
  {
    /// <summary>
    ///   Defines the default database table name for data logging.
    /// </summary>
    public const string DefaultMeasurementsTableName = "Measurements";

    /// <summary>
    ///   Gets or sets a connection string key from the common <see cref="GlobalSettings.ConnectionStrings" />
    ///   dictionary to be used for MySQL connection.
    /// </summary>
    public string UseConnectionStringKey { get; set; } = GlobalSettings.DefaultConnectionStringKey;

    /// <summary>
    ///   Defines the database table name for the measurements.
    /// </summary>
    public string MeasurementsTableName { get; set; } = DefaultMeasurementsTableName;
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings for time period limiters.
    /// </summary>
    public MySqlOptions MySqlOptions { get; set; } = new();
  }
}
