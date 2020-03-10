using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PiClimate.Monitor.Pages
{
  public class Index : PageModel
  {
    public IActionResult OnGet() => RedirectToPage(nameof(Monitor));
  }
}
