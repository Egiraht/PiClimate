using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using PiClimate.Logger.Limiters;
using PiClimate.Logger.Loggers;
using PiClimate.Logger.Providers;

namespace PiClimate.Logger.Components
{
  /// <summary>
  ///   The measurement loop builder class.
  ///   Provides the preferred way of creating a new measurement loop.
  /// </summary>
  public class MeasurementLoopBuilder
  {
    /// <summary>
    ///   The common class name suffix of measurement providers.
    /// </summary>
    private const string MeasurementProviderClassNameSuffix = "Provider";

    /// <summary>
    ///   The common class name suffix of measurement loggers.
    /// </summary>
    private const string MeasurementLoggerClassNameSuffix = "Logger";

    /// <summary>
    ///   The common class name suffix of measurement limiters.
    /// </summary>
    private const string MeasurementLimiterClassNameSuffix = "Limiter";

    /// <summary>
    ///   The configuration to be used for configuring a new measurement loop.
    /// </summary>
    private IConfiguration _configuration = new ConfigurationBuilder().Build();

    /// <summary>
    ///   The measurement provider to be associated with a new measurement loop.
    /// </summary>
    private IMeasurementProvider? _measurementProvider;

    /// <summary>
    ///   The list of measurement loggers to be associated with a new measurement loop.
    /// </summary>
    private readonly List<IMeasurementLogger> _measurementLoggers = new List<IMeasurementLogger>();

    /// <summary>
    ///   The list of measurement limiters to be associated with a new measurement loop.
    /// </summary>
    private readonly List<IMeasurementLimiter> _measurementLimiters = new List<IMeasurementLimiter>();

    /// <summary>
    ///   The <see cref="MeasurementLoopOptions" /> instance containing the additional measurement loop options.
    /// </summary>
    private readonly MeasurementLoopOptions _options = new MeasurementLoopOptions();

    /// <summary>
    ///   Instructs the builder to use the provided configuration.
    /// </summary>
    /// <param name="configuration">
    ///   The <see cref="IConfiguration" /> instance to use.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder UseConfiguration(IConfiguration configuration)
    {
      _configuration = configuration;
      return this;
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement provider.
    /// </summary>
    /// <param name="measurementProvider">
    ///   The <see cref="IMeasurementProvider" /> instance to use.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder UseMeasurementProvider(IMeasurementProvider measurementProvider)
    {
      _measurementProvider?.Dispose();
      _measurementProvider = measurementProvider;
      return this;
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement provider.
    /// </summary>
    /// <param name="measurementProviderClassName">
    ///   The class name of the measurement provider to use.
    ///   The class name suffix (<see cref="MeasurementProviderClassNameSuffix" />) may be omitted.
    /// </param>
    /// <exception cref="ArgumentException">
    ///   Cannot find a measurement provider class with the provided name.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///   Failed to create a new instance of the provided measurement provider class.
    /// </exception>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder UseMeasurementProvider(string measurementProviderClassName)
    {
      if (string.IsNullOrWhiteSpace(measurementProviderClassName))
        return this;

      var measurementProviderType = Assembly.GetExecutingAssembly()
        .GetTypes()
        .FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IMeasurementProvider)) &&
          (type.Name == measurementProviderClassName ||
            type.Name == $"{measurementProviderClassName}{MeasurementProviderClassNameSuffix}"));
      if (measurementProviderType == null)
        throw new ArgumentException(
          $"Cannot find a measurement provider class with the name \"{measurementProviderClassName}\".");

      var measurementProvider = Activator.CreateInstance(measurementProviderType) as IMeasurementProvider;
      if (measurementProvider == null)
        throw new InvalidOperationException(
          $"Failed to create an instance of \"{measurementProviderClassName}\" class.");

