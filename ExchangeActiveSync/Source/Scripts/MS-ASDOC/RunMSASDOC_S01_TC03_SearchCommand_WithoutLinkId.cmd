@echo off
pushd %~dp0
"%VS120COMNTOOLS%..\IDE\mstest" /test:Microsoft.Protocols.TestSuites.MS_ASDOC.S01_SearchCommand.MSASDOC_S01_TC03_SearchCommand_WithoutLinkId /testcontainer:..\..\MS-ASDOC\TestSuite\bin\Debug\MS-ASDOC_TestSuite.dll /runconfig:..\..\MS-ASDOC\MS-ASDOC.testsettings /unique
pause