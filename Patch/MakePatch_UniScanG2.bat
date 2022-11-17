@echo off
cls

set hour=%time:~0,2%
set HH1=%time:~0,1%
set HH2=%time:~1,1%
if "%HH1%" == " " set hour=0%HH2%
set buildInfo=%DATE:~2,2%%DATE:~5,2%%DATE:~8,2%_%hour%%time:~3,2%
Set compFile=UniScanG_%buildInfo%.7z

:: 스위치가 없으면 압축 여부 물어봄
set YN=%1
if not "%YN%"=="" (
set YN=%YN:~-1,1%
)

if "%YN%"=="" (
set /p YN=If you want to Compress .7z, Press 'Y' [Y/N] :
)

echo %buildInfo%>.\UniScanG\ReleaseDate.txt

mkdir .\UniScanG\Build\Release
copy ..\Build\Release\DynMvp.dll .\UniScanG\Build\Release\
copy ..\Build\Release\DynMvp.pdb .\UniScanG\Build\Release\
copy ..\Build\Release\DynMvp.Data.dll .\UniScanG\Build\Release\
copy ..\Build\Release\DynMvp.Data.pdb .\UniScanG\Build\Release\
copy ..\Build\Release\DynMvp.Device.dll .\UniScanG\Build\Release\
copy ..\Build\Release\DynMvp.Device.pdb .\UniScanG\Build\Release\
copy ..\Build\Release\DynMvp.Vision.dll .\UniScanG\Build\Release\
copy ..\Build\Release\DynMvp.Vision.pdb .\UniScanG\Build\Release\

copy ..\Build\Release\UniEye.Base.dll .\UniScanG\Build\Release\
copy ..\Build\Release\UniEye.Base.pdb .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.Common.dll .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.Common.pdb .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.dll .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.pdb .\UniScanG\Build\Release\
REM copy ..\Build\Release\UniScanG.Vision.dll .\UniScanG\Build\Release\
REM copy ..\Build\Release\UniScanG.Vision.pdb .\UniScanG\Build\Release\
copy ..\Build\Release\WpfControlLibrary.dll .\UniScanG\Build\Release\
copy ..\Build\Release\WpfControlLibrary.pdb .\UniScanG\Build\Release\

copy ..\Build\Release\UniScanG.Module.Inspector.exe .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.Module.Inspector.pdb .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.Module.Inspector.exe.config .\UniScanG\Build\Release\

copy ..\Build\Release\UniScanG.Module.Controller.exe .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.Module.Controller.pdb .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.Module.Controller.exe.config .\UniScanG\Build\Release\

copy ..\Build\Release\UniScanG.Module.Monitor.exe .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.Module.Monitor.pdb .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.Module.Monitor.exe.config .\UniScanG\Build\Release\

copy ..\Build\Release\UniScanG.Module.Observer.exe .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.Module.Observer.pdb .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.Module.Observer.exe.config .\UniScanG\Build\Release\

copy ..\Build\Release\UnieyeLauncher.exe .\UniScanG\Build\Release\
copy ..\Build\Release\LicenseManager.exe .\UniScanG\Build\Release\

mkdir .\UniScanG\Runtime\Config
copy ..\Runtime\Config\StringTable_ko-kr.xml .\UniScanG\Runtime\Config\
copy ..\Runtime\Config\StringTable_zh-cn.xml .\UniScanG\Runtime\Config\
copy ..\Runtime\Config\LocalizeHelper_ko-kr.xml .\UniScanG\Runtime\Config\
copy ..\Runtime\Config\LocalizeHelper_zh-cn.xml .\UniScanG\Runtime\Config\
copy ..\Runtime\Config\log4net.xml .\UniScanG\Runtime\Config\

copy ..\Build\Copy_UniScanG2.bat .\UniScanG\Build\
copy ..\Build\Copy_UniScanG2_CMIM.bat .\UniScanG\Build\
copy ..\Build\Copy_UniScanG2_OM.bat .\UniScanG\Build\


if /i "%YN%"=="y" (
7z.exe a %compFile% .\UniScanG\
)

if "%YN%" == "" (
pause
)