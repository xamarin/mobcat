# Introduction 
This sample demonstrates the basic steps involved with incorporating a precompiled native C/C++ library as part of a Xamarin solution. It was created for use on the Xamarin show as a quick introduction to the topic with a focus on getting started with [P/Invoke](https://docs.microsoft.com/cpp/dotnet/calling-native-functions-from-managed-code?view=vs-2017) concepts.  

![Sample App and Unit Test Runner](illustrations/cpp_xam_xs.jpg "Sample App and Unit Test Runner")

The solution is comprised of 3 high-level areas:  

**App**  
Xamarin.Forms application demonstrating the use of the library functionality by displaying a Fibonacci sequence of a specified length. Platform-specific references are added to the Android and iOS targets for use within the shared code. 

**Library**  
Provides the underlying functionality for the sample app by wrapping a native library. Platform-specific Android and iOS targets are used to include the respective native libraries which are subsequently exposed to .NET in a cross-platform manner via shared code. This was the main focus of the session.

**Test**  
Validates the library functionality using platform-agnostic unit test code along with targets for Android and iOS facilitating the use of the platform-specific [NUnit based test runner](https://github.com/nunit/nunit.xamarin).

# Getting started  
Download or clone this repo and then:

1. Open SampleApp.sln in Visual Studio
2. Restore NuGet packages for the solution
3. Build the solution

## Running the tests
1. Set SampleLib.Test.{Android/iOS} as the **Startup project**
2. Press **COMMAND** + **RETURN** to start debugging
3. Press **Run Everything** to execute the test via the test runner

## Running the app
1. Set SampleApp.{Android/iOS} as the **Startup project**\
2. Press **COMMAND** + **RETURN** to start debugging
3. Press **Get Fibonacci** to show the sequence

**NOTE:**
You will need an Apple Developer account along with a suitable certificate and provisioning profile in order to deploy to an iOS device. 

# Further Reading  

[NUnit](https://github.com/nunit/nunit.xamarin)  
[P/Invoke](https://docs.microsoft.com/cpp/dotnet/calling-native-functions-from-managed-code?view=vs-2017)  
[Safe Handle](https://docs.microsoft.com/dotnet/api/system.runtime.interopservices.safehandle?view=netframework-4.7.2)  
[Use C/C++ libraries with Xamarin](https://docs.microsoft.com/xamarin/cross-platform/cpp/)