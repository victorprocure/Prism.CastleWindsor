@ECHO OFF

:build
echo ------------------
echo  Building Package 
echo ------------------
dotnet build ..\Source\Prism.CastleWindsor\
GOTO test

:test
echo ---------------
echo  Running Tests 
echo ---------------
dotnet test ..\Source\Prism.CastleWindsor.Wpf.Tests\