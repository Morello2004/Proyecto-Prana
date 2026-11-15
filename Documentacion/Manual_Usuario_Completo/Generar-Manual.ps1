param([switch]$SoloHtml)
$ErrorActionPreference = 'Stop'
& (Join-Path $PSScriptRoot 'Generar-Edicion-Final.ps1') -SoloHtml:$SoloHtml