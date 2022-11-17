using DynMvp.Base;
using DynMvp.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Device;
using UniEye.Base.UI;
using UniEye.Base.UI.CameraCalibration;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;
using UniScanG.Data;
using UniScanG.Data.UI;

namespace UniScanG.UI.InspectPage
{
    public partial class InspectPage : UserControl, IMainTabPage, IVncContainer, IOpStateListener, IMultiLanguageSupport
    {
        List<IVncControl> vncButtonList = new List<IVncControl>();
        IServerExchangeOperator server;
        
        public InspectPage()
        {
            InitializeComponent();
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
            ImagePanel imageViewPanel = new ImagePanel();

            IInspectDefectPanel inspectDefectPanel = SystemManager.Instance().UiChanger.CreateDefectPanel();
            inspectDefectPanel.AddDelegate(imageViewPanel.UpdateResult);
            defectPanel.Controls.Add((Control)inspectDefectPanel);
            imagePanel.Controls.Add(imageViewPanel);
            panelInfo.Controls.Add((Control)SystemManager.Instance().UiChanger.CreateDefectInfoPanel());
            //resultPanel.Controls.Add(new ResultPanel());

            SystemState.Instance().AddOpListener(this);
        }

        private void buttonStart_Click(object sender, System.EventArgs e)
        {
            ProductionG productionG = (ProductionG)ProductionManager.Instance().GetLastProduction();

            string curLotNo = "";
            if (productionG != null)
                curLotNo = productionG.LotNo;

            InputForm inputForm = new InputForm("Lot No", curLotNo);

            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                ProductionManager.Instance().LotChange(SystemManager.Instance().CurrentModel, inputForm.InputText);
                SystemManager.Instance().InspectRunner.EnterWaitInspection();
            }
        }

        public void EnableControls()
        {

        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == true)
            {

            }
            else
            {
                ProcessExited();
            }
        }
        
        public void ProcessStarted(IVncControl startVncButton)
        {
            foreach (IVncControl vncButton in vncButtonList)
            {
                if (vncButton != startVncButton)
                    vncButton.Disable();
            }
        }

        public void ProcessExited()
        {
            foreach (IVncControl vncButton in vncButtonList)
            {
                vncButton.ExitProcess();
                vncButton.Enable();
            }
        }
        
        public void UpdateControl(string item, object value)
        {
            throw new System.NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            throw new System.NotImplementedException();
        }

        delegate void OpStatusChangedDelegate(OpState curOpState, OpState prevOpState);
        public void OpStatusChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                Invoke(new OpStatusChangedDelegate(OpStatusChanged), curOpState, prevOpState);
                return;
            }

            switch (curOpState)
            {
                case OpState.Idle:
                    buttonStart.Visible = true;
                    buttonPause.Visible = false;
                    buttonStop.Visible = false;
                    break;
                case OpState.Inspect:
                    buttonStart.Visible = false;
                    buttonPause.Visible = false;
                    buttonStop.Visible = true;
                    break;
            }
        }

        private void buttonStop_Click(object sender, System.EventArgs e)
        {
            SystemManager.Instance().InspectRunner.ExitWaitInspection();
        }

        private void buttonSplitter_Click(object sender, System.EventArgs e)
        {
            defectPanel.Visible = !defectPanel.Visible;
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
    }
}