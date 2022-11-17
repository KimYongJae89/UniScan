using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.InspData;
using UniEye.Base.Data;
using DynMvp.Data.UI;
using DynMvp.Base;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using UniEye.Base;
using UniScanM.Operation;
using Infragistics.Win.Misc;
using System.Threading;
using DynMvp.Authentication;
using DynMvp.UI.Touch;
using UniScanM.Authorize;
using UniScanM.Settings;

namespace UniScanM.UI
{
    public partial class InspectionPage : UserControl, IInspectionPage, IOpStateListener, IMultiLanguageSupport, IInspectStateListener
    {
        private List<IInspectionPanel> inspectionPanelList;
        public List<IInspectionPanel> InspectionPanelList
        {
            get { return inspectionPanelList; }
        }

        Data.FigureDrawOption figureDrawOption = null;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        Task UpdatePvPosTask;

        public InspectionPage()
        {
            InitializeComponent();

            this.SuspendLayout();
            this.Dock = DockStyle.Fill;

            inspectionPanelList = new List<IInspectionPanel>();

            IInspectionPanel leftPanel = SystemManager.Instance().UiChanger.CreateInspectionPanel(0);
            if (leftPanel != null)
            {
                InspectionPanelList.Add(leftPanel);
                panelInspectLeft.Controls.Add((Control)leftPanel);
            }

            IInspectionPanel rightPanel = SystemManager.Instance().UiChanger.CreateInspectionPanel(1);
            if (rightPanel != null)
            {
                InspectionPanelList.Add(rightPanel);
                panelInspectRight.Controls.Add((Control)InspectionPanelList.Last());
            }

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            InitializFigureDrawOption();

            StringManager.AddListener(this);
            SystemState.Instance().AddOpListener(this);
            //SystemState.Instance().AddInspectListener(this);

            ErrorManager.Instance().OnStartAlarmState += ErrorManager_OnStartAlarm;
            ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmStatus;
            UpdatePvPosTask = new Task(new Action(UpdatePvPos));
            UpdatePvPosTask.Start();
            this.ResumeLayout(false);
        }

        private void InitializFigureDrawOption()
        {
            figureDrawOption = new Data.FigureDrawOption()
            {
                useTargetCoord = false,

                PatternConnection = false,

                TeachResult = new Data.FigureDrawOptionProperty()
                {
                    ShowFigure = true,
                    Good = new Data.DrawSet(new Pen(Color.FromArgb(64, 0x90, 0xEE, 0x90), 3), new SolidBrush(Color.FromArgb(32, 0x90, 0xEE, 0x90))),
                    Ng = new Data.DrawSet(new Pen(Color.FromArgb(64, 0xFF, 0x00, 0x00), 3), new SolidBrush(Color.FromArgb(32, 0xFF, 0x00, 0x00))),
                    Invalid = new Data.DrawSet(null, null),

                    ShowText = false,
                    FontSet = new Data.FontSet(new Font("Gulim", 20), Color.Yellow)
                },

                ProcessResult = new Data.FigureDrawOptionProperty()
                {
                    ShowFigure = true,
                    Good = new Data.DrawSet(new Pen(Color.FromArgb(64, 0x90, 0xEE, 0x90), 3), new SolidBrush(Color.FromArgb(32, 0x90, 0xEE, 0x90))),
                    Ng = new Data.DrawSet(new Pen(Color.FromArgb(64, 0xFF, 0x00, 0x00), 3), new SolidBrush(Color.FromArgb(32, 0xFF, 0x00, 0x00))),
                    Invalid = new Data.DrawSet(new Pen(Color.FromArgb(64, 0xFF, 0xFF, 0x00), 3), new SolidBrush(Color.FromArgb(32, 0xFF, 0xFF, 0x00))),

                    ShowText = false,
                    FontSet = new Data.FontSet(new Font("Gulim", 20), Color.Red)
                }
            };
        }

        private void MonitoringPage_Load(object sender, EventArgs e)
        {
            if (UniScanMSettings.Instance().ProgramStartWithAutoMode)
                ChangeStartMode(StartMode.Auto);
            else
                ChangeStartMode(StartMode.Stop);

            UpdateStatusLabel();
        }

