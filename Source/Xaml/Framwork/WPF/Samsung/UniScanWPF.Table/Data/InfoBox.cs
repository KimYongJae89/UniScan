using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using UniEye.Base.Data;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Operation.Operators;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.Helper;

namespace UniScanWPF.Table.Data
{
    public class InfoBox : INotifyPropertyChanged
    {
        static InfoBox instance;
        public static InfoBox Instance
        {
            get
            {
                if (instance == null)
                    instance = new InfoBox();

                return instance;
            }
        }

        string stateStr;
        public string StateStr
        {
            get => LocalizeHelper.GetString(this.GetType().FullName, stateStr);
            set
            {
                stateStr = value;
                OnPropertyChanged("StateStr");
            }
        }

        int progressPos;
        public int ProgressPos
        {
            get => progressPos;
            set
            {
                progressPos = value;
                OnPropertyChanged("ProgressPos");
            }
        }

        DateTime lastStartTime;
        public DateTime LastStartTime
        {
            get => lastStartTime;
            set
            {
                lastStartTime = value;
                OnPropertyChanged("LastStartTime");
            }
        }
        
        public bool Startable
        {
            get
            {
                if (SystemManager.Instance().CurrentModel == null)
                    return false;

                return !SystemManager.Instance().OperatorManager.IsRun;
            }
        }

        public bool Stopable
        {
            get
            {
                if (SystemManager.Instance().CurrentModel == null)
                    return false;

                return SystemManager.Instance().OperatorManager.IsRun;
            }
        }

        public bool Inspectable
        {
            get
            {
                if (SystemManager.Instance().CurrentModel == null)
                    return false;

                if (SystemManager.Instance().OperatorManager.TeachOperator.OperatorState != Operation.OperatorState.Idle)
                    return false;

                return SystemManager.Instance().CurrentModel.IsTaught();
            }
        }

        public bool Teachable
        {
            get
            {
                if (SystemManager.Instance().CurrentModel == null)
                    return false;

                if (SystemManager.Instance().OperatorManager.InspectOperator.OperatorState != Operation.OperatorState.Idle)
                    return false;

                return true;
            }
        }

        public bool ModelSelectable
        {
            get
            {
                return !SystemManager.Instance().OperatorManager.IsRun;
            }
        }

        public Model CurrentModel { get => SystemManager.Instance().CurrentModel; }
        public Production CurrentProduction { get => SystemManager.Instance().ProductionManager.CurProduction; }


        RectangleF dispScanRegion;
        RectangleF dispRobotRegion;
        PointF dispHomePos;

        public RectangleF DispRobotRegion { get => dispRobotRegion; }
        public PointF DispHomePos { get => dispHomePos; }
        public RectangleF DispScanRegion { get => dispScanRegion; }
        public SolidColorBrush CurMarkBrush { get => SystemManager.Instance().MachineObserver.MotionBox.OnMoveBrush; }
        public System.Windows.Point CurMarkPos
        {
            get => new System.Windows.Point(
                DispHomePos.X - (SystemManager.Instance().MachineObserver.MotionBox.ActualPositionX / DeveloperSettings.Instance.Resolution),
                DispHomePos.Y + (SystemManager.Instance().MachineObserver.MotionBox.ActualPositionY / DeveloperSettings.Instance.Resolution)
                );
        }

        public double ImageCanvasScale { get => 1 / Operator.DispResizeRatio; }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public InfoBox()
        {
            //StateStr = LocalizeHelper.GetString("Idle");
        }

        public void ProductionChanged()
        {
            OnPropertyChanged("CurrentProduction");
        }

        public void ModelChanged()
        {
            OnPropertyChanged("Inspectable");
            OnPropertyChanged("Teachable");
            OnPropertyChanged("CurrentModel");
            OperatorStateChanged();
        }

        public void OperatorStateChanged()
        {
            OnPropertyChanged("Startable");
            OnPropertyChanged("RobotLabelVisibility");
            OnPropertyChanged("Stopable");
            OnPropertyChanged("Teachable");
            OnPropertyChanged("Inspectable");
            OnPropertyChanged("ModelSelectable");
            OnPropertyChanged("CurrentProduction");
        }


        internal void UpdateRegion(ScanOperatorSettings scanOperatorSettings)
        {
            if (scanOperatorSettings == null)
                scanOperatorSettings = SystemManager.Instance().OperatorManager?.ScanOperator.Settings;
            if (scanOperatorSettings == null)
                return;

            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            if (axisHandler != null)
            {
                float res = DeveloperSettings.Instance.Resolution;
                AxisPosition[] limitPos = axisHandler.GetLimitPos();
      

                dispRobotRegion = RectangleF.FromLTRB(
                    0,
                    0,
                    limitPos[1].Position[0] - limitPos[0].Position[0],
                    limitPos[1].Position[1] - limitPos[0].Position[1]);

                dispHomePos = new PointF(limitPos[1].Position[0], -limitPos[0].Position[1]);

                dispScanRegion = RectangleF.FromLTRB(
                    dispHomePos.X - scanOperatorSettings.Dst.X,
                    dispHomePos.Y + scanOperatorSettings.Src.Y,
                    dispHomePos.X - scanOperatorSettings.Src.X,
                    dispHomePos.Y + scanOperatorSettings.Dst.Y);

                dispRobotRegion = DrawingHelper.Mul(dispRobotRegion, 1 / res);
                dispScanRegion = DrawingHelper.Mul(dispScanRegion, 1 / res);
                dispHomePos = DrawingHelper.Mul(dispHomePos, 1 / res);
            }

            OnPropertyChanged("DispRobotRegion");
            OnPropertyChanged("DispHomePos");
            OnPropertyChanged("DispScanRegion");
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
