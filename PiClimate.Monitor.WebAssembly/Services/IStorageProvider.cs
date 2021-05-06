// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Threading.Tasks;

namespace PiClimate.Monitor.WebAssembly.Services
{
  /// <summary>
  ///   An interface for data storage providers.
  /// </summary>
  public interface IStorageProvider
  {
    /// <summary>
    ///   Asynchronously sets the data item in the storage by its <paramref name="key" />.
    /// </summary>
    /// <param name="key">
    ///   The key string identifying the data item in the storage.
    /// </param>
    /// <param name="value">
    ///   The string value of the data item to be stored.
    /// </param>
    Task SetItemAsync(string key, string value);

    /// <summary>
    ///   Asynchronously gets the data item from the storage by its <paramref name="key" />.
    /// </summary>
    /// <param name="key">
    ///   The key string identifying the data item in the storage.
    /// </param>
    /// <returns>
    ///   The stored data item string value if the <paramref name="key" /> exists in the storage, or <c>null</c>
    ///   otherwise.
    /// </returns>
    Task<string?> GetItemAsync(string key);

    /// <summary>
    ///   Asynchronously removes the item from the storage by its <paramref name="key" />.
    /// </summary>
    /// <param name="key">
    ///   The key string identifying the data item in the storage.
    /// </param>
    Task RemoveItemAsync(string key);

    /// <summary>
    ///   Asynchronously clears all the items from the storage.
    /// </summary>
    Task ClearAsync();
  }
}
