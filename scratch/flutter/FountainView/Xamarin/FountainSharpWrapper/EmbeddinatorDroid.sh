#!/bin/bash 

echo Running Embeddinator Droid Build...

export TOOLS=~/.nuget/packages/embeddinator-4000/0.4.0/tools
export PATH=$PATH:/Library/Frameworks/Mono.framework/Commands

mono $TOOLS/Embeddinator-4000.exe 'FountainLib.Droid/bin/Release/FountainLib.Droid.dll' -gen=Java -platform=Android --outdir='bin/android' -c
