using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DynMvp.Devices.MotionController
{
    public class MotionPowerPmacParam
    {
        public enum eParam
        {
            PARAM_START = 0,
            MoveSpeed,
            ScanStep,
            EncoderLimit,
            EncoderUnits,
            PARAM_END
        };

        //ParamChangedDelegate paramChangedCallback = null;
        int[] homePos = new int[3] { 0, 0, 0 };

        List<int> moveSpeed = new List<int>();
        float scanStep = 0.5f;  // mm
        List<Point> encoderLimit = new List<Point>();
        List<Point> workingLimit = new List<Point>();
        List<int> encoderUnits = new List<int>();

        public int[] HomePos
        {
            get { return homePos; }
        }

        public List<int> MoveSpeed
        {
            get { return moveSpeed; }
        }

        public float ScanStep
        {
            get { return scanStep; }
        }

        public List<Point> WorkingLimit
        {
            get { return workingLimit; }
        }
        public List<Point> EncoderLimit
        {
            get { return encoderLimit; }
        }

        public List<int> EncoderUnits
        {
            get { return encoderUnits; }
        }


        public void Initialize()
        {
            moveSpeed.Add(32);  // X axis move Speed. [Counts/ms]
            moveSpeed.Add(24);  // Y axis move Speed. [Counts/ms]
            moveSpeed.Add(24);  // Z axis move Speed. [Counts/ms]

            //scanStep.Add(10000); // X axis Scan Step. [Counts]
            //scanStep.Add(1000); // Y axis Scan Step. [Counts]
            //scanStep.Add(3000); // Z axis Scan Step. [Counts]
            scanStep = 0.5f;

            encoderUnits.Add(10000);    // X axis unit. [counts/mm]
            encoderUnits.Add(1000); // Y axis unit. [counts/mm]
            encoderUnits.Add(3000); // Z axis unit. [counts/mm]

            //encoderLimit.Add(new Point(10000 * 0, 10000 * 100));    // X axis measureing region. (min,max). [Step/mm] * [mm]
            //encoderLimit.Add(new Point(1000 * 0, 1000 * 85)); // Y axis measureing region. (min,max). [Step/mm] * [mm]
            //encoderLimit.Add(new Point(3000 * 0, 3000 * 54)); // Z axis measureing region. (min,max). [Step/mm] * [mm]
            encoderLimit.Add(new Point(0, 100));    // X axis measureing region. (min,max). [Step/mm] * [mm]
            encoderLimit.Add(new Point(0, 85)); // Y axis measureing region. (min,max). [Step/mm] * [mm]
            encoderLimit.Add(new Point(0, 54)); // Z axis measureing region. (min,max). [Step/mm] * [mm]


            //workingLimit.Add(new Point(10000 * 20, 10000 * 80));    // X axis measureing region. (min,max). [Step/mm] * [mm]
            //workingLimit.Add(new Point(1000 * 20, 1000 * 80)); // Y axis measureing region. (min,max). [Step/mm] * [mm]
            //workingLimit.Add(new Point(3000 * 43, 3000 * 43)); // Z axis measureing region. (min,max). [Step/mm] * [mm]
            workingLimit.Add(new Point(0, 100));    // X axis measureing region. (min,max). [Step/mm] * [mm]
            workingLimit.Add(new Point(0, 85)); // Y axis measureing region. (min,max). [Step/mm] * [mm]
            workingLimit.Add(new Point(43, 43)); // Z axis measureing region. (min,max). [Step/mm] * [mm]
        }

        public RectangleF GetFullRegionMM()
        {
            float l = (float)encoderLimit[0].X / (float)encoderUnits[0];
            float r = (float)encoderLimit[0].Y / (float)encoderUnits[0];
            float t = (float)encoderLimit[1].X / (float)encoderUnits[1];
            float b = (float)encoderLimit[1].Y / (float)encoderUnits[1];

            return RectangleF.FromLTRB(l, t, r, b);
        }

        public RectangleF GetWorkingRegionMM()
        {
            float l = (float)workingLimit[0].X / (float)encoderUnits[0];
            float r = (float)workingLimit[0].Y / (float)encoderUnits[0];
            float t = (float)workingLimit[1].X / (float)encoderUnits[1];
            float b = (float)workingLimit[1].Y / (float)encoderUnits[1];

            return RectangleF.FromLTRB(l, t, r, b);
        }
        //public ParamChangedDelegate ParamChangedCallback
        //{
        //    get { return paramChangedCallback; }
        //    set { paramChangedCallback = value; }
        //}
    }

    public class MotionPowerPmac : Motion, IDigitalIo
    {

        uint deviceId = 0xFFFFFFFF;
        bool[] isHome = new bool[3] { false, false, false };

        string ipAddress;
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        uint numDigitalInput = 0;
        uint numDigitalOutput = 0;
        private MotionPowerPmacParam param = new MotionPowerPmacParam();
        public MotionPowerPmacParam Param
        {
            get { return param; }
            set { param = value; }
        }

        public MotionPowerPmac(string name) : base(MotionType.PowerPmac, name)
        {
        }

        public string GetName() { return Name; }
        public int GetNumInPort() { return (int)numDigitalInput; }
        public int GetNumOutPort() { return (int)numDigitalOutput; }
        public int GetNumInPortGroup() { return 1; }
        public int GetNumOutPortGroup() { return 1; }
        public int GetInPortStartGroupIndex() { return 0; }
        public int GetOutPortStartGroupIndex() { return 0; }

        public override bool Initialize(MotionInfo motionInfo)
        {
            uint selectId = 0;
            bool ok = false;
            bool loop = true;

            do
            {
                ok = PowerPmacDLL.OpenPmacDevice((uint)selectId);
                if (!ok)
                {
                    // 연결 실패. 재시도
                    selectId = PowerPmacDLL.PmacSelect(0);
                    if (selectId < 0 || selectId > 7)
                    {
                        loop = false;
                    }
                }
            } while (loop);

            if (!ok)
            {
                // 연결 실패.
                UpdateState(DeviceState.Error, "Can't connection device");
                return false;
            }

            // 연결 성공
            deviceId = selectId;
            UpdateState(DeviceState.Ready, "Connected");

            NumAxis = motionInfo.NumAxis;
            //NumAxis = 3;
            //if (NumAxis == 3)
            //{
            //    param.Initialize();
            //}

            return true;
        }

        public override void Release()
        {
            base.Release();

            if (deviceId != 0xFFFFFFFF && base.IsReady())
            {
                PowerPmacDLL.ClosePmacDevice(deviceId);

                deviceId = 0xFFFFFFFF;
                UpdateState(DeviceState.Idle, "Released");
            }
        }

        public bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            return true;
        }

        private string SendCommandAndGetResponse(string command)
        {
            if (base.IsReady() == false)
                return null;

            byte[] byResponce = new byte[255];
            string strResponce = null;
            long resp = -1;
            try
            {
                byte[] byCommand = System.Text.Encoding.ASCII.GetBytes(command);
                resp = PowerPmacDLL.PmacGetResponseA(deviceId, byResponce, 255, byCommand);
                strResponce = System.Text.Encoding.UTF8.GetString(byResponce).Trim(new char[] { '\r', '\n', '\0' });
            }
            catch (ArgumentException ex)
            {
                UpdateState(DeviceState.Warning, "Comm error");
            }
            return strResponce;
        }

        private void SetIValue(int axisNo, int Addr, int Val)
        {
            string strCommand = string.Format("I{0}{1}={2}", axisNo, Addr, Val);
            SendCommandAndGetResponse(strCommand);
        }

        public override bool CanSyncMotion()
        {
            return false;
        }

        public override void TurnOnServo(int axisNo, bool bOnOff)
        {
            SetIValue(axisNo, 00, (bOnOff == false) ? 0 : 1);
        }

        public override float GetCommandPos(int axisNo)
        {
            double posTarget = PowerPmacDLL.PmacDPRGetCommandedPos(deviceId, axisNo, 1000.0);
            return (float)posTarget;
        }

        public override float GetActualPos(int axisNo)
        {
            //double posReal = PowerPmacDLL.PmacDPRPosition(deviceId, axisNo, 1000.0);
            //return (float)posReal;
            string strCommand = string.Format("#{0}P", axisNo);
            string strResponce = SendCommandAndGetResponse(strCommand);

            return (float)Convert.ToDouble(strResponce);
        }

        public override float GetActualVel(int axisNo)
        {
            throw new NotImplementedException();
        }

        public override void SetPosition(int axisNo, float position)
        {
            throw new NotImplementedException();
        }

        public override bool StartHomeMove(int axisNo, HomeParam homeSpeed)
        {
            // Move to negative limit
            ContinuousMove(axisNo, new MovingParam("Jog-", 0, 0, 0, 0, 0), false);

            // wait done
            while (!(IsMoveDone(axisNo)));

            // 0-Position
            param.HomePos[axisNo-1] = (int)(Math.Round(GetActualPos(axisNo)));

            // Move to working region
            StartMove(axisNo, param.WorkingLimit[axisNo - 1].X, new MovingParam("", 0, 0, 0, 0, 0));
            while (!(IsMoveDone(axisNo)));

            isHome[axisNo-1] = true;

            return true;
        }

        private void SetSpeed(int axisNo, double velocity)
        {
            int countPerMilisec = (int)(Math.Abs(velocity) + 0.5);
            SetIValue(axisNo, 22, countPerMilisec);
        }

        public override bool StartMove(int axisNo, float position, MovingParam movingParam)
        {
            if (movingParam.MaxVelocity > 0)
            {
                SetSpeed(axisNo, movingParam.MaxVelocity);
            }

            int pos = param.HomePos[axisNo - 1] + (int)(position * Param.EncoderUnits[axisNo - 1]);
            string strCommand = string.Format("#{0}J={1}", axisNo, pos);
            SendCommandAndGetResponse(strCommand);
            isHome[axisNo] = false;

            return true;
        }

        public override bool StartRelativeMove(int axisNo, float position, MovingParam movingParam)
        {
            if (movingParam.MaxVelocity > 0)
            {
                SetSpeed(axisNo, movingParam.MaxVelocity);
            }

            int pos = param.HomePos[axisNo - 1] + (int)(position * Param.EncoderUnits[axisNo - 1]);
            string strCommand = string.Format("#{0}J:{1}", axisNo, pos);
            SendCommandAndGetResponse(strCommand);
            isHome[axisNo - 1] = false;

            return true;
        }

        public override bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative)
        {
            if (movingParam.MaxVelocity > 0)
            {
                SetSpeed(axisNo, movingParam.MaxVelocity);
            }

            string strCommand = string.Format("#{0}J", axisNo);
            if (movingParam.Name == "Jog+")
            {
                strCommand += "+";
            }
            else if (movingParam.Name == "Jog-")
            {
                strCommand += "-";
            }
            else
            {
                strCommand += "/";
            }
            SendCommandAndGetResponse(strCommand);

            return true;
        }

        public override void StopMove(int axisNo)
        {
            string strCommand = string.Format("#{0}J/", axisNo);
            SendCommandAndGetResponse(strCommand);
        }

        public override void EmergencyStop(int axisNo)
        {
            string strCommand = string.Format("#{0}J/", axisNo);
            SendCommandAndGetResponse(strCommand);
        }

        public override bool ClearHomeDone(int axisNo)
        {
            throw new NotImplementedException();
        }

        private void IsMoveDone(int axisNo, out bool isMoveDone)
        {
            //return PowerPmacDLL.PmacDPRInposition(deviceId, axisNo);
            string strCommand = string.Format("#{0}?", axisNo);
            string strResponce = SendCommandAndGetResponse(strCommand);

            long val = Convert.ToInt64(strResponce, 16);
            byte inPos = (byte)(val & 0x01);    // In-Position bit
            isMoveDone = (inPos == 1);
        }
        
        private void IsLimitOn(int axisNo, out bool posLimitOn, out bool negLimitOn)
        {
            string strCommand = string.Format("#{0}?", axisNo);
            string strResponce = SendCommandAndGetResponse(strCommand);
            long val = Convert.ToInt64(strResponce, 16);

            posLimitOn = (val >> 45 & 0x01) == 1;
            negLimitOn = (byte)(val >> 46 & 0x01) == 1;
        }
        
        public void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            throw new NotImplementedException();
        }

        public uint ReadOutputGroup(int groupNo)
        {
            throw new NotImplementedException();
        }

        public uint ReadInputGroup(int groupNo)
        {
            throw new NotImplementedException();
        }

        public override MotionStatus GetMotionStatus(int axisNo)
        {
            throw new NotImplementedException();
        }

        public void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public bool IsReady()
        {
            return deviceId != 0xFFFFFFFF;
        }

        public override bool ResetAlarm(int axisNo)
        {
            throw new NotImplementedException();
        }

        public override bool ResetAlarmOn(int axisNo, bool isOn)
        {
            throw new NotImplementedException();
        }

        public void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            throw new NotImplementedException();
		}

        public override bool StartMultiMove(int[] axisNos, float[] position, MovingParam movingParam)
        {
            throw new NotImplementedException();
        }
        
        public override bool StartCmp(int axisNo, int startPos, float dist, bool plus)
        {
            throw new NotImplementedException();
        }

        public override bool EndCmp(int axisNo)
        {
            throw new NotImplementedException();
        }

        public override bool ModifyPos(int axisNo, float position)
        {
            throw new NotImplementedException();
        }
    }
}
