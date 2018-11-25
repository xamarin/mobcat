using System;
using System.Windows.Input;
using Microsoft.MobCAT.MVVM;
using Xamarin.Essentials;

namespace Communicator.ViewModels
{
    public class LoginViewModel: BaseNavigationViewModel
    {
        public Command LoginCommand => new Command(async () => await Navigation.PopModalAsync());
        private string _userName;
        private string _password;

        public LoginViewModel()
        {
            UserName = "BenTset";
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                Preferences.Set("UserName", value);
                RaiseAndUpdate(ref _userName, value);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                Preferences.Set("Password", value);
                RaiseAndUpdate(ref _password, value);
            }
        }
    }
}