        private void ErrorManager_OnStartAlarm()
        {
            ChangeStartMode(StartMode.Stop);
        }

        private void ErrorManager_OnResetAlarmStatus()
        {
            //ChangeStartMode(StartMode.Auto);
        }

        IAsyncResult asyncUpdatePage = null;
        private delegate void UpdateControlTextDelegate(Label labelResult, string text, Color? foreColor, Color? backColor);
        private void UpdateControlText(Control control, string text = null, Color? foreColor = null, Color? backColor = null)
        {
            if (InvokeRequired)
            {
                if (asyncUpdatePage != null)
                {
                    if (asyncUpdatePage.IsCompleted == false)
                    {
                        return;
                    }
                }

                asyncUpdatePage = BeginInvoke(new UpdateControlTextDelegate(UpdateControlText), control, text, foreColor, backColor); // BeginInvoke 
                return;
            }

            if (control.IsDisposed == true)
                return;

            //UiHelper.SuspendDrawing(control);
            //control.SuspendLayout();

            control.Text = text;
            if (backColor != null)
                control.BackColor = backColor.Value;
            control.Update();

            //UiHelper.ResumeDrawing(control);
            //control.ResumeLayout(true);
        }

        private delegate void ProductInspectedDelegate(InspectionResult inspectionResult);
        public void ProductInspected(InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ProductInspectedDelegate(ProductInspected), inspectionResult);
                return;
            }

            this.inspectionPanelList.ForEach(panel => panel.ProductInspected(inspectionResult));

