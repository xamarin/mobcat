using System;

namespace Weather
{
    /// <summary>
    /// Secret key file.
    /// Replace these values with your API Key values
    /// </summary>
    public static class ServiceConfig
    {
        public static readonly string WeatherServiceApiKey = "weather_service_api_key";

        public static readonly string WeatherServiceUrl = "weather_service_url";

        public static readonly string AndroidAppCenterSecret = "android_appcenter_secret";

        public static readonly string iOSAppCenterSecret = "ios_appcenter_secret";
    }
}
