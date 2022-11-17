;NSIS Modern User Interface version 1.63

!define PROJECT_CODE "UniScanG" ;Define your own software name here

!define COMPANY_NAME "UniEye" ;Define your own software name here
!define PRODUCT_NAME "MLCC Print Inspector" ;Define your own software name here
!define SIMPLE_PRODUCT_NAME "MPI" ;Define your own software name here
!define PRODUCT_VERSION "2.5" ;Define your own software name here
!define SETUP_PATH "UniScan" ;Define your own software name here
!define BUILD_PATH "..\..\Build\Release" ;Define your own software version here
!define SHARED_PATH1 "..\..\..\Shared\ReferenceDll" ;Define your own software version her
!define SHARED_PATH2 "..\..\..\Shared\DependenctDll" ;Define your own software version her
!define RUNTIME_CONFIG_PATH "..\..\Runtime\Config" ;Define your own software version here

!define PRODUCT_REG_ROOT_KEY "HKLM"
!define PRODUCT_INST_REG_KEY "SOFTWARE\${COMPANY_NAME}\${SIMPLE_PRODUCT_NAME}" ; 인스톨 정보가 저장되는 레지스트리 위치
!define PRODUCT_UNINST_REG_KEY "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${SIMPLE_PRODUCT_NAME}"

!include "MUI.nsh"
!include "Sections.nsh"
!include "nsDialogs.nsh"

;--------------------------------
;Configuration

  Name "${PRODUCT_NAME}"

  InstallDirRegKey ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_INST_REG_KEY}" ""
  InstallDir "D:\${SETUP_PATH}"
  
  ;General
  OutFile "..\Setup\Setup_${PROJECT_CODE}_V${PRODUCT_VERSION}.exe"

  ;Remember the installer language
  !define MUI_LANGDLL_REGISTRY_ROOT "${PRODUCT_REG_ROOT_KEY}"
  !define MUI_LANGDLL_REGISTRY_KEY "${PRODUCT_UNINST_REG_KEY}"
  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"
  ;!define MUI_LANGDLL_REGISTRY_VALUENAME "NSIS:Language"
  
  ;!define MUI_SKIN "Windows XP"
  ;!define MUI_SKIN "Orange"
  ;!define MUI_DISABLEBG
  ;!define MUI_BGGRADIENT false
  
  !define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\box-install.ico"
  !define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\box-uninstall.ico"
  
  !define MUI_HEADERIMAGE
  !define MUI_HEADERIMAGE_BITMAP ".\Header.bmp"
  !define MUI_HEADERIMAGE_UNBITMAP ".\Header.bmp"
  
  !define MUI_WELCOMEFINISHPAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Wizard\nsis3-metro.bmp"
  !define MUI_WELCOMEFINISHPAGE_UNBITMAP "${NSISDIR}\Contrib\Graphics\Wizard\nsis3-metro.bmp"
  
;--------------------------------
;Modern UI Configuration
	!insertmacro MUI_PAGE_WELCOME
	!insertmacro MUI_PAGE_LICENSE "Licence.txt"
	!insertmacro MUI_PAGE_DIRECTORY
	!insertmacro MUI_PAGE_COMPONENTS
	!insertmacro MUI_PAGE_INSTFILES
	;!insertmacro MUI_PAGE_FINISH
	
	!define MUI_ABORTWARNING
	!define MUI_UNCONFIRMPAGE
	
;!define MUI_FINISHPAGE_SHOWREADME_FUNCTION finishpageaction
;--------------------------------
;Languages
	!define MUI_LANGDLL_ALLLANGUAGES
	!insertmacro MUI_LANGUAGE "Korean"
	!insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Language Strings

  ;Description
  LangString DESC_UNIEYES ${LANG_KOREAN} "${PRODUCT_NAME} 를 설치합니다."
  LangString DESC_UNIEYES ${LANG_ENGLISH} "Install ${PRODUCT_NAME}"

