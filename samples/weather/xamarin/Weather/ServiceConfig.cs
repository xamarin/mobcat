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
        public const string WeatherServiceApiKey = "B50996A0-9D60-44AF-BF08-81029CE2B8C7";
        public const string WeatherServiceUrl = "https://asapikjy6zobfbf6xe.azurewebsites.net/api/";

        public const string AndroidAppCenterSecret = "ANDROID_APP_CENTER_SECRET";
        public const string iOSAppCenterSecret = "iOS_APP_CENTER_SECRET";
    }
}
