#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#addin "Cake.FileHelpers"
#addin "Cake.Xamarin"

// --------------------------------------------------------------------------------
// ARGUMENTS
// --------------------------------------------------------------------------------

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// --------------------------------------------------------------------------------
// FILES & DIRECTORIES
// --------------------------------------------------------------------------------

var solutionFile = File("Xamarin/FountainSharpWrapper/FountainSharpWrapper.sln");
var androidProject = File("Xamarin/FountainSharpWrapper/FountainSharpWrapperDroid/FountainSharpWrapperDroid.csproj");
var androidBin = Directory("../bin/Droid") + Directory(configuration);
var iOSProject = File("Xamarin/FountainSharpWrapper/FountainSharpWrapperIOS/FountainSharpWrapperIOS.csproj");
var iOSBin = Directory("../bin/iPhone") + Directory(configuration);
// --------------------------------------------------------------------------------
// PREPARATION
// --------------------------------------------------------------------------------

Task("Clean")
    .Does(() =>
{
    CleanDirectory(androidBin);
    CleanDirectory(iOSBin);
});

Task("Restore-NuGet")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});

// --------------------------------------------------------------------------------
// ANDROID
// --------------------------------------------------------------------------------

Task("Build Android")
    .IsDependentOn("Restore-NuGet")
    .Does(() =>
{
 	MSBuild(androidProject, settings =>
        settings.SetConfiguration(configuration)
        .WithTarget("SignAndroidPackage")
        .WithProperty("OutputPath", $"{androidBin}"));
});

// --------------------------------------------------------------------------------
// IOS
// --------------------------------------------------------------------------------

Task("Build iOS")
    .WithCriteria(IsRunningOnUnix())
    .IsDependentOn("Build Android")
    .Does(() =>
{
    MSBuild (iOSProject, settings => 
	    settings.SetConfiguration(configuration)
    		.WithProperty("Platform", "iPhone")
    		.WithProperty("OutputPath", $"{iOSBin}"));
});

// --------------------------------------------------------------------------------
// TESTS
// --------------------------------------------------------------------------------

Task("Build tests")
    .IsDependentOn("Build iOS")
    .Does(() =>
{
    var parsedSolution = ParseSolution(solutionFile);
          
	foreach(var project in parsedSolution.Projects)
	{
	    if(project.Name.EndsWith("UnitTests"))
		{
            Information("Start Building Test: " + project.Name);

            MSBuild(project.Path, settings => settings.SetConfiguration(configuration));

		}
	} 
});

Task("Run unit tests")
    .IsDependentOn("Build tests")
    .Does(() =>
{
    NUnit3("../**/bin/" + configuration + "/*.UnitTests.dll", new NUnit3Settings {
        NoResults = true
    });
});

Task("Run UI tests")
    .IsDependentOn("Run unit tests")
    .Does(() =>
{
});

// --------------------------------------------------------------------------------
// TASK TARGETS
// --------------------------------------------------------------------------------

Task("Default")
    .IsDependentOn("Run UI tests");

// --------------------------------------------------------------------------------
// EXECUTION
// --------------------------------------------------------------------------------

RunTarget(target);