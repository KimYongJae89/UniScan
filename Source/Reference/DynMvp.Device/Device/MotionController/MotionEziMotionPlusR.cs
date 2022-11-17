using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Comm;
using DynMvp.Device.Device.MotionController;

namespace DynMvp.Devices.MotionController
{
    enum EzServoParam : byte
    {
        PULSEPERREVOLUTION = 0,
        AXISMAXSPEED,
        AXISSTARTSPEED,
        AXISACCTIME,
        AXISDECTIME,

        SPEEDOVERRIDE,
        JOGHIGHSPEED,
        JOGLOWSPEED,
        JOGACCDECTIME,

        SERVOALARMLOGIC,
        SERVOONLOGIC,
        SERVORESETLOGIC,

        SWLMTPLUSVALUE,
        SWLMTMINUSVALUE,
        SOFTLMTSTOPMETHOD,
        HARDLMTSTOPMETHOD,
        LIMITSENSORLOGIC,

        ORGSPEED,
        ORGSEARCHSPEED,
        ORGACCDECTIME,
        ORGMETHOD,
        ORGDIR,
        ORGOFFSET,
        ORGPOSITIONSET,
        ORGSENSORLOGIC,

        POSITIONLOOPGAIN,
        INPOSITIONVALUE,
        POSTRACKINGLIMIT,
        MOTIONDIR,

        LIMITSENSORDIR,
        ORGTORQUERATIO,

        POSERROVERFLOWLIMIT,
        POSVALUECOUNTINGMETHOD,
    }

    enum FasAxisStatus : uint
    {
        ERRORALL = 0x00000001,
        HWPOSILMT = 0x00000002,
        HWNEGALMT = 0x00000004,
        SWPOGILMT = 0x00000008,
        SWNEGALMT = 0x00000010,
        RESERVED0 = 0x00000020,
        RESERVED1 = 0x00000040,
        ERRPOSOVERFLOW = 0x00000080,
        ERROVERCURRENT = 0x00000100,
        ERROVERSPEED = 0x00000200,
        ERRPOSTRACKING = 0x00000400,
        ERROVERLOAD = 0x00000800,
        ERROVERHEAT = 0x00001000,
        ERRBACKEMF = 0x00002000,
        ERRMOTORPOWER = 0x00004000,
        ERRINPOSITION = 0x00008000,
        EMGSTOP = 0x00010000,
        SLOWSTOP = 0x00020000,
        ORIGINRETURNING = 0x00040000,
        INPOSITION = 0x00080000,
        SERVOON = 0x00100000,
        ALARMRESET = 0x00200000,
        PTSTOPPED = 0x00400000,
        ORIGINSENSOR = 0x00800000,
        ZPULSE = 0x01000000,
        ORIGINRETOK = 0x02000000,
        MOTIONDIR = 0x04000000,
        MOTIONING = 0x08000000,
        MOTIONPAUSE = 0x10000000,
        MOTIONACCEL = 0x20000000,
        MOTIONDECEL = 0x40000000,
        MOTIONCONST = 0x80000000
    }

    class MotionEziMotionPlusR : Motion, IDigitalIo
    {
        uint numDigitalInput = 0;
        uint numDigitalOutput = 0;

        byte portNo;

        SerialMotionInfo serialMotionInfo;

        public MotionEziMotionPlusR(string name)
            : base(MotionType.FastechEziMotionPlusR, name)
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
            this.serialMotionInfo = (SerialMotionInfo)motionInfo;
            SerialPortInfo serialPortInfo = serialMotionInfo.SerialPortInfo;

            if (FAS_EziMOTIONPlusR.FAS_Connect((byte)serialPortInfo.PortNo, (uint)serialPortInfo.BaudRate) == 0)
                throw new AlarmException(ErrorCodeMotion.Instance.FailToInitialize, ErrorLevel.Fatal,
                    motionInfo.Name, "Fail to Conntect.", null, "");

            for (byte i = 0; i < FAS_EziMOTIONPlusR.MAX_SLAVE_NUMS; i++)
            {
                if (FAS_EziMOTIONPlusR.FAS_IsSlaveExist((byte)serialPortInfo.PortNo, i) != 0)
                {
                    NumAxis++;
                    int retVal = 0;
                    retVal = FAS_EziMOTIONPlusR.FAS_SetParameter((byte)serialPortInfo.PortNo, i, (byte)EzServoParam.SWLMTMINUSVALUE, 0);
                    if (retVal != 0)
                        throw new AlarmException(ErrorCodeMotion.Instance.FailToInitialize, ErrorLevel.Fatal,
                            motionInfo.Name, GetErrorString(retVal), null, "");

                    retVal = FAS_EziMOTIONPlusR.FAS_SetParameter((byte)serialPortInfo.PortNo, i, (byte)EzServoParam.SWLMTPLUSVALUE, 0);
                    if (retVal != 0)
                        throw new AlarmException(ErrorCodeMotion.Instance.FailToInitialize, ErrorLevel.Fatal,
                            motionInfo.Name, GetErrorString(retVal), null, "");
                }
            }

