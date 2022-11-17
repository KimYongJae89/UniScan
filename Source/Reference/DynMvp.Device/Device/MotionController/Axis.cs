using System;
using System.Threading;
using System.Xml;

using DynMvp.Base;
using System.Windows.Forms;
using System.ComponentModel;
using DynMvp.Device.Device.MotionController;

namespace DynMvp.Devices.MotionController
{
    public enum AxisName
    {
        X, Y, Z, R
    }

    public enum Direction
    {
        Positive = 1, Negative = -1
    }

    public class AxisParam
    {
        bool useServo = true;
        [Category("misc"), Description("")]
        public bool UseServo
        {
            get { return useServo; }
            set { useServo = value; }
        }

        private int movingDoneWaitTimeMs = 30000;
        [Category("Timeout"), Description("Moving Timeout [ms]")]
        public int MovingDoneWaitTimeMs
        {
            get { return movingDoneWaitTimeMs; }
            set { movingDoneWaitTimeMs = value; }
        }

        private int homingDoneWaitTimeMs = 40000;
        [Category("Timeout"), Description("Home Search Timeout [ms]")]
        public int HomingDoneWaitTimeMs
        {
            get { return homingDoneWaitTimeMs; }
            set { homingDoneWaitTimeMs = value; }
        }

        float positiveLimit;
        [Category("Limit"), Description("Positive S/W Limit [pls]")]
        public float PositiveLimit
        {
            get { return positiveLimit; }
            set { positiveLimit = value; }
        }

        float negativeLimit;
        [Category("Limit"), Description("Negative S/W Limit [pls]")]
        public float NegativeLimit
        {
            get { return negativeLimit; }
            set { negativeLimit = value; }
        }

        HomeParam homeSpeed = new HomeParam();
        [Category("Speed"), Description("Home Search Speed")]
        public HomeParam HomeSpeed
        {
            get { return homeSpeed; }
            set { homeSpeed = value; }
        }

        MovingParam movingParam = new MovingParam();
        [Category("Speed"), Description("Moving Speed")]
        public MovingParam MovingParam
        {
            get { return movingParam; }
            set { movingParam = value; }
        }

        MovingParam jogParam = new MovingParam();
        [Category("Speed"), Description("Jog Speed")]
        public MovingParam JogParam
        {
            get { return jogParam; }
            set { jogParam = value; }
        }

        float originPulse = 0;
        [Category("misc"), Description("Origin Offset Pos [pls]")]
        public float OriginPulse
        {
            get { return originPulse; }
            set { originPulse = value; }
        }

        double micronPerPulse = 1;
        [Category("misc"), Description("Robot Resolution [um/pls]")]
        public double MicronPerPulse
        {
            get { return micronPerPulse; }
            set { micronPerPulse = value; }
        }

        bool inverse = false;
        [Category("misc"), Description("")]
        public bool Inverse
        {
            get { return inverse; }
            set { inverse = value; }
        }

        public bool IsValidPosition(float position)
        {
            return position > negativeLimit && position < positiveLimit;
        }

        public void Load(XmlElement axisParamElement)
        {
            if (axisParamElement == null)
                return;

            useServo = Convert.ToBoolean(XmlHelper.GetValue(axisParamElement, "UseServo", useServo.ToString()));
            positiveLimit = Convert.ToSingle(XmlHelper.GetValue(axisParamElement, "PositiveLimit", positiveLimit.ToString()));
            negativeLimit = Convert.ToSingle(XmlHelper.GetValue(axisParamElement, "NegativeLimit", negativeLimit.ToString()));
            movingDoneWaitTimeMs = Convert.ToInt32(XmlHelper.GetValue(axisParamElement, "MovingDoneWaitTime", movingDoneWaitTimeMs.ToString()));
            homingDoneWaitTimeMs = Convert.ToInt32(XmlHelper.GetValue(axisParamElement, "HomingDoneWaitTime", homingDoneWaitTimeMs.ToString()));

            inverse = Convert.ToBoolean(XmlHelper.GetValue(axisParamElement, "Inverse", inverse.ToString()));
            micronPerPulse = Convert.ToDouble(XmlHelper.GetValue(axisParamElement, "MicronPerPulse", micronPerPulse.ToString()));
            originPulse = Convert.ToSingle(XmlHelper.GetValue(axisParamElement, "OriginPulse", originPulse.ToString()));

            XmlElement homeSpeedElement = axisParamElement["HomeSpeed"];
            if (homeSpeedElement != null)
                homeSpeed.Load(homeSpeedElement);

            XmlElement movingParamElement = axisParamElement["MovingParam"];
            if (movingParamElement != null)
                movingParam.Load(movingParamElement);

            XmlElement jogParamElement = axisParamElement["JogParam"];
            if (jogParamElement != null)
                jogParam.Load(jogParamElement);
        }