;--------------------------------
;Data

  BrandingText /TRIMRIGHT "${COMPANY_NAME} ${PRODUCT_NAME} ${PRODUCT_VERSION}"

;--------------------------------
;Reserve Files

  ;Things that need to be extracted on first (keep these lines before any File command!)
  ;Only useful for BZIP2 compression
  !insertmacro MUI_RESERVEFILE_LANGDLL

;--------------------------------
;Installer Sections
Var DEVICETYPE
Var BINFILE
Var LNKFILE

Function UnzipDeviceConfigFile
		DetailPrint "Extract Device Config File(s)..."
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		File "..\Setup\Setup\Common\7z.exe"
		File "..\Setup\Setup\Common\7z.dll"
		nsExec::exec '7z.exe e *.7z -aoa'
		Delete "7z.exe"
		Delete "7z.dll"
FunctionEnd

Function SetNtpServer
	; NTP 서버 설정
	WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "DelayedAutostart" 0
	WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "Start" 2
	WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time\Config" "AnnounceFlags" 5
	WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\NtpServer" "Enabled" 1
	
	; NTP 서버 서비스 시작
	SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		File "..\Setup\Setup\w32Time\w32Time_Server.bat"
		ExecWait "w32Time_Server.bat"
		Delete "w32Time_Server.bat"
	
	; NTP 방화벽 예외 등록
	ExecWait '"cmd.exe" /C netsh advfirewall firewall add rule name="NTP" dir=in action=allow protocol=UDP localPort=123'
	
	; 인터넷 연결 공유(ICS) IP대역 설정
	WriteRegStr  HKLM "SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters" "ScopeAddress" "192.168.50.1"
	
	; 재부팅시 ICS 서비스 자동시작
	WriteRegDWORD  HKLM "Software\Microsoft\Windows\CurrentVersion\SharedAccess" "EnableRebootPersistConnection" 1
	WriteRegDWORD  HKLM "SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters" "Start" 2
	
FunctionEnd

Function SetNtpClient
	; NTP 클라이언트 설정
	WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "DelayedAutostart" 0
	WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "Start" 2
	
	; NTP 클라이언트 서비스 시작
	;ExecWait "$EXEDIR\Setup\w32Time\w32Time_Client.bat 192.168.50.1"
	SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		File "..\Setup\Setup\w32Time\w32Time_Client.bat"
		ExecWait "w32Time_Client.bat 192.168.50.1"
		Delete "w32Time_Client.bat"
FunctionEnd

