using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Daq
{
    public enum DaqChannelType
    {
        None, Virtual, Daqmx, MeDAQ, USB,
    }

    public abstract class DaqChannel : Device
    {
        DaqChannelType daqChannelType;
        public DaqChannelType DaqChannelType
        {
            get { return daqChannelType; }
            set { daqChannelType = value; }
        }

        DaqChannelProperty channelProperty;
        public DaqChannelProperty ChannelProperty
        {
            get { return channelProperty; }
            set { channelProperty = value; }
        }

        public DaqChannel(DaqChannelType daqChannelType)
        {
            Name = daqChannelType.ToString();
            DeviceType = DeviceType.DaqChannel;
            this.daqChannelType = daqChannelType;
            UpdateState(DeviceState.Idle);
        }

        public abstract void Initialize(DaqChannelProperty daqChannelProperty);

        public abstract double[] ReadVoltage(int numSamples);
        public abstract double[] ReadData(int numSamples);
    }
}
