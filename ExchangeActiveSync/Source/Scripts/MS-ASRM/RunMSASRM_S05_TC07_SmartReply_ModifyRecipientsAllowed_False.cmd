@echo off
pushd %~dp0
"%VS120COMNTOOLS%..\IDE\mstest" /test:Microsoft.Protocols.TestSuites.MS_ASRM.S05_SmartReply.MSASRM_S05_TC07_SmartReply_ModifyRecipientsAllowed_False /testcontainer:..\..\MS-ASRM\TestSuite\bin\Debug\MS-ASRM_TestSuite.dll /runconfig:..\..\MS-ASRM\MS-ASRM.testsettings /unique
pause