Function SetupCommon
	; 기존 파일 백업
	;RMDir $INSTDIR\$DEVICETYPE\Bin_backup
	;CreateDirectory $INSTDIR\$DEVICETYPE\Bin_backup
	;CopyFiles "$INSTDIR\$DEVICETYPE\Bin\UnieyeLauncher.exe" "$INSTDIR\$DEVICETYPE\Bin_backup\UnieyeLauncher.exe"
	;CopyFiles "$INSTDIR\$DEVICETYPE\Bin\DynMvp.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\DynMvp.dll"
	;CopyFiles "$INSTDIR\$DEVICETYPE\Bin\DynMvp.Device.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\DynMvp.Device.dll"
	;CopyFiles "$INSTDIR\$DEVICETYPE\Bin\DynMvp.Data.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\DynMvp.Data.dll"
	;CopyFiles "$INSTDIR\$DEVICETYPE\Bin\DynMvp.Vision.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\DynMvp.Vision.dll"
	;CopyFiles "$INSTDIR\$DEVICETYPE\Bin\UniEye.Base.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\UniEye.Base.dll"
	;CopyFiles "$INSTDIR\$DEVICETYPE\Bin\UniScanM.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\UniScanM.dll"
	;CopyFiles "$INSTDIR\$DEVICETYPE\Bin\$BINFILE" "$INSTDIR\$DEVICETYPE\Bin_backup\$BINFILE"
	
	; SharedDLL 복사
	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
		File "${SHARED_PATH1}\*.dll"
		File "${SHARED_PATH2}\*.dll"

	; UniScanG 프레임워크 복사
	SetOutPath "$INSTDIR\$DEVICETYPE"
		File /oname=UniEyeLauncher.exe "${BUILD_PATH}\UnieyeLauncherV2.exe"
		
	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
		File "${BUILD_PATH}\DynMvp.dll"
		File "${BUILD_PATH}\DynMvp.pdb"
		File "${BUILD_PATH}\DynMvp.Device.dll"
		File "${BUILD_PATH}\DynMvp.Device.pdb"
		File "${BUILD_PATH}\DynMvp.Data.dll"
		File "${BUILD_PATH}\DynMvp.Data.pdb"
		File "${BUILD_PATH}\DynMvp.Vision.dll"
		File "${BUILD_PATH}\DynMvp.Vision.pdb"
		File "${BUILD_PATH}\UniEye.Base.dll"
		File "${BUILD_PATH}\UniEye.Base.pdb"
		File "${BUILD_PATH}\UniScanG.Common.dll"
		File "${BUILD_PATH}\UniScanG.Common.pdb"
		File "${BUILD_PATH}\UniScanG.dll"
		File "${BUILD_PATH}\UniScanG.pdb"
		File "${BUILD_PATH}\WpfControlLibrary.dll"
		File "${BUILD_PATH}\WpfControlLibrary.pdb"
		
		File "${BUILD_PATH}\UserManager.exe"
		File "${BUILD_PATH}\LicenseManager.exe"
	
	; 공통 Config 복사
	SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		File "${RUNTIME_CONFIG_PATH}\log4net.xml"
		File "${RUNTIME_CONFIG_PATH}\StringTable_ko-kr.xml"
		File "${RUNTIME_CONFIG_PATH}\StringTable_zh-cn.xml"
		File "${RUNTIME_CONFIG_PATH}\Unieye.png"
		
	; 바로가기
	CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}
	
	; exe 바로가기
	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
	CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\$LNKFILE" "$INSTDIR\$DEVICETYPE\Bin\$BINFILE"
	CreateShortCut "$DESKTOP\$LNKFILE" "$INSTDIR\$DEVICETYPE\Bin\$BINFILE"	
		
	; 런처 바로가기
	SetOutPath "$INSTDIR\$DEVICETYPE"
	CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\UnieyeLauncher.lnk" "$INSTDIR\$DEVICETYPE\UnieyeLauncher.exe"	
	CreateShortCut "$DESKTOP\UnieyeLauncher.lnk" "$INSTDIR\$DEVICETYPE\UnieyeLauncher.exe"
	CreateShortCut "$SMSTARTUP\UnieyeLauncher.lnk" "$INSTDIR\$DEVICETYPE\UnieyeLauncher.exe"
	
	; 공유폴더 설정
	ExecWait '"cmd.exe" /C net share UniScan=$INSTDIR /grant:Everyone,full'
	ExecWait '"cmd.exe" /C ICACLS $INSTDIR /grant Everyone:F /t'

	; administrator 계정 활성화
	ExecWait '"cmd.exe" /C net user administrator /active:yes'
	
	; 콘솔 로그온 시 로컬 계정에서 빈 암호 사용 제한 '아니오'
	WriteRegDWORD "HKLM" "SYSTEM\CurrentControlSet\Control\Lsa" "limitblankpassworduse" 0	
	
	; 전원옵션 '빠른시작켜기' 해제
	WriteRegDWORD "HKLM" "SYSTEM\CurrentControlSet\Control\Session Manager\Power" "HiberbootEnabled" 0	
	
	; netplwiz 자동로그인 체크박스 활성화
	WriteRegDWORD "HKLM" "SOFTWARE\Microsoft\Windows NT\CurrentVersion\PasswordLess\Device" "DevicePasswordLessBuildVersion" 0	
	
	; 윈도우 자동로그인 활성화 (계정 user, 암호 admin1111)
	;System::Call 'kernel32.dll::GetComputerNameA(t.r0,*i ${NSIS_MAX_STRLEN} r1)i.r2'
	;WriteRegDWORD "HKLM" "Software\Microsoft\Windows NT\CurrentVersion\Winlogon" "AutoAdminLogon" 1
	;WriteRegDWORD "HKLM" "Software\Microsoft\Windows NT\CurrentVersion\Winlogon" "DefaultUserName" "user"
	;WriteRegDWORD "HKLM" "Software\Microsoft\Windows NT\CurrentVersion\Winlogon" "DefaultDomainName " $0
	;WriteRegDWORD "HKLM" "Software\Microsoft\Windows NT\CurrentVersion\Winlogon" "DefaultPassword" "admin1111"	
	
	; ICMP(ping) 방화벽 예외 설정
	ExecWait '"cmd.exe" /C netsh advfirewall firewall set rule name="파일 및 프린터 공유(에코 요청 - ICMPv4-In)" new enable=yes'
	
