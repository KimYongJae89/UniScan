using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UniScanWPF.Helper;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.UI;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MarginMeasurePos marginMeasurePos = new MarginMeasurePos();
            marginMeasurePos.BgBitmapSource = WPFImageHelper.LoadBitmapSource(@"C:\temp\topLightRot.bmp");
            marginMeasurePos.SubPosCollection.Add(new MarginMeasureSubPos(new Rect(100, 100, 100, 100)) { UseL = true, UseW = true });
            marginMeasurePos.SubPosCollection.Add(new MarginMeasureSubPos(new Rect(250, 100, 100, 100)) { UseL = false, UseW = true });
            marginMeasurePos.SubPosCollection.Add(new MarginMeasureSubPos(new Rect(100, 250, 100, 100)) { UseL = true, UseW = false });
            marginMeasurePos.SubPosCollection.Add(new MarginMeasureSubPos(new Rect(250, 250, 100, 100)) { UseL = false, UseW = false });

            MarginTeachWindow teach = new MarginTeachWindow();
            teach.Model = marginMeasurePos;
            teach.ShowDialog();
        }
    }
}
