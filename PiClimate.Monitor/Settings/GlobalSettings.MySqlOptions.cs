// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using PiClimate.Common.Settings;

namespace PiClimate.Monitor.Settings
{
  /// <summary>
  ///   The section of the global settings for MySQL connections.
  /// </summary>
  public class MySqlOptions : SettingsSection
  {
    /// <summary>
    ///   Defines the default database table name for data logging.
    /// </summary>
    public const string DefaultMeasurementsTableName = "Measurements";

    /// <summary>
    ///   Gets or sets a connection string key from the common <see cref="GlobalSettings.ConnectionStrings" />
    ///   dictionary to be used for MySQL connection.
    /// </summary>
    [Comment("Selects the connection string key from the root '" + nameof(GlobalSettings.ConnectionStrings) + "' " +
      "section that will be used for MySQL connection.")]
    public string UseConnectionStringKey { get; set; } = GlobalSettings.DefaultConnectionStringKey;

    /// <summary>
    ///   Gets or sets the database table name for the measurements.
    /// </summary>
    [Comment("Sets the table name in the database where climatic data are stored.")]
    public string MeasurementsTableName { get; set; } = DefaultMeasurementsTableName;
  }

  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings object for connecting MySQL databases.
    /// </summary>
    [Comment("The settings section related to the MySQL database connection options.")]
    public MySqlOptions MySqlOptions { get; set; } = new();
  }
}
