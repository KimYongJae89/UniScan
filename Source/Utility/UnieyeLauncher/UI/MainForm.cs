using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnieyeLauncher.Operation;
using UnieyeLauncher.UI;

namespace UnieyeLauncher
{
    public partial class MainForm : Form
    {
        bool autoRun;

        internal LauncherProcess LaunchProcess => this.launchProcess;
        LauncherProcess launchProcess;

        ToolStripStatusLabel pathStatus;
        ToolStripStatusLabel watchdogStatus;
        ToolStripStatusLabel autopatchStatus;
        ToolStripStatusLabel upsStatus;
        ToolStripStatusLabel remoteStatus;

        Timer updateTimer;
        Timer initTimer;

        Control[] controls;

        public MainForm(string version, DateTime buildDateTime , bool autoRun, string targetExe)
        {
            InitializeComponent();

            this.Text = string.Format("{0} {1}", this.Text, version);

            this.versionStringToolStripMenuItem.Text = buildDateTime.ToString("yyyyMMdd.HHmmss");            
            if (IsUserAdministrator())
                this.Text += " (Administrator)";

            this.autoRun = autoRun;
            this.launchProcess = new LauncherProcess(Program.WorkingDirectory, targetExe);
            this.launchProcess.ShowBalloonTip += UniEyeLauncherV2_ShowBalloonTip;
            this.launchProcess.ShowHide += UniEyeLauncherV2_ShowHide;
            this.launchProcess.OnLaunchableFilesUpdated += UniEyeLauncherV2_OnLaunchableFilesupdated;

            this.propertyGrid1.SelectedObject = this.launchProcess.Setting;

            this.statusStrip.Items.Add(this.pathStatus = new ToolStripStatusLabel(Program.WorkingDirectory) { Alignment = ToolStripItemAlignment.Left, BorderSides = ToolStripStatusLabelBorderSides.All });
            this.pathStatus.Click += new EventHandler((s, e) =>
            {
                ToolStripStatusLabel label = (ToolStripStatusLabel)s;
                System.Diagnostics.Process.Start(label.Text);
            });

            this.statusStrip.Items.Add(new ToolStripStatusLabel("") { Alignment = ToolStripItemAlignment.Left, Spring = true });

            this.statusStrip.Items.Add(this.watchdogStatus = new ToolStripStatusLabel("Watchdog") { Alignment = ToolStripItemAlignment.Right , BorderSides = ToolStripStatusLabelBorderSides.All});
            this.statusStrip.Items.Add(this.autopatchStatus = new ToolStripStatusLabel("Auto Update") { Alignment = ToolStripItemAlignment.Right, BorderSides = ToolStripStatusLabelBorderSides.All});
            this.statusStrip.Items.Add(this.upsStatus = new ToolStripStatusLabel("UPS") { Alignment = ToolStripItemAlignment.Right, BorderSides = ToolStripStatusLabelBorderSides.All});
            this.statusStrip.Items.Add(this.remoteStatus = new ToolStripStatusLabel("Remote") { Alignment = ToolStripItemAlignment.Right, BorderSides = ToolStripStatusLabelBorderSides.All});

            this.controls = new Control[] { this.buttonBackup, this.buttonLaunch, this.buttonRestore, this.comboBoxExcutable };

            this.updateTimer = new Timer();
            this.updateTimer.Interval = 500;
            this.updateTimer.Tick += new EventHandler((object sender, EventArgs e) =>
            {
                this.updateTimer.Stop();
                UpdateData();
                this.updateTimer.Start();
            });

            this.initTimer = new Timer();
            this.initTimer.Interval = 2000;
            this.initTimer.Tick += new EventHandler((object sender, EventArgs e) =>
            {
                this.initTimer.Stop();

                if (this.launchProcess.PatchOperator.IsActive)
                {
                    this.initTimer.Start();
                    return;
                }

                if (!string.IsNullOrEmpty(this.launchProcess.Setting.LaunchSettings.FileName))
                    this.launchProcess.Launch(this.launchProcess.Setting.LaunchSettings.FileName);
            });

            this.autoRun = autoRun && !string.IsNullOrEmpty(this.launchProcess.Setting.LaunchSettings.FileName);

            this.launchProcess.Start();
        }

        private void UniEyeLauncherV2_ShowHide(bool visible)
        {
            if(InvokeRequired)
            {
                Invoke(new ShoweHideDelegate(UniEyeLauncherV2_ShowHide), visible);
                return;
            }

            if (visible)
                this.Show();
            else
                this.Hide();
            //this.WindowState = visible ? FormWindowState.Normal : FormWindowState.Minimized;
        }

        private void UniEyeLauncherV2_ShowBalloonTip(string tipText)
        {
            this.notifyIcon.ShowBalloonTip(0, notifyIcon.Text, tipText, ToolTipIcon.Info);
        }


        private void UniEyeLauncherV2_OnLaunchableFilesupdated()
        {
            UpdateCombo(false);
        }

        public bool IsUserAdministrator()
        {
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(user);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private delegate void UpdateComboDelegate(bool updateSelect);
        private void UpdateCombo(bool updateSelect)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateComboDelegate(UpdateCombo), updateSelect);
                return;
            }

            string selectedItem = comboBoxExcutable.SelectedItem?.ToString();

