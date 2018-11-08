using System;
using Microsoft.MobCat.MVVM;

namespace Weather.ViewModels
{
    public class WeatherViewModel : BaseViewModel
    {
        string _cityName;

        public WeatherViewModel()
        {
            CityName = "London";
        }

        public string CityName
        {
            get { return _cityName; }
            set
            {

                RaiseAndUpdate(ref _cityName, value);
            }
        }

        public string Time
        {
            get { return DateTime.Now.ToShortTimeString(); }
        }




    }
}
