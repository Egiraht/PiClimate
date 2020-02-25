using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PiClimate.Monitor.Models;

namespace PiClimate.Monitor.Components
{
  public class MeasurementsResult : JsonResult
  {
    public MeasurementsResult(MeasurementFilter filter, IEnumerable<Measurement> measurements,
      object? serializerSettings = null) : base(GetJsonValue(filter, measurements), serializerSettings)
    {
    }

    private static object GetJsonValue(MeasurementFilter filter, IEnumerable<Measurement> measurements) => new
    {
      FromTime = filter.FromTime,
      ToTime = filter.ToTime,
      Resolution = filter.Resolution,
      Measurements = measurements
    };
  }
}
