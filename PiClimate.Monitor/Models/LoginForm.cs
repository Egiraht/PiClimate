// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System.ComponentModel.DataAnnotations;

namespace PiClimate.Monitor.Models
{
  /// <summary>
  ///   A model class representing a login input form.
  /// </summary>
  public class LoginForm
  {
    /// <summary>
    ///   Gets or sets the user's name.
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    public string Name { get; set; } = "";

    /// <summary>
    ///   Gets or sets the user's password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    /// <summary>
    ///   Gets or sets the flag indicating whether the user should be remembered between sessions.
    /// </summary>
    public bool Remember { get; set; } = false;
  }
}
