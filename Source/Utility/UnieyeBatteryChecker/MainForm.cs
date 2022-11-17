using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnieyeBatteryChecker
{
    public partial class MainForm : Form
    {
        private Timer batteryCheckTimer;

        private string pathFileName = "path.txt";

        private string exeFileFullName;
        private string exeFileName;

        private bool shutdownFlag = true;

        public MainForm()
        {
            InitializeComponent();

            batteryCheckTimer = new Timer();
            batteryCheckTimer.Interval = 500;
            batteryCheckTimer.Tick += batteryCheckTimer_Tick;
        }

        private void batteryCheckTimer_Tick(object sender, EventArgs e)
        {
            PowerLineStatus powerLineStatus = SystemInformation.PowerStatus.PowerLineStatus;

            if (powerLineStatus == PowerLineStatus.Online)
            {
                Process[] processes = Process.GetProcessesByName(exeFileName);

                if (processes.Length < 1)
                {
                    if (File.Exists(exeFileFullName) == true)
                        Process.Start(exeFileFullName);
                }

                shutdownFlag = true;
            }
            else
            {
                Process[] processes = Process.GetProcessesByName(exeFileName);

                if (processes.Length >= 1)
                {
                    foreach(Process process in processes)
                    {
                        if (process.ProcessName.StartsWith(exeFileName))
                        {
                            process.Kill();
                        }
                    }
                }

                if (shutdownFlag == true)
                {
                    Process.Start("shutdown.exe", "-s -t 60 -f");

                    shutdownFlag = false;

                    if (MessageBox.Show(null, "PC will shutdown after 1 minute", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                    {
                        Process.Start("shutdown.exe", "-a");
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(pathFileName) != true)
            {
                using (FileStream fileStream = File.Create(pathFileName)) { }

                File.AppendAllText(pathFileName, "C:\\UniScanM.exe");
            }

            string pathFileFullName = File.ReadAllText(pathFileName);

            txtPath.Text = pathFileFullName;
            exeFileFullName = pathFileFullName;

            string[] tempStr = pathFileFullName.Split('.', '\\');
            exeFileName = tempStr[tempStr.Length - 2];

            batteryCheckTimer.Start();
        }

        private void btnPathSetting_Click(object sender, EventArgs e)
        {
            batteryCheckTimer.Stop();

            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Multiselect = false;
            fileDialog.Filter = "응용프로그램 (*.exe) | *.exe";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = fileDialog.FileName;
                exeFileFullName = fileDialog.FileName;

                string[] tempStr = fileDialog.FileName.Split('.', '\\');
                exeFileName = tempStr[tempStr.Length - 2];

                File.Delete(pathFileName);
                using (FileStream fileStream = File.Create(pathFileName)) { }
                File.AppendAllText(pathFileName, exeFileFullName);
            }

            batteryCheckTimer.Start();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon.Visible = true;
                this.Hide();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                notifyIcon.Visible = true;
                this.Hide();
                e.Cancel = true;
            }
        }
         
        private void menuItemShow_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            this.Show();
            this.Activate();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            this.Show();
            this.Activate();
        }
    }
}
