using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;
using UniScanS.Common.Util;
using System.Drawing;
using DynMvp.Authentication;
using DynMvp.UI;
using DynMvp.Base;

namespace UniScanS.UI.Etc
{
    public partial class UserPanel : UserControl, IUserHandlerListener
    {
        public UserPanel()
        {
            InitializeComponent();
            
            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            UserHandler.Instance().AddListener(this);
            UserChanged();
        }

        public void UserChanged()
        {
            switch (UserHandler.Instance().CurrentUser.Id)
            {
                case "op":
                    userName.Text = StringManager.GetString(this.GetType().FullName, "Operator");
                    userName.ForeColor = Color.Green;
                    break;
                case "master":
                    userName.Text = StringManager.GetString(this.GetType().FullName, "Master");
                    userName.ForeColor = Color.Blue;
                    break;
                case "developer":
                    userName.Text = StringManager.GetString(this.GetType().FullName, "Developer");
                    userName.ForeColor = Color.Red;
                    break;
            }
        }

        private void userName_Click(object sender, EventArgs e)
        {
            LogInForm loginForm = new LogInForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
                UserHandler.Instance().CurrentUser = loginForm.LogInUser;
        }
    }
}
