using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspData;
using DynMvp.Vision;
using System.Collections.Generic;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;
using UniScanG.Data;
using UniScanG.Screen.Data;
using UniScanG.Screen.Vision.Detector;
using UniScanG.UI;

namespace UniScanG.UI.InspectPage.Inspector
{
    public partial class InfoPanel : UserControl, IInspectStateListener, IOpStateListener,IMultiLanguageSupport, IModelListener
    {
        public InfoPanel()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            SystemManager.Instance().InspectRunner.AddInspectDoneDelegate(InspectDone);
            SystemState.Instance().AddInspectListener(this);
            SystemState.Instance().AddOpListener(this);

            Clear();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        delegate void InspectDoneDelegate(InspectionResult inspectionResult);
        public void InspectDone(InspectionResult inspectionResult)
        {
            if (inspectionResult.AlgorithmResultLDic.ContainsKey(SheetInspector.TypeName) == false)
                return;
            
            UpdateData();

            if (InvokeRequired)
            {
                Invoke(new InspectDoneDelegate(InspectDone), inspectionResult);
                return;
            }

            AlgorithmResult algorithmResult = inspectionResult.AlgorithmResultLDic[SheetInspector.TypeName];
            SheetResult sheetResult = (SheetResult)algorithmResult;
            processTime.Text = string.Format("{0} s", sheetResult.SpandTime.ToString("ss\\.fff"));
        }
        
        delegate void UpdateDataDelegate();
        private void UpdateData()
        {
            ProductionG productionG = (ProductionG)ProductionManager.Instance().CurProduction;
            if (productionG == null)
                return;

            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(UpdateData));
                return;
            }
            
            lotNo.Text = productionG.LotNo;
            startTime.Text = productionG.StartTime.ToString("hh:mm");
            
            productionTotal.Text = productionG.Total.ToString();
            productionNG.Text = productionG.Ng.ToString();
            productionRatio.Text = string.Format("{0:00} %", productionG.NgRatio);

            sheetAttack.Text = productionG.SheetAttackNum.ToString();
            poleCircle.Text = productionG.PoleCircleNum.ToString();
            poleLine.Text = productionG.PoleLineNum.ToString();
            dielectric.Text = productionG.DielectricNum.ToString();
            pinHole.Text = productionG.PinHoleNum.ToString();
            shape.Text = productionG.ShapeNum.ToString();
           
            todayQuntity.Text = ProductionManager.Instance().GetTodayCount().ToString();
        }
        
        public void InspectStateChanged(UniEye.Base.Data.InspectState curInspectState)
        {
            if (InvokeRequired)
            {
                Invoke(new InspectStateChangedDelegate(InspectStateChanged), curInspectState);
                return;
            }
            
            switch (curInspectState)
            {
                case UniEye.Base.Data.InspectState.Run:
                    status.Text = StringManager.GetString(this.GetType().FullName, curInspectState.ToString());
                    status.Appearance.BackColor = Colors.Run;
                    break;
                case UniEye.Base.Data.InspectState.Wait:
                    status.Text = StringManager.GetString(this.GetType().FullName, curInspectState.ToString());
                    status.Appearance.BackColor = Colors.Wait;
                    break;
                case UniEye.Base.Data.InspectState.Done:
                    status.Text = StringManager.GetString(this.GetType().FullName, curInspectState.ToString());
                    status.Appearance.BackColor = Colors.Wait;
                    break;
            }
        }

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
                    status.Text = StringManager.GetString(this.GetType().FullName, curOpState.ToString());
                    status.Appearance.BackColor = Colors.Idle;
                    break;
                case OpState.Inspect:
                    status.Text = StringManager.GetString(this.GetType().FullName, curOpState.ToString());
                    status.Appearance.BackColor = Colors.Wait;
                    UpdateData();
                    break;
            }
        }

        private void Clear()
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(Clear));
                return;
            }

            lotNo.Text = string.Empty;
            startTime.Text = string.Empty;

            processTime.Text = string.Empty;

            productionTotal.Text = string.Empty;
            productionNG.Text = string.Empty;
            productionRatio.Text = string.Empty;

            sheetAttack.Text = string.Empty;
            poleCircle.Text = string.Empty;
            poleLine.Text = string.Empty;
            dielectric.Text = string.Empty;
            pinHole.Text = string.Empty;
            shape.Text = string.Empty;
        }

        public void ModelChanged()
        {
            Clear();
        }

        public void ModelTeachDone() { }
    }
}