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
using UniScanWPF.Table.Operation.Operators;
using WpfControlLibrary.Helper;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// InspectParamWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InspectParamWindow : Window, IMultiLanguageSupport
    {
        InspectOperatorSettings inspectOperatorSettings;

        public InspectParamWindow()
        {
            InitializeComponent();
            LocalizeHelper.AddListener(this);

            this.DataContext = SystemManager.Instance().OperatorManager.InspectOperator.Settings;
            this.CommoneExtractGrid.DataContext = SystemManager.Instance().OperatorManager.ExtractOperator.Settings;

            this.etcDebugMode.DataContext = UniEye.Base.Settings.OperationSettings.Instance();
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LocalizeHelper.RemoveListener(this);
            SystemManager.Instance().OperatorManager.InspectOperator.Settings.Save();
            SystemManager.Instance().OperatorManager.ExtractOperator.Settings.Save();
        }
    }
}
