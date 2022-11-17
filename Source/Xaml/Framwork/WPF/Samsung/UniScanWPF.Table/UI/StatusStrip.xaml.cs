using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using UniScanWPF.Table.Data;
using WpfControlLibrary.Helper;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// StatusStrip.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public partial class StatusStrip : UserControl, IMultiLanguageSupport
    {
        public StatusStrip()
        {
            InitializeComponent();

            LocalizeHelper.AddListener(this);

            this.IOStatus.DataContext = SystemManager.Instance().MachineObserver.IoBox;
            this.ModelStatus.DataContext = InfoBox.Instance;
            this.VersionBuildStatus.DataContext = VersionHelper.Instance();
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void IoLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            return;
            Label label = (Label)sender;

            IoPort ioPort = null;
            bool value = false;
            string content = label.Content.ToString();
            switch(content)
            {
                //case "POW":
                //    ioPort = SystemManager.Instance().MachineObserver.IoBox.InPower;
                //    break;
                //case "EMG":
                //    ioPort = SystemManager.Instance().MachineObserver.IoBox.InEmergency;
                //    break;
                case "FAN":
                    ioPort = SystemManager.Instance().MachineObserver.IoBox.OutFan;
                    value = SystemManager.Instance().MachineObserver.IoBox.OutOnFan;
                    break;

                //case "1":
                //    ioPort = SystemManager.Instance().MachineObserver.IoBox.InDoor1;
                //    break;
                //case "2":
                //    ioPort = SystemManager.Instance().MachineObserver.IoBox.InDoor2;
                //    break;
                case "Lock":
                    ioPort = SystemManager.Instance().MachineObserver.IoBox.OutDoorLock;
                    value = SystemManager.Instance().MachineObserver.IoBox.OutOnDoorLock;
                    break;

                case "RED":
                    ioPort = SystemManager.Instance().MachineObserver.IoBox.OutLampRed;
                    value = SystemManager.Instance().MachineObserver.IoBox.OutOnLampRed;
                    break;
                case "YLW":
                    ioPort = SystemManager.Instance().MachineObserver.IoBox.OutLampYellow;
                    value = SystemManager.Instance().MachineObserver.IoBox.OutOnLampYellow;
                    break;
                case "GRN":
                    ioPort = SystemManager.Instance().MachineObserver.IoBox.OutLampGreen;
                    value = SystemManager.Instance().MachineObserver.IoBox.OutOnLampGreen;
                    break;
            }

            if (ioPort == null)
                return;

            SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(ioPort, !value);
        }
    }
}
