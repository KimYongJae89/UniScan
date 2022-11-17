#include "pch.h"

#include "ABPLC_TagCommW.h"


#include "Lib/ABPLC_TagCommDef.h"
#pragma comment(lib, "Lib/ABPLC_TagComm.lib")

namespace ABPLCTagCommW
{
	// private
	CString ABPLCW::MarshalString(String^ s)
	{
		using namespace Runtime::InteropServices;
		CString os;
		const char* chars = (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
		os = chars;
		Marshal::FreeHGlobal(IntPtr((void*)chars));
		return os;
	}

	void ABPLCW::ThrowIfError(int iStatusCode)
	{
		if (iStatusCode != PLCTAG_STATUS_OK)
		{
			String^ managedString = GetErrorString(iStatusCode);
			throw gcnew Exception(managedString);
		}
	}

	// public
	System::IntPtr ABPLCW::InitConsol()
	{
		return (System::IntPtr)::SetConsole();
	}

	void ABPLCW::DisposeConsol()
	{
		::ReleaseConsole();
	}

	void ABPLCW::InitPLC(String^ ipAddress, String^ cpuType, String^ path, int debugLevel)
	{
		//::InitPLC(PLC_IPADDRESS, CPU_LGX, PLC_PATH, 0);

		::InitPLC(ipAddress, cpuType, path, 0);

		/*CString strIpAddr = MarshalString(ipAddress);
		CString strCpuType = MarshalString(cpuType);
		CString strPath = MarshalString(path);
		::InitPLC(strIpAddr, strCpuType, strPath, debugLevel);*/
	}

	void ABPLCW::ReleasePLC()
	{
		::ReleasePLC();
	}

	void ABPLCW::Register(int iTagIndex, String^ strTagName, int iDataType, int iCount)
	{
		bool bArray = (iCount > 1);
		CString csMessage = ::InitTag(iTagIndex, strTagName, iDataType, bArray, 0, iCount);
		if (csMessage != INIT_OK)
		{
			String^ managedString = gcnew String(csMessage);
			throw gcnew Exception(managedString);
		}
	}

	void ABPLCW::RegisterStr(int iTagIndex, String^ strTagName)
	{
		CString csMessage = ::InitTag(iTagIndex, strTagName, DT_STRING, false, 0, STRING_DATA_LEN);
		if (csMessage != INIT_OK)
		{
			String^ managedString = gcnew String(csMessage);
			throw gcnew Exception(managedString);
		}
	}

	void ABPLCW::UnRegister(int iTagIndex)
	{
		::RemoveTag(iTagIndex);
	}

	void ABPLCW::SetTagValue(int iTagIndex, int iOffset, int iDataType, System::Byte value)
	{
		ThrowIfError(::SetTagValue(iTagIndex, iOffset, iDataType, (USINT)value));
	}

	void ABPLCW::SetTagValue(int iTagIndex, int iOffset, int iDataType, System::SByte value)
	{
		ThrowIfError(::SetTagValue(iTagIndex, iOffset, iDataType, (SINT)value));
	}

	void ABPLCW::SetTagValue(int iTagIndex, int iOffset, int iDataType, System::Int16 value)
	{
		ThrowIfError(::SetTagValue(iTagIndex, iOffset, iDataType, (INT16)value));
	}

	void ABPLCW::SetTagValue(int iTagIndex, int iOffset, int iDataType, System::UInt16 value)
	{
		ThrowIfError(::SetTagValue(iTagIndex, iOffset, iDataType, (UINT16)value));
	}

	void ABPLCW::SetTagValue(int iTagIndex, int iOffset, int iDataType, System::Int32 value)
	{
		ThrowIfError(::SetTagValue(iTagIndex, iOffset, iDataType, (DINT)value));
	}

	void ABPLCW::SetTagValue(int iTagIndex, int iOffset, int iDataType, System::UInt32 value)
	{
		ThrowIfError(::SetTagValue(iTagIndex, iOffset, iDataType, (UDINT)value));
	}

	void ABPLCW::SetTagValue(int iTagIndex, int iOffset, int iDataType, System::Int64 value)
	{
		ThrowIfError(::SetTagValue(iTagIndex, iOffset, iDataType, (LINT)value));
	}

	void ABPLCW::SetTagValue(int iTagIndex, int iOffset, int iDataType, System::UInt64 value)
	{
		ThrowIfError(::SetTagValue(iTagIndex, iOffset, iDataType, (ULINT)value));
	}

	void ABPLCW::SetTagValue(int iTagIndex, int iOffset, int iDataType, float value)
	{
		ThrowIfError(::SetTagValue(iTagIndex, iOffset, iDataType, (REAL)value));
	}

	void ABPLCW::SetTagValue(int iTagIndex, int iOffset, int iDataType, double value)
	{
		ThrowIfError(::SetTagValue(iTagIndex, iOffset, iDataType, (LREAL)value));
	}

	void ABPLCW::WriteData(int iTagIndex, int iTimeout)
	{
		ThrowIfError(::WriteTag(iTagIndex, iTimeout));
	}

	void ABPLCW::ReadData(array<System::Byte>^ value, int iTagIndex, int iArrayCount, int iTimeout)
	{
		USINT *data = { 0 };
		ThrowIfError(::ReadData(data, iTagIndex, iArrayCount, iTimeout));

		pin_ptr< System::Byte> pinPtrArray = &value[0];
		memcpy_s(pinPtrArray, sizeof(System::Byte)* iArrayCount, data, sizeof(USINT)* iArrayCount);

		/*for (int i = 0; i < iArrayCount; i++)
			value[i] = data[i];*/
	}

	void ABPLCW::ReadData(array<System::SByte>^ value, int iTagIndex, int iArrayCount, int iTimeout)
	{
		SINT *data = { 0 };
		ThrowIfError(::ReadData(data, iTagIndex, iArrayCount, iTimeout));

		pin_ptr< System::SByte> pinPtrArray = &value[0];
		memcpy_s(pinPtrArray, sizeof(System::SByte)* iArrayCount, data, sizeof(SINT)* iArrayCount);
	}

	void ABPLCW::ReadData(array<System::Int16>^ value, int iTagIndex, int iArrayCount, int iTimeout)
	{
		Int16 *data = { 0 };
		ThrowIfError(::ReadData(data, iTagIndex, iArrayCount, iTimeout));

		pin_ptr< System::Int16> pinPtrArray = &value[0];
		memcpy_s(pinPtrArray, sizeof(System::Int16)* iArrayCount, data, sizeof(Int16)* iArrayCount);
	}

	void ABPLCW::ReadData(array<System::UInt16>^ value, int iTagIndex, int iArrayCount, int iTimeout)
	{
		UInt16 *data = { 0 };
		ThrowIfError(::ReadData(data, iTagIndex, iArrayCount, iTimeout));

		pin_ptr< System::UInt16> pinPtrArray = &value[0];
		memcpy_s(pinPtrArray, sizeof(System::UInt16)* iArrayCount, data, sizeof(UInt16)* iArrayCount);
	}

	void ABPLCW::ReadData(array<System::Int32>^  value, int iTagIndex, int iArrayCount, int iTimeout)
	{
		DINT *data = { 0 };
		ThrowIfError(::ReadData(data, iTagIndex, iArrayCount, iTimeout));

		pin_ptr< System::Int32> pinPtrArray = &value[0];
		memcpy_s(pinPtrArray, sizeof(System::Int32)* iArrayCount, data, sizeof(DINT)* iArrayCount);
	}

	void ABPLCW::ReadData(array<System::UInt32>^  value, int iTagIndex, int iArrayCount, int iTimeout)
	{
		UDINT *data = { 0 };
		ThrowIfError(::ReadData(data, iTagIndex, iArrayCount, iTimeout));

		pin_ptr< System::UInt32> pinPtrArray = &value[0];
		memcpy_s(pinPtrArray, sizeof(System::UInt32)* iArrayCount, data, sizeof(UDINT)* iArrayCount);
	}

	void ABPLCW::ReadData(array<System::Int64>^ value, int iTagIndex, int iArrayCount, int iTimeout)
	{
		LINT *data = { 0 };
		ThrowIfError(::ReadData(data, iTagIndex, iArrayCount, iTimeout));

		pin_ptr< System::Int64> pinPtrArray = &value[0];
		memcpy_s(pinPtrArray, sizeof(System::Int64)* iArrayCount, data, sizeof(LINT)* iArrayCount);
	}

	void ABPLCW::ReadData(array<System::UInt64>^ value, int iTagIndex, int iArrayCount, int iTimeout)
	{
		ULINT *data = { 0 };
		ThrowIfError(::ReadData(data, iTagIndex, iArrayCount, iTimeout));

		pin_ptr< System::UInt64> pinPtrArray = &value[0];
		memcpy_s(pinPtrArray, sizeof(System::UInt64)* iArrayCount, data, sizeof(ULINT)* iArrayCount);
	}

	void ABPLCW::ReadData(array<float>^ value, int iTagIndex, int iArrayCount, int iTimeout)
	{
		float *data = { 0 };
		ThrowIfError(::ReadData(data, iTagIndex, iArrayCount, iTimeout));

		pin_ptr< float> pinPtrArray = &value[0];
		memcpy_s(pinPtrArray, sizeof(float)* iArrayCount, data, sizeof(float)* iArrayCount);
	}

	void ABPLCW::ReadData(array<double>^ value, int iTagIndex, int iArrayCount, int iTimeout)
	{
		double *data = { 0 };
		ThrowIfError(::ReadData(data, iTagIndex, iArrayCount, iTimeout));

		pin_ptr< double> pinPtrArray = &value[0];
		memcpy_s(pinPtrArray, sizeof(double)* iArrayCount, data, sizeof(double)* iArrayCount);
	}

	void ABPLCW::WriteString(int iTagIndex, int iTimeout, String ^ string)
	{
		CString str = string;
		ThrowIfError(::WriteData(string, iTagIndex, iTimeout));
	}

	String^ ABPLCW::ReadString(int iTagIndex, int iTimeout)
	{
		CString retStr = _T("");
		ThrowIfError(::ReadData(retStr, iTagIndex, iTimeout));

		String^ string = gcnew String(retStr);
		return string;
	}

	String^ ABPLCW::GetErrorString(int errCode)
	{
		CString strString = ::DecodeError(errCode);
		String^ managedString = gcnew String(strString);
		return managedString;
	}
}

