using System;

namespace PiClimate.Monitor.Models
{
  public class MeasurementFilter
  {
    private int _resolution = DefaultResolution;

    public const int MinimalResolution = 1;

    public const int DefaultResolution = 1440;

    public static readonly TimeSpan DefaultTimePeriod = TimeSpan.FromDays(1);

    public DateTime FromTime { get; set; } = DateTime.Now - DefaultTimePeriod;

    public DateTime ToTime { get; set; } = DateTime.Now;

    public int Resolution
    {
      get => _resolution;
      set => _resolution = Math.Max(value, MinimalResolution);
    }
  }
}
