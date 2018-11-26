@echo off
rem Script for setting environment variables to aid in local client development

echo.
echo ^========== WeatherSample Environment Configuration started ==========
echo.

set apiKey=
set serviceEndpoint=
set androidAppCenterSecret=
set iOSAppCenterSecret=

rem Resolve parameters
:parse-params
if not "%1"=="" (
    if "%1"=="--api-key" (
        SET apiKey=%2
        SHIFT
    )
    if "%1"=="--service-endpoint" (
        SET serviceEndpoint=%2
        SHIFT
    )
    if "%1"=="--android-appcenter-secret" (
        SET androidAppCenterSecret=%2
        SHIFT
    )
    if "%1"=="--ios-appcenter-secret" (
        SET iOSAppCenterSecret=%2
        SHIFT
    )
    SHIFT
    GOTO :parse-params
)

rem Verify all parameters have values
if not defined apiKey (
    echo ^Missing --api-key parameter
    exit /B 1
)

if not defined serviceEndpoint (
    echo ^Missing --service-endpoint parameter
    exit /B 1
)

if not defined androidAppCenterSecret (
    echo ^Missing --android-appcenter-secret parameter
    exit /B 1
)

if not defined iOSAppCenterSecret (
    echo ^Missing --ios-appcenter-secret parameter
    exit /B 1
)

echo ^Setting Environement Variables:

echo ^- WeatherServiceApiKey
setx WeatherServiceApiKey %apiKey% >nul 2>&1

echo ^- WeatherServiceUrl
setx  WeatherServiceUrl %serviceEndpoint% >nul 2>&1

echo ^- AndroidAppCenterSecret
setx  AndroidAppCenterSecret %androidAppCenterSecret% >nul 2>&1

echo ^- iOSAppCenterSecret
setx  iOSAppCenterSecret %iOSAppCenterSecret% >nul 2>&1

echo.
echo ^========= WeatherSample Environment Configuration completed =========
echo.