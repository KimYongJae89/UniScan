using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Base
{
    /// <summary>
    /// 네트워크 리소스
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct NetworkResource
    {
        /// <summary>
        /// 범위
        /// </summary>
        public uint Scope;

        /// <summary>
        /// 타입
        /// </summary>
        public uint Type;

        /// <summary>
        /// 표시 타입
        /// </summary>
        public uint DisplayType;

        /// <summary>
        /// 용법
        /// </summary>
        public uint Usage;

        /// <summary>
        /// 로컬명
        /// </summary>
        public string LocalName;

        /// <summary>
        /// 원격명
        /// </summary>
        public string RemoteName;

        /// <summary>
        /// 주석
        /// </summary>
        public string Comment;

        /// <summary>
        /// 제공자
        /// </summary>
        public string Provider;
    }

    public class NetworkDrive
    {
        #region Consts
        const int RESOURCE_CONNECTED = 0x00000001;
        const int RESOURCE_GLOBALNET = 0x00000002;
        const int RESOURCE_REMEMBERED = 0x00000003;

        const int RESOURCETYPE_ANY = 0x00000000;
        const int RESOURCETYPE_DISK = 0x00000001;
        const int RESOURCETYPE_PRINT = 0x00000002;

        const int RESOURCEDISPLAYTYPE_GENERIC = 0x00000000;
        const int RESOURCEDISPLAYTYPE_DOMAIN = 0x00000001;
        const int RESOURCEDISPLAYTYPE_SERVER = 0x00000002;
        const int RESOURCEDISPLAYTYPE_SHARE = 0x00000003;
        const int RESOURCEDISPLAYTYPE_FILE = 0x00000004;
        const int RESOURCEDISPLAYTYPE_GROUP = 0x00000005;

        const int RESOURCEUSAGE_CONNECTABLE = 0x00000001;
        const int RESOURCEUSAGE_CONTAINER = 0x00000002;


        const int CONNECT_INTERACTIVE = 0x00000008;
        const int CONNECT_PROMPT = 0x00000010;
        const int CONNECT_REDIRECT = 0x00000080;
        const int CONNECT_UPDATE_PROFILE = 0x00000001;
        const int CONNECT_COMMANDLINE = 0x00000800;
        const int CONNECT_CMD_SAVECRED = 0x00001000;

        const int CONNECT_LOCALDRIVE = 0x00000100;
        #endregion

        #region Errors
        const int NO_ERROR = 0;

        const int ERROR_ACCESS_DENIED = 5;
        const int ERROR_ALREADY_ASSIGNED = 85;
        const int ERROR_BAD_DEVICE = 1200;
        const int ERROR_BAD_NET_NAME = 67;
        const int ERROR_BAD_PROVIDER = 1204;
        const int ERROR_CANCELLED = 1223;
        const int ERROR_EXTENDED_ERROR = 1208;
        const int ERROR_INVALID_ADDRESS = 487;
        const int ERROR_INVALID_PARAMETER = 87;
        const int ERROR_INVALID_PASSWORD = 1216;
        const int ERROR_MORE_DATA = 234;
        const int ERROR_NO_MORE_ITEMS = 259;
        const int ERROR_NO_NET_OR_BAD_PATH = 1203;
        const int ERROR_NO_NETWORK = 1222;

        const int ERROR_BAD_PROFILE = 1206;
        const int ERROR_CANNOT_OPEN_PROFILE = 1205;
        const int ERROR_DEVICE_IN_USE = 2404;
        const int ERROR_NOT_CONNECTED = 2250;
        const int ERROR_OPEN_FILES = 2401;
        #endregion

        // Created with excel formula:
        // ="new ErrorClass("&A1&", """&PROPER(SUBSTITUTE(MID(A1,7,LEN(A1)-6), "_", " "))&"""), "
        public static string GetErrorString(int errorCode)
        {
            switch(errorCode)
            {
                case ERROR_ACCESS_DENIED:
                    return "Error: Access Denied";
                case ERROR_ALREADY_ASSIGNED:
                    return "Error: Already Assigned";
                case ERROR_BAD_DEVICE:
                    return "Error: Bad Device";
                case ERROR_BAD_NET_NAME:
                    return "Error: Bad Net Name";
                case ERROR_BAD_PROVIDER:
                    return "Error: Bad Provider";
                case ERROR_CANCELLED:
                    return "Error: Cancelled";
                case ERROR_EXTENDED_ERROR:
                    return "Error: Extended Error";
                case ERROR_INVALID_ADDRESS:
                    return "Error: Invalid Address";
                case ERROR_INVALID_PARAMETER:
                    return "Error: Invalid Parameter";
                case ERROR_INVALID_PASSWORD:
                    return "Error: Invalid Password";
                case ERROR_MORE_DATA:
                    return "Error: More Data";
                case ERROR_NO_MORE_ITEMS:
                    return "Error: No More Items";
                case ERROR_NO_NET_OR_BAD_PATH:
                    return "Error: No Net Or Bad Path";
                case ERROR_NO_NETWORK:
                    return "Error: No Network";
                case ERROR_BAD_PROFILE:
                    return "Error: Bad Profile";
                case ERROR_CANNOT_OPEN_PROFILE:
                    return "Error: Cannot Open Profile";
                case ERROR_DEVICE_IN_USE:
                    return "Error: Device In Use";
                case ERROR_NOT_CONNECTED:
                    return "Error: Not Connected";
                case ERROR_OPEN_FILES:
                    return "Error: Open Files";
                default:
                    return "Error: Unknowen";
            }
        }


        #region API
        /// <summary>
        /// 네트워크 드라이브 연결하기
        /// </summary>
        /// <param name="ownerWindowHandle">소유자 윈도우 핸들</param>
        /// <param name="networkResource">네트워크 리소스</param>
        /// <param name="password">패스워드</param>
        /// <param name="userID">사용자 ID</param>
        /// <param name="flag">플래그</param>
        /// <param name="accessNameStringBuilder">액세스명 StringBuilder</param>
        /// <param name="bufferSize">버퍼 크기</param>
        /// <param name="result">결과 코드</param>
        /// <returns>처리 결과</returns>
        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        public static extern int WNetUseConnection
    (
        IntPtr ownerWindowHandle,
        [MarshalAs(UnmanagedType.Struct)] ref NetworkResource networkResource,

        string password,
        string userID,
        uint flag,
        StringBuilder accessNameStringBuilder,
        ref int bufferSize,
        out uint result
    );

        /// <summary>
        /// 네트워크 드라이브 연결 끊기
        /// </summary>
        /// <param name="localName">로컬명</param>
        /// <param name="flag">플래그</param>
        /// <param name="force">Force</param>
        /// <returns>처리 결과</returns>
        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = CharSet.Auto)]
        public static extern int WNetCancelConnection2A(string localName, int flag, int force);

        #endregion

        public NetworkResource NetworkResource => networkResource;
        NetworkResource networkResource;

        public NetworkDrive()
        {
            this.networkResource = new NetworkResource();
        }

        /// <summary>
        /// 네트워크 드라이브 연결하기
        /// </summary>
        /// <param name="networkDrive">네트워크 드라이브</param>
        /// <param name="shareFolder">공유 폴더</param>
        /// <param name="userID">사용자 ID</param>
        /// <param name="password">패스워드</param>
        /// <returns>
        /// 처리 결과
        /// 정상 : 0
        /// 오류 : 0이 아닌 값
        ///        - 1203 (공유 폴더 경로 오류)
        ///        - 1326 (사용자/패스워드 불일치)
        /// </returns>
        public int ConnectNetworkDrive(string networkDrive, string shareFolder, string userID, string password)
        {
            networkResource.Type = 1;
            networkResource.LocalName = networkDrive;
            networkResource.RemoteName = shareFolder;
            networkResource.Provider = null;

            uint flag = 0u;
            int bufferSize = 64;
            StringBuilder stringBuilder = new StringBuilder(bufferSize);
            uint result = 0u;

            return WNetUseConnection
            (
                IntPtr.Zero,
                ref networkResource,
                password,
                userID,
                flag,
                stringBuilder,
                ref bufferSize,
                out result
            );
        }

        /// <summary>
        /// 네트워크 드라이브 연결 끊기
        /// </summary>
        /// <param name="networkDrive">네트워드 드라이브</param>
        public void DisconnectNetworkDrive()
        {
            if (!string.IsNullOrEmpty(this.networkResource.LocalName))
                WNetCancelConnection2A(this.networkResource.LocalName, 1, 0);
        }
    }
}
