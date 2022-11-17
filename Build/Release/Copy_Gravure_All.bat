@echo off

set targetPath=\\Gravure-Insp\Gravure_Monitor
copy UnieyeLauncher.exe %targetPath%\bin
copy ..\..\Runtime\Config\StringTable_ko-kr.xml %targetPath%\Config\
copy *.dll  %targetPath%\Update
copy UniScan.Monitor.exe %targetPath%\Update
echo Gravure-Insp Copy Done

set targetPath=\\192.168.50.2\Gravire_Monitor
copy UnieyeLauncher.exe %targetPath%\bin
copy ..\..\Runtime\Config\StringTable_ko-kr.xml %targetPath%\Config\
copy *.dll  %targetPath%\Update
copy UniScan.Watch.exe %targetPath%\Update
echo Gravure-Monitor Copy Done

set targetPath=\\CAM1A\Gravure_Inspector
copy UnieyeLauncher.exe %targetPath%\bin
copy ..\..\Runtime\Config\StringTable_ko-kr.xml %targetPath%\Config\
copy *.dll  %targetPath%\Update
copy UniScan.Inspector.exe %targetPath%\Update
echo CAM1A Copy Done

set targetPath=\\CAM1B\Gravure_Inspector
copy UnieyeLauncher.exe %targetPath%\bin
copy ..\..\Runtime\Config\StringTable_ko-kr.xml %targetPath%\Config\
copy *.dll  %targetPath%\Update
copy UniScan.Inspector.exe %targetPath%\Update
echo CAM1B Copy Done

set targetPath=\\CAM2A\Gravure_Inspector
copy UnieyeLauncher.exe %targetPath%\bin
copy ..\..\Runtime\Config\StringTable_ko-kr.xml %targetPath%\Config\
copy *.dll  %targetPath%\Update
copy UniScan.Inspector.exe %targetPath%\Update
echo CAM2A Copy Done

set targetPath=\\CAM2B\Gravure_Inspector
copy UnieyeLauncher.exe %targetPath%\bin
copy ..\..\Runtime\Config\StringTable_ko-kr.xml %targetPath%\Config\
copy *.dll  %targetPath%\Update
copy UniScan.Inspector.exe %targetPath%\Update
echo CAM2B Copy Done

pause