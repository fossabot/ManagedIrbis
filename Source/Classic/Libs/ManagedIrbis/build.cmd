@echo off

SET MSBUILD=C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m

"%MSBUILD%" ManagedIrbis.csproj /p:Configuration=Debug /p:TargetFrameworkVersion=4.5 %PARAMS% /p:DefineConstants="CLASSIC;DESKTOP;FW4;FW45" /t:Rebuild
