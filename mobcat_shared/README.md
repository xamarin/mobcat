# MobCAT

[![Build Status](https://dotnetcst.visualstudio.com/MobCAT/_apis/build/status/MobCAT-CI?branchName=master)](https://dotnetcst.visualstudio.com/MobCAT/_build/latest?definitionId=60&branchName=master)

MobCAT is a toolbox created by our team to highlight best practices and good architecture pattersn in [Xamarin.Forms](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/),[Xamarin.Android](https://docs.microsoft.com/en-us/xamarin/#pivot=platforms&panel=Android) and [Xamarin.iOS](https://docs.microsoft.com/en-us/xamarin/#pivot=platforms&panel=iOS) apps. These projects provide a basis which can be adopted into your Xamarin solutions to have optimized performance and promote maximum amount of code share. 

The project themselves are .NET Standard Portable projects. The source can be cloned and the projects can be added to your solution. Please take a look at the [sample projects](https://github.com/xamarin/mobcat/tree/master/samples) to get a better understanding of their usage.

## Sections

* **MobCAT**

   In this project, you can find the shared logic for Xamarin.Forms and native Xamarin projects. 

   Useful files:
   
   - [BaseViewModel.cs](https://github.com/xamarin/mobcat/blob/master/mobcat_shared/MobCAT/MVVM/BaseViewModel.cs)
   - [AsyncCommand.cs](https://github.com/xamarin/mobcat/blob/master/mobcat_shared/MobCAT/MVVM/AsyncCommand.cs)
   - [BaseHTTPService.cs](https://github.com/xamarin/mobcat/blob/master/mobcat_shared/MobCAT/Services/BaseHttpService.cs)
   - [ServiceContainer.cs](https://github.com/xamarin/mobcat/blob/master/mobcat_shared/MobCAT/ServiceContainer.cs)


* **MobCAT.Forms**

   In this project you can find the Xamarin.Forms implementation for the shared architecture setup. You can also find some Xamarin.Forms specific implementation. 

   Useful files:

   - [BaseContentPage.cs](https://github.com/xamarin/mobcat/blob/master/mobcat_shared/MobCAT.Forms/Pages/BaseContentPage.cs)
   - [BaseNavigationPage.cs](https://github.com/xamarin/mobcat/blob/master/mobcat_shared/MobCAT.Forms/Pages/BaseNavigationPage.cs)
   - [NavigationService.cs](https://github.com/xamarin/mobcat/blob/master/mobcat_shared/MobCAT.Forms/Services/NavigationService.cs)


## Want more information?

### [Read the wiki](https://github.com/xamarin/mobcat/wiki)