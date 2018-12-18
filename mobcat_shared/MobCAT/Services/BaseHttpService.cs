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
    // TODO: 
    // 1. Handle retry - basic starting point added
    // 2. Make more generic than just JSON - DONE
    // 3. Make connectivity aware - separate base class?
    // 4. Enable use of cache
    // 5. Support auth workflow - separate base class?
    public class BaseHttpService
    {
        private const string AcceptHeaderKey = "Accept";

        private string _baseApiUri;
        private IStreamSerializer _streamSerializer;
        private ISerializer<string> _serializer;

        /// <summary>
        /// Gets the base API URL of the service.
        /// </summary>
        protected string BaseApiUrl => _baseApiUri;

        /// <summary>
        /// Gets or sets the string serializer the service will use.
        /// </summary>
        /// <value>The serializer.</value>
        /// <remarks>Defaults to a basic JSON serializer.</remarks>
        protected virtual ISerializer<string> Serializer
        {
            get => _serializer ?? (_serializer = new JsonSerializer());
            set
            {
                _serializer = value;
                _client?.DefaultRequestHeaders.Add(AcceptHeaderKey, StreamSerializer?.MediaType ?? value.MediaType);
            }
        }

        protected virtual Policy GetRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(5, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        /// <summary>
        /// Gets or sets the optional stream serializer the service could use.
        /// </summary>
        /// <value>The stream serializer.</value>
        protected virtual IStreamSerializer StreamSerializer
        {
            get => _streamSerializer;

            set 
            {
                _streamSerializer = value;
                _client?.DefaultRequestHeaders.Add(AcceptHeaderKey, value?.MediaType ?? Serializer.MediaType);
            } 
        }

        protected HttpClient _client;


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

            _client?.DefaultRequestHeaders.Add(AcceptHeaderKey, StreamSerializer?.MediaType ?? Serializer.MediaType);

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
        /// <param name="deserializeResponse">Indicates whether the reponse should be deserialized or returned directly.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        protected virtual Task<T> DeleteAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken), Action<HttpRequestMessage> modifyRequest = null, bool deserializeResponse = true) => SendWithRetryAsync<T>(HttpMethod.Delete, requestUri, cancellationToken, modifyRequest, null, deserializeResponse);

        /// <summary>
        /// Starts a Get call to the service.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <param name="deserializeResponse">Indicates whether the reponse should be deserialized or returned directly.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        protected virtual Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken), Action<HttpRequestMessage> modifyRequest = null, bool deserializeResponse = true) => SendWithRetryAsync<T>(HttpMethod.Get, requestUri, cancellationToken, modifyRequest, null, deserializeResponse);

        /// <summary>
        /// Starts a Put call to the service.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <param name="deserializeResponse">Indicates whether the reponse should be deserialized or returned directly.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        protected virtual Task<T> PutAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken), Action<HttpRequestMessage> modifyRequest = null, bool deserializeResponse = true) => SendWithRetryAsync<T>(HttpMethod.Put, requestUri, cancellationToken, modifyRequest, null, deserializeResponse);

        /// <summary>
        /// Starts a Put call to the service.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="obj">Payload of request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <param name="deserializeResponse">Indicates whether the reponse should be deserialized or returned directly.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        /// <typeparam name="K">The type of the payload.</typeparam>
        protected virtual Task<T> PutAsync<T, K>(string requestUri, K obj, CancellationToken cancellationToken = default(CancellationToken), Action<HttpRequestMessage> modifyRequest = null, bool deserializeResponse = true)
        {
            var serializedContent = !obj.Equals(default(K)) ? Serializer.Serialize(obj) : null;
            return SendWithRetryAsync<T>(HttpMethod.Put, requestUri, cancellationToken, modifyRequest, serializedContent, deserializeResponse);
        }

        /// <summary>
        /// Starts a Post call to the service.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <param name="deserializeResponse">Indicates whether the reponse should be deserialized or returned directly.</param>
        /// <typeparam name="T">The type of response..</typeparam>
        protected virtual Task<T> PostAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken), Action<HttpRequestMessage> modifyRequest = null, bool deserializeResponse = true) => SendWithRetryAsync<T>(HttpMethod.Post, requestUri, cancellationToken, modifyRequest, null, deserializeResponse);

        /// <summary>
        /// Starts a Post call to the service.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="obj">Payload of request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <param name="deserializeResponse">Indicates whether the reponse should be deserialized or returned directly.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        /// <typeparam name="K">The type of the payload.</typeparam>
        protected virtual Task<T> PostAsync<T, K>(string requestUri, K obj, CancellationToken cancellationToken = default(CancellationToken), Action<HttpRequestMessage> modifyRequest = null, bool deserializeResponse = true)
        {
            var serializedContent = !obj.Equals(default(K)) ? Serializer.Serialize(obj) : null;
            return SendWithRetryAsync<T>(HttpMethod.Post, requestUri, cancellationToken, modifyRequest, serializedContent, deserializeResponse);
        }

        /// <summary>
        /// Starts a Post call to the service.
        /// </summary>
        /// <returns>A task.</returns>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="obj">Payload of request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <param name="deserializeResponse">Indicates whether the reponse should be deserialized or returned directly.</param>
        /// <typeparam name="T">The type of payload.</typeparam>
        protected virtual async Task PostAsync<T>(string requestUri, T obj, CancellationToken cancellationToken = default(CancellationToken), Action<HttpRequestMessage> modifyRequest = null, bool deserializeResponse = true)
        {
            var serializedContent = !obj.Equals(default(T)) ? Serializer.Serialize(obj) : null;
            var response = await SendWithRetryAsync(HttpMethod.Post, requestUri, cancellationToken, modifyRequest, serializedContent).ConfigureAwait(false);
            response?.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Starts a request to the service with retry.
        /// </summary>
        /// <returns>Task with return type of HttpResponseMessage.</returns>
        /// <param name="requestType">Request type.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequestAction">Modify request action.</param>
        /// <param name="jsonRequest">Json request.</param>
        protected virtual Task<HttpResponseMessage> SendWithRetryAsync(HttpMethod requestType, string requestUri, CancellationToken cancellationToken = default(CancellationToken), Action<HttpRequestMessage> modifyRequestAction = null, string jsonRequest = null)
        {
            return GetRetryPolicy().ExecuteAsync( () => SendAsync(requestType, requestUri, cancellationToken, modifyRequestAction, jsonRequest));
        }

        /// <summary>
        /// Starts a request to the service with retry.
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestType">Request type.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequestAction">Modify request action.</param>
        /// <param name="jsonRequest">Json request.</param>
        /// <param name="deserializeResponse">If set to <c>true</c> deserialize response.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected virtual Task<T> SendWithRetryAsync<T>(HttpMethod requestType, string requestUri, CancellationToken cancellationToken = default(CancellationToken), Action<HttpRequestMessage> modifyRequestAction = null, string jsonRequest = null, bool deserializeResponse = true)
        {
            return GetRetryPolicy().ExecuteAsync(() => SendAsync<T>(requestType, requestUri, cancellationToken, modifyRequestAction, jsonRequest, deserializeResponse));
        }

        /// <summary>
        /// Starts a request to the service
        /// </summary>
        /// <returns>Task with return type of HttpResponseMessage.</returns>
        /// <param name="requestType">Request type.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <param name="requestContent">Json request.</param>
        protected Task<HttpResponseMessage> SendAsync(HttpMethod requestType, string requestUri, CancellationToken cancellationToken, Action<HttpRequestMessage> modifyRequest = null, string requestContent = null)
        {
            var request = new HttpRequestMessage(requestType, new Uri($"{_baseApiUri}{requestUri}"));

            if (requestContent != null)
                request.Content = new StringContent(requestContent, Encoding.UTF8, StreamSerializer?.MediaType ?? Serializer.MediaType);

            modifyRequest?.Invoke(request);

            return _client.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Starts a request to the service
        /// </summary>
        /// <returns>A task with result of type T.</returns>
        /// <param name="requestType">Request type.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <param name="requestContent">Json request.</param>
        /// <param name="deserializeResponse">Indicates whether the reponse should be deserialized or returned directly.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        protected virtual async Task<T> SendAsync<T>(HttpMethod requestType, string requestUri, CancellationToken cancellationToken = default(CancellationToken), Action<HttpRequestMessage> modifyRequest = null, string requestContent = null, bool deserializeResponse = false)
        {
            T result = default(T);

            try
            {
                result = await SendAndDeserialize<T>(requestType, requestUri, cancellationToken, modifyRequest, requestContent, deserializeResponse);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex);
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
        /// <param name="requestContent">Request content.</param>
        /// <param name="deserializeResponse">Indicates whether the reponse should be deserialized or returned directly.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        protected async Task<T> SendAndDeserialize<T>(HttpMethod requestType, string requestUri, CancellationToken cancellationToken = default(CancellationToken), Action<HttpRequestMessage> modifyRequest = null, string requestContent = null, bool deserializeResponse = true)
        {
            T result = default(T);

            var response = await SendAsync(requestType, requestUri, cancellationToken, modifyRequest, requestContent).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                if (response != null)
                {
                    if (StreamSerializer != null)
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                            result = StreamSerializer.Read<T>(responseStream);
                    }
                    else
                    {
                        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        if (!string.IsNullOrWhiteSpace(responseString))
                            result = deserializeResponse ? Serializer.Deserialize<T>(responseString) : (T)Convert.ChangeType(responseString, typeof(T));
                    }
                }
            }
            else
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw new HttpServiceException(response?.StatusCode ?? HttpStatusCode.ServiceUnavailable, ex);
                }
            }

            return result;
        }
    }
}