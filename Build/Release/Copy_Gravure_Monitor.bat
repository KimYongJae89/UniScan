@echo off

copy *.dll \\Gravure-Insp\Gravure_Monitor\Update
copy UniScan.Monitor.exe \\Gravure-Insp\Gravure_Monitor\Update
copy UnieyeLauncher.exe \\Gravure-Insp\Gravure_Monitor\Bin
echo Gravure-Insp Copy Done

copy *.dll \\192.168.50.2\Gravire_Monitor\Update
copy UniScan.Watch.exe \\192.168.50.2\Gravire_Monitor\Update
copy UnieyeLauncher.exe \\192.168.50.2\Gravire_Monitor\Bin
echo Gravure-Monitor Copy Done

pause