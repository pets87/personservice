@echo off


dotnet tool list -g | findstr /C:"dotnet-reportgenerator-globaltool" >nul
IF ERRORLEVEL 1 (
    echo Installing dotnet-reportgenerator-globaltool...
    dotnet tool install -g dotnet-reportgenerator-globaltool
)

set TEST_RESULTS_DIR=PersonService.Tests\TestResults
set COVERAGE_JSON=coverage.json

if exist "%TEST_RESULTS_DIR%" (
    echo Deleting old files from %TEST_RESULTS_DIR%...
    rmdir /s /q "%TEST_RESULTS_DIR%"
) else (
    echo Directory %TEST_RESULTS_DIR% does not exist, skipping deletion.
)

if exist "%COVERAGE_JSON%" (
    echo Deleting %COVERAGE_JSON% from root directory...
    del /f "%COVERAGE_JSON%"
) else (
    echo %COVERAGE_JSON% not found, skipping deletion.
)

dotnet test /p:CollectCoverage=true --collect:"XPlat Code Coverage"

for /f "delims=" %%i in ('dir /s /b coverage.cobertura.xml') do (
    reportgenerator -reports:"%%i" -targetdir:"%TEST_RESULTS_DIR%\Coverage" -reporttypes:Html
)
