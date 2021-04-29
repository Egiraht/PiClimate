// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using PiClimate.Common.Models;

namespace PiClimate.Monitor.WebAssembly.Services
{
  /// <summary>
  ///   The message handler class that handles the server-side authentication process and automatically signs out on
  ///   receiving the <c>401 Unauthorized</c> response.
  /// </summary>
  public class AuthenticationHandler : DelegatingHandler
  {
    /// <summary>
    ///   The storage provider service instance.
    /// </summary>
    private readonly IStorageProvider _storageProvider;

    /// <summary>
    ///   The authentication state provider service instance.
    /// </summary>
    private readonly AuthenticationStateProvider _authProvider;

    /// <summary>
    ///   Creates a new message handler instance.
    /// </summary>
    /// <param name="storageProvider">
    ///   The storage provider service instance.
    ///   Provided via dependency injection.
    /// </param>
    /// <param name="authProvider">
    ///   The authentication state provider service instance.
    ///   Provided via dependency injection.
    /// </param>
    public AuthenticationHandler(IStorageProvider storageProvider, AuthenticationStateProvider authProvider)
    {
      _storageProvider = storageProvider;
      _authProvider = authProvider;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      request.SetBrowserRequestMode(BrowserRequestMode.Cors);
      request.SetBrowserRequestCredentials(BrowserRequestCredentials.SameOrigin);

      var response = await base.SendAsync(request, cancellationToken);
      if (response.StatusCode != HttpStatusCode.Unauthorized)
        return response;

      await _storageProvider.RemoveItemAsync(nameof(AuthInfo));
      await _authProvider.GetAuthenticationStateAsync();
      return response;
    }

    /// <inheritdoc />
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken) =>
      SendAsync(request, cancellationToken).GetAwaiter().GetResult();
  }
}
