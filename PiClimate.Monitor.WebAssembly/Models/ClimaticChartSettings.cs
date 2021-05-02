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

// ReSharper disable UnusedMember.Local
namespace PiClimate.Monitor.WebAssembly.Models
{
  /// <summary>
  ///   The model class containing the climatic chart settings.
  /// </summary>
  public class ClimaticChartSettings
  {
    /// <summary>
    ///   Defines the default <i>canvas</i> HTML element ID for climatic charts.
    /// </summary>
    public const string DefaultCanvasElementId = "climatic-chart";

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
    ///   Gets the ID string identifying the chart <i>canvas</i> element in the HTML markup. Must be unique on the page.
    /// </summary>
    public string CanvasElementId { get; set; } = DefaultCanvasElementId;

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
    /// <param name="periodStart">
    ///   The starting timestamp of the chart.
    /// </param>
    /// <param name="periodEnd">
    ///   The ending timestamp of the chart.
    /// </param>
    /// <param name="measurements">
    ///   The enumeration of measurements to be displayed on the chart.
    /// </param>
    /// <returns>
    ///   The configured <i>Chart.js</i> configuration object.
    /// </returns>
    public object CreateChartJsConfiguration(DateTime periodStart, DateTime periodEnd,
      IEnumerable<Measurement> measurements) =>
      new ChartJsConfigurationBuilder(this, periodStart, periodEnd, measurements).ChartJsConfiguration;

    /// <summary>
    ///   The <i>Chart.js</i> climatic chart configuration builder class.
    /// </summary>
    private class ChartJsConfigurationBuilder
    {
      private const string DateAxisId = "date";
      private const string PressureAxisId = "pressure";
      private const string TemperatureAxisId = "temperature";
      private const string HumidityAxisId = "humidity";

      private readonly ClimaticChartSettings _settings;
      private readonly DateTime _periodStart;
      private readonly DateTime _periodEnd;
      private readonly Measurement[] _measurements;
      private object? _chartJsConfiguration;

