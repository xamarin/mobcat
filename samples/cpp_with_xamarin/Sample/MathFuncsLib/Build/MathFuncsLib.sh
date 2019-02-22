echo "========== Build All started =========="
echo ""

echo "Preparing working files and directories"

#Create directories and copy library files to working directory
{
    find . -name "bin" -type d -exec rm -rf "{}" \;
    find . -name "tmp" -type d -exec rm -rf "{}" \;
    mkdir bin
    mkdir bin/iOS
    mkdir bin/Android
    mkdir tmp    
    mkdir tmp/sourceFiles
    mkdir tmp/iOS
    mkdir tmp/Android

    find . -name "*.cpp" -exec cp {} tmp/sourceFiles \;
    find . -name "*.h" -exec cp {} tmp/sourceFiles \;
} &> /dev/null

declare -a AndroidArchitectures=("x86" "x86_64" "arm" "arm64")
declare -a iOSArchitectures=("x86_64" "arm64" "arm64e")

LibraryName="MathFuncs"
Android_NDK_Target="android-ndk-r15c"
Android_Minimum_Api_Version="21"
iOS_SDK_Version="12.1"
iOS_SDK_Min_Version="8.0"

echo ""
echo "=== BUILD TARGET (Android) ==="
echo ""

cd tmp

LibPath=${PWD}/sourceFiles

export ANDROID_NDK_HOME="/Users/$USER/Library/Developer/Xamarin/android-ndk/$Android_NDK_Target"

for i in "${AndroidArchitectures[@]}"
    do
        echo "Build for $i:"
        mkdir ../bin/Android/$i     
        mkdir Android/build      

        if [ $i == "x86" ]
        then
            CxxTarget="i686-linux-android-g++"
        elif [ $i == "x86_64" ]
        then
            CxxTarget="x86_64-linux-android-g++"
        elif [ $i == "arm" ]
        then
            CxxTarget="arm-linux-androideabi-g++"
        else
            CxxTarget="aarch64-linux-android-g++"
        fi

        echo "Installing customized toolchain"
        $ANDROID_NDK_HOME/build/tools/make_standalone_toolchain.py --api $Android_Minimum_Api_Version --arch $i --install-dir=${PWD}/Android/droid-toolchain --force 

        export CXX=$CxxTarget
        
        cd Android
        cd droid-toolchain
        cd bin

        echo "Compiling and linking (output as dynamic library)"

        for i2 in $LibPath/*.cpp; do
            ShortName="${i2##*/}"
            OutputName="${ShortName/cpp/o}"
            ${PWD}/$CXX -c $i2 -std=c++0x -o ../../build/$OutputName
        done

        ${PWD}/$CXX -shared -o ../../build/lib${LibraryName}.so ../../build/*.o

        cd ../../..

        echo "Copying lib${LibraryName}.so to bin/Android/$i"
        {
            find Android/build -name "*.so" -exec cp {} ../bin/Android/$i \;
        } &> /dev/null

        rm -rf Android/* 

        echo ""

done

cd ..
echo "** BUILD SUCCEEDED (Android) **"
echo ""     

echo ""
echo "=== BUILD TARGET (iOS) ==="
echo ""

cd tmp

for i in "${iOSArchitectures[@]}"
do
    SdkRootValue="iPhoneOS"
    echo "Build for $i:"
    if [ $i == "x86_64" ]
    then
        SdkRootValue="iPhoneSimulator"
    fi

    export DEVROOT=/Applications/Xcode.app/Contents/Developer/Platforms/$SdkRootValue.platform/Developer
    export IPHONEOS_DEPLOYMENT_TARGET=$iOS_SDK_Version
    export SDKROOT=$DEVROOT/SDKs/$SdkRootValue.sdk
    export CFLAGS="-std=c++0x -arch $i -pipe -no-cpp-precomp -fembed-bitcode -isysroot $SDKROOT -miphoneos-version-min=$iOS_SDK_Min_Version -I$SDKROOT/usr/include/"

    echo "Compiling and linking (output as static library)"

    cd sourceFiles
    g++ -c *.cpp $CFLAGS
    cd ..

    {
        ar ru iOS/${LibraryName}_${i}.a sourceFiles/*.o
    } &> /dev/null    

    cd sourceFiles
    find . -name "*.o" -type f -delete
    cd ..

    echo ""
done

echo "Build universal library:"
lipo iOS/*.a -output iOS/lib$LibraryName.a -create

echo "Copying lib${LibraryName}.a to bin/iOS"
{
    find iOS -name "lib${LibraryName}.a" -exec cp {} ../bin/iOS \;
} &> /dev/null

cd ..

echo ""
echo "** BUILD SUCCEEDED (iOS) **"
echo "" 

echo "========== Build All completed =========="
echo ""

cd ..

#Cleanup working directories
{
    find . -name "tmp" -type d -exec rm -rf "{}" \;
} &> /dev/null