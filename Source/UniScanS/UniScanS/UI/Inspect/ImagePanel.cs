using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.UI;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Settings;
using UniScanS.Common.Util;
using UniScanS.Data;
using UniScanS.Screen.Data;
using UniScanS.Screen.UI;
using UniScanS.Screen.Vision.Detector;
using UniScanS.UI.Etc;

namespace UniScanS.UI.Inspect
{
    public partial class ImagePanel : UserControl, IMultiLanguageSupport, IModelListener, IProductionListener
    {
        CanvasPanel canvasPanel;

        ContextInfoForm contextInfoForm = new ContextInfoForm();

        public ImagePanel()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            canvasPanel = new CanvasPanel(true, System.Drawing.Drawing2D.InterpolationMode.Bilinear);
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.TabIndex = 0;
            canvasPanel.ShowCenterGuide = false;
            canvasPanel.SetPanMode();
            canvasPanel.FigureMouseEnter = contextInfoForm.CanvasPanel_FigureFocused;
            canvasPanel.MouseLeaved = contextInfoForm.CanvasPanel_MouseLeaved;
            image.Controls.Add(canvasPanel);

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            ((ProductionManagerS)SystemManager.Instance().ProductionManager).AddListener(this);
        }

        public void ProductionChanged()
        {
            canvasPanel.WorkingFigures.Clear();
        }

        public void ModelChanged()
        {
            canvasPanel.WorkingFigures.Clear();
        }

        public void ModelTeachDone() { }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            //labelImage.Text = StringManager.GetString(this.GetType().FullName, labelImage.Text);
        }

        private void Clear()
        {
            canvasPanel.WorkingFigures.Clear();
            canvasPanel.UpdateImage(null);
        }

        public void UpdateResult(SheetResult sheetResult, List<DataGridViewRow> dataGridViewRowList)
        {
            canvasPanel.WorkingFigures.Clear();

            foreach (DataGridViewRow row in dataGridViewRowList)
            {
                SheetSubResult subResult = (SheetSubResult)row.Tag;
                canvasPanel.WorkingFigures.AddFigure(subResult.GetFigure(50, SystemTypeSettings.Instance().ResizeRatio));
            }

            if (sheetResult == null)
                return;

            labelInspectInfo.Text = sheetResult.SheetErrorType.ToString();

            switch (sheetResult.SheetErrorType)
            {
                case SheetErrorType.None:
                    if (sheetResult.SheetSubResultList.Count == 0)
                    {
                        labelInspectInfo.BackColor = Colors.Good;
                        labelInspectInfo.Text = "Good";
                    }
                    else
                    {
                        labelInspectInfo.BackColor = Colors.NG;
                        labelInspectInfo.Text = "NG";
                    }
                    break;
                case SheetErrorType.FiducialNG:
                    labelInspectInfo.BackColor = Colors.NG;
                    break;
                case SheetErrorType.Error:
                case SheetErrorType.InvalidInspect:
                case SheetErrorType.DifferenceModel:
                case SheetErrorType.InvalidPoleParam:
                case SheetErrorType.InvalidDielectricParam:
                    labelInspectInfo.BackColor = Colors.Idle;
                    break;
            }

            canvasPanel.UpdateImage(sheetResult.PrevImage);
        }
    }
}
