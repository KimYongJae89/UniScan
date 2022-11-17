using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.Comm
{
    public class PacketBuffer
    {
        byte[] dataByteFull;
        public byte[] DataByteFull
        {
            get { return dataByteFull; }
        }

        public bool Empty
        {
            get { return dataByteFull == null; }
        }

        const int MAXSIZE = 4096;

        public void AppendData(byte[] data, int numData)
        {
            int numDataFull = 0;
            if (dataByteFull != null)
                numDataFull = dataByteFull.Length;
            byte[] tempBuf = new byte[numDataFull + numData];
            if (dataByteFull != null)
                Array.Copy(dataByteFull, 0, tempBuf, 0, numDataFull);
            Array.Copy(data, 0, tempBuf, numDataFull, numData);
            dataByteFull = tempBuf;
        }

        public void RemoveData(int numData)
        {
            int numDataFull = dataByteFull.Length;

            if ((numDataFull - numData) > 0)
            {
                byte[] tempBuf = new byte[numDataFull - numData];
                Array.Copy(dataByteFull, numData, tempBuf, 0, numDataFull - numData);
                dataByteFull = tempBuf;
            }
            else
            {
                dataByteFull = null;
            }
        }

        public void Clear()
        {
            dataByteFull = null;
        }
    };

    public abstract class SendPacket
    {
        int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        bool waitResponse = false;
        public bool WaitResponse
        {
            get { return waitResponse; }
            set { waitResponse = value; }
        }

        public abstract bool IsValid();
    }

    public enum ProcessPacketResult
    {
        Complete, Incomplete, PacketError, OtherTarget
    }

    public interface IProtocol
    {
        byte[] MakePacket(SendPacket sendPacket);

        // 패킷을 읽은 후 receivedPacket을 생성하여 반환한다. 
        // 읽은 패킷은 packetBuffer에서 제거한다.
        ProcessPacketResult ParsePacket(PacketBuffer packetBuffer, out AsyncRecivedPacket receivedPacket);
    }
}
