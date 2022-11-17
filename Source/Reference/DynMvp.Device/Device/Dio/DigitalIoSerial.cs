using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.Dio
{
    public class DigitalIoSerial : DigitalIo
    {
        SerialDigitalIoInfo serialDigitalIoInfo = null;
        SerialPortEx serialPortEx = null;

        int dtrBitMask;
        int rtsBitMask;

        public DigitalIoSerial(string name)
            : base(DigitalIoType.Serial2DIO, name)
        {
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            if (IsReady())
                return false;

            SerialDigitalIoInfo serialDigitalIoInfo = digitalIoInfo as SerialDigitalIoInfo;
            if (serialDigitalIoInfo == null)
                return false;

            bool ok = base.Initialize(serialDigitalIoInfo);
            if (!ok)
                return false;

            this.serialDigitalIoInfo = serialDigitalIoInfo;

            this.dtrBitMask = this.serialDigitalIoInfo.DtrPort;
            this.rtsBitMask = this.serialDigitalIoInfo.RtsPort;

            this.serialPortEx = new SerialPortEx(new SimplePacketParser());
            this.serialPortEx.Open(this.Name, serialDigitalIoInfo.SerialPortInfo);
            UpdateState(DeviceState.Ready);
            return true;
        }

        public override void Release()
        {
            base.Release();

            this.serialPortEx.Close();
            UpdateState(DeviceState.Idle, "Device unloaded");

        }

        public override uint ReadInputGroup(int groupNo)
        {
            throw new NotImplementedException();
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            int dtrPort = this.serialPortEx.SerialPort.DtrEnable ? 1 : 0;
            int rtsPort = this.serialPortEx.SerialPort.RtsEnable ? 1 : 0;
            int value = (dtrPort << (dtrBitMask - 1)) | (rtsPort << (rtsBitMask - 1));
            return (uint)value;
            //if(serialDigitalIoInfo.rts)
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            bool dtrValue = (outputPortStatus & this.dtrBitMask) > 0;
            bool rtsValue = (outputPortStatus & this.rtsBitMask) > 0;

            serialPortEx.SerialPort.DtrEnable = dtrValue;
            serialPortEx.SerialPort.RtsEnable = rtsValue;
        }
    }
}
