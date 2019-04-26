using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.MobCAT.Converters;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using Polly;

namespace Microsoft.MobCAT.Services
{
    public class BaseHttpService
    {
        string _baseApiUri;

        /// <summary>
        /// Gets the base API URL of the service.
        /// </summary>
        protected string BaseApiUrl => _baseApiUri;

        ISerializer<string> _serializer;

        /// <summary>
        /// Gets or sets the string serializer the service will use.
        /// </summary>
        /// <value>The serializer.</value>
        protected virtual ISerializer<string> Serializer
        {
            get
            {
                if (_serializer == null)
                {
                    _serializer = ServiceContainer.Resolve<ISerializer<string>>(true);
                    if (_serializer == null)
                    {
                        Logger.Debug("[BaseHttpService] An ISerializer<string> has not been registered, falling back to a JsonSerializer." +
                                     " For improved performance, you could use the JsonNetSerializer in the MobCat.Core.Json package.");
                        _serializer = new JsonSerializer();
                    }
                }

                return _serializer;
            }
            set => _serializer = value;
        }

        IStreamSerializer _streamSerializer;

        /// <summary>
        /// Gets or sets the optional stream serializer the service could use.
        /// </summary>
        /// <value>The stream serializer.</value>
        protected virtual IStreamSerializer StreamSerializer
        {
            get
            {
                if (_streamSerializer == null)
                {
                    _streamSerializer = ServiceContainer.Resolve<IStreamSerializer>(true);
                }

                return _streamSerializer;
            }
            set => _streamSerializer = value;
        }

        protected HttpClient _client;

        /// <summary>
        /// This message is broadcast when a HttpResponseMessage is received from the service. This can be especially useful for debugging purposes. 
        /// </summary>
        public Action<HttpResponseMessage> HttpResponseMessageReceived { get; set; }

        /// <summary>
        /// This message is broadcast when a HttpRequestMessage is sent from the service. This can be especially useful for debugging purposes. 
        /// </summary>
        public Action<HttpRequestMessage> HttpRequestMessageSent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.MobCat.Core.Services.BaseHttpService"/> class.
        /// </summary>
        /// <param name="baseApiUri">Base API URI.</param>
        /// <param name="handler">Handler.</param>
        protected BaseHttpService(string baseApiUri, HttpMessageHandler handler = null)
        {
            if (handler != null)
                _client = new HttpClient(handler);
            else
                _client = new HttpClient();

            _client.DefaultRequestHeaders.Add("Accept", "application/json");

            _baseApiUri = baseApiUri;
        }

        /// <summary>
        /// Sets the default request headers for the HttpClient.
        /// </summary>
        /// <param name="shouldClear">If set to <c>true</c> should clear before setting new headers.</param>
        /// <param name="headers">Headers to set.</param>
        protected void SetDefaultRequestHeaders(bool shouldClear, params KeyValuePair<string, string>[] headers)
        {
            if (shouldClear)
                _client.DefaultRequestHeaders.Clear();

            foreach (var kvp in headers)
                _client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Modifies the http client.
        /// </summary>
        /// <param name="modifyAction">Modify action.</param>
        protected void ModifyHttpClient(Action<HttpClient> modifyAction) => modifyAction?.Invoke(_client);

        /// <summary>
        /// Starts a Delete call to the service.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        protected virtual Task<T> DeleteAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken), Task<Action<HttpRequestMessage>> modifyRequest = null) =>
            SendAsync<T>(HttpMethod.Delete, requestUri, cancellationToken, modifyRequest);

        /// <summary>
        /// Starts a Get call to the service.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        protected virtual Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken), Task<Action<HttpRequestMessage>> modifyRequest = null) =>
            SendAsync<T>(HttpMethod.Get, requestUri, cancellationToken, modifyRequest);

        /// <summary>
        /// Starts a Put call to the service.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        protected virtual Task<T> PutAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken), Task<Action<HttpRequestMessage>> modifyRequest = null) =>
            SendAsync<T>(HttpMethod.Put, requestUri, cancellationToken, modifyRequest);

        /// <summary>
        /// Starts a Put call to the service.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="obj">Payload of request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        /// <typeparam name="K">The type of the payload.</typeparam>
        protected virtual Task<T> PutAsync<T, K>(string requestUri, K obj, CancellationToken cancellationToken = default(CancellationToken), Task<Action<HttpRequestMessage>> modifyRequest = null)
        {
            var jsonRequest = obj != null ? Serializer.Serialize(obj) : null;
            return SendAsync<T>(HttpMethod.Put, requestUri, cancellationToken, modifyRequest, jsonRequest);
        }

        /// <summary>
        /// Starts a Post call to the service.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <typeparam name="T">The type of response..</typeparam>
        protected virtual Task<T> PostAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken), Task<Action<HttpRequestMessage>> modifyRequest = null) =>
            SendAsync<T>(HttpMethod.Post, requestUri, cancellationToken, modifyRequest);

