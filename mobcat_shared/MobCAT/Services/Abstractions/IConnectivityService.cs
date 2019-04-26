using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.MobCAT.Models;

namespace Microsoft.MobCAT.Abstractions
{
    /// <summary>
    /// Arguments to pass to event handlers
    /// </summary>
    public class ConnectivityChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Return whether or not the device is currently connected
        /// </summary>
        public bool IsConnected { get; private set; }

        public ConnectivityChangedEventArgs(
            bool isConnected)
        {
            IsConnected = isConnected;
        }
    }

    public class ConnectivityTypeChangedEventArgs : ConnectivityChangedEventArgs
    {
        /// <summary>
        /// Gets the connection types.
        /// </summary>
        /// <value>The connection types.</value>
        public IEnumerable<ConnectionType> ConnectionTypes { get; private set; }

        public ConnectivityTypeChangedEventArgs(
            bool isConnected,
            IEnumerable<ConnectionType> connectionTypes) : base(isConnected)
        {
            ConnectionTypes = connectionTypes;
        }
    }

    public interface IConnectivityService
    {
        event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
        event EventHandler<ConnectivityTypeChangedEventArgs> ConnectivityTypeChanged;

        /// <summary>
        /// Return whether or not the device currently has a network connection
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Tests if a remote host name is reachable 
        /// </summary>
        /// <param name="host">Host name can be a remote IP or URL of website</param>
        /// <param name="port">Port to attempt to check is reachable.</param>
        /// <param name="msTimeout">Timeout in milliseconds.</param>
        Task<bool> IsHostReachable(string host, int msTimeout = 5000);

        /// <summary>
        /// Gets the supported connection types. 
        /// </summary>
        /// <value>The connection types. Cellular, Wifi or None.</value>
        IEnumerable<ConnectionType> ConnectionTypes { get; }
    }
}
