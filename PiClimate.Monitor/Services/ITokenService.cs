// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace PiClimate.Monitor.Services
{
  /// <summary>
  ///   A common interface for token creation and validation services.
  /// </summary>
  public interface ITokenService
  {
    /// <summary>
    ///   Gets or sets the parameters used for token validation.
    /// </summary>
    TokenValidationParameters TokenValidationParameters { get; }

    /// <summary>
    ///   Creates a new token using the defined <see cref="TokenValidationParameters" />.
    /// </summary>
    /// <param name="lifetime">
    ///   A <see cref="TimeSpan" /> instance defining the lifetime of the token from the moment it has been issued.
    /// </param>
    /// <param name="claims">
    ///   An array of claims to be added to the token.
    ///   May be empty if no claims are required.
    /// </param>
    /// <returns>
    ///   A string representation of the created token.
    /// </returns>
    string CreateToken(TimeSpan lifetime, params Claim[] claims);

    /// <summary>
    ///   Validates the provided token using the defined <see cref="TokenValidationParameters" />.
    /// </summary>
    /// <param name="token">
    ///   A string representation of the token to be validated.
    /// </param>
    /// <returns>
    ///   A <see cref="ClaimsPrincipal" /> instance representing a user with the claims contained within
    ///   the provided token, or <c>null</c> if the token validation fails.
    /// </returns>
    ClaimsPrincipal? ValidateToken(string token);

    /// <summary>
    ///   Creates a new hash for the provided claims using the defined <see cref="TokenValidationParameters" />.
    ///   Opposite to tokens a hash represents only a signature generated for a token payload part.
    ///   Also it has no lifetime limitations.
    /// </summary>
    /// <param name="claims">
    ///   An array of claims to be hashed.
    ///   The order of claims does not matter as they will be internally sorted by their type names.
    ///   May be empty if no claims are required.
    /// </param>
    /// <returns>
    ///   A string representation of the created hash.
    /// </returns>
    string CreateHash(params Claim[] claims);

    /// <summary>
    ///   Checks whether the hash string is valid for the provided collection of claims.
    /// </summary>
    /// <param name="hash">
    ///   A hash string to be validated.
    /// </param>
    /// <param name="claims">
    ///   An array of claims to be validated.
    ///   The order of claims does not matter as they will be internally sorted by their type names.
    ///   May be empty if no claims are required.
    /// </param>
    /// <returns></returns>
    bool ValidateHash(string hash, params Claim[] claims);
  }
}
