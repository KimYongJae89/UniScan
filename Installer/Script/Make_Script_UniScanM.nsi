;NSIS Modern User Interface version 1.63
;!define RVMS_ONLY

!define PROJECT_CODE "UniScanM" ;Define your own software name here

!define COMPANY_NAME "UniEye" ;Define your own software name here
!define PRODUCT_NAME "Gravure Annexation Equipment (GAE)" ;Define your own software name here
!define SIMPLE_PRODUCT_NAME "GAE" ;Define your own software name here
!define PRODUCT_VERSION "2.6" ;Define your own software name here
!define SETUP_PATH "UniScan" ;Define your own software name here
!define BUILD_PATH "..\..\Build\Release" ;Define your own software version here
!define SHARED_PATH1 "..\..\..\Shared\ReferenceDll" ;Define your own software version her
!define SHARED_PATH2 "..\..\..\Shared\DependenctDll" ;Define your own software version her
!define RUNTIME_CONFIG_PATH "..\..\Runtime\Config" ;Define your own software version here
!define RUNTIME_RESULT_PATH "..\..\Runtime\Result" ;Define your own software version here

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
!ifdef RVMS_ONLY
	OutFile "..\Setup\Setup_${PROJECT_CODE}_V${PRODUCT_VERSION}_RVMS.exe"
!else
	OutFile "..\Setup\Setup_${PROJECT_CODE}_V${PRODUCT_VERSION}.exe"
!endif

  ;Remember the installer language
  ;!define MUI_LANGDLL_REGISTRY_ROOT "${PRODUCT_REG_ROOT_KEY}"
  ;!define MUI_LANGDLL_REGISTRY_KEY "${PRODUCT_UNINST_REG_KEY}"
  ;!define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"
  ;!define MUI_LANGDLL_REGISTRY_VALUENAME "NSIS:Language"
  
  ;!define MUI_SKIN "Windows XP"
  ;!define MUI_SKIN "Orange"
  ;!define MUI_DISABLEBG
  ;!define MUI_BGGRADIENT false
;--------------------------------
;Modern UI Configuration
	!insertmacro MUI_PAGE_WELCOME
	!insertmacro MUI_PAGE_COMPONENTS
	!insertmacro MUI_PAGE_DIRECTORY
	!insertmacro MUI_PAGE_INSTFILES
	;!insertmacro MUI_PAGE_FINISH
	
	!define MUI_ABORTWARNING
	!define MUI_UNCONFIRMPAGE
	
;!define MUI_FINISHPAGE_SHOWREADME_FUNCTION finishpageaction
;--------------------------------
;Languages
	!define MUI_LANGDLL_ALWAYSSHOW
	!define MUI_LANGDLL_ALLLANGUAGES
	!insertmacro MUI_LANGUAGE "English"
	!insertmacro MUI_LANGUAGE "Korean"	
	!insertmacro MUI_LANGUAGE "SimpChinese"

;--------------------------------
;Language Strings

	;SelectedLan
	LangString selLanguage ${LANG_ENGLISH} "English[en-us]"
	LangString selLanguage ${LANG_KOREAN} "Korean[ko-kr]"
	LangString selLanguage ${LANG_SimpChinese} "Chinese(Simplified)[zh-cn]"
	
	;RVMS Sensor com port message
	LangString RVMSSensorComMessage ${LANG_ENGLISH} "Connect the RVMS sensor to the COM3 port"
	LangString RVMSSensorComMessage ${LANG_KOREAN} "RVMS 센서를 COM3 포트에 연결해 주세요"
	LangString RVMSSensorComMessage ${LANG_SimpChinese} "? RVMS ?感器?接到 COM3 端口"
	
	;Description
	LangString DESC_UNIEYES ${LANG_ENGLISH} "Install ${PRODUCT_NAME}"
	LangString DESC_UNIEYES ${LANG_KOREAN} "${PRODUCT_NAME} 를 설치합니다."
	LangString DESC_UNIEYES ${LANG_SimpChinese} "${PRODUCT_NAME} 를 설치합니다."
	

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
;	SetOutPath "$INSTDIR\$DEVICETYPE\Config"
	File "..\Setup\Setup\Common\7z.exe"
	File "..\Setup\Setup\Common\7z.dll"
	nsExec::exec '7z.exe e *.7z -aoa'
	Delete "7z.exe"
	Delete "7z.dll"
