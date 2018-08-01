@ECHO OFF

if "%1" == "" goto no_config
if "%1" NEQ "" goto set_config

:set_config
SET Configuration=%1
GOTO build

:no_config
SET Configuration=Release
GOTO build

:build
echo ------------------
echo  Building Package 
echo ------------------
dotnet build .\Source\Prism.CastleWindsor\ -c %Configuration% 
GOTO test

:test
echo ---------------
echo  Running Tests 
echo ---------------
dotnet test .\Source\Prism.CastleWindsor.Wpf.Tests\ || exit /b 1