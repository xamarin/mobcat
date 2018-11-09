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