            Color backColor = Color.LightSteelBlue;
            string resultStr = "";
            switch (inspectionResult.Judgment)
            {
                case Judgment.Warn:
                    backColor = Color.Gold;
                    resultStr = StringManager.GetString(this.GetType().FullName, "Warn");
                    break;
                case Judgment.Accept:
                    backColor = Color.LimeGreen;
                    resultStr = StringManager.GetString(this.GetType().FullName, "OK");
                    break;
                case Judgment.Reject:
                    backColor = Color.Red;
                    resultStr = StringManager.GetString(this.GetType().FullName, "NG");
                    break;
                case Judgment.Skip:
                    backColor = Color.LimeGreen;
                    resultStr = StringManager.GetString(this.GetType().FullName, "SKIP");
                    break;
                default:
                    backColor = Color.LightSteelBlue;
                    resultStr = "";
                    break;

            }
            UpdateControlText(result, resultStr, null, backColor);
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            this.Visible = visibleFlag;
        }

        private void InspectionPage_Resize(object sender, EventArgs e)
        {
            int splitterDistance = (int)Math.Round(this.Size.Width * 0.7f);
            if (splitterDistance >= splitContainer.Panel1MinSize)
                splitContainer.SplitterDistance = splitterDistance;
        }

        private void UpdatePvPos()
        {
            //HACK zmsong
            while (true)
            {
                if (SystemManager.Instance().InspectStarter != null && SystemManager.Instance().InspectStarter is PLCInspectStarter)
                {
                    if (SystemState.Instance().OpState != OpState.Idle)
                    {
                        PLCInspectStarter starter = SystemManager.Instance().InspectStarter as PLCInspectStarter;
                        string text = string.Format("{0:F0} m", starter.MelsecMonitor.State.PvPosition);
                        UpdateControlText(this.pVPos, text);
                    }
                }
                Thread.Sleep(333);
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            UpdateStatusLabel();
        }

        public void EnableControls(UserType User)
        {

        }

        public void UpdateControl(string item, object value)
        {
            //throw new NotImplementedException();

            this.inspectionPanelList.ForEach(panel => panel.InfomationChanged(value));
        }

        private void buttonAutoManSwitch_Click(object sender, EventArgs e)
        {
            UltraButton button = (UltraButton)sender;
            switch (button.Name)
            {
                case "buttonAutoManSwitch":
                    ChangeStartMode(StartMode.Auto);
                    break;

                case "buttonManStart":
                    OnManualButton_Click();
                    break;

                case "buttonManStop":
                    if (ChangeStartMode(StartMode.Stop))
                    {
                        SystemManager.Instance().InspectRunner.ExitWaitInspection();
                        this.inspectionPanelList.ForEach(panel => panel.ExitWaitInspection());
                    }
                    break;
            }
            UpdateStatusLabel();
        }
        public virtual void OnManualButton_Click()
        {
            if (UniScanMSettings.Instance().ManaulStartAuthrizeCheck == true)
            {
                if (AuthorizeHelper.Authorize(UserType.Admin | UserType.Maintrance) == false)
                {
                    MessageForm.Show(null, StringManager.GetString("Permission is invaild."));
                    return;
                }

                if (MessageForm.Show(null, StringManager.GetString("'Force Start Mode' can occur abnormal inspection. Continue?"), MessageFormType.YesNo) == DialogResult.Yes)
                {
                    if (ChangeStartMode(StartMode.Manual))
                    {
                        SystemManager.Instance().InspectStarter.PreStartInspect(false);
                        //UiHelper.SuspendDrawing(this);
                        SystemManager.Instance().InspectStarter.OnStartInspection();
                        //UiHelper.ResumeDrawing(this);
                    }
                }
            }
            else
            {
                if (ChangeStartMode(StartMode.Manual))
                {
                    SystemManager.Instance().InspectStarter.PreStartInspect(false);
                    //UiHelper.SuspendDrawing(this);
                    SystemManager.Instance().InspectStarter.OnStartInspection();
                    //UiHelper.ResumeDrawing(this);
                }
            }
        }
        private delegate bool ChangeStartModeDelegate(StartMode startMode);
        public bool ChangeStartMode(StartMode startMode)
        {
            if (InvokeRequired)
            {
                return (bool)Invoke(new ChangeStartModeDelegate(ChangeStartMode), startMode);
            }

            if (SystemState.Instance().OpState == OpState.Wait)
                return false;

            StartMode curStartMode = SystemManager.Instance().InspectStarter.StartMode;
            bool ok = (curStartMode != startMode);

            if (ok)
            {
                SystemManager.Instance().InspectStarter.StartMode = startMode;

                buttonAutoManSwitch.Appearance.BackColor = startMode == StartMode.Auto ? Color.CornflowerBlue : Color.Transparent;
                buttonAutoManSwitch.Enabled = startMode != StartMode.Manual;

                buttonManStart.Appearance.BackColor = startMode == StartMode.Manual ? Color.CornflowerBlue : Color.Transparent;
                buttonManStart.Enabled = startMode != StartMode.Auto;

                buttonManStop.Appearance.BackColor = startMode == StartMode.Stop ? Color.CornflowerBlue : Color.Transparent;
            }
            return ok;
        }



        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            UpdateStatusLabel();

            if (curOpState == OpState.Inspect && prevOpState == OpState.Idle)
                this.inspectionPanelList.ForEach(f => f.ClearPanel());
        }

        public void InspectStateChanged(InspectState curInspectState)
        {
            UpdateStatusLabel();
        }

        public delegate void UpdateStatusLabelDelegate();
        public void UpdateStatusLabel()
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateStatusLabelDelegate(UpdateStatusLabel));
                return;
            }

            if (OperationOption.Instance().OnTune)
            {
                UpdateControlText(status, StringManager.GetString(this.GetType().FullName, "Setting"), Color.White, Color.Orange);
                return;
            }

            StartMode startMode = SystemManager.Instance().InspectStarter.StartMode;
            OpState opState = SystemState.Instance().OpState;
            if (opState == OpState.Idle)
            {
                string status = startMode == StartMode.Auto ? "Wait" : "Stop";
                UpdateControlText(this.status, StringManager.GetString(this.GetType().FullName, status), null, Color.CornflowerBlue);
                UpdateControlText(this.result, StringManager.GetString(this.GetType().FullName, ""), null, Color.LightSteelBlue);
                UpdateControlText(this.pVPos, StringManager.GetString(this.GetType().FullName, ""));
            }
            else if (opState == OpState.Inspect)
            {
                UpdateControlText(status, StringManager.GetString(this.GetType().FullName, "Run"), Color.White, Color.DarkGreen);
            }
        }
    }
}
