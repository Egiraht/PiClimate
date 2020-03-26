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
    /// <param name="measurementSourceClassType">
    ///   <see cref="Type" /> of the measurement source class to be set as a singleton service.
    /// </param>
    /// <exception cref="ArgumentException">
    ///   The provided <see cref="Type" /> is not a measurement source class type.
    /// </exception>
    /// <returns>
    ///   The modified service collection.
    /// </returns>
    public static IServiceCollection AddMeasurementSource(this IServiceCollection services,
      Type measurementSourceClassType)
    {
      if (measurementSourceClassType.GetInterface(nameof(IMeasurementSource)) == null)
        throw new ArgumentException(
          $"Type \"{measurementSourceClassType.Name}\" is not a measurement source class type.");

      services.AddSingleton(typeof(IMeasurementSource), measurementSourceClassType);
      return services;
    }

    /// <summary>
    ///   Adds the specified measurement source as a singleton service of type <see cref="IMeasurementSource" />
    ///   within the provided service collection.
    /// </summary>
    /// <param name="services">
    ///   The service collection where the measurement source should be added.
    /// </param>
    /// <param name="measurementSource">
    ///   The <see cref="IMeasurementSource" /> instance to be set as a singleton service.
    /// </param>
    /// <returns>
    ///   The modified service collection.
    /// </returns>
    public static IServiceCollection AddMeasurementSource(this IServiceCollection services,
      IMeasurementSource measurementSource)
    {
      services.AddSingleton(measurementSource);
      return services;
    }

    /// <summary>
    ///   Adds the specified measurement source as a singleton service of type <see cref="IMeasurementSource" />
    ///   within the provided service collection.
    /// </summary>
    /// <param name="services">
    ///   The service collection where the measurement source should be added.
    /// </param>
    /// <param name="measurementSourceClassName">
    ///   The class name of the measurement source to be set as a singleton service.
    ///   The class name suffix (<see cref="MeasurementSourceClassNameSuffix" />) may be omitted.
    /// </param>
    /// <exception cref="ArgumentException">
    ///   Cannot find a measurement source class with the provided name.
    /// </exception>
    /// <returns>
    ///   The modified service collection.
    /// </returns>
    public static IServiceCollection AddMeasurementSource(this IServiceCollection services,
      string measurementSourceClassName)
    {
      if (string.IsNullOrWhiteSpace(measurementSourceClassName))
        return services;

      var measurementSourceType = Assembly.GetExecutingAssembly()
        .GetTypes()
        .FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IMeasurementSource)) &&
          (type.Name == measurementSourceClassName ||
            type.Name == $"{measurementSourceClassName}{MeasurementSourceClassNameSuffix}"));
      if (measurementSourceType == null)
        throw new ArgumentException(
          $"Cannot find a measurement source class with the name \"{measurementSourceClassName}\".");

      return AddMeasurementSource(services, measurementSourceType);
    }
  }
}