      /// <summary>
      ///   Gets the JSON-serializable mini-chart configuration object for <i>Chart.js</i>.
      /// </summary>
      public object ChartJsConfiguration => _chartJsConfiguration ??= new
      {
        Type = "scatter",
        Data = new
        {
          Datasets = new object[]
          {
            // Pressure data points.
            new
            {
              XAxisID = DateAxisId,
              YAxisID = PressureAxisId,
              Label = _settings.PressureChartLabel,
              BackgroundColor = _settings.PressureLineColor,
              BorderColor = _settings.PressureLineColor,
              ShowLine = true,
              Data = _measurements.Select(measurement => new
              {
                X = measurement.Timestamp.ToString("s"),
                Y = measurement.Pressure.As(_settings.PressureUnit)
              })
            },
            // Temperature data points.
            new
            {
              XAxisID = DateAxisId,
              YAxisID = TemperatureAxisId,
              Label = _settings.TemperatureChartLabel,
              BackgroundColor = _settings.TemperatureLineColor,
              BorderColor = _settings.TemperatureLineColor,
              ShowLine = true,
              Data = _measurements.Select(measurement => new
              {
                X = measurement.Timestamp.ToString("s"),
                Y = measurement.Temperature.As(_settings.TemperatureUnit)
              })
            },
            // Humidity data points.
            new
            {
              XAxisID = DateAxisId,
              YAxisID = HumidityAxisId,
              Label = _settings.HumidityChartLabel,
              BackgroundColor = _settings.HumidityLineColor,
              BorderColor = _settings.HumidityLineColor,
              ShowLine = true,
              Data = _measurements.Select(measurement => new
              {
                X = measurement.Timestamp.ToString("s"),
                Y = measurement.Humidity.As(_settings.HumidityUnit)
              })
            }
          }
        },
        Options = new
        {
          Scales = new
          {
            Date = new
            {
              Id = DateAxisId,
              Axis = "x",
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
              SuggestedMin = _periodStart,
              SuggestedMax = _periodEnd
            },
            Pressure = new
            {
              Id = PressureAxisId,
              Axis = "y",
              Type = "linear",
              Display = "auto",
              Position = "left",
              Title = new
              {
                Display = true,
                Color = _settings.PressureLineColor,
                Text = $"{_settings.PressureChartLabel}, {GetUnitAbbreviation(_settings.PressureUnit)}"
              },
              Grid = new
              {
                Display = true,
                BorderColor = _settings.PressureLineColor,
                BorderDash = new[] {2, 10},
                BorderDashOffset = 0,
                Color = _settings.PressureLineColor,
                LineWidth = 0.5
              },
              Ticks = new
              {
                Display = true,
                Color = _settings.PressureLineColor
              }
            },
            Temperature = new
            {
              Id = TemperatureAxisId,
              Axis = "y",
              Type = "linear",
              Display = "auto",
              Position = "right",
              Title = new
              {
                Display = true,
                Color = _settings.TemperatureLineColor,
                Text = $"{_settings.TemperatureChartLabel}, {GetUnitAbbreviation(_settings.TemperatureUnit)}"
              },
              Grid = new
              {
                Display = true,
                BorderColor = _settings.TemperatureLineColor,
                BorderDash = new[] {2, 10},
                BorderDashOffset = 4,
                Color = _settings.TemperatureLineColor,
                LineWidth = 0.5
              },
              Ticks = new
              {
                Display = true,
                Color = _settings.TemperatureLineColor
              }
            },
            Humidity = new
            {
              Id = HumidityAxisId,
              Axis = "y",
              Type = "linear",
              Display = "auto",
              Position = "right",
              Title = new
              {
                Display = true,
                Color = _settings.HumidityLineColor,
                Text = $"{_settings.HumidityChartLabel}, {GetUnitAbbreviation(_settings.HumidityUnit)}"
              },
              Grid = new
              {
                Display = true,
                BorderColor = _settings.HumidityLineColor,
                BorderDash = new[] {2, 10},
                BorderDashOffset = 8,
                Color = _settings.HumidityLineColor,
                LineWidth = 0.5
              },
              Ticks = new
              {
                Display = true,
                Color = _settings.HumidityLineColor
              }
            }
          },
          Elements = new
          {
            Point = new
            {
              Radius = 0.5,
              HitRadius = 3,
              HoverRadius = 0.5
            },
            Line = new
            {
              BorderWidth = 2,
              Tension = 0,
              Fill = false
            }
          },
          Plugins = new
          {
            Tooltip = new
            {
              Enabled = true,
              Mode = "index",
              Intersect = true,
              Position = "nearest",
              Callbacks = new { }
            }
          },
          Animation = false,
          Locale = _settings.CultureInfo.Name
        },
        Format = new
        {
          Locale = _settings.CultureInfo.Name,
          Units = new
          {
            PressureUnits = GetUnitAbbreviation(_settings.PressureUnit),
            TemperatureUnits = GetUnitAbbreviation(_settings.TemperatureUnit),
            HumidityUnits = GetUnitAbbreviation(_settings.HumidityUnit)
          }
        }
      };

      /// <summary>
      ///   Creates a new <i>Chart.js</i> configuration object.
      /// </summary>
      /// <param name="settings">
      ///   The chart settings object.
      /// </param>
      /// <param name="periodStart">
      ///   The chart period starting timestamp.
      /// </param>
      /// <param name="periodEnd">
      ///   The chart period ending timestamp.
      /// </param>
      /// <param name="measurements">
      ///   The enumeration of measurements to be displayed on the chart.
      /// </param>
      public ChartJsConfigurationBuilder(ClimaticChartSettings settings, DateTime periodStart, DateTime periodEnd,
        IEnumerable<Measurement> measurements)
      {
        _settings = settings;
        _measurements = measurements.ToArray();
        _periodStart = periodStart;
        _periodEnd = periodEnd;
      }

      /// <summary>
      ///   Gets the localized unit abbreviation string using the locale provided in chart settings.
      /// </summary>
      /// <typeparam name="TUnit">
      ///   The enumeration type containing units for a particular physical magnitude (e.g. <see cref="PressureUnit" />).
      /// </typeparam>
      /// <param name="unit">
      ///   The unit value within the <typeparamref name="TUnit" /> enumeration to get an abbreviation for.
      /// </param>
      /// <returns>
      ///   The unit abbreviation string.
      /// </returns>
      private string GetUnitAbbreviation<TUnit>(TUnit unit) where TUnit : Enum =>
        UnitAbbreviationsCache.Default.GetDefaultAbbreviation(unit, _settings.CultureInfo);
    }
  }
}
