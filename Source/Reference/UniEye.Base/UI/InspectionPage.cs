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
using DynMvp.Devices.Light;
using System.IO;
using DynMvp.Inspection;
using DynMvp.InspData;
using DynMvp.UI.Touch;
using UniEye.Base.Settings;
using UniEye.Base.Data;
using DynMvp.Authentication;

namespace UniEye.Base.UI
{
    public partial class InspectionPage : UserControl, IInspectionPage, IMainTabPage
    {
        List<InspectionResult> inspectionResultList = new List<InspectionResult>();

        ImageAcquisition imageAcquisition;

        List<IInspectionPanel> inspectionPanelList = null;
        public List<IInspectionPanel> InspectionPanelList
        {
            get { return inspectionPanelList; }
        }

        int fontSize = 40;

        private object drawingLock = new object();
        private LightCtrlHandler lightCtrlHandler;

        private int offlineImageIndex = 0;
        private string[] offlineImagePathList = null;
        Data.Production productionSingle = new Data.Production("", DateTime.Now, "");

        public InspectionPage()
        {
            LogHelper.Debug(LoggerType.StartUp, "Begin Constructor Inspection Page");

            InitializeComponent();

            labelModelName.Text = StringManager.GetString(this.GetType().FullName,labelModelName.Text);
            labelInspectionNo.Text = StringManager.GetString(this.GetType().FullName,labelInspectionNo.Text);
            labelInspTime.Text = StringManager.GetString(this.GetType().FullName,labelInspTime.Text);
            labelTotal.Text = StringManager.GetString(this.GetType().FullName,labelTotal.Text);
            labelAccept.Text = StringManager.GetString(this.GetType().FullName,labelAccept.Text);
            labelDefect.Text = StringManager.GetString(this.GetType().FullName,labelDefect.Text);
            buttonResetCount.Text = StringManager.GetString(this.GetType().FullName,buttonResetCount.Text);
            buttonSelectImage.Text = StringManager.GetString(this.GetType().FullName,buttonSelectImage.Text);


#if !DEBUG
            buttonSelectImage.Visible = false;
            //checkBoxButtonStart.Visible = false;
#endif
            LogHelper.Debug(LoggerType.StartUp, "End Constructor Inspection Page");
        }

        public void Initialize()
        {
            IInspectionPanel inspectionPanel = SystemManager.Instance().UiChanger.CreateInspectionPanel(0);
            inspectionPanelList.Add(inspectionPanel);
            viewContainer.Controls.Add((UserControl)inspectionPanel);

            ((UserControl)inspectionPanel).Dock = DockStyle.Fill;
            inspectionPanel.Initialize();
        }

        public void UpdatePage()
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectionPage - UpdatePage");

            if (SystemManager.Instance().CurrentModel == null)
            {
                modelName.Text = "";

                productionNg.Text = "";
                productionGood.Text = "";
                productionTotal.Text = "";
            }
            else if (modelName.Text != SystemManager.Instance().CurrentModel.Name)
            {
                modelName.Text = SystemManager.Instance().CurrentModel.Name;
                
                offlineImagePathList = new string[1] { Path.Combine(SystemManager.Instance().CurrentModel.ModelPath, "Image") };

                //SystemManager.Instance().CurrentModel.LoadProduction();

                //productionSingle = SystemManager.Instance().CurrentModel.ProductionList[0];

                productionNg.Text = productionSingle.Ng.ToString();
                productionGood.Text = productionSingle.Good.ToString();
                productionTotal.Text = productionSingle.Total.ToString();

                inspectionPanelList.ForEach(panel => panel.ClearPanel());
            }
        }

        delegate void PrepareInspectionDelegate();

