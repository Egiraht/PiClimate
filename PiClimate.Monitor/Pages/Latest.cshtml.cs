using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PiClimate.Monitor.Components;
using PiClimate.Monitor.Models;
using PiClimate.Monitor.Sources;
using PiClimate.Monitor.Settings;

namespace PiClimate.Monitor.Pages
{
  /// <summary>
  ///   The latest data page code-behind class.
  /// </summary>
  [Authorize]
  public class Latest : PageModel
  {
    /// <summary>
    ///   The measurement source object.
    /// </summary>
    private readonly IMeasurementSource _source;

    /// <summary>
    ///   Gets the time period from the latest logged measurement till the current moment beyond which the data
    ///   can be treated obsolete, so the corresponding warning will appear.
    /// </summary>
    public TimeSpan ObsoleteDataPeriod { get; }

    /// <summary>
    ///   Gets the collection of the latest measurements.
    /// </summary>
    public IEnumerable<Measurement> LatestMeasurements { get; private set; } = new List<Measurement>();

    /// <summary>
    ///   Gets the measurement units the pressure is expressed in.
    /// </summary>
    public string PressureUnits { get; } = MeasurementDefaults.DefaultPressureUnits;

    /// <summary>
    ///   Gets the measurement units the temperature is expressed in.
    /// </summary>
    public string TemperatureUnits { get; } = MeasurementDefaults.DefaultTemperatureUnits;

    /// <summary>
    ///   Gets the measurement units the humidity is expressed in.
    /// </summary>
    public string HumidityUnits { get; } = MeasurementDefaults.DefaultHumidityUnits;

    /// <summary>
    ///   Initializes the new instance of the page.
    /// </summary>
    /// <param name="settings">
    ///   The global settings instance.
    ///   Provided via dependency injection.
    /// </param>
    /// <param name="source">
    ///   The used measurement source service.
    ///   Provided via dependency injection.
    /// </param>
    public Latest(GlobalSettings settings, IMeasurementSource source)
    {
      ObsoleteDataPeriod = TimeSpan.FromSeconds(settings.ObsoleteDataPeriod);
      _source = source;
    }

    /// <summary>
    ///   The callback handler for GET HTTP requests.
    /// </summary>
    /// <param name="request">
    ///   The latest data request object.
    ///   Provided via the request parameters binding.
    /// </param>
    /// <returns>
    ///   An HTTP response with the processed page contents.
    /// </returns>
    public async Task<IActionResult> OnGetAsync(LatestDataRequest request)
    {
      LatestMeasurements = await _source.GetLatestMeasurementsAsync(request);
      return Page();
    }
  }
}
