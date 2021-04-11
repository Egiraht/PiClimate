// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

namespace PiClimate.Common
{
  /// <summary>
  ///   The static class containing the set of available API endpoints.
  /// </summary>
  public static class ApiEndpoints
  {
    /// <summary>
    ///   Defines the endpoint path for user signing in.
    /// </summary>
    public const string UserSignInEndpoint = "/Api/Auth/SignIn";

    /// <summary>
    ///   Defines the endpoint path for user signing out.
    /// </summary>
    public const string UserSignOutEndpoint = "/Api/Auth/SignOut";

    /// <summary>
    ///   Defines the endpoint path for getting a response with the specified status code.
    /// </summary>
    public const string StatusEndpoint = "/Api/Status";

    /// <summary>
    ///   Defines the endpoint path for acquiring filtered measurements data.
    /// </summary>
    public const string FilteredMeasurementsDataEndpoint = "/Api/Data";

    /// <summary>
    ///   Defines the endpoint path for acquiring latest measurements data.
    /// </summary>
    public const string LatestMeasurementsDataEndpoint = "/Api/Data/Latest";
  }
}
