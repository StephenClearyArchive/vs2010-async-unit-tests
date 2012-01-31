$project = Get-Project
$regSubkey = "\EnterpriseTools\QualityTools\TestTypes\{13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b}\TestTypeExtensions\AsyncTestClassAttribute"
$machineReg = "hklm:\" + $project.DTE.RegistryRoot
$userReg = "hkcu:\" + $project.DTE.RegistryRoot
$machineConfigReg = "hklm:\" + $project.DTE.RegistryRoot + "_Config"
$userConfigReg = "hkcu:\" + $project.DTE.RegistryRoot + "_Config"
$installDir = (Get-ItemProperty -Path ($machineReg + "\Setup\VS")).ProductDir + "Common7\IDE\PrivateAssemblies"
$existingDllPath = $installDir + "\Nito.AsyncEx.UnitTests.dll"
$targetTemplatePath = (Get-ItemProperty -Path $userReg).UserItemTemplatesLocation + "\Visual C#\AsyncUnitTest.zip"

function Uninstall($path)
{
  if (Test-Path $path)
  {
    "Deleting " + $path
    Remove-Item -Path $path -Recurse
  }
}

Uninstall ($machineReg + $regSubkey)
Uninstall ($userReg + $regSubkey)
Uninstall ($machineConfigReg + $regSubkey)
Uninstall ($userConfigReg + $regSubkey)
Uninstall $existingDllPath
Uninstall $targetTemplatePath

"Uninstall completed."
