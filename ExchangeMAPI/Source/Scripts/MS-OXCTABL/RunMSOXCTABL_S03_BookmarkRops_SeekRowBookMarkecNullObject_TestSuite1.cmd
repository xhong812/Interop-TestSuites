@echo off
pushd %~dp0
"%VS120COMNTOOLS%..\IDE\mstest" /test:Microsoft.Protocols.TestSuites.MS_OXCTABL.S03_BookmarkRops_SeekRowBookMarkecNullObject_TestSuite.MSOXCTABL_S03_BookmarkRops_SeekRowBookMarkecNullObject_TestSuite1 /testcontainer:..\..\MS-OXCTABL\TestSuite\bin\Debug\MS-OXCTABL_TestSuite.dll /runconfig:..\..\MS-OXCTABL\MS-OXCTABL.testsettings /unique
pause