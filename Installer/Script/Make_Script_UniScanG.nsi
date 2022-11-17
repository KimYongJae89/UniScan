;NSIS Modern User Interface version 1.63

!define PROJECT_CODE "UniScanG" ;Define your own software name here

!define COMPANY_NAME "UniEye" ;Define your own software name here
!define PRODUCT_NAME "MLCC Print Inspector" ;Define your own software name here
!define SIMPLE_PRODUCT_NAME "MPI" ;Define your own software name here
!define PRODUCT_VERSION "1.5" ;Define your own software name here
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
	Page custom AdditionalTasks AdditionalTasksLeave
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

Var ExtractDevConfig
Var InstDevUtil
Var InstCommonUtil

Function .onInit
	!insertmacro MUI_LANGDLL_DISPLAY
	StrCpy $1 "초기 설정 복사"
	StrCpy $2 "종속 유틸리티 설치"
	StrCpy $3 "공통 유틸리티 설치"
FunctionEnd

Function .onSelChange
FunctionEnd

Function .onInstSuccess
FunctionEnd

Function AdditionalTasks
	Var /GLOBAL MUI_PAGE_CUSTOM
	Var /GLOBAL CheckboxExtractDevConfig
	Var /GLOBAL CheckboxInstDevUtil
	Var /GLOBAL CheckboxInstCommonUtil

	nsDialogs::Create /NOUNLOAD 1018
	Pop $MUI_PAGE_CUSTOM

	${If} $MUI_PAGE_CUSTOM = error
		Abort
	${EndIf} 

	${NSD_CreateCheckBox} 5 40 100% 20 $1
	Pop $CheckboxExtractDevConfig

	${NSD_CreateCheckBox} 5 70 100% 20 $2
	Pop $CheckboxInstDevUtil

	${NSD_CreateCheckBox} 5 100 100% 20 $3
	Pop $CheckboxInstCommonUtil
	
	nsDialogs::Show
FunctionEnd

Function AdditionalTasksLeave
${NSD_GetState} $CheckboxExtractDevConfig $ExtractDevConfig
${NSD_GetState} $CheckboxInstDevUtil $InstDevUtil
${NSD_GetState} $CheckboxInstCommonUtil $InstCommonUtil

FunctionEnd

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
FunctionEnd

Function SetupCommon
	${If} $InstCommonUtil  == ${BST_CHECKED}
		Call InstCommon
	${EndIf}
	
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

Section /o "Controller" SecController
	StrCpy $DEVICETYPE "Gravure_Controller"
	StrCpy $BINFILE "UniScanG.Module.Controller.exe"
	StrCpy $LNKFILE "UniScanG.Controller.lnk"

	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
		FILE "${BUILD_PATH}\UniScanG.Module.Controller.exe"
		FILE "${BUILD_PATH}\UniScanG.Module.Controller.pdb"
		FILE "${BUILD_PATH}\UniScanG.Module.Controller.exe.config"

	Call SetupCommon
	
	${If} $ExtractDevConfig  == ${BST_CHECKED}
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
			FILE "${RUNTIME_CONFIG_PATH}\UniScanG.Module.Controller.Config.7z"
			Call UnzipDeviceConfigFile
	${EndIf}
	
	${If} $InstDevUtil  == ${BST_CHECKED}
		Call SetNtpServer
		;Call InstPci2Serial
		Call InstADLink
		Call InstMIL
	${EndIf}		
SectionEnd


Section /o "Monitor" SecMonitor
	StrCpy $DEVICETYPE "Gravure_Monitor"
	StrCpy $BINFILE "UniScanG.Module.Monitor.exe"
	StrCpy $LNKFILE "UniScanG.Monitor.lnk"

	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
		FILE "${BUILD_PATH}\UniScanG.Module.Monitor.exe"
		FILE "${BUILD_PATH}\UniScanG.Module.Monitor.pdb"

	Call SetupCommon
	
	${If} $ExtractDevConfig  == ${BST_CHECKED}
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
			FILE "${RUNTIME_CONFIG_PATH}\UniScanG.Module.Monitor.Config.7z"
			Call UnzipDeviceConfigFile
	${EndIf}
	
	${If} $InstDevUtil  == ${BST_CHECKED}	
		Call SetNtpClient
	${EndIf}		
SectionEnd

Section /o "Inspector" SecInspector
	StrCpy $DEVICETYPE "Gravure_Inspector"
	StrCpy $BINFILE "UniScanG.Module.Inspector.exe"
	StrCpy $LNKFILE "UniScanG.Inspector.lnk"
		
	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
		FILE "${BUILD_PATH}\UniScanG.Module.Inspector.exe"
		FILE "${BUILD_PATH}\UniScanG.Module.Inspector.pdb"
		FILE "${BUILD_PATH}\UniScanG.Module.Inspector.exe.config"

	Call SetupCommon
	
	${If} $ExtractDevConfig  == ${BST_CHECKED}
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
			FILE "${RUNTIME_CONFIG_PATH}\UniScanG.Module.Inspector.Config.7z"
			Call UnzipDeviceConfigFile
	${EndIf}
	
	${If} $InstDevUtil  == ${BST_CHECKED}
		Call SetNtpClient
		Call InstCoaxLink
		Call InstMIL
	${EndIf}
