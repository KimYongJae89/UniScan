using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspData;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using UniEye.Base.Data;
using UniEye.Base.Device;
using UniEye.Base.UI;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Util;
using UniScanS.Data;
using UniScanS.Data.UI;
using UniScanS.Screen.Data;

namespace UniScanS.UI.Inspect
{
    public partial class InspectPage : UserControl, IMainTabPage, IVncContainer, IOpStateListener, IMultiLanguageSupport
    {
        List<IVncControl> vncButtonList = new List<IVncControl>();
        IServerExchangeOperator server;
        
        public InspectPage()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;
            
            ImagePanel imageViewPanel = new ImagePanel();

            IInspectDefectPanel inspectDefectPanel = SystemManager.Instance().UiChanger.CreateDefectPanel();
            inspectDefectPanel.AddDelegate(imageViewPanel.UpdateResult);
            defectPanel.Controls.Add((Control)inspectDefectPanel);
            imagePanel.Controls.Add(imageViewPanel);
            panelInfo.Controls.Add((Control)SystemManager.Instance().UiChanger.CreateDefectInfoPanel());

            SystemState.Instance().AddOpListener(this);
        }
        
        private void buttonStart_Click(object sender, System.EventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            string modelCheckMessage = string.Format("Is screen model [ {0} ]. Do you want to continue?", SystemManager.Instance().CurrentModel.Name);
            if (MessageForm.Show(this.ParentForm, modelCheckMessage, MessageFormType.YesNo) == DialogResult.No)
                return;

            ProductionS productionG = (ProductionS)SystemManager.Instance().ProductionManager.GetLastProduction(SystemManager.Instance().CurrentModel);
            string lotNo = "";
            DialogResult dialogResult = DialogResult.No;
            
            bool newLot = true;
            if (productionG != null)
            {
                if (productionG.Name == SystemManager.Instance().CurrentModel.Name)
                {
                    lotNo = productionG.LotNo;

                    string message = string.Format("There is a last lot [ {0} ] in {1} . Will you produce it again?", productionG.LotNo, productionG.StartTime.ToString("yyyy-MM-dd"));
                    message = StringManager.GetString(this.GetType().FullName, message);
                    if (MessageForm.Show(this.ParentForm, message, MessageFormType.YesNo) == DialogResult.Yes)
                        newLot = false;
                }
            }

            bool overrited = false;
            if (newLot == true)
            {
                InputForm inputForm = new InputForm("Lot No");

                inputForm.InputText = lotNo;

                if (inputForm.ShowDialog() == DialogResult.Cancel)
                    return;

                lotNo = inputForm.InputText;

                if (string.IsNullOrEmpty(lotNo) == true)
                {
                    MessageForm.Show(this.ParentForm, "Invalid lot No.");
                    return;
                }

                ProductionS overriteProduction = (ProductionS)SystemManager.Instance().ProductionManager.List.Find(p => p.LotNo == lotNo && p.Name == SystemManager.Instance().CurrentModel.Name);
                if (dialogResult == DialogResult.No && overriteProduction != null)
                {
                    string message = string.Format("This Lot [{0}] was produced in {1}. Do you want to overwrite it?", overriteProduction.LotNo, overriteProduction.StartTime.ToString("yy-MM-dd"));
                    if (MessageForm.Show(this.ParentForm, message, MessageFormType.YesNo) == DialogResult.No)
                        return;

                    overrited = true;
                }
            }

            SystemManager.Instance().ProductionManager.LotChange(SystemManager.Instance().CurrentModel, lotNo);

            if (overrited == true)
                SystemManager.Instance().ProductionManager.CurProduction.Reset();
            
            SystemManager.Instance().InspectRunner.EnterWaitInspection();
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
            //foreach (IVncControl vncButton in vncButtonList)
            //{
            //    if (vncButton != startVncButton)
            //        vncButton.Disable();
            //}
        }

        public void ProcessExited()
        {
            //foreach (IVncControl vncButton in vncButtonList)
            //{
            //    vncButton.ExitProcess();
            //    vncButton.Enable();
            //}
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
                    buttonReset.Enabled = true;
                    break;
                case OpState.Inspect:
                    buttonStart.Visible = false;
                    buttonPause.Visible = false;
                    buttonStop.Visible = true;
                    buttonReset.Enabled = false;
                    break;
            }
        }

        private void buttonStop_Click(object sender, System.EventArgs e)
        {
            SystemManager.Instance().InspectRunner.ExitWaitInspection();
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

        private void buttonReset_Click(object sender, System.EventArgs e)
        {
            if (MessageForm.Show(this.ParentForm, "All inspection data is deleted. Do you want to reset?", MessageFormType.YesNo) == DialogResult.No)
                return;

            CancellationTokenSource token = new CancellationTokenSource();

            SimpleProgressForm loadingForm = new SimpleProgressForm("Reset");
            loadingForm.Show(new Action(() =>
            {
                SystemManager.Instance().ExchangeOperator.SendCommand(UniScanS.Common.Exchange.ExchangeCommand.M_RESET);

                if (SystemManager.Instance().ProductionManager.CurProduction != null)
                {
                    ProductionS productionG = (ProductionS)SystemManager.Instance().ProductionManager.CurProduction;
                    productionG.Reset();
                }

                if (SystemManager.Instance().ExchangeOperator is IServerExchangeOperator)
                {
                    IServerExchangeOperator server = SystemManager.Instance().ExchangeOperator as IServerExchangeOperator;
                    
                    foreach (InspectorObj obj in server.GetInspectorList())
                    {
                        if (obj.CurProduction != null)
                            obj.CurProduction.Reset();
                    }
                }
            }), token);
        }

        private void buttonSplitter_Click(object sender, System.EventArgs e)
        {

        }

        public void EnableControls(UserType userType)
        {
        }
    }
}