// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System.Threading.Tasks;

namespace PiClimate.Monitor.WebAssembly.Components
{
  /// <summary>
  ///   The common interface for updatable components.
  /// </summary>
  public interface IUpdatable
  {
    /// <summary>
    ///   Checks if the component is updating at the moment.
    /// </summary>
    bool IsUpdating { get; }

    /// <summary>
    ///   Checks if the last component update has failed.
    /// </summary>
    bool HasUpdateFailed { get; }

    /// <summary>
    ///   Forces the component to asynchronously update its state.
    /// </summary>
    Task UpdateAsync();
  }
}
