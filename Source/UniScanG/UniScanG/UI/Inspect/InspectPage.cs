using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Device;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Util;
using UniScanG.Data;
using UniScanG.Data.UI;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Settings;
using UniScanG.Inspect;

namespace UniScanG.UI.Inspect
{
    public partial class InspectPage : UserControl, IMainTabPage, /*IVncContainer,*/ IOpStateListener, IMultiLanguageSupport, IUserHandlerListener, IModelListener
    {
        public IInspectDefectPanel InspectDefectPanel { get; private set; }
        IInspectExtraPanel extraImageControl;
        IInspectExtraPanel extraInfoControl;
        IInspectExtraPanel extraLegnthControl;

        public IRepeatedDefectAlarmForm RepeatedDefectAlarmForm { get => this.repeatedDefectAlarmForm; set => this.repeatedDefectAlarmForm = value; }
        IRepeatedDefectAlarmForm repeatedDefectAlarmForm;

        //List<IVncControl> vncButtonList = new List<IVncControl>();
        IServerExchangeOperator server;
        InspectStarter inspectStarter;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public InspectPage()
        {
            InitializeComponent();
            this.VisibleChanged += InspectPage_VisibleChanged;
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            //server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;

            //MonitorUiChanger monitorUiChanger = (MonitorUiChanger)SystemManager.Instance().UiChanger;

            //List<Control> buttonList = monitorUiChanger.GetInspectButtons(this);            
            //foreach (Control button in buttonList)
            //{
            //    buttonPanel.Controls.Add(button);
            //    if (button is IVncControl)
            //        vncButtonList.Add((IVncControl)button);
            //}

            //foreach (IVncControl vncButton in vncButtonList)
            //    vncButton.InitHandle(layoutInspect.Handle);

            //// Vnc Post
            ///

            this.InspectDefectPanel = SystemManager.Instance().UiChanger.CreateDefectPanel();

            ImagePanel imageViewPanel = new ImagePanel();
            this.panelImage.Controls.Add(imageViewPanel);
            this.InspectDefectPanel.AddDelegate(imageViewPanel.UpdateResult);

            this.extraImageControl = SystemManager.Instance().UiChanger.CreateExtraImagePanel();
            if (this.extraImageControl != null)
            {
                this.panelImage.Controls.Add((Control)extraImageControl);
                this.InspectDefectPanel.AddDelegate(extraImageControl.UpdateResult);
            }

            this.extraInfoControl = SystemManager.Instance().UiChanger.CreateExtraInfoPanel();
            if (this.extraInfoControl != null)
            {
                this.panelExtraInfo.Visible = true;
                this.panelExtraInfo.Controls.Add((Control)extraInfoControl);
                this.InspectDefectPanel.AddDelegate(extraInfoControl.UpdateResult);
            }

            this.extraLegnthControl = SystemManager.Instance().UiChanger.CreateExtraLengthPanel();
            if (this.extraLegnthControl != null)
            {
                this.ultraSplitter1.Visible = true;
                this.panelLengthChart.Visible = true;
                this.panelLengthChart.Controls.Add((Control)extraLegnthControl);
                this.InspectDefectPanel.AddDelegate(this.extraLegnthControl.UpdateResult);
            }


            defectPanel.Controls.Add((Control)InspectDefectPanel);

            panelInfo.Controls.Add((Control)SystemManager.Instance().UiChanger.CreateInfoBufferPanel());

            UpdateBlockStateChange(true);

            this.inspectStarter = (SystemManager.Instance().InspectRunnerG as UniScanG.Inspect.InspectRunner)?.InspectStarter;

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            UserHandler.Instance().AddListener(this);
            SystemState.Instance().AddOpListener(this);

            buttonBlinkTimer.Start();
        }

        private void InspectPage_VisibleChanged(object sender, EventArgs e)
        {
            extraImageControl?.UpdateVisible();
            //extraInfoControl?.UpdateVisible();
        }

        private void buttonStart_Click(object sender, System.EventArgs e)
        {
            if (ErrorManager.Instance().IsAlarmed())
            {
                ErrorManager.Instance().ErrorItemList.FindAll(f => !f.IsCleared).ForEach(f => f.SetShowen(false));
                return;
            }

            this.repeatedDefectAlarmForm?.Clear();

            bool ok = this.inspectStarter.EnterWaitInspection(this, true, new CancellationTokenSource());
            UpdateBlockStateChange(!ok);
        }

        public void EnableControls(UserType userType)
        {

        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == true)
            {

            }
            else
            {
                //ProcessExited();
            }
        }

