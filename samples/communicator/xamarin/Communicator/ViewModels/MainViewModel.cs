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
        private ObservableCollection<Message> _messages;
        private string _messageText;
        private ObservableCollection<string> _conectedUsers;

        public Command BroadcastMessageCommand => new Command(() => SendBroadcastMessage());
        public Command SendDirectMessageCommand => new Command(() => SendDirectMessage());
        public Command AppearCommand => new Command(async () => await ConnectToHub());

        private string remoteAddress = "https://mccommunicator.azurewebsites.net";
        private string localAddrewss = "http://localhost:5000";

        public MainViewModel()
        {
            Messages = new ObservableCollection<Message>();
            _hubConnection = new HubConnectionBuilder()
              .WithUrl($"{localAddrewss}/chat")
              .Build();
            _hubConnection.Closed += _hubConnection_Closed;
            _hubConnection.On<string, string>("broadcastMessage", (user, message) =>
             {
                Messages.Add(new Message(user,message){ IsSender = user == _userName });
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

        public ObservableCollection<Message> Messages
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

        internal void SendBroadcastMessage()
        {
            _hubConnection.InvokeAsync("BroadcastMessage",
                UserName,
                MessageText
                );
        }

        internal void SendDirectMessage()
        {
            _hubConnection.InvokeAsync("SendPrivateMessage",
                UserName,
                MessageText
                );
        }

        private async Task _hubConnection_Closed(Exception arg)
        {
            Messages.Add(new Message("Event", "Connection closed") { IsNotification = true });
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await ConnectToHub();
        }

        public async Task ConnectToHub()
        {
            Messages.Add(new Message("Event", "Opening connection...") { IsNotification = true });
            UserName = Preferences.Get("UserName", DefaultUser);

            if (UserName == DefaultUser)
            {
                await Navigation.PushModalAsync(new LoginViewModel());
            }
            else
            {
                try
                {
                    await _hubConnection.StartAsync();
                    Messages.Add(new Message("Event","Connection open.") { IsNotification = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
