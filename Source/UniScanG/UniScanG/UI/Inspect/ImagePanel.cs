using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Inspect;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Settings;
using UniScanG.Common.Util;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision;
using UniScanG.UI.Etc;

namespace UniScanG.UI.Inspect
{
    public partial class ImagePanel : UserControl, IMultiLanguageSupport
    {
        CanvasPanel canvasPanel;

        ContextInfoForm contextInfoForm = new ContextInfoForm();

        public ImagePanel()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            canvasPanel = new CanvasPanel();
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.TabIndex = 0;
            canvasPanel.ShowCenterGuide = false;
            canvasPanel.ReadOnly = true;
            canvasPanel.SetPanMode();
            canvasPanel.FigureMouseEnter = contextInfoForm.CanvasPanel_FigureFocused;
            canvasPanel.FigureMouseLeave = contextInfoForm.CanvasPanel_FigureMouseLeaved;
            canvasPanel.MouseLeaved = contextInfoForm.CanvasPanel_MouseLeaved;
            canvasPanel.MouseClicked = CanvasPanel_MouseClicked;
            canvasPanel.SizeChanged += CanvasPanel_SizeChanged;
            image.Controls.Add(canvasPanel);
        }

        private void ImagePanel_Load(object sender, EventArgs e)
        {
            //this.layoutMarginInfo.Visible = AlgorithmSetting.Instance().UseExtMargin;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            //labelImage.Text = StringManager.GetString(this.GetType().FullName, labelImage.Text);
        }

        public void UpdateResult(Bitmap image, List<DataGridViewRow> dataGridViewRowList)
        {
            LogHelper.Debug(LoggerType.Operation, $"ImagePanel::UpdateResult");
            canvasPanel.WorkingFigures.Clear();

            float ratio = SystemTypeSettings.Instance().ResizeRatio;
            if (dataGridViewRowList != null)
            {
                dataGridViewRowList.ForEach(f =>
                {
                    FoundedObjInPattern subResult = (FoundedObjInPattern)f.Tag;
                    canvasPanel.WorkingFigures.AddFigure(subResult.GetFigure(ratio));
                });
            }

            bool zoomFit = false;
            if (image == null)
            {
                zoomFit = true;
                if (SystemManager.Instance().CurrentModel != null)
                {
                    string imagePath = (SystemManager.Instance().ModelManager as ModelManager)?.GetPreviewImagePath(SystemManager.Instance().CurrentModel.ModelDescription, "");
                    if (System.IO.File.Exists(imagePath))
                    {
                        //image = (Bitmap)ImageHelper.LoadImage(imagePath);
                        image = new Image2D(imagePath).ToBitmap();
                    }
                }
            }

            if (image != null)
                canvasPanel.UpdateImage(image);

            if (zoomFit)
                canvasPanel.ZoomFit();

            canvasPanel.Invalidate();
        }


        private void CanvasPanel_MouseClicked(CanvasPanel canvasPanel, PointF point, ref bool processingCancelled)
        {
            canvasPanel.ZoomFit();
            processingCancelled = true;
        }

        private void CanvasPanel_SizeChanged(object sender, EventArgs e)
        {
            canvasPanel.ZoomFit();
        }


        //private void InspectDone(InspectionResult inspectionResult)
        //{
        //   
        //}
        
    }
}
