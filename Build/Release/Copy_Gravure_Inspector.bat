@echo off

copy *.dll \\192.168.50.10\Gravure_Inspector\Update
copy UniScan.Inspector.exe \\192.168.50.10\Gravure_Inspector\Update
copy UnieyeLauncher.exe \\192.168.50.10\Gravure_Inspector\Bin
echo CAM1A Copy Done

copy *.dll \\192.168.50.11\Gravure_Inspector\Update
copy UniScan.Inspector.exe \\192.168.50.11\Gravure_Inspector\Update
copy UnieyeLauncher.exe \\192.168.50.11\Gravure_Inspector\Bin
echo CAM1B Copy Done

copy *.dll \\192.168.50.20\Gravure_Inspector\Update
copy UniScan.Inspector.exe \\192.168.50.20\Gravure_Inspector\Update
copy UnieyeLauncher.exe \\192.168.50.20\Gravure_Inspector\Bin
echo CAM2A Copy Done

copy *.dll \\192.168.50.21\Gravure_Inspector\Update
copy UniScan.Inspector.exe \\192.168.50.21\Gravure_Inspector\Update
copy UnieyeLauncher.exe \\192.168.50.21\Gravure_Inspector\Bin
echo CAM2B Copy Done

pause