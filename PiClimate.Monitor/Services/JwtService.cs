// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PiClimate.Monitor.Settings;

namespace PiClimate.Monitor.Services
{
  /// <summary>
  ///   A service class for JWT type tokens creation and validation.
  /// </summary>
  public class JwtService : ITokenService
  {
    /// <summary>
    ///   The global settings of the server.
    /// </summary>
    private readonly GlobalSettings _settings;

    /// <summary>
    ///   Defines the algorithm type used for JWT signing.
    /// </summary>
    private const string SigningAlgorithm = SecurityAlgorithms.HmacSha256;

    /// <inheritdoc />
    public TokenValidationParameters TokenValidationParameters { get; }

    /// <summary>
    ///   The JWT service constructor.
    /// </summary>
    /// <param name="settings">
    ///   The global settings of the server.
    ///   Provided via dependency injection.
    /// </param>
    public JwtService(GlobalSettings settings)
    {
      _settings = settings;
      TokenValidationParameters = new TokenValidationParameters
      {
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role,
        ValidIssuer = $"{nameof(PiClimate)}.{nameof(Monitor)}.{nameof(Monitor)}",
        ValidAudience = $"{nameof(PiClimate)}.{nameof(Monitor)}.{nameof(WebAssembly)}",
        IssuerSigningKey =
          new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.AuthenticationOptions.TokenSigningKey)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(settings.AuthenticationOptions.ClockSkewPeriod),
      };
    }

    /// <inheritdoc />
    public string CreateToken(TimeSpan lifetime, params Claim[] claims)
    {
      var token = new JwtSecurityToken(TokenValidationParameters.ValidIssuer, TokenValidationParameters.ValidAudience,
        claims.OrderBy(claim => claim.Type), DateTime.Now, DateTime.Now + lifetime.Duration(),
        new SigningCredentials(TokenValidationParameters.IssuerSigningKey, SigningAlgorithm));
      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <inheritdoc />
    public ClaimsPrincipal? ValidateToken(string token)
    {
      try
      {
        return new JwtSecurityTokenHandler().ValidateToken(token, TokenValidationParameters, out _);
      }
      catch
      {
        return null;
      }
    }

    /// <inheritdoc />
    public string CreateHash(params Claim[] claims)
    {
      var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_settings.AuthenticationOptions.HashSigningKey)), SigningAlgorithm);
      var payload = new JwtPayload(claims.OrderBy(claim => claim.Type));
      return JwtTokenUtilities.CreateEncodedSignature(payload.Base64UrlEncode(), signingCredentials);
    }

    /// <inheritdoc />
    public bool ValidateHash(string hash, params Claim[] claims) => hash == CreateHash(claims);
  }
}
