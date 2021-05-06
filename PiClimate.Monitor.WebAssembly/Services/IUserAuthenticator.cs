// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Threading.Tasks;

namespace PiClimate.Monitor.WebAssembly.Services
{
  /// <summary>
  ///   The common interface for authenticator service classes.
  ///   This basic interface variant supports only signing out operations. For signing in operations use the generic
  ///   interface variant <see cref="IUserAuthenticator{TAuthRequest,TAuthResponse}" />.
  /// </summary>
  public interface IUserAuthenticator
  {
    /// <summary>
    ///   Asynchronously signs the user out.
    /// </summary>
    Task SignOutAsync();
  }

  /// <summary>
  ///   The common interface for authenticator service classes.
  /// </summary>
  /// <typeparam name="TAuthData">
  ///   The type of the authentication data object.
  /// </typeparam>
  /// <typeparam name="TAuthResponse">
  ///   The type of the authentication response object.
  /// </typeparam>
  public interface IUserAuthenticator<in TAuthData, TAuthResponse> : IUserAuthenticator
  {
    /// <summary>
    ///   Asynchronously signs the user in using the provided authentication data.
    /// </summary>
    /// <param name="authRequest">
    ///   The authentication data object.
    /// </param>
    /// <returns>
    ///   The authentication response object.
    /// </returns>
    Task<TAuthResponse> SignInAsync(TAuthData authRequest);
  }
}
