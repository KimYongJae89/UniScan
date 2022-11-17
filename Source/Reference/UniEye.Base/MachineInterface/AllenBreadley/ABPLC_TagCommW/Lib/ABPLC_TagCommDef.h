#pragma once
//#include <atlstr.h>
//#include "stdafx.h"

//===========================================================================================
//                          PLC Tag 정보 Structure
//===========================================================================================

public struct PLCTag
{
	int		m_iTagIndex;					// Tag Index
	CString m_csTagName;					// TAG 이름
	int		m_iDataType;					// Tag DataType
	BOOL	m_bArray = FALSE;	// Array Data를 받아올것인가?
	int		m_iArrayCount = 1;		// Arrary 개수
	int		m_iArrayStartAddr = 0;		// Arrary 시작 위치
	int		m_iTimeOut = 1000;		// Timeout
};


//===========================================================================================
//                          AB PLC Data Type
//     (16bit INT/UNIT 형은 VC 변수형과 중복되어 INT16, UINT16을 사용)
//===========================================================================================

typedef signed char			SINT;			// 08bit
typedef int					DINT;			// 32bit
typedef long long			LINT;			// 64bit
typedef unsigned char		USINT;			// 08bit
typedef unsigned int		UDINT;			// 32bit
typedef unsigned long long	ULINT;			// 64bit
typedef double				LREAL;			// 64bit
typedef float				REAL;			// 32bit


//===========================================================================================
//                         CPU_Type
//===========================================================================================

#define CPU_LGX			_T("LGX")			// Logix PLC
#define CPU_SLC			_T("SLC")			// 
#define CPU_PLC5		_T("PLC5")			// PLC5


//===========================================================================================
//                         DataType
//===========================================================================================

/* 1-byte / 8-bit types */
#define DT_Int8			1
#define DT_SINT			1
#define DT_USINT		1
/* 2-byte / 16-bit types */
#define DT_Int16		2
#define DT_UINT			2
#define DT_INT			2
/* 4-byte / 32-bit types */
#define DT_Int32		4
#define DT_UDINT		4
#define DT_DINT			4
#define DT_Float32		4
#define DT_REAL			4
/* 8-byte / 64-bit types */
#define DT_Int64		8
#define DT_ULINT		8
#define DT_LINT			8
#define DT_Float64		8
#define DT_LREAL		8
/* String */
#define DT_STRING		88
#define STRING_DATA_LEN	82
/* String Return Value */
#define INIT_OK			_T("INIT_SUCCESS")


//===========================================================================================
//                        library internal status
//===========================================================================================

#define PLCTAG_STATUS_PENDING			(1)
#define PLCTAG_STATUS_OK				(0)
#define PLCTAG_ERR_ABORT				(-1)
#define PLCTAG_ERR_BAD_CONFIG			(-2)
#define PLCTAG_ERR_BAD_CONNECTION		(-3)
#define PLCTAG_ERR_BAD_DATA				(-4)
#define PLCTAG_ERR_BAD_DEVICE			(-5)
#define PLCTAG_ERR_BAD_GATEWAY			(-6)
#define PLCTAG_ERR_BAD_PARAM			(-7)
#define PLCTAG_ERR_BAD_REPLY			(-8)
#define PLCTAG_ERR_BAD_STATUS			(-9)
#define PLCTAG_ERR_CLOSE				(-10)
#define PLCTAG_ERR_CREATE				(-11)
#define PLCTAG_ERR_DUPLICATE			(-12)
#define PLCTAG_ERR_ENCODE				(-13)
#define PLCTAG_ERR_MUTEX_DESTROY		(-14)
#define PLCTAG_ERR_MUTEX_INIT			(-15)
#define PLCTAG_ERR_MUTEX_LOCK			(-16)
#define PLCTAG_ERR_MUTEX_UNLOCK			(-17)
#define PLCTAG_ERR_NOT_ALLOWED			(-18)
#define PLCTAG_ERR_NOT_FOUND			(-19)
#define PLCTAG_ERR_NOT_IMPLEMENTED		(-20)
#define PLCTAG_ERR_NO_DATA				(-21)
#define PLCTAG_ERR_NO_MATCH				(-22)
#define PLCTAG_ERR_NO_MEM				(-23)
#define PLCTAG_ERR_NO_RESOURCES			(-24)
#define PLCTAG_ERR_NULL_PTR				(-25)
#define PLCTAG_ERR_OPEN					(-26)
#define PLCTAG_ERR_OUT_OF_BOUNDS		(-27)
#define PLCTAG_ERR_READ					(-28)
#define PLCTAG_ERR_REMOTE_ERR			(-29)
#define PLCTAG_ERR_THREAD_CREATE		(-30)
#define PLCTAG_ERR_THREAD_JOIN			(-31)
#define PLCTAG_ERR_TIMEOUT				(-32)
#define PLCTAG_ERR_TOO_LARGE			(-33)
#define PLCTAG_ERR_TOO_SMALL			(-34)
#define PLCTAG_ERR_UNSUPPORTED			(-35)
#define PLCTAG_ERR_WINSOCK				(-36)
#define PLCTAG_ERR_WRITE				(-37)
#define PLCTAG_ERR_PARTIAL				(-38)
#define PLCTAG_ERR_BUSY					(-39)

