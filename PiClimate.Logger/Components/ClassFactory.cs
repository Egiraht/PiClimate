using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PiClimate.Logger.Limiters;
using PiClimate.Logger.Loggers;
using PiClimate.Logger.Providers;

namespace PiClimate.Logger.Components
{
  /// <summary>
  ///   The factory class creating the measurements-related class instances by their names.
  /// </summary>
  static class ClassFactory
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
    ///   Gets a measurement provider type by its name.
    /// </summary>
    /// <param name="providerName">
    ///   The name of the measurement provider to get a type for.
    ///   Can be a full class name or a class name with a <see cref="MeasurementProviderClassNameSuffix" /> omitted.
    /// </param>
    /// <returns>
    ///   The measurement provider type on success or <c>null</c> otherwise.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///   Cannot find a measurement provider class with the provided name or the name is empty.
    /// </exception>
    public static Type GetMeasurementProviderType(string providerName)
    {
      providerName = providerName.Trim();
      if (string.IsNullOrEmpty(providerName))
        throw new ArgumentException("No measurement provider name is provided.");

      var providerType = Assembly.GetExecutingAssembly()
        .GetTypes()
        .FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IMeasurementProvider)) &&
          (type.Name == providerName || type.Name == $"{providerName}{MeasurementProviderClassNameSuffix}"));
      if (providerType == null)
        throw new ArgumentException($"Cannot find a measurement provider class with the name \"{providerName}\".");
      return providerType;
    }

    /// <summary>
    ///   Gets an enumeration of measurement logger types by their names.
    /// </summary>
    /// <param name="loggerNamesString">
    ///   The comma-separated list of the measurement logger names to get types for.
    ///   Can include full class names or class names with a <see cref="MeasurementLoggerClassNameSuffix" /> omitted.
    ///   The duplicates of class names will be removed.
    /// </param>
    /// <returns>
    ///   The enumeration of measurement logger types.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///   Cannot find a measurement logger class with one of the provided names.
    /// </exception>
    public static IEnumerable<Type> GetMeasurementLoggerTypes(string loggerNamesString)
    {
      var loggerNames =
        loggerNamesString.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
      if (!loggerNames.Any())
        return Array.Empty<Type>();

      var loggers = new List<Type>();
      foreach (var loggerName in loggerNames)
      {
        var loggerType = Assembly.GetExecutingAssembly()
          .GetTypes()
          .FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IMeasurementLogger)) &&
            (type.Name == loggerName || type.Name == $"{loggerName}{MeasurementLoggerClassNameSuffix}"));
        if (loggerType == null)
          throw new ArgumentException($"Cannot find a measurement logger class with the name \"{loggerName}\".");
        if (!loggers.Contains(loggerType))
          loggers.Add(loggerType);
      }

      return loggers;
    }

    /// <summary>
    ///   Gets an enumeration of measurement limiter types by their names.
    /// </summary>
    /// <param name="limiterNamesString">
    ///   The comma-separated list of the measurement limiter names to get types for.
    ///   Can include full class names or class names with a <see cref="MeasurementLimiterClassNameSuffix" /> omitted.
    ///   The duplicates of class names will be removed.
    /// </param>
    /// <returns>
    ///   The enumeration of measurement limiter types.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///   Cannot find a measurement limiter class with one of the provided names.
    /// </exception>
    public static IEnumerable<Type> GetMeasurementLimiterTypes(string limiterNamesString)
    {
      var limiterNames =
        limiterNamesString.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
      if (!limiterNames.Any())
        return Array.Empty<Type>();

      var limiters = new List<Type>();
      foreach (var limiterName in limiterNames)
      {
        var limiterType = Assembly.GetExecutingAssembly()
          .GetTypes()
          .FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IMeasurementLimiter)) &&
            (type.Name == limiterName || type.Name == $"{limiterName}{MeasurementLimiterClassNameSuffix}"));
        if (limiterType == null)
          throw new ArgumentException($"Cannot find a measurement limiter class with the name \"{limiterName}\".");
        if (!limiters.Contains(limiterType))
          limiters.Add(limiterType);
      }

      return limiters;
    }

    /// <summary>
    ///   Creates a measurement provider instance by its name.
    /// </summary>
    /// <param name="providerName">
    ///   The name of the measurement provider to create an instance for.
    ///   Can be a full class name or a class name with a <see cref="MeasurementProviderClassNameSuffix" /> omitted.
    /// </param>
    /// <returns>
    ///   The created measurement provider instance.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///   Failed to create a new instance of the provided measurement provider class.
    /// </exception>
    public static IMeasurementProvider CreateMeasurementProvider(string providerName)
    {
      var providerType = GetMeasurementProviderType(providerName);
      if (!(Activator.CreateInstance(providerType) is IMeasurementProvider provider))
        throw new InvalidOperationException($"Failed to create an instance of \"{providerType}\" class.");
      return provider;
    }

    /// <summary>
    ///   Creates an enumeration of measurement logger instances by their names.
    /// </summary>
    /// <param name="loggerNamesString">
    ///   The comma-separated list of the measurement logger names to create instances for.
    ///   Can include full class names or class names with a <see cref="MeasurementLoggerClassNameSuffix" /> omitted.
    ///   The duplicates of class names will be removed.
    /// </param>
    /// <returns>
    ///   The created enumeration of measurement logger instances.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///   Failed to create a new instance for one of the resolved measurement logger classes.
    /// </exception>
    public static IEnumerable<IMeasurementLogger> CreateMeasurementLoggers(string loggerNamesString)
    {
      var loggerTypes = GetMeasurementLoggerTypes(loggerNamesString);
      foreach (var loggerType in loggerTypes)
      {
        if (!(Activator.CreateInstance(loggerType) is IMeasurementLogger logger))
          throw new InvalidOperationException($"Failed to create an instance of \"{loggerType}\" class.");
        yield return logger;
      }
    }

    /// <summary>
    ///   Creates an enumeration of measurement limiter instances by their names.
    /// </summary>
    /// <param name="limiterNamesString">
    ///   The comma-separated list of the measurement limiter names to create instances for.
    ///   Can include full class names or class names with a <see cref="MeasurementLimiterClassNameSuffix" /> omitted.
    ///   The duplicates of class names will be removed.
    /// </param>
    /// <returns>
    ///   The created enumeration of measurement limiter instances.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///   Failed to create a new instance for one of the resolved measurement limiter classes.
    /// </exception>
    public static IEnumerable<IMeasurementLimiter> CreateMeasurementLimiters(string limiterNamesString)
    {
      var limiterTypes = GetMeasurementLimiterTypes(limiterNamesString);
      foreach (var limiterType in limiterTypes)
      {
        if (!(Activator.CreateInstance(limiterType) is IMeasurementLimiter limiter))
          throw new InvalidOperationException($"Failed to create an instance of \"{limiterType}\" class.");
        yield return limiter;
      }
    }
  }
}
