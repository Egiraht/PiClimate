// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020-2021 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PiClimate.Monitor.WebAssembly.Components
{
  /// <summary>
  ///   A static class containing the extension methods fot the <see cref="HttpClient" /> service that simplify
  ///   JSON-encoded WebAPI communication.
  /// </summary>
  public static class HttpClientExtensions
  {
    /// <summary>
    ///   Sends the JSON-encoded HTTP request of type <typeparamref name="TRequest"/> to the WebAPI endpoint located
    ///   at <paramref name="uri" /> using the specified HTTP <paramref name="method" /> and deserializes
    ///   the JSON-encoded response to the object of type <typeparamref name="TResponse" />.
    ///   The method throws an exception on communication failure.
    /// </summary>
    /// <typeparam name="TRequest">
    ///   Type of the request body object to be serialized into JSON.
    /// </typeparam>
    /// <typeparam name="TResponse">
    ///   Type of the response body object to be deserialized from JSON.
    /// </typeparam>
    /// <param name="httpClient">
    ///   The <see cref="HttpClient" /> service instance used for communication.
    /// </param>
    /// <param name="uri">
    ///   The URI of the WebAPI endpoint to send the request to.
    /// </param>
    /// <param name="method">
    ///   The HTTP method used for the request.
    /// </param>
    /// <param name="requestData">
    ///   The request body data object to be serialized into JSON.
    /// </param>
    /// <returns>
    ///   The object from the response body deserialized from JSON, or <c>null</c> if the response body is empty.
    /// </returns>
    /// <exception cref="HttpRequestException">
    ///   Unable to send the HTTP request because of WebAPI endpoint connection failure.
    /// </exception>
    /// <exception cref="HttpResponseException">
    ///   The HTTP status code of the response is not a successful one (200-299).
    /// </exception>
    public static async Task<TResponse?> SendJsonAsync<TRequest, TResponse>(this HttpClient httpClient, string uri,
      HttpMethod method, TRequest? requestData)
      where TRequest : class
      where TResponse : class
    {
      var message = new HttpRequestMessage
      {
        Content = requestData != null
          ? JsonContent.Create(requestData,
            options: new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}})
          : null,
        Method = method,
        RequestUri = new Uri(uri.StartsWith("/") ? $"{httpClient.BaseAddress}{uri.Remove(0, 1)}" : uri)
      };

      var response = await httpClient.SendAsync(message);

      if (!response.IsSuccessStatusCode)
        throw new HttpResponseException(
          $"The HTTP request failed: [{(int) response.StatusCode}] {response.ReasonPhrase}",
          response);

      return response.Content.Headers.ContentLength > 0 ? await response.Content.ReadFromJsonAsync<TResponse?>() : null;
    }

    /// <summary>
    ///   Sends the HTTP request to the WebAPI endpoint located at <paramref name="uri" /> using the GET method and
    ///   deserializes the JSON-encoded response to the object of type <typeparamref name="TResponse" />.
    ///   The method throws an exception on communication failure.
    /// </summary>
    /// <typeparam name="TResponse">
    ///   Type of the response body object to be deserialized from JSON.
    /// </typeparam>
    /// <param name="httpClient">
    ///   The <see cref="HttpClient" /> service instance used for communication.
    /// </param>
    /// <param name="uri">
    ///   The URI of the WebAPI endpoint to send the request to.
    /// </param>
    /// <returns>
    ///   The object from the response body deserialized from JSON, or <c>null</c> if the response body is empty.
    /// </returns>
    /// <exception cref="HttpRequestException">
    ///   Unable to send the HTTP request because of WebAPI endpoint connection failure.
    /// </exception>
    /// <exception cref="HttpResponseException">
    ///   The HTTP status code of the response is not a successful one (200-299).
    /// </exception>
    public static async Task<TResponse?> GetJsonAsync<TResponse>(this HttpClient httpClient, string uri)
      where TResponse : class =>
      await SendJsonAsync<object, TResponse>(httpClient, uri, HttpMethod.Get, null);

    /// <summary>
    ///   Sends the JSON-encoded HTTP request of type <typeparamref name="TRequest"/> to the WebAPI endpoint located
    ///   at <paramref name="uri" /> using the POST method and deserializes the JSON-encoded response to the object
    ///   of type <typeparamref name="TResponse" />.
    ///   The method throws an exception on communication failure.
    /// </summary>
    /// <typeparam name="TRequest">
    ///   Type of the request body object to be serialized into JSON.
    /// </typeparam>
    /// <typeparam name="TResponse">
    ///   Type of the response body object to be deserialized from JSON.
    /// </typeparam>
    /// <param name="httpClient">
    ///   The <see cref="HttpClient" /> service instance used for communication.
    /// </param>
    /// <param name="uri">
    ///   The URI of the WebAPI endpoint to send the request to.
    /// </param>
    /// <param name="requestData">
    ///   The request body data object to be serialized into JSON.
    /// </param>
    /// <returns>
    ///   The object from the response body deserialized from JSON, or <c>null</c> if the response body is empty.
    /// </returns>
    /// <exception cref="HttpRequestException">
    ///   Unable to send the HTTP request because of WebAPI endpoint connection failure.
    /// </exception>
    /// <exception cref="HttpResponseException">
    ///   The HTTP status code of the response is not a successful one (200-299).
    /// </exception>
    public static async Task<TResponse?> PostJsonAsync<TRequest, TResponse>(this HttpClient httpClient, string uri,
      TRequest requestData)
      where TRequest : class
      where TResponse : class =>
      await SendJsonAsync<TRequest, TResponse>(httpClient, uri, HttpMethod.Post, requestData);

    /// <summary>
    ///   Sends the JSON-encoded HTTP request of type <typeparamref name="TRequest"/> to the WebAPI endpoint located
    ///   at <paramref name="uri" /> using the PUT method and deserializes the JSON-encoded response to the object
    ///   of type <typeparamref name="TResponse" />.
    ///   The method throws an exception on communication failure.
    /// </summary>
    /// <typeparam name="TRequest">
    ///   Type of the request body object to be serialized into JSON.
    /// </typeparam>
    /// <typeparam name="TResponse">
    ///   Type of the response body object to be deserialized from JSON.
    /// </typeparam>
    /// <param name="httpClient">
    ///   The <see cref="HttpClient" /> service instance used for communication.
    /// </param>
    /// <param name="uri">
    ///   The URI of the WebAPI endpoint to send the request to.
    /// </param>
    /// <param name="requestData">
    ///   The request body data object to be serialized into JSON.
    /// </param>
    /// <returns>
    ///   The object from the response body deserialized from JSON, or <c>null</c> if the response body is empty.
    /// </returns>
    /// <exception cref="HttpRequestException">
    ///   Unable to send the HTTP request because of WebAPI endpoint connection failure.
    /// </exception>
    /// <exception cref="HttpResponseException">
    ///   The HTTP status code of the response is not a successful one (200-299).
    /// </exception>
    public static async Task<TResponse?> PutJsonAsync<TRequest, TResponse>(this HttpClient httpClient, string uri,
      TRequest requestData)
      where TRequest : class
      where TResponse : class =>
      await SendJsonAsync<TRequest, TResponse>(httpClient, uri, HttpMethod.Put, requestData);

    /// <summary>
    ///   Sends the JSON-encoded HTTP request of type <typeparamref name="TRequest"/> to the WebAPI endpoint located
    ///   at <paramref name="uri" /> using the PATCH method and deserializes the JSON-encoded response to the object
    ///   of type <typeparamref name="TResponse" />.
    ///   The method throws an exception on communication failure.
    /// </summary>
    /// <typeparam name="TRequest">
    ///   Type of the request body object to be serialized into JSON.
    /// </typeparam>
    /// <typeparam name="TResponse">
    ///   Type of the response body object to be deserialized from JSON.
    /// </typeparam>
    /// <param name="httpClient">
    ///   The <see cref="HttpClient" /> service instance used for communication.
    /// </param>
    /// <param name="uri">
    ///   The URI of the WebAPI endpoint to send the request to.
    /// </param>
    /// <param name="requestData">
    ///   The request body data object to be serialized into JSON.
    /// </param>
    /// <returns>
    ///   The object from the response body deserialized from JSON, or <c>null</c> if the response body is empty.
    /// </returns>
    /// <exception cref="HttpRequestException">
    ///   Unable to send the HTTP request because of WebAPI endpoint connection failure.
    /// </exception>
    /// <exception cref="HttpResponseException">
    ///   The HTTP status code of the response is not a successful one (200-299).
    /// </exception>
    public static async Task<TResponse?> PatchJsonAsync<TRequest, TResponse>(this HttpClient httpClient, string uri,
      TRequest requestData)
      where TRequest : class
      where TResponse : class =>
      await SendJsonAsync<TRequest, TResponse>(httpClient, uri, HttpMethod.Patch, requestData);

    /// <summary>
    ///   Sends the JSON-encoded HTTP request of type <typeparamref name="TRequest"/> to the WebAPI endpoint located
    ///   at <paramref name="uri" /> using the DELETE method and deserializes the JSON-encoded response to the object
    ///   of type <typeparamref name="TResponse" />.
    ///   The method throws an exception on communication failure.
    /// </summary>
    /// <typeparam name="TRequest">
    ///   Type of the request body object to be serialized into JSON.
    /// </typeparam>
    /// <typeparam name="TResponse">
    ///   Type of the response body object to be deserialized from JSON.
    /// </typeparam>
    /// <param name="httpClient">
    ///   The <see cref="HttpClient" /> service instance used for communication.
    /// </param>
    /// <param name="uri">
    ///   The URI of the WebAPI endpoint to send the request to.
    /// </param>
    /// <param name="requestData">
    ///   The request body data object to be serialized into JSON.
    /// </param>
    /// <returns>
    ///   The object from the response body deserialized from JSON, or <c>null</c> if the response body is empty.
    /// </returns>
    /// <exception cref="HttpRequestException">
    ///   Unable to send the HTTP request because of WebAPI endpoint connection failure.
    /// </exception>
    /// <exception cref="HttpResponseException">
    ///   The HTTP status code of the response is not a successful one (200-299).
    /// </exception>
    public static async Task<TResponse?> DeleteJsonAsync<TRequest, TResponse>(this HttpClient httpClient, string uri,
      TRequest requestData)
      where TRequest : class
      where TResponse : class =>
      await SendJsonAsync<TRequest, TResponse>(httpClient, uri, HttpMethod.Delete, requestData);
  }
}
