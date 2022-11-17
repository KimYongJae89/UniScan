using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shared;

using DynMvp.Base;
using DynMvp.Device.Device.Dio;

namespace DynMvp.Devices.Dio
{
    class DigitalIoComizoa : DigitalIo
    {
        bool initialized = true;

        public DigitalIoComizoa(DigitalIoType type, string name)
            : base(type, name)
        {
            switch (type)
            {
                case DigitalIoType.ComizoaSd424f:
                    NumInPort = NumOutPort = 16;
                    break;
            }
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            try
            {
                bool ok = base.Initialize(digitalIoInfo);
                if (!ok)
                    return false;

                int nNumAxes = 0;
                switch (digitalIoType)
                {
                    case DigitalIoType.ComizoaSd424f:
                        if (CMDLL.cmmGnDeviceLoad((int)Defines._TCmBool.cmTRUE, ref nNumAxes) != Defines.cmERR_NONE)
                        {
                            ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.FailToInitialize, ErrorLevel.Error,
                                digitalIoType.ToString(), "Comizoa Device registeration is failed. ( Type = {0} )", new object[] { digitalIoType.ToString() });
                            return false;
                        }

                        NumInPort = NumOutPort = 16;
                        initialized = true;

                        UpdateState(DeviceState.Ready);
                        return true;
                    default:
                        ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.InvalidType, ErrorLevel.Error,
                            this.name, "Invalid Digital IO Type. {0}", new object[] { digitalIoType.ToString() });
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.FailToInitialize, ErrorLevel.Error,
                    this.name, "Comizoa Device initalization is failed. ( Type = {0} ) : {1}", new object[] { digitalIoType.ToString(), ex.Message });
            }

            return false;
        }

        public override void Release()
        {
            base.Release();

            try
            {
                if (IsReady())
                    CMDLL.cmmGnDeviceUnload();
            }
            catch
            {
            }
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            CMDLL.cmmDoPutMulti(0, 16, (int)outputPortStatus);
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            int outputPortStatus = 0;
            CMDLL.cmmDoGetMulti(0, 16, ref outputPortStatus);

            return (uint)outputPortStatus;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            int inputPortStatus = 0;
            CMDLL.cmmDiGetMulti(0, 16, ref inputPortStatus);

            return (uint)inputPortStatus;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }
    }
}
