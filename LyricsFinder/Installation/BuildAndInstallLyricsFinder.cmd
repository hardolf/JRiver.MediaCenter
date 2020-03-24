@echo off
echo.

:environment
setlocal
set _my_dir=%~dp0
set _my_name=%~n0
set _exit_code=0
set _cmd=call Elevate.cmd BuildRelease.subroutine.cmd
pushd "%_my_dir%"

:run
echo.
echo %_cmd%...
%_cmd%
rem set _exit_code=%errorlevel%

:prompt
echo.
echo Wait for the build completion before answering the next question...
echo.
set /P _yesno=Install LyricsFinder (YN)? [Y] || Set _yesno=y
if /i "%_yesno%" equ "n" goto end
echo.
Output\Setup.exe

:end
rem endlocal is not used
rem echo.
rem if %_exit_code% equ 0 (
rem     echo %_my_name% succeeded.
rem ) else (
rem     echo %_my_name% FAILED!
rem )
rem echo.
popd
rem pause
rem exit /b %_exit_code%
