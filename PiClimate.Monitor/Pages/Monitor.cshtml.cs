// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PiClimate.Monitor.Models;

namespace PiClimate.Monitor.Pages
{
  /// <summary>
  ///   The data monitor page code-behind class.
  /// </summary>
  [Authorize]
  public class Monitor : PageModel
  {
    /// <summary>
    ///   Defines the chart ID string.
    /// </summary>
    public const string ChartId = "measurements-chart";

    /// <summary>
    ///   Defines the climatic data source URI used for HTTP requests.
    /// </summary>
    public const string DataSourceRequestUri = nameof(Data);

    /// <summary>
    ///   Defines the HTTP method used for HTTP requests to <see cref="DataSourceRequestUri" />.
    /// </summary>
    public const string DataSourceRequestMethod = "POST";

    /// <summary>
    ///   The chart parameters used for the climatic data chart creation.
    /// </summary>
    public readonly ChartParameters ChartParameters = new ChartParameters(ChartId)
    {
      RequestUri = DataSourceRequestUri,
      RequestMethod = DataSourceRequestMethod
    };

    /// <summary>
    ///   The callback handler for GET HTTP requests.
    /// </summary>
    /// <param name="filter">
    ///   The measurement filter object composed from the HTTP request data.
    /// </param>
    /// <returns>
    ///   An HTTP response with the processed page contents.
    /// </returns>
    public IActionResult OnGet(MeasurementFilter filter)
    {
      ChartParameters.Filter = filter;
      return Page();
    }
  }
}
