using System;
using System.Windows.Input;
using Microsoft.MobCAT.MVVM;

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
                RaiseAndUpdate(ref _userName, value);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                RaiseAndUpdate(ref _password, value);
            }
        }
    }
}
