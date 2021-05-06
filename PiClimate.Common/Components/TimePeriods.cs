// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using PiClimate.Common.Localization;

namespace PiClimate.Common.Components
{
  /// <summary>
  ///   A static class containing the common time periods expressed in seconds.
  /// </summary>
  public static class TimePeriods
  {
    public const int Immediate = 0;
    public const int Second = 1;
    public const int Minute = 60;
    public const int Hour = 60 * Minute;
    public const int Day = 24 * Hour;
    public const int Week = 7 * Day;
    public const int Month = 30 * Day;
    public const int Year = 365 * Day;

    /// <summary>
    ///   Gets the string describing the time period in a human-readable form.
    /// </summary>
    /// <param name="periodSeconds">
    ///   The total number of seconds defining the period to get an explanation string for.
    /// </param>
    /// <param name="formatProvider">
    ///   The optional format provider object.
    /// </param>
    /// <returns>
    ///   A string containing a number of minutes/hours/days describing the specified time scale period.
    /// </returns>
    /// <remarks>
    ///   This method does not provide calendar-precise descriptions as time period calculations are performed using
    ///   constant time periods, e.g. a year is always 365 days.
    /// </remarks>
    public static string GetTimePeriodString(int periodSeconds, IFormatProvider? formatProvider = null)
    {
      periodSeconds = Math.Abs(periodSeconds);
      return periodSeconds switch
      {
        < Minute => Strings.TimePeriods_SecondOrSecondsF.Format(formatProvider,
          periodSeconds.ToString("0.#", formatProvider)),
        < Hour => Strings.TimePeriods_MinuteOrMinutesF.Format(formatProvider,
          (periodSeconds / Minute).ToString("0.#", formatProvider)),
        < Day => Strings.TimePeriods_HourOrHoursF.Format(formatProvider,
          (periodSeconds / Hour).ToString("0.#", formatProvider)),
        < Week => Strings.TimePeriods_DayOrDaysF.Format(formatProvider,
          (periodSeconds / Day).ToString("0.#", formatProvider)),
        < Month => Strings.TimePeriods_WeekOrWeeksF.Format(formatProvider,
          (periodSeconds / Week).ToString("0.#", formatProvider)),
        < Year => Strings.TimePeriods_MonthOrMonthsF.Format(formatProvider,
          (periodSeconds / Month).ToString("0.#", formatProvider)),
        _ => Strings.TimePeriods_YearOrYearsF.Format(formatProvider,
          (periodSeconds / Year).ToString("0.#", formatProvider))
      };
    }

    /// <summary>
    ///   Gets the string describing the time period in a human-readable form.
    /// </summary>
    /// <param name="timeSpan">
    ///   The <see cref="TimeSpan" /> value defining the period to get an explanation string for.
    /// </param>
    /// <param name="formatProvider">
    ///   The optional format provider object.
    /// </param>
    /// <returns>
    ///   A string containing a number of minutes/hours/days describing the specified time scale period.
    /// </returns>
    public static string GetTimePeriodString(TimeSpan timeSpan, IFormatProvider? formatProvider = null) =>
      GetTimePeriodString((int) timeSpan.Duration().TotalSeconds, formatProvider);
  }
}