FunctionEnd

Function SetupController
	StrCpy $DEVICETYPE "Gravure_Controller"
	StrCpy $BINFILE "UniScanG.Module.Controller.exe"
	StrCpy $LNKFILE "UniScanG.Controller.lnk"

	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
		FILE "${BUILD_PATH}\UniScanG.Module.Controller.exe"
		FILE "${BUILD_PATH}\UniScanG.Module.Controller.pdb"
		FILE "${BUILD_PATH}\UniScanG.Module.Controller.exe.config"

	Call SetupCommon
FunctionEnd

Function SetupMonitor
	StrCpy $DEVICETYPE "Gravure_Monitor"
	StrCpy $BINFILE "UniScanG.Module.Monitor.exe"
	StrCpy $LNKFILE "UniScanG.Monitor.lnk"

	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
		FILE "${BUILD_PATH}\UniScanG.Module.Monitor.exe"
		FILE "${BUILD_PATH}\UniScanG.Module.Monitor.pdb"

	Call SetupCommon
FunctionEnd

Function SetupInspector
	StrCpy $DEVICETYPE "Gravure_Inspector"
	StrCpy $BINFILE "UniScanG.Module.Inspector.exe"
	StrCpy $LNKFILE "UniScanG.Inspector.lnk"
		
	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
		FILE "${BUILD_PATH}\UniScanG.Module.Inspector.exe"
		FILE "${BUILD_PATH}\UniScanG.Module.Inspector.pdb"
		FILE "${BUILD_PATH}\UniScanG.Module.Inspector.exe.config"

	Call SetupCommon
FunctionEnd

Function SetupObserver
	StrCpy $DEVICETYPE "Gravure_Observer"
	StrCpy $BINFILE "UniScanG.Module.Observer.exe"
	StrCpy $LNKFILE "UniScanG.Observer.lnk"

	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
		FILE "${BUILD_PATH}\UniScanG.Module.Observer.exe"
		FILE "${BUILD_PATH}\UniScanG.Module.Observer.pdb"
		FILE "${BUILD_PATH}\UniScanG.Module.Observer.exe.config"

	Call SetupCommon	
FunctionEnd

