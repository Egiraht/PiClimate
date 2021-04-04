// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Common.Models
{
  /// <summary>
  ///   The record containing the authentication information.
  /// </summary>
  public record AuthInfo
  {
    /// <summary>
    ///   Gets the user name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    ///   Gets the user role.
    /// </summary>
    public string Role { get; init; } = string.Empty;
  }
}
