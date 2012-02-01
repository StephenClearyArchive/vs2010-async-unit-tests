@echo off
if not exist ..\Binaries mkdir ..\Binaries
if not exist ..\Binaries\NuGet mkdir ..\Binaries\NuGet
..\Util\nuget.exe pack -sym AsyncUnitTests-MSTest.nuspec -o ..\Binaries\NuGet
pause