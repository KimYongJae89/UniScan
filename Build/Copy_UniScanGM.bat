@echo off
cls
rem 경로가 d 드라이브일 경우 echo를 켠다.
if "%1"=="/d" (
	@echo on
)
rem 배치 파일이 있는 경로로 이동
cd /d %~dp0

echo.
echo Select Build Option
echo 0:All, 1:Bebug, 2:Release
echo ex 1) 1(Bebug) + 2(Release) = 3(Bebug and Release)
rem 코드를 입력 받는다.
set /p BuildCode=Write Code : 

rem All 일 경우 모두 1로 해준다.
if "%BuildCode%"=="0" (
	set DebugCode=1
	set ReleaseCode=1
) else (
	set /a DebugCode="(%BuildCode%>>0)&1"
	set /a ReleaseCode="(%BuildCode%>>1)&1"
)

set UniScanGMDirectory=..\Source\UniScanM

echo.
echo Copy GM
if "%DebugCode%"=="1" (
	call :CopyBuild %UniScanGMDirectory%"\UniScanM.Gloss\bin\x64\Debug\" "GM" "Debug"
)
if "%ReleaseCode%"=="1" (
	call :CopyBuild %UniScanGMDirectory%"\UniScanM.Gloss\bin\x64\Release\" "GM" "Release"
)
echo Copy GM Done

pause
goto :EOF

rem 패치 파일 생성 기능에 관한 내용
:CopyBuild
echo SourcePath: %1
echo DestinationPathName: %2
echo DuildCode: %3

rem 디렉토리 복사
xcopy %1\*.* .\%3\%2\*.* /e /h /y
rem pdb 형식 파일은 삭제
del .\%3\%2\*.pdb