rem //////////////////////////////////////////////
set hour=%time:~0,2%
set HH1=%time:~0,1%
set HH2=%time:~1,1%
if "%HH1%" == " " set hour=0%HH2%
rem //////////////////////////////////////////////
Set MainFolder=UniScanM_%DATE:~0,4%%DATE:~5,2%%DATE:~8,2%_%hour%%time:~3,2%
SET ReleaseDirectory=.\%MainFolder%\Build\Release


mkdir %ReleaseDirectory%
copy ..\Build\Release\DynMvp.dll %ReleaseDirectory%
copy ..\Build\Release\DynMvp.Data.dll %ReleaseDirectory%
copy ..\Build\Release\DynMvp.Device.dll %ReleaseDirectory%
copy ..\Build\Release\DynMvp.Vision.dll %ReleaseDirectory%

copy ..\Build\Release\UniEye.Base.dll %ReleaseDirectory%
copy ..\Build\Release\UniScanM.dll %ReleaseDirectory%

copy ..\Build\Release\UniScanM.Pinhole.exe %ReleaseDirectory%
copy ..\Build\Release\UniScanM.StillImage.exe %ReleaseDirectory%
copy ..\Build\Release\UniScanM.ColorSens.exe %ReleaseDirectory%
copy ..\Build\Release\UniScanM.EDMS.exe %ReleaseDirectory%
copy ..\Build\Release\UniScanM.RVMS.exe %ReleaseDirectory%
copy ..\Build\Release\UniScanM.EDMSW.exe %ReleaseDirectory%

copy ..\Build\Release\UnieyeLauncher.exe %ReleaseDirectory%
copy ..\Build\Release\UserManager.exe %ReleaseDirectory%

SET ConfigDirectory=.\%MainFolder%\Runtime\Config
mkdir %ConfigDirectory%
copy ..\Runtime\Config\StringTable_ko-kr.xml %ConfigDirectory%
copy ..\Runtime\Config\StringTable_zh-cn.xml %ConfigDirectory%
copy ..\Runtime\Config\log4net.xml %ConfigDirectory%

SET ResultDirectory=.\%MainFolder%\Runtime\Result
mkdir %ResultDirectory%
copy ..\Runtime\Result\RawDataTemplate_ColorSensor.xlsx %ResultDirectory%
copy ..\Runtime\Result\RawDataTemplate_EDMS.xlsx %ResultDirectory%
copy ..\Runtime\Result\RawDataTemplate_PinHole.xlsx %ResultDirectory%
copy ..\Runtime\Result\RawDataTemplate_RVMS.xlsx %ResultDirectory%
copy ..\Runtime\Result\RawDataTemplate_StillImage.xlsx %ResultDirectory%
copy ..\Runtime\Result\RawDataTemplate_EDMS-W.xlsx %ResultDirectory%

copy ..\Build\Copy_UniScanM.bat .\%MainFolder%\Build\

7z.exe a %MainFolder%.zip .\%MainFolder%\
pause