FunctionEnd

Function WriteSelectedLanguage	
	FileOpen $9 InstallLanguage.txt w ;Opens a Empty File and fills it
	FileWrite $9 $(selLanguage)
	FileClose $9 ;Closes the filled file
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
FunctionEnd

Function SetNtpClient
	; NTP 클라이언트 설정
	WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "DelayedAutostart" 0
	WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "Start" 2
	
	; NTP 클라이언트 서비스 시작
;	ExecWait "$EXEDIR\Setup\w32Time\w32Time_Client.bat 192.168.1.100"
	SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		File "..\Setup\Setup\w32Time\w32Time_Client.bat"
		ExecWait "w32Time_Client.bat 192.168.1.100"
FunctionEnd

Function SetupCommon
	; 기존 파일 백업
	RMDir $INSTDIR\$DEVICETYPE\Bin_backup
	CreateDirectory $INSTDIR\$DEVICETYPE\Bin_backup
		CopyFiles "$INSTDIR\$DEVICETYPE\UnieyeLauncher.exe" "$INSTDIR\$DEVICETYPE\Bin_backup\UnieyeLauncher.exe"
		CopyFiles "$INSTDIR\$DEVICETYPE\Bin\DynMvp.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\DynMvp.dll"
		CopyFiles "$INSTDIR\$DEVICETYPE\Bin\DynMvp.Device.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\DynMvp.Device.dll"
		CopyFiles "$INSTDIR\$DEVICETYPE\Bin\DynMvp.Data.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\DynMvp.Data.dll"
		CopyFiles "$INSTDIR\$DEVICETYPE\Bin\DynMvp.Vision.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\DynMvp.Vision.dll"
		CopyFiles "$INSTDIR\$DEVICETYPE\Bin\UniEye.Base.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\UniEye.Base.dll"
		CopyFiles "$INSTDIR\$DEVICETYPE\Bin\UniScanM.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\UniScanM.dll"
		CopyFiles "$INSTDIR\$DEVICETYPE\Bin\$BINFILE" "$INSTDIR\$DEVICETYPE\Bin_backup\$BINFILE"
	
	; SharedDLL 복사
	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
		File "${SHARED_PATH1}\*.dll"
		File "${SHARED_PATH2}\*.dll"

	; UniScanM 프레임워크 복사
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
		File "${BUILD_PATH}\UniScanM.dll"
		File "${BUILD_PATH}\UniScanM.pdb"
		File "${BUILD_PATH}\UserManager.exe"
		File "${BUILD_PATH}\LicenseManager.exe"		
		
	; 공통 Config 복사
	SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		File "${RUNTIME_CONFIG_PATH}\log4net.xml"
		File "${RUNTIME_CONFIG_PATH}\StringTable_ko-kr.xml"
		File "${RUNTIME_CONFIG_PATH}\StringTable_zh-cn.xml"
		File "${RUNTIME_CONFIG_PATH}\Unieye.png"
		
	; 바로가기
	CreateDirectory "$DESKTOP\UniScaM"
	CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}
	
	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
	CreateShortCut "$DESKTOP\UniScaM\$LNKFILE" "$INSTDIR\$DEVICETYPE\Bin\$BINFILE"
	CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\$LNKFILE" "$INSTDIR\$DEVICETYPE\Bin\$BINFILE"	
	
	SetOutPath "$INSTDIR\$DEVICETYPE"
	CreateShortCut "$DESKTOP\UniScaM\UnieyeLauncher.lnk" "$INSTDIR\$DEVICETYPE\UnieyeLauncher.exe"	
	CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\UnieyeLauncher.lnk" "$INSTDIR\$DEVICETYPE\UnieyeLauncher.exe"
	
	CreateShortCut “$APPDATA\Microsoft\Windows\Start Menu\Programs\Startup\UnieyeLauncher.lnk” "$INSTDIR\$DEVICETYPE\UnieyeLauncher.exe"
	
	; 공유폴더 설정
	ExecWait '"cmd.exe" /C net share UniScan=$INSTDIR /grant:Everyone,full'
	ExecWait '"cmd.exe" /C ICACLS $INSTDIR /grant Everyone:F /t'

	; administrator 계정 활성화
	ExecWait '"cmd.exe" /C net user administrator /active:yes'
	
	; 콘솔 로그온 시 로컬 계정에서 빈 암호 사용 제한 '아니오'
	WriteRegDWORD "HKLM" "SYSTEM\CurrentControlSet\Control\Lsa" "limitblankpassworduse" 0	
	
	; 전원옵션 '빠른시작켜기' 해제
	WriteRegDWORD "HKLM" "SYSTEM\CurrentControlSet\Control\Session Manager\Power" "HiberbootEnabled" 0	
	
