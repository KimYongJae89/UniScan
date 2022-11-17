cd /d %~dp0

set targetPath=\\192.168.1.100\UniScan\RVMS
set templateFile=RawDataTemplate_RVMS.xlsx
copy *.dll %targetPath%\Update
copy ..\..\Runtime\Config\StringTable_ko-kr.xml %targetPath%\Config\
copy ..\..\Runtime\Config\StringTable_zh-cn.xml %targetPath%\Config\
copy ..\..\Runtime\Result\%templateFile% %targetPath%\Result\
copy UnieyeLauncher.exe %targetPath%\bin
copy UniEyeLauncher_RVMS.xml %targetPath%\bin\UniEyeLauncher.xml
copy DataCollector.exe %targetPath%\bin
copy UserManager.exe %targetPath%\bin
copy UniScanM.RVMS.exe %targetPath%\Update
echo %targetPath% Copy Done

set targetPath=\\192.168.1.102\UniScan\Pinhole
copy *.dll %targetPath%\Update
set templateFile=RawDataTemplate_Pinhole.xlsx
copy ..\..\Runtime\Config\StringTable_ko-kr.xml %targetPath%\Config\
copy ..\..\Runtime\Config\StringTable_zh-cn.xml %targetPath%\Config\
copy ..\..\Runtime\Result\%templateFile% %targetPath%\Result\
copy UnieyeLauncher.exe %targetPath%\bin
copy UniEyeLauncher_Pinhole.xml %targetPath%\bin\UniEyeLauncher.xml
copy CameraCalibration.exe %targetPath%\bin
copy UserManager.exe %targetPath%\bin
copy UniScanM.Pinhole.exe %targetPath%\Update
echo %targetPath% Copy Done

set targetPath=\\192.168.1.103\UniScan\ColorSensor
set templateFile=RawDataTemplate_ColorSensor.xlsx
copy *.dll %targetPath%\Update
copy ..\..\Runtime\Config\StringTable_ko-kr.xml %targetPath%\Config\
copy ..\..\Runtime\Config\StringTable_zh-cn.xml %targetPath%\Config\
copy ..\..\Runtime\Result\%templateFile% %targetPath%\Result\
copy UnieyeLauncher.exe %targetPath%\bin
copy UniEyeLauncher_ColorSensor.xml %targetPath%\bin\UniEyeLauncher.xml
copy CameraCalibration.exe %targetPath%\bin
copy UserManager.exe %targetPath%\bin
copy UniScanM.ColorSens.exe %targetPath%\Update
echo %targetPath% Copy Done

set targetPath=\\192.168.1.104\UniScan\EDMS
set templateFile=RawDataTemplate_EDMS.xlsx
copy *.dll %targetPath%\Update
copy ..\..\Runtime\Config\StringTable_ko-kr.xml %targetPath%\Config\
copy ..\..\Runtime\Config\StringTable_zh-cn.xml %targetPath%\Config\
copy ..\..\Runtime\Result\%templateFile% %targetPath%\Result\
copy UnieyeLauncher.exe %targetPath%\bin
copy UniEyeLauncher_EDMS.xml %targetPath%\bin\UniEyeLauncher.xml
copy CameraCalibration.exe %targetPath%\bin
copy UserManager.exe %targetPath%\bin
copy UniScanM.EDMS.exe %targetPath%\Update
echo %targetPath% Copy Done

set targetPath=\\192.168.1.105\UniScan\StillImage
set templateFile=RawDataTemplate_StillImage.xlsx
copy *.dll %targetPath%\Update
copy ..\..\Runtime\Config\StringTable_ko-kr.xml %targetPath%\Config\
copy ..\..\Runtime\Config\StringTable_zh-cn.xml %targetPath%\Config\
copy ..\..\Runtime\Result\%templateFile% %targetPath%\Result\
copy UnieyeLauncher.exe %targetPath%\bin
copy UniEyeLauncher_StillImage.xml %targetPath%\bin\UniEyeLauncher.xml
copy CameraCalibration.exe %targetPath%\bin
copy UserManager.exe %targetPath%\bin
copy UniScanM.StillImage.exe %targetPath%\Update
echo %targetPath% Copy Done