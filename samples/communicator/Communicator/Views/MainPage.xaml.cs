using Communicator.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Communicator
{
    public partial class MainPage : ContentPage
    {

        MainViewModel _viewmodel;
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = _viewmodel = new MainViewModel();

        }

        private void SendClicked(object sender, EventArgs e)
        {
            _viewmodel.SendMessage();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            await _viewmodel.ConnectToHub();
        }
    }
}
