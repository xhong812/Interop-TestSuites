@echo off
pushd %~dp0
"%VS120COMNTOOLS%..\IDE\mstest" /test:Microsoft.Protocols.TestSuites.MS_OXNSPI.S03_ANRRelatedBehavior.MSOXNSPI_S03_TC03_ResolveNamesStringConversion /testcontainer:..\..\MS-OXNSPI\TestSuite\bin\Debug\MS-OXNSPI_TestSuite.dll /runconfig:..\..\MS-OXNSPI\MS-OXNSPI.testsettings /unique
pause