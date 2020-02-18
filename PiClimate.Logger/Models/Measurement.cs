using System;
using System.Globalization;

namespace PiClimate.Logger.Models
{
  public class Measurement
  {
    public DateTime Timestamp { get; set; }

    public double Pressure { get; set; }

    public double Temperature { get; set; }

    public double Humidity { get; set; }

    public override string ToString() =>
      $"[{Timestamp.ToString(CultureInfo.InvariantCulture)}] P = {Pressure:F2} mmHg, T = {Temperature:F2} °C, H = {Humidity:F2}%";
  }
}
