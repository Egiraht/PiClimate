// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PiClimate.Monitor.Components;
using PiClimate.Monitor.ConfigurationLayout;
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
    ///   The web host configuration.
    /// </summary>
    private readonly IConfiguration _configuration;

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
      _configuration = configuration;
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
      services.AddRazorPages();
      services.AddAuthentication(Auth.SchemeName)
        .AddCookie(Auth.SchemeName, options =>
        {
          options.LoginPath = Auth.LoginPath;
          options.LogoutPath = Auth.LogoutPath;
          options.ReturnUrlParameter = Auth.ReturnQueryParameterName;
          options.Cookie.Name = Auth.CookieName;
        });
      services.AddAuthorization();
      services.AddAntiforgery(options =>
      {
        options.FormFieldName = AntiForgeryTokenParameterName;
        options.Cookie.Name = AntiForgeryTokenParameterName;
      });
      services.AddCors();
      services.AddMeasurementSource(_configuration[Root.UseMeasurementSource]);
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

      app.UseStatusCodePages();
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseCors(builder => builder.AllowAnyOrigin());
      app.UseEndpoints(endpoints => endpoints.MapRazorPages());
    }
  }
}
