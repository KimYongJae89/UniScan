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
using UniEye.Base.MachineInterface;
using UniScanG.Module.Monitor.Device;
using UniScanG.Module.Monitor.MachineIF;

namespace UniScanG.Module.Monitor.UI
{
    /// <summary>
    /// StatusControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StatusControl : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        MachineIF.MachineIfData machineIfData = null;

        public Brush ColorConnected => machineIfData.IsConnected ? Brushes.LightGreen : Brushes.LightPink;
        public Brush ColorRunStillImage => machineIfData.GET_START_STILLIMAGE ? Brushes.LightGreen : Brushes.LightPink;
        public Brush ColorRewinder => machineIfData.GET_REWINDER_CUT ? Brushes.LightGreen : Brushes.LightPink;

        public float SVSPD => (float)Math.Round(machineIfData.GET_TARGET_SPEED_REAL, 1);
        public float PVSPD => (float)Math.Round(machineIfData.GET_PRESENT_SPEED_REAL, 1);
        public float PVPOS => (float)Math.Round(machineIfData.GET_PRESENT_POSITION, 1);
        public string Model => machineIfData.GET_MODEL;
        public string Lot => machineIfData.GET_LOT;
        public string Worker => machineIfData.GET_WORKER;


        public StatusControl()
        {
            UniScanG.MachineIF.MachineIfMonitor machineIfMonitor = ((DeviceController)SystemManager.Instance().DeviceController).MachineIfMonitor;
            machineIfMonitor.OnUpdated += Update;

            this.machineIfData = machineIfMonitor.MachineIfData as MachineIF.MachineIfData;

            InitializeComponent();

        }

        public void Update()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColorConnected"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColorRunStillImage"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColorRewinder"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SVSPD"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PVSPD"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PVPOS"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Model"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Lot"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Worker"));
        }

        private void Connected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!SystemManager.Instance().DeviceBox.MachineIf.IsVirtual)
                return;

            IVirtualMachineIf virtualMachineIf = SystemManager.Instance().DeviceBox.MachineIf as IVirtualMachineIf;
            virtualMachineIf.SetStateConnect(!machineIfData.IsConnected);
        }

        private void StillImage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!SystemManager.Instance().DeviceBox.MachineIf.IsVirtual)
                return;

            machineIfData.GET_START_STILLIMAGE = !machineIfData.GET_START_STILLIMAGE;
        }

        private void Rewinder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!SystemManager.Instance().DeviceBox.MachineIf.IsVirtual)
                return;

            machineIfData.GET_REWINDER_CUT = !machineIfData.GET_REWINDER_CUT;
            machineIfData.GET_PRESENT_POSITION = 0;
        }

        private void SVSPD_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!SystemManager.Instance().DeviceBox.MachineIf.IsVirtual)
                return;

            string input = ShowInputBox(machineIfData.GET_TARGET_SPEED.ToString());
            if (string.IsNullOrEmpty(input))
                return;

            float value;
            if (float.TryParse(input, out value))
                machineIfData.GET_TARGET_SPEED_REAL = value;
        }

        private void PVSPD_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!SystemManager.Instance().DeviceBox.MachineIf.IsVirtual)
                return;

            string input = ShowInputBox(machineIfData.GET_PRESENT_SPEED_REAL.ToString());
            if (string.IsNullOrEmpty(input))
                return;

            float value;
            if (float.TryParse(input, out value))
                machineIfData.GET_PRESENT_SPEED_REAL = value;
        }

        private void PVPOS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!SystemManager.Instance().DeviceBox.MachineIf.IsVirtual)
                return;

            string input = ShowInputBox(machineIfData.GET_PRESENT_POSITION.ToString());
            if (string.IsNullOrEmpty(input))
                return;

            float value;
            if (float.TryParse(input, out value))
                machineIfData.GET_PRESENT_POSITION = value;
        }

        private void Model_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!SystemManager.Instance().DeviceBox.MachineIf.IsVirtual)
                return;

            string input = ShowInputBox(machineIfData.GET_MODEL);
            if (string.IsNullOrEmpty(input))
                return;

            machineIfData.GET_MODEL = input;
        }

        private void Lot_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!SystemManager.Instance().DeviceBox.MachineIf.IsVirtual)
                return;

            string input = ShowInputBox(machineIfData.GET_LOT);
            if (string.IsNullOrEmpty(input))
                return;

            machineIfData.GET_LOT = input;
        }

        private void Worker_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!SystemManager.Instance().DeviceBox.MachineIf.IsVirtual)
                return;

            string input = ShowInputBox(machineIfData.GET_WORKER);
            if (string.IsNullOrEmpty(input))
                return;

            machineIfData.GET_WORKER = input;
        }

        private string ShowInputBox(string prev)
        {
            Tuple<MessageBoxResult, string> tuple = WpfControlLibrary.UI.CustomInputForm.Show("Input Value", "Message", MessageBoxImage.None, prev);
            if (tuple.Item1 == MessageBoxResult.Cancel)
                return null;

            return tuple.Item2;
        }
    }
}