FunctionEnd

SectionGroup "RVMS" SG_RVMS
	Section /o "RVMS" SecRVMS
		StrCpy $DEVICETYPE "RVMS"
		StrCpy $BINFILE "UniScanM.RVMS.exe"
		StrCpy $LNKFILE "RVMS.lnk"
		Call SetupCommon
		
		SetOutPath "$INSTDIR\RVMS\Bin"
			File "${BUILD_PATH}\UniScanM.RVMS.exe"
			File "${BUILD_PATH}\UniScanM.RVMS.pdb"
			File "${BUILD_PATH}\UniScanM.RVMS.exe.config" 
		
		SetOutPath "$INSTDIR\RVMS\Result"
			File "${RUNTIME_RESULT_PATH}\RawDataTemplate_RVMS.xlsx"
		
		Call SetNtpServer
		
		MessageBox MB_OK $(RVMSSensorComMessage)
	SectionEnd	
	Section /o "Dependenct" SecRVMSDep
		Call InstSentinalHASP
	SectionEnd	
	Section /o "Configuration" SecRVMSConf
		SetOutPath "$INSTDIR\RVMS\Config"
			File "${RUNTIME_CONFIG_PATH}\Specific\UniScanM.RVMS.Config.7z"
			Call UnzipDeviceConfigFile
			Call WriteSelectedLanguage
	SectionEnd
SectionGroupEnd

SectionGroup "Pinhole"  SG_PINHOLE
	Section /o "Pinhole" SecPinhole
		StrCpy $DEVICETYPE "Pinhole"
		StrCpy $BINFILE "UniScanM.Pinhole.exe"
		StrCpy $LNKFILE "Pinhole.lnk"	
		Call SetupCommon
		
		SetOutPath "$INSTDIR\Pinhole\Bin"
			File "${BUILD_PATH}\UniScanM.Pinhole.exe"
			File "${BUILD_PATH}\UniScanM.Pinhole.pdb"
			File "${BUILD_PATH}\UniScanM.Pinhole.exe.config" 	
		
		SetOutPath "$INSTDIR\Pinhole\Result"
			File "${RUNTIME_RESULT_PATH}\RawDataTemplate_PinHole.xlsx"
		
		Call SetNtpClient
	SectionEnd
	Section /o "Dependenct" SecPinholeDep
		Call SetNtpClient
		Call InstPylon	
		Call InstDigitalPro
		Call InstVCRedist
		Call InstMIL
	SectionEnd	
	Section /o "Configuration" SecPinholeConf
		SetOutPath "$INSTDIR\Pinhole\Config"
			File "${RUNTIME_CONFIG_PATH}\Specific\UniScanM.Pinhole.Config.7z"
			Call UnzipDeviceConfigFile	
			Call WriteSelectedLanguage
	SectionEnd
SectionGroupEnd

