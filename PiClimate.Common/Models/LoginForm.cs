// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.ComponentModel.DataAnnotations;

namespace PiClimate.Common.Models
{
  /// <summary>
  ///   The record containing the user authentication data.
  /// </summary>
  public record LoginForm
  {
    /// <summary>
    ///   Gets the username.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///   Gets the password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///   Gets the flag indicating whether the authentication state should persist between browser sessions.
    /// </summary>
    public bool Remember { get; set; } = false;
  }
}
