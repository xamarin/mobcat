using System;
using System.Collections.Generic;
using Communicator.ViewModels;
using Microsoft.MobCAT.Forms.Pages;
using Xamarin.Forms;

namespace Communicator
{
    public partial class LoginPage : BaseContentPage<LoginViewModel>
    {
        private LoginViewModel _viewmodel;

        public LoginPage()
        {
            InitializeComponent();
        }
    }
}