SectionGroup "ColorSens"  SG_COLORSENS
	Section /o "ColorSens" SecColor
		StrCpy $DEVICETYPE "ColorSensor"
		StrCpy $BINFILE "UniScanM.ColorSens.exe"
		StrCpy $LNKFILE "ColorSensor.lnk"	
		Call SetupCommon
		
		SetOutPath "$INSTDIR\ColorSensor\Bin"
			File "${BUILD_PATH}\UniScanM.ColorSens.exe"
			File "${BUILD_PATH}\UniScanM.ColorSens.pdb"
			File "${BUILD_PATH}\UniScanM.ColorSens.exe.config" 
	
		SetOutPath "$INSTDIR\ColorSensor\Result"
			File "${RUNTIME_RESULT_PATH}\RawDataTemplate_ColorSensor.xlsx"
		
		Call SetNtpClient
	SectionEnd	
	Section /o "Dependenct" SecColorDep		
		Call InstPylon	
	SectionEnd	
	Section /o "Configuration" SecColorConf
		SetOutPath "$INSTDIR\ColorSensor\Config"
			File "${RUNTIME_CONFIG_PATH}\Specific\UniScanM.ColorSens.Config.7z"
			Call UnzipDeviceConfigFile
			Call WriteSelectedLanguage
	SectionEnd
SectionGroupEnd

SectionGroup "EDMS" SG_EDMS
	Section /o "EDMS" SecEDMS
		StrCpy $DEVICETYPE "EDMS"
		StrCpy $BINFILE "UniScanM.EDMS.exe"
		StrCpy $LNKFILE "EDMS.lnk"
		Call SetupCommon	
		
		SetOutPath "$INSTDIR\EDMS\Bin"
			File "${BUILD_PATH}\UniScanM.EDMS.exe"
			File "${BUILD_PATH}\UniScanM.EDMS.pdb"
			File "${BUILD_PATH}\UniScanM.EDMS.exe.config" 

		SetOutPath "$INSTDIR\EDMS\Result"
			File "${RUNTIME_RESULT_PATH}\RawDataTemplate_EDMS.xlsx"
		
		Call SetNtpClient
	SectionEnd	
	Section /o "Dependenct" SecEDMSDep
		Call InstCoaxLink
	SectionEnd	
	Section /o "Configuration" SecEDMSConf
		SetOutPath "$INSTDIR\EDMS\Config"
			File "${RUNTIME_CONFIG_PATH}\Specific\UniScanM.EDMS.Config.7z"
			Call UnzipDeviceConfigFile
			Call WriteSelectedLanguage
	SectionEnd
SectionGroupEnd

SectionGroup "StopImage" SG_STOPIMAGE
	Section /o "StopImage" SecSI
		StrCpy $DEVICETYPE "StillImage"
		StrCpy $BINFILE "UniScanM.StillImage.exe"
		StrCpy $LNKFILE "StillImage.lnk"
		Call SetupCommon
		
		SetOutPath "$INSTDIR\StillImage\Bin"
			File "${BUILD_PATH}\UniScanM.StillImage.exe"
			File "${BUILD_PATH}\UniScanM.StillImage.pdb"
			File "${BUILD_PATH}\UniScanM.StillImage.exe.config" 

		SetOutPath "$INSTDIR\StillImage\Result"
			File "${RUNTIME_RESULT_PATH}\RawDataTemplate_StillImage.xlsx"		
		
		Call SetNtpClient
	SectionEnd
	Section /o "Dependenct" SecSIDep
		Call InstCoaxLink
		Call InstMotionComposer	
		Call InstVCRedist
		Call InstFuji
		Call InstPanasonic
	SectionEnd
	Section /o "Configuration" SecSIConf
		SetOutPath "$INSTDIR\StillImage\Config"
			File "${RUNTIME_CONFIG_PATH}\Specific\UniScanM.StillImage.Config.7z"
			Call UnzipDeviceConfigFile
			Call WriteSelectedLanguage
	SectionEnd
SectionGroupEnd

SectionGroup "Independenct" SG_INDEPUTIL
	Section /o "7zip" SecIndep7z
		SetOutPath "$INSTDIR\Utility\"
		CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}	
;		ExecWait "$EXEDIR\Setup\Common\7z1805-x64.msi /passive"
		ExecWait "$EXEDIR\Setup\Common\7z1805-x64.bat"		
	SectionEnd

	Section /o "ImageJ" SecIndepImageJ
		SetOutPath "$INSTDIR\Utility\"
		CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}		
		ExecWait "$EXEDIR\Setup\Common\ij149-jre8-64.exe -y -o$INSTDIR\Utility\"
