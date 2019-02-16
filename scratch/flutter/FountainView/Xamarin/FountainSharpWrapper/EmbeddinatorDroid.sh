#!/bin/bash 

echo Running Embeddinator Droid Build...

export TOOLS=~/.nuget/packages/embeddinator-4000/0.4.0/tools
export PATH=$PATH:/Library/Frameworks/Mono.framework/Commands

mono $TOOLS/Embeddinator-4000.exe 'FountainSharpWrapperDroid/bin/Release/FountainSharpWrapperDroid.dll' -gen=Java -platform=Android --outdir='bin/android' -c
cp bin/android/FountainSharpWrapperDroid.aar ../../Native/DroidSampleApp/app/libs
cp bin/android/FountainSharpWrapperDroid.aar ../../Flutter/fountainview/android/libs