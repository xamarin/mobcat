using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.MobCAT.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Communicator
{

    public class MainViewModel : BaseNavigationViewModel

    {
        HubConnection _hubConnection;
        public ObservableCollection<string> _messages;

        public MainViewModel()
        {
            Messages = new ObservableCollection<string>();
            _hubConnection = new HubConnectionBuilder()
              .WithUrl($"https://mccommunicator.azurewebsites.net/chat")
              .Build();
            _hubConnection.Closed += _hubConnection_Closed;
            _hubConnection.On<string, string>("broadcastMessage", (user, message) =>
             {
                 Messages.Add(message);
             });
        }

        public ObservableCollection<string> Messages
        {
            get { return _messages; }
            set
            {
                RaiseAndUpdate(ref _messages, value);
            }
        }

        internal void SendMessage()
        {
            _hubConnection.InvokeAsync("BroadcastMessage",
                "BenBtgTestApp",
                "Testing testing."
                );
        }

        private Task _hubConnection_Closed(Exception arg)
        {
            Messages.Add("Connection closed");
            return ConnectToHub();
        }

        public async Task ConnectToHub()
        {
            Messages.Add("Opening connection...");

            try
            {
                await _hubConnection.StartAsync();
                Messages.Add("Connection open.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async void ConnectClicked(object sender, EventArgs e)
        {
            await ConnectToHub();
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            _hubConnection.InvokeAsync("BroadcastMessage",
                "BenBtgTestApp",
                "Testing testing."
                );
        }
    }
}
