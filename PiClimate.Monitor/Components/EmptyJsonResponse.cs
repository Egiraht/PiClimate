// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using Microsoft.AspNetCore.Http;

namespace PiClimate.Monitor.Components
{
  /// <summary>
  ///   An action response class containing only a serialized status code and description.
  /// </summary>
  public class EmptyJsonResponse : JsonResponse<object>
  {
    /// <summary>
    ///   Creates a new JSON response containing an empty object as a data payload.
    /// </summary>
    /// <param name="statusCode">
    ///   An HTTP status code describing the request processing status.
    ///   Defaults to status code 200.
    /// </param>
    /// <param name="statusDescription">
    ///   An optional HTTP status code description string.
    ///   If set to <c>null</c>, a standard status code description will be used.
    /// </param>
    public EmptyJsonResponse(int statusCode = StatusCodes.Status200OK, string? statusDescription = null) :
      base(null, statusCode, statusDescription)
    {
    }
  }
}
