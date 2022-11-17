using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;

using DynMvp.Base;

using System.Threading;
using DynMvp.Devices.Comm;
using System.Threading.Tasks;

namespace DynMvp.Devices.MotionController
{
    //public class ErrorSubSectionMotion
    //{
    //    enum MotionError
    //    {
    //        Homing ,
    //        HomingTimeOut,
    //        Moving,
    //        MovingTimeOut,
    //        ContinuousMoving,
    //        StopMove,
    //        EmergencyStop,
    //        PosLimit,
    //        NegLimit,
    //        AmpFault,
    //        ServoOff,
    //        HomeFound,
    //        CantFindNegLimit,
    //        CantFindPosLimit,
    //    }
    //    public static ErrorSubSection Homing => ErrorSubSections.GetSubSection(MotionError.Homing);
    //    public static ErrorSubSection HomingTimeOut => ErrorSubSections.GetSubSection(MotionError.HomingTimeOut);
    //    public static ErrorSubSection Moving => ErrorSubSections.GetSubSection(MotionError.Moving);
    //    public static ErrorSubSection MovingTimeOut => ErrorSubSections.GetSubSection(MotionError.MovingTimeOut);
    //    public static ErrorSubSection ContinuousMoving => ErrorSubSections.GetSubSection(MotionError.ContinuousMoving);
    //    public static ErrorSubSection StopMove => ErrorSubSections.GetSubSection(MotionError.StopMove);
    //    public static ErrorSubSection EmergencyStop => ErrorSubSections.GetSubSection(MotionError.EmergencyStop);
    //    public static ErrorSubSection PosLimit => ErrorSubSections.GetSubSection(MotionError.PosLimit);
    //    public static ErrorSubSection NegLimit => ErrorSubSections.GetSubSection(MotionError.NegLimit);
    //    public static ErrorSubSection AmpFault => ErrorSubSections.GetSubSection(MotionError.AmpFault);
    //    public static ErrorSubSection ServoOff => ErrorSubSections.GetSubSection(MotionError.ServoOff);
    //    public static ErrorSubSection HomeFound => ErrorSubSections.GetSubSection(MotionError.HomeFound);
    //    public static ErrorSubSection CantFindNegLimit => ErrorSubSections.GetSubSection(MotionError.CantFindNegLimit);
    //    public static ErrorSubSection CantFindPosLimit => ErrorSubSections.GetSubSection(MotionError.CantFindPosLimit);
    //}

    public enum MotionType
    {
        None, Virtual, AlphaMotion302, AlphaMotion304, AlphaMotion314,
        AlphaMotionBx, FastechEziMotionPlusR, PowerPmac, Ajin,
        AlphaMotionBBx
    }

    public enum MovingProfileType
    {
        None, TCurve, SCurve
    }

    public struct MotionStatus
    {
        public bool origin; // Origin sensor
        public bool ez;     // signal
        public bool emg;    // signal
        public bool inp;    // signal
        public bool alarm;  // signal
        public bool posLimit;// pos-limit sensor
        public bool negLimit;// neg-limit sensor
        public bool run;    // signal
        public bool err;    // signal
        public bool home;   // signal
        public bool homeOk;   // signal
        public bool cClr;   // signal
        public bool servoOn;    // signal
        public bool aRst;   // signal
    }

    public struct AxisStatus
    {
        public bool isServoOn;
        public bool isMoving;
        public bool isFault;

        public static AxisStatus operator |(AxisStatus a, AxisStatus b)
        {
            return new AxisStatus { isServoOn = a.isServoOn | b.isServoOn, isMoving = a.isMoving | b.isMoving, isFault = a.isFault | b.isFault };
        }
        public void ResetStatus()
        {
            isServoOn = isMoving = isFault = false;
        }
    }


    public abstract class MotionInfo
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        MotionType type;
        public MotionType Type
        {
            get { return type; }
            set { type = value; }
        }

        int numAxis;
        public int NumAxis
        {
            get { return numAxis; }
            set { numAxis = value; }
        }

        public MotionInfo()
        {

        }

        public MotionInfo(string name, MotionType type, int numAxis)
        {
            this.name = name;
            this.type = type;
            this.numAxis = numAxis;
        }

        public virtual void LoadXml(XmlElement motionElement)
        {
            name = XmlHelper.GetValue(motionElement, "Name", "");
            type = (MotionType)Enum.Parse(typeof(MotionType), XmlHelper.GetValue(motionElement, "Type", "AlphaMotion302"));
            numAxis = Convert.ToInt32(XmlHelper.GetValue(motionElement, "NumAxis", "4"));
        }

