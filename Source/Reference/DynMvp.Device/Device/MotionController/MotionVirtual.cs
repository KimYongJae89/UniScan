using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.Base;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Devices.MotionController
{
    public class MotionVirtual : Motion
    {
        bool[] on;
        bool[] home;
        bool[] homeOk;
        bool[] alarm;
        bool[] emg;
        float[] position;
        float[] speed;
        ManualResetEvent[] onMoving = null;
        CancellationTokenSource[] cancellationTokenSource = null;

        public override bool IsVirtual => true;

        public MotionVirtual(string name) : base(MotionType.Virtual, name)
        {
        }

        public MotionVirtual(MotionType motionType, string name) : base(motionType, name)
        {
        }

        public override bool Initialize(MotionInfo motionInfo)
        {
            this.NumAxis = motionInfo.NumAxis;

            on = new bool[this.NumAxis];
            home = new bool[this.NumAxis];
            homeOk = new bool[this.NumAxis];
            alarm = new bool[this.NumAxis];
            position = new float[this.NumAxis];
            speed = new float[this.NumAxis];
            emg = new bool[this.NumAxis];
            onMoving = new ManualResetEvent[this.NumAxis];
            cancellationTokenSource = new CancellationTokenSource[this.NumAxis];
            for (int i=0; i< this.NumAxis; i++)
            {
                on[i] = false;
                alarm[i] = false;
                home[i] = false;
                homeOk[i] = false;
                position[i] = 0;
                speed[i] = 0;
                emg[i] = false;
                onMoving[i] = new ManualResetEvent(false);
                cancellationTokenSource[i] = null;
            }

            return true;
        }

        public override void Release()
        {
            base.Release();
        }

        public override bool CanSyncMotion()
        {
            return false;
        }

        public override void TurnOnServo(int axisNo, bool bOnOff)
        {
            on[axisNo] = bOnOff;
        }

        public override float GetCommandPos(int axisNo)
        {
            if (position.Length <= axisNo)
                return float.NaN;

            return position[axisNo];
        }

        public override float GetActualPos(int axisNo)
        {
            if (position != null && position.Count() > axisNo)
            {
                return position[axisNo];
            }
            return 0;
        }

        public override float GetActualVel(int axisNo)
        {
            return this.speed[axisNo];
        }

        public override void SetPosition(int axisNo, float position)
        {
            this.position[axisNo] = position;
        }

        public override bool StartHomeMove(int axisNo, HomeParam homeSpeed)
        {
            if (home[axisNo])
                return false;

            //position[axisNo] = 0;
            //speed[axisNo] = 0;
            //homeOk[axisNo] = true;
            //home[axisNo] = false;

            this.cancellationTokenSource[axisNo] = new CancellationTokenSource();
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                cancellationTokenSource[axisNo].Token.ThrowIfCancellationRequested();

                position[axisNo] = 0;
                speed[axisNo] = 0;
                homeOk[axisNo] = true;
                home[axisNo] = false;
            }, cancellationTokenSource[axisNo].Token);

            return true;
        }

        public override bool StartMove(int axisNo, float position, MovingParam movingParam)
        {
            return Moving(axisNo, position, movingParam);
        }

        public override bool StartRelativeMove(int axisNo, float offset, MovingParam movingParam)
        {
            if (position == null || position.Count() <= axisNo)
                return false;

            return Moving(axisNo, position[axisNo] + offset, movingParam);
        }

        public override bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative)
        {
            if (onMoving[axisNo].WaitOne(0))
                return false;

            onMoving[axisNo].Set();
            cancellationTokenSource[axisNo] = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                double srcPos = this.position[axisNo];
                this.speed[axisNo] = (movingParam.MaxVelocity ) * (negative ? -1 : 1);
                while(cancellationTokenSource[axisNo].Token.IsCancellationRequested==false)
                {
                    srcPos += this.speed[axisNo];
                    this.position[axisNo] = (int)Math.Round(srcPos);
                    Thread.Sleep(1);
                }
                this.speed[axisNo] = 0;
                onMoving[axisNo].Reset();
            }, cancellationTokenSource[axisNo].Token);

            return true;
        }

        private bool Moving(int axisNo, float position, MovingParam movingParam)
        {
            if (homeOk[axisNo] == false)
                return false;

            if (onMoving[axisNo].WaitOne(0))
                return false;

            onMoving[axisNo].Set();
            cancellationTokenSource[axisNo] = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                int sampleTimeMs = 20;
                double srcPos = this.position[axisNo];  // um
                double vecPos = position - this.position[axisNo];   // um
                double spd = (movingParam.MaxVelocity / 1000) * (vecPos > 0 ? 1 : -1);  // mm/s
                int timeMs = (int)Math.Abs(vecPos / spd);   // ms
                if (timeMs < 20)
                    sampleTimeMs = 1;
                for (int i = 0; i < timeMs / sampleTimeMs; i++)
                {
                    if (cancellationTokenSource[axisNo].Token.IsCancellationRequested)
                        break;
                    this.position[axisNo] = (int)Math.Round(srcPos + (i * sampleTimeMs * spd));
                    Thread.Sleep(sampleTimeMs);
                }

                if (cancellationTokenSource[axisNo].Token.IsCancellationRequested == false)
                    this.position[axisNo] = position;
                onMoving[axisNo].Reset();
            }, cancellationTokenSource[axisNo].Token);

            return true;
        }

        public override void StopMove(int axisNo)
        {
            if (cancellationTokenSource[axisNo] != null)
                cancellationTokenSource[axisNo].Cancel();
            while (this.onMoving[axisNo].WaitOne(0)) ;
        }

        public override void EmergencyStop(int axisNo)
        {
            if (cancellationTokenSource[axisNo] != null)
                cancellationTokenSource[axisNo].Cancel();
            this.emg[axisNo] = true;
        }

        public override bool ClearHomeDone(int axisNo)
        {
            this.homeOk[axisNo] = false;
            return true;
        }
        
        public override MotionStatus GetMotionStatus(int axisNo)
        {
            MotionStatus motionStatus = new MotionStatus()
            {
                inp = !onMoving[axisNo].WaitOne(0),
                alarm = false,
                emg = this.emg[axisNo],
                err = false,
                ez = false,
                homeOk = this.homeOk[axisNo],
                negLimit = false,
                origin = this.position[axisNo]==0,
                posLimit = false,
                run = onMoving[axisNo].WaitOne(0),
                servoOn = on[axisNo]
            };            

            return motionStatus;
        }
        
        public override bool ResetAlarm(int axisNo)
        {
            alarm[axisNo] = emg[axisNo] = false;
            return true;
        }

        public override bool ResetAlarmOn(int axisNo, bool isOn)
        {
            alarm[axisNo] = emg[axisNo] = false;
            return true;
        }

        public override bool StartMultiMove(int[] axisNos, float[] position, MovingParam movingParam)
        {
            bool good = true;
            for (int i = 0; i < position.Length; i++)
                good &= this.StartMove(axisNos[i], position[i], movingParam);

            return good;
        }
        
        public override bool StartCmp(int axisNo, int startPos, float dist, bool plus)
        {
            return true;
        }

        public override bool EndCmp(int axisNo)
        {
            return true;
        }

        public override bool ModifyPos(int axisNo, float position)
        {
            throw new NotImplementedException();
        }
    }
}
