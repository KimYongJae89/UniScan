using DynMvp.Base;
using DynMvp.Device.Device.MotionController;
using DynMvp.Devices.MotionController;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Operation;
using WpfControlLibrary.Helper;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// SettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingPage : Page, INotifyPropertyChanged, IMultiLanguageSupport
    {
        public SettingPage()
        {
            InitializeComponent();
            LocalizeHelper.AddListener(this);

            motionDockPanel.DataContext = SystemManager.Instance().DeviceController.RobotStage;
            scanRegionDockPanel.DataContext = SystemManager.Instance().OperatorManager.ScanOperator.Settings;
            lightTuneDockPanel.DataContext = SystemManager.Instance().OperatorManager.LightTuneOperator.Settings;
            marginDockPanel.DataContext = SystemManager.Instance().OperatorManager.InspectOperator.Settings;
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Motion_Find_Limit_Click(object sender, RoutedEventArgs e)
        {
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            if (axisHandler.IsMoveDone() == false)
                return;

            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() =>
            {
                TimeOutTimer timeOutTimer = new TimeOutTimer();
                axisHandler.AxisList.ForEach(f =>
                {
                    MovingParam movingParam = f.AxisParam.MovingParam.Clone();
                    movingParam.MaxVelocity /= 2;
                    f.ContinuousMove(movingParam, true);

                    timeOutTimer.Start(20000);
                    while (f.IsNegativeOn() == false)
                    {
                        if (timeOutTimer.TimeOut == true)
                        {
                            ErrorManager.Instance().Report(ErrorCodeMotion.Instance.CantFindNegLimit, ErrorLevel.Error,
                                axisHandler.Name, "Can't Find Neg Limit.", null);
                            return;
                        }

                        Thread.Sleep(10);
                    }
                    timeOutTimer.Reset();

                    f.StopMove();

                    Thread.Sleep(1000);
                    f.AxisParam.NegativeLimit = f.GetActualPulse() /*+ offset*/;

                    f.ContinuousMove(movingParam);

                    timeOutTimer.Start(20000);
                    while (f.IsPositiveOn() == false)
                    {
                        if (timeOutTimer.TimeOut == true)
                        {
                            ErrorManager.Instance().Report(ErrorCodeMotion.Instance.CantFindPosLimit, ErrorLevel.Error,
                                axisHandler.Name, "Can't Find Pos Limit.", null);
                            return;
                        }

                        Thread.Sleep(10);
                    }

                    f.StopMove();
                    Thread.Sleep(1000);

                    f.AxisParam.PositiveLimit = f.GetActualPulse() /*- offset*/;
                    f.Move(0);
                });
                SystemManager.Instance().DeviceBox.AxisConfiguration.SaveConfiguration();
                OnPropertyChanged("AxisList");
            });
        }

        private void MotionHome_Click(object sender, RoutedEventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            if (MachineOperator.MoveHome(0, null, cancellationTokenSource) == false)
                MessageForm.Show(null, "Homing fail !!");
        }

        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue) == false)
            {
                SystemManager.Instance().DeviceBox.AxisConfiguration.SaveConfiguration();
                SystemManager.Instance().OperatorManager.ScanOperator.Settings.Save();
                SystemManager.Instance().OperatorManager.LightTuneOperator.Settings.Save();
                InfoBox.Instance.UpdateRegion(null);
            }
        }
    }
}
