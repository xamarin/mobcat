using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using CoreFoundation;
using Microsoft.MobCAT.Models;
using SystemConfiguration;
using System.Linq;

namespace Microsoft.MobCAT.iOS.Services
{
    /// <summary>
    /// Reachability helper
    /// </summary>
    [Foundation.Preserve(AllMembers = true)]
    internal class Reachability
    {
        /// <summary>
        /// Default host name to use
        /// </summary>
        public string HostName = "www.google.com";

        NetworkReachability _remoteHostReachability;
        NetworkReachability _defaultRouteReachability;

        /// <summary>
        /// Checks if reachable without requiring a connection
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            // Is it reachable with the current network configuration?
            bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

            // Do we need a connection to reach it?
            bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

            // Since the network stack will automatically try to get the WAN up,
            // probe that
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                noConnectionRequired = true;

            return isReachable && noConnectionRequired;
        }

        /// <summary>
        /// Raised every time there is an interesting reachable event,
        /// we do not even pass the info as to what changed, and
        /// we lump all three status we probe into one
        /// </summary>
        public event EventHandler ReachabilityChanged;

        async void OnChange(NetworkReachabilityFlags flags)
        {
            await Task.Delay(100);
            ReachabilityChanged?.Invoke(null, EventArgs.Empty);
        }

        bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (_defaultRouteReachability == null)
            {
                var ip = new IPAddress(0);
                _defaultRouteReachability = new NetworkReachability(ip);
                _defaultRouteReachability.SetNotification(OnChange);
                _defaultRouteReachability.Schedule(CFRunLoop.Main, CFRunLoop.ModeDefault);
            }
            if (!_defaultRouteReachability.TryGetFlags(out flags))
                return false;
            return IsReachableWithoutRequiringConnection(flags);
        }

        /// <summary>
        /// Checks the remote host status
        /// </summary>
        /// <returns></returns>
        public ConnectionType RemoteHostStatus()
        {
            NetworkReachabilityFlags flags;
            bool reachable;

            if (_remoteHostReachability == null)
            {
                _remoteHostReachability = new NetworkReachability(HostName);

                // Need to probe before we queue, or we wont get any meaningful values
                // this only happens when you create NetworkReachability from a hostname
                reachable = _remoteHostReachability.TryGetFlags(out flags);

                _remoteHostReachability.SetNotification(OnChange);
                _remoteHostReachability.Schedule(CFRunLoop.Main, CFRunLoop.ModeDefault);
            }
            else
                reachable = _remoteHostReachability.TryGetFlags(out flags);

            if (!reachable)
                return ConnectionType.None;

            if (!IsReachableWithoutRequiringConnection(flags))
                return ConnectionType.None;

            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return ConnectionType.Cellular;

            return ConnectionType.WiFi;
        }

        /// <summary>
        /// Checks internet connection status
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ConnectionType> GetActiveConnectionType()
        {
            var status = new List<ConnectionType>();

            NetworkReachabilityFlags flags;
            bool defaultNetworkAvailable = IsNetworkAvailable(out flags);

            // If it's a WWAN connection..
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
            {
                status.Add(ConnectionType.Cellular);
            }
            else if (defaultNetworkAvailable)
            {
                status.Add(ConnectionType.WiFi);
            }
            else if (((flags & NetworkReachabilityFlags.ConnectionOnDemand) != 0
                || (flags & NetworkReachabilityFlags.ConnectionOnTraffic) != 0)
                && (flags & NetworkReachabilityFlags.InterventionRequired) == 0)
            {
                // If the connection is on-demand or on-traffic and no user intervention
                // is required, then assume WiFi.
                status.Add(ConnectionType.WiFi);
            }

            if (!status.Any())
            {
                status.Add(ConnectionType.None);
            }

            return status;
        }

        /// <summary>
        /// Checks internet connection status
        /// </summary>
        /// <returns></returns>
        public ConnectionType InternetConnectionStatus()
        {
            var status = ConnectionType.None;

            NetworkReachabilityFlags flags;
            bool defaultNetworkAvailable = IsNetworkAvailable(out flags);

            // If it's a WWAN connection..
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
            {
                status = ConnectionType.Cellular;
            }

            // If the connection is reachable and no connection is required, then assume it's WiFi
            if (defaultNetworkAvailable)
            {
                status = ConnectionType.WiFi;
            }

            // If the connection is on-demand or on-traffic and no user intervention
            // is required, then assume WiFi.
            if (((flags & NetworkReachabilityFlags.ConnectionOnDemand) != 0
                || (flags & NetworkReachabilityFlags.ConnectionOnTraffic) != 0)
                && (flags & NetworkReachabilityFlags.InterventionRequired) == 0)
            {
                status = ConnectionType.WiFi;
            }

            return status;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _remoteHostReachability?.Dispose();
            _remoteHostReachability = null;

            _defaultRouteReachability?.Dispose();
            _defaultRouteReachability = null;
        }
    }
}
