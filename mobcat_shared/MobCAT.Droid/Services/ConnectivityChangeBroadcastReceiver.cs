using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Microsoft.MobCAT.Abstractions;

namespace Microsoft.MobCAT.Droid.Services
{
    /// <summary>
    /// Broadcast receiver to get notifications from Android on connectivity change
    /// </summary>
    [BroadcastReceiver(Enabled = true, Label = "MobCat Connectivity Broadcast Receiver")]
    [Android.Runtime.Preserve(AllMembers = true)]
    internal class ConnectivityChangeBroadcastReceiver : BroadcastReceiver
    {
        public event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
        public event EventHandler<ConnectivityTypeChangedEventArgs> ConnectivityTypeChanged;

        private bool _isConnected;
        private ConnectivityManager _connectivityManager;

        public ConnectivityChangeBroadcastReceiver()
        {
        }

        public ConnectivityChangeBroadcastReceiver(ConnectivityService service)
        {
            ConnectivityService = service;
            _isConnected = IsConnected;
        }

        internal ConnectivityService ConnectivityService { get; set; }

        ConnectivityManager ConnectivityManager
        {
            get
            {
                if (_connectivityManager == null || _connectivityManager.Handle == IntPtr.Zero)
                    _connectivityManager = (ConnectivityManager)(Application.Context.GetSystemService(Context.ConnectivityService));

                return _connectivityManager;
            }
        }

        /// <summary>
        /// Gets if there is an active internet connection
        /// </summary>
        bool IsConnected => ConnectivityService.GetIsConnected(ConnectivityManager);

        /// <summary>
        /// Received a notification via BR.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override async void OnReceive(Context context, Intent intent)
        {
            if (intent.Action != ConnectivityManager.ConnectivityAction)
                return;

            //await 500ms to ensure that the the connection manager updates
            await Task.Delay(500);

            var newConnection = IsConnected;
            if (newConnection != _isConnected)
            {
                _isConnected = newConnection;

                ConnectivityChanged?.Invoke(this, new ConnectivityChangedEventArgs(_isConnected));
            }


            var connectionTypes = ConnectivityService.GetConnectionTypes(ConnectivityManager);

            ConnectivityTypeChanged?.Invoke(this, new ConnectivityTypeChangedEventArgs(newConnection, connectionTypes));
        }
    }
}
