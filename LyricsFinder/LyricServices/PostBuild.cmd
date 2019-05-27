@echo off
echo.

rem ***********************************************
rem ************ Main routine begin ***************
rem ***********************************************

:environment
setlocal enabledelayedexpansion
set _my_dir=%~dp0
set _my_name=%~n0
set _cmd=
set _exit_code=0
set _out_dir=%~1
set _src_dir=%~2
set _src_file=%~3
set _msg_inspect=..\..\SharedComponents\MessageInspection\%_out_dir%MessageInspection.*
set _tdirs=..\Installation\Build\LyricServices\

:start
echo %_my_name% started...
echo.
pushd "%_my_dir%"

:check
if "%_src_file%" equ "" (
    echo.
	echo Syntax: %_my_name%  out-dir  source-dir  source-file  [source-file  ...]
	set _exit_code=1
    goto :end
)

:run
set _cmd=xcopy /e /i /v /y "%_msg_inspect%" "%_src_dir%"
echo.
echo %_cmd%...
%_cmd%
set _cmd=xcopy /e /i /v /y "%_msg_inspect%" "%_src_dir%%_out_dir%"
echo.
echo %_cmd%...
%_cmd%
for /d %%d in (%_tdirs%) do call :copy_dir "%_msg_inspect%" "%%d"

:run_next
if %_exit_code% neq 0 goto :end
if "%_src_file%" equ "" goto :end
for /d %%d in (%_tdirs%) do call :copy_dir "%_src_dir%%_out_dir%%_src_file%" "%%d"
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
set _cmd=xcopy /e /i /v /y "%_sfile%" "%_tdir%"
echo.
echo %_cmd%...
%_cmd%
exit /b %_exit_code%

rem ***********************************************
rem ************* Sub routines end ****************
rem ***********************************************
