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
using UniEye.Base.Settings;
using UniScanG.Gravure.Data;
using UniScanG.Module.Monitor.Data;

namespace UniScanG.Module.Monitor.UI
{
    /// <summary>
    /// TopPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TopPage : Page, INotifyPropertyChanged
    {
        System.Timers.Timer timer;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Date => DateTime.Now.ToString("yyyy-MM-dd");
        public string Time => DateTime.Now.ToString("HH:mm:ss");

        public string ProgramTitle => CustomizeSettings.Instance().ProgramTitle;
        //public string Model => curProduction?.Name;
        //public string LotNo => curProduction?.LotNo;
        //public string Worker => DynMvp.Authentication.UserHandler.Instance().CurrentUser.Id;

        //Production curProduction = null;

        public TopPage()
        {
            InitializeComponent();

            InfoPanel.DataContext = new Production("",DateTime.Now, "", UniScanG.Data.RewinderZone.Unknowen, 0);

            SystemManager.Instance().ProductionManager.OnLotChanged += ProductionManager_OnLotChanged;

            this.timer = new System.Timers.Timer();
            this.timer.Interval = 950;
            this.timer.AutoReset = true;
            this.timer.Elapsed += Timer_Elapsed;

        }

        private void ProductionManager_OnLotChanged()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                InfoPanel.DataContext = SystemManager.Instance().ProductionManager.CurProduction;
            }));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LotNo"));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Paste"));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Worker"));
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
            }
            catch
            {

            }
            finally
            {

            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;

            this.timer.Start();
        }
    }
}