SectionGroup "Controller" SG_CONTROLLER
	Section /o "Controller" sController
		Call SetupController
		Call SetNtpServer
	SectionEnd
	
	SectionGroup "Dependenct"
		Section /o "ADLink" sControllerUtilAdlink
			Call InstADLink
		SectionEnd		
		Section /o "OXPCIe95x" sControllerUtilOXPCIe95x
			Call InstOXPCIe95x
		SectionEnd		
		Section /o "DigitalPro16" sControllerUtilDigitalPro16
			Call InstDigitalPro16
		SectionEnd		
		Section /o "DigitalPro21" sControllerUtilDigitalPro21
			Call InstDigitalPro21
		SectionEnd
		Section /o "MIL10" sControllerUtilMil10
			Call InstMIL10
		SectionEnd
		Section /o "MILX" sControllerUtilMilX
			Call InstMILX
		SectionEnd
	SectionGroupEnd
	
	Section /o "Settings" sControllerConfig
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
			FILE "${RUNTIME_CONFIG_PATH}\Specific\UniScanG.Module.Controller.Config.7z"
			Call UnzipDeviceConfigFile
	SectionEnd
SectionGroupEnd

SectionGroup "Monitor" SG_MONITOR
	Section /o "Monitor" sMonitor
		Call SetupMonitor
		Call SetNtpClient
	SectionEnd

	Section /o "Dependenct" sMonitorUtil
	SectionEnd
	
	Section /o "Settings" sMonitorConfig
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
			FILE "${RUNTIME_CONFIG_PATH}\Specific\UniScanG.Module.Monitor.Config.7z"
			Call UnzipDeviceConfigFile
	SectionEnd
SectionGroupEnd

SectionGroup "Inspector" SG_INSPECTOR
	Section /o "Inspector" sInspector
		Call SetupInspector
		Call SetNtpClient
	SectionEnd
	SectionGroup "Dependenct" 
		Section /o "CoaxLink12" sInspectorUtilCoaxlink12
			Call InstCoaxLink
		SectionEnd		
		Section /o "MIL10" sInspectorUtilMil10
			Call InstMIL10
		SectionEnd		
		Section /o "MILX" sInspectorUtilMilX
			Call InstMILX
		SectionEnd
	SectionGroupEnd	
	SectionGroup /e "Settings" sgInspectorConfig
		Section /o "1A Settings" SecDefaultConfigIM1A
			SetOutPath "$INSTDIR\$DEVICETYPE\Config"
			FILE "${RUNTIME_CONFIG_PATH}\Specific\UniScanG.Module.Inspector.1A.Config.7z"
			Call UnzipDeviceConfigFile
		SectionEnd		
		Section /o "1B Settings" SecDefaultConfigIM1B
			SetOutPath "$INSTDIR\$DEVICETYPE\Config"
			FILE "${RUNTIME_CONFIG_PATH}\Specific\UniScanG.Module.Inspector.1B.Config.7z"
			Call UnzipDeviceConfigFile
		SectionEnd		
		Section /o "2A Settings" SecDefaultConfigIM2A
			SetOutPath "$INSTDIR\$DEVICETYPE\Config"
			FILE "${RUNTIME_CONFIG_PATH}\Specific\UniScanG.Module.Inspector.2A.Config.7z"
			Call UnzipDeviceConfigFile
		SectionEnd		
		Section /o "2B Settings" SecDefaultConfigIM2B
			SetOutPath "$INSTDIR\$DEVICETYPE\Config"
			FILE "${RUNTIME_CONFIG_PATH}\Specific\UniScanG.Module.Inspector.2B.Config.7z"
			Call UnzipDeviceConfigFile
		SectionEnd
	SectionGroupEnd
SectionGroupEnd

SectionGroup "Observer" SG_OBSERVER
	Section /o "Observer" sObserver
		Call SetupObserver
		Call SetNtpClient
	SectionEnd
	Section /o "Dependenct" sObserverUtil
	SectionEnd
	Section /o "Settings" sObserverConfig
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
			FILE "${RUNTIME_CONFIG_PATH}\Specific\UniScanG.Module.Observer.Config.7z"
			Call UnzipDeviceConfigFile
	SectionEnd
SectionGroupEnd

SectionGroup "Independenct" sIndepUtil
	Section /o "7zip"
		SetOutPath "$INSTDIR\Utility\"
		CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}	
