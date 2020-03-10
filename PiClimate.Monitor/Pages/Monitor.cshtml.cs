using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PiClimate.Monitor.Components;
using PiClimate.Monitor.Models;

namespace PiClimate.Monitor.Pages
{
  public class Monitor : PageModel
  {
    public const string PressurePageHandler = "Pressure";

    public const string TemperaturePageHandler = "Temperature";

    public const string HumidityPageHandler = "Humidity";

    public static string DefaultPageHandler => PressurePageHandler;

    public const string DataSourceRequestUri = nameof(Data);

    public const string DataSourceRequestMethod = "POST";

    public readonly ChartParameters ChartParameters = new ChartParameters
    {
      RequestUri = DataSourceRequestUri,
      RequestMethod = DataSourceRequestMethod
    };

    public IActionResult OnGet() => RedirectToPage(nameof(Monitor), DefaultPageHandler);

    public IActionResult OnGetPressure(MeasurementFilter filter)
    {
      ChartParameters.ChartId = "pressure-chart";
      ChartParameters.ChartLabel = "Pressure";
      ChartParameters.MeasurementParameter = MeasurementParameterTypes.Pressure;
      ChartParameters.LineColor = "blue";
      ChartParameters.Filter = filter;

      return Page();
    }

    public IActionResult OnGetTemperature(MeasurementFilter filter)
    {
      ChartParameters.ChartId = "temperature-chart";
      ChartParameters.ChartLabel = "Temperature";
      ChartParameters.MeasurementParameter = MeasurementParameterTypes.Temperature;
      ChartParameters.LineColor = "green";
      ChartParameters.Filter = filter;

      return Page();
    }

    public IActionResult OnGetHumidity(MeasurementFilter filter)
    {
      ChartParameters.ChartId = "humidity-chart";
      ChartParameters.ChartLabel = "Humidity";
      ChartParameters.MeasurementParameter = MeasurementParameterTypes.Humidity;
      ChartParameters.LineColor = "red";
      ChartParameters.Filter = filter;

      return Page();
    }
  }
}
