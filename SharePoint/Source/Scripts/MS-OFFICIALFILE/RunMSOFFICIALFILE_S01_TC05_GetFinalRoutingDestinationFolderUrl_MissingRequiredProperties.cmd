@echo off
pushd %~dp0
"%VS120COMNTOOLS%..\IDE\mstest" /test:Microsoft.Protocols.TestSuites.MS_OFFICIALFILE.S01_GetRoutingDestinationUrlAndSubmitFile.MSOFFICIALFILE_S01_TC05_GetFinalRoutingDestinationFolderUrl_MissingRequiredProperties /testcontainer:..\..\MS-OFFICIALFILE\TestSuite\bin\Debug\MS-OFFICIALFILE_TestSuite.dll /runconfig:..\..\MS-OFFICIALFILE\MS-OFFICIALFILE.testsettings /unique
pause