#define PLCTAG_DEBUG_NONE				(0)
#define PLCTAG_DEBUG_ERROR				(1)
#define PLCTAG_DEBUG_WARN				(2)
#define PLCTAG_DEBUG_INFO				(3)
#define PLCTAG_DEBUG_DETAIL				(4)
#define PLCTAG_DEBUG_SPEW				(5)

#define PLCTAG_EVENT_READ_STARTED		(1)
#define PLCTAG_EVENT_READ_COMPLETED		(2)
#define PLCTAG_EVENT_WRITE_STARTED		(3)
#define PLCTAG_EVENT_WRITE_COMPLETED	(4)
#define PLCTAG_EVENT_ABORTED			(5)
#define PLCTAG_EVENT_DESTROYED			(6)


//===========================================================================================
//									 DLL Import
//===========================================================================================

//Colsole창 등록 및 제거
extern "C" __declspec(dllimport) HANDLE SetConsole();															// Debugging Console 창 생성
	// return : Console Handle	
extern "C" __declspec(dllimport) void ReleaseConsole();															// Debugging Console 창 제거

// PLC 설정
extern "C" __declspec(dllimport) void InitPLC(CString IpAddress, CString Cpu, CString Path, int DebugLevel = 0);
	// CPU : LGX, SLX, PLC5
	// Path : A,B
	//		A : Communication Port Type : 1 - Backplane, 2 - Control Net / Ethernet, DH + Channel A, DH + Channel B, 3 - Serial
	//		B : Slot number where cpu is installed: 0,1.. </param>

// Tag 생성 및 등록
extern "C" __declspec(dllimport) CString InitTag(int TagIndex, CString TagName, int DataType, bool bArray = FALSE, int ArrayStartAddr = 0, int ArrayCount = 1);
// return : 생성 결과에 대한 문자
//			INIT_SUCCESS : Success
//			ELSE : Failure
// PLC Tag Release
extern "C" __declspec(dllimport) void ReleasePLC();

// Tag 관리
extern "C" __declspec(dllimport) void RemoveTag(int TagIndex);												// Tag를 List에서 삭제
extern "C" __declspec(dllimport) void Dispose();															// Tag List 초기화

// Tag 실행 결과 Descript 확인
extern "C" __declspec(dllimport) CString DecodeError(int errCode);											// Error 정보 Script 가져오기
	// return : Code에 대한 Descript

