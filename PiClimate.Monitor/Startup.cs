using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PiClimate.Monitor.Services;

namespace PiClimate.Monitor
{
  /// <summary>
  ///   The web host startup class.
  /// </summary>
  public class Startup
  {
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
      services.AddCors();
      services.AddSingleton<IMeasurementSource>(new MySqlSource(_configuration));
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
        app.UseExceptionHandler();

      app.UseStatusCodePages();
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseCors(builder => builder.AllowAnyOrigin());
      app.UseEndpoints(endpoints => endpoints.MapRazorPages());
    }
  }
}
