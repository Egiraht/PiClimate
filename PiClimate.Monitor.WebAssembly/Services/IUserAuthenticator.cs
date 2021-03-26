// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Security.Claims;
using System.Threading.Tasks;
using PiClimate.Common.Models;

namespace PiClimate.Monitor.WebAssembly.Services
{
  /// <summary>
  ///   The interface for services performing the user authentication operations.
  /// </summary>
  public interface IUserAuthenticator
  {
    /// <summary>
    ///   Gets the current user object.
    /// </summary>
    ClaimsPrincipal User { get; }

    /// <summary>
    ///   Gets the flag indicating whether the authentication process is running at the moment.
    /// </summary>
    bool IsAuthenticating { get; }

    /// <summary>
    ///   Asynchronously signs the user in using the provided login credentials.
    /// </summary>
    /// <param name="loginForm">
    ///   The user's login credentials.
    /// </param>
    Task SignInAsync(LoginForm loginForm);

    /// <summary>
    ///   Asynchronously signs the user out.
    /// </summary>
    Task SignOutAsync();

    /// <summary>
    ///   Asynchronously restores the user data from the previous session if it is possible.
    /// </summary>
    Task RestoreSessionAsync();
  }
}
