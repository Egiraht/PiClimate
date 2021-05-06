// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using PiClimate.Common.Settings;

namespace PiClimate.Monitor.Settings
{
  public partial class GlobalSettings
  {
    /// <summary>
    ///   Gets or sets the settings object shared with the WebAssembly browser client.
    /// </summary>
    [Comment("The settings section related to the browser client options.")]
    public ClientOptions ClientOptions { get; set; } = new();
  }
}
