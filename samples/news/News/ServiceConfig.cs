using System;
//using MobCAT.ClientSecrets;

namespace News
{
    /// <summary>
    /// Secret key file.
    /// Secrets are replaced environment variables using MobCAT.ClientSecrets: https://github.com/xamarin/mobcat/tree/master/mobcat_client_secrets
    /// Run build/environment.sh with the variable flags to set your environment variables
    /// </summary>
    public static class ServiceConfig
    {
        //[ClientSecret]
        public static readonly string NEWSSERVICEAPIKEY = string.Empty;

        //[ClientSecret]
        public static readonly string NEWSSERVICEURL = string.Empty;

        //[ClientSecret]
        public static readonly string ANDROIDAPPCENTERSECRET = string.Empty;

        //[ClientSecret]
        public static readonly string IOSAPPCENTERSECRET = string.Empty;
    }
}
