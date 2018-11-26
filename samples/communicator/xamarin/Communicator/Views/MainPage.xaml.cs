using Communicator.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.MobCAT.Forms.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Communicator
{
    public partial class MainPage : BaseContentPage<MainViewModel>
{

        // MainViewModel _viewmodel;
        public MainPage()
        {
            InitializeComponent();

        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            await ViewModel.ConnectToHub();
        }
    }
}
