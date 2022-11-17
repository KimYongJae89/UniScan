using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Common.Util;
using System.Drawing;
using DynMvp.UI.Touch;
using System.Threading;
using System.Collections.Generic;
using DynMvp.Base;
using DynMvp.UI;
using UniEye.Base.Data;

namespace UniScanG.UI.Etc
{
    public partial class VncCamButton : UserControl, IVncControl, IModelListener, IMultiLanguageSupport, IOpStateListener
    {
        IServerExchangeOperator serverExchangeOperator;

        static object lockObject = new object();
        static Process process = null;
        static VncCamButton vncCamButton = null;

        IntPtr handle;
        InspectorObj inspector;
        ExchangeCommand visit;

        Action<IVncControl> vncViwerStarted;
        Action vncViwerExited;

        public VncCamButton(ExchangeCommand visit, InspectorObj inspector, Action<IVncControl> vncViwerStarted, Action vncViwerExited)
        {
            InitializeComponent();

            this.inspector = inspector;

            this.vncViwerStarted = vncViwerStarted;
            this.vncViwerExited = vncViwerExited;

            this.visit = visit;
            this.serverExchangeOperator = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;

            this.TabIndex = 0;

            StringManager.AddListener(this);

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            SystemState.Instance().AddOpListener(this);

        }

        public void InitHandle(IntPtr handle)
        {
            this.handle = handle;
        }

        private void buttonCam_Click(object sender, EventArgs e)
        {
            ButtonClick();
        }

        protected void ButtonClick()
        {
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Connect"));
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Form mainForm = ConfigHelper.Instance().MainForm;

            simpleProgressForm.Show(mainForm , new Action(() =>
            {
                if (vncCamButton == this)
                // 같은 버튼 클릭시 연결 해제
                {
                    ExitProcess();

                    this.serverExchangeOperator.SendCommand(ExchangeCommand.V_INSPECT);
                    this.vncViwerExited?.Invoke();
                }
                else if (vncCamButton != null)
                // 다른 버튼 클릭시 연결 해제 후 다른 연결
                {
                    ExitProcess();
                    System.Threading.Thread.Sleep(500);
                    if(!StartProcess())
                    // 실패시 연결 해제.
                    {
                        this.serverExchangeOperator.SendCommand(ExchangeCommand.V_INSPECT);
                        this.vncViwerExited?.Invoke();
                    }
                }
                else
                // 첫 연결시 연결 후 초기화
                {
                    if (StartProcess())
                    {
                        this.serverExchangeOperator.SendCommand(this.visit);
                        this.vncViwerStarted?.Invoke(this);
                    }
                }
            }), cancellationTokenSource);

            //simpleProgressForm.Close();
        }

        delegate void EnableDelegate();
        public void Disable()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EnableDelegate(Disable));
                return;
            }

            buttonCam.Enabled = false;
        }

        public void Enable()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EnableDelegate(Enable));
                return;
            }

            buttonCam.Enabled = true;
        }

        protected bool OpenVnc()
        {
            if (UniEye.Base.Data.SystemState.Instance().OpState != UniEye.Base.Data.OpState.Idle)
                return false;

            bool existProcess = false;
            lock (lockObject)
            {
                Process newProcess = serverExchangeOperator.OpenVnc(process, inspector.Info.IpAddress, handle);
                existProcess = newProcess != null;

                if (newProcess != process)
                {
                    //newProcess.EnableRaisingEvents = true;
                    newProcess.Exited += ProcessExited;

                    process = newProcess;
                }
            }
            return existProcess;
        }

        public void ExitProcess()
        {
            lock (lockObject)
            {
                if (process != null)
                {
                    if (process.HasExited == false)
                        process.Kill();

                    process = null;
                }

                // 버튼 상태 바꿈
                VncCamButton vncCamButton2 = vncCamButton;
                vncCamButton = null;
                vncCamButton2?.ButtonStateChage();
            }
        }

        public bool StartProcess()
        {
            if (OpenVnc() == true)
            {
                vncCamButton = this;
                ButtonStateChage();
                return true;
            }
            return false;
        }

        public void ProcessExited(object sender, EventArgs e)
        {
            serverExchangeOperator.CloseVnc();
            //lock (lockObject)
            //    process = null;

            //if (vncViwerExited != null)
            //    vncViwerExited();

            //ButtonStateChage();
        }

        public InspectorObj GetInspector()
        {
            return inspector;
        }

        public void ModelChanged()
        {
            ButtonStateChage();
        }

        public void ModelTeachDone(int camId)
        {
            ButtonStateChage();
        }

        public void ModelRefreshed() { }

        delegate void ButtonStateChageDelegate();
        private void ButtonStateChage()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ButtonStateChageDelegate(ButtonStateChage));
                return;
            }

            if (SystemManager.Instance().CurrentModel == null)
            {
                buttonCam.Enabled = false;
                return;
            }

            buttonCam.Enabled = true;

            if (vncCamButton == this)
                buttonCam.Appearance.BackColor = Colors.Wait;
            else if (serverExchangeOperator.ModelTrained(inspector.Info.CamIndex, inspector.Info.ClientIndex, SystemManager.Instance().CurrentModel.ModelDescription))
                buttonCam.Appearance.BackColor = Colors.Trained;
            else
                buttonCam.Appearance.BackColor = Colors.Untrained;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            buttonCam.Text = StringManager.GetString(this.GetType().FullName, inspector.Info.GetName());
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new OpStateChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            this.Enabled = (curOpState == OpState.Idle);
            if (this.Enabled == false && process != null)
                ExitProcess();
        }
    }
}
