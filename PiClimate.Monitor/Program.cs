using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PiClimate.Monitor
{
  internal static class Program
  {
    private static void Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();
      host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseStartup<Startup>();
          webBuilder.UseWebRoot("Static");
        });
  }
}
