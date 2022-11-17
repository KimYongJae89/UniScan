using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Authentication;
using DynMvp.Base;

namespace UserManager
{
    public partial class EditUserForm : Form
    {
        User user;
        public User User
        {
            get { return user; }
            set { user = value; }
        }

        UserList userList;
        public UserList UserList
        {
          set { userList = value; }
        }

        public EditUserForm(UserList userList)
        {
            InitializeComponent();

            this.userList = userList;
        }

        private void EditUserForm_Load(object sender, EventArgs e)
        {
            if (user == null)
            {
                this.Text = "New User";
                this.userId.Text = "NewUser";
            }
            else
            {
                this.userId.Text = user.Id;
                userId.Enabled = false;

                UserType userType = (UserType)user.UserType;

                if (userType == UserType.Admin)
                    buttonAdmin.Checked = true;
                else if (userType == UserType.Operator)
                    buttonOperator.Checked = true;
                else if (userType == UserType.Maintrance)
                    buttonMain.Checked = true;
            }
            
            /*TaskItemTable taskTable = TaskAuthManager.Instance().TaskItemTable;
            foreach (TaskItem task in taskTable)
            {
                int index = taskList.Items.Add(task);

                if (user != null && user.permissionControl.IsGranted(task.Code) == true)
                {
                    taskList.SetItemChecked(index, true);
                }
            }*/
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (userId.Text == "")
            {
                MessageBox.Show(StringManager.GetString("ID is null. Please input the ID"));
                return;
            }

            if (password.Text == "")
            {
                MessageBox.Show(StringManager.GetString("Password is null. Please input password"));
                return;
            }

            if (password.Text != retrypePassword.Text)
            {
                MessageBox.Show(StringManager.GetString("Check the password"));
                return;
            }

            if (password.Text.Count() < 4)
            {
                MessageBox.Show(StringManager.GetString("password length is Invalid. The password length is higher than 4 character"));
                return;
            }

            if (password.Text.Count() > 50)
            {
                MessageBox.Show(StringManager.GetString("password length is Invalid. The password length is lower than 50 character"));
                return;
            }

            UserType type = UserType.Admin;
            if (buttonAdmin.Checked == true)
                type = UserType.Admin;
            else if (buttonMain.Checked == true)
                type = UserType.Maintrance;
            else if (buttonOperator.Checked == true)
                type = UserType.Operator;

            if (user == null)
            {
                foreach(User tempUser in userList)
                {
                    if (tempUser.Id == userId.Text)
                    {
                        MessageBox.Show(StringManager.GetString("Alreay registered ID."));
                        return;
                    }
                }

                user = new User(userId.Text, password.Text, type);
            }
            else
            {
                if (password.Text != "")
                    user.PasswordHash = User.GetPasswordHash(password.Text);
                user.UserType = type;
            }

            user.permissionControl.Clear();
            

            
            
            TaskItem task = new TaskItem((int)type, type.ToString());

            user.permissionControl.AddGranted(task.Code);
            
            DialogResult = DialogResult.OK;
            
            Close();
        }

        private bool IsDataValid()
        {
            if (userId.Text == "")
                return false;

            if (user == null)
            {
                if (password.Text == "" || (password.Text != retrypePassword.Text))
                    return false;
            }
            else
            {
                if (password.Text != retrypePassword.Text)
                    return false;
            }

            return true;
        }
    }
}
