# Script for setting environment variables to aid in local client development

echo ""
echo "========== WeatherSample Environment Configuration started =========="
echo ""

# Resolve parameters
for i in "$@"; do
    case $1 in
        "" ) break ;;
        -k | --api-key ) API_KEY="$2"; shift ;;
        -s | --service-endpoint ) SERVICE_ENDPOINT="$2"; shift ;;
        -a | --android-appcenter-secret ) ANDROID_APP_CENTER_SECRET="$2"; shift ;;
        -i | --ios-appcenter-secret ) iOS_APP_CENTER_SECRET="$2"; shift ;;
        -* | --*) echo "Unknown option: '$1'"; exit 1 ;;
        * ) echo "Unknown argument: '$1'"; exit 1 ;;
    esac
    shift
done

# Verify all parameters have values
if [ -z "$API_KEY" ]
then
    echo "Missing --api-key parameter"
    exit 1
fi

if [ -z "$SERVICE_ENDPOINT" ]
then
    echo "Missing --service-endpoint parameter"
    exit 1
fi

if [ -z "$ANDROID_APP_CENTER_SECRET" ]
then
    echo "Missing --android-appcenter-secret parameter"
    exit 1
fi

if [ -z "$iOS_APP_CENTER_SECRET" ]
then
    echo "Missing --ios-appcenter-secret parameter"
    exit 1
fi

echo "Setting Environement Variables:"

echo "- WeatherServiceApiKey"
launchctl setenv WeatherServiceApiKey $API_KEY

echo "- WeatherServiceUrl"
launchctl setenv WeatherServiceUrl $SERVICE_ENDPOINT

echo "- AndroidAppCenterSecret"
launchctl setenv AndroidAppCenterSecret $ANDROID_APP_CENTER_SECRET

echo "- iOSAppCenterSecret"
launchctl setenv iOSAppCenterSecret $iOS_APP_CENTER_SECRET

echo ""
echo "NOTE: You must quit and re-open Visual Studio (if it was open)"

echo ""
echo "========= WeatherSample Environment Configuration completed ========="
echo ""