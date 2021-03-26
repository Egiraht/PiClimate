// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Common.Components
{
  /// <summary>
  ///   The record representing a JSON-encoded payload model that will be serialized into JSON format during the.
  /// </summary>
  /// <typeparam name="TData">
  ///   The type of the data stored in the payload.
  /// </typeparam>
  public record JsonPayload<TData> where TData : class
  {
    /// <summary>
    ///   Gets or sets the status code of the response.
    /// </summary>
    public int StatusCode { get; init; }

    /// <summary>
    ///   Gets or sets the status description message of the response.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    ///   Gets or sets the data object of type <typeparamref name="TData" />.
    /// </summary>
    public TData? Data { get; init; }
  }
}
