namespace PiClimate.Monitor.Models
{
  /// <summary>
  ///   A view model class for the <i>Chart</i> partial view.
  /// </summary>
  public class ChartParameters
  {
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
    ///   Gets or sets the ID string identifying the chart in the HTML markup.
    /// </summary>
    public string ChartId { get; set; } = "measurements-chart";

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
    ///   Gets or sets the flag indicating whether empty spaces should be trimmed at the beginning and ending of the
    ///   selected timespan.
    /// </summary>
    public bool TrimSpaces { get; set; } = false;

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
