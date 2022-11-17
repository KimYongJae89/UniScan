cls

cd /d %~dp0
mkdir Log\CM
mkdir Log\IM1
mkdir Log\IM1B
mkdir Log\IM2
mkdir Log\IM2B

xcopy "\\127.0.0.1\UniScan\Gravure_Controller\Log" "Log\CM" /e /y
xcopy "\\192.168.50.10\UniScan\Gravure_Inspector\Log" "Log\IM1" /e /y
xcopy "\\192.168.50.11\UniScan\Gravure_Inspector\Log" "Log\IM1B" /e /y
xcopy "\\192.168.50.20\UniScan\Gravure_Inspector\Log" "Log\IM2" /e /y
xcopy "\\192.168.50.21\UniScan\Gravure_Inspector\Log" "Log\IM2B" /e /y

7z.exe a Log.7z .\Log