ls
#!/bin/bash 

echo Running Embeddinator iOS Build...

export TOOLS=Xamarin/FountainSharpWrapper/packages/Embeddinator-4000.0.4.0/tools
export PATH=$PATH:/Library/Frameworks/Mono.framework/Commands

$TOOLS/objcgen Xamarin/FountainSharpWrapper/bin/iPhone/Release/FountainSharpWrapperIOS.dll  --target=framework --platform=iOS --outdir=bin/Embeddinator/iOS -c
mv bin/Embeddinator/iOS/FountainSharpWrapperIOS.framework Flutter/fountainview/ios