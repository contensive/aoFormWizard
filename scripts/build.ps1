#Requires -Version 5.1
<#
.SYNOPSIS
    aoFormWizard collection build — configuration only.
    All build steps are defined in the shared Contensive build library.
    Entry point: build.cmd
.PARAMETER Configuration
    Build configuration (Debug or Release). Defaults to Debug.
#>
[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug'
)

$ErrorActionPreference = 'Stop'

Import-Module (Join-Path $PSScriptRoot '..\..\Contensive5\scripts\contensive-build.psm1') -Force

$projectRoot = (Resolve-Path "$PSScriptRoot\..").Path

Invoke-ContensiveBuild `
    -CollectionName    'aoFormWizard' `
    -CollectionPath    "$projectRoot\Collections\aoFormWizard" `
    -SolutionPath      "$projectRoot\Source\aoFormWizard.sln" `
    -BinPath           "$projectRoot\Source\aoFormWizard3\bin\$Configuration\net48" `
    -DeploymentRoot    'C:\Deployments\aoFormWizard' `
    -Configuration     $Configuration `
    -CleanFolders      @(
                           "$projectRoot\Source\aoFormWizard3\bin"
                           "$projectRoot\Source\aoFormWizard3\obj"
                       ) `
    -UiPath            "$projectRoot\ui" `
    -PackagesDirectory "$projectRoot\Source\packages"