        //public void PrepareInspection()
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new PrepareInspectionDelegate(PrepareInspection));
        //        return;
        //    }

        //    inspectionPanelIf.PrepareInspection();
        //}

        //public void InspectionFinished(InspectionResult inspectionResult)
        //{
        //    if (inspectionResult.Judgment == Judgment.Accept)
        //        ChangeInspectionStatus(InspectionState.Good);
        //    else if (inspectionResult.Judgment == Judgment.FalseReject)
        //        inspectionPage.ChangeInspectionStatus(InspectionState.FalseReject);
        //    else
        //        inspectionPage.ChangeInspectionStatus(InspectionState.NG);
        //}

        private void ShowCount()
        {

        }

        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new InspectionStepInspectedDelegate(InspectionStepInspected), inspectionStep, sequenceNo, inspectionResult);
                return;
            }

            inspectionPanelList.ForEach(panel => panel.InspectionStepInspected(inspectionStep, sequenceNo, inspectionResult));
        }

        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult)
        {
            if (InvokeRequired)
            {
                Invoke(new TargetGroupInspectedDelegate(TargetGroupInspected), targetGroup, inspectionResult, objectInspectionResult);
                return;
            }

            inspectionPanelList.ForEach(panel => panel.TargetGroupInspected(targetGroup, inspectionResult, objectInspectionResult));
        }

        public void TargetInspected(Target target, InspectionResult targetInspectionResult)
        {
            if (InvokeRequired)
            {
                IAsyncResult result = BeginInvoke(new TargetInspectedDelegate(TargetInspected), target, targetInspectionResult);
                return;
            }

            inspectionPanelList.ForEach(panel => panel.TargetInspected(target, targetInspectionResult));
        }

        delegate void ProductInspectedDelegate(InspectionResult inspectionResult);

        public void ProductInspected(InspectionResult inspectionResult)
        {
            inspectionResult.InspectionEndTime = DateTime.Now;

            if (InvokeRequired)
            {
                if (OperationSettings.Instance().ShowFinalImage == true)
                {
                    LogHelper.Debug(LoggerType.Inspection, "Product Inspected - Start Final Grab");

                    InspectionStep inspectionStep = SystemManager.Instance().CurrentModel.GetInspectionStep(0);
                    inspectionStep.UpdateImageBuffer(imageAcquisition.ImageBuffer);

                    imageAcquisition.Acquire(inspectionStep.StepNo, 0);

                    LogHelper.Debug(LoggerType.Inspection, "Product Inspected - End Final Grab");
                }

                LogHelper.Debug(LoggerType.Inspection, "Add InspectionResultList");

                inspectionResultList.Add(inspectionResult);

                LogHelper.Debug(LoggerType.Inspection, "Begin Invoke ProductInspected");
                Invoke(new ProductInspectedDelegate(ProductInspected), inspectionResult);

                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "Product Insepected");

            inspectionPanelList.ForEach(panel => panel.ProductInspected(inspectionResult));

            UpdateProductionInfo(inspectionResult);

            buttonResetCount.Enabled = true;

            //ShowResult(inspectionResult);

            if (inspectionResultList.IndexOf(inspectionResult) > -1)
            {
                inspectionResultList.Remove(inspectionResult);
            }
        }

        delegate void OnPreInspectionDelegate();
        public void OnPreInspection()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new OnPreInspectionDelegate(OnPreInspection));
                return;
            }
        }

        delegate void OnPostInspectionDelegate();
        public void OnPostInspection()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new OnPostInspectionDelegate(OnPostInspection));
                return;
            }
        }

        delegate void UpdateProductionInfoDelegate(InspectionResult inspectionResult);

        private void UpdateProductionInfo(InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateProductionInfoDelegate(UpdateProductionInfo), inspectionResult);
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "InspectionPage - UpdateProductionInfo");

            TimeSpan inspectionProcessTime = inspectionResult.GetInspectionProcessTime();
            inspTime.Text =string.Format("{0:00}:{1:00}.{2:000}", inspectionProcessTime.Minutes, inspectionProcessTime.Seconds, inspectionProcessTime.Milliseconds);
            if (inspectionResult.InspectionNo != null)
                inspectionNo.Text = inspectionResult.InspectionNo.ToString();

            if (inspectionResult.IsGood() == true)
            {
                this.productionSingle.AddGood();
            }
            else
            {
                this.productionSingle.AddNG();
            }

            productionNg.Text = this.productionSingle.Ng.ToString();
            productionGood.Text = this.productionSingle.Good.ToString();
            productionTotal.Text = this.productionSingle.Total.ToString();
        }
        
        private void EnterWaitInspection()
        {
            if (MachineSettings.Instance().RunningMode == RunningMode.Virtual)
            {
                if (offlineImagePathList == null)
                {
                    MessageBox.Show("Offline image path is not defined.");
                    return;
                }

                offlineImageIndex++;
                if (offlineImageIndex >= offlineImagePathList.Count())
                    offlineImageIndex = 0;
            }

            inspectionPanelList.ForEach(panel => panel.EnterWaitInspection());

            if (SystemManager.Instance().InspectRunner.EnterWaitInspection() == false)
                return;

            buttonResetCount.Enabled = false;
        }

        private void ExitWaitInspection()
        {
            inspectionPanelList.ForEach(panel => panel.ExitWaitInspection());

            SystemManager.Instance().InspectRunner.ExitWaitInspection();

            buttonResetCount.Enabled = true;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (SystemState.Instance().OpState == OpState.Idle)
            {
                EnterWaitInspection();
                buttonStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Start_90x116;
            }
            else
            {
                ExitWaitInspection();
                buttonStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Stop_90x116;
            }
        }
        
        
        //public void UpdateImage()
        //{
        //    LogHelper.Debug(LoggerType.Inspection, "InspectionPage - UpdateImage");

        //    InspectionStep inspectionStep = SystemManager.Instance().CurrentModel.GetInspectionStep(0);
        //    inspectionStep.UpdateImageBuffer(imageAcquisition.ImageBuffer);

        //    if (lightCtrlHandler != null)
        //    {
        //        LightValueList lightValueList = imageAcquisition.ImageBuffer.GetLightValueList(0);
        //        lightCtrlHandler.TurnOn(lightValueList[0]);
        //    }
            
        //    imageAcquisition.Acquire(inspectionStep.StepNo, 0);

        //    int numOfCameraImage = Math.Min(OperationSettings.Instance().NumOfResultView, MachineSettings.Instance().NumCamera);

        //    for (int i = 0; i < numOfCameraImage; i++)
        //    {
        //        Bitmap preBitmap = resultView[i].Image;

        //        ImageDevice imageDevice = SystemManager.Instance().Machine.ImageDeviceHandler.GetImageDevice(i);

        //        resultView[i].ZoomScale = -1;

        //        if (imageDevice.IsDepthScanner())
        //        {
        //            resultView[i].Image3d = imageAcquisition.ImageBuffer.GetImageBuffer3dItem(i).ResultImage;
        //            resultView[i].MouseDoubleClicked = resultView_mouseDoubleClicked;
        //        }
        //        else
        //        {
        //            resultView[i].Image = imageAcquisition.ImageBuffer.GetImageBuffer2dItem(i, 0).Image.ToBitmap();
        //            resultView[i].MouseDoubleClicked = null;
        //        }

        //        if (preBitmap != null)
        //            preBitmap.Dispose();
        //    }
        //}
        
        delegate void UpdateInspectionNoDelegate(string serialNo);
        public void UpdateInspectionNo(string serialNo)
        {
            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Inspection, "UpdateInspectionNo -> Delegated");

                BeginInvoke(new UpdateInspectionNoDelegate(UpdateInspectionNo), serialNo);
                return;
            }

            lock (inspectionNo)
            {
                inspectionNo.Text = serialNo;
            }
        }

        private void buttonResetCount_Click(object sender, EventArgs e)
        {
            string message = StringManager.GetString(this.GetType().FullName, "Inspection count will be Reset. Continue?");
            DialogResult result = MessageForm.Show(this.ParentForm, message, MessageFormType.YesNo);
            if(result != DialogResult.Yes)
            {
                return;
            }

            productionSingle = null;
            //productionSingle.Reset();
            productionNg.Text = "0";
            productionGood.Text = "0";
            productionTotal.Text = "0";
            SystemManager.Instance().InspectRunner.ResetState(); // 치약 바코드, 샴푸바코드는 IO와 검사 결과까지 모두 초기화 해줘야 한다.
        }

        private void buttonSelectImage_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string[] extensions = { ".jpg", ".gif", ".png", ".bmp" };
                string[] fileNames = Directory.GetFiles(dialog.SelectedPath, "Image_*.*").Where(f => extensions.Contains(new FileInfo(f).Extension.ToLower())).ToArray();
                if (fileNames.Count() > 0)
                {
                    offlineImagePathList = new string[1] { dialog.SelectedPath };
                }
                else
                {
                    offlineImagePathList = Directory.GetFiles(dialog.SelectedPath, "*.*");
                }

                offlineImageIndex = 0;
            }
        }

        void UpdateStatusLabel(string text, Color foreColor, Color backColor)
        {
            labelStatus.BackColor = backColor;
            labelStatus.ForeColor = foreColor;
            labelStatus.Text = StringManager.GetString(this.GetType().FullName,text);
        }

        public void ChangeStatus() 
        {
            if (!ErrorManager.Instance().IsCleared())
            {
                UpdateStatusLabel("Alarm", Color.White, Color.Red);
            }
            else
            {
                switch (SystemState.Instance().OpState)
                {
                    case OpState.Inspect:
                        switch (SystemState.Instance().InspectState)
                        {
                            case InspectState.Done:
                                switch (SystemState.Instance().InspectionResult)
                                {
                                    case Judgment.Accept:
                                        UpdateStatusLabel("Good", Color.Black, Color.LimeGreen);
                                        break;
                                    case Judgment.FalseReject:
                                        UpdateStatusLabel("Overkill", Color.Black, Color.Yellow);
                                        break;
                                    case Judgment.Reject:
                                        UpdateStatusLabel("NG", Color.White, Color.Red);
                                        break;
                                }
                                break;
                            case InspectState.Run:
                                UpdateStatusLabel("Inspecting", Color.Black, Color.CornflowerBlue);
                                break;

                        }
                        break;
                    case OpState.Wait:
                        UpdateStatusLabel("Waitting", Color.Black, Color.CornflowerBlue);
                        break;
                    case OpState.Idle:
                        UpdateStatusLabel("Idle", Color.Black, Color.Gray);
                        break;
                }
            }
        }

        public void Load2dImage(int cameraIndex, int stepIndex, int lightTypeIndex)
        {
            ImageDevice imageDevice = imageAcquisition.ImageDeviceHandler.GetImageDevice(cameraIndex);
            Image2D sourceImage2d = (Image2D)imageAcquisition.ImageBuffer.GetImageBuffer2dItem(cameraIndex, lightTypeIndex).Image;

            int index = offlineImageIndex - 1;
            if (index == -1)
                index = offlineImagePathList.Count();
            if (index == offlineImagePathList.Count())
                index = 0;

            string imagePath = offlineImagePathList[index];
            string searchImageFileName = ImageBuffer.GetImage2dFileName(cameraIndex, stepIndex, lightTypeIndex, System.Drawing.Imaging.ImageFormat.Bmp, "*");

            string[] imageFiles = null;
            if (Directory.Exists(imagePath) == true)
            {
                imageFiles = Directory.GetFiles(imagePath, searchImageFileName);
            }
            else
            {
                imagePath = string.Format(@"{0}\Image", SystemManager.Instance().CurrentModel.ModelPath);
                imageFiles = Directory.GetFiles(imagePath, searchImageFileName);
            }

            if (imageFiles != null && imageFiles.Count() == 0)
            {
                string modelImgaePath = string.Format(@"{0}\Image", SystemManager.Instance().CurrentModel.ModelPath);
                if (Directory.Exists(modelImgaePath))
                    imageFiles = Directory.GetFiles(modelImgaePath, searchImageFileName);
            }

            if (imageFiles != null && imageFiles.Count() > 0)
            {
                string imageFilePath = Path.Combine(imagePath, imageFiles[0]);
                Image2D fileImage = new Image2D(imageFilePath);
                if (imageDevice.IsCompatibleImage(fileImage) == true)
                {
                    sourceImage2d = fileImage;
                    //imageAcquisition.ImageBuffer.GetImageBuffer2dItem(cameraIndex, lightTypeIndex).Image;
                    imageAcquisition.ImageBuffer.Set2dImage(cameraIndex, lightTypeIndex, sourceImage2d);
                }
                else
                    sourceImage2d.Clear();
            }
            else
            {
                sourceImage2d.Clear();
            }
            //#if DEBUG
            //            string saveDebugImagePath = string.Format("{0}\\{1}.bmp", Configuration.TempFolder, "Load2dImage");
            //            sourceImage2d.SaveImage(saveDebugImagePath, ImageFormat.Bmp);
            //#endif 
        }

        private void labelLiveView_Click(object sender, EventArgs e)
        {

        }

        private void InspectionPage_Load(object sender, EventArgs e)
        {

        }

        private void buttonTrigger_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabOnce();
        }

        public void EnableControls(UserType user)
        {

        }

        public void PageVisibleChanged(bool visible)
        {
        }

        public Production GetCurrentProduction()
        {
            throw new NotImplementedException();
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }
    }
}
