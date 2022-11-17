@echo off

cd /d %~dp0

if "%~1" == "" goto LOCAL
git pull %1 head
goto END
 

 :LOCAL
git pull "UniEye.bundle" head
goto END

 :END
pause