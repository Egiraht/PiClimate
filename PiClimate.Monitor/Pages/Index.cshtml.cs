using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PiClimate.Monitor.Pages
{
  /// <summary>
  ///   The index page code-behind class.
  /// </summary>
  public class Index : PageModel
  {
    /// <summary>
    ///   The callback handler for GET HTTP requests.
    /// </summary>
    /// <returns>
    ///   An HTTP response redirecting to the <see cref="Monitor" /> page.
    /// </returns>
    public IActionResult OnGet() => RedirectToPage(nameof(Monitor));
  }
}
