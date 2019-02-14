#!/bin/bash 

echo Running Embeddinator iOS Build...

export TOOLS=~/.nuget/packages/embeddinator-4000/0.4.0/tools
export PATH=$PATH:/Library/Frameworks/Mono.framework/Commands

$TOOLS/objcgen FountainSharpWrapperIOS/bin/Release/FountainSharpWrapperIOS.dll  --target=framework --platform=iOS --outdir=bin/ios -c