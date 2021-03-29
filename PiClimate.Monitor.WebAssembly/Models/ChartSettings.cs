// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PiClimate.Common.Models;
using UnitsNet;
using UnitsNet.Units;

namespace PiClimate.Monitor.WebAssembly.Models
{
  public class ChartSettings
  {
    /// <summary>
    ///   Defines the default chart HTML element ID.
    /// </summary>
    public const string DefaultChartElementId = "chart";

    /// <summary>
    ///   Defines the default line color value for pressure.
    /// </summary>
    public const string DefaultPressureLineColor = "blue";

    /// <summary>
    ///   Defines the default line color value for temperature.
    /// </summary>
    public const string DefaultTemperatureLineColor = "green";

    /// <summary>
    ///   Defines the default line color value for humidity.
    /// </summary>
    public const string DefaultHumidityLineColor = "red";

    /// <summary>
    ///   Gets the ID string identifying the chart in the HTML markup.
    /// </summary>
    public string ChartId { get; set; } = DefaultChartElementId;

    /// <summary>
    ///   Gets or sets the chart's label string for pressure.
    /// </summary>
    public string PressureChartLabel { get; set; } = nameof(Measurement.Pressure);

    /// <summary>
    ///   Gets or sets the chart's label string for temperature.
    /// </summary>
    public string TemperatureChartLabel { get; set; } = nameof(Measurement.Temperature);

    /// <summary>
    ///   Gets or sets the chart's label string for humidity.
    /// </summary>
    public string HumidityChartLabel { get; set; } = nameof(Measurement.Humidity);

    /// <summary>
    ///   Gets or sets the string of units the pressure is expressed in.
    /// </summary>
    public PressureUnit PressureUnit { get; set; } = PressureUnit.MillimeterOfMercury;

    /// <summary>
    ///   Gets or sets the string of units the temperature is expressed in.
    /// </summary>
    public TemperatureUnit TemperatureUnit { get; set; } = TemperatureUnit.DegreeCelsius;

    /// <summary>
    ///   Gets or sets the string of units the humidity is expressed in.
    /// </summary>
    public RelativeHumidityUnit HumidityUnit { get; set; } = RelativeHumidityUnit.Percent;

    /// <summary>
    ///   Gets or sets the chart's line color value for pressure.
    /// </summary>
    public string PressureLineColor { get; set; } = DefaultPressureLineColor;

    /// <summary>
    ///   Gets or sets the chart's line color value for temperature.
    /// </summary>
    public string TemperatureLineColor { get; set; } = DefaultTemperatureLineColor;

    /// <summary>
    ///   Gets or sets the chart's line color value for humidity.
    /// </summary>
    public string HumidityLineColor { get; set; } = DefaultHumidityLineColor;

    /// <summary>
    ///   Gets or sets the culture information object used for data formatting.
    /// </summary>
    public CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;

    /// <summary>
    ///   Creates a <i>Chart.js</i> configuration object that can be provided when creating a new chart instance.
    /// </summary>
    /// <param name="measurements">
    ///   The enumeration of measurements to be displayed on the chart.
    /// </param>
    /// <param name="periodStart">
    ///   The starting timestamp of the chart.
    /// </param>
    /// <param name="periodEnd">
    ///   The ending timestamp of the chart.
    /// </param>
    /// <returns>
    ///   The configured <i>Chart.js</i> options object.
    /// </returns>
    public object CreateChartJsOptions(DateTime periodStart, DateTime periodEnd,
      IEnumerable<Measurement> measurements) =>
      new ChartJsConfig(this, periodStart, periodEnd, measurements);

    /// <summary>
    ///   The JSON-serializable object containing the configuration for rendering a <i>Chart.js</i> chart.
    /// </summary>
    // ReSharper disable UnusedMember.Local
    private class ChartJsConfig
    {
      private const string DateAxisId = "date";
      private const string PressureAxisId = "pressure";
      private const string TemperatureAxisId = "temperature";
      private const string HumidityAxisId = "humidity";

