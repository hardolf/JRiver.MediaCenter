@echo off
echo.

:environment
setlocal
set _my_dir=%~dp0
set _my_name=%~n0
set _exit_code=0
set _out_file=BuildRelease.txt
pushd "%_my_dir%"

:build
set _cmd=call BuildRelease.cmd
echo.
echo %_cmd%...
%_cmd%
rem set _exit_code=%errorlevel%

:prompt
echo.
echo Waiting for the build completion before installation...

:wait
sleep 1
if exist %_out_file% goto :wait

echo.
:install_question
set /P _yesno=Install LyricsFinder (YN)? [Y] || Set _yesno=y
if /i "%_yesno:~0,1%" equ "n" goto :end
if /i "%_yesno:~0,1%" equ "y" goto :install
goto :install_question

:install
set _cmd=Output\Setup.exe
echo.
echo %_cmd%...
%_cmd%

if errorlevel 1 goto :error
echo.
echo LyricsFinder installation succeeded, closing...
goto :end

:error
echo.
echo LyricsFinder installation FAILED!
echo.
pause
goto :end

:end
rem endlocal is not used
echo.
rem if %_exit_code% equ 0 (
rem     echo %_my_name% succeeded.
rem ) else (
rem     echo %_my_name% FAILED!
rem )
rem echo.
popd
rem pause
rem exit /b %_exit_code%
