using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Authentication;

namespace UserManager
{
    public partial class MainForm : Form
    {
        UserHandler userHandler;
        public UserHandler UserHandler
        {
            get { return userHandler; }
            set { userHandler = value; }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (User user in userHandler.UserList)
            {
                //user.permissionControl.GetPermissions();

                int index = dataGridUser.Rows.Add(user.Id, user.UserType);
                dataGridUser.Rows[index].Tag = user;
            }
        }

        private void toolStripButtonAddUser_Click(object sender, EventArgs e)
        {
            EditUserForm editUserForm = new EditUserForm(userHandler.UserList);
            if (editUserForm.ShowDialog(this) == DialogResult.OK)
            {
                User user = editUserForm.User;

                UserType userType = (UserType)user.permissionControl.PermisionList[0].Code;
                user.permissionControl.PermisionList[0].Code = (int)userType;
                
                int index = dataGridUser.Rows.Add(user.Id, ((UserType)user.permissionControl.PermisionList[0].Code).ToString());
                dataGridUser.Rows[index].Tag = user;

                userHandler.UserList.AddUser(user);
            }
        }

        private void toolStripButtonEditUser_Click(object sender, EventArgs e)
        {
            EditUser();
        }

        private void EditUser()
        {
            if (dataGridUser.SelectedRows.Count == 0)
                return;

            int selIndex = dataGridUser.SelectedRows[0].Index;

            EditUserForm editUserForm = new EditUserForm(userHandler.UserList);
            editUserForm.User = (User)dataGridUser.Rows[selIndex].Tag;
            if (editUserForm.ShowDialog(this) == DialogResult.OK)
            {
                User user = editUserForm.User;
                UserType userType = user.UserType;
                //user.permissionControl.PermisionList[0].Code = (int)userType;
                dataGridUser.Rows[selIndex].Cells[1].Value = userType.ToString();
            }
        }

        private void toolStripButtonDeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridUser.SelectedRows.Count == 0)
                return;

            int selIndex = dataGridUser.SelectedRows[0].Index;
            User user = (User)dataGridUser.Rows[selIndex].Tag;

            if (MessageBox.Show(String.Format("[{0} 사용자를 삭제 하시겠습니까?]", user.Id), "Delete User", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dataGridUser.Rows.RemoveAt(selIndex);
                userHandler.UserList.RemoveUser(user);
            }
        }

        private void dataGridUser_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditUser();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            userHandler.SaveUserList();
        }
    }
}
