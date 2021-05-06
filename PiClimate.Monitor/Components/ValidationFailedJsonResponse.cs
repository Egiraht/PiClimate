// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PiClimate.Monitor.Components
{
  /// <summary>
  ///   A <see cref="JsonResponse{TData}" /> describing the request validation failure.
  /// </summary>
  public class ValidationFailedJsonResponse : JsonResponse<object>
  {
    /// <summary>
    ///   Creates a new <see cref="JsonResponse{TData}" /> instance describing the request validation error.
    /// </summary>
    /// <param name="errorMessage">
    ///   An error message giving a summary of the request validation failure.
    /// </param>
    public ValidationFailedJsonResponse(string? errorMessage = null) :
      base(new { }, StatusCodes.Status400BadRequest, $"The request validation failed: {errorMessage}")
    {
    }

    /// <summary>
    ///   Creates a new <see cref="JsonResponse{TData}" /> instance describing the request validation error.
    /// </summary>
    /// <param name="validationDictionary">
    ///   A <see cref="ModelStateDictionary" /> instance containing a set of request body validation errors.
    /// </param>
    public ValidationFailedJsonResponse(ModelStateDictionary validationDictionary) :
      base(validationDictionary.ToDictionary(
          modelState => modelState.Key,
          modelState => modelState.Value.Errors.Select(error => error.ErrorMessage)),
        StatusCodes.Status400BadRequest, "The request body validation failed.")
    {
    }
  }
}
