using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Authentication;

namespace DynMvp.UI
{
    public partial class LogInForm : Form, IMultiLanguageSupport
    {
        User logInUser;
        public User LogInUser
        {
            get { return logInUser; }
            set { logInUser = value; }
        }

        public LogInForm(bool showLogoutButton = true)
        {
            InitializeComponent();

            StringManager.AddListener(this);

            this.buttonLogout.Visible = showLogoutButton;
#if DEBUG
            userId.Text = "developer";
            password.Text = "masterkey";
#endif

        }

        public void UpdateLanguage()
        {   
            StringManager.UpdateString(this);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            logInUser = UserHandler.Instance().GetUser(userId.Text, password.Text);
            if (logInUser == null)
            {
                MessageBox.Show(StringManager.GetString(this.GetType().FullName, "Invalid user id or password."), StringManager.GetString(this.GetType().FullName, "LogIn"));
                return;
            }

            DialogResult = DialogResult.OK;

            Close();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            logInUser = UserHandler.Instance().GetUser("op", "op");
            if (logInUser == null)
            {
                MessageBox.Show(StringManager.GetString(this.GetType().FullName, "Invalid user id or password."), StringManager.GetString(this.GetType().FullName, "LogIn"));
                return;
            }

            DialogResult = DialogResult.OK;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void LogInForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StringManager.RemoveListener(this);
        }

        private void userId_Enter(object sender, EventArgs e)
        {
            this.userId.SelectAll();
        }

        private void password_Enter(object sender, EventArgs e)
        {
            this.password.SelectAll();
        }
    }
}
