// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;

namespace PiClimate.Common.Settings
{
  /// <summary>
  ///   The setting comment attribute class.
  ///   Attaches a comment text to the decorated setting property.
  ///   Should be used within the classes derived from the <see cref="SettingsSection" /> class, so the comment message
  ///   can be serialized into JSON along with the decorated property value.
  ///   Multiple comment attributes can be used with a single property.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
  public class CommentAttribute : Attribute
  {
    /// <summary>
    ///   The setting comment message.
    /// </summary>
    public string Comment { get; }

    /// <summary>
    ///   Initializes a new attribute instance.
    /// </summary>
    /// <param name="comment">
    ///   The setting comment message to attach to the decorated property.
    /// </param>
    public CommentAttribute(string comment) => Comment = comment;
  }
}
