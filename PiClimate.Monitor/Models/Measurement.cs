using System;

namespace PiClimate.Monitor.Models
{
  public class Measurement
  {
    public DateTime Timestamp { get; set; }

    public double Pressure { get; set; }

    public double Temperature { get; set; }

    public double Humidity { get; set; }
  }
}
