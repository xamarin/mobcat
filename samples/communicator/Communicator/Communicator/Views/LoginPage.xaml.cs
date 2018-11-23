using System;
using System.Collections.Generic;
using Communicator.ViewModels;
using Xamarin.Forms;

namespace Communicator
{
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _viewmodel;

        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = _viewmodel = new LoginViewModel();
        }
    }
}