;		ExecWait "$EXEDIR\Setup\Common\7z1805-x64.msi /passive"
		ExecWait "$EXEDIR\Setup\Common\7z1805-x64.bat"		
	SectionEnd
	Section /o "ImageJ"
		SetOutPath "$INSTDIR\Utility\"
		CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}		
		ExecWait "$EXEDIR\Setup\Common\ij149-jre8-64.exe -y -o$INSTDIR\Utility\"
;		CreateShortCut "$DESKTOP\ImageJ.lnk" "$INSTDIR\Utility\ij149-jre8-64\ImageJ\ImageJ.exe"
		CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\ImageJ.lnk" "$INSTDIR\Utility\ij149-jre8-64\ImageJ\ImageJ.exe"
	SectionEnd
	Section /o "VIT Light-Controller S/W"
		SetOutPath "$INSTDIR\Utility\"
		CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}		
		CopyFiles "$EXEDIR\Setup\Common\JconTester2019.exe" "$INSTDIR\Utility\JconTester2019.exe"
;		CreateShortCut "$DESKTOP\JconTester2019.lnk" "$INSTDIR\Utility\JconTester2019.exe"
		CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\LED-JCON-VIT.lnk" "$INSTDIR\Utility\LED-JCON-VIT.exe"	
	SectionEnd
	Section /o "Notepad 7.5.4"
		SetOutPath "$INSTDIR\Utility\"
		CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}		
;		ExecWait "$EXEDIR\Setup\Common\npp.7.5.4.Installer.x64.exe /S"
		ExecWait "$EXEDIR\Setup\Common\npp.7.5.4.Installer.x64.bat"	
	SectionEnd
	Section /o "RealVNC E4.6.3"
		SetOutPath "$INSTDIR\Utility\"
		CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}		
;		ExecWait "$EXEDIR\Setup\Common\vnc-E4_6_3-x86_x64_win32.exe /s"
		ExecWait "$EXEDIR\Setup\Common\vnc-E4_6_3-x86_x64_win32.bat"	
	SectionEnd
	Section /o "Putty"
		SetOutPath "$INSTDIR\Utility\"
		CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}		
		CopyFiles "$EXEDIR\Setup\Common\putty.exe" "$INSTDIR\Utility\putty.exe"
;		CreateShortCut "$DESKTOP\putty.lnk" "$INSTDIR\Utility\putty.exe"
		CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\putty.lnk" "$INSTDIR\Utility\putty.exe"	
	SectionEnd
SectionGroupEnd
	
Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
;  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\AppMainExe.exe"
  WriteRegStr ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_UNINST_REG_KEY}" "DisplayName" "${PRODUCT_NAME}"
  WriteRegStr ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_UNINST_REG_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
;  WriteRegStr ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_UNINST_REG_KEY}" "DisplayIcon" "$INSTDIR\AppMainExe.exe"
  WriteRegStr ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_UNINST_REG_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
;  WriteRegStr ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_UNINST_REG_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
;  WriteRegStr ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_UNINST_REG_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

Function InstADLink
	DetailPrint "Install ADLink DIO Driver..."
	ExecWait "$EXEDIR\Setup\ADLink\PCIS-DASK\PCIS-DASK.exe"
FunctionEnd

Function InstDigitalPro16
	DetailPrint "Install Digital Pro 16..."
	ExecWait "$EXEDIR\Setup\DigitalPro16\Setup.0.0.0002_win64.bat"
FunctionEnd

Function InstDigitalPro21
	DetailPrint "Install Digital Pro 21..."
	ExecWait "$EXEDIR\Setup\DigitalPro21\Setup.bat"
FunctionEnd

Function InstOXPCIe95x
	DetailPrint "Install OXPCIe95x..."
	ExecWait "$EXEDIR\Setup\OXPCIe95x\Setup.exe"
FunctionEnd

Function InstMIL10
	DetailPrint "Install MIL10..."
	ExecWait "$EXEDIR\Setup\MIL10\Setup.bat"	