      private readonly ChartSettings _chartSettings;
      private readonly DateTime _periodStart;
      private readonly DateTime _periodEnd;
      private readonly Measurement[] _measurements;

      /// <summary>
      ///   Defines the <i>Chart.js</i> chart type.
      /// </summary>
      public string Type { get; } = "scatter";

      /// <summary>
      ///   Gets the <c>data</c> section of the <i>Chart.js</i> configuration.
      /// </summary>
      public object Data => new
      {
        Datasets = new object[]
        {
          new
          {
            XAxisID = DateAxisId,
            YAxisID = PressureAxisId,
            Label = _chartSettings.PressureChartLabel,
            BackgroundColor = _chartSettings.PressureLineColor,
            BorderColor = _chartSettings.PressureLineColor,
            ShowLine = true,
            Data = _measurements.Select(measurement => new
            {
              X = measurement.Timestamp.ToString("s"),
              Y = measurement.Pressure.As(_chartSettings.PressureUnit)
            })
          },
          new
          {
            XAxisID = DateAxisId,
            YAxisID = TemperatureAxisId,
            Label = _chartSettings.TemperatureChartLabel,
            BackgroundColor = _chartSettings.TemperatureLineColor,
            BorderColor = _chartSettings.TemperatureLineColor,
            ShowLine = true,
            Data = _measurements.Select(measurement => new
            {
              X = measurement.Timestamp.ToString("s"),
              Y = measurement.Temperature.As(_chartSettings.TemperatureUnit)
            })
          },
          new
          {
            XAxisID = DateAxisId,
            YAxisID = HumidityAxisId,
            Label = _chartSettings.HumidityChartLabel,
            BackgroundColor = _chartSettings.HumidityLineColor,
            BorderColor = _chartSettings.HumidityLineColor,
            ShowLine = true,
            Data = _measurements.Select(measurement => new
            {
              X = measurement.Timestamp.ToString("s"),
              Y = measurement.Humidity.As(_chartSettings.HumidityUnit)
            })
          }
        }
      };

      /// <summary>
      ///   Gets the <c>options</c> section of the <i>Chart.js</i> configuration.
      /// </summary>
      public object Options => new
      {
        Scales = new
        {
          XAxes = new[]
          {
            new
            {
              Id = DateAxisId,
              Type = "time",
              Display = true,
              Time = new
              {
                IsoWeekday = true,
                MinUnit = "second",
                DisplayFormats = new
                {
                  Second = "LTS",
                  Minute = "LT",
                  Hour = "LT",
                  Day = "L",
                  Month = "L"
                }
              },
              Ticks = new
              {
                Min = GetMinTimestamp().ToString("s"),
                Max = GetMaxTimestamp().ToString("s")
              }
            }
          },

          YAxes = new[]
          {
            new
            {
              Id = PressureAxisId,
              Type = "linear",
              Display = "auto",
              Position = "left",
              ScaleLabel = new
              {
                Display = true,
                LabelString =
                  $"{_chartSettings.PressureChartLabel}, {Pressure.GetAbbreviation(_chartSettings.PressureUnit, _chartSettings.CultureInfo)}",
                FontColor = _chartSettings.PressureLineColor
              },
              GridLines = new
              {
                Color = _chartSettings.PressureLineColor,
                LineWidth = 0.5
              },
              Ticks = new
              {
                FontColor = _chartSettings.PressureLineColor
              }
            },
            new
            {
              Id = TemperatureAxisId,
              Type = "linear",
              Display = "auto",
              Position = "right",
              ScaleLabel = new
              {
                Display = true,
                LabelString =
                  $"{_chartSettings.TemperatureChartLabel}, {Temperature.GetAbbreviation(_chartSettings.TemperatureUnit, _chartSettings.CultureInfo)}",
                FontColor = _chartSettings.TemperatureLineColor
              },
              GridLines = new
              {
                Color = _chartSettings.TemperatureLineColor,
                LineWidth = 0.5
              },
              Ticks = new
              {
                FontColor = _chartSettings.TemperatureLineColor
              }
            },
            new
            {
              Id = HumidityAxisId,
              Type = "linear",
              Display = "auto",
              Position = "right",
              ScaleLabel = new
              {
                Display = true,
                LabelString =
                  $"{_chartSettings.HumidityChartLabel}, {RelativeHumidity.GetAbbreviation(_chartSettings.HumidityUnit, _chartSettings.CultureInfo)}",
                FontColor = _chartSettings.HumidityLineColor
              },
              GridLines = new
              {
                Color = _chartSettings.HumidityLineColor,
                LineWidth = 0.5
              },
              Ticks = new
              {
                FontColor = _chartSettings.HumidityLineColor
              }
            }
          }
        },

