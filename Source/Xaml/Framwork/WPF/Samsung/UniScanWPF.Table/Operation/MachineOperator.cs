using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using UniScanWPF.Table.Device;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation
{
    public abstract class MachineOperator : Operator
    {
        const int interval = 100;
        const int stableNum = 5;
        const float commandActualDiffLimit = 2;

        protected AxisHandler axisHandler;
        protected IoPort ioLock = null;
        
        public MachineOperator() : base()
        {
            axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            PortMap portMap = SystemManager.Instance().DeviceBox.PortMap as PortMap;
            if (portMap != null)
                ioLock = portMap.GetOutPort(PortMap.OutPortName.OutDoorLock);
        }

        public static bool IsHomeOK()
        {
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            if (axisHandler == null)
                return false;

            bool isHomeDone = axisHandler.IsAllHomeDone();
            if (isHomeDone == false)
                return false;

            AxisPosition actualPosition = axisHandler.GetActualPos();
            AxisPosition commandPosition = axisHandler.GetCommandPos();
            bool done = true;
            for (int i = 0; i < actualPosition.NumAxis; i++)
                done &= (Math.Abs(actualPosition.Position[i] - commandPosition.Position[i]) <= commandActualDiffLimit);

            return done;
        }

        public static bool MoveHome(int delay, List<IoPort> ioPortList, CancellationTokenSource cancellationTokenSource)
        {
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            if (axisHandler == null)
                return false;

            if (axisHandler.IsMoveOn().ToList().Exists(onMove => onMove == true))
                return false;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                SimpleProgressWindow homeMoveWindow = new SimpleProgressWindow(LocalizeHelper.GetString("Move Home"));
                homeMoveWindow.Topmost = true;
                homeMoveWindow.Show(() =>
                {
                    if (ioPortList != null)
                        ioPortList.ForEach(ioPort => SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioPort, true));
                    
                    Thread.Sleep(delay);

                    if (cancellationTokenSource.IsCancellationRequested)
                        return;

                    axisHandler.StartMultipleHomeMove(cancellationTokenSource);
                    WaitHomeMoveDone(axisHandler, cancellationTokenSource);

                    axisHandler.AxisList.ForEach(axis => axis.SetPosition(0));

                    if (ioPortList != null)
                        ioPortList.ForEach(ioPort => SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ioPort, false));

                }, cancellationTokenSource);
            }));


            return !cancellationTokenSource.IsCancellationRequested;
        }

        protected static void WaitHomeMoveDone(AxisHandler axisHandler, CancellationTokenSource cancellationTokenSource)
        {
            int curStableNum = 0;
            float[] prevPos = axisHandler.GetActualPos().Position;
            while (cancellationTokenSource.Token.IsCancellationRequested == false)
            {
                if (axisHandler.IsAllHomeDone() == false)
                    continue;

                float[] curPos = axisHandler.GetActualPos().Position;
                bool isStable = true;
                for (int i = 0; i < axisHandler.NumAxis; i++)
                {
                    if (prevPos[i] != curPos[i])
                        isStable = false;
                }
                
                if (isStable == true)
                    curStableNum++;
                else
                    curStableNum = 0;

                if (curStableNum >= stableNum)
                    break;

                prevPos = curPos;

                Thread.Sleep(interval);
            }
        }

        protected void WaitMoveDone()
        {
            axisHandler.WaitMoveDone(cancellationTokenSource);

            while (cancellationTokenSource.IsCancellationRequested == false)
            {
                AxisPosition actualPosition = axisHandler.GetActualPos();
                AxisPosition commandPosition = axisHandler.GetCommandPos();

                bool done = true;
                for (int i = 0; i < actualPosition.NumAxis; i++)
                {
                    if (Math.Abs(actualPosition.Position[i] - commandPosition.Position[i]) > commandActualDiffLimit)
                        done = false;
                }

                if (done)
                    break;

                System.Threading.Thread.Sleep(interval);
            }
        }
    }
    
    public abstract class MachineOperatorSettings : OperatorSettings
    {
        static protected float velocity = 150000;

        [CatecoryAttribute("Machine"), NameAttribute("Velocity")]
        public float Velocity
        {
            get => velocity;
            set
            {
                if (velocity != value)
                {
                    velocity = value;
                    OnPropertyChanged("Velocity");
                }
            }
        }

        public override void Load(XmlElement xmlElement)
        {
            velocity = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "Velocity", "150000"));
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Velocity", velocity.ToString());
        }
    }
}
