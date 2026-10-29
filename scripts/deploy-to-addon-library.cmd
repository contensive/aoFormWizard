@echo off
setlocal

rem Uploads the built collection to the Addon Collection Library on contensive.com.

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0deploy-to-addon-library.ps1" ^
    -CollectionName "Form Wizard" ^
    -CollectionPath "%~dp0..\Collections\aoFormWizard" ^
    -DeploymentPath "C:\Deployments\aoFormWizard" ^
    -UiPath "%~dp0..\ui"

set exitCode=%errorlevel%
pause
exit /b %exitCode%
