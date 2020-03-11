using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PiClimate.Monitor.Models;

namespace PiClimate.Monitor.Pages
{
  /// <summary>
  ///   The data monitor page code-behind class.
  /// </summary>
  public class Monitor : PageModel
  {
    /// <summary>
    ///   Defines the climatic data source URI used for HTTP requests.
    /// </summary>
    public const string DataSourceRequestUri = nameof(Data);

    /// <summary>
    ///   Defines the HTTP method used for HTTP requests to <see cref="DataSourceRequestUri" />.
    /// </summary>
    public const string DataSourceRequestMethod = "POST";

    /// <summary>
    ///   The chart parameters used for the climatic data chart creation.
    /// </summary>
    public ChartParameters ChartParameters { get; private set; } = new ChartParameters();

    /// <summary>
    ///   The callback handler for GET HTTP requests.
    /// </summary>
    /// <param name="filter">
    ///   The measurement filter object composed from the HTTP request data.
    /// </param>
    /// <returns>
    ///   An HTTP response with the processed page contents.
    /// </returns>
    public IActionResult OnGet(MeasurementFilter filter)
    {
      ChartParameters = new ChartParameters
      {
        RequestUri = DataSourceRequestUri,
        RequestMethod = DataSourceRequestMethod,
        Filter = filter
      };

      return Page();
    }
  }
}
