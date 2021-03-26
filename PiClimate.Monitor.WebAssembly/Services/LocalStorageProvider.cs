// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace PiClimate.Monitor.WebAssembly.Services
{
  /// <summary>
  ///   The browser's local storage provider class.
  /// </summary>
  class LocalStorageProvider : IStorageProvider
  {
    /// <summary>
    ///   Defines the JavaScript method that sets the item in the local storage.
    /// </summary>
    private const string SetItemMethodName = "localStorage.setItem";

    /// <summary>
    ///   Defines the JavaScript method that gets the item from the local storage.
    /// </summary>
    private const string GetItemMethodName = "localStorage.getItem";

    /// <summary>
    ///   Defines the JavaScript method that removes the item from the local storage.
    /// </summary>
    private const string RemoveItemMethodName = "localStorage.removeItem";

    /// <summary>
    ///   Defines the JavaScript method that clears all items from the local storage.
    /// </summary>
    private const string ClearItemMethodName = "localStorage.clear";

    /// <summary>
    ///   The <see cref="JSRuntime" /> service instance used for JavaScript interoperation.
    /// </summary>
    private readonly IJSRuntime _jsRuntime;

    /// <summary>
    ///   Creates a new local storage provider instance.
    /// </summary>
    /// <param name="jsRuntime">
    ///   The <see cref="JSRuntime" /> service instance used for JavaScript interoperation.
    /// </param>
    public LocalStorageProvider(IJSRuntime jsRuntime) => _jsRuntime = jsRuntime;

    /// <inheritdoc />
    public async Task SetItemAsync(string key, string value) =>
      await _jsRuntime.InvokeVoidAsync(SetItemMethodName, key, value);

    /// <inheritdoc />
    public async Task<string?> GetItemAsync(string key) =>
      await _jsRuntime.InvokeAsync<string?>(GetItemMethodName, key);

    /// <inheritdoc />
    public async Task RemoveItemAsync(string key) =>
      await _jsRuntime.InvokeVoidAsync(RemoveItemMethodName, key);

    /// <inheritdoc />
    public async Task ClearAsync() =>
      await _jsRuntime.InvokeVoidAsync(ClearItemMethodName);
  }
}
