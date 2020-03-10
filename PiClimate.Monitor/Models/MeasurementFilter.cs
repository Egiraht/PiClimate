using System;

namespace PiClimate.Monitor.Models
{
  public class MeasurementFilter
  {
    private int _resolution = DefaultResolution;

    /// <summary>
    ///   Defines the minimal data resolution within the selected timespan.
    /// </summary>
    public const int MinimalResolution = 1;

    /// <summary>
    ///   Defines the default data resolution within the selected timespan.
    /// </summary>
    public const int DefaultResolution = 1440;

    /// <summary>
    ///   Defines the default time period to be used for the timespan.
    /// </summary>
    public static readonly TimeSpan DefaultTimePeriod = TimeSpan.FromDays(1);

    /// <summary>
    ///   Gets or sets the beginning of the selected timespan.
    /// </summary>
    public DateTime FromTime { get; set; } = DateTime.Now - DefaultTimePeriod;

    /// <summary>
    ///   Gets or sets the ending of the selected timespan.
    /// </summary>
    public DateTime ToTime { get; set; } = DateTime.Now;

    /// <summary>
    ///   Gets or sets the data resolution to be used within the selected timespan.
    ///   The actual number of filtered data entries may not be equal to the provided value.
    /// </summary>
    public int Resolution
    {
      get => _resolution;
      set => _resolution = Math.Max(value, MinimalResolution);
    }
  }
}
