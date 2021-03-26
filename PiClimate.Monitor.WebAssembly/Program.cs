// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PiClimate.Monitor.WebAssembly.Services;

namespace PiClimate.Monitor.WebAssembly
{
  /// <summary>
  ///   The main program's class.
  /// </summary>
  internal static class Program
  {
    /// <summary>
    ///   Test authentication state provider class.
    /// </summary>
    /// TODO: Remove after implementing normal authentication.
    private class AuthProvider : AuthenticationStateProvider
    {
      public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(
          new AuthenticationState(
            new ClaimsPrincipal(
              new ClaimsIdentity(
                new Claim[]
                {
                  new(ClaimTypes.Name, "User"),
                  new(ClaimTypes.Role, "User")
                },
                "Bearer", ClaimTypes.Name, ClaimTypes.Role))));
    };

    /// <summary>
    ///   The program's entry point.
    /// </summary>
    /// <param name="args">
    ///   The program arguments.
    /// </param>
    public static async Task Main(string[] args)
    {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);
      builder.RootComponents.Add<App>("#app");

      builder.Services.AddOptions();
      builder.Services.AddScoped(_ => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
      builder.Services.AddAuthorizationCore();
      builder.Services.AddScoped<AuthenticationStateProvider>(_ => new AuthProvider());
      builder.Services.AddSingleton<IStorageProvider, LocalStorageProvider>();

      await builder.Build().RunAsync();
    }
  }
}
