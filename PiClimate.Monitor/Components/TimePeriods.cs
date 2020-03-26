// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Monitor.Components
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
    public const int Quarter = 3 * Month;
    public const int Year = 365 * Day;
  }
}
