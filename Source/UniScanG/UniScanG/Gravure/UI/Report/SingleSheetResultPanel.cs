using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.UI;
using UniScanG.UI.Etc;
using UniScanG.Data;
using DynMvp.Base;
using UniScanG.Common.Settings;
using DynMvp.Data.UI;

namespace UniScanG.Gravure.UI.Report
{
    public partial class SingleSheetResultPanel : UserControl, IMultiLanguageSupport
    {
        ContextInfoForm contextInfoForm = new ContextInfoForm();
        CanvasPanel canvasPanel;

        public SingleSheetResultPanel()
        {
            InitializeComponent();

            canvasPanel = new CanvasPanel();
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.TabIndex = 0;
            canvasPanel.ShowCenterGuide = false;
            canvasPanel.SetPanMode();
            imagePanel.Controls.Add(canvasPanel);

            canvasPanel.FigureMouseEnter = contextInfoForm.CanvasPanel_FigureFocused;
            canvasPanel.FigureMouseLeave = contextInfoForm.CanvasPanel_FigureMouseLeaved;

            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Clear()
        {
            defectList.Rows.Clear();
            defectImage.Image = null;
            canvasPanel.WorkingFigures.Clear();
            canvasPanel.UpdateImage(null);
        }

        bool onSelect = false;
        public void SelectSheet(MergeSheetResult result)
        {
            onSelect = true;
            result.ImportPrevImage();
            if (result.PrevImage != null)
            {
                canvasPanel.UpdateImage(result.PrevImage);
                canvasPanel.WorkingFigures.Clear();
                //FigureGroup group = new FigureGroup();
                //foreach (UniScanG.Data.SheetSubResult subResult in result.SheetSubResultList)
                //    group.AddFigure(subResult.GetFigure(50, 0.1f));
                //canvasPanel.WorkingFigures.AddFigure(group);
                canvasPanel.ZoomFit();
            }

            defectList.Rows.Clear();
            for (int i = 0; i < result.SheetSubResultList.Count; i++)
            {
                FoundedObjInPattern subResult = result.SheetSubResultList[i];
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewTextBoxCell camIndexCell = new DataGridViewTextBoxCell() { Value = subResult.CamIndex + 1 };
                DataGridViewTextBoxCell indexCell = new DataGridViewTextBoxCell() { Value = i };
                DataGridViewTextBoxCell typeCell = new DataGridViewTextBoxCell() { Value = subResult.GetDefectType().GetLocalString() };
                dataGridViewRow.Cells.Add(camIndexCell);
                dataGridViewRow.Cells.Add(indexCell);
                dataGridViewRow.Cells.Add(typeCell);

                dataGridViewRow.Tag = subResult;

                defectList.Rows.Add(dataGridViewRow);
            }
            defectList.SelectAll();
            SelectDefect();
            onSelect = false;
        }

        private void defectList_SelectionChanged(object sender, EventArgs e)
        {
            if (onSelect == false)
                SelectDefect();
        }

        private void SelectDefect()
        {
            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
            Bitmap bitmap = null;
            string sizeW = "", sizeH = "";

            if (defectList.SelectedRows.Count > 0)
            {
                canvasPanel.WorkingFigures.Clear();
                foreach (DataGridViewRow row in defectList.SelectedRows)
                {
                    FoundedObjInPattern sheetSubResult = row.Tag as FoundedObjInPattern;
                    if (sheetSubResult != null)
                        canvasPanel.WorkingFigures.AddFigure(sheetSubResult.GetFigure(resizeRatio));
                }
                canvasPanel.Invalidate();

                FoundedObjInPattern firstSheetSubResult = defectList.SelectedRows[0].Tag as FoundedObjInPattern;

                if (firstSheetSubResult.Image != null)
                    bitmap = firstSheetSubResult.Image;

                sizeW = string.Format("{0:0.0} [um]", firstSheetSubResult.RealRegion.Width);
                sizeH = string.Format("{0:0.0} [um]", firstSheetSubResult.RealRegion.Height);
            }
            defectImage.Image = bitmap;
            defectSizeW.Text = sizeW;
            defectSizeH.Text = sizeH;
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            defectList.SelectAll();
        }
    }
}
