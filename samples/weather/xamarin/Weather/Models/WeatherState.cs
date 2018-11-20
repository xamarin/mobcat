using System;
using System.Runtime.Serialization;

namespace Weather.Models
{
    [DataContract]
    public class WeatherState
    {
        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public string CurrentTemp { get; set; }

        [DataMember]
        public string HighTemp { get; set; }

        [DataMember]
        public string LowTemp { get; set; }

        [DataMember]
        public string WeatherDescription { get; set; }

        [DataMember]
        public string WeatherImage { get; set; }

        [DataMember]
        public bool IsCelsius { get; set; }

        [DataMember]
        public string WeatherIcon { get; set; }
    }
}