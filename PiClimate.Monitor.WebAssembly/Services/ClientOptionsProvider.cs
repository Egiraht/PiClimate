// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Net.Http;
using System.Threading.Tasks;
using PiClimate.Common;
using PiClimate.Common.Components;
using PiClimate.Common.Settings;
using PiClimate.Monitor.WebAssembly.Components;

namespace PiClimate.Monitor.WebAssembly.Services
{
  /// <summary>
  ///   The client-side options provider service that acquires the options from the server.
  /// </summary>
  public class ClientOptionsProvider : IOptionsProvider<ClientOptions>
  {
    /// <summary>
    ///   The HTTP client factory service.
    /// </summary>
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    ///   Initializes the service.
    /// </summary>
    /// <param name="httpClientFactory">
    ///   The HTTP client factory service.
    ///   Provided via dependency injection.
    /// </param>
    public ClientOptionsProvider(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    /// <inheritdoc />
    public async Task<ClientOptions> GetOptionsAsync()
    {
      try
      {
        using var httpClient = _httpClientFactory.CreateClient();
        return (await httpClient.GetJsonAsync<JsonPayload<ClientOptions>>(ApiEndpoints.OptionsEndpoint))?.Data ?? new();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return new();
      }
    }
  }
}
