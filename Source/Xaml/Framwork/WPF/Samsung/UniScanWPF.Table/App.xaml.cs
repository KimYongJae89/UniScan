using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UniEye.Base.Settings;
using UniEye.Base.Util;
using UniScanWPF.Settings;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool bNew;
            Mutex mutex = new Mutex(true, "UniEye", out bNew);

            if (bNew)
            {
                try
                {
                    //TEST();
                    App.Current.MainWindow = new MainWindow();

                    ConfigHelper configHelper = new UniScanWPF.Table.Settings.ConfigHelper();
                    ApplicationHelper.LoadSettings();

                    string logConfigFile = String.Format("{0}\\log4net.xml", PathSettings.Instance().Config);
                    if (LogHelper.InitializeLogSystem(logConfigFile))
                    {
                        LogLevel logLevel = OperationSettings.Instance().LogLevel;
                        logLevel = LogLevel.Debug;
                        LogHelper.ChangeLevel(OperationSettings.Instance().LogLevel.ToString());
                    }
                    else
                    {
                        CustomMessageBox.Show(LocalizeHelper.GetString("Can't find log configuration file."), "UniScan", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    LogHelper.Info(LoggerType.StartUp, "Start Up");

                    InitStringTable();
                    ErrorManager.Instance().LoadErrorList(PathSettings.Instance().Config);
                    ApplicationHelper.InitAuthentication();

                    if (configHelper.Setup() == true)
                    {
                        App.Current.MainWindow.Show();
                        UpdateLoc(App.Current.MainWindow);
                      
                        LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");
                    }
                    else
                    {
                        Application.Current.Shutdown();
                    }
                }
                catch (Exception ex)
                {
                    string name = ex.GetType().Name;
                    string message = ex.Message;
                    CustomMessageBox.Show(string.Format("{0}::{1}{2}", name, Environment.NewLine, message), "UniScan", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
            }
            else
            {
                CustomMessageBox.Show(LocalizeHelper.GetString("Application already started."), "UniScan", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void UpdateLoc(Window window)
        {
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens.FirstOrDefault(f => !f.Primary);
            if (screen != null)
            {
                //window.WindowState = WindowState.Normal;
                window.WindowStartupLocation = WindowStartupLocation.Manual;
                window.Left = screen.WorkingArea.X;
                window.Top = screen.WorkingArea.Top;
                window.Width = screen.WorkingArea.Width;
                window.Height = screen.WorkingArea.Height;
            }
            window.WindowState = WindowState.Maximized;
        }

        public void InitStringTable()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init StringTable.");
            string localeCode = OperationSettings.Instance().GetLocaleCode();
            string configPath = PathSettings.Instance().Config;

            string fileName;
            if (localeCode == "")
                fileName = String.Format("LocalizeHelper.xml", configPath);
            else
                fileName = String.Format("LocalizeHelper_{1}.xml", configPath, localeCode);

            LocalizeHelper.Clear();
            LocalizeHelper.Load(Path.Combine(configPath, fileName));
        }


        private void TEST()
        {
            string xlsxFile = @"D:\UniScan\UniScanWPF\Margin\2022\08.xlsx";
           
            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Microsoft.Office.Interop.Excel.Workbooks workbooks = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel.Sheets sheets = null;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                System.Diagnostics.Debug.WriteLine(string.Format("ExcelApp.Version is {0}", excelApp.Version));

                workbooks = excelApp.Workbooks;
                workbook = workbooks.Open(xlsxFile);
                sheets = workbook.Worksheets;
                worksheet = sheets.get_Item("Sheet1");

                int iRow = 22;
                while (worksheet.Cells[iRow, 2].Value != null)
                    iRow++;

                string[] values = new string[9];
                values[0] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                values[1] = "ROLLNO";
                values[2] = "MODELNAME";

                values[3] = "33";
                values[4] = "44";
                values[5] = "55";
                values[6] = "66";
                values[7] = "77";
                values[8] = "OK";
                
                for (int i = 0; i < values.Length; i++)
                    worksheet.Cells[iRow, i + 2].Value = values[i];

    

                workbook.Save();

            }
            finally
            {
                excelApp?.Quit();
            }
        }

    }
}