      return UseMeasurementProvider(measurementProvider);
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement logger.
    /// </summary>
    /// <param name="measurementLogger">
    ///   The <see cref="IMeasurementLogger" /> instance to use.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder AddMeasurementLogger(IMeasurementLogger measurementLogger)
    {
      _measurementLoggers.Add(measurementLogger);
      return this;
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement logger.
    /// </summary>
    /// <param name="measurementLoggerClassName">
    ///   The class name of the measurement logger to use.
    ///   The class name suffix (<see cref="MeasurementLoggerClassNameSuffix" />) may be omitted.
    /// </param>
    /// <exception cref="ArgumentException">
    ///   Cannot find a measurement logger class with the provided name.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///   Failed to create a new instance of the provided measurement logger class.
    /// </exception>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder AddMeasurementLogger(string measurementLoggerClassName)
    {
      if (string.IsNullOrWhiteSpace(measurementLoggerClassName))
        return this;

      var measurementLoggerType = Assembly.GetExecutingAssembly()
        .GetTypes()
        .FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IMeasurementLogger)) &&
          (type.Name == measurementLoggerClassName ||
            type.Name == $"{measurementLoggerClassName}{MeasurementLoggerClassNameSuffix}"));
      if (measurementLoggerType == null)
        throw new ArgumentException(
          $"Cannot find a measurement logger class with the name \"{measurementLoggerClassName}\".");

      var measurementLogger = Activator.CreateInstance(measurementLoggerType) as IMeasurementLogger;
      if (measurementLogger == null)
        throw new InvalidOperationException(
          $"Failed to create an instance of \"{measurementLoggerClassName}\" class.");

