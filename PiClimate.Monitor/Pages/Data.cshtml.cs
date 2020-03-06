using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PiClimate.Monitor.Components;
using PiClimate.Monitor.Models;
using PiClimate.Monitor.Sources;

namespace PiClimate.Monitor.Pages
{
  /// <summary>
  ///   The page that filters the measurement data using the provided measurement filter and returns the result
  ///   in JSON format.
  /// </summary>
  [IgnoreAntiforgeryToken]
  public class Data : PageModel
  {
    /// <summary>
    ///   The used measurement source service.
    /// </summary>
    private readonly IMeasurementSource _source;

    /// <summary>
    ///   Initializes the new instance of the page.
    /// </summary>
    /// <param name="source">
    ///   The used measurement source service.
    ///   Provided via dependency injection.
    /// </param>
    public Data(IMeasurementSource source)
    {
      _source = source;
    }

    /// <summary>
    ///   Filters the measurement data using the provided measurement filter and returns the data
    ///   in JSON format.
    /// </summary>
    /// <param name="filter">
    ///   The measurement filter used for measurement data filtering.
    ///   Provided via the request parameters binding.
    /// </param>
    /// <returns>
    ///   The JSON-encoded HTTP response containing the filtered measurement data.
    /// </returns>
    public async Task<MeasurementsResult> OnGetAsync(MeasurementFilter filter)
    {
      if (filter == null)
        filter = new MeasurementFilter();

      var measurements = await _source.GetMeasurementsAsync(filter);
      return new MeasurementsResult(filter, measurements);
    }

    /// <inheritdoc cref="OnGetAsync" />
    public async Task<MeasurementsResult> OnPostAsync([FromBody] MeasurementFilter filter) =>
      await OnGetAsync(filter);
  }
}