            portNo = (byte)serialPortInfo.PortNo;

            UpdateState(DeviceState.Ready);

            return true;
        }

        public override void Release()
        {
            base.Release();

            this.TurnOnServo(false);
            SerialPortInfo serialPortInfo = serialMotionInfo.SerialPortInfo;

            FAS_EziMOTIONPlusR.FAS_Close((byte)serialPortInfo.PortNo);
            UpdateState(DeviceState.Idle);
        }

        public bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            return true;
        }

        public override bool CanSyncMotion()
        {
            return false;
        }

        string GetErrorString(int value)
        {
            switch (value)
            {
                case FAS_EziMOTIONPlusR.FMM_INVALID_PORT_NUM:
                    return "Invalid Port Number";
                case FAS_EziMOTIONPlusR.FMM_INVALID_SLAVE_NUM:
                    return "Invalid Slave Number";
                case FAS_EziMOTIONPlusR.FMC_DISCONNECTED:
                    return "Disconnected";
                case FAS_EziMOTIONPlusR.FMC_TIMEOUT_ERROR:
                    return "Timeout";
                case FAS_EziMOTIONPlusR.FMC_CRCFAILED_ERROR:
                    return "CRC Failed";
                case FAS_EziMOTIONPlusR.FMC_RECVPACKET_ERROR:
                    return "Receive Packet Error";
                case FAS_EziMOTIONPlusR.FMM_POSTABLE_ERROR:
                    return "Postable Error";
                case FAS_EziMOTIONPlusR.FMP_FRAMETYPEERROR:
                    return "Frame Type Error";
                case FAS_EziMOTIONPlusR.FMP_DATAERROR:
                    return "Data Error";
                case FAS_EziMOTIONPlusR.FMP_PACKETERROR:
                    return "Packet Error";
                case FAS_EziMOTIONPlusR.FMP_RUNFAIL:
                    return "Run Failed";
                case FAS_EziMOTIONPlusR.FMP_RESETFAIL:
                    return "Reset Failed";
                case FAS_EziMOTIONPlusR.FMP_SERVOONFAIL1:
                    return "Servo On Failed(1)";
                case FAS_EziMOTIONPlusR.FMP_SERVOONFAIL2:
                    return "Servo On Failed(2)";
                case FAS_EziMOTIONPlusR.FMP_SERVOONFAIL3:
                    return "Servo On Failed(3)";
                case FAS_EziMOTIONPlusR.FMP_PACKETCRCERROR:
                    return "Packet CRC Error";
            }

            return string.Format("Unknown Error Code(0x{0})", Convert.ToString(value, 16).ToUpper());
        }

        public override void TurnOnServo(int axisNo, bool bOnOff)
        {
            int retVal = FAS_EziMOTIONPlusR.FAS_ServoEnable(portNo, (byte)axisNo, (bOnOff ? 1 : 0));
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToWriteParam, ErrorLevel.Error,
              this.name, GetErrorString(retVal), null);
            }
        }

        public override float GetCommandPos(int axisNo)
        {
            int commandPos = 0;
            int retVal = FAS_EziMOTIONPlusR.FAS_GetCommandPos(portNo, (byte)axisNo, ref commandPos);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToReadParam, ErrorLevel.Error,
              this.name, GetErrorString(retVal), null);
            }

            return commandPos;
        }

        public override float GetActualPos(int axisNo)
        {
            int actualPos = 0;
            int retVal = FAS_EziMOTIONPlusR.FAS_GetActualPos(portNo, (byte)axisNo, ref actualPos);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToReadParam, ErrorLevel.Error,
              this.name, GetErrorString(retVal), null);
            }

            return actualPos;
        }

        public override float GetActualVel(int axisNo)
        {
            int actualVel = 0;
            int retVal = FAS_EziMOTIONPlusR.FAS_GetActualVel(portNo, (byte)axisNo, ref actualVel);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToReadParam, ErrorLevel.Error,
              this.name, GetErrorString(retVal), null);
            }

            return actualVel;
        }

        public override void SetPosition(int axisNo, float position)
        {
            int retVal = FAS_EziMOTIONPlusR.FAS_ClearPosition(portNo, (byte)axisNo);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToWriteParam, ErrorLevel.Error,
                     this.name, GetErrorString(retVal), null);
            }
        }

        public override bool StartHomeMove(int axisNo, HomeParam homeSpeed)
        {
            try
            {
                int retVal = 0;

                retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, (byte)EzServoParam.ORGSPEED, (int)homeSpeed.HighSpeed.MaxVelocity);
                if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
                    throw new Exception(GetErrorString(retVal));

                retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, (byte)EzServoParam.ORGSEARCHSPEED, (int)homeSpeed.FineSpeed.MaxVelocity);
                if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
                    throw new Exception(GetErrorString(retVal));

                retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, (byte)EzServoParam.ORGDIR, homeSpeed.HomeDirection == MoveDirection.CW ? 0 : 1);
                if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
                    throw new Exception(GetErrorString(retVal));

                retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, (byte)EzServoParam.ORGMETHOD, homeSpeed.HomeMode == HomeMode.HomeSensor ? 1 : 2);   // z origin
                if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
                    throw new Exception(GetErrorString(retVal));

                retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, (byte)EzServoParam.HARDLMTSTOPMETHOD, (int)0);
                if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
                    throw new Exception(GetErrorString(retVal));

                retVal = FAS_EziMOTIONPlusR.FAS_MoveOriginSingleAxis(portNo, (byte)axisNo);
                if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
                    throw new Exception(GetErrorString(retVal));

            }
            catch (Exception ex)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.Moving, ErrorLevel.Error,
                           this.name, ex.Message, null);
                return false;
            }

            return true;
        }

        public override bool StartMove(int axisNo, float position, MovingParam movingParam)
        {
            int retVal = 0;
            retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, 3, (int)movingParam.AccelerationTimeMs);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToWriteParam, ErrorLevel.Error,
                               this.name, GetErrorString(retVal), null);

                return false;
            }

            retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, 4, (int)movingParam.DecelerationTimeMs);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToWriteParam, ErrorLevel.Error,
                             this.name, GetErrorString(retVal), null);

                return false;
            }

            retVal = FAS_EziMOTIONPlusR.FAS_MoveSingleAxisAbsPos(portNo, (byte)axisNo, (int)position, (uint)movingParam.MaxVelocity);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.Moving, ErrorLevel.Error,
                                this.name, GetErrorString(retVal), null);

                return false;
            }

            return true;
        }

        public override bool StartRelativeMove(int axisNo, float position, MovingParam movingParam)
        {
            int retVal = FAS_EziMOTIONPlusR.FAS_MoveSingleAxisIncPos(portNo, (byte)axisNo, (int)position, (uint)movingParam.MaxVelocity);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.Moving, ErrorLevel.Error,
                                this.name, GetErrorString(retVal), null);

                return false;
            }

            return true;
        }

        public override bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative)
        {
            int retVal = 0;
            retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, 8, (int)movingParam.AccelerationTimeMs);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToWriteParam, ErrorLevel.Error,
                    this.name, GetErrorString(retVal), null);

                return false;
            }

            retVal = FAS_EziMOTIONPlusR.FAS_MoveVelocity(portNo, (byte)axisNo, (uint)movingParam.MaxVelocity, negative ? 0 : 1);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.Moving, ErrorLevel.Error,
                    this.name, GetErrorString(retVal), null);

                return false;
            }

            return true;
        }

        public override void StopMove(int axisNo)
        {
            FAS_EziMOTIONPlusR.FAS_MoveStop(portNo, (byte)axisNo);
        }

        public override void EmergencyStop(int axisNo)
        {
            FAS_EziMOTIONPlusR.FAS_EmergencyStop(portNo, (byte)axisNo);
        }

        public override bool ClearHomeDone(int axisNo)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
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
            MotionStatus motionStatus = new MotionStatus() { alarm = true };

            uint axisStatus = 0;
            int retVal = FAS_EziMOTIONPlusR.FAS_GetAxisStatus(portNo, (byte)axisNo, ref axisStatus);
            if (retVal != 0)
                return motionStatus;

            motionStatus.origin = (axisStatus & (int)FasAxisStatus.ORIGINSENSOR) != 0;
            motionStatus.inp = (axisStatus & (int)FasAxisStatus.INPOSITION) != 0;
            motionStatus.run = (axisStatus & (int)FasAxisStatus.MOTIONING) != 0;
            motionStatus.posLimit = (axisStatus & (int)FasAxisStatus.HWPOSILMT) != 0;
            motionStatus.negLimit = (axisStatus & (int)FasAxisStatus.HWNEGALMT) != 0;
            motionStatus.servoOn = (axisStatus & (int)FasAxisStatus.SERVOON) != 0;
            motionStatus.emg = (axisStatus & (int)FasAxisStatus.EMGSTOP) != 0;
            motionStatus.alarm = (axisStatus & (int)FasAxisStatus.ERRORALL) != 0;
            motionStatus.home = (axisStatus & (int)FasAxisStatus.ORIGINRETURNING) != 0;
            motionStatus.homeOk = (axisStatus & (int)FasAxisStatus.ORIGINRETOK) != 0;
            return motionStatus;
        }

        public void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public override bool ResetAlarm(int axisNo)
        {
            int retVal = FAS_EziMOTIONPlusR.FAS_ServoAlarmReset(portNo, (byte)axisNo);
            return retVal == FAS_EziMOTIONPlusR.FMM_OK;
        }

        public override bool ResetAlarmOn(int axisNo, bool isOn)
        {
            int retVal = FAS_EziMOTIONPlusR.FAS_ServoAlarmReset(portNo, (byte)axisNo);
            return retVal == FAS_EziMOTIONPlusR.FMM_OK;
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
            return false;
        }

        public override bool EndCmp(int axisNo)
        {
            return false;
        }

        public override bool ModifyPos(int axisNo, float position)
        {
            throw new NotImplementedException();
        }
    }
}
