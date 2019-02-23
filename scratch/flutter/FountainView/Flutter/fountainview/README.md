# Flutter Fountain View

FountainView is a Flutter plugin used for displaying [Fountain](https://fountain.io/) screenplay documents. It takes a single string and displays the formatted document in a widget.

This Flutter Plugin is created using the Xamarin library FountainSharp written in F#.

This is possible with the use of [Embeddinator-4000](https://github.com/mono/Embeddinator-4000) which allows the creation of native libraries from Xamarin .NET libraries. The native Android (.aar) and iOS (Framework) is then embedded in a Flutter Plugin that can then be reused in any Flutter application.

For example, the **[Big Fish](https://fountain.io/_downloads/Big-Fish.fountain)** sample fountain doc looks like this when rendered.
![image info ](https://raw.githubusercontent.com/xamarin/mobcat/FountainView/scratch/flutter/FountainView/Flutter/fountainview/screenshots/FountainViewiOSScreenShot-small.jpg)

## Usage
Usage is very straight forward, simply construct a new FountainView widget and provide it with a string containing valid [Fountain](https://fountain.io/) Markup text.

    new FountainView(fountainText)
    
The text will be parsed and converted to HTML and displayed as using the [flutter_html](https://pub.dartlang.org/packages/flutter_html) plugin.

Note I plan to switch to the recently released official [webview_flutter](https://pub.dartlang.org/packages/webview_flutter) once it is stable.

## Known Issues
If you get the following error on Android 

     No assemblies found in '(null)' or '<unavailable>'. Assuming this is part of Fast Deployment.

Open the build.gradle file under android/app/ and add the follow config inside android {}

    aaptOptions {
        noCompress 'dll'
    }

## Acknowledgments

This plugin is made using [FountainSharp](https://github.com/bryancostanich/FountainSharp) with thanks to [Bryan Costanich](https://github.com/bryancostanich).

FountainSharp is an F# based Fountain Markdown processor for use via .NET/Xamarin projects.