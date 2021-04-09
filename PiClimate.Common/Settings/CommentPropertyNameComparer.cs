// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;

namespace PiClimate.Common.Settings
{
  /// <summary>
  ///   The string comparer class for sorting JSON names of properties representing comments.
  ///   The comment property names having the format <c>{propertyName}#{commentIndex}</c>, must appear before the
  ///   names of the commented properties.
  /// </summary>
  public class CommentPropertyNameComparer : StringComparer
  {
    /// <inheritdoc />
    public override int Compare(string? x, string? y)
    {
      x ??= string.Empty;
      y ??= string.Empty;

      if (Equals(x, y))
        return 0;
      if (x.StartsWith(y))
        return -1;
      if (y.StartsWith(x))
        return 1;

      return FromComparison(StringComparison.InvariantCultureIgnoreCase).Compare(x, y);
    }

    /// <inheritdoc />
    public override bool Equals(string? x, string? y) =>
      string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);

    /// <inheritdoc />
    public override int GetHashCode(string obj) => obj.GetHashCode();
  }
}
