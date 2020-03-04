using System;
using System.Text.Json.Serialization;

namespace PiClimate.Monitor.Models
{
  public class Measurement
  {
    [JsonPropertyName("d")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("p")]
    public double Pressure { get; set; }

    [JsonPropertyName("t")]
    public double Temperature { get; set; }

    [JsonPropertyName("h")]
    public double Humidity { get; set; }
  }
}
