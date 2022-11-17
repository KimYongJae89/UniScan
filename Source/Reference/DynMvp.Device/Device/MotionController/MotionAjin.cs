using System;
using DynMvp.Devices.Dio;
using System.Xml;
using DynMvp.Base;
using System.IO;
using System.Threading;
using static DynMvp.Devices.MotionController.AjinMotionInfo;
using System.Collections.Generic;
using DynMvp.Device.Device.Dio;
using DynMvp.Device.Device.MotionController;

namespace DynMvp.Devices.MotionController
{
    public class AjinMotionInfo : MotionInfo
    {
        //public enum TrigLevel { Low,High,Falling,Riging }
        //public enum TrigTime { None, AbsPeriod, OnlyAbs, Block, OneShot, TimerOneShot }

        string paramFile;
        public string ParamFile
        {
            get { return paramFile; }
            set { paramFile = value; }
        }

        public override void LoadXml(XmlElement motionElement)
        {
            base.LoadXml(motionElement);

            paramFile = XmlHelper.GetValue(motionElement, "ParamFile", "");

            if (File.Exists(paramFile))
            {
                this.NumAxis = 0;
                string[] lines = File.ReadAllLines(paramFile);
                foreach (string line in lines)
                {
                    string[] token = line.Split('=');
                    if (line.StartsWith("00:AXIS_NO."))
                    {
                        this.NumAxis++;
                        int axisNo = int.Parse(token[1]);
                    }
                }
            }
        }

        public override void SaveXml(XmlElement motionElement)
        {
            base.SaveXml(motionElement);

            XmlHelper.SetValue(motionElement, "ParamFile", paramFile);
        }

        public override MotionInfo Clone()
        {
            AjinMotionInfo ajinMotionInfo = new AjinMotionInfo();
            ajinMotionInfo.Copy(this);

            return ajinMotionInfo;
        }

        public override void Copy(MotionInfo srcMotionInfo)
        {
            base.Copy(srcMotionInfo);

            AjinMotionInfo ajinMotionInfo = (AjinMotionInfo)srcMotionInfo;
            paramFile = ajinMotionInfo.paramFile;
        }
    }

    public class MotionAjin : Motion, IDigitalIo
    {
        bool dioExist = false;

        uint numDigitalInput = 0;
        uint numDigitalOutput = 0;
        int numInPortGroup;
        int inPortStartGroupIndex;
        int numOutPortGroup;
        int outPortStartGroupIndex;

        object commandLock = new object();

        public new bool IsVirtual => false;

        public MotionAjin(string name)
            : base(MotionType.Ajin, name)
        {
        }

        public string GetName() { return Name; }
        public int GetNumInPort() { return (int)numDigitalInput; }
        public int GetNumOutPort() { return (int)numDigitalOutput; }

        public int GetNumInPortGroup() { return numInPortGroup; }
        public int GetNumOutPortGroup() { return numOutPortGroup; }
        public int GetInPortStartGroupIndex() { return inPortStartGroupIndex; }
        public int GetOutPortStartGroupIndex() { return outPortStartGroupIndex; }

        public override bool Initialize(MotionInfo motionInfo)
        {
            if (base.IsReady() == false)
            {
                AjinMotionInfo ajinMotionInfo = (AjinMotionInfo)motionInfo;

                try
                {
                    uint result = CAXL.AxlOpen(7);
                    if (result != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToInitialize, ErrorLevel.Error,
                            this.name, "Can't initilaize Ajin Library.", null);
                        throw new MotionException(((AXT_FUNC_RESULT)result).ToString());
                    }

                    if (InitMotion(ajinMotionInfo) == false)
                        return false;

                    UpdateState(DeviceState.Ready, "Device Loaded");
                }
                catch (Exception ex)
                {
                    string message = string.Format("MotionAjin::Initialize fail. {0}", ex.Message);
                    //throw new Exception(message);
                    return false;
                }
            }

            return true;
        }

