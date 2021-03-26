// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PiClimate.Monitor.WebAssembly.Services
{
  /// <summary>
  ///   The interface for authentication access token providers.
  /// </summary>
  public interface IAccessTokenProvider
  {
    /// <summary>
    ///   Gets the name of the authentication scheme.
    /// </summary>
    string AuthenticationSchemeName { get; }

    /// <summary>
    ///   Asynchronously gets the authentication access token.
    /// </summary>
    /// <returns>
    ///   A string representation of the access token.
    /// </returns>
    Task<string?> GetAccessTokenValueAsync();

    /// <summary>
    ///   Asynchronously creates the HTTP authorization header containing the authentication scheme name and
    ///   the access token value.
    /// </summary>
    /// <returns>
    ///   The <see cref="AuthenticationHeaderValue" /> object containing the header information, or <c>null</c>
    ///   if no access token is available.
    /// </returns>
    async Task<AuthenticationHeaderValue?> CreateAuthorizationHeaderAsync() =>
      !string.IsNullOrEmpty(AuthenticationSchemeName) && !string.IsNullOrWhiteSpace(await GetAccessTokenValueAsync())
        ? new AuthenticationHeaderValue(AuthenticationSchemeName, await GetAccessTokenValueAsync())
        : null;
  }
}
