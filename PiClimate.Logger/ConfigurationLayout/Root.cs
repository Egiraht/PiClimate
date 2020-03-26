// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Logger.ConfigurationLayout
{
  /// <summary>
  ///   The configuration's root section.
  /// </summary>
  public static class Root
  {
    /// <summary>
    ///   Defines the used measurement provider.
    /// </summary>
    public const string UseMeasurementProvider = nameof(UseMeasurementProvider);

    /// <summary>
    ///   Defines a comma-separated list of the used measurement loggers.
    /// </summary>
    public const string UseMeasurementLoggers = nameof(UseMeasurementLoggers);

    /// <summary>
    ///   Defines a comma-separated list of the used measurement limiters.
    /// </summary>
    public const string UseMeasurementLimiters = nameof(UseMeasurementLimiters);

    /// <summary>
    ///   Defines the measurement loop delay value in seconds.
    /// </summary>
    public const string MeasurementLoopDelay = nameof(MeasurementLoopDelay);

    /// <summary>
    ///   Defines the named list of connection strings providing the database connection parameters.
    /// </summary>
    public const string ConnectionStrings = nameof(ConnectionStrings);
  }
}
