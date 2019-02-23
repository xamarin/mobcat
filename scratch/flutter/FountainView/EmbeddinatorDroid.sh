#!/bin/bash 

echo Running Embeddinator Droid Build...

export TOOLS=Xamarin/FountainSharpWrapper/packages/Embeddinator-4000.0.4.0/tools
export PATH=$PATH:/Library/Frameworks/Mono.framework/Commands

mono $TOOLS/Embeddinator-4000.exe 'Xamarin/FountainSharpWrapper/bin/Droid/Release/FountainSharpWrapperDroid.dll' -gen=Java -platform=Android --outdir='bin/Embeddinator/Droid' -c
mkdir Flutter/fountainview/android/libs/
mv bin/Embeddinator/Droid/FountainSharpWrapperDroid.aar Flutter/fountainview/android/libs/