FunctionEnd

Function InstMILX
	DetailPrint "Install MILX..."
	ExecWait "$EXEDIR\Setup\MILX_CXP\Setup.bat"	
FunctionEnd

Function InstCoaxLink
;	DetailPrint "Install CoaxLink 9..."
;	ExecWait "$EXEDIR\Setup\CoaxLink9\coaxlink-win10-9.5.2.131.exe /s"

	DetailPrint "Install CoaxLink 12..."
	ExecWait "$EXEDIR\Setup\CoaxLink12\coaxlink-win10-x86_64-12.2.1.24.exe /s"
	ExecWait "$EXEDIR\Setup\CoaxLink12\memento-win10-x86_64-12.1.1.24.exe /s"
FunctionEnd

Function .onInit
	!insertmacro MUI_LANGDLL_DISPLAY
FunctionEnd

Function .onInstSuccess
FunctionEnd

Function .onSelChange
;MessageBox MB_OK $0
SectionGetFlags $0 $R0
;MessageBox MB_OK $R0

IntOp $R0 $R0 & ${SF_SELECTED}
${If} $R0 == ${SF_SELECTED}
	${Switch} $0
		;Default value when initially selected CONTROLLER
		${case} ${SG_CONTROLLER}
			!insertmacro SelectSection ${sController}
			!insertmacro UnSelectSection ${sControllerUtilAdlink}
			!insertmacro UnSelectSection ${sControllerUtilOXPCIe95x}			
			!insertmacro UnSelectSection ${sControllerUtilDigitalPro16}
			!insertmacro SelectSection ${sControllerUtilDigitalPro21}
			!insertmacro UnSelectSection ${sControllerUtilMil10}
			!insertmacro UnSelectSection ${sControllerUtilMilX}
			!insertmacro SelectSection ${sControllerConfig}	
		${Break}
		
		;Default value when initially selected MONITOR
		${case} ${SG_MONITOR}
		${Break}

		;Default value when initially selected INSPECTOR
		${case} ${SG_INSPECTOR}
			!insertmacro SelectSection ${sInspector}
			!insertmacro SelectSection ${sInspectorUtilCoaxlink12}
			!insertmacro UnSelectSection ${sInspectorUtilMil10}
			!insertmacro SelectSection ${sInspectorUtilMilX}
			
			!insertmacro UnSelectSection ${SecDefaultConfigIM1A}
			!insertmacro UnSelectSection ${SecDefaultConfigIM1B}
			!insertmacro UnSelectSection ${SecDefaultConfigIM2A}
			!insertmacro UnSelectSection ${SecDefaultConfigIM2B}
		${Break}

		;Default value when initially selected OBSERVER
		${case} ${SG_OBSERVER}
		${Break}
		
		; Select IM Configuration
		${case} ${SecDefaultConfigIM1A}
			;MessageBox MB_OK 'SecDefaultConfigIM1A'
			!insertmacro UnSelectSection ${SecDefaultConfigIM1B}
			!insertmacro UnSelectSection ${SecDefaultConfigIM2A}
			!insertmacro UnSelectSection ${SecDefaultConfigIM2B}
			${Break}
		${case} ${SecDefaultConfigIM1B}
			!insertmacro UnSelectSection ${SecDefaultConfigIM1A}
			;MessageBox MB_OK 'SecDefaultConfigIM1B'
			!insertmacro UnSelectSection ${SecDefaultConfigIM2A}
			!insertmacro UnSelectSection ${SecDefaultConfigIM2B}
			${Break}
		${case} ${SecDefaultConfigIM2A}
			!insertmacro UnSelectSection ${SecDefaultConfigIM1A}
			!insertmacro UnSelectSection ${SecDefaultConfigIM1B}
			;MessageBox MB_OK 'SecDefaultConfigIM2A'
			!insertmacro UnSelectSection ${SecDefaultConfigIM2B}
			${Break}
		${case} ${SecDefaultConfigIM2B}
			!insertmacro UnSelectSection ${SecDefaultConfigIM1A}
			!insertmacro UnSelectSection ${SecDefaultConfigIM1B}
			!insertmacro UnSelectSection ${SecDefaultConfigIM2A}
			;MessageBox MB_OK 'SecDefaultConfigIM2B'			
			${Break}
		${Default}
			!insertmacro UnSelectSection ${SecDefaultConfigIM1A}
			!insertmacro UnSelectSection ${SecDefaultConfigIM1B}
			!insertmacro UnSelectSection ${SecDefaultConfigIM2A}
			!insertmacro UnSelectSection ${SecDefaultConfigIM2B}
			${Break}
	${EndSwitch}