        public void Save(XmlElement axisParamElement)
        {
            if (axisParamElement == null)
                return;

            XmlHelper.SetValue(axisParamElement, "UseServo", useServo.ToString());
            XmlHelper.SetValue(axisParamElement, "PositiveLimit", positiveLimit.ToString());
            XmlHelper.SetValue(axisParamElement, "NegativeLimit", negativeLimit.ToString());
            XmlHelper.SetValue(axisParamElement, "MovingDoneWaitTime", movingDoneWaitTimeMs.ToString());
            XmlHelper.SetValue(axisParamElement, "HomingDoneWaitTime", homingDoneWaitTimeMs.ToString());

            XmlHelper.SetValue(axisParamElement, "Inverse", inverse.ToString());
            XmlHelper.SetValue(axisParamElement, "MicronPerPulse", micronPerPulse.ToString());
            XmlHelper.SetValue(axisParamElement, "OriginPulse", originPulse.ToString());

            XmlElement homeSpeedElement = axisParamElement.OwnerDocument.CreateElement("", "HomeSpeed", "");
            axisParamElement.AppendChild(homeSpeedElement);
            homeSpeed.Save(homeSpeedElement);

            XmlElement movingParamElement = axisParamElement.OwnerDocument.CreateElement("", "MovingParam", "");
            axisParamElement.AppendChild(movingParamElement);
            movingParam.Save(movingParamElement);

            XmlElement jogParamElement = axisParamElement.OwnerDocument.CreateElement("", "JogParam", "");
            axisParamElement.AppendChild(jogParamElement);
            jogParam.Save(jogParamElement);
        }
    }

    public class Axis
    {
        private string name;
        public string Name
        {
            get { return name; }
        }

        private Motion motion;
        public Motion Motion
        {
            get { return motion; }
        }

        private int axisNo;
        public int AxisNo
        {
            get { return axisNo; }
        }

        private bool isHomeNeed;
        public bool IsHomeNeed
        {
            get { return isHomeNeed; }
        }

        private AxisParam axisParam = new AxisParam();
        public AxisParam AxisParam
        {
            get { return axisParam; }
        }

        TimeOutTimer timeOutTimer = new TimeOutTimer();

        int homeOrder = 0;
        public int HomeOrder
        {
            get { return homeOrder; }
            set { homeOrder = value; }
        }

        public Axis(string name, Motion motion, int axisNo, bool isHomeNeed)
        {
            Update(name, motion, axisNo, isHomeNeed);
        }

        public void Update(string name, Motion motion, int axisNo)
        {
            Update(name, motion, axisNo, this.isHomeNeed);
        }

        public void Update(string name, Motion motion, int axisNo, bool isHomeNeed)
        {
            this.name = name;
            this.motion = motion;
            this.axisNo = axisNo;
            this.isHomeNeed = isHomeNeed;
        }

        public override string ToString()
        {
            return name;
        }

        private bool IsInWorkingRegion(double pulse)
        {
            // 값이 설정되어 있지 않으면 두 값 모두 '0'
            double posLim = GetPositiveLimitPos();
            double negLim = GetNegativeLimitPos();
            if (negLim == posLim)
                return true;

            if (posLim < negLim)
                return false;

            if (pulse < negLim)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.NegLimit, ErrorLevel.Warning,
                    this.name, "Axis No = {0}", new object[] { axisNo.ToString() });
                return false;
            }

