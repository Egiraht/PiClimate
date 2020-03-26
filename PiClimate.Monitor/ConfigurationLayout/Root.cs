// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Monitor.ConfigurationLayout
{
  /// <summary>
  ///   The configuration's root section.
  /// </summary>
  public static class Root
  {
    /// <summary>
    ///   Defines the used measurement source.
    /// </summary>
    public const string UseMeasurementSource = nameof(UseMeasurementSource);

    /// <summary>
    ///   Defines the named list of connection strings providing the database connection parameters.
    /// </summary>
    public const string ConnectionStrings = nameof(ConnectionStrings);
  }
}
