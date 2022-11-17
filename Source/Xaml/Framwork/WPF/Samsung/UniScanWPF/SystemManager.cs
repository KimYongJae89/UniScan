using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UniScanWPF.UI;

namespace UniScanWPF
{
    public class SystemManager : UniEye.Base.SystemManager
    {
        UniScanWPF.UI.WPFUiChanger wpfUiChanger;
        public WPFUiChanger WpfUiChanger { get => wpfUiChanger; set => wpfUiChanger = value; }
        
        public new static SystemManager Instance()
        {
            return (SystemManager)_instance;
        }
        
        public void Close()
        {
            inspectRunner.Dispose();
            Release();

            App.Current.MainWindow.Close();

            Application.Current.Shutdown();
        }
    }
}