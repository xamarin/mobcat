# FountainView is a Flutter Plugin created using a Xamarin library written in F#

The purpose is to demonstrate the use of Embedinatior 4000 to create a native libraries written in Xamarin that can be reused in a Flutter application.

## Getting Started

Build this project is split into 3 main folders. 

* Xamarin
* Native
* Flutter

### Xamarin
This contains the FountainSharpWrapper for iOS and Android. These creat Xamarin native librarys that can be embedded in native iOS and Android app with the help of embeddinator 4000.

### Native
This contains native iOS (Objective-C) and Android(Java) projects to test the native librarys created from ther Xamarin project.

### Flutter
This contain the Flutter Plugin and exmaple app.


## Acknowdledgments

This plugin is made using FountainSharp with thanks to Bryan Costanich https://github.com/bryancostanich

FountainSharp is an F# based Fountain Markdown processor for use via .NET/Xamarin projects. FoutainSharp parses Fountain-formatted scripts and loads them into model that can be transformed or used for WYSIWYG editing.

It ships with a sample transformation engine that transforms Fountain markdown into HTML.
https://github.com/bryancostanich/FountainSharp

