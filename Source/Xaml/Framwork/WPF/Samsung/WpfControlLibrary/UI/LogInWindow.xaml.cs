using DynMvp.Authentication;
using DynMvp.Base;
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
using WpfControlLibrary.Helper;

namespace WpfControlLibrary.UI
{
    /// <summary>
    /// LogInWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogInWindow : Window, IMultiLanguageSupport
    {
        User user;
        public User User { get => user; set => user = value; }

        public LogInWindow()
        {
            InitializeComponent();

            LocalizeHelper.AddListener(this);
#if DEBUG
            UserID.Text = "developer";
            Passward.Password = "masterkey";
#endif

        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnOk)
            {
                user = UserHandler.Instance().GetUser(UserID.Text, Passward.Password);
                if (user == null)
                {
                    CustomMessageBox.Show(StringManager.GetString("Invalid user id or password."), "UniEye", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DialogResult = true;
            }
            else if (sender == btnCancel)
                DialogResult = false;
            
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserID.Focus();
        }
        private void UserID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Passward.Focus();
        }

        private void Passward_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(btnOk, null);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            LocalizeHelper.RemoveListener(this);
        }
    }
}
