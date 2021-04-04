// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using Microsoft.AspNetCore.Mvc;
using PiClimate.Common;
using PiClimate.Monitor.Components;

namespace PiClimate.Monitor.Controllers
{
  /// <summary>
  ///   The status code API controller class.
  /// </summary>
  [ApiController]
  public class Status : Controller
  {
    /// <summary>
    ///   Returns a JSON response with the provided status code using GET and POST requests.
    /// </summary>
    /// <param name="status">
    ///   The HTTP status code.
    ///   Provided via the route parameter.
    /// </param>
    /// <returns>
    ///   A JSON response object.
    /// </returns>
    [AcceptVerbs("GET", "POST", Route = ApiEndpoints.StatusEndpoint + "/{status:int}")]
    public IActionResult OnGetStatus([FromRoute] int status)
    {
      HttpContext.Response.StatusCode = status;
      return DefaultRequestHandlers.StatusCodeHandler(HttpContext);
    }
  }
}
