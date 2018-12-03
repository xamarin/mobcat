# News-Man 2.0

News-Man 9000 2.0 a news app sample that showcases best patterns and practices for building, testing, and deploying mobile apps using [Xamarin](https://visualstudio.microsoft.com/xamarin/), [Azure DevOps](https://azure.microsoft.com/en-us/solutions/devops/), and [AppCenter](https://appcenter.ms/)

(Add Screenshots)

## Prerequisites

To run the sample successfully, local app secrets need to be set up for your development environment. Please refer to the following **App Secrets** sections to get started.

## Backend

The sample uses the [News API](https://newsapi.org/) backend, where you can register and generate a developer key which is required to run this sample.

## AppCenter

AppCenter integration is built in to the sample using the AppCenter NuGet packages for analytics and crashes. To get analytics in AppCenter, set up the iOS & Android apps and get the AppCenter secret key for each platform from the App's Overview tab.

## App Secrets

The app secrets in News are protected using [mobcat_client_secrets](https://github.com/xamarin/mobcat/tree/master/mobcat_client_secrets) which gets the secret values from your local development environment variables to prevent secret keys from being committed into source code. The following app secret variables can be found in ServiceConfig.cs and have the `[ClientSecret]` attribute:

1. NewsServiceApiKey
2. NewsServiceUrl
3. AndroidAppCenterSecret
4. iOSAppCenterSecret

### MacOS

To set up the environment variables using Terminal on MacOS, navigate to the `/build` folder and run the following:

```. environment.sh --api-key <Your API key> --service-endpoint <Your service endpoint> --android-appcenter-secret <Your Android AppCenter secret> --ios-appcenter-secret <Your iOS AppCenter secret>```

### Windows

TODO

## Solution Overview

The solution consists of the following projects:
- MobCAT: The MobCAT library which contains useful services and pattern base classes
- MobCAT.Forms: The MobCAT Forms library which contains useful Forms base classes, services, behaviors, and converters
- News: The News Forms project which contains the XAML and shared code
- News.Android: The News Android specific project contains Android assets
- News.iOS: The News iOS specific project contains iOS assets
- News.UITests: The UI Test project
- News.UnitTests: The unit test project which uses mock services using [Moq](https://github.com/moq/moq4)

## MVVM

The app uses the [MVVM(Model-View-ViewModel)](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm) pattern to decouple business logic & presentation code. MVVM utility classes can be found in MobCAT/MVVM

## Service Container

The MobCAT library also includes a service container to register & consume services within the app. This enables registering platform specific services and consuming them through a common interface within shared code. It also enables mock services to be registered for UI test or unit test purposes.

## Bootstrap

Bootstrap.cs is where the app services are instantiated and registered in the ServiceContainer. Platform specific startup actions can also be invoked using the provided parameter in the Begin method. The Begin method is called from the AppDelegate for iOS, and MainActivity for Android respectively.
