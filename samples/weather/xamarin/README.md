# WeatherTron 9000
WeatherTron 9000 is a single view sample app that demonstrates a number of recommended patterns and practices for building, testing, and deploying mobile apps using [Xamarin](https://visualstudio.microsoft.com/xamarin/), [Azure DevOps](https://azure.microsoft.com/en-us/solutions/devops/), and [AppCenter](https://appcenter.ms/).

<p>
<img src="../readme_illustrations/weather_app_android.png" alt="Weather App for Android" height="350" style="display:inline-block;" />

<img src="../readme_illustrations/weather_app_ios.png" alt="Weather App for iOS" height="350" style="display:inline-block;" />
</p>

## Solution Overview
The solution consists of the following projects:  

**MobCAT:**  
Foundational components supporting the implementation of recommended patterns and practices in a simple manner

**MobCAT.Forms:**  
Foundational components specific to [Xamarin.Forms](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/)  

**Weather:**  
The Weather Forms project which contains the XAML and shared code  

**Weather.Android:**  
The Weather Android specific project contains Android assets  

**Weather.iOS:**  
The Weather iOS specific project contains iOS assets  

**Weather.UITests:**  
The UI Test project  

**Weather.UnitTests:**  
The unit test project which uses mock services using [Moq](https://github.com/moq/moq4)


## Getting Started
You must perform the following prerequisite actions in order to run this sample app:  

### 1. Create an app in the App Center Portal
The sample demonstrates analytics and crash reporting via [App Center](https://appcenter.ms) using the [NuGet packages](https://www.nuget.org/packages/Microsoft.AppCenter) provided.  

To enable this you must [create an app in the App Center Portal](https://docs.microsoft.com/en-us/appcenter/sdk/getting-started/xamarin#2-create-your-app-in-the-app-center-portal-to-obtain-the-app-secret) for each platform.  

Once you have created the apps, you can obtain the respective **App Secret** values on the **Getting Started** or **Manage App** sections of the [App Center Portal](https://appcenter.ms).  

### 2. Provision the weather service
The sample is underpinned by a [web service](https://github.com/xamarin/mobcat/tree/dev/samples/weather/azure), built with [asp.net core 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1), which you must host yourself in order to run the app. Once you have [provisioned the service to Azure](https://github.com/xamarin/mobcat/blob/dev/samples/weather/azure/README.md), you should have the following details for the service:  

- **API Key** (you created this)
- **API Endpoint URL** e.g. *https://<app_name>.azurewebsites.net/api/*

### 3. Set Environment Variables for the app secrets
App secrets are resolved and set at built-time using our [MobCAT.ClientSecrets.Fody](https://github.com/xamarin/mobcat/tree/master/mobcat_client_secrets) add-in keeping the secrets out of the source code.   

The following app secrets are defined in the **ServiceConfig** class and are decorated by the `[ClientSecret]` attribute to denote that they will be set using an environment variable by the same name:
1. WeatherServiceApiKey
2. WeatherServiceUrl
3. AndroidAppCenterSecret
4. iOSAppCenterSecret

To configure the environment variables: 

1. Open **Command Prompt/Terminal** 
2. Change directory to the **mobcat/samples/weather/xamarin/build** folder
3. Execute the **environment** script passing in the requisite parameters. For example:

    **macOS:**  
    ```
    ./environment.sh --api-key <API Key> --service-endpoint <API Endpoint URL> --android-appcenter-secret <Android AppCenter App Secret> --ios-appcenter-secret <iOS AppCenter App Secret>
    ```  

    **Windows:**  
    ```
    environment.bat --api-key <API Key> --service-endpoint <API Endpoint URL> --android-appcenter-secret <Android AppCenter App Secret> --ios-appcenter-secret <iOS AppCenter App Secret>
    ```

    **NOTE:** You must restart **Visual Studio for Mac** in order for changes to these environment variables to be recognized (if it was open at the time these were set)

## Key Concepts

### MVVM
The app uses the [MVVM(Model-View-ViewModel)](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm) pattern to decouple business logic & presentation code. The relevant supporting components can be found in **MobCAT/MVVM**

### Service Container
The MobCAT library includes a simple **ServiceContainer** class. This enables both platform-specific and platform-agnostic components to be registered and subsequently resolved via a common interface within shared code. It also enables mock services to be registered for UI test or unit test purposes.

### Bootstrap
The **Bootstrap** class facilitates the process of initializing and registering the respective services using the **ServiceContainer** class. Platform-specific startup actions can also be invoked using the provided parameter in the Begin method. The Begin method is called from the **AppDelegate** for iOS, and **MainActivity** for Android respectively.

### BaseHttpService
The **BaseHttpService** base class simplifies working with REST APIs by abstracting the details associated with using **HttpClient**, handling common tasks such as serialization and implementing recommended patterns such as retry and circuit breaker.

### Localization
Localization is implemented using .resx resources based on the [official Xamarin sample](https://github.com/xamarin/xamarin-forms-samples/tree/master/UsingResxLocalization). Malay (ms) and Arabic (ar) localization has been implemented with Right-to-Left language support.

<p>
<img src="../readme_illustrations/redmond_malay.png" alt="Weather App in Malay" height="350" style="display:inline-block;" />

<img src="../readme_illustrations/redmond_english.png" alt="Weather App in English" height="350" style="display:inline-block;" />

<img src="../readme_illustrations/redmond_arabic.png" alt="Weather App in Arabic" height="350" style="display:inline-block;" />
</p>

### Text Size Accessibility
Accessibility text size is supported using a custom control on iOS, the AccessibilityLabel. On Android, text size accessibility is supported out of the box.

<p>
<img src="../readme_illustrations/accessibility_small.png" alt="Weather App with small text" height="350" style="display:inline-block;" />

<img src="../readme_illustrations/redmond_english.png" alt="Weather App with default text size" height="350" style="display:inline-block;" />

<img src="../readme_illustrations/accessibility_large.png" alt="Weather App with large text" height="350" style="display:inline-block;" />
</p>
