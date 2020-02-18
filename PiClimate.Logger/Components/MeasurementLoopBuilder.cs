using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Loggers;
using PiClimate.Logger.Providers;

namespace PiClimate.Logger.Components
{
  public class MeasurementLoopBuilder
  {
    private IConfiguration _configuration = new ConfigurationBuilder().Build();

    private IMeasurementProvider? _measurementProvider;

    private readonly List<IMeasurementLogger> _measurementLoggers = new List<IMeasurementLogger>();

    private readonly MeasurementLoopOptions _options = new MeasurementLoopOptions();

    public MeasurementLoopBuilder UseConfiguration(IConfiguration configuration)
    {
      _configuration = configuration;
      return this;
    }

    public MeasurementLoopBuilder UseMeasurementProvider(IMeasurementProvider measurementProvider)
    {
      _measurementProvider?.Dispose();
      _measurementProvider = measurementProvider;
      return this;
    }

    public MeasurementLoopBuilder AddMeasurementLogger(IMeasurementLogger measurementLogger)
    {
      _measurementLoggers.Add(measurementLogger);
      return this;
    }

    public MeasurementLoopBuilder SetMeasurementLoopDelay(int delay)
    {
      _options.MeasurementLoopDelay = delay;
      return this;
    }

    public MeasurementLoop Build()
    {
      if (_measurementProvider == null)
        throw new InvalidOperationException("Cannot build the measurement loop as no measurement provider is set.");

      return new MeasurementLoop(_configuration, _measurementProvider, _measurementLoggers, _options);
    }
  }
}
