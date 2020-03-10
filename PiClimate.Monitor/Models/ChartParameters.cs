using System;
using System.Linq;
using System.Reflection;
using PiClimate.Monitor.Components;

namespace PiClimate.Monitor.Models
{
  /// <summary>
  ///   A view model class for the <i>Chart</i> partial view.
  /// </summary>
  public class ChartParameters
  {
    private string _measurementParameter = MeasurementParameterTypes.Pressure;

    /// <summary>
    ///   Defines the default line color value.
    /// </summary>
    public const string DefaultLineColor = "blue";

    /// <summary>
    ///   Gets or sets the ID string identifying the chart in the HTML markup.
    /// </summary>
    public string ChartId { get; set; } = $"{nameof(Measurement.Pressure).ToLower()}-chart";

    /// <summary>
    ///   Gets or sets the chart's label string.
    /// </summary>
    public string ChartLabel { get; set; } = nameof(Measurement.Pressure);

    /// <summary>
    ///   Gets or sets the measurement parameter name used as a data source for the chart.
    ///   The value must be the one of the constants defined in <see cref="MeasurementParameterTypes" /> class.
    /// </summary>
    public string MeasurementParameter
    {
      get => _measurementParameter;
      set
      {
        var properties = typeof(MeasurementParameterTypes)
          .GetProperties(BindingFlags.Public | BindingFlags.Static)
          .Where(property => property.PropertyType == typeof(string) && property.GetValue(null) != null)
          .Select(property => (string) property.GetValue(null)!);

        _measurementParameter = properties.Contains(value, StringComparer.InvariantCultureIgnoreCase)
          ? value
          : MeasurementParameterTypes.Pressure;
      }
    }

    /// <summary>
    ///   Gets or sets the chart's line color value.
    /// </summary>
    public string LineColor { get; set; } = DefaultLineColor;

    /// <summary>
    ///   Gets or sets the URI string where the HTTP request should be made.
    /// </summary>
    public string RequestUri { get; set; } = "";

    /// <summary>
    ///   Gets or sets the HTTP method for the request.
    /// </summary>
    public string RequestMethod { get; set; } = "POST";

    /// <summary>
    ///   Gets or sets the measurement filter assigned for the chart.
    /// </summary>
    public MeasurementFilter? Filter { get; set; }
  }
}
