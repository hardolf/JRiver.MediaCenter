@echo off
echo.

rem ----------------------------------------
rem Prepare the LyricsFinder release package
rem Automatically runs in elevated mode
rem ----------------------------------------
rem Automatically check & get admin rights
rem Source inspired by:
rem https://stackoverflow.com/questions/7044985/how-can-i-auto-elevate-my-batch-file-so-that-it-requests-from-uac-administrator
rem https://stackoverflow.com/a/12264592/1016343
rem ----------------------------------------

echo **************************************
echo Running script in elevated mode
echo **************************************

set _debug=0

:init
if '%_debug%'=='1' echo init
setlocal DisableDelayedExpansion
set _cmdInvoke=0
set _winSysFolder=System32
set "_batchPath=%~0"
for %%k in (%0) do set _batchName=%%~nk
set "vbsGetPrivileges=%temp%\OEgetPriv_%_batchName%.vbs"
rem set "vbsGetPrivileges=OEgetPriv_%_batchName%.vbs"
if '%_debug%'=='1' set
setlocal EnableDelayedExpansion
 
:checkPrivileges
if '%_debug%'=='1' echo checkPrivileges
NET FILE 1>NUL 2>NUL
if '%errorlevel%'=='0' ( goto gotPrivileges ) else ( goto getPrivileges )

:getPrivileges
if '%_debug%'=='1' echo getPrivileges
if '%1'=='ELEV' (echo ELEV & shift /1 & goto gotPrivileges)
echo.
echo **************************************
echo Invoking UAC for privilege escalation
echo **************************************

echo Set UAC = CreateObject^("Shell.Application"^) > "%vbsGetPrivileges%"
echo args = "ELEV " >> "%vbsGetPrivileges%"
echo For Each strArg in WScript.Arguments >> "%vbsGetPrivileges%"
echo args = args ^& strArg ^& " "  >> "%vbsGetPrivileges%"
echo Next >> "%vbsGetPrivileges%"

if '%_cmdInvoke%'=='1' goto invokeCmd 

echo UAC.ShellExecute "!_batchPath!", args, "", "runas", 1 >> "%vbsGetPrivileges%"
goto execElevation

:invokeCmd
if '%_debug%'=='1' echo invokeCmd
echo args = "/c """ + "!_batchPath!" + """ " + args >> "%vbsGetPrivileges%"
echo UAC.ShellExecute "%SystemRoot%\%_winSysFolder%\cmd.exe", args, "", "runas", 1 >> "%vbsGetPrivileges%"

:execElevation
if '%_debug%'=='1' echo execElevation
"%SystemRoot%\%_winSysFolder%\WScript.exe" "%vbsGetPrivileges%" %*
set _exit_level=%errorlevel%
if '%_debug%'=='1' echo ErrorLevel = %_exit_level%
exit /b %_exit_level%

:gotPrivileges
if '%_debug%'=='1' echo gotPrivileges
setlocal & cd /d %~dp0
if '%1'=='ELEV' (del "%vbsGetPrivileges%" 1>nul 2>nul  &  shift /1)

:run
if '%_debug%'=='1' echo run
rem Run shell as admin (example) - put here code as you like
if '%_debug%'=='1' echo %_batchName% Arguments: P1=%1 P2=%2 P3=%3 P4=%4 P5=%5 P6=%6 P7=%7 P8=%8 P9=%9
if '%_debug%'=='1' echo cmd.exe /c %1 %2 %3 %4 %5 %6 %7 %8 %9

cmd.exe /c %1 %2 %3 %4 %5 %6 %7 %8 %9
set _exit_level=%errorlevel%

echo.
if '%_debug%'=='1' echo ErrorLevel = %_exit_level%
if '%_debug%'=='1' pause
endlocal & exit /b %_exit_level%
