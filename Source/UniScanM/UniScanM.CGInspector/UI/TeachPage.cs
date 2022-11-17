using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.UI;
using DynMvp.Authentication;
using UniScanM.CGInspector.Algorithm.Glass;
using DynMvp.Data;
using DynMvp.UI;
using System.IO;
using DynMvp.Devices;
using UniScanM.CGInspector.Data;
using System.Drawing.Imaging;
using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.UI.Touch;
using DynMvp.Devices.MotionController;

namespace UniScanM.CGInspector.UI
{
    public partial class TeachPage : UserControl, ITeachPage
    {
        CanvasPanel canvasPanel;
        CanvasPanel canvasPanel2;
        GlassParamController glassParamController;

        VisionProbe addingProbe;

        public TeachPage()
        {
            InitializeComponent();

            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.FastMode = true;
            this.canvasPanel.SetPanMode();
            this.canvasPanel.SizeChanged += new EventHandler((s, e) => (s as CanvasPanel)?.ZoomFit());
            this.canvasPanel.MouseDoubleClick += canvasPanel_MouseDoubleClick;
            this.canvasPanel.FigureCreated += CanvasPanel_FigureCreated;
            this.canvasPanel.FigureSelected += CanvasPanel_FigureSelected;
            this.canvasPanel.FigureModified += CanvasPanel_FigureModified;
            this.canvasPanel.FigureMouseEnter += CanvasPanel_FigureMouseEnter;
            this.panel1.Controls.Add(canvasPanel);

            this.glassParamController = new GlassParamController();
            this.glassParamController.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(glassParamController);

            this.canvasPanel2 = new CanvasPanel();
            this.canvasPanel2.Dock = DockStyle.Fill;
            this.canvasPanel2.FastMode = true;
            this.canvasPanel2.SetPointMode();
            this.canvasPanel2.SizeChanged += new EventHandler((s, e) => (s as CanvasPanel)?.ZoomFit());
            this.panel3.Controls.Add(canvasPanel2);

        }

        private void canvasPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CanvasPanel canvasPanel = sender as CanvasPanel;
            canvasPanel.ZoomFit();
            canvasPanel.WorkingFigures.Clear();
            SystemManager.Instance().CurrentModel.GetTarget(0, 0, 0).AppendFigures(canvasPanel.WorkingFigures, null, true);
            canvasPanel.SetPanMode();
        }

        private void CanvasPanel_FigureMouseEnter(Figure figure)
        {
            if (figure.Tag is DefectOnScreen)
            {
                DefectOnScreen defectOnScreen = (DefectOnScreen)figure.Tag;
                this.canvasPanel2.UpdateImage(defectOnScreen.SrcBitmap);
            }
        }

        private void CanvasPanel_FigureModified(List<Figure> figureList)
        {
            figureList.ForEach(f =>
            {
                VisionProbe visionProbe = f?.Tag as VisionProbe;
                visionProbe.UpdateRegion(f.GetRectangle(), null);
            });

            this.canvasPanel.WorkingFigures.Clear();
            SystemManager.Instance().CurrentModel.GetTarget(0, 0, 0).AppendFigures(this.canvasPanel.WorkingFigures, null, true);
            this.canvasPanel.Invalidate();
        }

        private void CanvasPanel_FigureSelected(List<Figure> figureList)
        {
            Figure figure = figureList.FirstOrDefault();
            VisionProbe visionProbe = figure?.Tag as VisionProbe;
            this.glassParamController.UpdateValue(visionProbe?.InspAlgorithm);
        }

        private void CanvasPanel_FigureCreated(Figure figure, CoordMapper coordMapper)
        {
            if (addingProbe == null)
                return;

            Target target = SystemManager.Instance().CurrentModel.GetTarget(0, 0, 0);
            target.AddProbe(addingProbe);

            addingProbe.UpdateRegion(figure.GetRectangle(), null);
            figure.Tag = addingProbe;
            this.glassParamController.UpdateValue(addingProbe.InspAlgorithm.Param);
            addingProbe = null;
        }

