// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PiClimate.Monitor.Components
{
  /// <summary>
  ///   A static class containing the default action handlers for erroneous requests.
  /// </summary>
  public static class DefaultRequestHandlers
  {
    /// <summary>
    ///   A default request handler for requests not passing the model validation.
    /// </summary>
    /// <param name="context">
    ///   An <see cref="ActionContext" /> instance containing the request model validation state.
    /// </param>
    /// <returns>
    ///   A JSON-encoded response explaining the request body validation failure.
    /// </returns>
    public static IActionResult InvalidModelStateHandler(ActionContext context) =>
      new ValidationFailedJsonResponse(context.ModelState);

    /// <summary>
    ///   A default request handler for requests that have caused an exception during processing.
    /// </summary>
    /// <param name="context">
    ///   An <see cref="HttpContext" /> instance containing the related HTTP connection data.
    /// </param>
    /// <returns>
    ///   A JSON-encoded response describing the occured server exception.
    /// </returns>
    public static IActionResult ExceptionHandler(HttpContext context) =>
      new EmptyJsonResponse(StatusCodes.Status500InternalServerError,
          "The request has caused an exception on the server.");

    /// <summary>
    ///   A default request handler for requests that were processed with HTTP error status codes (400-599).
    /// </summary>
    /// <param name="context">
    ///   An <see cref="HttpContext" /> instance containing the related HTTP connection data.
    /// </param>
    /// <returns>
    ///   A JSON-encoded response describing the HTTP error status.
    /// </returns>
    public static IActionResult StatusCodeHandler(HttpContext context) =>
      new EmptyJsonResponse(context.Response.StatusCode);
  }
}