        Elements = new
        {
          Point = new
          {
            Radius = 0.5,
            HitRadius = 5,
            HoverRadius = 5
          },
          Line = new
          {
            BorderWidth = 2,
            Tension = 0,
            Fill = false
          }
        },

        Tooltips = new
        {
          Mode = "index",
          Intersect = true,
          Position = "nearest",
          Callbacks = new { }
        },

        Animation = new
        {
          Duration = 0
        },

        Hover = new
        {
          AnimationDuration = 0
        },

        ResponsiveAnimationDuration = 0,
        Locale = _chartSettings.CultureInfo.Name
      };

      /// <summary>
      ///   Gets the additional non-standard <c>format</c> section of the configuration used for chart customization.
      /// </summary>
      public object Format => new
      {
        Locale = _chartSettings.CultureInfo.Name,
        Units = new
        {
          PressureUnits = Pressure.GetAbbreviation(_chartSettings.PressureUnit, _chartSettings.CultureInfo),
          TemperatureUnits = Temperature.GetAbbreviation(_chartSettings.TemperatureUnit, _chartSettings.CultureInfo),
          HumidityUnits = RelativeHumidity.GetAbbreviation(_chartSettings.HumidityUnit, _chartSettings.CultureInfo)
        }
      };

      /// <summary>
      ///   Creates a new <i>Chart.js</i> configuration object.
      /// </summary>
      /// <param name="chartSettings">
      ///   The chart settings object.
      /// </param>
      /// <param name="periodStart">
      ///   The chart period starting timestamp.
      /// </param>
      /// <param name="periodEnd">
      ///   The chart period ending timestamp.
      /// </param>
      /// <param name="measurements">
      ///   The enumeration of measurements that will be displayed on the chart.
      ///   Used for the chart's time axis bounds calculation only.
      /// </param>
      public ChartJsConfig(ChartSettings chartSettings, DateTime periodStart, DateTime periodEnd,
        IEnumerable<Measurement> measurements)
      {
        _chartSettings = chartSettings;
        _measurements = measurements.ToArray();
        _periodStart = periodStart;
        _periodEnd = periodEnd;
      }

      /// <summary>
      ///   Gets the minimal timestamp for the chart's time axis.
      /// </summary>
      /// <returns>
      ///   A <see cref="DateTime" /> representing the minimal timestamp.
      /// </returns>
      private DateTime GetMinTimestamp()
      {
        if (!_measurements.Any())
          return _periodStart;

        var minTimestamp = _measurements.Min(measurement => measurement.Timestamp);
        return _periodStart < minTimestamp ? _periodStart : minTimestamp;
      }

      /// <summary>
      ///   Gets the maximal timestamp for the chart's time axis.
      /// </summary>
      /// <returns>
      ///   A <see cref="DateTime" /> representing the maximal timestamp.
      /// </returns>
      private DateTime GetMaxTimestamp()
      {
        if (!_measurements.Any())
          return _periodEnd;

        var maxTimestamp = _measurements.Max(measurement => measurement.Timestamp);
        return _periodEnd > maxTimestamp ? _periodEnd : maxTimestamp;
      }
    }
  }
}
