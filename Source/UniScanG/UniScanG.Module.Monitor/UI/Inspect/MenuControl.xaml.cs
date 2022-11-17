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
using UniScanG.Module.Monitor.Device;
using WpfControlLibrary.Helper;

namespace UniScanG.Module.Monitor.UI.Inspect
{
    /// <summary>
    /// MenuControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MenuControl : UserControl,IMultiLanguageSupport
    {
        public MenuControl()
        {
            InitializeComponent();
            LocalizeHelper.AddListener(this);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ((DeviceController)SystemManager.Instance().DeviceController).InspectStarter.StartMode = Operation.StartMode.Force;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            ((DeviceController)SystemManager.Instance().DeviceController).InspectStarter.StartMode = Operation.StartMode.Stop;
        }

        private void AutoButton_Click(object sender, RoutedEventArgs e)
        {
            ((DeviceController)SystemManager.Instance().DeviceController).InspectStarter.StartMode = Operation.StartMode.Auto;
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }
    }
}
