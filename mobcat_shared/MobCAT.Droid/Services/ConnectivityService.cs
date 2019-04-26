using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Java.Net;
using Microsoft.MobCAT.Abstractions;
using Microsoft.MobCAT.Models;

namespace Microsoft.MobCAT.Droid.Services
{
    public class ConnectivityService : IConnectivityService, IDisposable
    {
        /// <inheritdoc />
        public event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;

        /// <inheritdoc />
        public event EventHandler<ConnectivityTypeChangedEventArgs> ConnectivityTypeChanged;

        private ConnectivityChangeBroadcastReceiver _receiver;
        private ConnectivityManager _connectivityManager;

        public ConnectivityService()
        {
            _receiver = new ConnectivityChangeBroadcastReceiver(this);

            Application.Context.RegisterReceiver(_receiver, new IntentFilter(ConnectivityManager.ConnectivityAction));
            _receiver.ConnectivityChanged += OnConnectivityChange;
            _receiver.ConnectivityTypeChanged += OnConnectivityTypeChange;
        }

        ConnectivityManager ConnectivityManager
        {
            get
            {
                if (_connectivityManager == null || _connectivityManager.Handle == IntPtr.Zero)
                    _connectivityManager = (ConnectivityManager)(Application.Context.GetSystemService(Context.ConnectivityService));

                return _connectivityManager;
            }
        }

        internal bool GetIsConnected(ConnectivityManager manager)
        {
            try
            {
                //When on API 21+ need to use getAllNetworks, else fall base to GetAllNetworkInfo
                //https://developer.android.com/reference/android/net/ConnectivityManager.html#getAllNetworks()
                if ((int)Android.OS.Build.VERSION.SdkInt >= 21)
                {
                    foreach (var network in manager.GetAllNetworks())
                    {
                        try
                        {
                            var capabilities = manager.GetNetworkCapabilities(network);

                            if (capabilities == null)
                                continue;

                            //check to see if it has the internet capability
                            if (!capabilities.HasCapability(NetCapability.Internet))
                                continue;

                            var info = manager.GetNetworkInfo(network);

                            if (info == null || !info.IsAvailable)
                                continue;

                            if (info.IsConnected)
                                return true;
                        }
                        catch
                        {
                            //there is a possibility, but don't worry
                        }
                    }
                }
                else
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    foreach (var info in manager.GetAllNetworkInfo())
#pragma warning restore CS0618 // Type or member is obsolete
                    {
                        if (info == null || !info.IsAvailable)
                            continue;

                        if (info.IsConnected)
                            return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                Logger.Warn("Unable to get connected state - do you have ACCESS_NETWORK_STATE permission? - error: {0}", e);
                return false;
            }
        }

        /// <inheritdoc />
        public bool IsConnected => GetIsConnected(ConnectivityManager);


        /// <inheritdoc />
        public async Task<bool> IsHostReachable(string host, int msTimeout = 5000)
        {
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException(nameof(host));

            if (!IsConnected)
                return false;

            host = host.Replace("http://www.", string.Empty).
                        Replace("http://", string.Empty).
                        Replace("https://www.", string.Empty).
                        Replace("https://", string.Empty).
                        TrimEnd('/');

            return await Task.Run(() =>
            {
                bool reachable;
                try
                {
                    reachable = InetAddress.GetByName(host).IsReachable(msTimeout);
                }
                catch (UnknownHostException ex)
                {
                    Logger.Warn("Unable to reach: " + host + " Error: " + ex);
                    reachable = false;
                }
                catch (Exception ex2)
                {
                    Logger.Warn("Unable to reach: " + host + " Error: " + ex2);
                    reachable = false;
                }
                return reachable;
            });

        }

        /// <inheritdoc />
        public IEnumerable<ConnectionType> ConnectionTypes
        {
            get
            {
                return GetConnectionTypes(ConnectivityManager);
            }
        }

        internal IEnumerable<ConnectionType> GetConnectionTypes(ConnectivityManager manager)
        {
            //When on API 21+ need to use getAllNetworks, else fall base to GetAllNetworkInfo
            //https://developer.android.com/reference/android/net/ConnectivityManager.html#getAllNetworks()
            if ((int)Android.OS.Build.VERSION.SdkInt >= 21)
            {
                foreach (var network in manager.GetAllNetworks())
                {
                    NetworkInfo info = null;
                    try
                    {
                        info = manager.GetNetworkInfo(network);
                    }
                    catch
                    {
                        //there is a possibility, but don't worry about it
                    }

                    if (info == null)
                        continue;

                    if (!info.IsAvailable)
                        yield return ConnectionType.None;

                    yield return GetConnectionType(info.Type);
                }
            }
            else
            {
#pragma warning disable CS0618 // Type or member is obsolete
                foreach (var info in manager.GetAllNetworkInfo())
#pragma warning restore CS0618 // Type or member is obsolete
                {
                    if (info == null)
                        continue;

                    if (!info.IsAvailable)
                        yield return ConnectionType.None;

                    yield return GetConnectionType(info.Type);
                }
            }
        }

        void OnConnectivityChange(object sender, ConnectivityChangedEventArgs e) => ConnectivityChanged?.Invoke(sender, e);

        void OnConnectivityTypeChange(object sender, ConnectivityTypeChangedEventArgs e) => ConnectivityTypeChanged?.Invoke(sender, e);

        internal ConnectionType GetConnectionType(ConnectivityType connectivityType)
        {

            switch (connectivityType)
            {
                case ConnectivityType.Ethernet:
                case ConnectivityType.Wimax:
                case ConnectivityType.Wifi:
                case ConnectivityType.Bluetooth:
                    return ConnectionType.WiFi;
                case ConnectivityType.Mobile:
                case ConnectivityType.MobileDun:
                case ConnectivityType.MobileHipri:
                case ConnectivityType.MobileMms:
                case ConnectivityType.Dummy:
                    return ConnectionType.Cellular;
                default:
                    return ConnectionType.Cellular;
            }
        }

        #region IDisposable Support

        private bool disposed = false;
        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (_receiver != null)
                        Application.Context.UnregisterReceiver(_receiver);

                    _receiver.ConnectivityChanged -= OnConnectivityChange;
                    _receiver.ConnectivityTypeChanged -= OnConnectivityTypeChange;

                    _connectivityManager?.Dispose();
                    _connectivityManager = null;
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
