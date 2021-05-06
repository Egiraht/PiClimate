// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using PiClimate.Common.Components;

namespace PiClimate.Monitor.Components
{
  /// <summary>
  ///   An action response class containing a JSON-encoded data payload.
  /// </summary>
  /// <typeparam name="TData">
  ///   Type of the data object to be sent as a resulting data payload.
  /// </typeparam>
  public class JsonResponse<TData> : JsonResult
  {
    /// <summary>
    ///   Creates a new JSON response.
    /// </summary>
    /// <param name="data">
    ///   A <typeparamref name="TData" /> object defining the actual payload of the response.
    ///   It will be serialized into the <see cref="JsonPayload{TData}.Data" /> property within the response
    ///   JSON object.
    ///   Can be <c>null</c> if no data are necessary.
    /// </param>
    /// <param name="statusCode">
    ///   An HTTP status code describing the request processing status.
    ///   Defaults to status code 200.
    /// </param>
    /// <param name="statusDescription">
    ///   An optional HTTP status code description string.
    ///   If set to <c>null</c>, a standard status code description will be used.
    /// </param>
    public JsonResponse(TData? data, int statusCode = StatusCodes.Status200OK, string? statusDescription = null) :
      base(null)
    {
      StatusCode = statusCode;
      ContentType = "application/json; charset=utf-8";
      Value = new JsonPayload<TData>
      {
        StatusCode = statusCode,
        Description = statusDescription ?? ReasonPhrases.GetReasonPhrase(statusCode),
        Data = data
      };
    }
  }
}
