// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PiClimate.Monitor.Components;
using PiClimate.Monitor.Configuration;
using PiClimate.Monitor.Pages;

namespace PiClimate.Monitor
{
  /// <summary>
  ///   The web host startup class.
  /// </summary>
  public class Startup
  {
    /// <summary>
    ///   Defines the name for anti-forgery token parameters.
    /// </summary>
    private const string AntiForgeryTokenParameterName = "AntiForgeryToken";

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
    /// <param name="configuration">
    ///   The web host configuration.
    ///   Provided via dependency injection.
    /// </param>
    /// <param name="environment">
    ///   The web host environment.
    ///   Provided via dependency injection.
    /// </param>
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      _settings = configuration.Get<GlobalSettings>();
      _environment = environment;
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

      // Add services related to the Razor Pages.
      services.AddRazorPages();

      // Add cookie-based user authentication services.
      services.AddAuthentication(Auth.SchemeName)
        .AddCookie(Auth.SchemeName, options =>
        {
          options.LoginPath = Auth.LoginPath;
          options.LogoutPath = Auth.LogoutPath;
          options.AccessDeniedPath = $"/{nameof(Status)}/{StatusCodes.Status403Forbidden}";
          options.ReturnUrlParameter = Auth.ReturnQueryParameterName;
          options.Cookie.Name = Auth.CookieName;
        });

      // Add authorization services.
      services.AddAuthorization();

      // Configure the anti-forgery protection services.
      services.AddAntiforgery(options =>
      {
        options.FormFieldName = AntiForgeryTokenParameterName;
        options.Cookie.Name = AntiForgeryTokenParameterName;
      });

      // Add cross-origin request sharing services.
      services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin()));

      // Add the selected measurement source as a service.
      services.AddMeasurementSource(_settings.UseMeasurementSource);

      // Configure the data protection services to put the persistent protection keys to the dedicated directory.
      var protectionKeysDirectoryPath = _settings.ProtectionKeysDirectoryPath;
      if (string.IsNullOrWhiteSpace(protectionKeysDirectoryPath))
        protectionKeysDirectoryPath = GlobalSettings.DefaultProtectionKeysDirectoryPath;
      if (!Directory.Exists(protectionKeysDirectoryPath))
        Directory.CreateDirectory(protectionKeysDirectoryPath);
      services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(protectionKeysDirectoryPath));
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
        app.UseDeveloperExceptionPage();
      else
        app.UseExceptionHandler($"/{nameof(Error)}");

      app.UseStatusCodePagesWithReExecute($"/{nameof(Status)}/{{0}}");
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseCors();
      app.UseEndpoints(endpoints => endpoints.MapRazorPages());
    }
  }
}
