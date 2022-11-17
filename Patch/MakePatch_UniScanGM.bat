@echo off
cls
rem 경로가 d 드라이브일 경우 echo를 켠다.
if "%1"=="/d" (
	@echo on
)
rem 배치 파일이 있는 경로로 이동
cd /d %~dp0

echo.
echo Select ETC Option
echo 0:All, 1:Config, 2:Launcher
echo ex 1) 1(Config) + 2(Launcher) = 3(Config and Launcher)
rem 코드를 입력 받는다.
set /p ETCCode=Write Code : 

echo.
echo Select Patch Option
echo 0:All, 1:Major
rem 코드를 입력 받는다.
set /p PatchCode=Write Code : 

echo.
echo Select Compress Option
echo 0:Yes, 1:No
rem 코드를 입력 받는다.
set /p CompressCode=Write Code : 

rem All 일 경우 모두 1로 해준다.
if "%ETCCode%"=="0" (
	set ConfigCode=1
	set LauncherCode=1
) else (
	set /a ConfigCode="(%ETCCode%>>0)&1"
	set /a LauncherCode="(%ETCCode%>>1)&1"
)

if "%PatchCode%"=="0" (
	set MajorCode=0
	set IncludeDllCode=1
) else (
	set MajorCode=1
	set IncludeDllCode=0
)

if "%CompressCode%"=="0" (
	set CompressYN=1
) else (
	set CompressYN=0
)

rem 시간표기가 다른 경우를 생각하여 if 문 추가
set HH1=%time:~0,1%
set HH2=%time:~1,1%
set hour=%time:~0,2%
if "%HH1%" == " " set hour=0%HH2%
rem 금일 날짜로 Patch 폴더를 생성한다.
set PatchDirectoryName=UniScanGM_Patch_%date:~0,4%%date:~5,2%%date:~8,2%_%hour%%time:~3,2%%time:~6,2%
set PatchDirectory=.\%PatchDirectoryName%
mkdir %PatchDirectory%

echo.
echo Copy GM
call :CopyPatch %PatchDirectory%"\GM" "GM" "UniScanM.Gloss.exe"
echo Copy GM Done

if "%CompressYN%"=="1" (
	rem 7z으로 압축하기
	7z.exe a %PatchDirectoryName%.7z %PatchDirectory%
	rem 기존 폴더는 삭제
	rmdir /s /q %PatchDirectory%
)

pause
goto :EOF

rem 패치 파일 생성 기능에 관한 내용
:CopyPatch
echo TargetPath: %1
echo TargetPathName: %2
echo Binary: %3

rem 프로그램 구성 요소 복사
if "%MajorCode%"=="1" (
	rem 주요 dll 복사
	mkdir %1\Bin
	copy ..\Build\Release\%2\DynMvp.dll %1\Bin
	copy ..\Build\Release\%2\DynMvp.Data.dll %1\Bin
	copy ..\Build\Release\%2\DynMvp.Device.dll %1\Bin
	copy ..\Build\Release\%2\DynMvp.Vision.dll %1\Bin
	copy ..\Build\Release\%2\UniEye.Base.dll %1\Bin
	copy ..\Build\Release\%2\UniScanM.dll %1\Bin
	rem 실행 파일 복사
	copy ..\Build\Release\%2\%3 %1\Bin
) else (
	rem 모든 구성요소 복사
	xcopy ..\Build\Release\%2\*.* %1\Bin\*.* /e /h /y
)

rem 언어파일 및 설정 파일 복사
if "%ConfigCode%"=="1" (
	mkdir %1\Config
	copy ..\Runtime\Config\StringTable_ko-kr.xml %1\Config
	copy ..\Runtime\Config\StringTable_zh-cn.xml %1\Config
	copy ..\Runtime\Config\log4net.xml %1\Config\
)

if "%LauncherCode%"=="1" (
	echo.
	echo Copy Watcher
	call copy ..\Runtime\UnieyeLauncher.exe %1\%2_Launcher.exe"
	echo Copy Watcher Done
)

echo %date%>%1\Bin\PatchDate