        /// <summary>
        /// Starts a Post call to the service.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="obj">Payload of request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        /// <typeparam name="K">The type of the payload.</typeparam>
        protected virtual Task<T> PostAsync<T, K>(string requestUri, K obj, CancellationToken cancellationToken = default(CancellationToken), Task<Action<HttpRequestMessage>> modifyRequest = null)
        {
            var jsonRequest = obj != null ? Serializer.Serialize(obj) : null;
            return SendAsync<T>(HttpMethod.Post, requestUri, cancellationToken, modifyRequest, jsonRequest);
        }

        /// <summary>
        /// Starts a Post call to the service.
        /// </summary>
        /// <returns>A task.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="obj">Payload of request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <typeparam name="T">The type of payload.</typeparam>
        protected virtual async Task PostAsync<T>(string requestUri, T obj, CancellationToken cancellationToken = default(CancellationToken), Task<Action<HttpRequestMessage>> modifyRequest = null)
        {
            var jsonRequest = obj != null ? Serializer.Serialize(obj) : null;
            var response = await SendAsync(HttpMethod.Post, requestUri, cancellationToken, modifyRequest, jsonRequest).ConfigureAwait(false);
            response?.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Starts a request to the service
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestType">Request type.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <param name="jsonRequest">Json request.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        protected virtual async Task<T> SendAsync<T>(HttpMethod requestType, string requestUri, CancellationToken cancellationToken = default(CancellationToken),
            Task<Action<HttpRequestMessage>> modifyRequest = null, string jsonRequest = null)
        {
            T result = default(T);

            try
            {
                result = await SendAndDeserialize<T>(requestType, requestUri, cancellationToken, modifyRequest, jsonRequest);
            }
            catch (Exception exc)
            {
                Logger.Warn(exc);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Starts a request and deserializes the response.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestType">Request type.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <param name="jsonRequest">Json request.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        protected async Task<T> SendAndDeserialize<T>(HttpMethod requestType, string requestUri, CancellationToken cancellationToken = default(CancellationToken),
            Task<Action<HttpRequestMessage>> modifyRequest = null, string jsonRequest = null)
        {
            T result = default(T);

            var response = await SendAsync(requestType, requestUri, cancellationToken, modifyRequest, jsonRequest).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                if (response != null)
                {
                    if (StreamSerializer != null)
                    {
                        using (var jsonStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                            result = StreamSerializer.Read<T>(jsonStream);
                    }
                    else
                    {
                        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        if (!string.IsNullOrWhiteSpace(json))
                            result = Serializer.Deserialize<T>(json);
                    }
                }
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }

            return result;
        }

        /// <summary>
        /// Starts a request to the service
        /// </summary>
        /// <returns>Task with return type of HttpResponseMessage.</returns>
        /// <param name="requestType">Request type.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <param name="jsonRequest">Json request.</param>
        protected async Task<HttpResponseMessage> SendAsync(HttpMethod requestType, string requestUri, CancellationToken cancellationToken, Task<Action<HttpRequestMessage>> modifyRequest = null,
            string jsonRequest = null)
        {
            var request = new HttpRequestMessage(requestType, new Uri($"{_baseApiUri}{requestUri}"));

            if (jsonRequest != null)
                request.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            if (modifyRequest != null)
                (await modifyRequest)?.Invoke(request);

            HttpRequestMessageSent?.Invoke(request);

            var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);

            HttpResponseMessageReceived?.Invoke(response);

            return response;
        }
    }
}