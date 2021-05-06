// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Collections.Generic;
using PiClimate.Common.Settings;
using PiClimate.Monitor.Sources;

namespace PiClimate.Monitor.Settings
{
  /// <summary>
  ///   The global settings of the program.
  /// </summary>
  public partial class GlobalSettings : SettingsSection
  {
    /// <summary>
    ///   Defines the default used connection string key.
    /// </summary>
    public const string DefaultConnectionStringKey = "Default";

    /// <summary>
    ///   Defines the default used connection string value.
    /// </summary>
    public const string DefaultConnectionStringValue =
      "Server=localhost; Port=3306; UserId=root; Password=; Database=PiClimate";

    /// <summary>
    ///   Defines the default used connection string value.
    /// </summary>
    public const string DefaultProtectionKeysDirectoryPath = "./Keys";

    /// <summary>
    ///   Gets or sets the name of the used measurement source.
    /// </summary>
    [Comment("Sets the used measurement source.")]
    [Comment("The value cannot be empty and must be one of these values: " +
      "'" + nameof(RandomDataSource) + "' (random data generation), " +
      "'" + nameof(MySqlSource) + "' (acquiring data from a MySQL database).")]
    public string UseMeasurementSource { get; set; } = nameof(RandomDataSource);

    /// <summary>
    ///   Gets or sets the path to the directory where the data protection keys will be stored.
    /// </summary>
    [Comment("Sets the path to the directory where the data protection keys will be stored.")]
    [Comment("The directory must be accessible for file reading and writing.")]
    public string ProtectionKeysDirectoryPath { get; set; } = DefaultProtectionKeysDirectoryPath;

    /// <summary>
    ///   Gets or sets the dictionary of connection strings providing the database connection parameters.
    /// </summary>
    [Comment("Defines a section of key-value pairs containing the connection strings.")]
    [Comment("Connection strings provide specific options for establishing database connections.")]
    public Dictionary<string, string> ConnectionStrings { get; set; } = new()
    {
      {DefaultConnectionStringKey, DefaultConnectionStringValue}
    };
  }
}
