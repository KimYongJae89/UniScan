using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;
using UniScanS.Common.Util;
using System.Drawing;
using DynMvp.UI.Touch;
using System.Threading;
using System.Collections.Generic;
using DynMvp.Base;

namespace UniScanS.UI.Etc
{
    public partial class VncCamButton : UserControl, IVncControl, IModelListener, IMultiLanguageSupport
    {
        IServerExchangeOperator serverExchangeOperator;

        Process process;
        IntPtr handle;
        InspectorObj inspector;

        Action<IVncControl> vncViwerStarted;
        Action vncViwerExited;

        ExchangeCommand eVisit;
        
        public VncCamButton(ExchangeCommand eVisit, InspectorObj inspector, Action<IVncControl> vncViwerStarted, Action vncViwerExited)
        {
            InitializeComponent();

            this.eVisit = eVisit;

            this.inspector = inspector;

            this.vncViwerStarted = vncViwerStarted;
            this.vncViwerExited = vncViwerExited;

            serverExchangeOperator = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;

            this.TabIndex = 0;
            
            buttonCam.Text = string.Format("Cam {0}", inspector.Info.CamIndex + 1);

            StringManager.AddListener(this);

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
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
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm("Connect");
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            
            simpleProgressForm.Show(new Action(() => {
                if (process == null)
                {
                    if (OpenVnc() == true)
                    {
                        ProcessStarted();
                    }
                }
                else
                {
                    ExitProcess();
                }
            }), cancellationTokenSource);

            simpleProgressForm.Close();
        }

        delegate void EnableDelegate();
        public void Disable()
        {
            if (InvokeRequired)
            {
                Invoke(new EnableDelegate(Disable));
                return;
            }

            buttonCam.Enabled = false;
        }

        public void Enable()
        {
            if (InvokeRequired)
            {
                Invoke(new EnableDelegate(Enable));
                return;
            }

            buttonCam.Enabled = true;
        }

        protected bool OpenVnc()
        {
            Process newProcess = serverExchangeOperator.OpenVnc(eVisit, process, inspector.Info.Address, handle);

            bool existProcess = newProcess != null;

            if (newProcess != process)
            {
                newProcess.EnableRaisingEvents = true;
                newProcess.Exited += ProcessExited;
                process = newProcess;
            }

            return existProcess;
        }

        public void ExitProcess()
        {
            if (process != null)
            {
                if (process.HasExited == false)
                    process.Kill();

                process = null;
            }
        }

        public void ProcessStarted()
        {
            if (vncViwerStarted != null)
                vncViwerStarted(this);

            //buttonCam.Appearance.BackColor = Colors.Wait;
        }

        public void ProcessExited(object sender, EventArgs e)
        {
            serverExchangeOperator.CloseVnc();

            process = null;

            if (vncViwerExited != null)
                vncViwerExited();
        }
        
        public InspectorObj GetInspector()
        {
            return inspector;
        }

        public void ModelChanged()
        {
            ButtonStateChage();
        }

        public void ModelTeachDone()
        {
            ButtonStateChage();
        }

        delegate void ButtonStateChageDelegate();
        private void ButtonStateChage()
        {
            if (InvokeRequired)
            {
                Invoke(new ButtonStateChageDelegate(ButtonStateChage));
                return;
            }

            if (SystemManager.Instance().CurrentModel == null)
            {
                buttonCam.Enabled = false;
                return;
            }

            buttonCam.Enabled = true;

            //if (serverExchangeOperator.ModelTrained(inspector.Info.CamIndex, SystemManager.Instance().CurrentModel.ModelDescription))
            //    buttonCam.Appearance.BackColor = Colors.Trained;
            //else
            //    buttonCam.Appearance.BackColor = Colors.Untrained;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
