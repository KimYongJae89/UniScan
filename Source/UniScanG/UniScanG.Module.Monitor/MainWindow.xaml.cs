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
using UniScanG.Data.Model;
using UniScanG.Module.Monitor.Device;
using UniScanG.Module.Monitor.MachineIF;
using UniScanG.Module.Monitor.UI;
using WpfControlLibrary.Helper;

namespace UniScanG.Module.Monitor
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window, IMultiLanguageSupport
    {
        Page inspectPage;
        Page reportpage = null;
        Page settingPage = null;

        public MainWindow()
        {
            InitializeComponent();
            LocalizeHelper.AddListener(this);
        }

        public void Initialize()
        {
            this.Title = UniEye.Base.Settings.CustomizeSettings.Instance().ProgramTitle;

            this.inspectPage = new UI.Inspect.InspectPage();
            this.reportpage = new UI.Report.ReportPage();
            this.settingPage = new UI.Setting.SettingPage();
            this.statusDockPanel.Children.Add(new StatusControl());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frameMain.Navigate(inspectPage);

            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens.FirstOrDefault(f => !f.Primary);
            if (screen != null)
            {
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = screen.WorkingArea.X;
                this.Top = screen.WorkingArea.Y;
                this.Width= screen.WorkingArea.Width;
                this.Height= screen.WorkingArea.Height;
            }
            this.WindowState = WindowState.Maximized;

            DeviceController deviceController = SystemManager.Instance().DeviceController as DeviceController;
            MachineIfData machineIfData = deviceController.MachineIfMonitor.MachineIfData as MachineIfData;

            string modelName = string.IsNullOrEmpty(machineIfData.GET_MODEL) ? "NoModel" : machineIfData.GET_MODEL;
            string pasteName = string.IsNullOrEmpty(machineIfData.GET_MODEL) ? "NoPaste" : machineIfData.GET_PASTE;
            ModelDescription curModelDescription = SystemManager.Instance().CurrentModel?.ModelDescription;
            if (curModelDescription == null || curModelDescription.Name != modelName || curModelDescription.Paste != pasteName)
            {
                UniEye.Base.Data.ModelManager modelManager = SystemManager.Instance().ModelManager;
                ModelDescription modelDescription = modelManager.ModelDescriptionList.Find(f =>
                {
                    ModelDescription g = f as ModelDescription;
                    if (g == null)
                        return false;

                    return g.Name == modelName && g.Paste == pasteName;
                }) as ModelDescription;

                if (modelDescription == null)
                {
                    modelDescription = modelManager.CreateModelDescription() as ModelDescription;
                    modelDescription.Name = modelName;
                    modelDescription.Paste = pasteName;
                    modelDescription.IsTrained = true;

                    modelManager.AddModel(modelDescription);
                    modelManager.SaveModelDescription(modelDescription);
                }
                SystemManager.Instance().LoadModel(modelDescription);
                SystemManager.Instance().CurrentModel.Modified = true;
                SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
            }

            deviceController.InspectStarter.Start();

        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            frameMain.Navigate(settingPage);
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            frameMain.Navigate(reportpage);
        }

        private void BtnInspect_Click(object sender, RoutedEventArgs e)
        {
            //frameMain.Navigate(inspectPage);
            frameMain.Navigate(inspectPage);
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }
    }
}
