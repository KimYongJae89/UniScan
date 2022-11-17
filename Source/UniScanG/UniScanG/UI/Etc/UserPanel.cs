using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Common.Util;
using System.Drawing;
using DynMvp.Authentication;
using DynMvp.UI;
using DynMvp.Base;

namespace UniScanG.UI.Etc
{
    public partial class UserPanel : UserControl, IUserHandlerListener, IMultiLanguageSupport
    {
        public UserPanel()
        {
            InitializeComponent();
            
            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            UserHandler.Instance().AddListener(this);
            UserChanged();
        }

        public void UpdateLanguage()
        {
            UserChanged();
        }

        public void UserChanged()
        {
            User curUser = UserHandler.Instance().CurrentUser;
            if (curUser == null)
                return;

            userName.Text = StringManager.GetString(this.GetType().FullName, curUser.UserType.ToString());
            userName.ForeColor = curUser.IsMasterAccount ? Color.Red : Color.Green;
        }

        private void userName_Click(object sender, EventArgs e)
        {
            LogInForm loginForm = new LogInForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
                UserHandler.Instance().CurrentUser = loginForm.LogInUser;
        }
    }
}