;		CreateShortCut "$DESKTOP\ImageJ.lnk" "$INSTDIR\Utility\ij149-jre8-64\ImageJ\ImageJ.exe"
		CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\ImageJ.lnk" "$INSTDIR\Utility\ij149-jre8-64\ImageJ\ImageJ.exe"
	SectionEnd
	Section /o "VIT Light-Controller S/W" SecIndepVit
		SetOutPath "$INSTDIR\Utility\"
		CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}		
		CopyFiles "$EXEDIR\Setup\Common\JconTester2019.exe" "$INSTDIR\Utility\JconTester2019.exe"
;		CreateShortCut "$DESKTOP\JconTester2019.lnk" "$INSTDIR\Utility\JconTester2019.exe"
		CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\LED-JCON-VIT.lnk" "$INSTDIR\Utility\LED-JCON-VIT.exe"	
	SectionEnd

	Section /o "Notepad 7.5.4" SecIndepNpp
		SetOutPath "$INSTDIR\Utility\"
		CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}		
;		ExecWait "$EXEDIR\Setup\Common\npp.7.5.4.Installer.x64.exe /S"
		ExecWait "$EXEDIR\Setup\Common\npp.7.5.4.Installer.x64.bat"	
	SectionEnd
	Section /o "RealVNC E4.6.3" SecIndepRealVnc
		SetOutPath "$INSTDIR\Utility\"
		CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}		
;		ExecWait "$EXEDIR\Setup\Common\vnc-E4_6_3-x86_x64_win32.exe /s"
		ExecWait "$EXEDIR\Setup\Common\vnc-E4_6_3-x86_x64_win32.bat"	
	SectionEnd
	Section /o "Putty" SecIndepPutty
		SetOutPath "$INSTDIR\Utility\"
		CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}		
		CopyFiles "$EXEDIR\Setup\Common\putty.exe" "$INSTDIR\Utility\putty.exe"
;		CreateShortCut "$DESKTOP\putty.lnk" "$INSTDIR\Utility\putty.exe"
		CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\putty.lnk" "$INSTDIR\Utility\putty.exe"	
	SectionEnd
SectionGroupEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_UNINST_REG_KEY}" "DisplayName" "${PRODUCT_NAME}"
  WriteRegStr ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_UNINST_REG_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_UNINST_REG_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
SectionEnd

Function InstMIL
	DetailPrint "Install MILX..."
	ExecWait "$EXEDIR\Setup\MILX_CXP\Setup.bat"
FunctionEnd

Function InstSentinalHASP
	DetailPrint "Install Sentinal HASP..."
	
;!ifdef RVMS_ONLY
	SetOutPath "$INSTDIR\$DEVICETYPE\Temp\HASP"
	File "..\Setup\Setup\HASP\HASPUserSetup.exe"
	File "..\Setup\Setup\HASP\Setup.bat"
	ExecWait "$INSTDIR\$DEVICETYPE\Temp\HASP\Setup.bat"
;!else	
;	ExecWait "$EXEDIR\Setup\HASP\Setup.bat"
;!endif
FunctionEnd

Function InstPylon
	DetailPrint "Install Pylon 5.0..."
	ExecWait "$EXEDIR\Setup\Pylon5\Basler_pylon_5.0.5.8999.exe /s"
FunctionEnd

Function InstCoaxLink
;	DetailPrint "Install CoaxLink 9..."
;	ExecWait "$EXEDIR\Setup\CoaxLink9\coaxlink-win10-9.5.2.131.exe /s"

	DetailPrint "Install CoaxLink 12..."
	ExecWait "$EXEDIR\Setup\CoaxLink12\coaxlink-win10-x86_64-12.2.1.24.exe /s"
	ExecWait "$EXEDIR\Setup\CoaxLink12\memento-win10-x86_64-12.1.1.24.exe /s"
FunctionEnd

Function InstFuji
	DetailPrint "Install Fuji A7 1.6..."
	ExecWait "$EXEDIR\Setup\Fuji_ALPHA7_V1.6\setup.exe /s /v/qn"
