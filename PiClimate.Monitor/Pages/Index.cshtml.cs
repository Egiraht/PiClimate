using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PiClimate.Monitor.Components;
using PiClimate.Monitor.Models;

namespace PiClimate.Monitor.Pages
{
  /// <summary>
  ///   The index page code-behind class.
  /// </summary>
  public class Index : PageModel
  {
    /// <summary>
    ///   Defines the default time period to be set for the <see cref="Monitor" /> page after redirection.
    /// </summary>
    public const int DefaultTimePeriod = TimePeriods.Day;

    /// <summary>
    ///   The callback handler for GET HTTP requests.
    /// </summary>
    /// <returns>
    ///   An HTTP response redirecting to the <see cref="Monitor" /> page.
    /// </returns>
    public IActionResult OnGet() => RedirectToPage(nameof(Monitor),
      new Dictionary<string, object> {{nameof(MeasurementFilter.TimePeriod), DefaultTimePeriod}});
  }
}
