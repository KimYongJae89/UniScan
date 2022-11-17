mkdir .\UniScanG\Build\Release
copy ..\Build\Release\DynMvp.dll .\UniScanG\Build\Release\
copy ..\Build\Release\DynMvp.Data.dll .\UniScanG\Build\Release\
copy ..\Build\Release\DynMvp.Device.dll .\UniScanG\Build\Release\
copy ..\Build\Release\DynMvp.Vision.dll .\UniScanG\Build\Release\

copy ..\Build\Release\UniEye.Base.dll .\UniScanG\Build\Release\
copy ..\Build\Release\UniScanG.dll .\UniScanG\Build\Release\
copy ..\Build\Release\UniScan.Common.dll .\UniScanG\Build\Release\

copy ..\Build\Release\UniScan.Inspector.exe .\UniScanG\Build\Release\
copy ..\Build\Release\UniScan.Watch.exe .\UniScanG\Build\Release\
copy ..\Build\Release\UniScan.Monitor.exe .\UniScanG\Build\Release\
copy ..\Build\Release\UnieyeLauncher.exe .\UniScanG\Build\Release\

mkdir .\UniScanG\Runtime\Config
copy ..\Runtime\Config\StringTable_ko-kr.xml .\UniScanG\Runtime\Config\
copy ..\Runtime\Config\StringTable_zh-cn.xml .\UniScanG\Runtime\Config\
copy ..\Runtime\Config\log4net.xml .\UniScanG\Runtime\Config\

copy ..\Build\Copy_UniScanG.bat .\UniScanG\Build\

pause