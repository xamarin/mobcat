#!/usr/bin/env bash

# Parse arguments.
for i in "$@"; do
    case $1 in
        "" ) break ;;
        -b ) BUILD_NUMBER="$2"; shift ;;
        -e ) APP_ENVIRONMENT="$2"; shift ;;
        -* ) echo "Unknown option: '$1'"; exit 1 ;;
        * ) echo "Unknown argument: '$1'"; exit 1 ;;
    esac
    shift
done

echo "Build number '$BUILD_NUMBER'"
echo "Environment '$APP_ENVIRONMENT'"

# Variables
APP_ENVIRONMENT=$(echo $APP_ENVIRONMENT | tr '[:upper:]' '[:lower:]' | sed "s/_/./g")
MANIFEST_FILE=$PWD/samples/weather/xamarin/Weather.Android/Properties/AndroidManifest.xml
INFO_PLIST=$PWD/samples/weather/xamarin/Weather.iOS/Info.plist
IOS_ICONS=$PWD/samples/weather/xamarin/Weather.iOS/Assets.xcassets/AppIcon.appiconset/
ANDROID_ICONS=$PWD/samples/weather/xamarin/Weather.Android/Resources/
TARGET_ANDROID_ICONS=$PWD/Android/$APP_ENVIRONMENT/.
TARGET_IOS_ICONS=$PWD/iOS/$APP_ENVIRONMENT/AppIcon.appiconset/.

# Edit build number
if [ $BUILD_NUMBER ]; then
    sed -E -i '' "s/(versionCode=\")[^\"]*(\")/\\1$BUILD_NUMBER\\2/g" $MANIFEST_FILE
    plutil -replace CFBundleVersion -string $BUILD_NUMBER $INFO_PLIST
    echo "Build numbers updated with '$BUILD_NUMBER'"
fi

# Edit ID and icons
if [ $APP_ENVIRONMENT ]; then
    if [ $APP_ENVIRONMENT = prod ]; then
        SUFFIX=
    else
        SUFFIX=".$APP_ENVIRONMENT"
    fi

    PATTERN='(package="[^"]*).dev(")'
    sed -E -i '' "s/$PATTERN/\\1$SUFFIX\\2/g" $MANIFEST_FILE

    BUNDLE_ID=$(defaults read $INFO_PLIST CFBundleIdentifier | sed "s/.dev/$SUFFIX/")
    plutil -replace CFBundleIdentifier -string $BUNDLE_ID $INFO_PLIST

    echo "MANIFEST:"
    cat $MANIFEST_FILE
    echo "INFO.PLIST:"
    cat $INFO_PLIST
    echo "App IDs updated with '$SUFFIX'"

#Uncomment below to update icons for different environments
    # cp -rv "$TARGET_ANDROID_ICONS" "$ANDROID_ICONS"
    # cp -rv "$TARGET_IOS_ICONS" "$IOS_ICONS"
    # echo "Icons updated"
fi
