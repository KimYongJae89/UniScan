using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using DynMvp.UI;
using DynMvp.Authentication;
using DynMvp.Base;
using UniEye.Base.Util;
using UniEye.Base.Settings;

namespace UserManager
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            PathSettings.Instance().Load();
            OperationSettings.Instance().Load();

            ApplicationHelper.InitStringTable();

            TaskAuthManager.Instance().LoadTaskAuthTable("../Config/taskTable.xml");

            UserHandler.Instance().Initialize("../Config/UserList.dat");

            LogInForm logInForm = new LogInForm();
            if (logInForm.ShowDialog() == DialogResult.OK)
            {
                if (logInForm.LogInUser.IsSuperAccount != true)
                {
                    MessageBox.Show("Permission is invalid.");
                    return;
                }

                TaskAuthManager.Instance().DoTask("Admin", logInForm.LogInUser, false,
                delegate ()
                {
                    MainForm mainForm = new MainForm();
                    mainForm.UserHandler = UserHandler.Instance();
                    Application.Run(mainForm);
                }
                );
            }
        }
    }
}
