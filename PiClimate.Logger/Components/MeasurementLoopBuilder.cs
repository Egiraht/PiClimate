using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Loggers;
using PiClimate.Logger.Providers;

namespace PiClimate.Logger.Components
{
  public class MeasurementLoopBuilder
  {
    private const string MeasurementProviderClassNameSuffix = "Provider";

    private const string MeasurementLoggerClassNameSuffix = "Logger";

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

    public MeasurementLoopBuilder UseMeasurementProvider(string measurementProviderClassName)
    {
      if (string.IsNullOrWhiteSpace(measurementProviderClassName))
        return this;

      var measurementProviderType = GetType().Assembly.GetTypes()
        .FirstOrDefault(type =>
          type.GetInterfaces().Contains(typeof(IMeasurementProvider)) && (type.Name == measurementProviderClassName ||
            type.Name == $"{measurementProviderClassName}{MeasurementProviderClassNameSuffix}"));
      if (measurementProviderType == null)
        throw new ArgumentException(
          $"Cannot find a measurement provider class with the name \"{measurementProviderClassName}\".");

      var measurementProvider = Activator.CreateInstance(measurementProviderType) as IMeasurementProvider;
      if (measurementProvider == null)
        throw new ArgumentException(
          $"Failed to create an instance of \"{measurementProviderClassName}\" class.");

      return UseMeasurementProvider(measurementProvider);
    }

    public MeasurementLoopBuilder AddMeasurementLogger(IMeasurementLogger measurementLogger)
    {
      _measurementLoggers.Add(measurementLogger);
      return this;
    }

    public MeasurementLoopBuilder AddMeasurementLogger(string measurementLoggerClassName)
    {
      if (string.IsNullOrWhiteSpace(measurementLoggerClassName))
        return this;

      var measurementLoggerType = GetType().Assembly.GetTypes()
        .FirstOrDefault(type =>
          type.GetInterfaces().Contains(typeof(IMeasurementLogger)) && (type.Name == measurementLoggerClassName ||
            type.Name == $"{measurementLoggerClassName}{MeasurementLoggerClassNameSuffix}"));
      if (measurementLoggerType == null)
        throw new ArgumentException(
          $"Cannot find a measurement logger class with the name \"{measurementLoggerClassName}\".");

      var measurementLogger = Activator.CreateInstance(measurementLoggerType) as IMeasurementLogger;
      if (measurementLogger == null)
        throw new ArgumentException(
          $"Failed to create an instance of \"{measurementLoggerClassName}\" class.");

      return AddMeasurementLogger(measurementLogger);
    }

    public MeasurementLoopBuilder AddMeasurementLoggers(params IMeasurementLogger[] measurementLoggers)
    {
      _measurementLoggers.AddRange(measurementLoggers);
      return this;
    }

    public MeasurementLoopBuilder AddMeasurementLoggers(params string[] measurementLoggerClassNames)
    {
      foreach (var className in measurementLoggerClassNames)
        AddMeasurementLogger(className);
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
