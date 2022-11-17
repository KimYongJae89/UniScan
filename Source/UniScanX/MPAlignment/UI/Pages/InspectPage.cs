using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using System.IO;
using DynMvp.Vision;
using DynMvp.UI;
using System.Threading.Tasks;
using System.Diagnostics;
using DynMvp.Data.UI;
using System.Drawing.Imaging;
using System.Reflection;
//using UniScanX.MPAlignment.Data;
using ReaLTaiizor;
using DynMvp.InspData;
using UniScanX.MPAlignment;


namespace UniScanX.MPAlignment.UI.Pages
{
    public partial class InspectPage : UserControl
    {
        #region SetDoubleBuffered
        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        void SetDoubleBuffer()
        {
            SetDoubleBuffered(txtTotal);
            SetDoubleBuffered(txtGood);
            SetDoubleBuffered(txtNg);
            SetDoubleBuffered(txtModelName);
            SetDoubleBuffered(txtInspectionNo);
            SetDoubleBuffered(txtBarcodeNo);
            SetDoubleBuffered(txtInspResult);
            SetDoubleBuffered(txtTotalModules);
            SetDoubleBuffered(btnStartInspection);
            SetDoubleBuffered(labelStatus);
            SetDoubleBuffered(labelResult);
            SetDoubleBuffered(lblContinuousDefect);
        }
        #endregion

        List<InspectionResult> inspectResultList = new List<InspectionResult>();
        List<LastInspectResult> lastInspectResultList = new List<LastInspectResult>();


  //      ProductTextResult productTextResult;

        private object drawingLock = new object();

        CanvasPanel inspectView;

        MPAlignment.Data.MPModel CurrentModel;

        Image startIcon = Properties.Resources.start_128;
        Image stopIcon = Properties.Resources.stop_128;

        Calibration calibration;

 //       PgAdminHelper pgAdminHelper; 

        public string TabName
        {
            get { return "Inspect"; }
        }

        public bool IsAdminPage
        {
            get { return false; }
        }
 
        public InspectPage()
        {
            InitializeComponent();
            
            SuspendLayout();

            inspectView = new CanvasPanel();
            inspectView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            inspectView.Dock = System.Windows.Forms.DockStyle.Fill;
            inspectView.Location = new System.Drawing.Point(3, 3);
            inspectView.Name = "inspectViwe ";
            inspectView.Size = new System.Drawing.Size(409, 523);
            inspectView.TabIndex = 8;
            inspectView.TabStop = false;
 //           inspectView.Enable = false;
 //           inspectView.NoneClickMode = true;
            panelLargeView.Controls.Add(inspectView);


            //conveyorMonitor = new ConveyorMonitor(DeviceManager.Instance.ConveyorUnit);
            //conveyorMonitor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //conveyorMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
            //conveyorMonitor.Location = new System.Drawing.Point(3, 3);
            //conveyorMonitor.Name = "conveyorMonitor";
            //conveyorMonitor.Size = new System.Drawing.Size(409, 523);
            //conveyorMonitor.TabIndex = 8;
            //conveyorMonitor.TabStop = false;
            //pnlConveyorSystem.Controls.Add(conveyorMonitor);

 //           ((MPAlignment.Data.ModelManager)SystemManager.Instance().ModelManager).ModelLoaded += ModelChanged;
     //       SystemManager.Instance().InspectRunner.InsepctStartedEvent += InsepctStarted;
     //       SystemManager.Instance().InspectRunner.ProductInspectedEvent += ProductInspected;

     //       pgAdminHelper = new PgAdminHelper(SystemConfig.Instance.PgAdminInfo);
     //       productTextResult = new ProductTextResult();

            ResumeLayout();
        }

        

        public void ChangeCaption()
        {
            labelModelName.Text = StringManager.GetString(labelModelName.Text);
            labelInspTotal.Text = StringManager.GetString(labelInspTotal.Text);
            labelBarcodeNo.Text = StringManager.GetString(labelBarcodeNo.Text);
            labelInspectionTime.Text = StringManager.GetString(labelInspectionTime.Text);
            labelInspTotal.Text = StringManager.GetString(labelInspTotal.Text);
            labelAccept.Text = StringManager.GetString(labelAccept.Text);
            labelDefect.Text = StringManager.GetString(labelDefect.Text);
            btnResetCount.Text = StringManager.GetString(btnResetCount.Text);
        }

        public void ExitWaitInspection()
        {
//            SystemManager.Instance().InspectRunner.RepeatMode = false;
            btnResetCount.Enabled = true;
        }

        public void Initialize()
        {

        }

        public void ModelListChanged()
        {
            // 필요시 구현 
        }

        public void OnIdle()
        {

        }

