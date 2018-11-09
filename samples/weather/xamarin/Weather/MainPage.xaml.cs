using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.MobCat;
using Weather.Services.Abstractions;
using Weather.ViewModels;
using Xamarin.Forms;

namespace Weather
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new WeatherViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as WeatherViewModel).InitAsync();
        }
    }
}