// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

// ReSharper disable UnusedMember.Local
namespace PiClimate.Monitor.WebAssembly.Models
{
  /// <summary>
  ///   The model class containing the mini-chart settings.
  /// </summary>
  public class MiniChartSettings
  {
    /// <summary>
    ///   Defines the default <i>canvas</i> HTML element ID for mini-charts.
    /// </summary>
    public const string DefaultCanvasElementId = "mini-chart";

    /// <summary>
    ///   Defines the default line color value.
    /// </summary>
    public const string DefaultLineColor = "gray";

    /// <summary>
    ///   Gets the ID string identifying the chart <i>canvas</i> element in the HTML markup. Must be unique on the page.
    /// </summary>
    public string CanvasElementId { get; set; } = DefaultCanvasElementId;

    /// <summary>
    ///   Gets or sets the chart's line color value.
    /// </summary>
    public string LineColor { get; set; } = DefaultLineColor;

    /// <summary>
    ///   Gets or sets the culture information object used for data formatting.
    /// </summary>
    public CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;

    /// <summary>
    ///   Creates a <i>Chart.js</i> configuration object that can be provided when creating a new chart instance.
    /// </summary>
    /// <param name="periodStart">
    ///   The starting timestamp of the chart.
    /// </param>
    /// <param name="periodEnd">
    ///   The ending timestamp of the chart.
    /// </param>
    /// <param name="dataPoints">
    ///   The enumeration of data points to be displayed on the mini-chart. Each data point represents a tuple
    ///   consisting of a timestamp and a corresponding data value.
    /// </param>
    /// <returns>
    ///   The configured <i>Chart.js</i> options object.
    /// </returns>
    public object CreateChartJsOptions(DateTime periodStart, DateTime periodEnd,
      IEnumerable<(DateTime Timestamp, double Value)> dataPoints) =>
      new ChartJsConfig(this, periodStart, periodEnd, dataPoints);

    /// <summary>
    ///   The JSON-serializable object containing the configuration for rendering a <i>Chart.js</i> chart.
    /// </summary>
    private class ChartJsConfig
    {
      private const string DateAxisId = "date";
      private const string ValuesAxisId = "values";

      private readonly MiniChartSettings _miniChartSettings;
      private readonly DateTime _periodStart;
      private readonly DateTime _periodEnd;
      private readonly (DateTime Timestamp, double Value)[] _dataPoints;

      /// <summary>
      ///   Gets the <i>Chart.js</i> chart type.
      /// </summary>
      public string Type => "scatter";

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
            YAxisID = ValuesAxisId,
            Label = string.Empty,
            BackgroundColor = _miniChartSettings.LineColor,
            BorderColor = _miniChartSettings.LineColor,
            ShowLine = true,
            Data = _dataPoints.Select(dataPoint => new
            {
              X = dataPoint.Timestamp.ToString("s"),
              Y = dataPoint.Value
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
          Date = new
          {
            Id = DateAxisId,
            Axis = "x",
            Type = "time",
            Display = false,
            SuggestedMin = _periodStart.ToString("s"),
            SuggestedMax = _periodEnd.ToString("s"),
            Ticks = new
            {
              Display = false
            }
          },

          Values = new
          {
            Id = ValuesAxisId,
            Axis = "y",
            Type = "linear",
            Display = false,
            ScaleLabel = new
            {
              Display = false
            },
            GridLines = new
            {
              Display = false
            },
            Ticks = new
            {
              Display = false
            }
          }
        },

        Elements = new
        {
          Point = new
          {
            Radius = 0.5,
            HitRadius = 0,
            HoverRadius = 0.5
          },
          Line = new
          {
            BorderWidth = 1,
            Tension = 0,
            Fill = false
          }
        },

        Plugins = new
        {
          Legend = new
          {
            Display = false
          },

          Tooltip = new
          {
            Enabled = false
          }
        },

        Animation = false,
        Locale = _miniChartSettings.CultureInfo.Name
      };

      /// <summary>
      ///   Creates a new <i>Chart.js</i> configuration object.
      /// </summary>
      /// <param name="miniChartSettings">
      ///   The mini-chart settings object.
      /// </param>
      /// <param name="periodStart">
      ///   The chart period starting timestamp.
      /// </param>
      /// <param name="periodEnd">
      ///   The chart period ending timestamp.
      /// </param>
      /// <param name="dataPoints">
      ///   The enumeration of data points to be displayed on the mini-chart.
      /// </param>
      public ChartJsConfig(MiniChartSettings miniChartSettings, DateTime periodStart, DateTime periodEnd,
        IEnumerable<(DateTime Timestamp, double Value)> dataPoints)
      {
        _miniChartSettings = miniChartSettings;
        _dataPoints = dataPoints.ToArray();
        _periodStart = periodStart;
        _periodEnd = periodEnd;
      }
    }
  }
}
