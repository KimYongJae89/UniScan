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

namespace UniScanWPF.UI
{
    /// <summary>
    /// LogInWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogInWindow : Window
    {
        User user;
        public User User { get => user; set => user = value; }

        public LogInWindow()
        {
            InitializeComponent();
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
    }
}
