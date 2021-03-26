// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Common.Models
{
  /// <summary>
  ///   The record containing user authentication tokens.
  /// </summary>
  public record AuthTokens
  {
    /// <summary>
    ///   The access token used for user authentication.
    /// </summary>
    public string? AccessToken { get; init; }

    /// <summary>
    ///   The refresh token used for access token refreshing.
    /// </summary>
    public string? RefreshToken { get; init; }
  }
}
