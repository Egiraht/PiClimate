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
  [ApiController, Authorize]
  public class Data : Controller
  {
    /// <summary>
    ///   The used measurement source service.
    /// </summary>
    private readonly IMeasurementSource _source;

    /// <summary>
    ///   Initializes a new instance of the controller.
    /// </summary>
    /// <param name="source">
    ///   The used measurement source service.
    ///   Provided via dependency injection.
    /// </param>
    public Data(IMeasurementSource source) => _source = source;

    /// <summary>
    ///   Gets the filtered measurements data using the GET request.
    /// </summary>
    /// <param name="filter">
    ///   The measurement filter object.
    ///   Bound from the HTTP request query string.
    /// </param>
    /// <returns>
    ///   The JSON object containing the filtered measurements data.
    /// </returns>
    [HttpGet(ApiEndpoints.FilteredMeasurementsDataEndpoint)]
    public async Task<IActionResult> OnGetAsync([FromQuery] MeasurementFilter filter)
    {
      var measurements = await _source.GetMeasurementsAsync(filter);
      return new JsonResponse<IEnumerable<Measurement>>(measurements);
    }

    /// <summary>
    ///   Gets the filtered measurements data using the POST request.
    /// </summary>
    /// <param name="filter">
    ///   The measurement filter object.
    ///   Bound from the HTTP request body.
    /// </param>
    /// <returns>
    ///   The JSON object containing the filtered measurements data.
    /// </returns>
    [HttpPost(ApiEndpoints.FilteredMeasurementsDataEndpoint)]
    public async Task<IActionResult> OnPostAsync([FromBody] MeasurementFilter filter) =>
      await OnGetAsync(filter);

    /// <summary>
    ///   Gets the filtered measurements data using the GET request.
    /// </summary>
    /// <param name="request">
    ///   The latest data request object.
    ///   Bound from the HTTP request query string.
    /// </param>
    /// <returns>
    ///   The JSON object containing the latest measurements data.
    /// </returns>
    [HttpGet(ApiEndpoints.LatestMeasurementsDataEndpoint)]
    public async Task<IActionResult> OnGetLatestAsync([FromQuery] LatestDataRequest request)
    {
      var measurements = await _source.GetLatestMeasurementsAsync(request);
      return new JsonResponse<IEnumerable<Measurement>>(measurements);
    }

    /// <summary>
    ///   Gets the filtered measurements data using the POST request.
    /// </summary>
    /// <param name="request">
    ///   The latest data request object.
    ///   Bound from the HTTP request body.
    /// </param>
    /// <returns>
    ///   The JSON object containing the latest measurements data.
    /// </returns>
    [HttpPost(ApiEndpoints.LatestMeasurementsDataEndpoint)]
    public async Task<IActionResult> OnPostLatestAsync([FromBody] LatestDataRequest request) =>
      await OnGetLatestAsync(request);
  }
}
