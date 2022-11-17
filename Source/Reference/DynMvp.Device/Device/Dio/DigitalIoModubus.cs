using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynMvp.Base;
using DynMvp.Device.Device.Dio;
using ModbusRTU;

namespace DynMvp.Devices.Dio
{
    class DigitalIoModubus : DigitalIo
    {
        ModbusLib modbus;
        object lockObject = new object();

        public DigitalIoModubus(string name)
            : base(DigitalIoType.Modubus, name)
        {
            NumInPort = NumOutPort = 16;
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            SerialDigitalIoInfo serialDigitalIoInfo = (SerialDigitalIoInfo)digitalIoInfo;

            modbus = new ModbusLib();
            modbus.Port = serialDigitalIoInfo.SerialPortInfo.PortName; // "COM3"
            modbus.Baudrate = serialDigitalIoInfo.SerialPortInfo.BaudRate; // 57600;
            if (modbus.Open() == false)
            {
                ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.FailToInitialize, ErrorLevel.Error,
                    this.name, "Unknown error on initialization process for MODBUS device", null);
                return false;
            }
            modbus.Timeout = 1000;

            UpdateState(DeviceState.Ready);
            return true;
        }

        public override void Release()
        {
            base.Release();
            modbus.Close();
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            LogHelper.Debug(LoggerType.IO, "WriteOutputGroup");

            try
            {
                lock (lockObject)
                {
                    LogHelper.Debug(LoggerType.IO, "Start WriteOutputGroup");

                    bool[] value = new bool[16];
                    string valueStr = Convert.ToString(outputPortStatus, 2).PadLeft(16);
                    for (int i = 0; i < 16; i++)
                    {
                        value[i] = valueStr.Substring(15 - i, 1)[0] == '1';
                    }

                    modbus.MultiBitWrite(1, 5980, value);

                    LogHelper.Debug(LoggerType.IO, "End WriteOutputGroup");
                }
            }
            catch (TimeoutException ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Modubus Timeout Error : WriteOutputGroup {0}", ex.Message));
            }
            catch (ModbusLib.CRCErrorException ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Modubus CRC Error : WriteOutputGroup {0}", ex.Message));
            }
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            LogHelper.Debug(LoggerType.IO, "ReadOutputGroup");

            try
            {
                lock (lockObject)
                {
                    LogHelper.Debug(LoggerType.IO, "Start ReadOutputGroup");

                    BitArray bitData = modbus.BitRead(1, 5980, 16);

                    uint data = 0;
                    for (int i = 0; i < 16; i++)
                    {
                        if (bitData[i])
                        {
                            data = data | (((uint)1) << i);
                        }
                    }

                    LogHelper.Debug(LoggerType.IO, "End ReadOutputGroup");

                    return data;
                }
            }
            catch (TimeoutException ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Modubus Timeout Error : ReadOutputGroup {0}", ex.Message));
            }
            catch (ModbusLib.CRCErrorException ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Modubus CRC Error : ReadOutputGroup {0}", ex.Message));
            }

            return 0;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            LogHelper.Debug(LoggerType.IO, "ReadInputGroup");

            try
            {
                lock (lockObject)
                {
                    LogHelper.Debug(LoggerType.IO, "Start ReadInputGroup");

                    BitArray bitData = modbus.BitRead(1, 2980, 16);

                    uint data = 0;
                    for (int i = 0; i < 16; i++)
                    {
                        if (bitData[i])
                        {
                            data = data | (((uint)1) << i);
                        }
                    }

                    LogHelper.Debug(LoggerType.IO, "End ReadInputGroup");

                    return data;
                }
            }
            catch (TimeoutException ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Modubus Timeout Error : ReadInputGroup {0}", ex.Message));
            }
            catch (ModbusLib.CRCErrorException ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Modubus CRC Error : ReadInputGroup {0}", ex.Message));
            }

            return 0;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }
    }
}
