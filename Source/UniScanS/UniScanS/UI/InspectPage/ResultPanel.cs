using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.Vision;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;
using UniScanG.Screen.Data;
using UniScanG.Screen.Vision;
using UniScanG.Screen.Vision.Detector;
using UniScanG.Settings;
using UniScanG.Vision.FiducialFinder;

namespace UniScanG.UI.InspectPage
{
    public partial class ResultPanel : UserControl, IMultiLanguageSupport
    {
        public ResultPanel()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            SystemManager.Instance().InspectRunner.AddInspectDoneDelegate(FidInspectDone);
            SystemManager.Instance().InspectRunner.AddInspectDoneDelegate(SheetInspectDone);
        }

        public void FidInspectDone(InspectionResult inspectionResult)
        {
            if (inspectionResult.AlgorithmResultLDic.ContainsKey(FiducialFinder.TypeName) == false)
                return;
            
            if (InvokeRequired)
            {
                Invoke(new InspectDoneDelegate(FidInspectDone), inspectionResult);
                return;
            }

            AlgorithmResult algorithmResult = inspectionResult.AlgorithmResultLDic[FiducialFinder.TypeName];
            
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell indexCell = new DataGridViewTextBoxCell() { Value = inspectionResult.InspectionNo };
            DataGridViewTextBoxCell timeCell = new DataGridViewTextBoxCell() { Value = algorithmResult.SpandTime.ToString("fff") };
            row.DefaultCellStyle.BackColor = algorithmResult.Good == true ? Color.Green : Color.Red;
            row.Cells.Add(indexCell);
            row.Cells.Add(timeCell);
            row.Tag = algorithmResult;

            if (fidResultGrid.Rows.Count >= CustomSettings.Instance().MaxShowDefectNum)
                fidResultGrid.Rows.RemoveAt(fidResultGrid.Rows.Count - 1);

            fidResultGrid.Rows.Insert(0, row);
        }

        public void SheetInspectDone(InspectionResult inspectionResult)
        {
            AlgorithmResult algorithmResult = inspectionResult.AlgorithmResultLDic[SheetInspector.TypeName];
            if (algorithmResult == null)
                return;

            if (InvokeRequired)
            {
                Invoke(new InspectDoneDelegate(SheetInspectDone), inspectionResult);
                return;
            }

            SheetResult sheetResult = (SheetResult)algorithmResult;
            
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell indexCell = new DataGridViewTextBoxCell() { Value = inspectionResult.InspectionNo };
            DataGridViewTextBoxCell numCell = new DataGridViewTextBoxCell() { Value = sheetResult.DefectNum};
            DataGridViewTextBoxCell inspTimeCell = new DataGridViewTextBoxCell() { Value = sheetResult.SpandTime.ToString("ss\\.fff") };
            DataGridViewTextBoxCell exportTimeCell = new DataGridViewTextBoxCell() { Value = inspectionResult.ExportTime.ToString("ss\\.fff") };

            row.Tag = sheetResult;
            
            row.DefaultCellStyle.BackColor = sheetResult.Good == true ? Color.Green : Color.Red;
            row.Cells.Add(indexCell);
            row.Cells.Add(numCell);
            row.Cells.Add(inspTimeCell);
            row.Cells.Add(exportTimeCell);

            if (sheetResultGrid.Rows.Count >= CustomSettings.Instance().MaxShowDefectNum)
                sheetResultGrid.Rows.RemoveAt(sheetResultGrid.Rows.Count - 1);

            sheetResultGrid.Rows.Insert(0, row);
        }

        public void UpdateLanguage()
        {
            throw new System.NotImplementedException();
        }

        private void ResultPanel_VisibleChanged(object sender, System.EventArgs e)
        {
            layoutFiducial.Visible = AlgorithmSetting.Instance().IsFiducial;
        }
    }
}
