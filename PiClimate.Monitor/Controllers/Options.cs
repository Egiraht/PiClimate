// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using Microsoft.AspNetCore.Mvc;
using PiClimate.Common;
using PiClimate.Common.Settings;
using PiClimate.Monitor.Components;
using PiClimate.Monitor.Settings;

namespace PiClimate.Monitor.Controllers
{
  /// <summary>
  ///   The API controller class that provides the client-side options.
  /// </summary>
  [ApiController]
  public class Options : Controller
  {
    /// <summary>
    ///   The server's global settings object.
    /// </summary>
    private readonly GlobalSettings _globalSettings;

    /// <summary>
    ///   Initializes the API controller.
    /// </summary>
    /// <param name="globalSettings">
    ///   The server's global settings object.
    ///   Provided via dependency injection.
    /// </param>
    public Options(GlobalSettings globalSettings) => _globalSettings = globalSettings;

    /// <summary>
    ///   Provides the client-side options taken from the global settings.
    /// </summary>
    /// <returns>
    ///   The JSON-serialized client options object.
    /// </returns>
    [HttpGet(ApiEndpoints.OptionsEndpoint)]
    public IActionResult GetOptions() => new JsonResponse<ClientOptions>(_globalSettings.ClientOptions);
  }
}
