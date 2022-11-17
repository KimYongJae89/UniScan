using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniEye.Base.Data;
using WpfControlLibrary.Helper;

namespace UniScanG.Module.Monitor.UI.Inspect
{
    /// <summary>
    /// Status.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StateControl : UserControl, INotifyPropertyChanged, IOpStateListener, IInspectStateListener, IMultiLanguageSupport
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public float Distance => this.distance;
        float distance = 0;

        public string LastInspJudgment => LocalizeHelper.GetString(this.GetType().FullName, this.lastInspJudgment.ToString());
        Judgment lastInspJudgment;

        public string State
        {
            get
            {
                if (SystemState.Instance().OpState == OpState.Inspect)
                    return LocalizeHelper.GetString(this.GetType().FullName, SystemState.Instance().InspectState.ToString());
                return LocalizeHelper.GetString(this.GetType().FullName, SystemState.Instance().OpState.ToString());
            }
        }

        System.Timers.Timer timer;

        public StateControl()
        {
            InitializeComponent();

            this.timer = new System.Timers.Timer();
            this.timer.Interval = 500;
            this.timer.AutoReset = true;
            this.timer.Elapsed += Timer_Elapsed;

            SystemState.Instance().AddOpListener(this);
            SystemState.Instance().AddInspectListener(this);
            this.DataContext = this;
            LocalizeHelper.AddListener(this);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SystemManager.Instance().InspectRunner.OnProductInspected += InspectRunner_OnProductInspected;
            this.timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.distance = ((Device.DeviceController)SystemManager.Instance().DeviceController).MachineIfMonitor.MachineIfData.GET_PRESENT_POSITION;
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("Distance"));
        }

        private void InspectRunner_OnProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            this.lastInspJudgment = inspectionResult.Judgment;
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("LastInspJudgment"));
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("State"));
        }

        public void InspectStateChanged(InspectState curInspectState)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("State"));
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }
    }
}
