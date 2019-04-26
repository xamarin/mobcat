using System;
namespace Microsoft.MobCAT.Models
{
    /// <summary>
    /// Types of connections
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// No internet connection
        /// </summary>
        None,
        /// <summary>
        /// Reachable view Cellular.
        /// </summary>
        Cellular,
        /// <summary>
        /// Reachable view wifi
        /// </summary>
        WiFi
    }
}
