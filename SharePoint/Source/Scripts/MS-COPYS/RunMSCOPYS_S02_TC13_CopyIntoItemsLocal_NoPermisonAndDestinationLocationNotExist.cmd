@echo off
pushd %~dp0
"%VS120COMNTOOLS%..\IDE\mstest" /test:Microsoft.Protocols.TestSuites.MS_COPYS.S02_CopyIntoItemsLocal.MSCOPYS_S02_TC13_CopyIntoItemsLocal_NoPermisonAndDestinationLocationNotExist /testcontainer:..\..\MS-COPYS\TestSuite\bin\Debug\MS-COPYS_TestSuite.dll /runconfig:..\..\MS-COPYS\MS-COPYS.testsettings /unique
pause