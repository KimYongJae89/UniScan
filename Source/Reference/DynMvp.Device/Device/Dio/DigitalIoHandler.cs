using DynMvp.Base;
using DynMvp.Device.Device.Dio;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.Dio
{
    public class DioValue
    {
        Dictionary<string, uint> valueList = new Dictionary<string, uint>();

        public int Count
        {
            get { return valueList.Count; }
        }

        string GetKeyName(int deviceNo, int groupNo)
        {
            return string.Format("{0:00}_{1:000}", deviceNo, groupNo);
        }

        public uint GetValue(int deviceNo, int groupNo)
        {
            return GetValue(GetKeyName(deviceNo, groupNo));
        }

        public uint GetValue(string keyName)
        {
            if (valueList.ContainsKey(keyName))
                return valueList[keyName];
            return 0;
        }

        public bool GetValue(IoPort ioPort)
        {
            if (ioPort.IsValid() == false)
                return false;

            uint groupValue = GetValue(GetKeyName(ioPort.DeviceNo, ioPort.GroupNo));
            return (((groupValue >> ioPort.PortNo) & 0x01) == (ioPort.ActiveLow ? 0 : 1));
        }

        public void AddValue(int deviceNo, int groupNo, uint value)
        {
            valueList.Add(GetKeyName(deviceNo, groupNo), value);
        }

        public void Copy(DioValue dioValue)
        {
            valueList.Clear();
            foreach (KeyValuePair<string, uint> keyValue in dioValue.valueList)
                valueList.Add(keyValue.Key, keyValue.Value);
        }

        public void UpdateBitFlag(IoPort ioPort, bool flag)
        {
            if (valueList.Count > ioPort.DeviceNo)
            {
                string keyName = GetKeyName(ioPort.DeviceNo, ioPort.GroupNo);
                uint value = valueList[keyName];
                DigitalIo.UpdateBitFlag(ref value, ioPort.PortNo, flag);

                valueList[keyName] = value;
            }
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            DioValue p = obj as DioValue;
            if ((System.Object)p == null)
            {
                return false;
            }

            if (valueList.Count != p.valueList.Count)
            {
                return false;
            }

            foreach (KeyValuePair<string, uint> keyValue in valueList)
            {
                if (keyValue.Value != p.GetValue(keyValue.Key))
                    return false;
            }

            return true;
        }

        public bool Equals(DioValue p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            if (valueList.Count != p.valueList.Count)
            {
                return false;
            }

            foreach (KeyValuePair<string, uint> keyValue in valueList)
            {
                if (keyValue.Value != p.GetValue(keyValue.Key))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int value = 0;

            foreach (KeyValuePair<string, uint> keyValue in valueList)
            {
                value ^= (int)keyValue.Value;
            }

            return value;
        }


    }

    public class DigitalIoHandler
    {
        public int Count
        {
            get { return digitalIoList.Count; }
        }

        private object lockOutput = new object();
        private List<IDigitalIo> digitalIoList = new List<IDigitalIo>();

        public IEnumerator<IDigitalIo> GetEnumerator()
        {
            return digitalIoList.GetEnumerator();
        }

        public IDigitalIo Get(int index)
        {
            if (index > -1 && index < digitalIoList.Count())
                return digitalIoList[index];

            return null;
        }

        public IDigitalIo Get(string deviceName)
        {
            int index = GetIndex(deviceName);
            if (index > -1)
            {
                return Get(index);
            }

            return null;
        }

        public int GetIndex(string deviceName)
        {
            for (int i = 0; i < digitalIoList.Count; i++)
            {
                if (digitalIoList[i].GetName() == deviceName)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Add(IDigitalIo digitalIo)
        {
            digitalIoList.Add(digitalIo);
        }

        public void Release()
        {
            foreach (IDigitalIo digitalIo in digitalIoList)
            {
                if (digitalIo.IsReady())
                    digitalIo.Release();
            }
            digitalIoList.Clear();
        }

        public void WriteOutputGroup(int deviceNo, int groupNo, uint outputPortStatus)
        {
            if (deviceNo < digitalIoList.Count())
            {
                digitalIoList[deviceNo].WriteOutputGroup(groupNo, outputPortStatus);
            }
        }

        public uint ReadOutputGroup(int deviceNo, int groupNo)
        {
            if (deviceNo < digitalIoList.Count())
                return digitalIoList[deviceNo].ReadOutputGroup(groupNo);

            return 0;
        }

        public void WriteInputGroup(int deviceNo, int groupNo, uint outputPortStatus)
        {
            if (deviceNo < digitalIoList.Count())
            {
                //digitalIoList[deviceNo].Write WriteInputGroup(groupNo, outputPortStatus);
            }
        }

        public uint ReadInputGroup(int deviceNo, int groupNo)
        {
            if (deviceNo < digitalIoList.Count())
                return digitalIoList[deviceNo].ReadInputGroup(groupNo);

            return 0;
        }

        public void LockOutput()
        {
            Monitor.Enter(lockOutput);
        }

        public void UnlockOutput()
        {
            Monitor.Exit(lockOutput);
        }

        public DioValue ReadOutput(bool lockProcess = false)
        {
            if (lockProcess)
                Monitor.Enter(lockOutput);

            DioValue dioValue = new DioValue();

            int index = 0;
            foreach (IDigitalIo digitalIo in digitalIoList)
            {
                for (int groupNo = 0; groupNo < digitalIo.GetNumOutPortGroup(); groupNo++)
                    dioValue.AddValue(index, groupNo, digitalIo.ReadOutputGroup(groupNo));
                index++;
            }

            return dioValue;
        }

        public bool ReadOutput(IoPort ioPort)
        {
            if (ioPort.DeviceNo < digitalIoList.Count())
            {
                uint value = digitalIoList[ioPort.DeviceNo].ReadOutputGroup(ioPort.GroupNo);
                return (((value >> ioPort.PortNo) & 0x1) == 1);
            }

            return false;
        }

        public void WriteOutput(DioValue dioValue, bool lockProcess = false)
        {
            for (int deviceNo = 0; deviceNo < digitalIoList.Count; deviceNo++)
            {
                for (int groupNo = 0; groupNo < digitalIoList[deviceNo].GetNumOutPortGroup(); groupNo++)
                    digitalIoList[deviceNo].WriteOutputGroup(groupNo, dioValue.GetValue(deviceNo, groupNo));
            }

            if (lockProcess == true)
                Monitor.Exit(lockOutput);
        }

        /// <summary>
        /// IO출력 활성화. (ActiveLow 옵션 적용)
        /// </summary>
        /// <param name="ioPort"></param>
        /// <param name="active"></param>
        public void SetOutput(IoPort ioPort, bool active)
        {
            bool writeValue = ioPort.ActiveLow ? !active : active;
            WriteOutput(ioPort, writeValue);
        }

        /// <summary>
        /// IO출력 활성화. (ActiveLow 옵션 적용)
        /// </summary>
        /// <param name="ioPort"></param>
        public bool SetOutputActive(IoPort ioPort)
        {
            if (!ioPort.IsValid())
                return false;

            LogHelper.Debug(LoggerType.IO, string.Format("DigitalIoHandler::SetOutputActive, Name: {0}, Group: {1}, Port: {2}", ioPort.Name, ioPort.GroupNo, ioPort.PortNo));
            bool writeValue = ioPort.ActiveLow ? false : true;
            WriteOutput(ioPort, writeValue);
            return true;
        }

        /// <summary>
        /// IO출력 비활성화. (ActiveLow 옵션 적용)
        /// </summary>
        /// <param name="ioPort"></param>
        public bool SetOutputDeactive(IoPort ioPort)
        {
            if (!ioPort.IsValid())
                return false;

            LogHelper.Debug(LoggerType.IO, string.Format("DigitalIoHandler::SetOutputDeactive, Name: {0}, Group: {1}, Port: {2}", ioPort.Name, ioPort.GroupNo, ioPort.PortNo));
            bool writeValue = ioPort.ActiveLow ? true : false;
            WriteOutput(ioPort, writeValue);
            return true;
        }

        /// <summary>
        /// IO 출력 설정 (ActiveLow 옵션 미적용)
        /// </summary>
        /// <param name="ioPort"></param>
        /// <param name="value"></param>
        public void WriteOutput(IoPort ioPort, bool value)
        {
            if (ioPort == null || ioPort.PortNo == IoPort.UNUSED_PORT_NO)
                return;

            if (ioPort.DeviceNo < digitalIoList.Count())
            {
                // Group No 사용하지 않은 코드. I/O에 문제 있을 경우, 참고 요
                //int bitPos = ioPort.PortNo;
                //int groupNo = bitPos / 32;
                //bitPos = bitPos - groupNo * 32;
                //Monitor.Enter(lockOutput);

                //uint outputPortStatus = digitalIoList[ioPort.DeviceNo].ReadOutputGroup(ioPort.GroupNo);

                //DigitalIo.UpdateBitFlag(ref outputPortStatus, ioPort.PortNo, value);

                //digitalIoList[ioPort.DeviceNo].WriteOutputGroup(ioPort.GroupNo, outputPortStatus);

                //Monitor.Exit(lockOutput);

                digitalIoList[ioPort.DeviceNo].WriteOutputPort(ioPort.GroupNo, ioPort.PortNo, value);
            }
        }

        public void WriteInput(DioValue dioValue)
        {
            for (int deviceNo = 0; deviceNo < digitalIoList.Count; deviceNo++)
            {
                for (int groupNo = 0; groupNo < digitalIoList[deviceNo].GetNumInPortGroup(); groupNo++)
                    digitalIoList[deviceNo].WriteInputGroup(groupNo, dioValue.GetValue(deviceNo, groupNo));
            }
        }

        public void WriteInput(IoPort ioPort, bool value)
        {
            if (ioPort.DeviceNo < digitalIoList.Count())
            {
                //int bitPos = ioPort.PortNo;
                //int groupNo = bitPos / 32;
                //bitPos = bitPos - groupNo * 32;
                uint inputPortStatus = digitalIoList[ioPort.DeviceNo].ReadInputGroup(ioPort.GroupNo);

                DigitalIo.UpdateBitFlag(ref inputPortStatus, ioPort.PortNo, value);

                IDigitalIo digitalIo = digitalIoList[ioPort.DeviceNo];
                if (digitalIo.IsVirtual)
                    digitalIoList[ioPort.DeviceNo].WriteInputGroup(ioPort.GroupNo, inputPortStatus);
            }
        }

        public DioValue ReadInput()
        {
            DioValue dioValue = new DioValue();

            int index = 0;
            foreach (IDigitalIo digitalIo in digitalIoList)
            {
                for (int groupNo = 0; groupNo < digitalIo.GetNumInPortGroup(); groupNo++)
                    dioValue.AddValue(index, groupNo, digitalIo.ReadInputGroup(groupNo));
                index++;
            }

            return dioValue;
        }

        public bool ReadInput(IoPort ioPort)
        {
            return ReadInput(ioPort.DeviceNo, ioPort.GroupNo, ioPort.PortNo);
        }

        public bool ReadInput(int deviceNo, int groupNo, int bitPos)
        {
            if (deviceNo < digitalIoList.Count())
            {
                //int groupNo = bitPos / 32;
                //bitPos = bitPos - groupNo * 32;
                uint inputPortStatus = digitalIoList[deviceNo].ReadInputGroup(groupNo);

                return (((inputPortStatus >> bitPos) & 0x1) == 1);
            }

            return false;
        }

        public string GetInputValueString()
        {
            string resultString = "";

            foreach (DigitalIo digitalIo in digitalIoList)
            {
                resultString += Convert.ToString(digitalIo.ReadInputGroup(0), 2).PadLeft(8, '0');
            }

            return resultString;
        }

        IDigitalIo ConnectToMaster(DigitalIoInfo digitalIoInfo, MotionInfoList motionInfoList)
        {
            SlaveDigitalIoInfo slaveDigitalIoInfo = (SlaveDigitalIoInfo)digitalIoInfo;
            if (String.IsNullOrEmpty(slaveDigitalIoInfo.MasterDeviceName))
            {
                ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.InvalidSetting, ErrorLevel.Fatal,
                    digitalIoInfo.Name, "Slave Digital Io must have the master device name.", null);
                return null;
            }

            IDigitalIo idigitalIo = null;

            Motion motion = (Motion)DeviceManager.Instance().GetDevice(DeviceType.MotionController, slaveDigitalIoInfo.MasterDeviceName);
            if (motion == null)
            {
                MotionInfo motionInfo = motionInfoList.GetMotionInfo(slaveDigitalIoInfo.MasterDeviceName);
                if (motionInfo == null)
                {
                    ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.CantFindMasterMotion, ErrorLevel.Fatal,
                        digitalIoInfo.Name, "Can't find master device info.", null);
                    return null;
                }

                motion = MotionFactory.Create(motionInfo);
            }

            if (motion != null)
            {
                if (motion is IDigitalIo)
                {
                    idigitalIo = (IDigitalIo)motion;
                }
                else
                {
                    ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.InvalidMasterMotion, ErrorLevel.Error,
                        digitalIoInfo.Name, "The master device can't work for Digital I/O.", null);
                    motion.Release();
                }
            }

            return idigitalIo;
        }

        public void Build(DigitalIoInfoList digitalIoInfoList, MotionInfoList motionInfoList)
        {
            foreach (DigitalIoInfo digitalIoInfo in digitalIoInfoList)
            {
                IDigitalIo idigitalIo = null;
                if (DigitalIoFactory.IsSlaveDevice(digitalIoInfo.Type))
                {
                    idigitalIo = ConnectToMaster(digitalIoInfo, motionInfoList);
                }
                else
                {
                    idigitalIo = DigitalIoFactory.Create(digitalIoInfo);
                }

                if (idigitalIo != null)
                    Add(idigitalIo);
            }
        }
    }
}
