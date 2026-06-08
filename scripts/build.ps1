#Requires -Version 5.1
[CmdletBinding()]
param(
    [string]   $LocalDeployTarget  = '',
    [hashtable]$RemoteDeployTarget = $null
)

$ErrorActionPreference = 'Stop'

Import-Module (Join-Path $PSScriptRoot '..\..\Contensive5\scripts\build-addon-collection.psm1') -Force

$projectRoot = (Resolve-Path "$PSScriptRoot\..").Path

Invoke-ContensiveBuild `
    -CollectionName    'Form Wizard' `
    -CollectionPath    "$projectRoot\Collections\aoFormWizard" `
    -SolutionPath      "$projectRoot\Source\aoFormWizard.sln" `
    -BinPath           "$projectRoot\Source\aoFormWizard3\bin\Release\netstandard2.0" `
    -DeploymentRoot    'C:\Deployments\aoFormWizard' `
    -CleanFolders      @(
                           "$projectRoot\Source\aoFormWizard3\bin"
                           "$projectRoot\Source\aoFormWizard3\obj"
                       ) `
    -UiPath            "$projectRoot\ui" `
    -LocalDeployTarget  $LocalDeployTarget `
    -RemoteDeployTarget $RemoteDeployTarget
