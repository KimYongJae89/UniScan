@echo off
cd /d %~dp0
cls

if "%1"=="" (
	:: 인자 없음 -> 수동 입력. 패치 후 화면 닫지 않음.
	echo. 0:All, 1:Controller, 2:Monitor, 4:Inspector, 8:Observer
	echo. "ex) 1(Controller) + 4(Inspector) = 5(Controller and Inspector)"
	set /p code=Code? 
) else (
	:: 인자 하나 이상 -> 패치 후 화면 자동 닫기.
	if "%2"=="" (
		:: 인자 하나 -> 입력받은 인자가 코드임
		set code=%1
	) else (
		:: 인자 둘 -> 입력받은 인자는 주소와 대상임.
		call :Patch %1 %2
		goto :EOF
	)
)

if "%code%"=="0" (
	set controllerCode=1
	set monitorCode=1
	set inspectorCode=1
	set observerCode=1
set
) else (
	set /a controllerCode="(%code%>>0)&1"
	set /a monitorCode="(%code%>>1)&1"
	set /a inspectorCode="(%code%>>2)&1"
	set /a observerCode="(%code%>>3)&1"
)

if "%controllerCode%"=="1" (
	echo.
	echo Copy Controller
	call :Patch "\\192.168.50.1\UniScan\Gravure_Controller" "UniScanG.Module.Controller"
)

if "%monitorCode%"=="1" (
	echo.
	echo Copy Monitor
	call :Patch "\\192.168.50.2\UniScan\Gravure_Monitor" "UniScanG.Module.Monitor"
	echo.
)

if "%inspectorCode%"=="1" (
	echo.
	echo Copy Inspector
	call :Patch "\\192.168.50.10\UniScan\Gravure_Inspector" "UniScanG.Module.Inspector"
	echo.
	call :Patch "\\192.168.50.11\UniScan\Gravure_Inspector" "UniScanG.Module.Inspector"
	echo.
	call :Patch "\\192.168.50.20\UniScan\Gravure_Inspector" "UniScanG.Module.Inspector"
	echo.
	call :Patch "\\192.168.50.21\UniScan\Gravure_Inspector" "UniScanG.Module.Inspector"
	echo.
)

if "%observerCode%"=="1" (
	echo.
	echo Copy Observer
	call :Patch "\\192.168.50.3\UniScan\Gravure_Observer" "UniScanG.Module.Observer"
	echo.
)

if "%1"=="" (
	pause
)
goto :EOF

:Patch
echo --------------------------------------------------
echo TargetPath: %1
echo Binary: %2

echo. Chacking...
net use "%1" "" /user:administrator
if exist %1\ (
	echo. Patching...
	copy /y Release\UnieyeLauncher.exe %1\Update\>nul

	if not exist %1\Update\Config\ (
		mkdir %1\Update\Config\
	)
	copy /y ..\Runtime\Config\StringTable_ko-kr.xml %1\Update\Config\>nul
	copy /y ..\Runtime\Config\StringTable_zh-cn.xml %1\Update\Config\>nul
	copy /y ..\Runtime\Config\LocalizeHelper_ko-kr.xml %1\Update\Config\>nul
	copy /y ..\Runtime\Config\LocalizeHelper_zh-cn.xml %1\Update\Config\>nul
	copy /y ..\Runtime\Config\log4net.xml %1\Update\Config\>nul
	
	if not exist %1\Update\bin\ (
		mkdir %1\Update\bin\
	)
	copy /y Release\UserManager.exe %1\Update\bin>nul
	copy /y Release\LicenseManager.exe %1\Update\bin>nul
	
	copy /y Release\*.dll %1\Update\bin\>nul
	copy /y Release\*.pdb %1\Update\bin\>nul

	copy /y Release\%2.exe %1\Update\bin>nul
	copy /y Release\%2.pdb %1\Update\bin>nul
	copy /y Release\%2.exe.config %1\Update\bin>nul
	
	echo %date%>%1\Update\PatchDate
	echo. Done.
) else (
	echo. Target is not exist.
)
net use /d "%1"