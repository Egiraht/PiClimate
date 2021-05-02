// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PiClimate.Common.Models;
using PiClimate.Common.Settings;
using PiClimate.Monitor.WebAssembly.Services;

namespace PiClimate.Monitor.WebAssembly
{
  /// <summary>
  ///   The main program's class.
  /// </summary>
  internal static class Program
  {
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

      // Adding framework services.
      builder.Services.AddOptions();
      builder.Services.AddAuthorizationCore();

      // Adding a storage provider service.
      builder.Services.AddTransient<IStorageProvider, LocalStorageProvider>();

      // Adding the HTTP client services.
      builder.Services.AddTransient<AuthenticationHandler>();
      builder.Services
        .AddHttpClient(string.Empty, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
        .AddHttpMessageHandler<AuthenticationHandler>();

      // Adding the authentication services.
      builder.Services.AddSingleton<AuthInfoAuthenticator>();
      builder.Services.AddSingleton<AuthenticationStateProvider>(provider =>
        provider.GetRequiredService<AuthInfoAuthenticator>());
      builder.Services.AddSingleton<IUserAuthenticator>(provider =>
        provider.GetRequiredService<AuthInfoAuthenticator>());
      builder.Services.AddSingleton<IUserAuthenticator<LoginForm, AuthInfo>>(provider =>
        provider.GetRequiredService<AuthInfoAuthenticator>());

      // Adding the client-side options services.
      builder.Services.AddTransient<IOptionsProvider<ClientOptions>, ClientOptionsProvider>();
      await using (var provider = builder.Services.BuildServiceProvider())
        builder.Services.AddSingleton<ClientOptions>(await provider
          .GetRequiredService<IOptionsProvider<ClientOptions>>()
          .GetOptionsAsync());

      await builder.Build().RunAsync();
    }
  }
}
