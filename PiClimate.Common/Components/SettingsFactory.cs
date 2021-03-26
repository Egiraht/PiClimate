// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PiClimate.Common.Components
{
  /// <summary>
  ///   The factory class that creates new settings objects.
  /// </summary>
  public static class SettingsFactory
  {
    /// <summary>
    ///   Defines the default settings JSON file name.
    /// </summary>
    public const string DefaultSettingsJsonFilePath = "./Settings.json";

    /// <summary>
    ///   Asynchronously reads the settings object of the specified <typeparamref name="TSettings" /> type from the
    ///   JSON settings file and optional command line arguments.
    ///   The JSON file will be recreated using the initialized values.
    /// </summary>
    /// <typeparam name="TSettings">
    ///   The type of the settings object to be read.
    /// </typeparam>
    /// <param name="filePath">
    ///   The path string to the JSON settings file to read.
    ///   If set to <c>null</c>, the <see cref="DefaultSettingsJsonFilePath" /> value will be used.
    /// </param>
    /// <param name="args">
    ///   An optional sequence of command line arguments overriding the values read from the JSON file.
    /// </param>
    /// <returns>
    ///   An awaitable task with the created settings object.
    /// </returns>
    public static async Task<TSettings> ReadSettingsAsync<TSettings>(string? filePath = null, params string[] args)
      where TSettings : new()
    {
      // Getting the JSON file path.
      filePath = Path.GetFullPath(filePath ?? DefaultSettingsJsonFilePath);
      var builder = new ConfigurationBuilder();

      // Trying to read the JSON file with the global settings.
      builder.AddJsonFile(filePath, true);

      // Recreating the JSON file with the initialized values.
      var settingsFromJson = builder.Build().Get<TSettings>() ?? new TSettings();
      if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? ".");
      await using (var recreatedJsonFile = File.CreateText(filePath))
        await JsonSerializer.SerializeAsync(recreatedJsonFile.BaseStream, settingsFromJson,
          new JsonSerializerOptions
          {
            WriteIndented = true,
            Converters = {new JsonStringEnumConverter()}
          });

      // Overriding the settings with the values from the command line arguments.
      builder.AddCommandLine(args);
      return builder.Build().Get<TSettings>();
    }

    /// <summary>
    ///   Reads the settings object of the specified <typeparamref name="TSettings" /> type from the JSON settings file
    ///   and optional command line arguments.
    ///   The JSON file will be recreated using the initialized values.
    /// </summary>
    /// <returns>
    ///   The created settings object.
    /// </returns>
    /// <inheritdoc cref="ReadSettingsAsync{TSettings}(string?,string[])" />
    public static TSettings ReadSettings<TSettings>(string? filePath = null, params string[] args)
      where TSettings : new() =>
      ReadSettingsAsync<TSettings>(filePath, args)
        .GetAwaiter()
        .GetResult();
  }
}
