@echo off

copy *.dll \\192.168.0.100\UniScan\bin
copy UniScan.Inspector.exe \\192.168.0.110\UniScan\bin
echo 192.168.0.110 Copy Done

copy *.dll \\192.168.0.110\UniScan\bin
copy UniScan.Monitor.exe \\192.168.0.110\UniScan\bin
echo 192.168.0.110 Copy Done

copy *.dll \\192.168.0.120\UniScan\bin
copy UniScan.Inspector.exe \\192.168.0.120\UniScan\bin
echo 192.168.0.120 Copy Done

copy *.dll \\192.168.0.130\UniScan\bin
copy UniScan.Inspector.exe \\192.168.0.130\UniScan\bin
echo 192.168.0.130 Copy Done

pause