// Tag Data 가져오기
extern "C" __declspec(dllimport) int ReadData(CString &retData, int TagIndex, int timeout);					// Get STRING Data
__declspec(dllimport) int ReadData(ULINT* &retData, int TagIndex, int ArrayCount, int timeout);				// Get ULINT(Unsigned 64bit) Data
__declspec(dllimport) int ReadData(LINT* &retData, int TagIndex, int ArrayCount, int timeout);				// Get LINT(Signed 64bit) Data
__declspec(dllimport) int ReadData(LREAL* &retData, int TagIndex, int ArrayCount, int timeout);				// Get LREAL(Signed 64bit) Data
__declspec(dllimport) int ReadData(UDINT* &retData, int TagIndex, int ArrayCount, int timeout);				// Get UDINT(Unsigned 32bit) Data
__declspec(dllimport) int ReadData(DINT* &retData, int TagIndex, int ArrayCount, int timeout);				// Get DINT(Signed 32bit) Data
__declspec(dllimport) int ReadData(REAL* &retData, int TagIndex, int ArrayCount, int timeout);				// Get REAL(Signed 32bit) Data
__declspec(dllimport) int ReadData(UINT16* &retData, int TagIndex, int ArrayCount, int timeout);			// Get UINT(Unsigned 16bit) Data
__declspec(dllimport) int ReadData(INT16* &retData, int TagIndex, int ArrayCount, int timeout);				// Get INT(Signed 16bit) Data
__declspec(dllimport) int ReadData(USINT* &retData, int TagIndex, int ArrayCount, int timeout);				// Get USINT(Unsigned 8bit) Data
__declspec(dllimport) int ReadData(SINT* &retData, int TagIndex, int ArrayCount, int timeout);				// Get SINT(Signed 8bit) Data
	// return : 명령어 수행 결과 (DecodeError()에 입력하면 Descript 확인 가능
	// retData : Tag Value
	// TagIndex : InitTag 시 정의된 Tag Index
	// ArrayCount : 데이터 개수(InitTag 시 Array = TRUE에 해당, Array=FALSE일경우 무조건 1)
	// timeout : 명령 수행 Timeout 시간

// Tag Data 쓰기
extern "C" __declspec(dllimport) int SetTagValue(int TagIndex, int offset, int DataType, ULINT value);		// Set ULINT(Unsigned 64bit) Data
__declspec(dllimport) int SetTagValue(int TagIndex, int offset, int DataType, LINT value);					// Set LINT(Signed 64bit) Data
__declspec(dllimport) int SetTagValue(int TagIndex, int offset, int DataType, LREAL value);					// Set LREAL(Signed 64bit) Data
__declspec(dllimport) int SetTagValue(int TagIndex, int offset, int DataType, UDINT value);					// Set UDINT(Unsigned 32bit) Data
__declspec(dllimport) int SetTagValue(int TagIndex, int offset, int DataType, DINT value);					// Set DINT(Signed 32bit) Data
__declspec(dllimport) int SetTagValue(int TagIndex, int offset, int DataType, REAL value);					// Set REAL(Signed 32bit) Data
__declspec(dllimport) int SetTagValue(int TagIndex, int offset, int DataType, UINT16 value);				// Set UINT(Unsigned 16bit) Data
__declspec(dllimport) int SetTagValue(int TagIndex, int offset, int DataType, INT16 value);					// Set INT(Signed 16bit) Data
__declspec(dllimport) int SetTagValue(int TagIndex, int offset, int DataType, USINT value);					// Set USINT(Unsigned 8bit) Data
__declspec(dllimport) int SetTagValue(int TagIndex, int offset, int DataType, SINT value);					// Set SINT(Signed 8bit) Data
__declspec(dllimport) int SetTagValue(int TagIndex, int DataType, int index, bool value, int timeout);		// Get BOOL(1Bit) Data
	// return : 명령어 수행 결과 (DecodeError()에 입력하면 Descript 확인 가능
	// TagIndex : InitTag 시 정의된 Tag Index
	// Offset : Array의 경우 데이터 순번(InitTag 시 Array = TRUE에 해당, Array=FALSE일경우 무조건 0)
	// DataType : 가져올 데이터 형 (InitTag 시 정의된 m_iDataType
	// timeout : 명령 수행 Timeout 시간
	// retData : Tag Value

// Tag 쓰기 실행
extern "C" __declspec(dllimport) int WriteTag(int TagIndex, int timeout);
// return : 명령어 수행 결과 (DecodeError()에 입력하면 Descript 확인 가능

// String형 Tag 쓰기 실행
extern "C" __declspec(dllimport) int WriteData(CString resData, int TagIndex, int timeout);
// return : 명령어 수행 결과 (DecodeError()에 입력하면 Descript 확인 가능

