// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Collections.Generic;
using PiClimate.Common.Components;
using PiClimate.Monitor.Sources;

namespace PiClimate.Monitor.Settings
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
    public const string DefaultConnectionStringValue =
      "Server=localhost; Port=3306; UserId=root; Password=; Database=PiClimate";

    /// <summary>
    ///   Defines the default used connection string value.
    /// </summary>
    public const string DefaultProtectionKeysDirectoryPath = "./Keys";

    /// <summary>
    ///   Defines the default time period in seconds beyond which the latest data can be treated obsolete.
    /// </summary>
    public const int DefaultObsoleteDataPeriod = 10 * TimePeriods.Minute;

    /// <summary>
    ///   Gets or sets the name of the used measurement source.
    /// </summary>
    public string UseMeasurementSource { get; set; } = nameof(RandomDataSource);

    /// <summary>
    ///   Gets or sets the path to the directory where the data protection keys will be stored.
    /// </summary>
    public string ProtectionKeysDirectoryPath { get; set; } = DefaultProtectionKeysDirectoryPath;

    /// <summary>
    ///   Gets the time period in seconds from the latest logged measurement till the current moment beyond which
    ///   the data can be treated obsolete, so the corresponding warning will appear.
    /// </summary>
    public int ObsoleteDataPeriod { get; set; } = DefaultObsoleteDataPeriod;

    /// <summary>
    ///   Gets or sets the named list of connection strings providing the database connection parameters.
    /// </summary>
    public Dictionary<string, string> ConnectionStrings { get; set; } = new()
    {
      {DefaultConnectionStringKey, DefaultConnectionStringValue}
    };
  }
}
