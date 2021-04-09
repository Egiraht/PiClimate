// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace PiClimate.Common.Settings
{
  /// <summary>
  ///   The abstract class representing a settings section with serializable comments support.
  /// </summary>
  public abstract class SettingsSection
  {
    /// <summary>
    ///   The JSON-serializable dictionary of comments created using the derived class definition.
    ///   Dictionary keys represent comment names generated using the corresponding names of the public properties
    ///   decorated with the <see cref="CommentAttribute"/>, and have the following format:
    ///   <c>{propertyName}#{commentIndex}</c>.
    ///   Dictionary values contain the corresponding comment messages.
    /// </summary>
    /// <example>
    ///   The class definition
    ///   <code>
    ///     public class Settings : SettingsSection
    ///     {
    ///       [Comment("Comment message 1")]
    ///       [Comment("Comment message 2")]
    ///       public int Property { get; set; } = 0;
    ///     }
    ///   </code>
    ///   will be serialized into
    ///   <code>
    ///     {
    ///       "property": 0,
    ///       "property#0": "Comment message 1",
    ///       "property#1": "Comment message 2"
    ///     }
    ///   </code>
    /// </example>
    [JsonExtensionData]
    public IDictionary<string, object?> Comments { get; }

    /// <summary>
    ///   Initializes a new settings section instance.
    /// </summary>
    protected SettingsSection() => Comments = GenerateComments();

    /// <summary>
    ///   Creates a dictionary containing all comments defined for properties in the class.
    ///   Dictionary keys represent comment names generated using the corresponding names of the public properties
    ///   decorated with the <see cref="CommentAttribute"/>, and have the following format:
    ///   <c>{propertyName}#{commentIndex}</c>.
    ///   Dictionary values contain the corresponding comment messages.
    /// </summary>
    private IDictionary<string, object?> GenerateComments() => GetType()
      .GetProperties()
      .Where(property => property.GetCustomAttributes<CommentAttribute>().Any())
      .Aggregate(new SortedDictionary<string, object?>(), (comments, property) =>
      {
        var attributeComments = property.GetCustomAttributes<CommentAttribute>()
          .Select(attribute => attribute.Comment).ToArray();
        for (var index = 0; index < attributeComments.Length; index++)
          comments.Add($"{property.Name}#{index}", attributeComments[index]);
        return comments;
      });
  }
}
