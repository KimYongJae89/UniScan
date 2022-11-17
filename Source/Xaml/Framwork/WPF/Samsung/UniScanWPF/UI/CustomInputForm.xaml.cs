using System;
using System.Collections.Generic;
using System.IO;
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

namespace UniScanWPF.UI
{
    /// <summary>
    /// MessageBox.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    public partial class CustomInputForm : Window
    {
        public CustomInputForm()
        {
            InitializeComponent();
        }

        static CustomInputForm inputForm;
        static MessageBoxResult _result = MessageBoxResult.No;
        static string inputText = null;
        public static Tuple<MessageBoxResult, string> Show(string caption, string msg, MessageBoxImage icon)
        {
            inputForm = new CustomInputForm { txtMsg = { Text = caption }, MessageTitle = { Text = msg } };
            inputForm.SetImage(icon.ToString() + ".png");
            inputForm.ShowDialog();
            
            return new Tuple<MessageBoxResult, string> (_result, inputText);
        }

        public static void Show(object stringManager, string v)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnOk)
                _result = MessageBoxResult.OK;
            else if (sender == btnCancel)
                _result = MessageBoxResult.Cancel;
            else
                _result = MessageBoxResult.None;

            inputText = txtInput.Text;

            inputForm.Close();
            inputForm = null;
        }
        private void SetImage(string imageName)
        {
            string uri = string.Format("pack://siteoforigin:,,,/Resources/{0}", imageName);
            var uriSource = new Uri(uri, UriKind.Absolute);
            img.Source = new BitmapImage(uriSource);
        }
    }
}
