// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PiClimate.Monitor.Sources;

namespace PiClimate.Monitor.Components
{
  /// <summary>
  ///   A static class containing the extension methods for service collections.
  /// </summary>
  public static class ServiceCollectionExtensions
  {
    /// <summary>
    ///   The common class name suffix of measurement sources.
    /// </summary>
    public const string MeasurementSourceClassNameSuffix = "Source";

    /// <summary>
    ///   Adds the specified measurement source as a singleton service of type <see cref="IMeasurementSource" />
    ///   within the provided service collection.
    /// </summary>
    /// <typeparam name="TMeasurementSource">
    ///   <see cref="Type" /> of the measurement source class to be set as a singleton service.
    /// </typeparam>
    /// <param name="services">
    ///   The service collection where the measurement source should be added.
    /// </param>
    /// <returns>
    ///   The modified service collection.
    /// </returns>
    public static IServiceCollection AddMeasurementSource<TMeasurementSource>(this IServiceCollection services)
      where TMeasurementSource : class, IMeasurementSource
    {
      services.AddSingleton<IMeasurementSource, TMeasurementSource>();
      return services;
    }

    /// <summary>
    ///   Adds the specified measurement source as a singleton service of type <see cref="IMeasurementSource" />
    ///   within the provided service collection.
    /// </summary>
    /// <param name="services">
    ///   The service collection where the measurement source should be added.
    /// </param>
    /// <param name="sourceType">
    ///   <see cref="Type" /> of the measurement source class to be set as a singleton service.
    /// </param>
    /// <exception cref="ArgumentException">
    ///   The provided <see cref="Type" /> is not a measurement source class type.
    /// </exception>
    /// <returns>
    ///   The modified service collection.
    /// </returns>
    public static IServiceCollection AddMeasurementSource(this IServiceCollection services,
      Type sourceType)
    {
      if (sourceType.GetInterface(nameof(IMeasurementSource)) == null)
        throw new ArgumentException(
          $"Type \"{sourceType.Name}\" is not a measurement source class type.");

      services.AddSingleton(typeof(IMeasurementSource), sourceType);
      return services;
    }

    /// <summary>
    ///   Adds the specified measurement source as a singleton service of type <see cref="IMeasurementSource" />
    ///   within the provided service collection.
    /// </summary>
    /// <param name="services">
    ///   The service collection where the measurement source should be added.
    /// </param>
    /// <param name="source">
    ///   The <see cref="IMeasurementSource" /> instance to be set as a singleton service.
    /// </param>
    /// <returns>
    ///   The modified service collection.
    /// </returns>
    public static IServiceCollection AddMeasurementSource(this IServiceCollection services,
      IMeasurementSource source)
    {
      services.AddSingleton(source);
      return services;
    }

    /// <summary>
    ///   Adds the specified measurement source as a singleton service of type <see cref="IMeasurementSource" />
    ///   within the provided service collection.
    /// </summary>
    /// <param name="services">
    ///   The service collection where the measurement source should be added.
    /// </param>
    /// <param name="sourceName">
    ///   The name of the measurement provider to create an instance for.
    ///   Can be a full class name or a class name with a <see cref="MeasurementSourceClassNameSuffix" /> omitted.
    /// </param>
    /// <exception cref="ArgumentException">
    ///   Cannot find a measurement source class with the provided name or the name is empty.
    /// </exception>
    /// <returns>
    ///   The modified service collection.
    /// </returns>
    public static IServiceCollection AddMeasurementSource(this IServiceCollection services,
      string sourceName)
    {
      sourceName = sourceName.Trim();
      if (string.IsNullOrEmpty(sourceName))
        throw new ArgumentException("No measurement source name is provided.");

      var measurementSourceType = Assembly.GetExecutingAssembly()
        .GetTypes()
        .FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IMeasurementSource)) &&
          (type.Name == sourceName ||
            type.Name == $"{sourceName}{MeasurementSourceClassNameSuffix}"));
      if (measurementSourceType == null)
        throw new ArgumentException(
          $"Cannot find a measurement source class with the name \"{sourceName}\".");

      return AddMeasurementSource(services, measurementSourceType);
    }
  }
}