      return AddMeasurementLogger(measurementLogger);
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement loggers.
    /// </summary>
    /// <param name="measurementLoggers">
    ///   The collection of <see cref="IMeasurementLogger" /> instances to use.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder AddMeasurementLoggers(IEnumerable<IMeasurementLogger> measurementLoggers)
    {
      _measurementLoggers.AddRange(measurementLoggers);
      return this;
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement loggers.
    /// </summary>
    /// <param name="measurementLoggers">
    ///   The array or sequence of <see cref="IMeasurementLogger" /> instances to use.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder AddMeasurementLoggers(params IMeasurementLogger[] measurementLoggers) =>
      AddMeasurementLoggers(measurementLoggers.AsEnumerable());

    /// <summary>
    ///   Instructs the builder to use the provided measurement loggers.
    /// </summary>
    /// <param name="measurementLoggerClassNames">
    ///   The collection of class names of the measurement loggers to use.
    ///   The class name suffixes (<see cref="MeasurementLoggerClassNameSuffix" />) may be omitted.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder AddMeasurementLoggers(IEnumerable<string> measurementLoggerClassNames)
    {
      foreach (var className in measurementLoggerClassNames)
        AddMeasurementLogger(className);
      return this;
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement loggers.
    /// </summary>
    /// <param name="measurementLoggerClassNames">
    ///   The array or sequence of class names of the measurement loggers to use.
    ///   The class name suffixes (<see cref="MeasurementLoggerClassNameSuffix" />) may be omitted.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder AddMeasurementLoggers(params string[] measurementLoggerClassNames) =>
      AddMeasurementLoggers(measurementLoggerClassNames.AsEnumerable());

    /// <summary>
    ///   Instructs the builder to clear the list of the used measurement loggers.
    /// </summary>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder ClearMeasurementLoggers()
    {
      _measurementLoggers.Clear();
      return this;
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement limiter.
    /// </summary>
    /// <param name="measurementLimiter">
    ///   The <see cref="IMeasurementLimiter" /> instance to use.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder AddMeasurementLimiter(IMeasurementLimiter measurementLimiter)
    {
      _measurementLimiters.Add(measurementLimiter);
      return this;
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement limiter.
    /// </summary>
    /// <param name="measurementLimiterClassName">
    ///   The class name of the measurement limiter to use.
    ///   The class name suffix (<see cref="MeasurementLimiterClassNameSuffix" />) may be omitted.
    /// </param>
    /// <exception cref="ArgumentException">
    ///   Cannot find a measurement limiter class with the provided name.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///   Failed to create a new instance of the provided measurement limiter class.
    /// </exception>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder AddMeasurementLimiter(string measurementLimiterClassName)
    {
      if (string.IsNullOrWhiteSpace(measurementLimiterClassName))
        return this;

      var measurementLimiterType = Assembly.GetExecutingAssembly()
        .GetTypes()
        .FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IMeasurementLimiter)) &&
          (type.Name == measurementLimiterClassName ||
            type.Name == $"{measurementLimiterClassName}{MeasurementLimiterClassNameSuffix}"));
      if (measurementLimiterType == null)
        throw new ArgumentException(
          $"Cannot find a measurement limiter class with the name \"{measurementLimiterClassName}\".");

      var measurementLimiter = Activator.CreateInstance(measurementLimiterType) as IMeasurementLimiter;
      if (measurementLimiter == null)
        throw new InvalidOperationException(
          $"Failed to create an instance of \"{measurementLimiterClassName}\" class.");

      return AddMeasurementLimiter(measurementLimiter);
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement limiters.
    /// </summary>
    /// <param name="measurementLimiters">
    ///   The collection of <see cref="IMeasurementLimiter" /> instances to use.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder AddMeasurementLimiters(IEnumerable<IMeasurementLimiter> measurementLimiters)
    {
      _measurementLimiters.AddRange(measurementLimiters);
      return this;
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement limiters.
    /// </summary>
    /// <param name="measurementLimiters">
    ///   The array or sequence of <see cref="IMeasurementLimiter" /> instances to use.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder AddMeasurementLimiters(params IMeasurementLimiter[] measurementLimiters) =>
      AddMeasurementLimiters(measurementLimiters.AsEnumerable());

    /// <summary>
    ///   Instructs the builder to use the provided measurement limiters.
    /// </summary>
    /// <param name="measurementLimiterClassNames">
    ///   The collection of class names of the measurement limiters to use.
    ///   The class name suffixes (<see cref="MeasurementLimiterClassNameSuffix" />) may be omitted.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder AddMeasurementLimiters(IEnumerable<string> measurementLimiterClassNames)
    {
      foreach (var className in measurementLimiterClassNames)
        AddMeasurementLimiter(className);
      return this;
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement limiters.
    /// </summary>
    /// <param name="measurementLimiterClassNames">
    ///   The array or sequence of class names of the measurement limiters to use.
    ///   The class name suffixes (<see cref="MeasurementLimiterClassNameSuffix" />) may be omitted.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder AddMeasurementLimiters(params string[] measurementLimiterClassNames) =>
      AddMeasurementLimiters(measurementLimiterClassNames.AsEnumerable());

    /// <summary>
    ///   Instructs the builder to clear the list of the used measurement limiters.
    /// </summary>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder ClearMeasurementLimiters()
    {
      _measurementLimiters.Clear();
      return this;
    }

    /// <summary>
    ///   Instructs the builder to use the provided measurement loop delay.
    /// </summary>
    /// <param name="delay">
    ///   The measurement loop delay value in seconds.
    /// </param>
    /// <returns>
    ///   The modified builder instance.
    /// </returns>
    public MeasurementLoopBuilder SetMeasurementLoopDelay(int delay)
    {
      _options.MeasurementLoopDelay = delay;
      return this;
    }

    /// <summary>
    ///   Builds a new measurement loop using the settings provided for the builder.
    /// </summary>
    /// <returns>
    ///   A new configured <see cref="MeasurementLoop" /> instance.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///   No measurement provider is set for the builder.
    /// </exception>
    public MeasurementLoop Build()
    {
      if (_measurementProvider == null)
        throw new InvalidOperationException("Cannot build the measurement loop as no measurement provider is set.");

      return new MeasurementLoop(_configuration, _measurementProvider, _measurementLoggers, _measurementLimiters,
        _options);
    }
  }
}
