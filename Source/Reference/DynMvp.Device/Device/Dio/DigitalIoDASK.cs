using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynMvp.Base;
using DynMvp.Device.Device.Dio;

namespace DynMvp.Devices.Dio
{
    class DigitalIoDASK : DigitalIo
    {
        short cardId;
        uint inputPortStatus = 0;
        uint outputPortStatus = 0;

        static int numAdlink7230 = 0;
        static int numAdlink7432 = 0;

        public DigitalIoDASK(DigitalIoType type, string name) : base(type, name)
        {
            switch (type)
            {
                case DigitalIoType.Adlink7230:
                    NumInPort = NumOutPort = 16;
                    break;
                case DigitalIoType.Adlink7432:
                    NumInPort = NumOutPort = 32;
                    break;
            }
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            bool ok = base.Initialize(digitalIoInfo);
            if (!ok)
                return false;

            switch (digitalIoType)
            {
                case DigitalIoType.Adlink7230:
                    cardId = DASK.Register_Card(DASK.PCI_7230, (ushort)numAdlink7230);
                    NumInPort = NumOutPort = 16;
                    numAdlink7230++;
                    break;

                case DigitalIoType.Adlink7432:
                    cardId = DASK.Register_Card(DASK.PCI_7432, (ushort)numAdlink7432);
                    NumInPort = NumOutPort = 32;
                    numAdlink7432++;
                    break;

                default:
                    throw new AlarmException(ErrorCodeDigitalIo.Instance.InvalidType, ErrorLevel.Fatal,
                        this.name, "Invalid Digital IO Type. {0}", new object[] { digitalIoType.ToString() }, "");
                    break;
            }

            if (cardId < 0)
                throw new AlarmException(ErrorCodeDigitalIo.Instance.FailToInitialize, ErrorLevel.Fatal,
                        this.name, "Fail to Initialize Digital IO", null, "");

            inputPortStatus = 0;
            DASK.DI_ReadPort((ushort)cardId, 0, out inputPortStatus);
            UpdateState(DeviceState.Ready);
            return true;

            //catch (Exception ex)
            //{
            //    ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.FailToInitialize, ErrorLevel.Error,
            //        this.name, String.Format("DASK DIO Device initalization is failed. ( Type = {0} ) : {1}", digitalIoType.ToString(), ex.Message));
            //}
        }

        public override void Release()
        {
            base.Release();

            try
            {
                if (IsReady())
                    DASK.Release_Card((ushort)cardId);
            }
            catch
            {
            }
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            lock (this)
            {
                this.outputPortStatus = outputPortStatus;
                for (ushort i = 0; i < 32; i++)
                    DASK.DO_WriteLine((ushort)cardId, 0, i, (ushort)((outputPortStatus >> i) & 1));
            }
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            DASK.DO_ReadPort((ushort)cardId, 0, out this.outputPortStatus);
            return outputPortStatus;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public override uint ReadInputGroup(int groupNo)
        {
            DASK.DI_ReadPort((ushort)cardId, 0, out this.inputPortStatus);
            return this.inputPortStatus;
        }
    }
}
