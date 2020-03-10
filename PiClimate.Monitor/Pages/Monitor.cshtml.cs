using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PiClimate.Monitor.Models;

namespace PiClimate.Monitor.Pages
{
  public class Monitor : PageModel
  {
    public const string DataSourceRequestUri = nameof(Data);

    public const string DataSourceRequestMethod = "POST";

    public ChartParameters ChartParameters { get; private set; } = new ChartParameters();

    public IActionResult OnGet(MeasurementFilter filter, bool trimSpaces)
    {
      ChartParameters = new ChartParameters
      {
        TrimSpaces = trimSpaces,
        RequestUri = DataSourceRequestUri,
        RequestMethod = DataSourceRequestMethod,
        Filter = filter
      };

      return Page();
    }
  }
}