            if (posLim < pulse)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.PosLimit, ErrorLevel.Warning,
                    this.name, "Axis No = {0}", new object[] { axisNo.ToString() });
                return false;
            }

            //float curPulse = GetActualPulse();
            //if (motion.IsPositiveOn(axisNo) && curPulse < pulse)
            //{
            //    ErrorManager.Instance().Report(ErrorSections.Motion, ErrorSubSectionMotion.PosLimit, ErrorLevel.Warning, String.Format("Axis No = {0}", axisNo.ToString()));
            //    return false;
            //}

            //if (motion.IsNegativeOn(axisNo) && curPulse > pulse)
            //{
            //    ErrorManager.Instance().Report(ErrorSections.Motion, ErrorSubSectionMotion.PosLimit, ErrorLevel.Warning, String.Format("Axis No = {0}", axisNo.ToString()));
            //    return false;
            //}

            return true;
        }

        public bool CheckValidState()
        {
            if (!ErrorManager.Instance().IsCleared())
                return false;

            if (this.axisParam.UseServo == false)
                return false;

            MotionStatus motionStatus = motion.GetMotionStatus(axisNo);
            //if (this.isHomeNeed && (motionStatus.homeOk == false) && (motionStatus.home == false))
            //{
            //    ErrorManager.Instance().Report(ErrorSections.Motion, ErrorSubSectionMotion.HomeFound, ErrorLevel.Error,
            //            ErrorSections.Motion.ToString(), ErrorSubSectionMotion.HomeFound.ToString(), String.Format("Axis No = {0}", axisNo.ToString()));
            //    return false;
            //}

            if (motionStatus.alarm == true)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.AmpFault, ErrorLevel.Error,
                    this.name, "Axis No = {0}", new object[] { axisNo.ToString() });
                return false;
            }

            if (motionStatus.servoOn == false)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.ServoOff, ErrorLevel.Error,
                    this.name, "Axis No = {0}", new object[] { axisNo.ToString() });
                return false;
            }

            //if (motion.IsPositiveOn(axisNo) == true)
            //{
            //    ErrorManager.Instance().Report(ErrorSections.Motion, ErrorSubSectionMotion.PosLimit, ErrorLevel.Error,
            //        ErrorSections.Motion.ToString(), ErrorSubSectionMotion.PosLimit.ToString(), String.Format("Axis No = {0}", axisNo.ToString()));
            //    return false;
            //}

            //if (motion.IsNegativeOn(axisNo) == true)
            //{
            //    ErrorManager.Instance().Report(ErrorSections.Motion, ErrorSubSectionMotion.PosLimit, ErrorLevel.Error,
            //        ErrorSections.Motion.ToString(), ErrorSubSectionMotion.NegLimit.ToString(), String.Format("Axis No = {0}", axisNo.ToString()));
            //    return false;
            //}

            //if (IsMovingTimeOut() == true)
            //{
            //    if (onHomeMoving == true)
            //        ErrorManager.Instance().Report(ErrorSections.Motion, ErrorSubSectionMotion.HomingTimeOut, ErrorLevel.Warning, 
            //            ErrorSections.Motion.ToString(), ErrorSubSectionMotion.HomingTimeOut.ToString(), String.Format("Axis No = {0}", axisNo.ToString()));
            //    else
            //        ErrorManager.Instance().Report(ErrorSections.Motion, ErrorSubSectionMotion.MovingTimeOut, ErrorLevel.Warning,
            //            ErrorSections.Motion.ToString(), ErrorSubSectionMotion.MovingTimeOut.ToString(), String.Format("Axis No = {0}", axisNo.ToString()));

            //    ResetState();

            //    return false;
            //}

            return true;
        }

        public bool StartMove(float position, MovingParam movingParam = null, bool rawMove = false)
        {
            if (CheckValidState() == false)
                return false;

            float pulse;
            if (rawMove == false)
            {
                pulse = ToPulse(position);
            }
            else
            {
                pulse = position;
            }

            if (!IsInWorkingRegion(pulse))
                return false;

            LogHelper.Debug(LoggerType.Machine, String.Format("StartMove : Axis Id {0} / Position {1} / Pulse {2} ", axisNo, position, pulse));

            bool result;
            if (movingParam != null)
                result = motion.StartMove(axisNo, pulse, movingParam);
            else
                result = motion.StartMove(axisNo, pulse, axisParam.MovingParam);

            if (result == true)
            {
                timeOutTimer.Start(axisParam.MovingDoneWaitTimeMs);
            }

            return result;
        }

        public bool Move(float position, MovingParam movingParam = null, bool rawMove = false)
        {
            LogHelper.Debug(LoggerType.Machine, String.Format("Move : Axis Id {0} / Position {1} ", axisNo, position));

            if (StartMove(position, movingParam, rawMove) == false)
                return false;

            return WaitMoveDone();
        }

        public bool StartRelativeMove(float offset, MovingParam movingParam = null)
        {
            if (CheckValidState() == false)
                return false;

            float position = GetActualPos() + offset;

            if (!IsInWorkingRegion(ToPulse(position)))
                return false;

            float offsetPulse = ToOffsetPulse(offset);

            LogHelper.Debug(LoggerType.Machine, String.Format("StartRelativeMove : Axis Id {0} / Offset {1} / Offset Pulse {2} ", axisNo, offset, offsetPulse));

            bool result;
            if (movingParam != null)
                result = motion.StartRelativeMove(axisNo, offsetPulse, movingParam);
            else
                result = motion.StartRelativeMove(axisNo, offsetPulse, axisParam.MovingParam);

            if (result == true)
            {
                timeOutTimer.Start(axisParam.MovingDoneWaitTimeMs);
            }

            return result;
        }

        public bool RelativeMove(float offset, MovingParam movingParam = null)
        {
            LogHelper.Debug(LoggerType.Machine, String.Format("RelativeMove : Axis Id {0} / Offset {1} ", axisNo, offset));

            if (StartRelativeMove(offset, movingParam) == false)
                return false;

            return WaitMoveDone();
        }

        public bool EjectHome(int axisNo)
        {
            if (motion.MotionType == MotionType.Virtual)
                return true;

            if (IsMoveDone() == false)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.HomingTimeOut, ErrorLevel.Warning,
                    this.name, "Eject Home : Axis No = {0}", new object[] { axisNo.ToString() });
                return false;
            }

            if (CheckValidState() == false)
                return false;

            if (ContinuousMove(axisParam.HomeSpeed.FineSpeed) == false)
                return false;

            timeOutTimer.Start(axisParam.MovingDoneWaitTimeMs);

            while (motion.IsHomeOn(axisNo))
            {
                Application.DoEvents();

                if (timeOutTimer.TimeOut)
                {
                    StopMove();
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.HomingTimeOut, ErrorLevel.Warning,
                                this.name, "Eject Home : Axis No = {0}", new object[] { axisNo.ToString() });
                    return false;
                }

                if (CheckValidState() == false)
                {
                    StopMove();
                    return false;
                }

                Thread.Sleep(AxisHandler.MotionDoneCheckIntervalMs);
            }

            LogHelper.Debug(LoggerType.Machine, String.Format("Eject Home Elapsed Time : {0} ms", axisParam.MovingDoneWaitTimeMs));

            ResetState();
            return true;
        }

        public void ResetAlarm()
        {
            motion.ResetAlarm(axisNo);
        }

        public void ResetAlarmOn(bool isOn)
        {
            motion.ResetAlarmOn(axisNo, isOn);
        }

        public void StopMove()
        {
            ResetState();

            motion.StopMove(axisNo);
        }

        public void EmergencyStop()
        {
            ResetState();

            motion.EmergencyStop(axisNo);
        }

        public bool StartHomeMove()
        {
            motion.ClearHomeDone(axisNo);
            timeOutTimer.Start(axisParam.HomingDoneWaitTimeMs);

            return motion.StartHomeMove(axisNo, axisParam.HomeSpeed);
        }

        public bool HomeMove()
        {
            if (IsMoveOn())
                return false;

            if (StartHomeMove() == false)
                return false;

            return WaitHomeDone();
        }

        public bool IsOnError()
        {
            return motion.IsAmpFault(axisNo);
        }

        public bool IsOnEmgStop()
        {
            return motion.IsEmgStop(axisNo);
        }

        public float GetNegativeLimitPos()
        {
            if (axisParam.Inverse)
                return ToPosition(axisParam.PositiveLimit);
            else
                return ToPosition(axisParam.NegativeLimit);
        }

        public float GetPositiveLimitPos()
        {
            if (axisParam.Inverse)
                return ToPosition(axisParam.NegativeLimit);
            else
                return ToPosition(axisParam.PositiveLimit);
        }

        public bool ContinuousMove(MovingParam movingParam = null, bool negative = false)
        {
            if (CheckValidState() == false)
                return false;

            bool result;
            if (movingParam != null)
                result = motion.ContinuousMove(axisNo, movingParam, negative);
            else
                result = motion.ContinuousMove(axisNo, axisParam.JogParam, negative);

            return result;
        }

        public void TurnOnServo(bool bOnOff)
        {
            motion.TurnOnServo(axisNo, bOnOff);
        }

        public bool IsMoveDone()
        {
            return motion.IsMoveDone(axisNo);
        }

        public bool IsMovingTimeOut()
        {
            return (timeOutTimer.TimeOut);
        }

        public bool WaitMoveDone()
        {
            while (motion.IsMoveDone(axisNo) == false)
            {
                Application.DoEvents();

                if (CheckValidState() == false)
                {
                    StopMove();
                    return false;
                }

                Thread.Sleep(AxisHandler.MotionDoneCheckIntervalMs);
            }

            ResetState();

            return true;
        }

        public bool WaitHomeDone()
        {
            while (motion.IsHomeDone(axisNo) == false)
            {
                Application.DoEvents();

                if (CheckValidState() == false)
                {
                    ResetState();
                    StopMove();
                    return false;
                }

                Thread.Sleep(AxisHandler.MotionDoneCheckIntervalMs);
            }

            Thread.Sleep(500);

            ResetState();

            SetPosition(0);

            return true;
        }

        public void ResetState()
        {
            timeOutTimer.Reset();
        }

        public bool IsAmpFault()
        {
            return motion.IsAmpFault(axisNo);
        }

        public bool IsServoOn()
        {
            return motion.IsServoOn(axisNo);
        }

        public bool IsHomeOn()
        {
            return motion.IsHomeOn(axisNo);
        }

        public bool IsPositiveOn()
        {
            return motion.IsPositiveOn(axisNo);
        }

        public bool IsNegativeOn()
        {
            return motion.IsNegativeOn(axisNo);
        }

        public bool IsMoveOn()
        {
            return motion.IsMoveOn(axisNo);
        }

        public bool IsPositiveLimit(double? axisPosition = null)
        {
            if (axisPosition == null)
                axisPosition = this.GetActualPos();
            return (axisPosition >= GetPositiveLimitPos());
        }

        public bool IsNegativeLimit(double? axisPosition = null)
        {
            if (axisPosition == null)
                axisPosition = this.GetActualPos();
            return (axisPosition <= GetNegativeLimitPos());
        }

        public float GetCommandPos()
        {
            float pulse = motion.GetCommandPos(axisNo);
            return ToPosition(pulse);
        }

        public float GetActualPos()
        {
            float pulse = motion.GetActualPos(axisNo);
            return ToPosition(pulse);
        }

        public float GetCommandPulse()
        {
            return motion.GetCommandPos(axisNo);
        }

        public float GetActualPulse()
        {
            return motion.GetActualPos(axisNo);
        }

        public float GetActualVel()
        {
            return motion.GetActualVel(axisNo);
        }

        public void SetPosition(float position)
        {
            float pulse = ToPulse(position);
            motion.SetPosition(axisNo, pulse);
        }

        public void SetPulse(float pulse)
        {
            motion.SetPosition(axisNo, pulse);
        }

        public AxisStatus GetAxisStatus()
        {
            return new AxisStatus { isServoOn = this.IsServoOn(), isMoving = !this.IsMoveDone(), isFault = this.IsAmpFault() };
        }

        public MotionStatus GetMotionStatus()
        {
            return motion.GetMotionStatus(axisNo);
        }

        public float ToPulse(float position)
        {
            double pulse = position / axisParam.MicronPerPulse * (axisParam.Inverse ? -1 : 1) + axisParam.OriginPulse;
            return (float)pulse;
        }

        public float ToOffsetPulse(float position)
        {
            double pulse = position / axisParam.MicronPerPulse * (axisParam.Inverse ? -1 : 1);
            return (float)pulse;
        }

        public float ToPosition(float pulse)
        {
            double position = (pulse - axisParam.OriginPulse) * axisParam.MicronPerPulse * (axisParam.Inverse ? -1 : 1);
            return (float)position;
        }

        public bool StartMultiMove(int[] axisNos, float[] position, MovingParam movingParam = null)
        {
            if (CheckValidState() == false)
                return false;

            bool result;
            if (movingParam != null)
                result = motion.StartMultiMove(axisNos, position, movingParam);
            else
                result = motion.StartMultiMove(axisNos, position, axisParam.MovingParam);

            if (result == true)
                timeOutTimer.Start(axisParam.MovingDoneWaitTimeMs);

            return result;
        }
    }
}
