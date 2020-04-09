// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PiClimate.Monitor.Pages
{
  [IgnoreAntiforgeryToken]
  public class Status : PageModel
  {
    [BindProperty(SupportsGet = true)]
    public int Code { get; set; }
  }
}
