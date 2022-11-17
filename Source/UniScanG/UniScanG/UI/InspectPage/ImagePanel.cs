using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Inspect;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Settings;
using UniScan.Common.Util;
using UniScanG.Screen.Data;
using UniScanG.Screen.UI;
using UniScanG.Screen.Vision.Detector;

namespace UniScanG.UI.InspectPage
{
    public partial class ImagePanel : UserControl, IMultiLanguageSupport
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

            canvasPanel = new CanvasPanel();
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.TabIndex = 0;
            canvasPanel.ShowCenterGuide = false;
            canvasPanel.DragMode = DragMode.Pan;
            canvasPanel.FigureFocused = contextInfoForm.CanvasPanel_FigureFocused;
            canvasPanel.MouseLeaved = contextInfoForm.CanvasPanel_MouseLeaved;
            image.Controls.Add(canvasPanel);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            //labelImage.Text = StringManager.GetString(this.GetType().FullName, labelImage.Text);
        }

        public void UpdateResult(Bitmap image, List<DataGridViewRow> dataGridViewRowList)
        {
            canvasPanel.WorkingFigures.Clear();

            foreach (DataGridViewRow row in dataGridViewRowList)
            {
                SheetSubResult subResult = (SheetSubResult)row.Tag;
                canvasPanel.WorkingFigures.AddFigure(subResult.GetFigure(1, SystemTypeSettings.Instance().ResizeRatio));
            }

            if (image != null)
                canvasPanel.UpdateImage(image);
        }
    }
}
