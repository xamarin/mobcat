rem Script for provisioning the weather sample backend via Azure CLI

echo.
echo ^========== WeatherSample Provisioning started ==========
echo.

set targetTenant=
set targetSubscription=
set adminUpnOrObjectId=
set openWeatherMapAppId=
set apiKey=

rem Resolve parameters
:parse-params
if not "%1"=="" (
    if "%1"=="--tenant" (
        SET targetTenant=%2
        SHIFT
    )
    if "%1"=="--subscription" (
        SET targetSubscription=%2
        SHIFT
    )
    if "%1"=="--admin" (
        SET adminUpnOrObjectId=%2
        SHIFT
    )
    if "%1"=="--openweathermap-appid" (
        SET openWeatherMapAppId=%2
        SHIFT
    )
    if "%1"=="--api-key" (
        SET apiKey=%2
        SHIFT
    )
    SHIFT
    GOTO :parse-params
)

rem Verify all parameters have values
if not defined targetTenant (
    echo ^Missing --tenant parameter
    exit 1
)

if not defined targetSubscription (
    echo ^Missing --subscription parameter
    exit 1
)

if not defined adminUpnOrObjectId (
    echo ^Missing --admin parameter
    exit 1
)

if not defined apiKey (
    echo ^Missing --api-key parameter
    exit 1
)

if not defined openWeatherMapAppId (
    echo ^Missing --openweathermap-appid parameter
    exit 1
)

echo ^Account:
echo %adminUpnOrObjectId%
echo.

echo ^Tenant:
echo %targetTenant%
echo.

echo ^Subscription:
echo %targetSubscription%
echo.

set resourceGroupName=WeatherSample
set resourceGroupLocation=northeurope
set keyVaultAuditStorageAccountName=kvastg
set keyVaultName=kv
set searchAccountName=csacc
set appServicePlanName=asasp
set apiAppName=asapi
set appInsightsName=appins
set cacheName=rcache
set keyVaultEndpointSettingKey=KEYVAULT_ENDPOINT
set searchAccountInstrumentationKeyName=csacckey
set searchAccountInstrumentationKeyNameSettingKey=COGNITIVE_SERVICES_SEARCH_KEY_NAME
set appInsightsInstrumentationKeyName=appinskey
set appInsightsInstrumentationKeyNameSettingKey=APP_INSIGHTS_KEY_NAME
set cacheConnectionStringKeyName=cacheconnstr
set cacheConnectionStringKeyNameSettingKey=CACHE_CONNECTION_STRING_KEY_NAME
set apiKeyName=apikey
set apiKeyNameSettingKey=API_KEY_NAME
set openWeatherMapAppIdKeyName=owmapid
set openWeatherMapAppIdKeyNameSettingKey=OPEN_WEATHER_MAP_APP_ID_KEY_NAME

rem Login for Azure CLI must be handled as prerequisite!
echo ^Validating Azure Login
set token=
for /f "usebackq" %%i in ( `az account get-access-token --output tsv` ) do set token=%%i

if not defined token (
    echo ^Login required. Aborting
    exit 1
)

call az account set --subscription %targetSubscription% >nul 2>&1

rem Resolve adminId from user principal name or objectId
for /f "usebackq" %%i in ( `az ad user show --upn %adminUpnOrObjectId% --query "objectId" --output tsv` ) do set adminId=%%i

echo ^Preparing globally unique resource names
for /f "usebackq" %%i in ( `az group deployment create --name GloballyUniqueNameDeployment --resource-group %resourceGroupName% --template-file ../Templates/globallyuniquename.json --query "properties.outputs.uniqueValue.value" --output tsv` ) do set uniquePostfix=%%i

set keyVaultAuditStorageAccountName=%keyVaultAuditStorageAccountName%%uniquePostfix%
set keyVaultName=%keyVaultName%%uniquePostfix%
set searchAccountName=%searchAccountName%%uniquePostfix%
set appServicePlanName=%appServicePlanName%%uniquePostfix%
set apiAppName=%apiAppName%%uniquePostfix%
set appInsightsName=%appInsightsName%%uniquePostfix%
set cacheName=%cacheName%%uniquePostfix%

echo ^Creating or updating resource group
call az group create --name %resourceGroupName% --location %resourceGroupLocation% >nul 2>&1

echo ^Creating Storage for KeyVault Audit

echo ^Creating KeyVault

echo ^Creating KeyVault: Configuring diagnostic settings

echo ^Creating Cognitive Services Bing Search

echo ^Creating Application Insights

echo ^Creating Redis Cache (can take upwards of 20 minutes)

echo ^Creating App Service Plan

echo ^Creating API App

echo ^Creating API App: Configuring App Settings

echo ^Creating API App: Configuring Managed Service Identity

echo ^Creating API App: Configuring Acess Policy on KeyVault

echo ^Configuring KeyVault: Retrieving secrets from resources

echo ^Configuring KeyVault: Writing secrets

echo.
echo ^========== WeatherSample Provisioning completed ==========
echo.