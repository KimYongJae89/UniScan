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
using WpfControlLibrary.Helper;

namespace WpfControlLibrary.UI
{
    /// <summary>
    /// MessageBox.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    public partial class CustomMessageBox : Window, IMultiLanguageSupport
    {
        public CustomMessageBox()
        {
            InitializeComponent();

            LocalizeHelper.AddListener(this);
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        static CustomMessageBox _messageBox;
        static MessageBoxResult _result = MessageBoxResult.No;

        public static MessageBoxResult Show(string text, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            if (string.IsNullOrEmpty(caption))
                caption = UniEye.Base.Settings.CustomizeSettings.Instance().ProgramTitle;

            _messageBox = new CustomMessageBox { txtMsg = { Text = text }, MessageTitle = { Text = caption } };
            SetVisibilityOfButtons(button);
            if (icon != MessageBoxImage.None)
                _messageBox.SetImage(icon.ToString() + ".png");
            _messageBox.ShowDialog();
            return _result;
        }

        public static MessageBoxResult Show(string msg, MessageBoxButton button)
        {
            return Show(msg, string.Empty, button);
        }

        public static MessageBoxResult Show(string msg)
        {
            return Show(msg, string.Empty, MessageBoxButton.OK, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(string caption, string text)
        {
            return Show(text, caption, MessageBoxButton.OK, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(string text, string caption, MessageBoxButton button)
        {
            return Show(text, caption, button, MessageBoxImage.None);
        }

        private static void SetVisibilityOfButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    _messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    _messageBox.btnOk.Focus();
                    break;
                case MessageBoxButton.OKCancel:
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Focus();
                    break;
                case MessageBoxButton.YesNo:
                    _messageBox.btnOk.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.btnNo.Focus();
                    break;
                case MessageBoxButton.YesNoCancel:
                    _messageBox.btnOk.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Focus();
                    break;
                default:
                    break;
            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnOk)
                _result = MessageBoxResult.OK;
            else if (sender == btnYes)
                _result = MessageBoxResult.Yes;
            else if (sender == btnNo)
                _result = MessageBoxResult.No;
            else if (sender == btnCancel)
                _result = MessageBoxResult.Cancel;
            else
                _result = MessageBoxResult.None;
            _messageBox?.Close();
            _messageBox = null;
            this.Close();
        }
        private void SetImage(string imageName)
        {
            //string uri = string.Format("pack://application:,,,/Resources/{0}", imageName);
            string uri = string.Format("pack://application:,,,/WpfControlLibrary;component/Resources/{0}", imageName);
            var uriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
            img.Source = new BitmapImage(uriSource);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            LocalizeHelper.RemoveListener(this);
        }
    }
}
