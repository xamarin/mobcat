using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.MobCAT.Abstractions;
using Microsoft.MobCAT.Models;
using Microsoft.MobCAT.Services;

namespace Microsoft.MobCAT.iOS.Services
{
    public class ConnectivityService : IConnectivityService, IDisposable
    {
        Task _initialTask = null;
        Reachability _reachability;

        /// <inheritdoc />
        public event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;

        /// <inheritdoc />
        public event EventHandler<ConnectivityTypeChangedEventArgs> ConnectivityTypeChanged;

        public ConnectivityService()
        {
            _reachability = new Reachability();
            _reachability.ReachabilityChanged += ReachabilityChanged;

            //start an update on the background.
            _initialTask = Task.Run(() => { UpdateConnected(false); });
        }

        async void ReachabilityChanged(object sender, EventArgs e)
        {
            //Add in artifical delay so the connection status has time to change
            //else it will return true no matter what.
            await Task.Delay(100);
            UpdateConnected();
        }


        private bool _isConnected;
        private ConnectionType _previousInternetStatus = ConnectionType.None;

        private void UpdateConnected(bool triggerChange = true)
        {
            var remoteHostStatus = _reachability?.RemoteHostStatus();
            var internetStatus = _reachability?.InternetConnectionStatus();

            var previouslyConnected = _isConnected;
            _isConnected = (internetStatus == ConnectionType.Cellular ||
                            internetStatus == ConnectionType.WiFi) ||
                           (remoteHostStatus == ConnectionType.Cellular ||
                            remoteHostStatus == ConnectionType.WiFi);

            if (triggerChange)
            {
                if (previouslyConnected != _isConnected || _previousInternetStatus != internetStatus)
                    ConnectivityChanged?.Invoke(this, new ConnectivityChangedEventArgs(_isConnected));

                var connectionTypes = this.ConnectionTypes.ToArray();
                ConnectivityTypeChanged?.Invoke(this, new ConnectivityTypeChangedEventArgs(_isConnected, connectionTypes));
            }

            _previousInternetStatus = internetStatus ?? ConnectionType.None;
        }

        /// <inheritdoc />
        public bool IsConnected
        {
            get
            {
                if (_initialTask?.IsCompleted ?? true)
                {
                    UpdateConnected(false);
                    return _isConnected;
                }

                //await for the initial run to complete
                _initialTask.Wait();
                return _isConnected;
            }
        }

        /// <inheritdoc />
        public async Task<bool> IsHostReachable(string host, int msTimeout = 5000)
        {
            if (string.IsNullOrEmpty(host))
                return false;

            if (!IsConnected)
                return false;

            return await IsRemoteReachable(host, 80, msTimeout);
        }

        private async Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000)
        {
            if (string.IsNullOrEmpty(host))
                return false;

            if (!IsConnected)
                return false;

            host = host.Replace("http://www.", string.Empty).Replace("http://", string.Empty).Replace("https://www.", string.Empty).Replace("https://", string.Empty).TrimEnd('/');


            return await Task.Run(() =>
            {
                try
                {
                    var clientDone = new ManualResetEvent(false);
                    var reachable = false;

                    var hostEntry = new DnsEndPoint(host, port);

                    using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
                    {
                        var socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = hostEntry };
                        socketEventArg.Completed += (s, e) =>
                        {
                            reachable = e.SocketError == SocketError.Success;

                            clientDone.Set();
                        };

                        clientDone.Reset();

                        socket.ConnectAsync(socketEventArg);

                        clientDone.WaitOne(msTimeout);

                        return reachable;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warn("Unable to reach: " + host + " Error: " + ex);
                    return false;
                }
            });
        }

        /// <inheritdoc />
        public IEnumerable<ConnectionType> ConnectionTypes
        {
            get { return _reachability?.GetActiveConnectionType() ?? new[] { ConnectionType.None }; }
        }

        #region IDisposable Support

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _reachability.ReachabilityChanged -= ReachabilityChanged;
                    _reachability?.Dispose();
                    _reachability = null;
                }

                disposed = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}
