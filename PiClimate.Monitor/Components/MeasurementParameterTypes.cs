using System.Reflection;
using System.Text.Json.Serialization;
using PiClimate.Monitor.Models;

namespace PiClimate.Monitor.Components
{
  /// <summary>
  ///   Defines the array of string literals that are allowed to be used as values for
  ///   the <see cref="ChartParameters.MeasurementParameter" /> property.
  ///   These values match the names of the <see cref="Measurement" /> class properties after they being serialized
  ///   to JSON format.
  /// </summary>
  public static class MeasurementParameterTypes
  {
    /// <summary>
    ///   The measurement parameter for pressure.
    /// </summary>
    public static string Pressure { get; } = typeof(Measurement)
      .GetProperty(nameof(Measurement.Pressure))
      ?.GetCustomAttribute<JsonPropertyNameAttribute>()
      ?.Name ?? nameof(Measurement.Pressure).ToLower();

    /// <summary>
    ///   The measurement parameter for temperature.
    /// </summary>
    public static string Temperature { get; } = typeof(Measurement)
      .GetProperty(nameof(Measurement.Temperature))
      ?.GetCustomAttribute<JsonPropertyNameAttribute>()
      ?.Name ?? nameof(Measurement.Temperature).ToLower();

    /// <summary>
    ///   The measurement parameter for humidity.
    /// </summary>
    public static string Humidity { get; } = typeof(Measurement)
      .GetProperty(nameof(Measurement.Humidity))
      ?.GetCustomAttribute<JsonPropertyNameAttribute>()
      ?.Name ?? nameof(Measurement.Humidity).ToLower();
  }
}
