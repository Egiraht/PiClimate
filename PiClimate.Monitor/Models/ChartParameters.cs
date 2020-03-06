namespace PiClimate.Monitor.Models
{
  /// <summary>
  ///   A view model class for the <i>Chart</i> partial view.
  /// </summary>
  public class ChartParameters
  {
    /// <summary>
    ///   Gets or sets the ID string identifying the chart in the HTML markup.
    /// </summary>
    public string ChartId { get; set; } = "";

    /// <summary>
    ///   Gets or sets the measurement filter assigned for the chart.
    /// </summary>
    public MeasurementFilter? Filter { get; set; }
  }
}
