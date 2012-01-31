$regSubkey = "\EnterpriseTools\QualityTools\TestTypes\{13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b}\TestTypeExtensions\AsyncTestClassAttribute"
$regName = "AttributeProvider"
$regValue = "Nito.AsyncEx.UnitTests.AsyncTestClassAttribute, Nito.AsyncEx.UnitTests"

$machineReg = "hklm:\" + $project.DTE.RegistryRoot
$userReg = "hkcu:\" + $project.DTE.RegistryRoot
$machineConfigReg = "hklm:\" + $project.DTE.RegistryRoot + "_Config"
$userConfigReg = "hkcu:\" + $project.DTE.RegistryRoot + "_Config"

$installDir = (Get-ItemProperty -Path ($machineReg + "\Setup\VS")).ProductDir + "Common7\IDE\PrivateAssemblies"
$existingDllPath = $installDir + "\Nito.AsyncEx.UnitTests.dll"
$myDllPath = $toolsPath + "\Nito.AsyncEx.UnitTests.dll"
$overwriteDlls = $true

if (Test-Path $existingDllPath)
{
  $existingDll = Get-Command $existingDllPath
  $existingDllVersion = New-Object Version -ArgumentList $existingDll.FileVersionInfo.FileVersion
  $myDll = Get-Command $myDllPath
  $myDllVersion = New-Object Version -ArgumentList $myDll.FileVersionInfo.FileVersion
  $overwriteDlls = $myDllVersion -gt $existingDllVersion
}

function Register($regRoot)
{
  $regKey = $regRoot + $regSubKey
  if (Test-Path $regKey)
  {
    New-ItemProperty -Path $regKey -Name $regName -Value $regValue -Force
  }
  else
  {
    New-Item $regKey
    New-ItemProperty -Path $regKey -Name $regName -Value $regValue
  }
}

if ($overwriteDlls)
{
  Copy-Item -Path $myDllPath -Destination $existingDllPath -Force
  Register $machineReg
  Register $userReg
  Register $machineConfigReg
  Register $userConfigReg
}

$myTemplatePath = $toolsPath + "\AsyncUnitTest.zip"
$targetTemplatePath = (Get-ItemProperty -Path $userReg).UserItemTemplatesLocation + "\Visual C#\AsyncUnitTest.zip"
$overwriteTemplate = true

if (Test-Path $targetTemplatePath)
{
  $targetTemplate = Get-Item $targetTemplatePath
  $myTemplate = Get-Item $myTemplatePath
  if ($myTemplate.LastWriteTime -lt $targetTemplate.LastWriteTime)
    $overwriteTemplate = false
}

if ($overwriteTemplate)
{
  Copy-Item -Path $myTemplatePath -Destination $targetTemplatePath -Force
}
