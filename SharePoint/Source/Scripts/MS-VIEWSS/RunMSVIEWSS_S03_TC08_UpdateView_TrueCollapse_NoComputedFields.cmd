@echo off
pushd %~dp0
"%VS120COMNTOOLS%..\IDE\mstest" /test:Microsoft.Protocols.TestSuites.MS_VIEWSS.S03_MaintainViewDefinition.MSVIEWSS_S03_TC08_UpdateView_TrueCollapse_NoComputedFields /testcontainer:..\..\MS-VIEWSS\TestSuite\bin\Debug\MS-VIEWSS_TestSuite.dll /runconfig:..\..\MS-VIEWSS\MS-VIEWSS.testsettings /unique
pause