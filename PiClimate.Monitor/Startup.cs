using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PiClimate.Monitor
{
  public class Startup
  {
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      _configuration = configuration;
      _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddRazorPages();
      services.AddServerSideBlazor();
    }

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
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapRazorPages();
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/app");
      });
    }
  }
}
