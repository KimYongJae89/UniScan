using DynMvp.Base;
using DynMvp.UI;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCI_Test.Vision;

namespace WCI_Test
{
    public partial class Form1 : Form
    {
        ImageD masterImageD;
        ImageD imageD;
        ImageD resultImageD;

        CanvasPanel canvasPanelMaster;
        CanvasPanel canvasPanelImage;
        CanvasPanel canvasPanelResult;

        WciAlgorithm wciAlgorithm;
        WciInspectionResult wciInspectionResult;
        ToolStripMenuItem toolStripMenuItem;

        public Form1()
        {
            InitializeComponent();

            this.canvasPanelMaster = new CanvasPanel();
            this.canvasPanelImage = new CanvasPanel();
            this.canvasPanelResult = new CanvasPanel();

            this.canvasPanelMaster.FastMode = this.canvasPanelImage.FastMode = this.canvasPanelResult.FastMode = true;

            this.canvasPanelImage.SetPanMode();
            this.canvasPanelMaster.SetPanMode();
            this.canvasPanelResult.SetPanMode();

            this.canvasPanelMaster.MouseDblClicked += CanvasPanel_MouseDblClicked;
            this.canvasPanelImage.MouseDblClicked += CanvasPanel_MouseDblClicked;
            this.canvasPanelResult.MouseDblClicked += CanvasPanel_MouseDblClicked;

            this.groupMaster.Controls.Add(canvasPanelMaster);
            this.groupImage.Controls.Add(canvasPanelImage);
            this.groupResult.Controls.Add(canvasPanelResult);
        }

        private void CanvasPanel_MouseDblClicked(CanvasPanel canvasPanel)
        {
            canvasPanel.ZoomFit();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.toolStripMenuItem = startInspectToolStripMenuItem;
            this.toolStripSplitButton.Text = this.toolStripMenuItem.Text;

            this.wciAlgorithm = new WciAlgorithm();
            this.wciAlgorithm.Initialize();
            this.propertyGrid1.SelectedObject = this.wciAlgorithm.Param;

            string masterFileName = @"D:\UniScan\Test\Image\Image_C00_S000_L00.bmp";
            if (File.Exists(masterFileName))
                LoadMasterImage(masterFileName);

            string imageFileName = @"D:\UniScan\Test\Image\Image_C00_S001_L00.bmp";
            if (File.Exists(imageFileName))
                LoadImage(imageFileName);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.wciAlgorithm.Dispose();
        }

        private FileInfo SelectBitmap(bool isOpenMode)
        {
            FileDialog fileDialog;
            if (isOpenMode)
                fileDialog = new OpenFileDialog();
            else
                fileDialog = new SaveFileDialog();

            fileDialog.Filter = "BITAMP (*.bmp)|*.bmp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
                return new FileInfo(fileDialog.FileName);

            return null;
        }

        private void LoadMasterImage(string fullName)
        {
            new SimpleProgressForm("Load Master Image").Show(() =>
            {
                this.masterImageD = new Image2D(fullName);
                this.canvasPanelMaster.UpdateImage(this.masterImageD.ToBitmap());

                Figure figure = GetFigure(this.wciAlgorithm.Param);
                this.canvasPanelMaster.Clear();
                this.canvasPanelMaster.WorkingFigures.AddFigure(figure);
            });
        }

        private void LoadImage(string fullName)
        {
            new SimpleProgressForm("Load Image").Show(() =>
            {
                this.imageD = new Image2D(fullName);
                this.canvasPanelImage.UpdateImage(this.imageD.ToBitmap());
            });
        }

        private void ClearResult()
        {
            this.resultImageD?.Dispose();
            this.resultImageD = null;
            this.canvasPanelResult.UpdateImage(null);
        }

        private void SaveResultImage(string fullName)
        {
            this.resultImageD?.SaveImage(fullName);
        }

        private void loadMasterImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItem = loadMasterImageToolStripMenuItem;
            this.toolStripSplitButton.Text = this.toolStripMenuItem.Text;

            FileInfo fileInfo = SelectBitmap(true);
            if (fileInfo != null)
                LoadMasterImage(fileInfo.FullName);
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItem = loadImageToolStripMenuItem;
            this.toolStripSplitButton.Text = this.toolStripMenuItem.Text;

            FileInfo fileInfo = SelectBitmap(true);
            if (fileInfo != null)
                LoadImage(fileInfo.FullName);
        }

        private void clearResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItem = clearResultToolStripMenuItem;
            this.toolStripSplitButton.Text = this.toolStripMenuItem.Text;

            ClearResult();
        }

        private void saveResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItem = saveResultToolStripMenuItem;
            this.toolStripSplitButton.Text = this.toolStripMenuItem.Text;

            FileInfo fileInfo = SelectBitmap(false);
            if (fileInfo != null)
                SaveResultImage(fileInfo.FullName);
        }

        private void startInspectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItem = startInspectToolStripMenuItem;
            this.toolStripSplitButton.Text = this.toolStripMenuItem.Text;

            this.wciInspectionResult = null;

            WciInspectionParam inspectParam = new WciInspectionParam();
            inspectParam.MasterImageD = this.masterImageD;
            inspectParam.ImageD = this.imageD;

            ProgressForm progressForm = new ProgressForm();
            progressForm.BackgroundWorker.DoWork += this.wciAlgorithm.BackgroundInspect;
            progressForm.BackgroundWorker.RunWorkerCompleted += this.BackgroundInspectCompleted;
            progressForm.Argument = inspectParam;
            progressForm.ShowDialog();

            this.resultImageD = wciInspectionResult?.ResultImage;
            this.canvasPanelResult.UpdateImage(this.resultImageD?.ToBitmap());
        }

        private void BackgroundInspectCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
            //    MessageBox.Show(e.Error.Message, e.Error.GetType().Name);
                return;
            }

            this.wciInspectionResult = e.Result as WciInspectionResult;
        }

        private void toolStripMenuItem_ButtonClick(object sender, EventArgs e)
        {
            this.toolStripMenuItem?.PerformClick();
        }

        private Figure GetFigure(WciAlgorithmParam param)
        {
            FigureGroup figureGroup = new FigureGroup();
            figureGroup.Selectable = false;
            param.rectangleList.ForEach(f =>
            {
                figureGroup.AddFigure(new RectangleFigure(f, new Pen(Color.Yellow, 1)));
            });
            return figureGroup;
        }
    }
}