        public virtual void SaveXml(XmlElement motionElement)
        {
            if (String.IsNullOrEmpty(name))
                XmlHelper.SetValue(motionElement, "Name", "");
            else
                XmlHelper.SetValue(motionElement, "Name", name.ToString());

            XmlHelper.SetValue(motionElement, "Type", type.ToString());
            XmlHelper.SetValue(motionElement, "NumAxis", numAxis.ToString());
        }

        public abstract MotionInfo Clone();

        public virtual void Copy(MotionInfo srcMotionInfo)
        {
            name = srcMotionInfo.name;
            type = srcMotionInfo.type;
            numAxis = srcMotionInfo.numAxis;
        }
    }

    public class VirtualMotionInfo : MotionInfo
    {
        public override MotionInfo Clone()
        {
            VirtualMotionInfo virtualMotionInfo = new VirtualMotionInfo();
            virtualMotionInfo.Copy(this);

            return virtualMotionInfo;
        }
    }

    public class PciMotionInfo : MotionInfo
    {
        int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public PciMotionInfo()
        {

        }

        public PciMotionInfo(string name, MotionType type, int numAxis, int index) : base(name, type, numAxis)
        {
            this.index = index;
        }

        public override void LoadXml(XmlElement motionElement)
        {
            base.LoadXml(motionElement);

            index = Convert.ToInt32(XmlHelper.GetValue(motionElement, "Index", "0"));
        }

        public override void SaveXml(XmlElement motionElement)
        {
            base.SaveXml(motionElement);

            XmlHelper.SetValue(motionElement, "Index", index.ToString());
        }

        public override MotionInfo Clone()
        {
            PciMotionInfo pciMotionInfo = new PciMotionInfo();
            pciMotionInfo.Copy(this);

            return pciMotionInfo;
        }

        public override void Copy(MotionInfo srcMotionInfo)
        {
            base.Copy(srcMotionInfo);

            PciMotionInfo srcPciMotionInfo = (PciMotionInfo)srcMotionInfo;
            index = srcPciMotionInfo.index;
        }
    }

    public class SerialMotionInfo : MotionInfo
    {
        SerialPortInfo serialPortInfo = new SerialPortInfo();
        public SerialPortInfo SerialPortInfo
        {
            get { return serialPortInfo; }
            set { serialPortInfo = value; }
        }

        public override void LoadXml(XmlElement motionElement)
        {
            base.LoadXml(motionElement);

            serialPortInfo.Load(motionElement, "SerialPortInfo");
        }

        public override void SaveXml(XmlElement motionElement)
        {
            base.SaveXml(motionElement);

            serialPortInfo.Save(motionElement, "SerialPortInfo");
        }

        public override MotionInfo Clone()
        {
            SerialMotionInfo serialMotionInfo = new SerialMotionInfo();
            serialMotionInfo.Copy(this);

            return serialMotionInfo;
        }

        public override void Copy(MotionInfo srcMotionInfo)
        {
            base.Copy(srcMotionInfo);

            SerialMotionInfo serialMotionInfo = (SerialMotionInfo)srcMotionInfo;
            serialPortInfo = serialMotionInfo.SerialPortInfo.Clone();
        }
    }

    public class NetworkMotionInfo : MotionInfo
    {
        string ipAddress;
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        byte portNo;
        public byte PortNo
        {
            get { return portNo; }
            set { portNo = value; }
        }

        public override void LoadXml(XmlElement motionElement)
        {
            base.LoadXml(motionElement);

            ipAddress = XmlHelper.GetValue(motionElement, "IpAddress", "");
            portNo = Convert.ToByte(XmlHelper.GetValue(motionElement, "PortNo", "0"));
        }

        public override void SaveXml(XmlElement motionElement)
        {
            base.SaveXml(motionElement);

            XmlHelper.SetValue(motionElement, "IpAddress", ipAddress);
            XmlHelper.SetValue(motionElement, "PortNo", portNo.ToString());
        }

        public override MotionInfo Clone()
        {
            NetworkMotionInfo networkMotionInfo = new NetworkMotionInfo();
            networkMotionInfo.Copy(this);

            return networkMotionInfo;
        }

        public override void Copy(MotionInfo srcMotionInfo)
        {
            base.Copy(srcMotionInfo);

            NetworkMotionInfo networkMotionInfo = (NetworkMotionInfo)srcMotionInfo;
            ipAddress = networkMotionInfo.ipAddress;
            portNo = networkMotionInfo.portNo;
        }
    }

    public class MotionInfoFactory
    {
        public static MotionInfo CreateMotionInfo(MotionType motionType)
        {
            MotionInfo motionInfo = null;
            switch (motionType)
            {
                case MotionType.AlphaMotion302:
                case MotionType.AlphaMotion304:
                case MotionType.AlphaMotion314:
                case MotionType.AlphaMotionBx:
                case MotionType.AlphaMotionBBx:
                    motionInfo = new PciMotionInfo();
                    break;
                case MotionType.PowerPmac:
                    motionInfo = new NetworkMotionInfo();
                    break;
                case MotionType.FastechEziMotionPlusR:
                    motionInfo = new SerialMotionInfo();
                    break;
                default:
                    return null;
                case MotionType.Virtual:
                    motionInfo = new VirtualMotionInfo();
                    break;
                case MotionType.Ajin:
                    motionInfo = new AjinMotionInfo();
                    break;
            }

            motionInfo.Name = "";
            motionInfo.Type = motionType;

            return motionInfo;
        }
    }

    public class MotionInfoList : List<MotionInfo>
    {
        public MotionInfoList Clone()
        {
            MotionInfoList newMotionInfoList = new MotionInfoList();

            foreach (MotionInfo motionInfo in this)
            {
                newMotionInfoList.Add(motionInfo.Clone());
            }

            return newMotionInfoList;
        }

        internal MotionInfo GetMotionInfo(string masterDeviceName)
        {
            return this.Find(x => x.Name == masterDeviceName );
        }
    }

    public class MotionException : ApplicationException
    {
        string defaultMessage = "Motion Error";

        public MotionException()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public MotionException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public MotionException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public interface IMotion
    {
        bool CanSyncMotion();
        void TurnOnServo(int axisNo, bool bOnOff);
        float GetCommandPos(int axisNo);
        float GetActualPos(int axisNo);
        void SetPosition(int axisNo, float position);
        bool StartHomeMove(int axisNo, HomeParam homeSpeed);
        bool StartMove(int axisNo, float position, MovingParam movingParam);
        bool StartRelativeMove(int axisNo, float position, MovingParam movingParam);
        bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative);
        void StopMove(int axisNo);
        void EmergencyStop(int axisNo);
        bool IsMoveDone(int axisNo);
        bool IsAmpFault(int axisNo);
        bool IsHomeOn(int axisNo);
        bool IsPositiveOn(int axisNo);
        bool IsNegativeOn(int axisNo);
        bool IsEmgStop(int axisNo);

        bool IsVirtual { get; }

    }

    public class MotionList : List<Motion>
    {
        public void Initialize(MotionInfoList motionInfoList, bool isVirtual)
        {
            foreach (MotionInfo motionInfo in motionInfoList)
            {
                Motion motion = MotionFactory.Create(motionInfo, isVirtual);
                if (motion != null)
                {
                    Add(motion);
                }
            }
        }

        public void Release()
        {
            foreach (Motion motion in this)
            {
                DeviceManager.Instance().RemoveDevice(motion);
                bool isReady = motion.IsReady();
                if (isReady)
                {
                    motion.TurnOnServo(false);
                    motion.Release();
                }
            }
            this.Clear();
        }

        public Motion GetMotion(string name)
        {
            return Find(x => x.Name == name);
        }

        public Motion GetMotion(int index)
        {
            return this[index];
        }

        public void StopMove()
        {
            foreach (Motion motion in this)
            {
                motion.StopMove();
            }

            Thread.Sleep(1000);
        }

        public void EmergencyStop()
        {
            foreach (Motion motion in this)
            {
                motion.EmergencyStop();
            }

            Thread.Sleep(1000);
        }

        public void ResetAlarm()
        {
            foreach (Motion motion in this)
            {
                motion.ResetAlarm();
            }
        }

        public void TurnOnServo(bool bOnOff)
        {
            foreach (Motion motion in this)
            {
                motion.TurnOnServo(!bOnOff);
            }

            Thread.Sleep(500);
        }
    }

    public abstract class Motion : Device, IMotion
    {
        private MotionType motionType;
        public MotionType MotionType
        {
            get { return motionType; }
        }

        private int numAxis;
        public int NumAxis
        {
            get { return numAxis; }
            set { numAxis = value; }
        }

        public virtual bool IsVirtual { get => false; }

        public Motion(MotionType motionType, string name)
        {
            if (name == "")
                Name = motionType.ToString();
            else
                Name = name;

            DeviceType = DeviceType.MotionController;
            this.motionType = motionType;
            UpdateState(DeviceState.Idle, "Created");
        }

        public abstract bool Initialize(MotionInfo motionInfo);
        public abstract bool CanSyncMotion();

        public abstract void TurnOnServo(int axisNo, bool bOnOff);
   
        public abstract float GetCommandPos(int axisNo);
        public abstract float GetActualPos(int axisNo);
        public abstract float GetActualVel(int axisNo);

        public abstract void SetPosition(int axisNo, float position);

        public abstract bool ClearHomeDone(int axisNo);

        public abstract bool StartMove(int axisNo, float position, MovingParam movingParam);
        public abstract bool StartRelativeMove(int axisNo, float position, MovingParam movingParam);
        public abstract bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative);
        public abstract bool StartHomeMove(int axisNo, HomeParam homeSpeed);

        public abstract void StopMove(int axisNo);
        public abstract void EmergencyStop(int axisNo);
        
        public abstract bool StartMultiMove(int[] axisNos, float[] position, MovingParam movingParam);
        
        public abstract bool StartCmp(int axisNo, int startPos, float dist, bool plus);
        public abstract bool EndCmp(int axisNo);

        //public abstract bool SetCoordMove(int[] axisNos, float[] positions, MovingParam movingParam);
        //public abstract Task<bool> CoordMoveAbs(int[] axisNos, float[] positions);

        public abstract bool ModifyPos(int axisNo, float position);

        public bool IsServoOn(int axisNo)
        {
            return GetMotionStatus(axisNo).servoOn;
        }

        public bool IsMoveDone(int axisNo)
        {
            var status = GetMotionStatus(axisNo);
            return status.inp && (status.run == false);
        }

        public bool IsMoveOn(int axisNo)
        {
            return GetMotionStatus(axisNo).run;
        }

        public bool IsEmgStop(int axisNo)
        {
            return GetMotionStatus(axisNo).emg;
        }

        public bool IsHomeMoving(int axisNo)
        {
            return GetMotionStatus(axisNo).home;
        }
        public bool IsHomeDone(int axisNo)
        {
            return GetMotionStatus(axisNo).homeOk;
        }

        public bool IsAmpFault()
        {
            bool fault = false;
            for (int i = 0; i < numAxis; i++)
                fault |= IsAmpFault(i);
            return fault;
        }

        public bool IsAmpFault(int axisNo)
        {
            return GetMotionStatus(axisNo).alarm;
        }

        public bool IsHomeOn(int axisNo)
        {
            return GetMotionStatus(axisNo).origin;
        }

        public bool IsPositiveOn(int axisNo)
        {
            return GetMotionStatus(axisNo).posLimit;
        }

        public bool IsNegativeOn(int axisNo)
        {
            return GetMotionStatus(axisNo).negLimit;
        }

        public abstract MotionStatus GetMotionStatus(int axisNo);
        public abstract bool ResetAlarm(int axisNo);
        public abstract bool ResetAlarmOn(int axisNo, bool isOn);

        public void TurnOnServo(bool bOnOff)
        {
            for (int i = 0; i < NumAxis; i++)
                TurnOnServo(i, bOnOff);
        }

        public void StopMove()
        {
            for (int i = 0; i < NumAxis; i++)
                StopMove(i);
        }

        public void EmergencyStop()
        {
            for (int i = 0; i < NumAxis; i++)
            {
                if (IsEmgStop(i) == false)
                    EmergencyStop(i);
            }
        }

        public void Move(int axisNo, float position, MovingParam movingParam)
        {
            StartMove(axisNo, position, movingParam);
            while (IsMoveDone(axisNo) == false) ;
        }

        public void RelativeMove(int axisNo, float position, MovingParam movingParam)
        {
            StartRelativeMove(axisNo, position, movingParam);
            while (IsMoveDone(axisNo) == false) ;
        }

        public void HomeMove(int axisNo, HomeParam homeParam)
        {
            StartHomeMove(axisNo, homeParam);
            while (IsMoveDone(axisNo) == false)
            {
                Thread.Sleep(100);
            }

            Thread.Sleep(500);
        }

        public void ResetAlarm()
        {
            for(int i = 0; i < NumAxis; i++)
            {
                ResetAlarm(i);
            }
        }
    }
}
