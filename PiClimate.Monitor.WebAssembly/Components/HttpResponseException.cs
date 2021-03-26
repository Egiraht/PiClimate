// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Net.Http;

// ReSharper disable InvalidXmlDocComment
namespace PiClimate.Monitor.WebAssembly.Components
{
  /// <summary>
  ///   An exception class representing an HTTP response error.
  /// </summary>
  public class HttpResponseException : Exception
  {
    /// <summary>
    ///   Gets the <see cref="HttpResponseMessage" /> object containing the HTTP response that caused the exception.
    /// </summary>
    public HttpResponseMessage? Response { get; }

    /// <inheritdoc cref="Exception()" />
    public HttpResponseException()
    {
    }

    /// <inheritdoc cref="Exception(string)" />
    public HttpResponseException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="Exception(string, Exception)" />
    public HttpResponseException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <inheritdoc cref="Exception(string)" />
    /// <param name="response">
    ///   The <see cref="HttpResponseMessage" /> object containing the HTTP response that caused the exception.
    /// </param>
    public HttpResponseException(string message, HttpResponseMessage response) : this(message)
    {
      Response = response;
    }

    /// <inheritdoc cref="Exception(string, Exception)" />
    /// <param name="response">
    ///   The <see cref="HttpResponseMessage" /> object containing the HTTP response that caused the exception.
    /// </param>
    public HttpResponseException(string message, HttpResponseMessage response, Exception innerException) :
      this(message, innerException)
    {
      Response = response;
    }
  }
}
