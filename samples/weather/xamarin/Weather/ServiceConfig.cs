using System;
namespace Weather
{
    /// <summary>
    /// Secret key file.
    /// Keys are replaced by a bash script in the build pipeline to have the proper environment values.
    /// This file should only be committed to source control when adding new Keys with the following naming convention.
    /// Naming convention example: MySecretKey = "MY_SECRET_KEY"
    /// Each key must be unique.
    /// Ignore local updates using: > git update-index --assume-unchanged <file>
    /// Reference: https://docs.microsoft.com/en-us/azure/devops/repos/git/ignore-files?view=vsts&tabs=visual-studio
    /// </summary>
    public static class ServiceConfig
    {
        public const string WeatherServiceApiKey = "WEATHER_SERVICE_API_KEY";
        public const string WeatherServiceUrl = "WEATHER_SERVICE_URL";

        public const string AndroidAppCenterSecret = "ANDROID_APP_CENTER_SECRET";
        public const string iOSAppCenterSecret = "iOS_APP_CENTER_SECRET";
    }
}