        //public void ProcessStarted(IVncControl startVncButton)
        //{
        //    //foreach (IVncControl vncButton in vncButtonList)
        //    //{
        //    //    if (vncButton != startVncButton)
        //    //        vncButton.Disable();
        //    //}
        //    SystemManager.Instance().DeviceController.OnEnterWaitInspection();
        //}

        //public void ProcessExited()
        //{
        //    foreach (IVncControl vncButton in vncButtonList)
        //    {
        //        vncButton.ExitProcess();
        //        vncButton.Enable();
        //    }
        //    SystemManager.Instance().DeviceController.OnExitWaitInspection();
        //}

        public void UpdateControl(string item, object value)
        {
            throw new System.NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {

        }

        delegate void OpStatusChangedDelegate(OpState curOpState, OpState prevOpState);
        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                Invoke(new OpStatusChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            switch (curOpState)
            {
                case OpState.Idle:
                    buttonStart.Visible = true;
                    buttonPause.Visible = false;
                    buttonStop.Visible = false;
                    this.repeatedDefectAlarmForm?.Silent();
                    break;

                case OpState.Wait:
                    this.repeatedDefectAlarmForm?.Clear();
                    break;

                case OpState.Inspect:
                case OpState.Alarm:
                    buttonStart.Visible = false;
                    buttonPause.Visible = false;
                    buttonStop.Visible = true;
                    break;
            }
        }

        private void buttonStop_Click(object sender, System.EventArgs e)
        {
            this.inspectStarter.ExitWaitInspection();
        }

        private void buttonSplitter_Click(object sender, System.EventArgs e)
        {
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            //StringManager.UpdateString(this.GetType().FullName, buttonStart);
            //StringManager.UpdateString(this.GetType().FullName, buttonPause);
            //StringManager.UpdateString(this.GetType().FullName, buttonStop);
            //StringManager.UpdateString(this.GetType().FullName, buttonLot);
            //StringManager.UpdateString(this.GetType().FullName, buttonReset);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            this.InspectDefectPanel.Reset();
            this.repeatedDefectAlarmForm?.Clear();
        }

        private void ultraButtonUpdate_Click(object sender, EventArgs e)
        {
            UpdateBlockStateChange(!this.InspectDefectPanel.BlockUpdate);
        }

        public void UpdateBlockStateChange(bool v)
        {
            this.InspectDefectPanel.BlockUpdate = v;
            if (this.InspectDefectPanel.BlockUpdate)
                ultraButtonUpdate.Appearance.BackColor = Color.Red;
            else
                ultraButtonUpdate.Appearance.BackColor = Color.LightGreen;
        }

        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegate(UserChanged));
                return;
            }

            User curUser = UserHandler.Instance().CurrentUser;
            if (curUser == null)
                return;

            ultraButtonObserver.Visible = curUser.IsMasterAccount;

            //Form form = ((UniScanG.Inspect.InspectRunner)SystemManager.Instance().InspectRunner).InspectObserver as Form;
            //if (form != null)
            //{
            //    if (UserHandler.Instance().CurrentUser.SuperAccount)
            //        form.Show();
            //    else
            //        form.Hide();
            //}
        }

        private void ultraButtonObserver_Click(object sender, EventArgs e)
        {
            Form form = ((UniScanG.Inspect.InspectRunner)SystemManager.Instance().InspectRunnerG).InspectObserver as Form;
            if (form == null)
                return;

            if (form.Visible == false)
                form.Show();
            else
                form.Hide();
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().InspectRunnerG.EnterPauseInspection();
        }

        private void ultraButtonAlarm_Click(object sender, EventArgs e)
        {
            repeatedDefectAlarmForm?.Show();
        }

        private void buttonBlinkTimer_Tick(object sender, EventArgs e)
        {
            if (repeatedDefectAlarmForm != null && repeatedDefectAlarmForm.IsAlarmState)
            {
                ultraButtonAlarm.Appearance.BackColor = (DateTime.Now.Millisecond / 500) == 0 ? Colors.Alarm : Colors.Normal;
                //ultraButtonAlarm.Enabled = true;
                //this.ultraButtonAlarm.Visible = true;
            }
            else
            {
                ultraButtonAlarm.Appearance.BackColor = Colors.Normal;
                //ultraButtonAlarm.Enabled = false;
            }
        }

        private void InspectPage_Load(object sender, EventArgs e)
        {
        }

        public void ModelRefreshed() { }

        public void ModelChanged()
        {
            Reset();
        }

        public void ModelTeachDone(int camId) { }

        private void ultraSplitter1_CollapsedChanged(object sender, EventArgs e)
        {
            if (!ultraSplitter1.Collapsed)
                this.panelLengthChart.Height = Math.Max(this.panelLengthChart.Height, this.Height / 3);
        }
    }
}