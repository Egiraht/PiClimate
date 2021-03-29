// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Text.Json.Serialization;

namespace PiClimate.Common.Models
{
  /// <summary>
  ///   A model class for measurement data filtering.
  /// </summary>
  public class MeasurementFilter
  {
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
    ///   Defines the default time period timespan.
    /// </summary>
    public static readonly TimeSpan DefaultTimePeriod = TimeSpan.FromDays(1);

    /// <summary>
    ///   The backing field for the <see cref="Resolution" /> property.
    /// </summary>
    private int _resolution = DefaultResolution;

    /// <summary>
    ///   Gets or sets the period starting timestamp.
    /// </summary>
    public DateTime PeriodStart { get; set; } = DateTime.Now - DefaultTimePeriod;

    /// <summary>
    ///   Gets or sets the period ending timestamp.
    /// </summary>
    public DateTime PeriodEnd { get; set; } = DateTime.Now;

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
    ///   Gets the time step duration expressed in seconds according to the current period timestamps and resolution.
    /// </summary>
    [JsonIgnore]
    public int TimeStep => Math.Max((int) ((PeriodEnd - PeriodStart).Duration().TotalSeconds / Resolution), 1);
  }
}
