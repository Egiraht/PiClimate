// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

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
    ///   An HTTP response redirecting to the <see cref="Latest" /> page.
    /// </returns>
    public IActionResult OnGet() => RedirectToPage(nameof(Latest));
  }
}