        public async void ModelChanged(object obj, EventArgs e)
        {
            var currentModel = ((MPAlignment.Data.ModelManager)SystemManager.Instance().ModelManager).CurrentModel;
            if (currentModel == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate { ModelChanged(obj, e); }));
            }
            else
            {
                    LogHelper.Debug(LoggerType.Inspection, "InspectionPage - ModelChanged");

                txtModelName.Text = currentModel.Name;
                txtBarcodeNo.Text = "None";
                txtInspectionNo.Text = "None";

                //Production production = currentModel.Production;

                //txtNg.Text = production.Ng.ToString();
                //txtGood.Text = production.Good.ToString();
                //txtTotal.Text = production.Total.ToString();

                txtTotalModules.Text = $"0 / {currentModel.GetNumTarget()}";
            }
        }

        private void InsepctStarted(object sender)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate { InsepctStarted(sender); }));
                return;
            }
            labelStatus.Text = "Inspect";
        }

        public void ProductInspected(object data)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate { ProductInspected(data); }));
                return;
            }

            //labelStatus.Text = "Ready";
            //var productResult = data as InspectionResult;
            //inspectView.UpdateImage(ImageHelper.CloneImage(productResult.GrabImage)); // 이미지 업데이트
            //ShowInspectResult(productResult); // 타겟 영역 그리기 용
            //UpdateInspectResultInfo(productResult);
            //if (productResult.Judgment == Judgment.Good)
            //    DeviceManager.Instance.IoHandler?.TowerLampWorking();
            //else
            //    DeviceManager.Instance.IoHandler?.TowerLampNoSoundAlarm();

            //DeviceManager.Instance.ConveyorUnit?.ProductInspected(productResult);

            //if (OperationConfig.Instance.UseReviewMode)
            //{
            //    ShowReviewForm(productResult);
            //}

            //SaveDefaultImage(productResult.GrabImage);
            //SaveProductResult(productResult);

            //void SaveDefaultImage(Bitmap image)
            //{
            //    if (ModelManager.Instance.CurrentModel == null)
            //        return;
            //    string path = Path.Combine(ModelManager.Instance.CurrentModel.ModelPath, "DefaultImage");
            //    if (Directory.Exists(path) == false)
            //    {
            //        Directory.CreateDirectory(path);
            //    }
            //    string imagePath = Path.Combine(path, "DefaultImage.bmp");
            //    image.Save(imagePath, ImageFormat.Bmp);
            //}
            
        }

        private void ShowReviewForm(InspectionResult productResult)
        {
            if (productResult.IsGood())
                return;

            //var form = new InspectionReviewForm(productResult);
            //if (form.ShowDialog() == DialogResult.OK)
            //{
            //    LastInspectResult lastInspectResult = lastInspectResultList.Find(x => x.InspNo == productResult.InspectionNo);
            //    if (lastInspectResult != null)
            //    {
            //        lastInspectResult.Result = productResult.Judgment.ToString();

            //        dgvLastInspectionResult.AutoGenerateColumns = false;
            //        dgvLastInspectionResult.DataSource = null;
            //        dgvLastInspectionResult.DataSource = lastInspectResultList;
            //    }
            //}
        }

        private void InspectionResultForm_DebugClicked(object sender, EventArgs e)
        {

        }

        void SaveProductResult(InspectionResult productResult)
        {
            Task.Run(() =>
            {
                //pgAdminHelper.UpdateProductResult(productResult);
                //productTextResult.SaveResult(productResult);
                //productTextResult.SaveImage(productResult, null);
            });
        }

        Image InspectResultCapture()
        {
            var bmp = new Bitmap(inspectView.Width, inspectView.Height, PixelFormat.Format32bppArgb);

            var g = Graphics.FromImage(bmp);
            g.CopyFromScreen(inspectView.Location.X, inspectView.Location.Y, 0, 0,
                            inspectView.Size, CopyPixelOperation.SourceCopy);

            return bmp;
        }

        void ShowInspectResult(InspectionResult productResult)
        {
            //if (InvokeRequired)
            //{
            //    BeginInvoke(new MethodInvoker(delegate { ShowInspectResult(productResult); }));
            //    return;
            //}
            //LogHelper.Debug(LoggerType.Inspection, "Inspectpage - ShowImageResult");

            //inspectView.BackgroundFigures.Clear();
            //productResult.AppendResultTargetFigures(inspectView.BackgroundFigures, false, 10.0f);

            //inspectView.Invalidate();
        }

        void UpdateInspectResultInfo(InspectionResult productResult)
        {
            //if (InvokeRequired)
            //{
            //    BeginInvoke(new MethodInvoker(delegate { UpdateInspectResultInfo(productResult); }));
            //    return;
            //}
            //if (ModelManager.Instance.CurrentModel == null)
            //    return;
            //UpdateLastInspectResult(productResult);
            //CountUpdate(productResult);
            //UpdateInspectionTime(productResult);
            //UpdateInspectResult(productResult);
        }

        void CountUpdate(InspectionResult inspectResult)
        {
            //if (inspectResult.Judgment == Judgment.Good)
            //    ModelManager.Instance.CurrentModel.Production.ProduceGood();
            //else
            //    ModelManager.Instance.CurrentModel.Production.ProduceNG();
            //txtBarcodeNo.Text = inspectResult.InspectionNo;
            //txtGood.Text = ModelManager.Instance.CurrentModel.Production.Good.ToString();
            //txtNg.Text = ModelManager.Instance.CurrentModel.Production.Ng.ToString();
            //txtTotal.Text = ModelManager.Instance.CurrentModel.Production.Total.ToString();
        }

        void UpdateInspectionTime(InspectionResult inspectResult)
        {
            //TimeSpan inspectionTime = inspectResult.LastInspectionEndTime - inspectResult.LastInspectionStartTime;
            //txtInspectionTime.Text = $"{inspectionTime.Seconds} : {inspectionTime.Milliseconds}";
        }

        void UpdateLastInspectResult(InspectionResult productResult)
        {
            string inspNostr = productResult.InspectionNo;
            var lastInspectResult = new LastInspectResult
            {
                InspNo = inspNostr,
                Model = productResult.ModelName,
                Result = productResult.Judgment.ToString()
            };
            lastInspectResultList.Insert(0, lastInspectResult);
            if(lastInspectResultList.Count > 2000)
            {
                lastInspectResultList.RemoveAt(lastInspectResultList.Count - 1);
            }

            dgvLastInspectionResult.AutoGenerateColumns = false;
            dgvLastInspectionResult.DataSource = null;
            dgvLastInspectionResult.DataSource = lastInspectResultList;
        }

        void UpdateInspectResult(InspectionResult productResult)
        {
            //switch (productResult.Judgment)
            //{
            //    //default:
            //    //case Judgment.Good:
            //    //    UpdateResultLabel("Good", Color.Black, Color.LimeGreen);
            //    //    break;
            //    //case Judgment.Overkill:
            //    //    UpdateResultLabel("Overkill", Color.Black, Color.Yellow);
            //    //    break;
            //    //case Judgment.NG:
            //    //    UpdateResultLabel("NG", Color.White, Color.Red);
            //    //    break;
            //}
        }

        bool isInspectButtonOnProgress = false;

        private async void buttonStartInspection_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().InspectRunner.EnterWaitInspection();
        }

     
     
        private void btnResetCount_Click(object sender, EventArgs e)
        {
            //Model model = ModelManager.Instance.CurrentModel;

            //if (model != null)
            //{
            //    Production production = model.Production;
            //    production.Reset();
            //    txtTotal.Text = txtGood.Text = txtNg.Text = "0";
            //}

            //SystemManager.Instance.InspectRunner.ResetState();
        }

        void UpdateResultLabel(string text, Color foreColor, Color backColor)
        {
            labelResult.BackColor = backColor;
            labelResult.ForeColor = foreColor;
            labelResult.Text = StringManager.GetString(text);
        }

        public void TargetEndInspect(Target target, InspectionResult probeResultList)
        {

        }

        //void UpdateJudgmentWholeImage(Judgment judgment)
        //{
        //    string resultString = judgment.ToString();
        //    Color color = GetDrawResultColor(judgment);
            
        //}

        //Color GetDrawResultColor(Judgment judgment)
        //{
        //    switch (judgment)
        //    {
        //        case Judgment.Good:
        //            return Color.Green;
        //        case Judgment.NG:
        //            return Color.Red;
        //        case Judgment.Overkill:
        //            return Color.Yellow;
        //        default:
        //            return Color.Cyan;
        //    }
        //}

        public class LastInspectResult
        {
            public string InspNo { get; set; }
            public string Model { get; set; }
            public string Result { get; set; }
        }



        public void InspectCanceled()
        {
            UpdateResultLabel("Cancel", Color.White, Color.Red);
            //UpdateTextigureWholeImage("Cancel", 2, Color.Purple);

            //InspectRunner inspectRunner = SystemManager.Instance.InspectRunner;
            //inspectRunner.ExitWaitInspection();
        }

        private void InspectPage_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                var getModel = ((MPAlignment.Data.ModelManager)(SystemManager.Instance().ModelManager)).CurrentModel;
                if (this.CurrentModel != getModel)
                    ModelChanged(null, EventArgs.Empty);
            }

            lblTestMode.Visible = false;
            return;

            tgsTestMode.Visible = false;
#if DEBUG
            lblTestMode.Visible = true;
            tgsTestMode.Visible = true;
#endif
            tgsTestMode.Checked = false;
        }

        private void lblIsPassMode_Click(object sender, EventArgs e)
        {
            //if(DeviceManager.Instance.ConveyorUnit.IsPassMode == false) 
            //{
            //    lblIsPassMode.BackColor = Color.DeepSkyBlue;
            //    DeviceManager.Instance.ConveyorUnit.IsPassMode = true;
            //}
            //else
            //{
            //    lblIsPassMode.BackColor = Color.FromArgb(64, 64, 64);
            //    DeviceManager.Instance.ConveyorUnit.IsPassMode = false;
            //}
        }
    }
}
