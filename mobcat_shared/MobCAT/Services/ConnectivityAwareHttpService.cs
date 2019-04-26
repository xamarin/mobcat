using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.MobCAT.Abstractions;

namespace Microsoft.MobCAT.Services
{
    public class ConnectivityAwareHttpService : BaseHttpService
    {
        IConnectivityService _connectivity;

        /// <summary>
        /// Gets the connectivity service
        /// </summary>
        /// <value>The connectivity.</value>
        protected IConnectivityService Connectivity => _connectivity ?? (_connectivity = ServiceContainer.Resolve<IConnectivityService>(true));

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Microsoft.MobCat.Core.Services.ConnectivityAwareHttpService"/> class.
        /// </summary>
        /// <param name="baseApiUri">Base API URI.</param>
        /// <param name="handler">Handler.</param>
        protected ConnectivityAwareHttpService(string baseApiUri, HttpMessageHandler handler = null) : base(baseApiUri, handler)
        {
        }

        /// <summary>
        /// Starts a Connectivity-Aware request to the service
        /// </summary>
        /// <returns>Task with return type T.</returns>
        /// <param name="requestType">Request type.</param>
        /// <param name="requestUri">Request URI.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="modifyRequest">Modify request.</param>
        /// <param name="jsonRequest">Json request.</param>
        /// <typeparam name="T">The type of response.</typeparam>
        protected override async Task<T> SendAsync<T>(HttpMethod requestType, string requestUri, CancellationToken cancellationToken = default(CancellationToken), Task<Action<HttpRequestMessage>> modifyRequest = null, string jsonRequest = null)
        {
            T result = default(T);

            try
            {
                if (Connectivity != null && (await Connectivity.IsHostReachable(BaseApiUrl)))
                {
                    result = await SendAndDeserialize<T>(requestType, requestUri, cancellationToken, modifyRequest, jsonRequest);
                }
                else
                {
                    Logger.Info($"{BaseApiUrl} unavailable");
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw ex;
            }

            return result;
        }
    }
}
