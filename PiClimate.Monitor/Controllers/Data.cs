// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiClimate.Common;
using PiClimate.Common.Models;
using PiClimate.Monitor.Components;
using PiClimate.Monitor.Sources;

namespace PiClimate.Monitor.Controllers
{
  /// <summary>
  ///   An API controller that filters the measurement data using the provided measurement filter and returns the result
  ///   in JSON format.
  /// </summary>
  [ApiController, Authorize, Route(ApiEndpoints.MeasurementDataEndpoint)]
  public class Data : Controller
  {
    /// <summary>
    ///   The used measurement source service.
    /// </summary>
    private IMeasurementSource Source { get; }

    /// <summary>
    ///   Initializes a new instance of the controller.
    /// </summary>
    /// <param name="source">
    ///   The used measurement source service.
    ///   Provided via dependency injection.
    /// </param>
    public Data(IMeasurementSource source)
    {
      Source = source;
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
    [HttpGet]
    public async Task<IActionResult> OnGetAsync([FromQuery] MeasurementFilter filter)
    {
      var measurements = await Source.GetMeasurementsAsync(filter);
      return new JsonResponse<IEnumerable<Measurement>>(measurements);
    }

    /// <inheritdoc cref="OnGetAsync" />
    [HttpPost]
    public async Task<IActionResult> OnPostAsync([FromBody] MeasurementFilter filter) =>
      await OnGetAsync(filter);

    /// <summary>
    ///   Gets the latest logged measurements and returns them in JSON format.
    /// </summary>
    /// <param name="request">
    ///   The latest data request object.
    ///   Provided via the request parameters binding.
    /// </param>
    /// <returns>
    ///   The JSON-encoded HTTP response containing the filtered measurement data.
    /// </returns>
    [HttpGet("Latest")]
    public async Task<IActionResult> OnGetLatestAsync([FromQuery] LatestDataRequest request)
    {
      var measurements = await Source.GetLatestMeasurementsAsync(request);
      return new JsonResponse<IEnumerable<Measurement>>(measurements);
    }

    /// <inheritdoc cref="OnGetLatestAsync" />
    [HttpPost("Latest")]
    public async Task<IActionResult> OnPostLatestAsync([FromBody] LatestDataRequest request) =>
      await OnGetLatestAsync(request);
  }
}
