// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Text.Json.Serialization;
using PiClimate.Common.Components;

namespace PiClimate.Common.Models
{
  /// <summary>
  ///   A model class for measurement data filtering.
  /// </summary>
  public class MeasurementFilter
  {
    private int _timePeriod = DefaultTimePeriod;
    private DateTime? _fromTime;
    private int _resolution = DefaultResolution;

    /// <summary>
    ///   Defines the minimal data resolution within the selected timespan.
    /// </summary>
    public const int MinimalResolution = 1;

    /// <summary>
    ///   Defines the maximal data resolution within the selected timespan.
    /// </summary>
    public const int MaximalResolution = 3000;

    /// <summary>
    ///   Defines the default data resolution within the selected timespan.
    /// </summary>
    public const int DefaultResolution = 1500;

    /// <summary>
    ///   Defines the default time period in seconds to be used for the timespan.
    /// </summary>
    public const int DefaultTimePeriod = TimePeriods.Day;

    /// <summary>
    ///   Gets or sets the time period in seconds defining the beginning of the selected timespan relatively to
    ///   the <see cref="ToTime" /> property value.
    ///   This value is used only if the <see cref="FromTime" /> property is assigned to <c>null</c>.
    ///   The minimal value is 1 second.
    /// </summary>
    [JsonIgnore]
    public int TimePeriod
    {
      get => _timePeriod;
      set => _timePeriod = Math.Max(value, 1);
    }

    /// <summary>
    ///   Gets or sets the beginning of the selected timespan.
    ///   If the property is assigned to <c>null</c> the actual value will be calculated using the value of the
    ///   <see cref="TimePeriod" /> property. Otherwise the assigned value is used as the timespan beginning
    ///   date-time, and the <see cref="TimePeriod" /> value will be ignored.
    ///   On reading the value is never <c>null</c> so it can be safely cast to <see cref="DateTime" /> or accessed
    ///   using <c>FromTime!.Value</c> pattern.
    /// </summary>
    public DateTime? FromTime
    {
      get => _fromTime ?? ToTime - TimeSpan.FromSeconds(TimePeriod);
      set => _fromTime = value;
    }

    /// <summary>
    ///   Gets or sets the ending of the selected timespan.
    /// </summary>
    public DateTime ToTime { get; set; } = DateTime.Now;

    /// <summary>
    ///   Gets or sets the data resolution to be used within the selected timespan.
    ///   The actual number of filtered data entries may not be equal to the provided value.
    ///   The value must be in range between the <see cref="MinimalResolution" /> and <see cref="MaximalResolution" />
    ///   values.
    /// </summary>
    public int Resolution
    {
      get => _resolution;
      set => _resolution = Math.Clamp(value, MinimalResolution, MaximalResolution);
    }

    /// <summary>
    ///   Gets the time step in seconds to be used within the time period.
    /// </summary>
    [JsonIgnore]
    public int TimeStep => Math.Max((int) ((ToTime - FromTime!.Value).Duration().TotalSeconds / Resolution), 1);
  }
}
