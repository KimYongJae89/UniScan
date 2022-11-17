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

namespace UniScanWPF.Table.UI
{
    public enum PageType
    {
        NULL = -1, Inspect, Model, Teach, Setting, Report, History
    }

    /// <summary>
    /// MainPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPage : Page
    {
        //public delegate void OnNavigatedDelegate(PageType pageType);
        //public OnNavigatedDelegate OnNavigated = null;

        InspectPage inspectPage;
        TeachPage teachPage;
        InfoPage infoPage;
        ModelPage modelPage;
        SettingPage settingPage;

        public MainPage()
        {
            InitializeComponent();

            inspectPage = new InspectPage();
            teachPage = new TeachPage();
            infoPage = new InfoPage();
            modelPage = new ModelPage();
            settingPage = new SettingPage();

            mainFrame.Navigate(modelPage);

            //SystemManager.Instance().MainPage = this;
            //InfoFrame.Navigate(infoPage);
        }

        public void Navigate(PageType pageType)
        {
            switch (pageType)
            {
                case PageType.Inspect:
                    mainFrame.Navigate(inspectPage);
                    break;
                case PageType.Model:
                    mainFrame.Navigate(modelPage);
                    break;
                case PageType.Teach:
                    mainFrame.Navigate(teachPage);
                    break;
                case PageType.Setting:
                    mainFrame.Navigate(settingPage);
                    break;
            }

            //OnNavigated?.Invoke(pageType);
        }
    }
}