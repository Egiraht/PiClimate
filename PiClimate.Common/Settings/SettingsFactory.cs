// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PiClimate.Common.Settings
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
    ///   Asynchronously generates a settings JSON file using the provided configuration builder object.
    /// </summary>
    /// <typeparam name="TSettings">
    ///   The type of the settings object defining all the properties that must present in the JSON file.
    /// </typeparam>
    /// <param name="builder">
    ///   The configuration builder object containing the settings values to be used when generating the JSON file.
    /// </param>
    /// <param name="filePath">
    ///   A path string locating the settings JSON file.
    /// </param>
    private static async Task GenerateSettingsFileAsync<TSettings>(this IConfigurationBuilder builder, string filePath)
      where TSettings : SettingsSection, new()
    {
      // Building the settings object.
      var settingsFromJson = builder.Build().Get<TSettings>() ?? new TSettings();

      // Serializing the settings object into a JSON document.
      await using var originalJsonStream = new MemoryStream();
      await JsonSerializer.SerializeAsync(originalJsonStream, settingsFromJson, new JsonSerializerOptions
      {
        WriteIndented = true,
        Converters = {new JsonStringEnumConverter()},
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
      });
      originalJsonStream.Seek(0, SeekOrigin.Begin);
      using var jsonDocument = await JsonDocument.ParseAsync(originalJsonStream);

      // (Re)creating the settings file and opening it as a stream.
      if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? ".");
      await using var jsonFileStream = File.Create(filePath);

      // Sorting the JSON document contents, converting the comments, and writing the results into the new settings JSON
      // file.
      await using var jsonWriter = new Utf8JsonWriter(jsonFileStream, new JsonWriterOptions
      {
        Indented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        SkipValidation = false
      });
      SortPropertiesAndConvertComments(jsonDocument.RootElement, jsonWriter);
      await jsonWriter.FlushAsync();
    }

    /// <summary>
    ///   Sorts the properties of the provided JSON element, converts the comment properties into single-line comments,
    ///   and writes the resulting JSON element using the provided JSON writer.
    /// </summary>
    /// <param name="element">
    ///   The JSON element object to process.
    /// </param>
    /// <param name="writer">
    ///   The JSON writer used for writing the sorted elements.
    /// </param>
    private static void SortPropertiesAndConvertComments(JsonElement element, Utf8JsonWriter writer)
    {
      if (element.ValueKind == JsonValueKind.Object)
      {
        writer.WriteStartObject();
        var sortedChildProperties = element.EnumerateObject()
          .OrderBy(property => property.Name, new CommentPropertyNameComparer());
        foreach (var objectChild in sortedChildProperties)
        {
          if (objectChild.Name.Contains('#'))
            writer.WriteCommentValue($" {objectChild.Value.ToString()} ");
          else
          {
            writer.WritePropertyName(objectChild.Name);
            SortPropertiesAndConvertComments(objectChild.Value, writer);
          }
        }

        writer.WriteEndObject();
      }
      else if (element.ValueKind == JsonValueKind.Array)
      {
        writer.WriteStartArray();
        foreach (var arrayChild in element.EnumerateArray())
          SortPropertiesAndConvertComments(arrayChild, writer);
        writer.WriteEndArray();
      }
      else if (element.ValueKind == JsonValueKind.String)
        writer.WriteStringValue(element.GetString());
      else if (element.ValueKind == JsonValueKind.Number)
        writer.WriteNumberValue(element.GetInt64());
      else if (element.ValueKind == JsonValueKind.True)
        writer.WriteBooleanValue(true);
      else if (element.ValueKind == JsonValueKind.False)
        writer.WriteBooleanValue(false);
      else
        writer.WriteNullValue();
    }

    /// <summary>
    ///   Asynchronously reads the settings object of the specified <typeparamref name="TSettings" /> type from the
    ///   settings JSON file and optional command line arguments.
    ///   The JSON file will be recreated using the initialized values.
    /// </summary>
    /// <typeparam name="TSettings">
    ///   The type of the settings object to be read.
    /// </typeparam>
    /// <param name="filePath">
    ///   A path string locating the settings JSON file.
    ///   If set to <c>null</c>, the <see cref="DefaultSettingsJsonFilePath" /> value will be used.
    /// </param>
    /// <param name="args">
    ///   An optional sequence of command line arguments overriding the values read from the JSON file.
    /// </param>
    /// <returns>
    ///   An awaitable task with the created settings object.
    /// </returns>
    public static async Task<TSettings> ReadSettingsAsync<TSettings>(string? filePath = null, params string[] args)
      where TSettings : SettingsSection, new()
    {
      // Getting the JSON file path.
      filePath = Path.GetFullPath(filePath ?? DefaultSettingsJsonFilePath);
      var builder = new ConfigurationBuilder();

      // Trying to read the JSON file with the global settings.
      builder.AddJsonFile(filePath, true);

      // (Re)generating the JSON file with the initialized settings values.
      await builder.GenerateSettingsFileAsync<TSettings>(filePath);

      // Overriding the settings with the values from the command line arguments.
      builder.AddCommandLine(args);
      return builder.Build().Get<TSettings>();
    }

    /// <summary>
    ///   Reads the settings object of the specified <typeparamref name="TSettings" /> type from the settings JSON file
    ///   and optional command line arguments.
    ///   The JSON file will be recreated using the initialized values.
    /// </summary>
    /// <returns>
    ///   The created settings object.
    /// </returns>
    /// <inheritdoc cref="ReadSettingsAsync{TSettings}(string?,string[])" />
    public static TSettings ReadSettings<TSettings>(string? filePath = null, params string[] args)
      where TSettings : SettingsSection, new() =>
      ReadSettingsAsync<TSettings>(filePath, args)
        .GetAwaiter()
        .GetResult();
  }
}
