@echo off
echo.

rem ----------------------------------------
rem Prepare the LyricsFinder release package
rem Automatically runs in elevated mode
rem ----------------------------------------
rem Elevated sources:
rem https://stackoverflow.com/questions/7044985/how-can-i-auto-elevate-my-batch-file-so-that-it-requests-from-uac-administrator
rem https://stackoverflow.com/a/12264592/1016343
rem ----------------------------------------

:environment
setlocal ENABLEDELAYEDEXPANSION
set _my_dir=%~dp0
set _my_name=%~n0
set _exit_code=0
set _cmd=MSBuild.exe ..\..\LyricsFinder.sln -property:Configuration=Release
set _vs_versions=2017 2019
set _vs_net_var1=C:\Program Files (x86)\Microsoft Visual Studio
set _vs_net_var2=Community\Common7\Tools\VsDevCmd.bat
set _vs_net_var=

:init
call :init

:check
for %%v in (%_vs_versions%) do if exist "%_vs_net_var1%\%%v\%_vs_net_var2%" set _vs_net_var=%_vs_net_var1%\%%v\%_vs_net_var2%
if not exist "%_vs_net_var%" goto :no_vs

:run
pushd "%_my_dir%"
call "%_vs_net_var%"
call :execute

:end
rem endlocal is not used
echo.
if %_exit_code% equ 0 (
    echo %_my_name% succeeded.
) else (
    echo %_my_name% FAILED!
)
echo.
popd
pause
exit /b %_exit_code%


rem ***********************
rem ***** Subroutines *****
rem ***********************

rem -----------------------
:execute
rem -----------------------
echo.
echo %_cmd%
%_cmd%
set _exit_code=%ERRORLEVEL%
exit /b %_exit_code%
rem -----------------------


rem -----------------------
:init
rem -----------------------
 setlocal DisableDelayedExpansion
 set cmdInvoke=1
 set winSysFolder=System32
 set "batchPath=%~0"
 for %%k in (%0) do set batchName=%%~nk
 set "vbsGetPrivileges=%temp%\OEgetPriv_%batchName%.vbs"
 setlocal EnableDelayedExpansion
 
:checkPrivileges
  NET FILE 1>NUL 2>NUL
  if '%errorlevel%' == '0' ( goto gotPrivileges ) else ( goto getPrivileges )

:getPrivileges
  if '%1'=='ELEV' (echo ELEV & shift /1 & goto gotPrivileges)
  echo.
  echo **************************************
  echo Invoking UAC for Privilege Escalation
  echo **************************************

  echo Set UAC = CreateObject^("Shell.Application"^) > "%vbsGetPrivileges%"
  echo args = "ELEV " >> "%vbsGetPrivileges%"
  echo For Each strArg in WScript.Arguments >> "%vbsGetPrivileges%"
  echo args = args ^& strArg ^& " "  >> "%vbsGetPrivileges%"
  echo Next >> "%vbsGetPrivileges%"

  if '%cmdInvoke%'=='1' goto InvokeCmd 

  echo UAC.ShellExecute "!batchPath!", args, "", "runas", 1 >> "%vbsGetPrivileges%"
  goto ExecElevation

:InvokeCmd
  echo args = "/c """ + "!batchPath!" + """ " + args >> "%vbsGetPrivileges%"
  echo UAC.ShellExecute "%SystemRoot%\%winSysFolder%\cmd.exe", args, "", "runas", 1 >> "%vbsGetPrivileges%"

:ExecElevation
 "%SystemRoot%\%winSysFolder%\WScript.exe" "%vbsGetPrivileges%" %*
 exit /B

:gotPrivileges
 setlocal & cd /d %~dp0
 if '%1'=='ELEV' (del "%vbsGetPrivileges%" 1>nul 2>nul  &  shift /1)

exit /b %_exit_code%
rem -----------------------


rem -----------------------
:no_vs
rem -----------------------
echo.
echo Microsoft Visual Studio 2019 is not installed on this machine!
set _exit_code=1
goto :end
rem -----------------------
