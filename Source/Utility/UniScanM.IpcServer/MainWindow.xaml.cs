using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UniScanM.IpcServer
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        MachineState machineState = null;
        IpcServerChannel ipcServerChannel = null;
        Timer timer = null;
        Brush[] brushes = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.brushes = new Brush[] { new SolidColorBrush(Colors.LightGreen), new SolidColorBrush(Colors.LightPink) };

            this.ipcServerChannel = new IpcServerChannel("remote");
            ChannelServices.RegisterChannel(this.ipcServerChannel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(MachineState), "GAE", WellKnownObjectMode.Singleton);

            this.machineState = new MachineState();
            this.machineState.Reset();

            this.timer = new Timer();
            this.timer.Interval = 500;
            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                lblConnect.Content = machineState.IsConnected ? "Connected" : "Disconnected";
                lblConnect.Background = this.brushes[machineState.IsConnected ? 0 : 1];

                lblPinhole.Content = machineState.PinholeOnStart ? "On" : "Off";
                lblPinhole.Background = this.brushes[machineState.PinholeOnStart ? 0 : 1];

                lblRVMS.Content = machineState.RvmsOnStart ? "On" : "Off";
                lblRVMS.Background = this.brushes[machineState.RvmsOnStart ? 0 : 1];

                lblColorSensor.Content = machineState.ColorSensorOnStart ? "On" : "Off";
                lblColorSensor.Background = this.brushes[machineState.ColorSensorOnStart ? 0 : 1];

                lblEDMS.Content = machineState.EdmsOnStart ? "On" : "Off";
                lblEDMS.Background = this.brushes[machineState.EdmsOnStart ? 0 : 1];

                lblStillImage.Content = machineState.StillImageOnStart ? "On" : "Off";
                lblStillImage.Background = this.brushes[machineState.StillImageOnStart ? 0 : 1];

                lblRewinder.Content = machineState.RewinderCut ? "On" : "Off";
                lblRewinder.Background = this.brushes[machineState.RewinderCut ? 0 : 1];

                lblSvSpd.Content = machineState.SpSpeed.ToString();

                lblPvSpd.Content = machineState.PvSpeed.ToString();

                lblPvPos.Content = machineState.PvPosition.ToString();

                lblLot.Content = machineState.LotNo.ToString();

                lblModel.Content = machineState.ModelName.ToString();

                lblWorker.Content = machineState.Worker.ToString();
            }));
        }
    }
}
