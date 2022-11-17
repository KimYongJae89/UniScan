using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.Device.Device.Light;
using DynMvp.Device.Device.Light.SerialLigth.iCore;

namespace UniScanX.MPAlignment.Devices
{
    public enum ICoreChannel
    {
        IR, Blue, White, Back
    }


    class IPulse
    {
        SerialLightIPulse iPulseCtrol = null;
        private const int ID = 0;

        public IPulse(LightCtrl ctrl)
        {
            if (ctrl is SerialLightIPulse)
            {
                iPulseCtrol = ctrl as SerialLightIPulse;
                Initialize();
            }
        }
        private void Initialize()
        {
            if (iPulseCtrol == null)
                return;
            //조명 정격 전류 W4080 IR720, BL900 mA 
            IPulseFrame[] command = {
                            IPulseFrame.CreateWFrame(ID, Address.OperationMode_RW,1),
                            IPulseFrame.CreateWFrame(ID, Address.TriggerInputSource_RW, 1), //DigitalIO
                            IPulseFrame.CreateWFrame(ID, Address.TriggerInputActivation_RW, 0), //Rising Edge
                            IPulseFrame.CreateWFrame(ID, Address.SequenceMode_RW,0), //off
                            IPulseFrame.CreateWFrame(ID, Address.LED1RatedCurrent_RW,720), //IR
                            IPulseFrame.CreateWFrame(ID, Address.LED2RatedCurrent_RW,900), //BL
                            IPulseFrame.CreateWFrame(ID, Address.LED3RatedCurrent_RW,2000), //W 4048ma
                            IPulseFrame.CreateWFrame(ID, Address.LED4RatedCurrent_RW,500), //Back
                            IPulseFrame.CreateWFrame(ID, Address.Duration_RW, 100.0f), //us
                        };
            iPulseCtrol.SendCommand(command);
        }

        private void EnableSingleChannel(ICoreChannel channel)
        {
            if (iPulseCtrol == null)
                return;

            ushort[] channelOn = new ushort[4] { 0, 0, 0, 0 };
            channelOn[(int)channel] = 1;

            //IPulseFrame Mode = IPulseFrame.CreateWFrame(ID, Address.OperationMode_RW, 1);//Continuous

            IPulseFrame[] command = { //Mode,
                            IPulseFrame.CreateWFrame(ID, Address.LED1Enable_RW, channelOn[0]),
                            IPulseFrame.CreateWFrame(ID, Address.LED2Enable_RW, channelOn[1]),
                            IPulseFrame.CreateWFrame(ID, Address.LED3Enable_RW, channelOn[2]),
                            IPulseFrame.CreateWFrame(ID, Address.LED4Enable_RW, channelOn[3]),
                        };
            iPulseCtrol.SendCommand(command);
        }

        private void SetLightValue(ICoreChannel channel, int value) //value = 0~100%
        {
            if (iPulseCtrol == null)
                return;

            value = value > 255 ? 255 : value;
            IPulseFrame[] command = {
                            IPulseFrame.CreateWFrame(ID,
                            Address.LED1CurrentRateContinuous_RW + (ushort)channel,
                            (ushort) value)
                        };
            iPulseCtrol.SendCommand(command);
        }

        private void LightContinuousOn(bool on)
        {
            ushort OnOff = on == true ? (ushort)1 : (ushort)0;
            IPulseFrame[] command = {
                            IPulseFrame.CreateWFrame(ID, Address.OperationMode_RW, OnOff) // continuous
                        };
            iPulseCtrol.SendCommand(command);
        }

        public void LightOn(ICoreChannel channel, int value)
        {
            if (iPulseCtrol == null)
                return;
            EnableSingleChannel(channel);
            SetLightValue(channel, value);
            LightContinuousOn(true);
        }


        public void LightOffAll()
        {
            if (iPulseCtrol == null)
                return;
            LightContinuousOn(false);
        }
    }
}
