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
using System.Windows.Shapes;
using UniEye.Base.Data;
using UniScanWPF.Screen.PinHoleColor.Color.Settings;
using UniScanWPF.Screen.PinHoleColor.PinHole.Settings;

namespace UniScanWPF.Screen.PinHoleColor.UI
{
    /// <summary>
    /// SettingWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PinHoleColor.PinHole.Settings.PinHoleSettings.Instance().Save();
            PinHoleColor.Color.Settings.ColorSettings.Instance().Save();

            if (SystemManager.Instance().InspectRunner.IsRunning == true)
                SystemState.Instance().SetWait();
            else
                SystemState.Instance().SetIdle();

            e.Cancel = true;
            this.Hide();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                SystemState.Instance().SetTeach();
                this.PinHoleSettings.DataContext = PinHoleColor.PinHole.Settings.PinHoleSettings.Instance();
                this.ColorSettings.DataContext = PinHoleColor.Color.Settings.ColorSettings.Instance();
            }
            else
            {
                this.PinHoleSettings.DataContext = null;
                this.ColorSettings.DataContext = null;
            }
        }
    }
}
