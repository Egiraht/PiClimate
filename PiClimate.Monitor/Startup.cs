// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PiClimate.Common;
using PiClimate.Common.Settings;
using PiClimate.Monitor.Components;
using PiClimate.Monitor.Controllers;
using PiClimate.Monitor.Settings;

namespace PiClimate.Monitor
{
  /// <summary>
  ///   The web host startup class.
  /// </summary>
  public class Startup
  {
    /// <summary>
    ///   The global settings to be used for configuring a host.
    /// </summary>
    private readonly GlobalSettings _settings;

    /// <summary>
    ///   The web host environment.
    /// </summary>
    private readonly IWebHostEnvironment _environment;

    /// <summary>
    ///   Creates a new web host startup class instance.
    /// </summary>
    /// <param name="environment">
    ///   The web host environment.
    ///   Provided via dependency injection.
    /// </param>
    public Startup(IWebHostEnvironment environment)
    {
      _environment = environment;
      _settings = SettingsFactory.ReadSettings<GlobalSettings>(Program.SettingsFilePath,
        Environment.GetCommandLineArgs());
    }

    /// <summary>
    ///   Configures the web host services.
    /// </summary>
    /// <param name="services">
    ///   The web host service collection.
    /// </param>
    public void ConfigureServices(IServiceCollection services)
    {
      // Add the global settings object as a service.
      services.AddSingleton(_settings);

      // Add services related to controllers.
      services.AddControllers()
        .ConfigureApiBehaviorOptions(options =>
          options.InvalidModelStateResponseFactory = DefaultRequestHandlers.InvalidModelStateHandler);

      // Add cookie-based user authentication services.
      services.AddAuthentication(Auth.SchemeName)
        .AddCookie(Auth.SchemeName, options =>
        {
          options.ClaimsIssuer = $"{nameof(PiClimate)}.{nameof(Monitor)}";
          options.LoginPath = $"{ApiEndpoints.StatusEndpoint}/{StatusCodes.Status401Unauthorized}";
          options.LogoutPath = ApiEndpoints.UserSignOutEndpoint;
          options.AccessDeniedPath = $"{ApiEndpoints.StatusEndpoint}/{StatusCodes.Status403Forbidden}";
          options.SlidingExpiration = true;
          options.ExpireTimeSpan = TimeSpan.FromSeconds(_settings.AuthenticationOptions.CookieExpirationPeriod);
          options.Cookie.Name = Auth.CookieName;
          options.Cookie.HttpOnly = true;
          options.Cookie.SameSite = SameSiteMode.Lax;
          options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
          options.Cookie.IsEssential = true;
        });

      // Add authorization services.
      services.AddAuthorization();

      // Add cross-origin request sharing services.
      services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin()));

      // Configure the data protection services to put the persistent protection keys to the dedicated directory.
      var protectionKeysDirectoryPath = _settings.ProtectionKeysDirectoryPath;
      if (string.IsNullOrWhiteSpace(protectionKeysDirectoryPath))
        protectionKeysDirectoryPath = GlobalSettings.DefaultProtectionKeysDirectoryPath;
      if (!Directory.Exists(protectionKeysDirectoryPath))
        Directory.CreateDirectory(protectionKeysDirectoryPath);
      services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(protectionKeysDirectoryPath));

      // Add the selected measurement source as a service.
      services.AddMeasurementSource(_settings.UseMeasurementSource);
    }

    /// <summary>
    ///   Configures the web host application's middleware pipeline.
    /// </summary>
    /// <param name="app">
    ///   The web host application builder to be configured.
    /// </param>
    public void Configure(IApplicationBuilder app)
    {
      if (_environment.IsDevelopment())
        app.UseWebAssemblyDebugging();
      app.UseExceptionHandler(builder => builder.Run(async context =>
        await DefaultRequestHandlers.ExceptionHandler(context)
          .ExecuteResultAsync(new ActionContext(context, context.GetRouteData(), new ActionDescriptor()))));
      app.UseStatusCodePages(builder => builder.Run(async context =>
        await DefaultRequestHandlers.StatusCodeHandler(context)
          .ExecuteResultAsync(new ActionContext(context, context.GetRouteData(), new ActionDescriptor()))));
      app.UseHttpsRedirection();
      app.UseBlazorFrameworkFiles();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseCors();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapFallbackToFile("index.html");
      });
    }
  }
}
