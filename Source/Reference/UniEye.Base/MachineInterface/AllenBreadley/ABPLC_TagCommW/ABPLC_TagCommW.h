#pragma once
#include <atlstr.h>
#include "Lib/ABPLC_TagCommDef.h"

using namespace System;

namespace ABPLCTagCommW {
	// ABPLC_TagComm 래퍼 클래스
	public ref class ABPLCW abstract sealed
	{
	private:
		static ABPLCW() {}
		static CString MarshalString(String^ s);
		static void ThrowIfError(int iStatusCode);

	public:
		static System::IntPtr InitConsol();
		static void DisposeConsol();

		static void InitPLC(String^ ipAddress, String^ cpuType, String^ path, int debugLevel);
		static void ReleasePLC();

		static void Register(int iTagIndex, String^ strTagName, int iDataType, int iCount);
		static void RegisterStr(int iTagIndex, String^ strTagName);
		static void UnRegister(int iTagIndex);

		static void SetTagValue(int iTagIndex, int iOffset, int iDataType, System::Byte value);
		static void SetTagValue(int iTagIndex, int iOffset, int iDataType, System::SByte value);
		static void SetTagValue(int iTagIndex, int iOffset, int iDataType, System::Int16 value);
		static void SetTagValue(int iTagIndex, int iOffset, int iDataType, System::UInt16 value);
		static void SetTagValue(int iTagIndex, int iOffset, int iDataType, System::Int32 value);
		static void SetTagValue(int iTagIndex, int iOffset, int iDataType, System::UInt32 value);
		static void SetTagValue(int iTagIndex, int iOffset, int iDataType, System::Int64 value);
		static void SetTagValue(int iTagIndex, int iOffset, int iDataType, System::UInt64 value);
		static void SetTagValue(int iTagIndex, int iOffset, int iDataType, float value);
		static void SetTagValue(int iTagIndex, int iOffset, int iDataType, double value);
		static void WriteData(int iTagIndex, int iTimeout);

		static void ReadData(array<System::Byte>^ value, int iTagIndex, int iArrayCount, int iTimeout);
		static void ReadData(array<System::SByte>^ value, int iTagIndex, int iArrayCount, int iTimeout);
		static void ReadData(array<System::Int16>^ value, int iTagIndex, int iArrayCount, int iTimeout);
		static void ReadData(array<System::UInt16>^ value, int iTagIndex, int iArrayCount, int iTimeout);
		static void ReadData(array<System::Int32>^ value, int iTagIndex, int iArrayCount, int iTimeout);
		static void ReadData(array<System::UInt32>^ value, int iTagIndex, int iArrayCount, int iTimeout);
		static void ReadData(array<System::Int64>^ value, int iTagIndex, int iArrayCount, int iTimeout);
		static void ReadData(array<System::UInt64>^ value, int iTagIndex, int iArrayCount, int iTimeout);
		static void ReadData(array<float>^ value, int iTagIndex, int iArrayCount, int iTimeout);
		static void ReadData(array<double>^ value, int iTagIndex, int iArrayCount, int iTimeout);

		static void WriteString(int iTagIndex, int iTimeout, String^ string);
		static String^ ReadString(int iTagIndex, int iTimeout);

		static String^ GetErrorString(int errCode);

	};
}
