using System;
using MobCAT.ClientSecrets;

namespace Weather
{
    /// <summary>
    /// Secret key file.
    /// Secrets are replaced environment variables using MobCAT.ClientSecrets: https://github.com/xamarin/mobcat/tree/master/mobcat_client_secrets
    /// Run build/environment.sh with the variable flags to set your environment variables
    /// </summary>
    public static class ServiceConfig
    {
        [ClientSecret]
        public static readonly string WeatherServiceApiKey = string.Empty;

        [ClientSecret]
        public static readonly string WeatherServiceUrl = string.Empty;

        [ClientSecret]
        public static readonly string AndroidAppCenterSecret = string.Empty;

        [ClientSecret]
        public static readonly string iOSAppCenterSecret = string.Empty;
    }
}
