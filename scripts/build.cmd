
rem @echo off

rem 
rem Must be run from the projects git\project\scripts folder - everything is relative
rem run >build [deploymentNumber]
rem deploymentNumber is YYMMDD.build-number, like 190824.5
rem
rem Setup deployment folder
rem


rem all paths are relative to the git scripts folder


set majorVersion=5
set minorVersion=19
set collectionName=aoFormWizard
set collectionPath=..\collections\aoFormWizard\
set solutionName=aoFormWizard.sln
set binPath=..\source\aoFormWizard3\bin\debug\
set msbuildLocation=C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\
set deploymentFolderRoot=C:\Deployments\aoFormWizard\Dev\

rem Setup deployment folder

set year=%date:~12,4%
set month=%date:~4,2%
if %month% GEQ 10 goto monthOk
set month=%date:~5,1%
:monthOk
set day=%date:~7,2%
if %day% GEQ 10 goto dayOk
set day=%date:~8,1%
:dayOk
set versionMajor=%year%
set versionMinor=%month%
set versionBuild=%day%
set versionRevision=1
rem
rem if deployment folder exists, delete it and make directory
rem
:tryagain
set versionNumber=%versionMajor%.%versionMinor%.%versionBuild%.%versionRevision%
if not exist "%deploymentFolderRoot%%versionNumber%" goto :makefolder
set /a versionRevision=%versionRevision%+1
goto tryagain
:makefolder
md "%deploymentFolderRoot%%versionNumber%"

rem ==============================================================
rem
rem copy UI files
rem

rem new install, as zip files
rem layouts are developed in a folder with a subfolder for assets, named catalogassets, etc.
rem when deployed, they are saved in the root folder so the asset subfolder is off the root, to make the html src consistent

cd ..\ui
"c:\program files\7-zip\7z.exe" a "..\collections\aoFormWizard\uiFormWizard.zip" 
cd ..\scripts

pause

rem ==============================================================
rem
echo build 
rem
cd ..\source
"%msbuildLocation%msbuild.exe" %solutionName%
if errorlevel 1 (
   echo failure building
   pause
   exit /b %errorlevel%
)
cd ..\scripts

rem pause

rem ==============================================================
rem
echo Build addon collection
rem

rem remove old DLL files from the collection folder
del "%collectionPath%"\*.DLL
del "%collectionPath%"\*.config

rem copy bin folder assemblies to collection folder
copy "%binPath%*.dll" "%collectionPath%"

rem create new collection zip file
c:
cd %collectionPath%
del "%collectionName%.zip" /Q
"c:\program files\7-zip\7z.exe" a "%collectionName%.zip"
xcopy "%collectionName%.zip" "%deploymentFolderRoot%%versionNumber%" /Y
cd ..\..\scripts

rem pause

rem ==============================================================
rem
echo clear collection folder
rem

cd %collectionPath%

del *.dll
del *.dll.config

del "*.html"
del "*.css"
del "*.js"
del "*.jpg"
del "*.png"
del "*.svg"

cd ..\..\scripts