        public bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            uint upStatus = 0;
            uint result = CAXD.AxdInfoIsDIOModule(ref upStatus);
            if (result == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                dioExist = (upStatus == (uint)AXT_EXISTENCE.STATUS_EXIST);
            }
            else
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToInitialize, ErrorLevel.Fatal,
                    this.name, "Fail to find digital I/O.", null);
                return false;
            }

            numDigitalInput = (uint)digitalIoInfo.NumInPort;
            numDigitalOutput = (uint)digitalIoInfo.NumOutPort;

            numInPortGroup = digitalIoInfo.NumInPortGroup;
            inPortStartGroupIndex = digitalIoInfo.InPortStartGroupIndex;
            numOutPortGroup = digitalIoInfo.NumOutPortGroup;
            outPortStartGroupIndex = digitalIoInfo.OutPortStartGroupIndex;

            return true;
        }

        bool InitMotion(AjinMotionInfo ajinMotionInfo)
        {
            uint upStatus = 0;
            uint result = CAXM.AxmInfoIsMotionModule(ref upStatus);
            if (result == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                if (upStatus != (uint)AXT_EXISTENCE.STATUS_EXIST)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToInitialize, ErrorLevel.Error,
                        this.name, "Can't find motion module.", null);
                    return false;
                }
            }
            else
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToInitialize, ErrorLevel.Error,
                    this.name, "Fail to find motion module.", null);// ((AXT_FUNC_RESULT)result).ToString());
                return false;
            }

            if (File.Exists(ajinMotionInfo.ParamFile) == false)
            {
                throw new MotionException("Can't found motion param file.");
            }

            result = CAXM.AxmMotLoadParaAll(ajinMotionInfo.ParamFile);
            if (result != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToReadParamFile, ErrorLevel.Error,
                    this.name, "Fail to Load Mot File.", null);// ((AXT_FUNC_RESULT)result).ToString());
                return false;
            }


            int numAxis = 0;
            result = CAXM.AxmInfoGetAxisCount(ref numAxis);
            if (result != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToReadParam, ErrorLevel.Error,
                    this.name, "Fail to Read Number of Axis .", null);// ((AXT_FUNC_RESULT)result).ToString());
                return false;
            }

            NumAxis = numAxis;

            for (int i = 0; i < numAxis; i++)
                CAXM.AxmMotSetAccelUnit(i, 1);

            return true;
        }

        void IDigitalIo.UpdateState(DeviceState state, String stateMessage)
        {
            State = state;
            StateMessage = stateMessage;
        }

        public override void Release()
        {
            base.Release();

            for (int i = 0; i < NumAxis; i++)
                TurnOnServo(i, false);

            bool isOpened = CAXL.AxlIsOpened() == 1;
            if (isOpened)
            {
                if (CAXL.AxlClose() == 0)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToRelease, ErrorLevel.Error,
                        this.name, "Fail to release Ajin library.", null);
                    return;
                }
            }

            UpdateState(DeviceState.Idle, "Device unloaded");
        }

        public override bool CanSyncMotion()
        {
            return false;
        }

        public override void TurnOnServo(int axisNo, bool bOnOff)
        {
            lock (commandLock)
            {
                CAXM.AxmSignalServoOn(axisNo, Convert.ToUInt32(bOnOff));
            }
        }

        public override float GetCommandPos(int axisNo)
        {
            double dCmdPos = 0.0;
            lock (commandLock)
            {
                CAXM.AxmStatusGetCmdPos(axisNo, ref dCmdPos);
            }

            return Convert.ToSingle(dCmdPos);
        }

        public override float GetActualPos(int axisNo)
        {
            double dCmdPos = 0.0;
            lock (commandLock)
            {
                CAXM.AxmStatusGetActPos(axisNo, ref dCmdPos);
            }

            return Convert.ToSingle(dCmdPos);
        }

        public override float GetActualVel(int axisNo)
        {
            double dCmdVel = 0.0;
            lock (commandLock)
            {
                CAXM.AxmStatusReadVel(axisNo, ref dCmdVel);
            }

            return Convert.ToSingle(dCmdVel);
        }

        public override void SetPosition(int axisNo, float position)
        {
            lock (commandLock)
            {
                CAXM.AxmStatusSetPosMatch(axisNo, position);
            }
        }

        int ConvertDirection(MoveDirection direction)
        {
            return Convert.ToInt32(direction);
        }

        uint ConvertHomeSignal(HomeMode homeMode)
        {
            switch (homeMode)
            {
                case HomeMode.HomeSensor:
                    return (uint)AXT_MOTION_HOME_DETECT.HomeSensor;
                case HomeMode.NegEndLimit:
                    return (uint)AXT_MOTION_HOME_DETECT.NegEndLimit;
                case HomeMode.PosEndLimit:
                    return (uint)AXT_MOTION_HOME_DETECT.PosEndLimit;
            }

            return (uint)AXT_MOTION_HOME_DETECT.HomeSensor;
        }

        public override bool StartHomeMove(int axisNo, HomeParam homeParam)
        {
            LogHelper.Debug(LoggerType.Machine, String.Format("HomeMove : Axis Id {0}", axisNo));

            uint duRetCode;
            lock (commandLock)
            {
                duRetCode = CAXM.AxmHomeSetMethod(axisNo, ConvertDirection(homeParam.HomeDirection),
                    ConvertHomeSignal(homeParam.HomeMode), 0, 1000, 0);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToWriteParam, ErrorLevel.Error,
                        this.name, "Axis Id {0} / Start Home Move Set HomeMethod Error - Code : {1}", new object[] { axisNo, duRetCode });
                    return false;
                }

                duRetCode = CAXM.AxmHomeSetVel(axisNo, homeParam.HighSpeed.MaxVelocity, homeParam.HighSpeed.MaxVelocity / 2, homeParam.MediumSpeed.MaxVelocity,
                            homeParam.FineSpeed.MaxVelocity, homeParam.HighSpeed.AccelerationTimeMs / 1000, homeParam.HighSpeed.DecelerationTimeMs / 1000);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToWriteParam, ErrorLevel.Error,
                        this.name, "Axis Id {0} / Start Home Move Set Velocity Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return false;
                }

                duRetCode = CAXM.AxmHomeSetStart(axisNo);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.Homing, ErrorLevel.Error,
                        this.name, "Axis Id {0} / Start Home Move Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return false;
                }
            }

            return true;
        }

        bool SetProfileMode(int axisNo)
        {
            uint velocityProfile = 0;
            if (AxisHandler.MovingProfileType == MovingProfileType.TCurve)
                velocityProfile = 0;
            else
                velocityProfile = 3;

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmMotSetProfileMode(axisNo, velocityProfile);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToWriteParam, ErrorLevel.Error,
                        this.name, "Axis Id {0} / Set Velocity Profile Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return false;
                }
            }

            return true;
        }

        public override bool StartMove(int axisNo, float position, MovingParam movingParam)
        {
            LogHelper.Debug(LoggerType.Machine, String.Format("StartMove : Axis Id {0} / Position {1} ", axisNo, position));

            if (IsMoveDone(axisNo) == false)
            {
                LogHelper.Debug(LoggerType.Machine, "StartMove : Moving Overlapped");
                return false;
            }

            SetProfileMode(axisNo);

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmMotSetAbsRelMode(axisNo, 0);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToWriteParam, ErrorLevel.Error,
                        this.name, "Axis Id {0} / StartMove Set ABS/Rel Mode Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return false;
                }

                duRetCode = CAXM.AxmMoveStartPos(axisNo, position, movingParam.MaxVelocity, movingParam.AccelerationTimeMs / 1000, movingParam.DecelerationTimeMs / 1000);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.Moving, ErrorLevel.Error,
                        this.name, "Axis Id {0} / StartMove Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return false;
                }
            }

            return true;
        }

        public override bool StartRelativeMove(int axisNo, float position, MovingParam movingParam)
        {
            LogHelper.Debug(LoggerType.Machine, String.Format("StartRelativeMove : Axis Id {0} / Position {1} ", axisNo, position));

            SetProfileMode(axisNo);

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmMotSetAbsRelMode(axisNo, 1);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToWriteParam, ErrorLevel.Error,
                        this.name, "Axis Id {0} /StartRelativeMove Set ABS/Rel Mode Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return false;
                }

                duRetCode = CAXM.AxmMoveStartPos(axisNo, position, movingParam.MaxVelocity, movingParam.AccelerationTimeMs / 1000, movingParam.DecelerationTimeMs / 1000);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.Moving, ErrorLevel.Error,
                        this.name, "Axis Id {0} / StartRelativeMove Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return false;
                }
            }

            return true;
        }

        public override bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative)
        {
            LogHelper.Debug(LoggerType.Machine, String.Format("ContinuousMove : Axis Id {0}", axisNo));

            SetProfileMode(axisNo);

            lock (commandLock)
            {
                double velocity = movingParam.MaxVelocity;
                if (negative == true)
                    velocity *= -1;

                uint duRetCode = CAXM.AxmMoveVel(axisNo, velocity, movingParam.AccelerationTimeMs / 1000, movingParam.DecelerationTimeMs / 1000);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.ContinuousMoving, ErrorLevel.Error,
                        this.name, "Axis Id {0} / ContinuousMove Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return false;
                }
            }

            return true;
        }

        public override void StopMove(int axisNo)
        {
            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmMoveSStop(axisNo);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.StopMove, ErrorLevel.Error,
                        this.name, "Axis Id {0} / Stop Move Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                }
            }
        }

        public override void EmergencyStop(int axisNo)
        {
            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmMoveEStop(axisNo);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.EmergencyStop, ErrorLevel.Error,
                        this.name, "Axis Id {0} / Emergency Stop Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                }
            }
        }

        public override bool ClearHomeDone(int axisNo)
        {
            throw new NotImplementedException();
        }

        //public override bool IsServoOn(int axisNo)
        //{
        //    uint upStatus = 0;

        //    lock (commandLock)
        //    {
        //        uint duRetCode = CAXM.AxmSignalIsServoOn(axisNo, ref upStatus);
        //        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
        //        {
        //            ErrorManager.Instance().Report(ErrorSections.Motion, (int)CommonError.FailToReadParam,
        //                ErrorLevel.Error, ErrorSections.Motion.ToString(), CommonError.FailToReadParam.ToString(), String.Format("Axis Id {0} / Read Servo On Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
        //        }
        //    }

        //    return upStatus == 1;
        //}

        //public override bool IsPositiveOn(int axisNo)
        //{
        //    uint upStopMode = 0;
        //    uint upPositiveStatus = 0;
        //    uint upNegativeStatus = 0;

        //    uint duRetCode = 0;
        //    lock (commandLock)
        //    {
        //        duRetCode = CAXM.AxmSignalGetLimit(axisNo, ref upStopMode, ref upPositiveStatus, ref upNegativeStatus);
        //        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
        //        {
        //            ErrorManager.Instance().Report(ErrorSections.Motion, (int)CommonError.FailToReadParam,
        //                ErrorLevel.Error, ErrorSections.Motion.ToString(), CommonError.FailToReadParam.ToString(), String.Format("Axis Id {0} / Get Positive Limit Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
        //            return false;
        //        }
        //    }

        //    if (upPositiveStatus == 0x2)
        //        return false;

        //    lock (commandLock)
        //    {
        //        duRetCode = CAXM.AxmSignalReadLimit(axisNo, ref upPositiveStatus, ref upNegativeStatus);
        //        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
        //        {
        //            ErrorManager.Instance().Report(ErrorSections.Motion, (int)CommonError.FailToReadParam,
        //                ErrorLevel.Error, ErrorSections.Motion.ToString(), CommonError.FailToReadParam.ToString(), String.Format("Axis Id {0} / Read Positive Limit Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
        //            return false;
        //        }
        //    }

        //    return upPositiveStatus != 0;
        //}

        //public override bool IsNegativeOn(int axisNo)
        //{
        //    uint upStopMode = 0;
        //    uint upPositiveStatus = 0;
        //    uint upNegativeStatus = 0;

        //    uint duRetCode;
        //    lock (commandLock)
        //    {
        //        duRetCode = CAXM.AxmSignalGetLimit(axisNo, ref upStopMode, ref upPositiveStatus, ref upNegativeStatus);
        //        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
        //        {
        //            ErrorManager.Instance().Report(ErrorSections.Motion, (int)CommonError.FailToReadParam,
        //                ErrorLevel.Error, ErrorSections.Motion.ToString(), CommonError.FailToReadParam.ToString(), String.Format("Axis Id {0} / Get Negative Limit Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
        //            return false;
        //        }
        //    }

        //    if (upNegativeStatus == 0x2)
        //        return false;

        //    lock (commandLock)
        //    {
        //        duRetCode = CAXM.AxmSignalReadLimit(axisNo, ref upPositiveStatus, ref upNegativeStatus);
        //        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
        //        {
        //            ErrorManager.Instance().Report(ErrorSections.Motion, (int)CommonError.FailToReadParam,
        //                ErrorLevel.Error, ErrorSections.Motion.ToString(), CommonError.FailToReadParam.ToString(), String.Format("Axis Id {0} / Read Negative Limit Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
        //            return false;
        //        }
        //    }

        //    return upNegativeStatus != 0;
        //}

        public override MotionStatus GetMotionStatus(int axisNo)
        {
            MotionStatus motionStatus = new MotionStatus();

            uint status = 0;

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmStatusReadMechanical(axisNo, ref status);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToReadParam, ErrorLevel.Error,
                        this.name, "Axis Id {0} / Read Mechenical State Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return motionStatus;
                }
            }


            /*
             * - [0001h] Bit 0, +Limit 급정지 신호 현재 상태
             * - [0002h] Bit 1, -Limit 급정지 신호 현재 상태
             * - [0004h] Bit 2, +limit 감속 정지 현재 상태
             * - [0008h] Bit 3, -limit 감속 정지 현재 상태
             * - [0010h] Bit 4, Alarm 신호 신호 현재 상태
             * - [0020h] Bit 5, InPos 신호 현재 상태
             * - [0040h] Bit 6, (ESTOP) 현재 상태
             * - [0080h] Bit 7, 원점 신호 현재 상태
             * - [0100h] Bit 8, Z 상 입력 신호 현재 상태
             * - [0200h] Bit 9, ECUP 터미널 신호 상태
             * - [0400h] Bit 10, ECDN 터미널 터미널 신호 상태
             */

            IsLimitOn(axisNo, out motionStatus.posLimit, out motionStatus.negLimit);
            motionStatus.alarm = (status & 0x0010) > 0;
            IsMoveDone(axisNo, out motionStatus.inp);
            motionStatus.emg = (status & 0x0040) > 0;
            motionStatus.origin = (status & 0x0080) > 0;
            motionStatus.run = !motionStatus.inp;
            IsServoOn(axisNo, out motionStatus.servoOn);

            return motionStatus;
        }

        private void IsMoveDone(int axisNo, out bool isMoveDone)
        {
            isMoveDone = false;

            uint upStatus = 0;
            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmStatusReadInMotion(axisNo, ref upStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToReadParam, ErrorLevel.Error,
                        this.name, "Axis Id {0} / Read Motion Done Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return;
                }
            }

            isMoveDone = (upStatus == 0);
        }

        private void IsServoOn(int axisNo, out bool servoOn)
        {
            servoOn = false;

            uint upStatus = 0;
            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmSignalIsServoOn(axisNo, ref upStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToReadParam, ErrorLevel.Error,
                        this.name, "Axis Id {0} / Get Negative Limit Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return;
                }
            }
            servoOn = (upStatus > 0);
        }

        private void IsLimitOn(int axisNo, out bool posLimitOn, out bool negLimitOn)
        {
            posLimitOn = negLimitOn = false;

            uint upStopMode = 0;
            uint upPositiveStatus = 0;
            uint upNegativeStatus = 0;

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmSignalGetLimit(axisNo, ref upStopMode, ref upPositiveStatus, ref upNegativeStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToReadParam, ErrorLevel.Error,
                        this.name, "Axis Id {0} / Get Negative Limit Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return;
                }
            }

            if (upPositiveStatus == 0x2 && upNegativeStatus == 0x2)
            {
                return;
            }

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmSignalReadLimit(axisNo, ref upPositiveStatus, ref upNegativeStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToReadParam, ErrorLevel.Error,
                        this.name, "Axis Id {0} / Read Negative Limit Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    return;
                }

                posLimitOn = (upNegativeStatus != 0);
                negLimitOn = (upNegativeStatus != 0);
            }
        }

        public override bool ResetAlarm(int axisNo)
        {
            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmSignalServoAlarmReset(axisNo, (uint)1);
                uint dwOutBit = 0;
                CAXM.AxmSignalReadOutputBit(axisNo, 1, ref dwOutBit);
                if (dwOutBit != 0)
                    CAXM.AxmSignalWriteOutputBit(axisNo, 1, (uint)AXT_BOOLEAN.FALSE);
                else
                    CAXM.AxmSignalWriteOutputBit(axisNo, 1, (uint)AXT_BOOLEAN.TRUE);
                Thread.Sleep(500);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToReadParam, ErrorLevel.Error,
                        this.name, "Axis Id {0} / Alarm Reset Error - Code : {1}", new object[] { axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    CAXM.AxmSignalWriteOutputBit(axisNo, 1, (uint)AXT_BOOLEAN.FALSE);
                    return false;
                }
            }
            CAXM.AxmSignalWriteOutputBit(axisNo, 1, (uint)AXT_BOOLEAN.FALSE);
            return true;
        }


        public override bool ResetAlarmOn(int axisNo, bool isOn)
        {
            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmSignalServoAlarmReset(axisNo, (uint)1);
                uint dwOutBit = 0;
                CAXM.AxmSignalReadOutputBit(axisNo, 1, ref dwOutBit);
                if (dwOutBit != 0)
                    CAXM.AxmSignalWriteOutputBit(axisNo, 1, (uint)AXT_BOOLEAN.FALSE);
                else
                    CAXM.AxmSignalWriteOutputBit(axisNo, 1, (uint)AXT_BOOLEAN.TRUE);
                Thread.Sleep(500);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToReadParam, ErrorLevel.Error,
                        this.name, string.Format("Alarm Reset Error - Axis {0}, Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()), null, "");
                    CAXM.AxmSignalWriteOutputBit(axisNo, 1, (uint)AXT_BOOLEAN.FALSE);
                    return false;
                }
            }
            CAXM.AxmSignalWriteOutputBit(axisNo, 1, (uint)AXT_BOOLEAN.FALSE);
            return true;
        }

        public void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            if (dioExist == true)
            {
                lock (commandLock)
                {
                    uint duRetCode = CAXD.AxdoWriteOutport(groupNo * 32 + portNo, Convert.ToUInt32(value));
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.FailToWriteValue, ErrorLevel.Error,
                            this.name, "Write Outport Error - Code : {0}", new string[] { ((AXT_FUNC_RESULT)duRetCode).ToString() }, "");
                        return;
                    }

                    Thread.Sleep(5);
                }
            }
        }

        public void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            if (dioExist == true)
            {
                lock (commandLock)
                {
                    uint duRetCode = CAXD.AxdoWriteOutportDword(groupNo + outPortStartGroupIndex, 0, outputPortStatus);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.FailToWriteValue, ErrorLevel.Error,
                            this.name, "Write Outport Error - Code : {0}", new string[] { ((AXT_FUNC_RESULT)duRetCode).ToString() }, "");
                        return;
                    }
                }
            }
        }

        public uint ReadOutputGroup(int groupNo)
        {
            uint value = 0;

            if (dioExist == true)
            {
                lock (commandLock)
                {
                    uint duRetCode = CAXD.AxdoReadOutportDword(groupNo + outPortStartGroupIndex, 0, ref value);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.FailToReadValue, ErrorLevel.Error,
                            this.name, "Read Outport Error - Code : {0}", new object[] { ((AXT_FUNC_RESULT)duRetCode).ToString() }, "");
                    }
                }
            }

            return value;
        }

        public uint ReadInputGroup(int groupNo)
        {
            uint value = 0;

            if (dioExist == true)
            {
                lock (commandLock)
                {
                    uint duRetCode = CAXD.AxdiReadInportDword(groupNo + inPortStartGroupIndex, 0, ref value);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.FailToReadValue, ErrorLevel.Error,
                            this.name, "Read Inport Error - Code : {0}", new object[] { ((AXT_FUNC_RESULT)duRetCode).ToString() });
                    }
                }
            }

            return value;
        }

        public void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            //do nothing
        }

        public bool ResetTrigger(int axisNo)
        {
            return CAXM.AxmTriggerSetReset(0) == 0;
        }

        //public bool SetTriggerLevel(int axisNo, TriggerType triggerType, double activeTimeUs, bool actualPos)
        //{
        //    return CAXM.AxmTriggerSetTimeLevel(axisNo, activeTimeUs, (uint)(triggerType == TriggerType.FallingEdge ? 0 : 1), (uint)(actualPos ? 1 : 0), 0) == 0;
        //}

        //public bool SetTriggerPosition(int axisNo, double position, bool isAbsolute = false)
        //{
        //    return CAXM.AxmTriggerSetAbsPeriod(axisNo, (uint)(isAbsolute ? 1 : 0), position) == 0;
        //}

        //public bool SetTriggerPosition(int axisNo, List<double> absPositions)
        //{
        //    return CAXM.AxmTriggerOnlyAbs(axisNo, absPositions.Count, absPositions.ToArray()) == 0;
        //}

        public override bool StartMultiMove(int[] axisNos, float[] position, MovingParam movingParam)
        {
            throw new NotImplementedException();
        }

        public override bool StartCmp(int axisNo, int startPos, float dist, bool plus)
        {
            CAXM.AxmTriggerSetTimeLevel(axisNo, 10, 1, 1, 0);
            return CAXM.AxmTriggerSetAbsPeriod(axisNo, 0, dist) == 0;
        }

        public override bool EndCmp(int axisNo)
        {
            return CAXM.AxmTriggerSetReset(axisNo) == 0;
        }

        public override bool ModifyPos(int axisNo, float position)
        {
            throw new NotImplementedException();
        }
    }
}
