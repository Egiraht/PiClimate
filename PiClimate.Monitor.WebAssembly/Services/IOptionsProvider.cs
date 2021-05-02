// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Threading.Tasks;

namespace PiClimate.Monitor.WebAssembly.Services
{
  /// <summary>
  ///   The common interface for client-side options provider services.
  /// </summary>
  /// <typeparam name="TOptions">
  ///   The type of the options object.
  /// </typeparam>
  public interface IOptionsProvider<TOptions>
  {
    /// <summary>
    ///   Asynchronously gets the options object.
    /// </summary>
    /// <returns>
    ///   The options object of type <typeparamref name="TOptions" />.
    /// </returns>
    Task<TOptions> GetOptionsAsync();
  }
}
