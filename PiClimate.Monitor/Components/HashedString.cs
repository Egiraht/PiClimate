// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PiClimate.Monitor.Components
{
  /// <summary>
  ///   The class representing a hash-encrypted string value.
  /// </summary>
  public class HashedString
  {
    /// <summary>
    ///   Defines the formatted hashed string format.
    /// </summary>
    public const string HashedStringFormat = "encrypted:{0}:{1}";

    /// <summary>
    ///   Defines the formatted hashed string regular expression pattern.
    /// </summary>
    public const string HashedStringRegex = @"^encrypted:([\w.]+):([A-Za-z0-9+/=]+)$";

    /// <summary>
    ///   Defines the default hashing algorithm name.
    /// </summary>
    public const string DefaultAlgorithmName = nameof(HMACSHA256);

    /// <summary>
    ///   Defines the default hashing key value.
    /// </summary>
    public const string DefaultKey = "";

    /// <summary>
    ///   Gets the hashing algorithm name that was used for this string encryption.
    /// </summary>
    public string AlgorithmName { get; }

    /// <summary>
    ///   Gets the base64-encoded hash value.
    /// </summary>
    public string Hash { get; }

    /// <summary>
    ///   Gets the formatted string value for the current hashed string instance.
    /// </summary>
    public string FormattedValue => string.Format(HashedStringFormat, AlgorithmName, Hash);

    /// <summary>
    ///   Creates a new hashed string.
    /// </summary>
    /// <param name="value">
    ///   The value string that must be hashed.
    /// </param>
    /// <param name="key">
    ///   The key string used for hash value customization.
    /// </param>
    /// <param name="algorithmName">
    ///   The hashing algorithm name used for string encryption.
    ///   The name must represent a short or full class name of one of the available <i>System.Security.Cryptography</i>
    ///   namespace classes.
    ///   See the <see cref="HMAC.Create(string)" /> method documentation for acceptable parameter values.
    /// </param>
    public HashedString(string value, string key = DefaultKey, string algorithmName = DefaultAlgorithmName)
    {
      if (TryParse(value, out var valueAlgorithmName, out var valueHash))
      {
        AlgorithmName = valueAlgorithmName;
        Hash = valueHash;
      }
      else
      {
        AlgorithmName = algorithmName;
        Hash = CreateHashedValue(value, key, algorithmName);
      }
    }

    /// <summary>
    ///   Tries to parse the formatted hashed string value.
    /// </summary>
    /// <param name="value">
    ///   The formatted hashed string value.
    /// </param>
    /// <param name="algorithmName">
    ///   Output string containing the hashing algorithm name that was used for encryption.
    /// </param>
    /// <param name="hash">
    ///   Output string containing the base64-encoded hash part.
    /// </param>
    /// <returns>
    ///   <c>true</c> if value parsing succeeds, otherwise <c>false</c>.
    /// </returns>
    public static bool TryParse(string value, out string algorithmName, out string hash)
    {
      algorithmName = string.Empty;
      hash = string.Empty;

      var match = Regex.Match(value, HashedStringRegex);
      if (!match.Success)
        return false;

      algorithmName = match.Groups[1].Value;
      hash = match.Groups[2].Value;
      return true;
    }

    /// <summary>
    ///   Creates a hash string.
    /// </summary>
    /// <param name="value">
    ///   The original string value.
    /// </param>
    /// <param name="key">
    ///   The key string used for hash value customization.
    /// </param>
    /// <param name="algorithmName">
    ///   The hashing algorithm name used for string encryption.
    ///   The name must represent a short or full class name of one of the available <i>System.Security.Cryptography</i>
    ///   namespace classes.
    ///   See the <see cref="HMAC.Create(string)" /> method documentation for acceptable parameter values.
    /// </param>
    /// <returns>
    ///   A base64-encoded hash string.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///   The provided hashing algorithm name is not a valid value.
    /// </exception>
    private static string CreateHashedValue(string value, string key, string algorithmName)
    {
      using var hmac = HMAC.Create(algorithmName) ?? throw new ArgumentException(
        $"Cannot create a new HMAC instance with algorithm name \"{algorithmName}\".");
      hmac.Key = Encoding.UTF8.GetBytes(key);
      return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(value)));
    }

    /// <summary>
    ///   Validates if the current hashed string instance matches the value of the specified original string, given the
    ///   original key string is identical.
    /// </summary>
    /// <param name="value">
    ///   The original string value.
    /// </param>
    /// <param name="key">
    ///   The original key string used for hash value customization.
    /// </param>
    /// <returns>
    ///   <c>true</c> if value validation succeeds, otherwise <c>false</c>.
    /// </returns>
    public bool ValidateOriginalValue(string value, string key = DefaultKey) =>
      Hash.Equals(CreateHashedValue(value, key, AlgorithmName));

    /// <inheritdoc cref="FormattedValue" />
    public override string ToString() => FormattedValue;

    /// <summary>
    ///   Implicitly casts a hashed string instance into a normal formatted string.
    /// </summary>
    public static implicit operator string(HashedString hashedString) => hashedString.ToString();
  }

  /// <summary>
  ///   The static class containing extension methods for hashed strings.
  /// </summary>
  public static class HashedStringExtensions
  {
    /// <summary>
    ///   Converts the specified string into a hashed string.
    /// </summary>
    /// <param name="value">
    ///   The value string that must be hashed.
    /// </param>
    /// <param name="key">
    ///   The key string used for hash value customization.
    /// </param>
    /// <param name="algorithmName">
    ///   The hashing algorithm name used for string encryption.
    ///   The name must represent a short or full class name of one of the available <i>System.Security.Cryptography</i>
    ///   namespace classes.
    ///   See the <see cref="HMAC.Create(string)" /> method documentation for acceptable parameter values.
    /// </param>
    /// <returns>
    ///   A new hashed string instance.
    /// </returns>
    public static HashedString ToHashedString(this string value, string key = HashedString.DefaultKey,
      string algorithmName = HashedString.DefaultAlgorithmName) => new(value, key, algorithmName);
  }
}
