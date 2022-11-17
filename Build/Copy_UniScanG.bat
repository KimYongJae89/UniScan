@echo off
cls
if "%1"=="/d" (
	@echo on
)
cd /d %~dp0

echo Debug mode: use /d flag
echo 0:All, 1:Monitor, 2:Watcher, 4:Inspector
echo ex) 1(Monitor) + 4(Inspector) = 5(Monitor and Inspector)
set /p code=Code? 

if "%code%"=="0" (
	set monitorCode=1
	set watcherCode=1
	set inspectorCode=1
) else (
set /a monitorCode="(%code%>>0)&1"
set /a watcherCode="(%code%>>1)&1"
set /a inspectorCode="(%code%>>2)&1"
)

if "%monitorCode%"=="1" (
	echo.
	echo Copy Monitor
	call :Patch "\\Gravure-Insp\Gravure_Monitor" "UniScan.Monitor.exe"
	REM call :Patch "D:\UniScan\Gravure_Monitor" "UniScan.Monitor.exe"
	echo Copy Monitor Done
)

if "%watcherCode%"=="1" (
	echo.
	echo Copy Watcher
	call :Patch "\\192.168.50.2\Gravure_Watcher" "UniScan.Watch.exe"
	REM call :Patch "D:\UniScan\Gravure_Watcher" "UniScan.Watch.exe"
	echo Copy Watcher Done
)

if "%inspectorCode%"=="1" (
	echo.
	echo Copy Inspector
	call :Patch "\\CAM1A\Gravure_Inspector" "UniScan.Inspector.exe"
	call :Patch "\\CAM1B\Gravure_Inspector" "UniScan.Inspector.exe"
	call :Patch "\\CAM2A\Gravure_Inspector" "UniScan.Inspector.exe"
	call :Patch "\\CAM2B\Gravure_Inspector" "UniScan.Inspector.exe"
	REM call :Patch "D:\UniScan\Gravure_Inspector" "UniScan.Inspector.exe"
	echo Copy Inspector Done
)

pause
goto :EOF

:Patch
echo TargetPath: %1
echo Binary: %2

copy ..\Runtime\Config\StringTable_ko-kr.xml %1\Config\
copy ..\Runtime\Config\StringTable_zh-cn.xml %1\Config\
copy ..\Runtime\Config\log4net.xml %1\Config\

REM copy Release\UnieyeLauncher.exe %1\bin

copy Release\UserManager.exe %1\bin

copy Release\DynMvp.dll %1\Update
copy Release\DynMvp.Data.dll %1\Update
copy Release\DynMvp.Device.dll %1\Update
copy Release\DynMvp.Vision.dll %1\Update
copy Release\UniEye.Base.dll %1\Update
copy Release\UniScanG.dll %1\Update
copy Release\UniScan.Common.dll %1\Update

copy Release\%2 %1\Update

echo %date%>%1\Update\PatchDate