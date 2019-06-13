@echo off
echo.

rem ----------------------------------------
rem Prepare the LyricsFinder release package
rem Subroutine - do not call directly
rem ----------------------------------------

:environment
setlocal
set _my_dir=%~dp0
set _my_name=%~n0
set _exit_code=0
set _cmd=MSBuild.exe ..\..\LyricsFinder.sln -property:Configuration=Release
set _vs_versions=2017 2019
set _vs_net_var1=C:\Program Files (x86)\Microsoft Visual Studio
set _vs_net_var2=Community\Common7\Tools\VsDevCmd.bat
set _vs_net_var=
echo.

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
:no_vs
rem -----------------------
echo.
echo Microsoft Visual Studio 2019 is not installed on this machine!
set _exit_code=1
goto :end
rem -----------------------
