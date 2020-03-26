// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Monitor.ConfigurationLayout
{
  /// <summary>
  ///   The configuration's section related to MySQL options.
  /// </summary>
  public static class MySqlOptions
  {
    /// <summary>
    ///   Defines a connection string key from the root <see cref="Root.ConnectionStrings" /> section to be used
    ///   for MySQL connection.
    /// </summary>
    public const string UseConnectionStringKey =
      nameof(MySqlOptions) + ":" + nameof(UseConnectionStringKey);

    /// <summary>
    ///   Defines the database table where the measurement data are stored.
    /// </summary>
    public const string MeasurementsTableName =
      nameof(MySqlOptions) + ":" + nameof(MeasurementsTableName);
  }
}
