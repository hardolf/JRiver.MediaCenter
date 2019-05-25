@echo off
echo.

rem ***********************************************
rem ************ Main routine begin ***************
rem ***********************************************

:environment
setlocal enabledelayedexpansion
set _my_dir=%~dp0
set _my_name=%~n0
set _exit_code=0
set _config=%~1
set _src_dir=%~2
set _src_file=%~3
set _msg_inspect=..\..\Shared\MessageInspection\bin\%_config%\MessageInspection.*
set _tdirs=..\Installation\Build\LyricServices\ ..\LyricsFinderExe\bin\%_config%\

:start
echo %_my_name% started...
echo.
pushd "%_my_dir%"

:check
if "%_src_file%" equ "" (
    echo.
	echo Syntax: %_my_name%  configuration  source-dir  source-file  [source-file  ...]
	set _exit_code=1
    goto :end
)

:run
echo Copy Message inspector...
xcopy /y "%_msg_inspect%" "%_src_dir%"
xcopy /y "%_msg_inspect%" "%_src_dir%bin\%_config%\"
for /d %%d in (%_tdirs%) do call :copy_dir "%_msg_inspect%" "%%d"

:run_next
if %_exit_code% neq 0 goto :end
if "%_src_file%" equ "" goto :end
for /d %%d in (%_tdirs%) do call :copy_dir "%_src_dir%bin\%_config%\%_src_file%" "%%d"
shift
set _src_file=%~3
goto :run_next

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
exit /b %_exit_code%

rem ***********************************************
rem ************* Main routine end ****************
rem ***********************************************


rem ***********************************************
rem ************ Sub routines begin****************
rem ***********************************************

:copy_dir
if %_exit_code% neq 0 exit /b %_exit_code%
set _sfile=%~1
set _tdir=%~2
if not exist "%_tdir%" md "%_tdir%"
echo.
echo Copy "%_sfile%" to "%_tdir%"...
xcopy /y "%_sfile%" "%_tdir%"
exit /b %_exit_code%

rem ***********************************************
rem ************* Sub routines end ****************
rem ***********************************************
