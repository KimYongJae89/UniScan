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
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.Device;
using UniScanWPF.Screen.PinHoleColor.Inspect;
using UniScanWPF.Screen.PinHoleColor.PinHole.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.UI
{
    /// <summary>
    /// StatusStrip.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public partial class StatusStrip : UserControl, IBufferViewer
    {
        public StatusStrip()
        {
            InitializeComponent();
        }

        public void UpdateBuffers(List<InspectSet> inspectSetList)
        {
            if (inspectSetList.Count > 0)
                PinHoleBuffer1.DataContext = inspectSetList[0];

            if (inspectSetList.Count > 1)
                PinHoleBuffer2.DataContext = inspectSetList[1];

            if (inspectSetList.Count > 2)
                ColorBuffer.DataContext = inspectSetList[2];
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PinHoleCombiner.DataContext = ResultCombiner.Instance();
            IoStatus.DataContext = new IoMonitor();
            SaveBuffer.DataContext = ResultExportManager.Instance();
        }
    }
}
