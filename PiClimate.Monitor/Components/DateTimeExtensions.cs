// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;

namespace PiClimate.Monitor.Components
{
  /// <summary>
  ///   Enumeration of date-time truncation limits.
  /// </summary>
  public enum TruncationLimit : long
  {
    /// <summary>
    ///   Truncate to the whole number of milliseconds.
    /// </summary>
    Milliseconds = TimeSpan.TicksPerMillisecond,

    /// <summary>
    ///   Truncate to the whole number of seconds.
    /// </summary>
    Seconds = TimeSpan.TicksPerSecond,

    /// <summary>
    ///   Truncate to the whole number of minutes.
    /// </summary>
    Minutes = TimeSpan.TicksPerMinute,

    /// <summary>
    ///   Truncate to the whole number of hours.
    /// </summary>
    Hours = TimeSpan.TicksPerHour,

    /// <summary>
    ///   Truncate to the whole number of days.
    /// </summary>
    Days = TimeSpan.TicksPerDay
  }

  /// <summary>
  ///   A static class containing the extension methods for <see cref="DateTime" /> values.
  /// </summary>
  public static class DateTimeExtensions
  {
    /// <summary>
    ///   Truncates the provided date-time value to the specified limit.
    /// </summary>
    /// <param name="dateTime">
    ///   The date-time value to truncate.
    /// </param>
    /// <param name="limit">
    ///   The limit used for date-time truncation.
    /// </param>
    /// <returns>
    ///   The truncated date-time value.
    /// </returns>
    public static DateTime Truncate(this DateTime dateTime, TruncationLimit limit) =>
      new(dateTime.Ticks - dateTime.Ticks % (long) limit, dateTime.Kind);
  }
}
