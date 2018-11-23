using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.MobCAT.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Communicator.ViewModels
{

    public class MainViewModel : BaseNavigationViewModel

    {
        readonly string DefaultUser = "_DefaultUser_";
        HubConnection _hubConnection;
        private string _userName;
        private ObservableCollection<string> _messages;
        private string _messageText;
        private ObservableCollection<string> _conectedUsers;


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

        public ObservableCollection<string> ConectedUsers
        {
            get { return _conectedUsers; }
            set
            {
                RaiseAndUpdate(ref _conectedUsers, value);
            }
        }

        public ObservableCollection<string> Messages
        {
            get { return _messages; }
            set
            {
                RaiseAndUpdate(ref _messages, value);
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                RaiseAndUpdate(ref _userName, value);
            }
        }

        public string MessageText
        {
            get { return _messageText; }
            set
            {
                RaiseAndUpdate(ref _messageText, value);
            }
        }



        internal void SendMessage()
        {
            _hubConnection.InvokeAsync("BroadcastMessage",
                "BenBtgTestApp",
                MessageText
                );
        }

        private async Task _hubConnection_Closed(Exception arg)
        {
            Messages.Add("Connection closed");
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await ConnectToHub();
        }

        public async Task ConnectToHub()
        {
            Messages.Add("Opening connection...");

            string userName = Preferences.Get("UserName", DefaultUser);
            if (userName == DefaultUser)
            {
                await Navigation.PushModalAsync(new LoginViewModel());
            }

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
