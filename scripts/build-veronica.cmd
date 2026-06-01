@echo off
setlocal

rem all paths are relative to the git scripts folder

set appName=veronica
set collectionName=Form Wizard

call build.cmd /nopause

if not "%errorlevel%"=="0" (
    echo Build failed, skipping deploy.
    pause
    exit /b 1
)

rem find the latest deployment folder
set deploymentRoot=C:\Deployments\aoFormWizard
for /f "delims=" %%i in ('dir "%deploymentRoot%" /b /ad /o-d ^| findstr "^[0-9]"') do (
    set latestFolder=%%i
    goto :found
)
:found

rem upload to contensive application
c:
cd "%deploymentRoot%\%latestFolder%"
cc -a %appName% --installFile "%collectionName%.zip"
cd %~dp0

pause