        public void EnableControls(UserType userType)
        {
            
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            if (!visibleFlag)
                return;

            UniScanM.CGInspector.Data.Model model = SystemManager.Instance().CurrentModel as UniScanM.CGInspector.Data.Model;
            if (model == null)
                return;

            Image2D image2D = model.ImageBuffer.GetImageBuffer2dItem(0, 0)?.Image;
            this.canvasPanel.UpdateImage(image2D.ToBitmap());

            this.canvasPanel.Clear();
            Target curTarget = SystemManager.Instance().CurrentModel.GetTarget(0, 0, 0);
            curTarget.AppendFigures(this.canvasPanel.WorkingFigures, null, true);
            glassParamController.UpdateValue(null);

            //Figure[] selFigures = this.canvasPanel.GetSelectedFigures();
            //Figure selFigure = Array.Find(selFigures, f => (f.Tag as VisionProbe)?.InspAlgorithm is GlassAlgorithm);
            //glassParamController.UpdateValue((selFigure?.Tag as VisionProbe)?.InspAlgorithm.Param);
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        private void toolStripButtonGrab_Click(object sender, EventArgs e)
        {
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.Convayor;

            // 시작 위치로 이동
            axisHandler.Move(0, -10000);

            // 카메라 작동
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.AddImageGrabbed(ImageDevice_ImageGrabbed);
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabMulti();

            // 끝 위치로 이동
            axisHandler.Move(0, 170000);

            // 카메라 종료
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.RemoveImageGrabbed(ImageDevice_ImageGrabbed);
        }

        private void ImageDevice_ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            Image2D image2D = (Image2D)imageDevice.GetGrabbedImage(ptr);
            UpdateImage(image2D);
        }

        private void toolStripButtonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "BITMAP(*.bmp)|*.bmp";
            dlg.FileName = SystemManager.Instance().CurrentModel.GetImagePathName(0, 0, 0);
            DialogResult dialogResult= UiHelper.ShowSTADialog(dlg);

            if (dialogResult == DialogResult.OK)
            {
                Image2D image2D = new Image2D(dlg.FileName);
                UpdateImage(image2D);
            }
        }

        private void UpdateImage(Image2D image2D)
        {
            Data.Model model = SystemManager.Instance().CurrentModel as Data.Model;
            model.ImageBuffer.Set2dImage(0, 0, image2D);

            this.canvasPanel.UpdateImage(image2D.ToBitmap(),new Rectangle(Point.Empty, image2D.Size));
        }

        private void toolStripButtonAddGlass_Click(object sender, EventArgs e)
        {
            this.addingProbe = new VisionProbe()
            {
                InspAlgorithm = new GlassAlgorithm()
                {
                    Param = new GlassAlgorithmParam()
                }
            };
            this.canvasPanel.SetAddMode(FigureType.Rectangle);
        }

        private void toolStripButtonAddPrint_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonInspection_Click(object sender, EventArgs e)
        {
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm("Inspection");
            simpleProgressForm.Show(() =>
            {
                Data.Model model = SystemManager.Instance().CurrentModel as Data.Model;

                Data.InspectionResult inspectionResult = new Data.InspectionResult();
                DeviceImageSet deviceImageSet = new DeviceImageSet();
                deviceImageSet.UpdateImage2D(model.ImageBuffer.GetImageBuffer2dItem(0, 0).Image);

                InspParam inspParam = new InspParam(0, deviceImageSet, true, false, false, false, ImageFormat.Bmp, 0);
                Target target = SystemManager.Instance().CurrentModel.GetTarget(0, 0, 0);
                target.Inspect(inspParam, inspectionResult);

                DynMvp.InspData.InspectionResult targetResult = inspectionResult.GetTargetResult(target);
                this.canvasPanel.Clear();
                target.AppendFigures(this.canvasPanel.WorkingFigures, null, true);
                targetResult.AppendResultFigures(this.canvasPanel.WorkingFigures);
                this.canvasPanel.Invalidate();
            });
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
        }
    }
}
