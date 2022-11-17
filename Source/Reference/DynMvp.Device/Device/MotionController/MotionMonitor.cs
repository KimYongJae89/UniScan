using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.MotionController
{
    public delegate bool MotionEvent(MotionEventHandler eventSource);
    public class MotionEventHandler
    {
        protected string name;
        public string Name
        {
            get { return name; }
        }

        protected AxisHandler axisHandler;
        protected int axisNo;

        AxisStatus motionStatus = new AxisStatus { isServoOn = false, isMoving = false, isFault = false };

        public MotionEvent OnServoOn;
        public MotionEvent OnServoOff;
        public MotionEvent OnStartMove;
        public MotionEvent OnMoveDone;
        public MotionEvent OnFault;

        public MotionEventHandler(string name, AxisHandler axisHandler, int axisNo)
        {
            this.name = name;
            this.axisHandler = axisHandler;
            this.axisNo = axisNo;
        }


        public void ResetStatus()
        {
            if (axisNo < 0)
            {
                motionStatus.ResetStatus();
                foreach (Axis axis in axisHandler.AxisList)
                {
                    AxisStatus axisStatus = axis.GetAxisStatus();
                    motionStatus |= axisStatus;
                }
            }
            else
            {
                motionStatus = axisHandler.GetAxis(axisNo).GetAxisStatus();
            }
        }

        public bool CheckStatus()
        {
            AxisStatus axisStatus;
            if (axisNo < 0)
            {
                axisStatus = new AxisStatus();
                foreach (Axis axis in this.axisHandler.AxisList)
                    axisStatus |= axis.GetAxisStatus();
            }
            else
            {
                axisStatus = axisHandler.GetAxis(axisNo).GetAxisStatus();
            }
            return CheckStatus(axisStatus);
        }

        private bool CheckStatus(AxisStatus axisStatus)
        {
            bool processOk = true;

            if (axisStatus.isServoOn != motionStatus.isServoOn)
            {
                if (axisStatus.isServoOn)
                {
                    if (OnServoOn != null)
                        processOk &= OnServoOn(this);
                }
                else
                {
                    if (OnServoOff != null)
                        processOk &= OnServoOff(this);
                }
            }
            else if (axisStatus.isMoving != motionStatus.isMoving)
            {
                if (axisStatus.isMoving)
                {
                    if (OnStartMove != null)
                        processOk &= OnStartMove(this);
                }
                else
                {
                    if (OnMoveDone != null)
                        processOk &= OnMoveDone(this);
                }
            }
            else if (axisStatus.isFault != motionStatus.isFault)
            {
                if (axisStatus.isFault)
                {
                    if (OnFault != null)
                        processOk &= OnFault(this);
                }
            }

            if (processOk)
                motionStatus = axisStatus;

            return processOk;
        }
    }

    public class MotionMonitor : ThreadHandler
    {
        MotionList motionList;
        List<MotionEventHandler> motionEventHandlerList;

        public MotionMonitor(MotionList motionList, List<MotionEventHandler> motionEventHandlerList) : base("MotionMonitor")
        {
            this.WorkingThread = new Thread(MonitoringProc);
            this.RequestStop = false;

            this.motionList = motionList;
            this.motionEventHandlerList = motionEventHandlerList;
            ThreadManager.AddThread(this);
        }

        public void MonitoringProc()
        {
            try
            {
                //bool isServoOn = false;
                //bool isMoving = false;

                while (this.RequestStop == false)
                {
                    try
                    {
                        if (ErrorManager.Instance().IsAlarmed())
                        {
                            motionList.ForEach(x => x.EmergencyStop());
                        }
                        else
                        {
                            Motion faultMotion = motionList.Find(f => f.IsAmpFault());
                            if (faultMotion != null)
                            {
                                ErrorManager.Instance().Report(ErrorCodeMotion.Instance.AmpFault, ErrorLevel.Error,
                                    faultMotion.Name, "Motion \"{0}\" Amp Fault", new object[] { faultMotion.Name });
                                continue;
                            }

                            motionEventHandlerList.ForEach(x =>
                            {
                                x.CheckStatus();
                            });
                            //motionList.ForEach(x =>
                            //{
                            //    for (int i = 0; i < x.NumAxis; i++)
                            //    {
                            //        if (x.IsAmpFault(i) == true)
                            //        {
                            //            ErrorManager.Instance().Report(ErrorSections.Motion, ErrorSubSectionMotion.AmpFault, ErrorLevel.Error,
                            //                ErrorSections.Motion.ToString(), ErrorSubSectionMotion.AmpFault.ToString(), String.Format("Amp Fault : Axis No = {0}", i.ToString()));
                            //            break;
                            //        }

                            //        if (x.IsServoOn(i))
                            //            isServoOn = true;

                            //        if (x.IsMoveDone(i) == false)
                            //            isMoving = true;
                            //    }

                            //    if (isMoving != moveState )
                            //    {
                            //        if (isMoving)
                            //        {
                            //            OnStartMove?.Invoke(x, -1);
                            //        }
                            //        else
                            //        {
                            //            OnMoveDone?.Invoke(x, -1);
                            //        }

                            //    }
                            //    else if (isServoOn != servoState )
                            //    {
                            //        if (isServoOn)
                            //        {
                            //            OnServoOn?.Invoke(x, -1);
                            //        }
                            //        else
                            //        {
                            //            OnServoOff?.Invoke(x, -1);
                            //        }
                            //    }
                            //});

                            //moveState = isMoving;
                            //servoState = isServoOn;
                        }

                        Thread.Sleep(100);
                    }
                    catch (System.ComponentModel.InvalidAsynchronousStateException exception) { LogHelper.Error(LoggerType.Error, "dd"); }
                }
            }
            finally { }
        }
    }
}