${EndIf}
;
;SectionGetFlags ${SecDefaultConfigIM1B} $R0
;IntOp $R0 $R0 & ${SF_SELECTED}
;${If} $R0 == ${SF_SELECTED}
;    !insertmacro UnSelectSection ${SecDefaultConfigIM1A}
;    !insertmacro UnSelectSection ${SecDefaultConfigIM2A}
;  	!insertmacro UnSelectSection ${SecDefaultConfigIM2B}
;${EndIf}
;
;SectionGetFlags ${SecDefaultConfigIM2A} $R0
;IntOp $R0 $R0 & ${SF_SELECTED}
;${If} $R0 == ${SF_SELECTED}
;    !insertmacro UnSelectSection ${SecDefaultConfigIM1A}
;    !insertmacro UnSelectSection ${SecDefaultConfigIM1B}
;	!insertmacro UnSelectSection ${SecDefaultConfigIM2B}
;${EndIf}
;
;SectionGetFlags ${SecDefaultConfigIM2B} $R0
;IntOp $R0 $R0 & ${SF_SELECTED}
;${If} $R0 == ${SF_SELECTED}
;    !insertmacro UnSelectSection ${SecDefaultConfigIM1A}
;    !insertmacro UnSelectSection ${SecDefaultConfigIM2A}
;	!insertmacro UnSelectSection ${SecDefaultConfigIM1B}
;${EndIf}

;  !insertmacro StartRadioButtons $1
;    !insertmacro RadioButton ${SecDefaultConfigIM1A}
;    !insertmacro RadioButton ${SecDefaultConfigIM1B}
;    !insertmacro RadioButton ${SecDefaultConfigIM2A}
;	!insertmacro RadioButton ${SecDefaultConfigIM2B}
;  !insertmacro EndRadioButtons
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name)는(은) 완전히 제거되었습니다."
FunctionEnd

Function un.onInit
	!insertmacro MUI_UNGETLANGUAGE
	MessageBox MB_ICONINFORMATION|MB_OK "$INSTDIR 폴더가 제거됩니다."
	
	MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "$(^Name)을(를) 제거하시겠습니까?" IDYES +2
		Abort
FunctionEnd

Section Uninstall
  RMDir /r "$DESKTOP\UniScaG"
  RMDir /r "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}"
  RMDir /r "$INSTDIR"

  DeleteRegKey ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_UNINST_REG_KEY}"
  DeleteRegKey ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_INST_REG_KEY}"
  SetAutoClose true
SectionEnd

;--------------------------------
;Descriptions

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${SG_CONTROLLER} "UniScanG.Module.Controller"
	!insertmacro MUI_DESCRIPTION_TEXT ${SG_INSPECTOR} "UniScanG.Module.Inspector"
	!insertmacro MUI_DESCRIPTION_TEXT ${SG_MONITOR} "UniScanG.Module.Monitor"	
	!insertmacro MUI_DESCRIPTION_TEXT ${SG_OBSERVER} "UniScanG.Module.Observer"	
!insertmacro MUI_FUNCTION_DESCRIPTION_END
