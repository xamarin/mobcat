using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobCATShell.Forms.Services;
using MobCATShell.Forms.Views;

namespace MobCATShell.Forms
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Bootstrap.Begin();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