FunctionEnd

Function InstPanasonic
	DetailPrint "Install Panasonic MINAS A6 V6.0.5.0..."
	ExecWait "$EXEDIR\Setup\Panasonic_MINAS_A6_V6.0.5.0\setup.bat"
FunctionEnd

Function InstMotionComposer
		DetailPrint "Install MotionComposer 21.0..."
		ExecWait "$EXEDIR\Setup\MotionComposer21\Setup.bat"
FunctionEnd

Function InstDigitalPro
	DetailPrint "Install Digital Pro 21..."
	ExecWait "$EXEDIR\Setup\DigitalPro21\Setup.bat"		
FunctionEnd

Function InstVCRedist
		DetailPrint "Install VCRedist..."
		ExecWait "$EXEDIR\Setup\VCRedist\vcredist_x64 /q"
FunctionEnd

Function .onInit
	!insertmacro MUI_LANGDLL_DISPLAY
	
!ifdef RVMS_ONLY
!insertmacro UnSelectSection ${SG_PINHOLE}
SectionSetText ${SG_PINHOLE} ""
SectionSetText ${SecPinhole} ""
SectionSetText ${SecPinholeDep} ""
SectionSetText ${SecPinholeConf} ""

!insertmacro UnSelectSection ${SG_COLORSENS}
SectionSetText ${SG_COLORSENS} ""
SectionSetText ${SecColor} ""
SectionSetText ${SecColorDep} ""
SectionSetText ${SecColorConf} ""

!insertmacro UnSelectSection ${SG_EDMS}
SectionSetText ${SG_EDMS} ""
SectionSetText ${SecEDMS} ""
SectionSetText ${SecEDMSDep} ""
SectionSetText ${SecEDMSConf} ""

!insertmacro UnSelectSection ${SG_STOPIMAGE}
SectionSetText ${SG_STOPIMAGE} ""
SectionSetText ${SecSI} ""
SectionSetText ${SecSIDep} ""
SectionSetText ${SecSIConf} ""

!insertmacro UnSelectSection ${SG_INDEPUTIL}
SectionSetText ${SG_INDEPUTIL} ""
SectionSetText ${SecIndep7z} ""
SectionSetText ${SecIndepImageJ} ""
SectionSetText ${SecIndepVit} ""
SectionSetText ${SecIndepNpp} ""
SectionSetText ${SecIndepRealVnc} ""
SectionSetText ${SecIndepPutty} ""
!endif
FunctionEnd

Function .onSelChange
FunctionEnd

Function .onInstSuccess
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
  RMDir /r "$DESKTOP\UniScaM"
  RMDir /r "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}"
  RMDir /r "$INSTDIR"

  DeleteRegKey ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_UNINST_REG_KEY}"
  DeleteRegKey ${PRODUCT_REG_ROOT_KEY} "${PRODUCT_INST_REG_KEY}"
  SetAutoClose true
SectionEnd

;--------------------------------
;Descriptions

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${SecRVMS} "UniScanM.RVMS"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecRVMSDep} "Sentinal HASP"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecRVMSConf} "Default Configuration"
	
	!insertmacro MUI_DESCRIPTION_TEXT ${SecPinhole} "UniScanM.Pinhole"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecPinholeDep} "Pylon, DigitalPro, MILX"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecPinholeConf} "Default Configuration"
	
	!insertmacro MUI_DESCRIPTION_TEXT ${SecColor} "UniScanM.ColorSens"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecColorDep} "Pylon"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecColorConf} "Default Configuration"
	
	!insertmacro MUI_DESCRIPTION_TEXT ${SecEDMS} "UniScanM.EDMS"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecEDMSDep} "GeniCam"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecEDMSConf} "Default Configuration"
	
	!insertmacro MUI_DESCRIPTION_TEXT ${SecSI} "UniScanM.StillImage"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecSIDep} "GeniCam, MotionComposer, AlphaServo, Minas"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecSIConf} "Default Configuration"
!insertmacro MUI_FUNCTION_DESCRIPTION_END