SectionEnd

Section /o "Observer" SecObserver
	StrCpy $DEVICETYPE "Gravure_Observer"
	StrCpy $BINFILE "UniScanG.Module.Observer.exe"
	StrCpy $LNKFILE "UniScanG.Observer.lnk"

	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
		FILE "${BUILD_PATH}\UniScanG.Module.Observer.exe"
		FILE "${BUILD_PATH}\UniScanG.Module.Observer.pdb"
		FILE "${BUILD_PATH}\UniScanG.Module.Observer.exe.config"

	Call SetupCommon
	
	${If} $ExtractDevConfig  == ${BST_CHECKED}
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
			FILE "${RUNTIME_CONFIG_PATH}\UniScanG.Module.Observer.Config.7z"
			Call UnzipDeviceConfigFile
	${EndIf}
		
	${If} $InstDevUtil  == ${BST_CHECKED}
		Call SetNtpClient
	${EndIf}		
SectionEnd

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

Function InstCommon
	SetOutPath "$INSTDIR\Utility\"
	CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}
	
	DetailPrint "Install Etc. Utility"
	CreateDirectory "$INSTDIR\Utility"
		CopyFiles "$EXEDIR\Setup\Common\JconTester2019.exe" "$INSTDIR\Utility\JconTester2019.exe"
		CreateShortCut "$DESKTOP\JconTester2019.lnk" "$INSTDIR\Utility\JconTester2019.exe"
		CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\LED-JCON-VIT.lnk" "$INSTDIR\Utility\LED-JCON-VIT.exe"
		CopyFiles "$EXEDIR\Setup\Common\putty.exe" "$INSTDIR\Utility\putty.exe"
		CreateShortCut "$DESKTOP\putty.lnk" "$INSTDIR\Utility\putty.exe"
		CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\putty.lnk" "$INSTDIR\Utility\putty.exe"
		ExecWait "$EXEDIR\Setup\Common\ij149-jre8-64.exe -y -o$INSTDIR\Utility\"
		CreateShortCut "$DESKTOP\ImageJ.lnk" "$INSTDIR\Utility\ij149-jre8-64\ImageJ\ImageJ.exe"
		CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\ImageJ.lnk" "$INSTDIR\Utility\ij149-jre8-64\ImageJ\ImageJ.exe"
		
	DetailPrint "Install 7z..."	
;		ExecWait "$EXEDIR\Setup\Common\7z1805-x64.msi /passive"
		ExecWait "$EXEDIR\Setup\Common\7z1805-x64.bat"
	
	DetailPrint "Install npp 7.5..."
;		ExecWait "$EXEDIR\Setup\Common\npp.7.5.4.Installer.x64.exe /S"
		ExecWait "$EXEDIR\Setup\Common\npp.7.5.4.Installer.x64.bat"
	
	DetailPrint "Install RealVNC 4.6..."
;		ExecWait "$EXEDIR\Setup\Common\vnc-E4_6_3-x86_x64_win32.exe /s"
		ExecWait "$EXEDIR\Setup\Common\vnc-E4_6_3-x86_x64_win32.bat"
FunctionEnd

Function InstADLink
	DetailPrint "Install ADLink DIO Driver..."
	ExecWait "$EXEDIR\Setup\ADLink\PCIS-DASK\PCIS-DASK.exe"
FunctionEnd

Function InstPci2Serial
	DetailPrint "Install PCIe2Serial..."
	ExecWait "$EXEDIR\Setup\PciE2Serial_I-360\Setup.exe"
FunctionEnd

Function InstMIL
	DetailPrint "Install MIL10..."
	;ExecWait "$EXEDIR\Setup\MIL10\MIL64Setup.exe"	
	ExecWait "$EXEDIR\Setup\MIL10\MIL64Setup.bat"	
FunctionEnd

Function InstCoaxLink
;	DetailPrint "Install CoaxLink 9..."
;	ExecWait "$EXEDIR\Setup\CoaxLink9\coaxlink-win10-9.5.2.131.exe /s"

	DetailPrint "Install CoaxLink 12..."
	ExecWait "$EXEDIR\Setup\CoaxLink12\coaxlink-win10-x86_64-12.2.1.24.exe /s"
	ExecWait "$EXEDIR\Setup\CoaxLink12\memento-win10-x86_64-12.1.1.24.exe /s"
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
	!insertmacro MUI_DESCRIPTION_TEXT ${SecController} "UniScanG.Module.Controller"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecInspector} "UniScanG.Module.Inspector"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecMonitor} "UniScanG.Module.Monitor"	
	!insertmacro MUI_DESCRIPTION_TEXT ${SecObserver} "UniScanG.Module.Observer"	
!insertmacro MUI_FUNCTION_DESCRIPTION_END

