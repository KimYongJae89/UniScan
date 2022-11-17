cd /d %~dp0

set targetPath=D:\UniScan\UniScanWPF
set templateFile=RawDataTemplate_Offline.xlsx

copy .\Release\UnieyeLauncherV2.exe %targetPath%\Update\

mkdir %targetPath%\Update\bin\
copy .\Release\*.dll %targetPath%\Update\bin\
copy .\Release\UniScanWPF.Table.exe %targetPath%\Update\bin\

mkdir %targetPath%\Update\config\
copy ..\Runtime\Config\LocalizeHelper_ko-kr.xml %targetPath%\Update\Config\

mkdir %targetPath%\Update\Result\
copy ..\Runtime\Result\%templateFile% %targetPath%\Update\Result\

echo %date%>%targetPath%\Update\PatchDate

echo %targetPath% Copy Done
pause