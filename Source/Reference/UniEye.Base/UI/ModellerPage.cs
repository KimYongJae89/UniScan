using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI.Touch;
using DynMvp.Data.UI;
using DynMvp.Vision;
using DynMvp.UI;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Data.Forms;
using DynMvp.Devices;
using System.Threading;
using DynMvp.Devices.UI;
using DynMvp.Devices.MotionController;
using DynMvp.Device.UI;
using DynMvp.InspData;
using System.Diagnostics;
using DynMvp.Inspection;
using DynMvp.Authentication;
using UniEye.Base.Settings;
using UniEye.Base.Command;
using UniEye.Base.Inspect;
using UniEye.Base.Data;

namespace UniEye.Base.UI
{
    public partial class ModellerPage : UserControl, IMainTabPage, ITeachPage
    {
        enum eTabPage
        {
            TabPageStep, TabPageCamera, TabPageProbe, TabPageView, TabPageMax
        }

        CommandManager commandManager = new CommandManager();

        Thread multiShotThread;
        PositionAligner positionAligner = new PositionAligner();

        string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value;  }
        }
        
        private TeachBox teachBox;
        public TeachBox TeachBox
        {
            get { return teachBox; }
        }

        private TeachHandlerProbe teachHandlerProbe;

        private TargetParamControl targetParamControl;
        private TryInspectionResultView2 tryInspectionResultView;
        private FovNavigator fovNavigator;
        private System.Windows.Forms.TabPage tabPageFovNavigator;
        private ILightParamForm lightParamForm;

        private InspectionResult modellerInspectionResult = new InspectionResult();
        private InspectionResult lastInspectionResult = null;
        private const int padding = 3;

        public ValueChangedDelegate ValueChanged = null;

        bool modified = false;
        private bool lockGrab = true;
        bool onValueUpdate = false;

        Image2D sourceImage2d;
        Image3D sourceImage3d;
        int deviceIndex = 0;
        int stepIndex = 0;
        int lightTypeIndex = 0;
        int previewIndex = 0;

        InspectionStep curInspectionStep;
        TargetGroup curTargetGroup;

        bool onPreviewMode = false;
        bool onLiveGrab = false;

        private int offlineImageIndex = 0;
        private string[] offlineImagePathList = null;

        ContextMenu objectContextMenu = new ContextMenu();

        ModellerPageExtender modellerPageExtender;

        JoystickAxisForm joystick;
        private AxisPosition currentPosition = new AxisPosition();

        CancellationTokenSource cancellationTokenSource;

        public ModellerPage()
        {
            InitializeComponent();

            this.tryInspectionResultView = new TryInspectionResultView2();
            this.teachBox = new TeachBox();
            this.targetParamControl = new TargetParamControl();

            teachHandlerProbe = new TeachHandlerProbe();

            this.SuspendLayout();

            objectContextMenu.Popup += objectContextMenu_Popup;
            objectContextMenu.MenuItems.Add(new MenuItem("Select Target", onClickSelectTarget));

            ultraTabPageCamera.Controls.Add(this.teachBox);

            this.teachBox.TeachHandler = this.teachHandlerProbe;
            this.teachBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.teachBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.teachBox.Location = new System.Drawing.Point(3, 3);
            this.teachBox.Name = "targetImage";
            this.teachBox.Size = new System.Drawing.Size(409, 523);
            this.teachBox.TabIndex = 8;
            this.teachBox.TabStop = false;
            this.teachBox.Enable = true;
            this.teachBox.RotationLocked = false;
            this.teachBox.ContextMenu = objectContextMenu;
            this.teachBox.InspectionResultSelected = modellerInspectionResult;
            this.teachBox.PositionShifted += teachBox_PositionShifted;
            this.teachBox.ObjectSelected = teachBox_Selected;
            this.teachBox.ObjectMultiSelected = teachBox_MultiSelected;
            this.teachBox.ObjectMoved = new ObjectMoved(teachBox_Moved);
            this.teachBox.ObjectAdded = new ObjectAdded(teachBox_Added);
            this.teachBox.MouseClicked += teachBox_MouseClicked;

            this.tabPageResult.Controls.Add(this.tryInspectionResultView);

            this.tryInspectionResultView.Location = new System.Drawing.Point(96, 95);
            this.tryInspectionResultView.Name = "tryInspectionResultView";
            this.tryInspectionResultView.Size = new System.Drawing.Size(74, 101);
            this.tryInspectionResultView.TabIndex = 0;
            this.tryInspectionResultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tryInspectionResultView.TeachHandlerProbe = teachHandlerProbe;
            this.tryInspectionResultView.TryInspectionResultCellClicked = UpdateImageFigure;

            this.paramContainer.Panel1.Controls.Add(this.targetParamControl);

            if (OperationSettings.Instance().UseSimpleLightParamForm)
            {
                LightParamSimpleForm lightParamSimpleForm = new LightParamSimpleForm();
                lightParamSimpleForm.Location = new System.Drawing.Point(96, 95);
                lightParamSimpleForm.Name = "tryInspectionResultView";
                lightParamSimpleForm.TabIndex = 0;
                lightParamSimpleForm.LightTypeChanged += lightParamPanel_LightTypeChanged;
                lightParamSimpleForm.LightValueChanged += lightParamPanel_LightValueChanged;
                lightParamSimpleForm.Visible = false;

                lightParamSimpleForm.TopMost = true;

                lightParamForm = lightParamSimpleForm;
            }
            else
            {
                LightParamForm defaultLightParamForm = new LightParamForm();
                defaultLightParamForm.Location = new System.Drawing.Point(96, 95);
                defaultLightParamForm.Name = "tryInspectionResultView";
                defaultLightParamForm.TabIndex = 0;
                defaultLightParamForm.LightTypeChanged += lightParamPanel_LightTypeChanged;
                defaultLightParamForm.LightValueChanged += lightParamPanel_LightValueChanged;
                defaultLightParamForm.Visible = false;

                defaultLightParamForm.TopMost = true;

                lightParamForm = defaultLightParamForm;
            }

            // 
            // chargerTargetParamControl
            // 
            this.targetParamControl.Location = new System.Drawing.Point(96, 95);
            this.targetParamControl.Name = "targetParamControl";
            this.targetParamControl.Size = new System.Drawing.Size(74, 101);
            this.targetParamControl.TabIndex = 0;
            this.targetParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.targetParamControl.TeachHandlerProbe = teachHandlerProbe;
            this.targetParamControl.ValueChanged = new ValueChangedDelegate(ParamControl_ValueChanged);
            this.targetParamControl.CommandManager = commandManager;

            this.ResumeLayout(false);

            // change language
            inspectionButton.Text = StringManager.GetString(this.GetType().FullName,inspectionButton.Text);
            addProbeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,addProbeToolStripButton.Text);
            copyProbeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,copyProbeToolStripButton.Text);
            pasteProbeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,pasteProbeToolStripButton.Text);
            deleteProbeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,deleteProbeToolStripButton.Text);
            setTargetCalibrationToolStripButton.Text = StringManager.GetString(this.GetType().FullName,setTargetCalibrationToolStripButton.Text);
            setFiducialToolStripButton.Text = StringManager.GetString(this.GetType().FullName,setFiducialToolStripButton.Text);
            syncParamToolStripButton.Text = StringManager.GetString(this.GetType().FullName,syncParamToolStripButton.Text);
            syncAllToolStripButton.Text = StringManager.GetString(this.GetType().FullName,syncAllToolStripButton.Text);
            zoomInToolStripButton.Text = StringManager.GetString(this.GetType().FullName,zoomInToolStripButton.Text);
            zoomOutToolStripButton.Text = StringManager.GetString(this.GetType().FullName,zoomOutToolStripButton.Text);
            zoomFitToolStripButton.Text = StringManager.GetString(this.GetType().FullName,zoomFitToolStripButton.Text);
            grabProcessToolStripButton.Text = StringManager.GetString(this.GetType().FullName,grabProcessToolStripButton.Text);
            showLightPanelToolStripButton.Text = StringManager.GetString(this.GetType().FullName,showLightPanelToolStripButton.Text);
            loadImageSetToolStripButton.Text = StringManager.GetString(this.GetType().FullName,loadImageSetToolStripButton.Text);
            selectPrevImageSetToolStripButton.Text = StringManager.GetString(this.GetType().FullName,selectPrevImageSetToolStripButton.Text);
            selectNextImageSetToolStripButton.Text = StringManager.GetString(this.GetType().FullName,selectNextImageSetToolStripButton.Text);
            singleShotToolStripButton.Text = StringManager.GetString(this.GetType().FullName,singleShotToolStripButton.Text);
            multiShotToolStripButton.Text = StringManager.GetString(this.GetType().FullName,multiShotToolStripButton.Text);
            addStepButton.Text = StringManager.GetString(this.GetType().FullName,addStepButton.Text);
            deleteStepButton.Text = StringManager.GetString(this.GetType().FullName,deleteStepButton.Text);
            groupProbeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,groupProbeToolStripButton.Text);
            ungroupProbeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,ungroupProbeToolStripButton.Text);
            tabPageFov.Text = StringManager.GetString(this.GetType().FullName,tabPageFov.Text);
            tabPageProbe.Text = StringManager.GetString(this.GetType().FullName,tabPageProbe.Text);
            tabPageView.Text = StringManager.GetString(this.GetType().FullName,tabPageView.Text);

            saveButton.Text = StringManager.GetString(this.GetType().FullName,saveButton.Text);
            resetModelButton.Text = StringManager.GetString(this.GetType().FullName,resetModelButton.Text);
            importGerberButton.Text = StringManager.GetString(this.GetType().FullName,importGerberButton.Text);
            modelPropertyButton.Text = StringManager.GetString(this.GetType().FullName,modelPropertyButton.Text);
            exportFormatButton.Text = StringManager.GetString(this.GetType().FullName,exportFormatButton.Text);
            editStepButton.Text = StringManager.GetString(this.GetType().FullName,editStepButton.Text);

            toolStripButtonOrigin.Text = StringManager.GetString(this.GetType().FullName,toolStripButtonOrigin.Text);
            toolStripButtonJoystick.Text = StringManager.GetString(this.GetType().FullName,toolStripButtonJoystick.Text);
            toolStripButtonRobotSetting.Text = StringManager.GetString(this.GetType().FullName,toolStripButtonRobotSetting.Text);
            grabProcessToolStripButton.ToolTipText = StringManager.GetString(this.GetType().FullName,grabProcessToolStripButton.ToolTipText);
            showLightPanelToolStripButton.ToolTipText = StringManager.GetString(this.GetType().FullName,showLightPanelToolStripButton.ToolTipText);
            multiShotToolStripButton.ToolTipText = StringManager.GetString(this.GetType().FullName,multiShotToolStripButton.ToolTipText);
            singleShotToolStripButton.ToolTipText = StringManager.GetString(this.GetType().FullName,singleShotToolStripButton.ToolTipText);
            dontMoveToolStripButton.ToolTipText = StringManager.GetString(this.GetType().FullName,dontMoveToolStripButton.ToolTipText);

            dontMoveToolStripButton.Text = StringManager.GetString(this.GetType().FullName,dontMoveToolStripButton.Text);
            toolStripButtonStop.Text = StringManager.GetString(this.GetType().FullName,toolStripButtonStop.Text);

            // 다국어지원
            // Step
            tabControlMain.TabPages[0].Text = StringManager.GetString(this.GetType().FullName,tabControlMain.TabPages[0].Text);
            addStepButton.Text = StringManager.GetString(this.GetType().FullName,addStepButton.Text);
            deleteStepButton.Text = StringManager.GetString(this.GetType().FullName,deleteStepButton.Text);

            // Camera
            tabControlMain.TabPages[1].Text = StringManager.GetString(this.GetType().FullName,tabControlMain.TabPages[1].Text);
            selectCameraButton.Text = StringManager.GetString(this.GetType().FullName,selectCameraButton.Text);
            grabProcessToolStripButton.Text = StringManager.GetString(this.GetType().FullName,grabProcessToolStripButton.Text);
            showLightPanelToolStripButton.Text = StringManager.GetString(this.GetType().FullName,showLightPanelToolStripButton.Text);
            loadImageSetToolStripButton.Text = StringManager.GetString(this.GetType().FullName,loadImageSetToolStripButton.Text);
            selectPrevImageSetToolStripButton.Text = StringManager.GetString(this.GetType().FullName,selectPrevImageSetToolStripButton.Text);
            selectNextImageSetToolStripButton.Text = StringManager.GetString(this.GetType().FullName,selectNextImageSetToolStripButton.Text);
            singleShotToolStripButton.Text = StringManager.GetString(this.GetType().FullName,singleShotToolStripButton.Text);
            multiShotToolStripButton.Text = StringManager.GetString(this.GetType().FullName,multiShotToolStripButton.Text);

            // Probe
            tabControlMain.TabPages[2].Text = StringManager.GetString(this.GetType().FullName,tabControlMain.TabPages[2].Text);
            addProbeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,addProbeToolStripButton.Text);
            copyProbeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,copyProbeToolStripButton.Text);
            pasteProbeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,pasteProbeToolStripButton.Text);
            deleteProbeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,deleteProbeToolStripButton.Text);
            setFiducialToolStripButton.Text = StringManager.GetString(this.GetType().FullName,setFiducialToolStripButton.Text);
            setTargetCalibrationToolStripButton.Text = StringManager.GetString(this.GetType().FullName,setTargetCalibrationToolStripButton.Text);
            syncParamToolStripButton.Text = StringManager.GetString(this.GetType().FullName,syncParamToolStripButton.Text);
            syncAllToolStripButton.Text = StringManager.GetString(this.GetType().FullName,syncAllToolStripButton.Text);
            addFiducialToolStripButton.Text = StringManager.GetString(this.GetType().FullName,addFiducialToolStripButton.Text);
            deleteFiducialToolStripButton.Text = StringManager.GetString(this.GetType().FullName,deleteFiducialToolStripButton.Text);
            toggleFiducialToolStripButton.Text = StringManager.GetString(this.GetType().FullName,toggleFiducialToolStripButton.Text);

            // View
            tabControlMain.TabPages[3].Text = StringManager.GetString(this.GetType().FullName,tabControlMain.TabPages[3].Text);
            previewTypeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,previewTypeToolStripButton.Text);
            zoomInToolStripButton.Text = StringManager.GetString(this.GetType().FullName,zoomInToolStripButton.Text);
            zoomOutToolStripButton.Text = StringManager.GetString(this.GetType().FullName,zoomOutToolStripButton.Text);
            zoomFitToolStripButton.Text = StringManager.GetString(this.GetType().FullName,zoomFitToolStripButton.Text);

            teachHandlerProbe.SingleTargetMode = OperationSettings.Instance().UseSingleTargetModel;

            ChangeMenuState();
        }

        private void teachBox_MouseClicked(DrawBox senderView, Point clickPos, ref bool processingCancelled)
        {
            targetParamControl.PointSelected(clickPos, ref processingCancelled);
        }

        private void onClickSelectTarget(object sender, EventArgs e)
        {
            List<Target> targetList = teachHandlerProbe.GetTargetList();

            UpdateImageFigure();

            teachHandlerProbe.ClearSelection();
            teachBox.DrawBox.ResetSelection();

            foreach (Target target in targetList)
            {
                teachHandlerProbe.Select(target);
                teachBox.DrawBox.SelectFigureByTag(target);
            }
        }

        private void objectContextMenu_Popup(object sender, EventArgs e)
        {
            objectContextMenu.MenuItems[0].Enabled = teachHandlerProbe.IsSelected();
        }

        private void UpdateLightTypeCombo()
        {
            LightParamSet lightParamSet = LightSettings.Instance().LightParamSet;
            if (curTargetGroup != null)
            {
                lightParamSet = curTargetGroup.GetLightParamSet();
            }

            LogHelper.Debug(LoggerType.Operation, "InitLightTypeList");

            int numLightType = lightParamSet.NumLightType;

            selectLightButton.DropDownItems.Clear();
            for (int i = 0; i < LightSettings.Instance().NumLightType; i++)
            {
                if(i >= lightParamSet.LightParamList.Count)
                {
                    lightParamSet.LightParamList.Add(new LightParam(0));
                }
                string lightTypeName = lightParamSet.LightParamList[i].Name;
                ToolStripItem lightToolStripItem = selectLightButton.DropDownItems.Add(lightTypeName);
                lightToolStripItem.Tag = i;
                lightToolStripItem.Click += lightToolStripItem_Click;
            }
                                                            
            if (selectLightButton.DropDownItems.Count > 0 && this.lightTypeIndex>=0)
            {
                selectLightButton.Text = selectLightButton.DropDownItems[this.lightTypeIndex].Text;
            }

            if (selectLightButton.DropDownItems.Count == 1)
                selectLightButton.Visible = false;
        }

        private void lightToolStripItem_Click(object sender, EventArgs e)
        {
            ToolStripItem toolStripItem = (ToolStripItem)sender;
            int lightTypeIndex = (int)toolStripItem.Tag;
            
            UpdatePage(this.stepIndex, this.deviceIndex, lightTypeIndex);
            UpdateLightTypeCombo();
        }

        private void ModellerPage_Load(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.StartUp, "Begin ModellerPage_Load");

            InitCameraToolstripButton();

            UpdateLightTypeCombo();
            lightParamForm.InitControls();

            lockGrab = false;

            SystemManager systemManager = SystemManager.Instance();
            ImageDeviceHandler imageDeviceHandler = systemManager.DeviceBox.ImageDeviceHandler;
            ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(0);
            AxisHandler robotStage = systemManager.DeviceController.RobotStage;
            if (robotStage != null)
            {
                joystick = new JoystickAxisForm(robotStage, imageDevice, systemManager.DeviceBox.LightCtrlHandler);
            }

            calibration3dButton.Visible = false;
            mainTabView.Controls.Remove(ultraTabPage3dViewer);

            if (robotStage == null)
            {
                tabControlMain.TabPages.RemoveAt(4);
                tabPageRobot.Hide();
            }

            modellerPageExtender = systemManager.UiChanger.CreateModellerPageExtender();

            modellerPageExtender.InspectionResult = modellerInspectionResult;

            if (robotStage != null)
            {
                this.tabPageFovNavigator = new TabPage();

                this.tabControlUtil.Controls.Add(this.tabPageFovNavigator);

                this.tabPageFovNavigator.Location = new System.Drawing.Point(4, 29);
                this.tabPageFovNavigator.Name = "tabPageFovNavigator";
                this.tabPageFovNavigator.Padding = new System.Windows.Forms.Padding(3);
                this.tabPageFovNavigator.Size = new System.Drawing.Size(490, 96);
                this.tabPageFovNavigator.TabIndex = 0;
                this.tabPageFovNavigator.Text = "FOV Navigator";
                this.tabPageFovNavigator.UseVisualStyleBackColor = true;

                fovNavigator = new FovNavigator();

                this.tabPageFovNavigator.Controls.Add(this.fovNavigator);

                this.fovNavigator.Location = new System.Drawing.Point(96, 95);
                this.fovNavigator.Name = "fovNavigator";
                this.fovNavigator.Size = new System.Drawing.Size(74, 101);
                this.fovNavigator.TabIndex = 0;
                this.fovNavigator.Dock = System.Windows.Forms.DockStyle.Fill;
                this.fovNavigator.RobotStage = robotStage;
                this.fovNavigator.Enable = true;
                this.fovNavigator.FovChanged += new FovChangedDelegate(this.fovNavigator_FovChanged);


                if (imageDevice != null && imageDevice is Camera)
                    this.fovNavigator.FovSize = ((Camera)imageDevice).FovSize;
            }
            ChangeControl();
            
            LogHelper.Debug(LoggerType.StartUp, "End ModellerPage_Load");
        }

        private void fovNavigator_FovChanged(int fovNo, PointF position)
        {
            if (InvokeRequired)
            {
                Invoke(new FovChangedDelegate(fovNavigator_FovChanged), fovNo, position);
                return;
            }

            LogHelper.Debug(LoggerType.Operation, String.Format("Change FOV - {0}", fovNo + 1));

            DynMvp.Data.Model currentModel = (DynMvp.Data.Model)SystemManager.Instance().CurrentModel;
            if (fovNo > -1)
            {
                InspectionStep selectedInspectionStep = currentModel.GetInspectionStep(fovNo/*, true*/);
                Debug.Assert(selectedInspectionStep != null, "Invalid Model Format. InspectionStep must have value");

                comboStep.SelectedIndex = selectedInspectionStep.StepNo;
            }
            else
            {
                SystemManager.Instance().DeviceController.RobotStage?.Move(new AxisPosition(position.X, position.Y));
            }
        }

        private void InitCameraToolstripButton()
        {
            LogHelper.Debug(LoggerType.Operation, "InitCameraToolstripList");

            selectCameraButton.DropDownItems.Clear();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                
                ToolStripButton toolStripCameraButton = new ToolStripButton(imageDevice.Name);
                toolStripCameraButton.AutoSize = true;
                toolStripCameraButton.Click += toolStripCameraButton_Click;
                selectCameraButton.DropDownItems.Add(toolStripCameraButton);
            }
            
            selectCameraButton.DropDown.Width = 200;
            selectCameraButton.DropDown.Height = 1000;

            if (selectCameraButton.DropDownItems.Count > 0)
            {
                if (selectCameraButton.DropDownItems.Count == 1)
                    selectCameraButton.Visible = false;

                deviceIndex = 0;
                selectCameraButton.Text = selectCameraButton.DropDownItems[0].Text;
            }
        }

        private void toolStripCameraButton_Click(object sender, EventArgs e)
        {
            ToolStripButton toolStripCameraItem = (ToolStripButton)sender;
            selectCameraButton.Text = toolStripCameraItem.Text;

            int cameraIndex = selectCameraButton.DropDownItems.IndexOf(toolStripCameraItem);
            this.deviceIndex = cameraIndex;
            teachBox.AutoFit(true);
            UpdatePage(stepIndex, cameraIndex, lightTypeIndex);
            teachBox.AutoFit(false);

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(cameraIndex);
            calibration3dButton.Enabled = imageDevice.IsDepthScanner();
        }

        public void Initialize()
        {
            LogHelper.Debug(LoggerType.StartUp, "Begin ModellerPage::Initlaize");

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            lightParamForm.Initialize(imageDeviceHandler);

            BuildAlgorithmTypeMenu();

            LogHelper.Debug(LoggerType.StartUp, "Begin ModellerPage::Initlaize End");
        }

        private void UpdateImageFigure()
        {
            teachBox.ShowCenterGuide = OperationSettings.Instance().ShowCenterGuide;
            if (teachBox.ShowCenterGuide)
            {
                teachBox.DrawBox.CenterGuidePos = OperationSettings.Instance().CenterGuidePos;
                teachBox.DrawBox.CenterGuideThickness = OperationSettings.Instance().CenterGuideThickness;
            }

            teachBox.UpdateFigure();
        }

        internal void Scan()
        {
            throw new NotImplementedException();
        }

        delegate void ObjectSelectedDelegate(ImageD objectImage);

        public void teachBox_Selected(ImageD objectImage)
        {
            if (InvokeRequired)
            {
                Invoke(new ObjectSelectedDelegate(teachBox_Selected), objectImage);
                return;
            }
            
            setFiducialToolStripButton.Checked = false;
            setTargetCalibrationToolStripButton.Checked = false;
            
            
            if (objectImage == null && OperationSettings.Instance().UseSingleTargetModel == false)
            {
                UpdateImage();
                targetParamControl.ClearProbeData();
                return;
            }

            ITeachObject teachObject = teachHandlerProbe.GetSingleSelected();
            if (teachObject != null)
            {
                targetParamControl.SelectObject(teachObject);

                if (teachObject is VisionProbe)
                {
                    VisionProbe visionProbe = (VisionProbe)teachObject;

                    Probe alignmentProbe = visionProbe.Target.GetAlignmentProbe();
                    UpdateToggleFiducialToolStripButtonColor(alignmentProbe);

                    LogHelper.Debug(LoggerType.Operation, "ModellerPage - Update Preview Type");

                    previewTypeToolStripButton.DropDownItems.Clear();

                    foreach (string previewName in visionProbe.GetPreviewNames())
                    {
                        ToolStripButton previewButton = new ToolStripButton(previewName);
                        previewButton.Click += PreviewToolStripButton_Click;
                        previewTypeToolStripButton.DropDownItems.Add(previewButton);
                    }

                    previewTypeToolStripButton.Text = previewTypeToolStripButton.DropDownItems[0].Text;

                    if (visionProbe.ActAsFiducialProbe == true)
                        setFiducialToolStripButton.Checked = true;
                    else if (visionProbe.ActAsCalibrationProbe == true)
                        setTargetCalibrationToolStripButton.Checked = true;

                    //lightParamForm.SetLightTypeIndex(visionProbe.LightTypeIndex);
                    UpdatePage(this.stepIndex, this.deviceIndex, visionProbe.LightTypeIndex);
                }
                else
                {
                    UpdateImageFigure();
                }
            }

            UpdateButtonState();
        }

        delegate void ObjectMultiSelectedDelegate();

        public void teachBox_MultiSelected()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ObjectMultiSelectedDelegate(teachBox_MultiSelected));
                return;
            }

            setFiducialToolStripButton.Checked = false;
            setTargetCalibrationToolStripButton.Checked = false;

            ITeachObject firstObject = teachHandlerProbe.GetFirstSelectedProbe();
            if (firstObject != null)
            {
                foreach(ITeachObject teachObj in teachHandlerProbe.SelectedObjs)
                {
                    targetParamControl.SelectObject(teachObj);
                }

                if (firstObject is VisionProbe)
                {
                    VisionProbe visionProbe = (VisionProbe)firstObject;

                    LogHelper.Debug(LoggerType.Operation, "ModellerPage - Update Preview Type");

                    previewTypeToolStripButton.DropDownItems.Clear();

                    foreach (string previewName in visionProbe.GetPreviewNames())
                    {
                        ToolStripButton previewButton = new ToolStripButton(previewName);
                        previewButton.Click += PreviewToolStripButton_Click;
                        previewTypeToolStripButton.DropDownItems.Add(previewButton);
                    }

                    previewTypeToolStripButton.Text = previewTypeToolStripButton.DropDownItems[0].Text;

                    UpdatePage(this.stepIndex, this.deviceIndex, visionProbe.LightTypeIndex);
                }
                else
                {
                    UpdateImageFigure();
                }
            }

            UpdateButtonState();
        }

        private void PreviewToolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton previewButton = (ToolStripButton)sender;

            previewIndex = previewTypeToolStripButton.DropDownItems.IndexOf(previewButton);

            previewTypeToolStripButton.Text = previewButton.Text;

            UpdateImage();
        }

        public void teachBox_Moved()
        {
            UpdateImage();
        }

        delegate void ObjectAddedDelegate();

        public void teachBox_Added()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ObjectAddedDelegate(teachBox_Added));
                return;
            }

            Probe probe = teachHandlerProbe.GetSingleSelectedProbe();
            if (probe != null)
            {
                targetParamControl.SelectProbe(probe);
            }
            UpdateButtonState();
        }

        public void UpdateData()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - UpdateData");

            onValueUpdate = true;

            Probe singleSelectedProbe = teachHandlerProbe.GetSingleSelectedProbe();
            bool itemSelected = teachHandlerProbe.IsSelected();

            this.addProbeToolStripButton.Enabled = (singleSelectedProbe != null);

            if (itemSelected == false)
            {
                ClearProbeData();
            }
            else
            {
                if (singleSelectedProbe != null)
                {
                    targetParamControl.SelectProbe(singleSelectedProbe);
                }
            }

            UpdateButtonState();

            onValueUpdate = false;
        }

        public void ClearProbeData()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ClearProbeData");

            teachBox.ClearSelection();
            targetParamControl.ClearProbeData();
        }

        private void ParamControl_ValueChanged(ValueChangedType valueChangedType, bool modified = true)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ParamControl_ValueChanged");

            if (onValueUpdate == false)
            {
                switch (valueChangedType)
                {
                    case ValueChangedType.Position:
                        UpdateImageFigure();
                        break;
                    case ValueChangedType.ImageProcessing:
                        UpdateImage();
                        break;
                    case ValueChangedType.Light:
                        ITeachObject teachObject = teachHandlerProbe.GetSingleSelected();
                        if (teachObject != null)
                        {
                            if (teachObject is VisionProbe)
                            {
                                //lightParamForm.SetLightTypeIndex(((VisionProbe)teachObject).LightTypeIndex);
                                this.lightTypeIndex = ((VisionProbe)teachObject).LightTypeIndex;
                                this.UpdatePage(this.stepIndex, this.deviceIndex, this.lightTypeIndex);
                            }
                        }
                        break;
                }

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, modified);
            }
        }

        public void StopLive()
        {
            onLiveGrab = false;
            multiShotToolStripButton.BackColor = Color.Transparent;

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
            if (imageDevice == null)
                return;

            imageDevice.Stop();

            teachBox.DrawBox.SetImageDevice(null);
        }

        delegate void UpdateImageDelegate(ImageD grabImage = null);

        public void Inspect()
        {
            modellerPageExtender.OnPreStepInspection(curInspectionStep);

            tryInspectionResultView.ClearResult();

            List<Calibration> cameraCalibrationList = SystemManager.Instance().DeviceBox.CameraCalibrationList;

            Calibration curCameraCalibration = null;
            foreach (Calibration cameraCalibration in cameraCalibrationList)
            {
                if (cameraCalibration.CameraIndex == deviceIndex)
                {
                    curCameraCalibration = cameraCalibration;
                    break;
                }
            }

            ImageAcquisition imageAcquisition = SystemManager.Instance().DeviceBox.GetImageAcquisition();
            DeviceImageSet deviceImageSet = imageAcquisition.ImageBuffer.GetDeviceImageSet(deviceIndex);

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
            if (imageDevice != null)
            {
                if (imageDevice.IsDepthScanner())
                {
                    deviceImageSet.UpdateImage3D(sourceImage3d);
                }
                else
                {
                    deviceImageSet.UpdateImage2D(sourceImage2d);
                }
            }

            teachBox.TeachHandler.PixelRes3d = MachineSettings.Instance().PixelRes3D;
            teachBox.Inspect(deviceImageSet, false, curCameraCalibration, SystemManager.Instance().DeviceBox.DigitalIoHandler, modellerInspectionResult);

            UpdateImageFigure();

            InspectionResult lastSelectedResult = null;

            if (lastInspectionResult != null)
            {
                lastSelectedResult = new InspectionResult();
                foreach (ITeachObject teachObject in teachHandlerProbe.SelectedObjs)
                {
                    Probe probe = teachObject as Probe;
                    if (probe != null)
                    {
                        lastSelectedResult.AddProbeResult(lastInspectionResult.GetProbeResult(probe));
                    }
                }
            }

            tryInspectionResultView.SetResult(teachBox.InspectionResultSelected);

            modellerPageExtender.OnPostStepInspection(curInspectionStep);
        }

        private void UpdateButtonState()
        {
            deleteProbeToolStripButton.Enabled = teachHandlerProbe.IsSelected();
        }

        private void Delete()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - deleteProbeButton_Click");
            
            teachHandlerProbe.DeleteObject();
            teachBox.ClearSelection();
            UpdateImageFigure();

            ParamControl_ValueChanged(ValueChangedType.ImageProcessing, true);
        }

        public VisionProbe CreateVisionProbe()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CreateVisionProbe");

            RotatedRect rect = GetDefaultProbeRegion();
            if (rect.IsEmpty)
                return null;

            VisionProbe visionProbe = (VisionProbe)ProbeFactory.Create(ProbeType.Vision);
            visionProbe.AlignedRegion = rect;
            visionProbe.BaseRegion = positionAligner.InvAlignFov(rect);

            return visionProbe;
        }

        private ComputeProbe CreateComputeProbe()
        {
            ComputeProbe computeProbe = (ComputeProbe)ProbeFactory.Create(ProbeType.Compute);

            return computeProbe;
        }

        private DaqProbe CreateDaqProbe()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CreateDaqProbe");

            RotatedRect rect = GetDefaultProbeRegion();
            if (rect.IsEmpty)
                return null;

            DaqProbe daqProbe = (DaqProbe)ProbeFactory.Create(ProbeType.Daq);
            daqProbe.AlignedRegion = rect;
            daqProbe.BaseRegion = positionAligner.InvAlignFov(rect);

            return daqProbe;
        }

        public TensionSerialProbe CreateTensionSerialProbe()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CreateTensionSerialProbe");

            RotatedRect rect = GetDefaultProbeRegion();
            if (rect.IsEmpty)
                return null;

            TensionSerialProbe tensionCheckerProbe = (TensionSerialProbe)ProbeFactory.Create(ProbeType.Tension);
            tensionCheckerProbe.AlignedRegion = rect;
            tensionCheckerProbe.BaseRegion = positionAligner.InvAlignFov(rect);

            return tensionCheckerProbe;
        }

        private RotatedRect GetDefaultProbeRegion()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - GetDefaultProbeRegion");

            Rectangle rectangle = new Rectangle(0, 0, sourceImage2d.Width, sourceImage2d.Height);

            float centerX = sourceImage2d.Width / 2;
            float centerY = sourceImage2d.Height / 2;

            float width = sourceImage2d.Width / 4;
            float height = sourceImage2d.Height / 4;

            float left = centerX - width / 2;
            float top = centerY - height / 2;
            return new RotatedRect(left, top, width, height, 0);
        }

        private bool Is3dAlgorithm(Algorithm algorithm)
        {
            if (algorithm is DepthChecker)
                return true;

            return false;
        }

        public void AddVisionProbe(VisionProbe visionProbe)
        {
            visionProbe.InspAlgorithm.Enabled = true;
            visionProbe.InspAlgorithm.Param.SourceImageType = ImageType.Color;
            if (teachBox.Image.PixelFormat == PixelFormat.Format8bppIndexed)
                visionProbe.InspAlgorithm.Param.SourceImageType = ImageType.Grey;

            AddProbe(visionProbe);
        }

        private void AddComputeProbe(ComputeProbe computeProbe)
        {
            AddProbe(computeProbe);
        }

        public void AddProbe(Probe probe)
        {
            if (curTargetGroup == null)
                return;

            Target selectedTarget = null;
            if (OperationSettings.Instance().UseSingleTargetModel)
            {
                selectedTarget = curTargetGroup.GetTarget(1);                
            }

            if (selectedTarget == null)
            {
                selectedTarget = new Target();
                curTargetGroup.AddTarget(selectedTarget);
            }

            commandManager.Execute(new AddProbeCommand(selectedTarget, probe, positionAligner));

            List<Probe> probeList = new List<Probe>();
            probeList.Add(probe);

            ProbeAdded(probeList);
        }

        private void TargetAdded(List<Target> targetList)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ProbeAdded");

            UpdateImageFigure();

            foreach (Target target in targetList)
            {
                teachHandlerProbe.Select(target);
                teachBox.DrawBox.SelectFigureByTag(target);
            }

            ParamControl_ValueChanged(ValueChangedType.None, true);

            UpdateButtonState();
        }

        private void ProbeAdded(List<Probe> probeList)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ProbeAdded");

            foreach (Probe probe in probeList)
            {
                teachHandlerProbe.Select(probe);

                if (probeList.Count == 1)
                    targetParamControl.ShowAlgorithmParamControl(probe);
            }

            ParamControl_ValueChanged(ValueChangedType.None, true);

            UpdateButtonState();

            UpdateImageFigure();
        }

        private Bitmap GetClipImage(RotatedRect clipRegion)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - GetClipImage");

            return ImageHelper.ClipImage((Bitmap)teachBox.Image, clipRegion);
        }

        private void Copy()
        {
            List<ICloneable> objList = new List<ICloneable>();
            foreach (ITeachObject teachObject in teachHandlerProbe.SelectedObjs)
            {
                objList.Add((ICloneable)teachObject.Clone());
            }

            CopyBuffer.SetData(objList);

            UpdateButtonState();
        }

        private void Paste()
        {
            if (OperationSettings.Instance().UseSingleTargetModel)
                PasteProbe();
            else
                PasteTarget();
        }

        private void PasteProbe()
        {
            if (curTargetGroup == null)
                return;

            Target selectedTarget = curTargetGroup.GetTarget(1);

            List<ICloneable> objList = CopyBuffer.GetData();

            List<Probe> newProbeList = new List<Probe>();

            teachHandlerProbe.ClearSelection();

            foreach (Object obj in objList)
            {
                try
                {
                    if (obj is Probe)
                    {
                        Probe srcProbe = (Probe)obj;
                        RotatedRect rectangle = srcProbe.BaseRegion;

                        Probe probe = (Probe)srcProbe.Clone();

                        selectedTarget.AddProbe(probe);

                        newProbeList.Add(probe);

                        rectangle.Offset(10, 10);
                        srcProbe.AlignedRegion = positionAligner.AlignFov(rectangle);
                        srcProbe.BaseRegion = rectangle;
                    }
                }
                catch (InvalidCastException)
                {

                }
            }

            ProbeAdded(newProbeList);
        }

        private void PasteTarget()
        {
            if (curTargetGroup == null)
                return;

            List<Target> targetList = CopyBuffer.GetTargetList();

            List<Target> newTargetList = new List<Target>();

            teachHandlerProbe.ClearSelection();

            foreach (Target srcTarget in targetList)
            {
                try
                {
                    RotatedRect baseRegion = srcTarget.BaseRegion;

                    Target newTarget = (Target)srcTarget.Clone();

                    curTargetGroup.AddTarget(newTarget);

                    newTargetList.Add(newTarget);

                    baseRegion.Offset(10, 10);

                    newTarget.UpdateRegion(baseRegion, positionAligner);
                }
                catch (InvalidCastException)
                {

                }
            }

            TargetAdded(newTargetList);
        }

        public void UpdateImage(ImageD grabImage = null)
        {
            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Operation, "Invoke UpdateImage");

                Invoke(new UpdateImageDelegate(UpdateImage), grabImage);
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "ModellerPage - UpdateImage 1");

            if (grabImage != null)
            {
                sourceImage2d = (Image2D)grabImage;
            }
            else
            {
                if (sourceImage2d == null)
                    return;

                if (sourceImage2d.Width == 0 || sourceImage2d.Height == 0)
                    return;
            }

            grabProcessToolStripButton.BackColor = Color.Transparent;

            LogHelper.Debug(LoggerType.Operation, "ModellerPage - UpdateImage 2");

            ImageD cloneImage = sourceImage2d.Clone();

            List<Probe> probeList = teachHandlerProbe.GetSelectedProbe();
            foreach (Probe probe in probeList)
            {
                if (probe != null && (cloneImage is Image2D) && onPreviewMode)
                {
                        cloneImage = probe.PreviewFilterResult(cloneImage, previewIndex, false);
                        //teachBox.UpdateImage(, false);
                }
            }

            teachBox.UpdateImage(cloneImage);

            targetParamControl.UpdateTargetGroupImage(sourceImage2d, lightTypeIndex);
        }

        private void ModellerPage_VisibleChanged(object sender, EventArgs e)
        {
            if (SystemManager.Instance().CurrentModel != null)
            {
                InitSelectStepButton();

                imagePath = SystemManager.Instance().GetImagePath();

                LoadModelImage();

                modellerInspectionResult.Clear();

                UpdatePage(0, 0, 0);
            }
        }

        private void LoadModelImage()
        {
            string searchFormat = string.Format(@"{0}\*.*", imagePath);
            string[] files = Directory.GetFiles(imagePath, "*.*");
            foreach (string file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                if (name.Length != 18)
                {
                    continue;
                }

                if (name.Substring(0, 7) != "Image_C")
                {
                    continue;
                }

                int number;
                bool ok;

                // device No
                string devNo = name.Substring(7, 2);
                ok = int.TryParse(devNo, out number);
                if (ok)
                {
                    deviceIndex = number;
                }

                string stepNo = name.Substring(11, 3);
                ok = int.TryParse(stepNo, out number);
                if (ok)
                {
                    stepIndex = number;
                }

                string lightTypeNo = name.Substring(16, 2);
                ok = int.TryParse(lightTypeNo, out number);
                if (ok)
                {
                    lightTypeIndex = number;
                }

                if (LoadImage() == true)
                    break;
            }
            // Image_C00_S000_L00
        }

        public void StopGrab()
        {
            modellerPageExtender.StopGrab();
        }

        public void InitSelectStepButton()
        {
            LogHelper.Debug(LoggerType.Operation, "InitSelectStepButton");
            comboStep.Items.Clear();

            int numInspectionStep = SystemManager.Instance().CurrentModel.NumInspectionStep;
            foreach (InspectionStep inspectionStep in SystemManager.Instance().CurrentModel.InspectionStepList)
            {
                string stepName = inspectionStep.Name;
                if (String.IsNullOrEmpty(inspectionStep.Name) == true)
                    stepName = inspectionStep.StepNo.ToString();

                comboStep.Items.Add(stepName);
            }
            LogHelper.Debug(LoggerType.Operation, "InitStepList StepList Add");

            //if(robotStage != null)
            //    AlignPosition();

            stepIndex = 0;
            if (comboStep.Items.Count > 0)
                comboStep.SelectedIndex = stepIndex;

            if (OperationSettings.Instance().UseFixedInspectionStep)
            {
                if (numInspectionStep == 1)
                {
                    comboStep.Visible = false;
                    moveNextStepButton.Visible = false;
                    movePrevStepButton.Visible = false;
                }

                addStepButton.Visible = false;
                deleteStepButton.Visible = false;
            }

            LogHelper.Debug(LoggerType.Operation, "InitStepList Finished");
        }

        private void comboBoxPreviewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - comboBoxPreviewType_SelectedIndexChanged");
            UpdateImage();
        }

        public void ProcessKeyDown(KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.KeyCode == Keys.C)
                {
                    Copy();
                }
                else if (e.KeyCode == Keys.V)
                {
                    Paste();
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                //Delete();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                Inspect();
            }
            else if (e.KeyCode == Keys.Z)
            {
                teachBox.DrawBox.HideLine = !teachBox.DrawBox.HideLine;
                teachBox.Invalidate(true);
            }
            else if (e.KeyCode == Keys.L)
            {
                //ToggleViewLabel();
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        //imageBox.Offset(-1, 0);
                        break;
                    case Keys.Right:
                        //imageBox.Offset(1, 0);
                        break;
                    case Keys.Up:
                        //imageBox.Offset(0, -1);
                        break;
                    case Keys.Down:
                        //imageBox.Offset(0, 1);
                        break;
                }
            }
        }

        private void buttonBatchInspect_Click(object sender, EventArgs e)
        {

        }

        private void SaveImage()
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);

            if (imageDevice.IsDepthScanner())
                Save3dImage(); // 수정 작업후 주석 삭제 해 주세요
            else
                Save2dImage(); // 수정 작업후 주석 삭제 해 주세요
        }

        private void Save3dImage()
        {
            // 나중에 추가 바랍니다
        }

        private void Save2dImage()
        {
            // 나중에 추가 바랍니다
        }

        private bool LoadImage()
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
            if (imageDevice == null)
                return false;

            return Load2dImage();
        }

        private bool Load2dImage()
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);

            ImageAcquisition imageAcquisition = SystemManager.Instance().DeviceBox.GetImageAcquisition();
            ImageBuffer2dItem imageBuffer2dItem = imageAcquisition.ImageBuffer.GetImageBuffer2dItem(deviceIndex, lightTypeIndex);
            if (imageBuffer2dItem != null)
                sourceImage2d = (Image2D)imageBuffer2dItem.Image;

            string searchImageFileName = String.Format("Image_C{0:00}_S{1:000}_L{2:00}.*", deviceIndex, stepIndex, lightTypeIndex);

            string[] imageFiles = null;
            if (String.IsNullOrEmpty(imagePath) == false && Directory.Exists(imagePath) == true )
            {
                //imageFiles = Directory.GetFiles(imagePath, searchImageFileName);
                imageFiles = Directory.GetFiles(imagePath, searchImageFileName);
            }

            //if (Directory.Exists(imagePath) == true)
            //{
            //    if(imagePath != null)
            //        imageFiles = Directory.GetFiles(imagePath, searchImageFileName);
            //}

            if (imageFiles != null && imageFiles.Count() == 0)
            {
                string modelImgaePath = string.Format(@"{0}\Image", SystemManager.Instance().CurrentModel.ModelPath);
                if(Directory.Exists(modelImgaePath))
                    imageFiles = Directory.GetFiles(modelImgaePath, searchImageFileName);
            }

            bool result = false;

            if (imageFiles != null && imageFiles.Count() > 0)
            {
                string imageFilePath = Path.Combine(Path.GetFullPath(imagePath), Path.GetFileName(imageFiles[0]));
                Image2D fileImage = new Image2D(imageFilePath);
                if (imageDevice.IsCompatibleImage(fileImage) == true)
                {
                    sourceImage2d = fileImage;
                    imageAcquisition.ImageBuffer.Set2dImage(deviceIndex, lightTypeIndex, sourceImage2d);
                    targetParamControl.UpdateTargetGroupImage(sourceImage2d, lightTypeIndex);
                    result = true;
                                     
                }
                else
                    sourceImage2d.Clear();
            }
            else
            {
                sourceImage2d.Clear();
            }
