// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System.Collections.Generic;
using PiClimate.Monitor.Sources;

namespace PiClimate.Monitor.Configuration
{
  /// <summary>
  ///   The global settings of the program.
  /// </summary>
  public partial class GlobalSettings
  {
    /// <summary>
    ///   Defines the default used connection string key.
    /// </summary>
    public const string DefaultConnectionStringKey = "Default";

    /// <summary>
    ///   Defines the default used connection string value.
    /// </summary>
    public const string DefaultConnectionStringValue = "";

    /// <summary>
    ///   Defines the default used connection string value.
    /// </summary>
    public const string DefaultProtectionKeysDirectoryPath = "./Keys";

    /// <summary>
    ///   Gets or sets the name of the used measurement source.
    /// </summary>
    public string UseMeasurementSource { get; set; } = nameof(MySqlSource);

    /// <summary>
    ///   Gets or sets the path to the directory where the data protection keys will be stored.
    /// </summary>
    public string ProtectionKeysDirectoryPath { get; set; } = DefaultProtectionKeysDirectoryPath;

    /// <summary>
    ///   Gets or sets the named list of connection strings providing the database connection parameters.
    /// </summary>
    public Dictionary<string, string> ConnectionStrings { get; set; } = new Dictionary<string, string>
    {
      {DefaultConnectionStringKey, DefaultConnectionStringValue}
    };
  }
}
