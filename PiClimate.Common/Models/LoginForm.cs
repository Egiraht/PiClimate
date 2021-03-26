// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.ComponentModel.DataAnnotations;

namespace PiClimate.Common.Models
{
  /// <summary>
  ///   The record representing user login form data.
  /// </summary>
  public record LoginForm
  {
    /// <summary>
    ///   User's name.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///   User's password.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///   The flag indicating whether the user should be remembered over different browser sessions.
    /// </summary>
    public bool Remember { get; set; }
  }
}