#if DEBUG
            string saveDebugImagePath = string.Format("{0}\\{1}.bmp", PathSettings.Instance().Temp, "Load2dImage");
            sourceImage2d.SaveImage(saveDebugImagePath, ImageFormat.Bmp);
#endif 
            return result;
        }

        private void lightParamPanel_LightTypeChanged()
        {
            this.lightTypeIndex = lightParamForm.LightTypeIndex;

            //GrabImage();
            //UpdateImage();
            UpdatePage(this.stepIndex, this.deviceIndex, this.lightTypeIndex);
            UpdateImageFigure();
        }

        private void lightParamPanel_LightValueChanged(bool imageUpdateRequired)
        {
            if (imageUpdateRequired == true)
                GrabImage();

            ParamControl_ValueChanged(ValueChangedType.None);
        }

        public void UpdatePage(int stepIndex, int cameraIndex, int lightTypeIndex)
        {
            Model curModel = SystemManager.Instance().CurrentModel;

            if (curModel == null)
                return;

            curInspectionStep = curModel.GetInspectionStep(stepIndex);
            if (curInspectionStep == null)
                return;

            curTargetGroup = curInspectionStep.GetTargetGroup(deviceIndex);
            if (curTargetGroup == null)
                return;

            lightParamForm.SetLightValues(curModel, curInspectionStep, curTargetGroup);

            this.lightTypeIndex = lightTypeIndex;
            this.stepIndex = stepIndex;
            this.deviceIndex = cameraIndex;
            selectCameraButton.Text = String.Format(StringManager.GetString(this.GetType().FullName, "Camera") + " {0}", cameraIndex + 1);

            if (curTargetGroup.GetLightParamSet().LightParamList.Count > 0)
            {
                LightParam lightParam = curTargetGroup.GetLightParamSet().LightParamList[lightTypeIndex];
                UpdateLightTypeCombo();
                selectLightButton.Text = String.Format("{0}", lightParam.Name);
            }
            
            //LightParam lightParam = MachineSettings.Instance().LightParamSet.LightParamList[lightTypeIndex];
            //selectLightButton.Text = String.Format("{0}", lightParam.Name);

            int numLight = MachineSettings.Instance().NumLight;
            int numLightType = MachineSettings.Instance().NumLightType;

            //if (curInspectionStep.AlignedPosition != null)
            //{
            //    systemManager.Machine.RobotStage.Move(curInspectionStep.AlignedPosition);
            //}

            if (curTargetGroup.TransformDataList != null)
                calibration3dButton.Checked = true;

            if (LoadImage() == false)
            {
                if (GrabImage() == false)
                    return;
            }

            teachBox.TargetGroup = curTargetGroup;
            teachHandlerProbe.Boundary = new Rectangle(0, 0, sourceImage2d.Width, sourceImage2d.Height);

            if (OperationSettings.Instance().UseSingleTargetModel)
            {
                Target selectedTarget = curTargetGroup.GetTarget(0);
                if (selectedTarget == null)
                {
                    selectedTarget = new Target();
                    selectedTarget.Image = (Image2D)sourceImage2d.Clone();
                    curTargetGroup.AddTarget(selectedTarget);
                }
                else
                {
                    RotatedRect targetRegion = new RotatedRect(DrawingHelper.ToRectF(teachHandlerProbe.Boundary), 0);

                    selectedTarget.Image = (Image2D)sourceImage2d.Clone();
                    selectedTarget.UpdateRegion(targetRegion, positionAligner);
                }

                //teachBox.ClearSelection();
                //teachHandlerProbe.ClearSelection();
            }
            else
            {
                //teachBox.ClearSelection();
                //teachHandlerProbe.ClearSelection();
            }

            //UpdateLightItems(lightTypeIndex);
            UpdateImage();
            UpdateImageFigure();
        }

        private bool GrabImage(bool grabAll = false)
        {
            LightParamSet lightParamSet = lightParamForm.LightParamSet;
            LightParam lightParam = lightParamSet.LightParamList[lightTypeIndex];

            if (Visible == false)
                return false;

            if (lockGrab == true)
                return false;

            if (curInspectionStep == null)
                return false;

            ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(deviceIndex);
            if (imageDevice == null)
                return false;

            string imagePath = Path.Combine(SystemManager.Instance().CurrentModel.ModelPath, "Image");
            if (Directory.Exists(imagePath) == false)
            {
                Directory.CreateDirectory(imagePath);
            }

            if (grabAll)
            {
                modellerPageExtender.GrabAll(stepIndex, lightParamSet);

                ImageAcquisition imageAcquisition = SystemManager.Instance().DeviceBox.GetImageAcquisition();
                sourceImage2d = imageAcquisition.ImageBuffer.GetImageBuffer2dItem(this.deviceIndex, lightTypeIndex).Image;

                //imageAcquisition.Acquire(stepIndex, lightTypeIndex, lightParam);
                //sourceImage2d = imageAcquisition.ImageBuffer.GetImageBuffer2dItem(this.deviceIndex, lightTypeIndex).Image;

                //sourceImage2d = modellerPageExtender.GrabOnce(stepIndex, deviceIndex, -1, lightParamSet.LightParamList[lightTypeIndex], lightParamSet);
            }
            else
            {
                sourceImage2d = modellerPageExtender.GrabOnce(stepIndex, deviceIndex, lightTypeIndex, lightParam, lightParamSet);
            }

            if (sourceImage2d == null)
            {
                string message = "Grab Error";
                MessageBox.Show(message);
                LogHelper.Error(LoggerType.Error, message);
            }

            UpdateImage();

            return true;
        }

        private void pasteProbeButton_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            teachBox.ZoomIn();
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            teachBox.ZoomOut();
        }

        // H/W Trigger 등에 의해 
        private void grabProcessToolStripButton_Click(object sender, EventArgs e)
        {
            modellerPageExtender.GrabProcess(stepIndex, deviceIndex, lightTypeIndex, lightParamForm.LightParamSet);
            grabProcessToolStripButton.BackColor = Color.LightGreen;

            UpdateImage();
        }

        private void showLightPanelToolStripButton_Click(object sender, EventArgs e)
        {
            if (MachineSettings.Instance().NumLight == 0)
                return;

            lightParamForm.ToggleVisible();
        }

        private void loadImageSetToolStripButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = imagePath;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string[] extensions = { ".jpg", ".png", ".bmp" };
                string[] fileNames = Directory.GetFiles(dialog.SelectedPath, "Image_*.*").Where(f => extensions.Contains(new FileInfo(f).Extension.ToLower())).ToArray();
                if (fileNames.Count() > 0)
                {
                    //offlineImagePathList = new string[1] { dialog.SelectedPath };
                    offlineImagePathList = new string[1] { string.Format("{0}\\{1}", dialog.SelectedPath, fileNames[0]) };
                    //offlineImagePathList = new string[1] { string.Format("{0}\\{1}", dialog.SelectedPath, fileNames[0]) };
                }
                else
                {
                    offlineImagePathList = Directory.GetDirectories(dialog.SelectedPath, "*.*");
                }

                offlineImageIndex = 0;

                if (offlineImagePathList.Count() == 0)
                    return;

                imagePath = offlineImagePathList[0];

                UpdatePage(0, 0, 0);
            }
        }

        private void selectPrevImageSetToolStripButton_Click(object sender, EventArgs e)
        {
            if (offlineImageIndex > 0 && offlineImagePathList != null)
            {
                offlineImageIndex--;

                imagePath = offlineImagePathList[offlineImageIndex];

                UpdatePage(0, 0, 0);
            }
        }

        private void selectNextImageSetToolStripButton_Click(object sender, EventArgs e)
        {
            if (offlineImagePathList == null)
                return;

            if (offlineImageIndex < offlineImagePathList.Count() - 1)
            {
                offlineImageIndex++;

                imagePath = offlineImagePathList[offlineImageIndex];

                UpdatePage(0, 0, 0);
            }
        }

        private void singleShotToolStripButton_Click(object sender, EventArgs e)
        {
            GrabImage();
        }


        private void multiShotToolStripButton_Click(object sender, EventArgs e)
        {
            if (onLiveGrab == false)
            {
                if (curInspectionStep == null)
                    return;

                onLiveGrab = true;
                multiShotToolStripButton.BackColor = Color.LightGreen;

                multiShotThread = new Thread(multiShotProc);
                multiShotThread.Start();
            }
            else
            {
                onLiveGrab = false;
                multiShotToolStripButton.BackColor = Color.Transparent;
                //multiShotThread.Abort();
                //multiShotThread = null;
            }
        }

        private void multiShotProc()
        {                   
            while (onLiveGrab)
            {
                Trace.Write("multiShotProc - GrabImage");
                GrabImage();
            }
        }


        //private void multiShotToolStripButton_Click(object sender, EventArgs e)
        //{
        //    if (onLiveGrab == false)
        //    {
        //        onLiveGrab = true;

        //        if (curInspectionStep == null)
        //            return;

        //        curInspectionStep.UpdateImageBuffer(imageAcquisition.ImageBuffer);

        //        ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
        //        if (imageDevice == null)
        //            return;

        //        teachBox.DrawBox.SetImageDevice(imageDevice);

        //        ImageBuffer2dItem imageBufferItem = imageAcquisition.ImageBuffer.GetImageBuffer2dItem((int)imageDevice.Index, 0);
        //        imageDevice.SetGrabDestImage(imageBufferItem.Image);

        //        imageDevice.GrabMulti();

        //        multiShotToolStripButton.BackColor = Color.LightGreen;
        //    }
        //    else
        //    {
        //        StopLive();
        //    }
        //}

        private void zoomInToolStripButton_Click(object sender, EventArgs e)
        {
            teachBox.ZoomIn();
        }

        private void zoomOutToolStripButton_Click(object sender, EventArgs e)
        {
            teachBox.ZoomOut();
        }

        private void zoomFitToolStripButton_Click(object sender, EventArgs e)
        {
            teachBox.ZoomFit();
        }

        private void syncParamToolStripButton_Click(object sender, EventArgs e)
        {
            ITeachObject teachObject = teachHandlerProbe.GetSingleSelected();
            if (teachObject != null)
            {
                if (teachObject is Probe)
                {
                    Probe probe = (Probe)teachObject;

                    IProbeFilter probeFilter;
                    if (modellerPageExtender.GetProbeFilter(probe, out probeFilter) == true)
                    {
                        Model curModel = SystemManager.Instance().CurrentModel;

                        InspectionStep curinspectionStep = curModel.GetInspectionStep(stepIndex);

                        foreach (InspectionStep inspectionStep in curModel.InspectionStepList)
                        {
                            if (inspectionStep.StepType == curinspectionStep.StepType)
                            {
                                inspectionStep.SyncParam(probe, probeFilter);
                            }
                        }
                    }
                }
            }
        }

        private void syncAllToolStripButton_Click(object sender, EventArgs e)
        {
            if (OperationSettings.Instance().UseFiducialStep == true)
            {
                if (stepIndex < 2)
                {
                    MessageBox.Show("Sync Operation is not working in fiducial Step.");
                    return;

                }
            }

            InspectionStep curinspectionStep = SystemManager.Instance().CurrentModel.GetInspectionStep(stepIndex);

            foreach (InspectionStep inspectionStep in SystemManager.Instance().CurrentModel.InspectionStepList)
            {
                if (OperationSettings.Instance().UseFiducialStep == true)
                {
                    if (inspectionStep.StepNo < 2)
                        continue;
                }

                if (inspectionStep.StepNo != stepIndex)
                {
                    inspectionStep.Copy(curinspectionStep);
                }
            }
        }

        private void copyProbeToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "Modeller - Copy target");
            Copy();
        }

        private void pasteProbeToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "Modeller - Paste target");
            Paste();
        }

        private void deleteProbeToolStripButton_Click(object sender, EventArgs e)
        {
            if (teachHandlerProbe.IsSelected() == false)
                return;

            if (MessageForm.Show(this.ParentForm, StringManager.GetString(this.GetType().FullName, "Do you want to delete selected probe?"), MessageFormType.YesNo) == DialogResult.No)
                return;

            Delete();
        }

        private void inspectionButton_Click(object sender, EventArgs e)
        {
            Inspect();
        }

        private void setTargetCalibrationToolStripButton_Click(object sender, EventArgs e)
        {
            Probe selectedProbe = teachHandlerProbe.GetSingleSelectedProbe();
            if (selectedProbe != null)
            {
                if (selectedProbe is VisionProbe)
                {
                    VisionProbe selectedVisionProbe = (VisionProbe)selectedProbe;
                    if (selectedVisionProbe.InspAlgorithm is CalibrationChecker)
                    {
                        if (selectedVisionProbe.ActAsCalibrationProbe == true)
                        {
                            selectedVisionProbe.ActAsCalibrationProbe = false;
                        }
                        else
                        {
                            selectedVisionProbe.ActAsCalibrationProbe = true;
                        }
                    }
                }
            }
        }

        private void setFiducialToolStripButton_Click(object sender, EventArgs e)
        {
            Probe selectedProbe = teachHandlerProbe.GetSingleSelectedProbe();
            if (selectedProbe != null)
            {
                if (selectedProbe is VisionProbe)
                {
                    VisionProbe selectedVisionProbe = (VisionProbe)selectedProbe;
                    if (selectedVisionProbe.InspAlgorithm is Searchable)
                    {
                        if (selectedVisionProbe.ActAsFiducialProbe == true)
                            selectedProbe.Target.DeselectFiducialProbe(selectedVisionProbe.Id);
                        else
                            selectedProbe.Target.SelectFiducialProbe(selectedVisionProbe.Id);
                    }
                }
            }
        }

        private void UpdateFovNavigator()
        {
            if (fovNavigator == null)
                return;

            fovNavigator.ClearFovList();

            foreach (InspectionStep inspectionStep in SystemManager.Instance().CurrentModel)
            {
                if (inspectionStep.NumTargets == 0)
                    continue;
                if (inspectionStep.AlignedPosition == null)
                    continue;
                Figure figure = fovNavigator.AddFovFigure(inspectionStep.AlignedPosition.ToPointF());

                figure.Tag = inspectionStep.StepNo;
                
            }
            fovNavigator.SelectFov(comboStep.SelectedIndex);

            fovNavigator.Invalidate();
        }

        private void addStepButton_Click(object sender, EventArgs e)
        {
            InspectionStep inspectionStep = SystemManager.Instance().CurrentModel.CreateInspectionStep();
            inspectionStep.AlignedPosition?.SetPosition(currentPosition.Position); 

            UpdateFovNavigator();
            InitSelectStepButton();

            UpdatePage(inspectionStep.StepNo, deviceIndex, lightTypeIndex);

            comboStep.SelectedIndex = inspectionStep.StepNo;
            if (fovNavigator != null)
                fovNavigator.SelectFigureByTag(inspectionStep);

            modified = true;
       }

        private void deleteStepButton_Click(object sender, EventArgs e)
        {
            if (stepIndex > 0)
            {
                if (MessageForm.Show(this.ParentForm, StringManager.GetString(this.GetType().FullName, "Do you want to delete selected step?"), MessageFormType.YesNo) == DialogResult.No)
                    return;

                InspectionStep inspectionStep = SystemManager.Instance().CurrentModel.GetInspectionStep(stepIndex);
                SystemManager.Instance().CurrentModel.RemoveInspectionStep(inspectionStep);

                int newStepIndex = stepIndex - 1;

                InitSelectStepButton();

                UpdatePage(newStepIndex, deviceIndex, lightTypeIndex);

                comboStep.SelectedIndex = newStepIndex;
            }
        }

        private void groupProbeToolStripButton_Click(object sender, EventArgs e)
        {
            if (teachHandlerProbe.IsSelected() == false)
                return;

            if (curTargetGroup == null)
                return;

            Target newTarget = new Target();

            List<Target> targetList = teachHandlerProbe.GetTargetList();
            foreach (Target target in targetList)
            {
                newTarget.AddProbe(target.ProbeList);
                curTargetGroup.RemoveTarget(target);
            }

            newTarget.UpdateRegion(positionAligner);

            curTargetGroup.AddTarget(newTarget);

            UpdateImageFigure();

            teachHandlerProbe.ClearSelection();
            teachBox.DrawBox.ResetSelection();

            teachHandlerProbe.Select(newTarget);
            teachBox.DrawBox.SelectFigureByTag(newTarget);
        }

        private void ungroupProbeToolStripButton_Click(object sender, EventArgs e)
        {
            if (teachHandlerProbe.IsSingleSelected() == false)
                return;

            if (curTargetGroup == null)
                return;

            List<Target> targetList = teachHandlerProbe.GetTargetList();
            foreach (Target target in targetList)
            {
                foreach(Probe probe in target.ProbeList)
                {
                    Target newTarget = new Target();
                    newTarget.AddProbe(probe);

                    newTarget.UpdateRegion(positionAligner);

                    curTargetGroup.AddTarget(newTarget);
                }

                curTargetGroup.RemoveTarget(target);
            }
        }

        private void grab3dToolStripButton_Click(object sender, EventArgs e)
        {
            // depthScannerHandler.Scan
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - SaveToolStripButton_Click");
            if (SystemManager.Instance().CurrentModel != null)
                SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButtonRobotSetting_Click(object sender, EventArgs e)
        {
            MotionSpeedForm motionSpeedForm = new MotionSpeedForm();
            motionSpeedForm.Intialize(SystemManager.Instance().DeviceBox.AxisConfiguration);
            motionSpeedForm.ShowDialog(this);
        }

        private void toolStripButtonJoystick_Click(object sender, EventArgs e)
        {
            joystick.ToggleView(this, curTargetGroup.GetLightParamSet());
        }

        private void toolStripButtonOrigin_Click(object sender, EventArgs e)
        {
            AxisHandler robotStage = SystemManager.Instance().DeviceController.RobotStage;
            if (robotStage != null)
            {
                cancellationTokenSource = new CancellationTokenSource();

                SimpleProgressForm loadingForm = new SimpleProgressForm("Move to origin");
                loadingForm.Show(new Action(() =>
                {
                    robotStage.HomeMove(cancellationTokenSource);
                }), cancellationTokenSource);
            }
        }

        public delegate void DisplayAlignInfoDelegate(PositionAligner positionAligner);
        public void DisplayAlignInfo(PositionAligner positionAligner)
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayAlignInfoDelegate(DisplayAlignInfo), positionAligner);
                return;
            }

            textBoxDesiredDistance.Text = positionAligner.DesiredFiducialDistance.ToString();
            textBoxFidDistance.Text = positionAligner.FiducialDistance.ToString();
            fidOffset.Text = positionAligner.FiducialDistanceOffset.ToString();
            fidAngle.Text = positionAligner.Angle.ToString();

            StepChanged();

            //GrabImage(true);
            //UpdateImage();
        }

        private void dontMoveToolStripButton_Click(object sender, EventArgs e)
        {
            teachHandlerProbe.Movable = !teachHandlerProbe.Movable;
            if (teachHandlerProbe.Movable == false)
            {
                dontMoveToolStripButton.BackColor = Color.LightGreen;
                teachBox.DrawBox.MoveLocked = true;
            }
            else
            {
                dontMoveToolStripButton.BackColor = Color.WhiteSmoke;
                teachBox.DrawBox.MoveLocked = false;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
        }

        private void modelPropertyButton_Click(object sender, EventArgs e)
        {
            ModelForm editModelForm = new ModelForm();
            editModelForm.ModelFormType = ModelFormType.Edit;
            editModelForm.ModelManager = SystemManager.Instance().ModelManager;
            editModelForm.ModelDescription = SystemManager.Instance().CurrentModel.ModelDescription;
            if (editModelForm.ShowDialog(this) == DialogResult.OK)
            {
                SystemManager.Instance().ModelManager.EditModel(editModelForm.ModelDescription);
            }
        }

        private void exportFormatButton_Click(object sender, EventArgs e)
        {
            SystemManager systemManager = SystemManager.Instance();
            OutputFormatForm form = new OutputFormatForm();
            form.Model = systemManager.CurrentModel;
            if (form.ShowDialog() == DialogResult.OK)
            {
                systemManager.ModelManager.SaveModelDescription(systemManager.CurrentModel.ModelDescription);
            }
        }

        private void editStepButton_Click(object sender, EventArgs e)
        {
            InspectionStep curInspectionStep = SystemManager.Instance().CurrentModel.GetInspectionStep(stepIndex);
            if (curInspectionStep == null)
                return;

            InspectionStepForm form = new InspectionStepForm();
            form.Initialize(curInspectionStep);
            if (form.ShowDialog() == DialogResult.OK)
            {
                string stepName = "";
                if (String.IsNullOrEmpty(curInspectionStep.Name))
                    stepName = curInspectionStep.StepNo.ToString();
                else
                    stepName = curInspectionStep.Name;

                comboStep.Items[stepIndex] = stepName;
                comboStep.Text = stepName;
            }
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().DeviceController.RobotStage.StopMove();
        }

        private void toolStripModel_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void movePrevStepButton_Click(object sender, EventArgs e)
        {
            if (stepIndex > 0)
            {
                movePrevStepButton.Enabled = false;
                comboStep.SelectedIndex = stepIndex - 1;
                movePrevStepButton.Enabled = true;
            }
        }

        private void moveNextStepButton_Click(object sender, EventArgs e)
        {
            if (stepIndex < (comboStep.Items.Count - 1))
            {
                moveNextStepButton.Enabled = false;
                comboStep.SelectedIndex = stepIndex + 1;
                moveNextStepButton.Enabled = true;
            }
        }

        private void comboStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            //UpdateLightItems();

            stepIndex = comboStep.SelectedIndex;

            StepChanged();
        }

        private void StepChanged()
        {
            teachBox.ClearSelection();

            // Robot move...
            InspectionStep inspectionStep = SystemManager.Instance().CurrentModel.GetInspectionStep(stepIndex);
            if (inspectionStep != null && inspectionStep.AlignedPosition != null 
                && SystemManager.Instance().DeviceController.RobotStage != null)
            {
                SystemManager.Instance().DeviceController.RobotStage.Move(inspectionStep.AlignedPosition);
            }

            UpdatePage(stepIndex, deviceIndex, inspectionStep.GetLightTypeIndex(deviceIndex));

            if (fovNavigator != null)
                UpdateFovNavigator();
        }

        private void editSchemaButton_Click(object sender, EventArgs e)
        {
            SchemaEditor modelSchemaEditor = new SchemaEditor();
            modelSchemaEditor.Initialize(SystemManager.Instance().CurrentModel, SystemManager.Instance().DeviceBox.GetImageAcquisition());
            modelSchemaEditor.ShowDialog(this);
        }

        private void toolStripButtonFineMove_Click(object sender, EventArgs e)
        {
            toolStripButtonFineMove.Checked = !toolStripButtonFineMove.Checked;
        }

        private void teachBox_PositionShifted(SizeF offsetF)
        {
            if (toolStripButtonFineMove.Checked)
            {
                SystemManager.Instance().DeviceController.RobotStage.RelativeMove(new AxisPosition(offsetF.Width, offsetF.Height));
                teachBox.Invalidate();
            }

            //UpdateFovRect();
        }

        private void previewToolStripButton_Click_1(object sender, EventArgs e)
        {
            Preview();
        }

        private void Preview()
        {
            onPreviewMode = !onPreviewMode;

            if (onPreviewMode == true)
            {
                previewToolStripButton.Checked = true;
                previewToolStripButton.BackColor = Color.Green;
            }
            else
            {
                previewToolStripButton.Checked = false;
                previewToolStripButton.BackColor = Color.Transparent;
            }

            UpdateImage();
        }

        private void undoToolStripButton_Click(object sender, EventArgs e)
        {
            commandManager.Undo();
        }

        private void RedoToolStripButton_Click(object sender, EventArgs e)
        {
            commandManager.Redo();
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            Preview();
        }

        private void ChangeControl()
        {
            if (OperationSettings.Instance().UseSingleTargetModel)
            {
                addFiducialToolStripButton.Visible = false;
                deleteFiducialToolStripButton.Visible = false;
                toggleFiducialToolStripButton.Visible = true;
            }
            else
            {
                addFiducialToolStripButton.Visible = true;
                deleteFiducialToolStripButton.Visible = true;
                toggleFiducialToolStripButton.Visible = false;
            }

            teachHandlerProbe.Movable = false;
            dontMoveToolStripButton.BackColor = Color.LightGreen;
            teachBox.DrawBox.MoveLocked = true;

//            if (OperationSettings.Instance().SystemType == SystemType.MaskInspector)
//            {
//                addStepButton.Visible = false;
//                undoToolStripButton.Visible = false;
//                RedoToolStripButton.Visible = false;
//#if !DEBUG
//                groupProbeToolStripButton.Visible = false;
//                ungroupProbeToolStripButton.Visible = false;
//#endif
//                singleShotToolStripButton.Visible = false;
//                multiShotToolStripButton.Visible = false;
//                setFiducialToolStripButton.Visible = false;
//                setTargetCalibrationToolStripButton.Visible = false;
//                toolStripSeparatorGroup.Visible = false;
//            }
//            else if(OperationSettings.Instance().SystemType == SystemType.PowerPack)
//            {
//                tabControlMain.TabPages.Remove(tabPageModel);

//                toolStripLabelStep.Visible = false;
//                comboStep.Visible = false;
//                movePrevStepButton.Visible = false;
//                moveNextStepButton.Visible = false;
//                addStepButton.Visible = false;
//                deleteStepButton.Visible = false;

//                toolStripButtonAlign.Visible = false;
//                addProbeToolStripButton.Visible = false;
//                groupProbeToolStripButton.Visible = false;
//                ungroupProbeToolStripButton.Visible = false;
//                setFiducialToolStripButton.Visible = false;
//                setTargetCalibrationToolStripButton.Visible = false;
//                syncParamToolStripButton.Visible = false;
//                syncAllToolStripButton.Visible = false;
//                dontMoveToolStripButton.Visible = false;
//                undoToolStripButton.Visible = false;
//                RedoToolStripButton.Visible = false;

//                toolStripSeparatorCalibration.Visible = false;
//                toolStripSeparatorFiducial.Visible = false;
//                toolStripSeparator3.Visible = false;
//                toolStripSeparatorGroup.Visible = false;

//                toolStripProbe.Items.Insert(0, new ToolStripSeparator());

//                if (AlgorithmBuilder.IsAlgorithmEnabled(BinaryCounter.TypeName))
//                {
//                    ToolStripButton binaryCounterToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Soldering"));

//                    binaryCounterToolStripButton.Image = global::UniEye.Base.Properties.Resources.add_32;
//                    binaryCounterToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
//                    binaryCounterToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
//                    binaryCounterToolStripButton.Name = "";
//                    binaryCounterToolStripButton.Size = new System.Drawing.Size(69, 57);
//                    binaryCounterToolStripButton.Text = StringManager.GetString(this.GetType().FullName, "Soldering");
//                    binaryCounterToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
//                    binaryCounterToolStripButton.Click += BinaryCounterToolStripButton_Click;
                    
//                    toolStripProbe.Items.Insert(0, binaryCounterToolStripButton);
//                }

//                if (AlgorithmBuilder.IsAlgorithmEnabled(PatternMatching.TypeName))
//                {
//                    ToolStripButton patternMatchingToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Pattern Matching"));

//                    patternMatchingToolStripButton.Image = global::UniEye.Base.Properties.Resources.gun_sight_32;
//                    patternMatchingToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
//                    patternMatchingToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
//                    patternMatchingToolStripButton.Name = "";
//                    patternMatchingToolStripButton.Size = new System.Drawing.Size(69, 57);
//                    patternMatchingToolStripButton.Text = StringManager.GetString(this.GetType().FullName, "Pattern Matching");
//                    patternMatchingToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
//                    patternMatchingToolStripButton.Click += PatternMatchinToolStripButton_Click;
                    
//                    toolStripProbe.Items.Insert(0, patternMatchingToolStripButton);
//                }
//            }
        }

        void UpdateFovNavigator(AxisPosition robotPosition)
        {
            if (fovNavigator == null)
                return;    
        }

        private void toolStripButtonAlign_Click(object sender, EventArgs e)
        {
            panelAlign.Visible = true;
            if (OperationSettings.Instance().UseFiducialStep)
            {
                numericUpDownDistanceOffset.Value = (Decimal)SystemManager.Instance().CurrentModel.ModelDescription.FiducialDistanceUm;
            }
        }

        public delegate void EnableFormDelegate(bool flag);
        public void EnableForm(bool flag)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EnableFormDelegate(EnableForm), flag);
                return;
            }
            this.Enabled = flag;            
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (OperationSettings.Instance().UseFiducialStep)
            {
                SystemManager.Instance().CurrentModel.ModelDescription.FiducialDistanceUm = Convert.ToInt32(numericUpDownDistanceOffset.Value);
                SystemManager.Instance().ModelManager.SaveModelDescription(SystemManager.Instance().CurrentModel.ModelDescription);
            }
            panelAlign.Visible = false;
        }

        private void buttonAlign_Click(object sender, EventArgs e)
        {
        }

        private void numericUpDownDistanceOffset_ValueChanged(object sender, EventArgs e)
        {
            if (OperationSettings.Instance().UseFiducialStep)
            {
                SystemManager.Instance().CurrentModel.ModelDescription.FiducialDistanceUm = Convert.ToInt32(numericUpDownDistanceOffset.Value);
            }
        }
       
        private void selectLightButton_DropDownOpening(object sender, EventArgs e)
        {
            UpdateLightTypeCombo();

            selectLightButton.Text = selectLightButton.DropDownItems[this.lightTypeIndex].Text;
        }

        private void addFiducialToolStripButton_Click(object sender, EventArgs e)
        {
            ITeachObject teachObject = teachHandlerProbe.GetSingleSelected();
            if (teachObject == null)
                return;

            targetParamControl.SelectObject(teachObject);

            LogHelper.Debug(LoggerType.Operation, "Modeller - addFiducialButton_Click");

            if (teachObject is VisionProbe)
            {
                VisionProbe probe = (VisionProbe)teachObject;
                Probe alignmentProbe = probe.Target.GetAlignmentProbe();
                curTargetGroup.FiducialSet.AddFiducial(alignmentProbe);
                UpdateCameraImageFigure();
            }
        }

        private void deleteFiducialToolStripButtonToolStripButton_Click(object sender, EventArgs e)
        {
            ITeachObject teachObject = teachHandlerProbe.GetSingleSelected();
            if (teachObject == null)
                return;

            targetParamControl.SelectObject(teachObject);

            LogHelper.Debug(LoggerType.Operation, "Modeller - addFiducialButton_Click");

            if (teachObject is VisionProbe)
            {
                VisionProbe probe = (VisionProbe)teachObject;
                Probe alignmentProbe = probe.Target.GetAlignmentProbe();
                curTargetGroup.FiducialSet.RemoveFiducial(alignmentProbe);
                UpdateCameraImageFigure();
            }
        }

        private void toggleFiducialToolStripButton_Click(object sender, EventArgs e)
        {
            ITeachObject teachObject = teachHandlerProbe.GetSingleSelected();
            if (teachObject == null)
                return;

            targetParamControl.SelectObject(teachObject);

            LogHelper.Debug(LoggerType.Operation, "Modeller - toggleFiducialToolStripButton_Click");

            if (teachObject is VisionProbe)
            {
                VisionProbe probe = (VisionProbe)teachObject;
                Probe alignmentProbe = probe.Target.GetAlignmentProbe();
                if (curTargetGroup.FiducialSet.FiducialExist(alignmentProbe))
                {
                    curTargetGroup.FiducialSet.RemoveFiducial(alignmentProbe);
                }
                else
                {
                    curTargetGroup.FiducialSet.AddFiducial(alignmentProbe);
                }

                UpdateToggleFiducialToolStripButtonColor(alignmentProbe);
                //UpdateCameraImageFigure();
            }
        }

        private void UpdateToggleFiducialToolStripButtonColor(Probe alignmentProbe)
        {
            if (curTargetGroup.FiducialSet.FiducialExist(alignmentProbe))
            {
                curTargetGroup.FiducialSet.AddFiducial(alignmentProbe);
                ((ToolStripButton)toggleFiducialToolStripButton).BackColor = Color.LightGreen;
            }
            else
            {
                curTargetGroup.FiducialSet.RemoveFiducial(alignmentProbe);
                ((ToolStripButton)toggleFiducialToolStripButton).BackColor = default(Color);
            }
        }

        private void UpdateCameraImageFigure()
        {
            if (curTargetGroup == null)
                return;

            LogHelper.Debug(LoggerType.Operation, "Modeller - UpdateCameraImageFigure");

            FigureGroup backgroundFigures = new FigureGroup();
            FigureGroup activeFigures = new FigureGroup();
            activeFigures.Selectable = false;
            curTargetGroup.AppendFigures(activeFigures, null);

            foreach (InspectionStep inspectionStep in SystemManager.Instance().CurrentModel)
            {
                Pen pen = new Pen(Color.LightCyan, 1.0F);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                TargetGroup targetGroup = inspectionStep.GetTargetGroup(curTargetGroup.GroupId, true);
                targetGroup.AppendFigures(backgroundFigures, pen);
            }

            curTargetGroup.FiducialSet.AppendFigures(activeFigures);

            FigureGroup tempFigureGroup = new FigureGroup();
            if (lastInspectionResult != null)
            {
                Pen redPen = new Pen(Color.Red);
                Pen yellowPen = new Pen(Color.Yellow);
                foreach (Target target in curTargetGroup)
                {
                    if (lastInspectionResult.IsDefected(target))
                    {
                        target.AppendFigures(tempFigureGroup, redPen, false);
                    }
                    else if (lastInspectionResult.IsPass(target))
                    {
                        target.AppendFigures(tempFigureGroup, yellowPen, false);
                    }
                }
            }
            teachBox.DrawBox.FigureGroup = activeFigures;
            teachBox.DrawBox.BackgroundFigures = backgroundFigures;
            teachBox.DrawBox.TempFigureGroup = tempFigureGroup;

            teachBox.DrawBox.Invalidate();
        }

        private void resetModelButton_Click(object sender, EventArgs e)
        {

        }

        private void scanButton_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().CurrentModel.CleanImage();

            imagePath = "";
        }

        public void EnableControls(UserType user)
        {

        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == false)
            {
                StopLive();
                StopGrab();

                Model curModel = SystemManager.Instance().CurrentModel;
                if (curModel != null && curModel.IsEmpty() == false)
                    SystemManager.Instance().ModelManager.SaveModel(curModel);
            }
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }
    }
}