            if (comboBoxExcutable.Items.Count == 0)
                selectedItem = this.launchProcess.Setting.LaunchSettings.FileName;

            comboBoxExcutable.DataSource = this.launchProcess.ExcutableFileNames;

            if (!this.launchProcess.ExcutableFileNames.Contains(selectedItem))
                comboBoxExcutable.SelectedItem = null;

            if (updateSelect)
            {
                if (this.launchProcess.ExcutableFileNames.Contains(this.launchProcess.Setting.LaunchSettings.FileName))
                    comboBoxExcutable.SelectedItem = this.launchProcess.Setting.LaunchSettings.FileName;
            }
            comboBoxExcutable.Update();
        }

        private delegate void UpdateDataDelegate();
        private void UpdateData()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData));
                return;
            }

            this.watchdogStatus.BackColor = this.launchProcess.WatchDogOperator.GetStripColor();
            this.autopatchStatus.BackColor = this.launchProcess.PatchOperator.GetStripColor();
            this.upsStatus.BackColor = this.launchProcess.UPSOperator.GetStripColor(); 
            this.remoteStatus.BackColor = this.launchProcess.RemoteOperator.GetStripColor(); 

            if (!this.initTimer.Enabled)
            {
                this.comboBoxExcutable.Enabled = !this.launchProcess.WatchDogOperator.IsRun;
                this.buttonLaunch.Enabled = !this.launchProcess.WatchDogOperator.IsRun;
                this.buttonRestore.Enabled = !this.launchProcess.WatchDogOperator.IsRun;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.notifyIcon.Text = this.Text;
            UpdateCombo(true);
            UpdateData();
            panelSetting.Visible = false;

            //Array.ForEach(this.controls, f => f.Enabled = false);

            this.updateTimer.Start();

            if (autoRun)
            {
                buttonLaunch.Enabled = comboBoxExcutable.Enabled = false;
                this.initTimer.Start();
            }
        }

        private void buttonLaunch_Click(object sender, EventArgs e)
        {
            if (!this.launchProcess.Setting.LaunchSettings.Use)
            {
                MessageBox.Show("LaunchSetting is Disabled.");
                return;
            }
            
            string launchTarget = comboBoxExcutable.SelectedItem as string;
            if (launchTarget != null)
            {
                this.launchProcess.Setting.LaunchSettings.FileName = launchTarget;
                this.launchProcess.SaveSetting();

                this.launchProcess.Launch(launchTarget);
            }
        }

        private void buttonKill_Click(object sender, EventArgs e)
        {
            this.initTimer.Stop();
            this.launchProcess.Kill();
        }

        private void buttonBackup_Click(object sender, EventArgs e)
        {
            string targetExe = this.comboBoxExcutable.SelectedItem == null ? "" : string.Format("{0}.exe", this.comboBoxExcutable.SelectedItem);
            ProgressForm progressForm = new ProgressForm("Backup", true, new DoWorkEventHandler(this.launchProcess.ArchiveOperator.StartArchive), false, targetExe);
            progressForm.ShowDialog(this);
        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            new ProgressForm("Refresh", false, new DoWorkEventHandler(this.launchProcess.ArchiveOperator.UpdateArchiveList), true).ShowDialog(this);
            //uniEyeLauncherV2.Archiver.UpdateArchiveList();

            ArchiveSelectForm form = new ArchiveSelectForm(this.launchProcess.ArchiveOperator);
            if (form.ShowDialog() == DialogResult.OK)
            {
                bool isStopped = Array.TrueForAll(launchProcess.ExcutableFileNames, f => !AppHandler.IsAppRun(f));
                if (!isStopped)
                {
                    if (MessageBox.Show(this, "Program is Running. Close?", "", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        return;

                    this.launchProcess.WatchDogOperator.Stop();
                    this.launchProcess.LaunchOperator.Kill(launchProcess.ExcutableFileNames);
                }

                ProgressForm progressForm = new ProgressForm("Restore", true, new DoWorkEventHandler(this.launchProcess.ArchiveOperator.StartRestore), false, form.SelectedArchiveItem);
                progressForm.ShowDialog(this);
                //uniEyeLauncherV2.Watchdog.Start();
            }
        }

        private void buttonSettingSave_Click(object sender, EventArgs e)
        {
            this.launchProcess.SaveSetting();
            panelSetting.Visible = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool close = false;
            close |= (e.CloseReason == CloseReason.WindowsShutDown);
            close |= (this.DialogResult == DialogResult.Abort);

            e.Cancel = !close;
            if (e.Cancel)
                this.Hide();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelSetting.Visible = !panelSetting.Visible;
        }

        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            //if (this.Visible == false)
            //    UniEyeLauncherV2_ShowBalloonTip("UniEyeLauncher is running in Background");
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void comboBoxExcutable_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.launchProcess.LaunchOperator.LaunchTarget = ((ComboBox)sender).SelectedItem as string;
        }

        private void buttonAddStartupMenu_Click(object sender, EventArgs e)
        {
            Program.AddStartupMenu(true);
        }

        private void buttonRemoveStartupMenu_Click(object sender, EventArgs e)
        {
            Program.AddStartupMenu(false);
        }

        private void buttonClearStartupMenu_Click(object sender, EventArgs e)
        {
            Program.CelarStartupMenu();
        }
    }
}
