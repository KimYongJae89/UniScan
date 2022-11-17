@echo off
cls
if "%1"=="/d" (
	@echo on
)
cd /d %~dp0

title ///////////////////////////  UniScan Backup V.190801 ///////////////////////////
color C

echo Debug mode: use /d flag
echo 0:All, 1:RVMS, 2:Pinhole, 4:ColorSensor, 8:EDMS, 16:StillImage
echo ex) 1(RVMS) + 8(EDMS) = 9(RVMS and EDMS)
set /p code=Code? 

set /a rvmsCode="(%code%>>0)&1"
set /a pinholeCode="(%code%>>1)&1"
set /a colorSensorCode="(%code%>>2)&1"
set /a edmsCode="(%code%>>3)&1"
set /a stillImageCode="(%code%>>4)&1"
if "%code%"=="0" (
	set rvmsCode=1
	set pinholeCode=1
	set colorSensorCode=1
	set edmsCode=1
	set stillImageCode=1
)

echo rvmsCode: %rvmsCode%
echo pinholeCode: %pinholeCode%
echo colorSensorCode: %colorSensorCode%
echo edmsCode: %edmsCode%
echo stillImageCode: %stillImageCode%

if "%rvmsCode%"=="1" (
	echo.
	echo Copy RVMS
	call :Patch "\\192.168.1.100\UniScan\RVMS" "UniScanM.RVMS.exe" "RawDataTemplate_RVMS.xlsx" "UniEyeLauncher_RVMS.xml"
	echo Copy RVMS Done
)

if "%pinholeCode%"=="1" (
	echo.
	echo Copy Pinhole
	call :Patch "\\192.168.1.102\UniScan\Pinhole" "UniScanM.Pinhole.exe" "RawDataTemplate_PinHole.xlsx" "UniEyeLauncher_Pinhole.xml"
	echo Copy Pinhole Done
)

if "%colorSensorCode%"=="1" (
	echo.
	echo Copy ColorSensor
	call :Patch "\\192.168.1.103\UniScan\ColorSensor" "UniScanM.ColorSens.exe" "RawDataTemplate_ColorSensor.xlsx" "UniEyeLauncher_ColorSensor.xml"
	echo Copy ColorSensor Done
)

if "%edmsCode%"=="1" (
	echo.
	echo Copy EDMS
	call :Patch "\\192.168.1.104\UniScan\EDMS" "UniScanM.EDMS.exe" "RawDataTemplate_EDMS.xlsx" "UniEyeLauncher_EDMS.xml"
	echo Copy EDMS Done
)

if "%stillImageCode%"=="1" (
	echo.
	echo Copy StillImage
	call :Patch "\\192.168.1.105\UniScan\StillImage" "UniScanM.StillImage.exe" "RawDataTemplate_StillImage.xlsx" "UniEyeLauncher_StillImage.xml"
	echo Copy StillImage Done
)



pause
goto :EOF

:Patch
echo TargetPath: %1
echo Binary: %2
echo Template: %3
echo Launcher: %4
rem //////////////////////////////////////////////
set hour=%time:~0,2%
set HH1=%time:~0,1%
set HH2=%time:~1,1%
if "%HH1%" == " " set hour=0%HH2%
rem //////////////////////////////////////////////

SET backupdir=%1\backup\%DATE:~0,4%%DATE:~5,2%%DATE:~8,2%_%hour%%time:~3,2%__
echo %backupdir%

rem net use %1 "" /user:administrator

xcopy %1\bin %backupdir%\bin\ /h /k /y /e
xcopy %1\config %backupdir%\config\ /h /k /y /e
MD %backupdir%\Result
copy %1\Result\*.xlsx %backupdir%\Result\

REM ////////////////////////////////////////////////////////////////////////////////
echo REM Restore......................................................... > %backupdir%\Restore.BAT
echo xcopy .\bin  .\..\..\Bin\  /h /k /y /e >>%backupdir%\Restore.BAT
echo xcopy .\config  .\..\..\Config\  /h /k /y /e >>%backupdir%\Restore.BAT
echo xcopy .\Result  .\..\..\Result\  /h /k /y /e >>%backupdir%\Restore.BAT
echo pause >>%backupdir%\Restore.BAT
REM ////////////////////////////////////////////////////////////////////////////////

rem net use %1 /delete

