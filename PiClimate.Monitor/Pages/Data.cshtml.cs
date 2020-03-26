// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PiClimate.Monitor.Models;
using PiClimate.Monitor.Sources;

namespace PiClimate.Monitor.Pages
{
  /// <summary>
  ///   The page that filters the measurement data using the provided measurement filter and returns the result
  ///   in JSON format.
  /// </summary>
  [IgnoreAntiforgeryToken]
  public class Data : PageModel
  {
    /// <summary>
    ///   The used measurement source service.
    /// </summary>
    private readonly IMeasurementSource _source;

    /// <summary>
    ///   Initializes the new instance of the page.
    /// </summary>
    /// <param name="source">
    ///   The used measurement source service.
    ///   Provided via dependency injection.
    /// </param>
    public Data(IMeasurementSource source)
    {
      _source = source;
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
    public async Task<JsonResult> OnGetAsync(MeasurementFilter filter)
    {
      if (filter == null)
        filter = new MeasurementFilter();

      try
      {
        var measurements = await _source.GetMeasurementsAsync(filter);
        return new JsonResult(new MeasurementsCollection(measurements));
      }
      catch (Exception e)
      {
        return new JsonResult(new {ErrorMessage = e.Message}) {StatusCode = StatusCodes.Status500InternalServerError};
      }
    }

    /// <inheritdoc cref="OnGetAsync" />
    public async Task<JsonResult> OnPostAsync([FromBody] MeasurementFilter filter) =>
      await OnGetAsync(filter);
  }
}
