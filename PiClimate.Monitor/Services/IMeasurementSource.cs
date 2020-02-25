using System.Collections.Generic;
using System.Threading.Tasks;
using PiClimate.Monitor.Models;

namespace PiClimate.Monitor.Services
{
  public interface IMeasurementSource
  {
    IEnumerable<Measurement> GetMeasurements(MeasurementFilter filter);

    Task<IEnumerable<Measurement>> GetMeasurementsAsync(MeasurementFilter filter);
  }
}
