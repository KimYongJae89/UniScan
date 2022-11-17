using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    partial class ModellerPage
    {
        private void ChangeMenuState()
        {
            importGerberButton.Visible = false;
            syncAllToolStripButton.Visible = false;

            SystemManager.Instance().UiChanger.ChangeModellerMenu(this);

            //SystemType systemType = OperationSettings.Instance().SystemType;
            //switch(systemType)
            //{
            //    case SystemType.ShampooBarcode:
            //        toolStripSeparator3.Visible = false;
            //        groupProbeToolStripButton.Visible = false;
            //        ungroupProbeToolStripButton.Visible = false;
            //        toolStripSeparator5.Visible = false;
            //        setFiducialToolStripButton.Visible = false;
            //        setTargetCalibrationToolStripButton.Visible = false;
            //        toolStripSeparator2.Visible = false;
            //        syncParamToolStripButton.Visible = false;
            //        syncAllToolStripButton.Visible = false;
            //        break;
            //    case SystemType.BoxBarcode:
            //        setFiducialToolStripButton.Visible = false;
            //        setTargetCalibrationToolStripButton.Visible = false;
            //        syncParamToolStripButton.Visible = false;
            //        syncAllToolStripButton.Visible = false;
            //        break;
            //    case SystemType.UniEyeS:
            //        setTargetCalibrationToolStripButton.Visible = false;
            //        multiShotToolStripButton.Visible = false;
            //        previewTypeToolStripButton.Visible = false;
            //        grabProcessToolStripButton.Visible = false;
            //        setFiducialToolStripButton.Visible = false;
            //        syncParamToolStripButton.Visible = false;
            //        syncAllToolStripButton.Visible = false;
            //        break;
            //    case SystemType.DrugPackaging:
            //        setFiducialToolStripButton.Visible = false;
            //        setTargetCalibrationToolStripButton.Visible = false;
            //        syncParamToolStripButton.Visible = false;
            //        syncAllToolStripButton.Visible = false;
            //        groupProbeToolStripButton.Visible = false;
            //        ungroupProbeToolStripButton.Visible = false;
            //        break;
            //    case SystemType.MaskInspector:
            //        importGerberButton.Visible = true;
            //        exportFormatButton.Visible = false;
            //        undoToolStripButton.Visible = false;
            //        RedoToolStripButton.Visible = false;
            //        break;
            //    case SystemType.FPCBAlignChecker:
            //    case SystemType.FPCBAlignChecker2:
            //        syncAllToolStripButton.Visible = false;
            //        break;
            //    case SystemType.KimmConfocal:
            //        exportFormatButton.Visible = false;
            //        break;
            //}

            if (MachineSettings.Instance().RunningMode == RunningMode.Real)
            {
                loadImageSetToolStripButton.Visible = false;
                selectPrevImageSetToolStripButton.Visible = false;
                selectNextImageSetToolStripButton.Visible = false;
                //toolStripSeparator8.Visible = false;
            }
            else
            {
                grabProcessToolStripButton.Visible = false;
                //showLightPanelToolStripButton.Visible = false;
                multiShotToolStripButton.Visible = false;
                singleShotToolStripButton.Visible = false;
            }
        }

        private void BuildAlgorithmTypeMenu()
        {
            addProbeToolStripButton.DropDownItems.Clear();

            BuildLiteAlgorithmTypeMenu();
            BuildDimensionAlgorithmTypeMenu();
            BuildIdentificationAlgorithmTypeMenu();

            SystemManager.Instance().UiChanger.BuildAdditionalAlgorithmTypeMenu(this, addProbeToolStripButton.DropDownItems);

            // BuildTubiAlgorithmTypeMenu();

            //if (AlgorithmBuilder.IsAlgorithmEnabled(FpcbAlignChecker.TypeName))
            //{
            //    ToolStripButton fpcbAlignCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "PCB Alignment Checker"));
            //    fpcbAlignCheckerToolStripButton.Click += FpcbAlignCheckerToolStripButton_Click;
            //    addProbeToolStripButton.DropDownItems.Add(fpcbAlignCheckerToolStripButton);
            //}

            //if (AlgorithmBuilder.IsAlgorithmEnabled(ContactLensChecker.TypeName))
            //{
            //    ToolStripButton contactLensCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Contact Lens Checker"));
            //    contactLensCheckerToolStripButton.Click += ContactLensCheckerToolStripButton_Click;
            //    addProbeToolStripButton.DropDownItems.Add(contactLensCheckerToolStripButton);
            //}

            //if (OperationSettings.Instance().SystemType == SystemType.Calinar)
            //{
            //    ToolStripButton calibrationCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Calibration Checker"));
            //    calibrationCheckerToolStripButton.Click += CalibrationCheckerToolStripButton_Click;
            //    addProbeToolStripButton.DropDownItems.Add(calibrationCheckerToolStripButton);

            //    ToolStripButton daqProbeToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Daq Value Checker"));
            //    daqProbeToolStripButton.Click += DaqProbeToolStripButton_Click;
            //    addProbeToolStripButton.DropDownItems.Add(daqProbeToolStripButton);

            //    ToolStripButton carAlignCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Car Align Checker"));
            //    carAlignCheckerToolStripButton.Click += CarAlignCheckerToolStripButton_Click;
            //    addProbeToolStripButton.DropDownItems.Add(carAlignCheckerToolStripButton);
            //}
            //if (OperationSettings.Instance().SystemType == SystemType.MaskInspector)
            //{
            //    ToolStripButton tensionCheckerParamControlToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Tension Checker"));
            //    tensionCheckerParamControlToolStripButton.Click += TensionParamControlToolStripButton_Click;
            //    addProbeToolStripButton.DropDownItems.Add(tensionCheckerParamControlToolStripButton);
            //}

            
            if (SystemManager.Instance().DeviceBox.ImageDeviceHandler.IsDepthScannerExist())
            {
                ToolStripButton depthCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Depth Checker"));
                depthCheckerToolStripButton.Click += DepthCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(depthCheckerToolStripButton);

                ToolStripButton markerProbeToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Marker"));
                markerProbeToolStripButton.Click += MarkerProbeToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(markerProbeToolStripButton);
            }

            addProbeToolStripButton.DropDown.Width = 200;
            addProbeToolStripButton.DropDown.Height = 1000;
        }

        private void MarkerProbeToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - MarkerProbeParamControlToolStripButton_Click");

            RotatedRect rect = GetDefaultProbeRegion();
            MarkerProbe markerProbe = (MarkerProbe)ProbeFactory.Create(ProbeType.Marker);
            markerProbe.BaseRegion = rect;

            AddProbe(markerProbe);
        }

        //private void BuildTubiAlgorithmTypeMenu()
        //{
        //    if (AlgorithmBuilder.IsAlgorithmEnabled(DirtyChecker.TypeName))
        //    {
        //        ToolStripButton dirtyCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Dirty Checker"));
        //        dirtyCheckerToolStripButton.Click += DirtyCheckerToolStripButton_Click;
        //        addProbeToolStripButton.DropDownItems.Add(dirtyCheckerToolStripButton);
        //    }

        //    if (AlgorithmBuilder.IsAlgorithmEnabled(SealingChecker.TypeName))
        //    {
        //        ToolStripButton sealingCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Sealing Checker"));
        //        sealingCheckerToolStripButton.Click += SealingCheckerToolStripButton_Click;
        //        addProbeToolStripButton.DropDownItems.Add(sealingCheckerToolStripButton);
        //    }
        //}

        void BuildLiteAlgorithmTypeMenu()
        {
            if (AlgorithmBuilder.IsAlgorithmEnabled(PatternMatching.TypeName))
            {
                ToolStripButton patternMatchingToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Pattern Matching"));
                patternMatchingToolStripButton.Click += PatternMatchinToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(patternMatchingToolStripButton);
            }

            if (AlgorithmBuilder.IsAlgorithmEnabled(BinaryCounter.TypeName))
            {
                ToolStripButton binaryCounterToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Binary Counter"));
                binaryCounterToolStripButton.Click += BinaryCounterToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(binaryCounterToolStripButton);
            }

            if (AlgorithmBuilder.IsAlgorithmEnabled(BrightnessChecker.TypeName))
            {
                ToolStripButton brightnessCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Brightness Checker"));
                brightnessCheckerToolStripButton.Click += BrightnessCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(brightnessCheckerToolStripButton);
            }

            if (AlgorithmBuilder.IsAlgorithmEnabled(ColorChecker.TypeName))
            {
                ToolStripButton colorCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Color Checker"));
                colorCheckerToolStripButton.Click += ColorCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(colorCheckerToolStripButton);
            }
        }

        void BuildDimensionAlgorithmTypeMenu()
        {
            if (AlgorithmBuilder.IsAlgorithmEnabled(LineChecker.TypeName))
            {
                ToolStripButton lineCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Line Checker"));
                lineCheckerToolStripButton.Click += LineCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(lineCheckerToolStripButton);
            }

            if (AlgorithmBuilder.IsAlgorithmEnabled(WidthChecker.TypeName))
            {
                ToolStripButton widthCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Width Checker"));
                widthCheckerToolStripButton.Click += WidthCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(widthCheckerToolStripButton);
            }

            if (AlgorithmBuilder.IsAlgorithmEnabled(CornerChecker.TypeName))
            {
                ToolStripButton cornerCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Corner Checker"));
                cornerCheckerToolStripButton.Click += CornerCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(cornerCheckerToolStripButton);
            }

            if (AlgorithmBuilder.IsAlgorithmEnabled(RectChecker.TypeName))
            {
                ToolStripButton rectCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Rect Checker"));
                rectCheckerToolStripButton.Click += RectCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(rectCheckerToolStripButton);
            }

            if (AlgorithmBuilder.IsAlgorithmEnabled(CircleChecker.TypeName))
            {
                ToolStripButton circleFinderToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Circle Finder"));
                circleFinderToolStripButton.Click += CircleFinderToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(circleFinderToolStripButton);
            }

            if (AlgorithmBuilder.IsAlgorithmEnabled(BlobChecker.TypeName))
            {
                ToolStripButton blobCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Blob Checker"));
                blobCheckerToolStripButton.Click += BlobCheckerToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(blobCheckerToolStripButton);
            }

            //if (AlgorithmBuilder.IsAlgorithmEnabled(TensionChecker.TypeName))
            //{
            //    ToolStripButton tensionCheckerToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Tension Checker"));
            //    tensionCheckerToolStripButton.Click += tensionCheckerToolStripButton_Click;
            //    addProbeToolStripButton.DropDownItems.Add(tensionCheckerToolStripButton);
            //}
        }
        void BuildIdentificationAlgorithmTypeMenu()
        {
            if (AlgorithmBuilder.IsAlgorithmEnabled(BarcodeReader.TypeName))
            {
                ToolStripButton barcodeReaderToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Barcode Reader"));
                barcodeReaderToolStripButton.Click += BarcodeReaderToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(barcodeReaderToolStripButton);
            }

            if (AlgorithmBuilder.IsAlgorithmEnabled(CharReader.TypeName))
            {
                ToolStripButton characterReaderToolStripButton = new ToolStripButton(StringManager.GetString(this.GetType().FullName, "Character Reader"));
                characterReaderToolStripButton.Click += CharacterReaderToolStripButton_Click;
                addProbeToolStripButton.DropDownItems.Add(characterReaderToolStripButton);
            }
        }

        void BuildOcrAlgorithmTypeMenu()
        {
        }

        private void ColorCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ColorCheckerToolStripButton_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;

            ColorChecker colorChecker = new ColorChecker();
            probe.InspAlgorithm = colorChecker;

            AddVisionProbe(probe);
        }

        private void DepthCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - DepthCheckerToolStripButton_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;

            DepthChecker depthChecker = new DepthChecker();
            probe.InspAlgorithm = depthChecker;

            AddVisionProbe(probe);
        }

        //private void ContactLensCheckerToolStripButton_Click(object sender, EventArgs e)
        //{
        //    LogHelper.Debug(LoggerType.OpDebug, "ModellerPage - contactLensCheckerToolStripMenuItem_Click");

        //    VisionProbe probe = CreateVisionProbe();
        //    if (probe == null)
        //        return;

        //    ContactLensChecker contactLensChecker = new ContactLensChecker();
        //    probe.InspAlgorithm = contactLensChecker;

        //    AddVisionProbe(probe);
        //}

        //private void FpcbAlignCheckerToolStripButton_Click(object sender, EventArgs e)
        //{
        //    LogHelper.Debug(LoggerType.OpDebug, "ModellerPage - fpcbAlignCheckerToolStripMenuItem_Click");

        //    VisionProbe probe = CreateVisionProbe();
        //    if (probe == null)
        //        return;

        //    FpcbAlignChecker fogAlignChecker = new FpcbAlignChecker();
        //    probe.InspAlgorithm = fogAlignChecker;

        //    AddVisionProbe(probe);
        //}

        //private void SealingCheckerToolStripButton_Click(object sender, EventArgs e)
        //{
        //    LogHelper.Debug(LoggerType.OpDebug, "ModellerPage - sealingCheckerToolStripMenuItem_Click");

        //    VisionProbe probe = CreateVisionProbe();
        //    if (probe == null)
        //        return;

        //    SealingChecker sealingChecker = new SealingChecker();
        //    probe.InspAlgorithm = sealingChecker;

        //    AddVisionProbe(probe);
        //}

        //private void DirtyCheckerToolStripButton_Click(object sender, EventArgs e)
        //{
        //    LogHelper.Debug(LoggerType.OpDebug, "ModellerPage - DirtyCheckerToolStripButton_Click");

        //    VisionProbe probe = CreateVisionProbe();
        //    if (probe == null)
        //        return;

        //    DirtyChecker dirtyChecker = new DirtyChecker();
        //    probe.InspAlgorithm = dirtyChecker;

        //    AddVisionProbe(probe);
        //}

        private void CharacterReaderToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CharacterReaderToolStripButton_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;

            CharReader charReader = AlgorithmBuilder.CreateCharReader();
            charReader.ImagingLibrary = OperationSettings.Instance().ImagingLibrary;

            probe.InspAlgorithm = charReader;

            AddVisionProbe(probe);
        }

        private void DaqProbeToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - DaqProbeToolStripButton_Click");

            DaqProbe daqProbe = CreateDaqProbe();
            if (daqProbe == null)
                return;

            AddProbe(daqProbe);
        }

        //private void TensionParamControlToolStripButton_Click(object sender, EventArgs e)
        //{
        //    LogHelper.Debug(LoggerType.OpDebug, "ModellerPage - TensionParamControlToolStripButton_Click");

        //    SerialProbe tensionSerialProbe = CreateTensionSerialProbe();
        //    if (tensionSerialProbe == null)
        //        return;

        //    AddProbe(tensionSerialProbe);
        //}

        private void CalibrationCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CalibrationCheckerToolStripButton_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;

            probe.InspAlgorithm = new CalibrationChecker();

            AddVisionProbe(probe);
        }

        //private void CarAlignCheckerToolStripButton_Click(object sender, EventArgs e)
        //{
        //    LogHelper.Debug(LoggerType.OpDebug, "ModellerPage - CarAlignCheckerToolStripButton_Click");

        //    VisionProbe probe = CreateVisionProbe();
        //    if (probe == null)
        //        return;

        //    probe.InspAlgorithm = new CarAlignChecker();

        //    AddVisionProbe(probe);
        //}

        //private void TensionCheckerParamControlToolStripButton_Click(object sender, EventArgs e)
        //{
        //    LogHelper.Debug(LoggerType.OpDebug, "ModellerPage - TensionCheckerParamControlToolStripButton_Click");

        //    Probe probe = ProbeFactory.Create(ProbeType.Tension);

        //    AddProbe(probe);
        //}

        private void BarcodeReaderToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - barcodeReaderToolStripMenuItem_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;

            RotatedRect inspRegion = probe.BaseRegion;

            BarcodeReader barcodeReader = AlgorithmBuilder.CreateBarcodeReader();

            probe.InspAlgorithm = barcodeReader;

            AddVisionProbe(probe);
        }

        private void DistanceCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - distanceCheckerToolStripMenuItem_Click");

            ComputeProbe computeProbe = CreateComputeProbe();

            //if (computeProbe == null)
            //    return;            

            //AddComputeProbe(computeProbe);
            ComputeParamControl computeParamControl = new ComputeParamControl();
            this.paramContainer.Panel1.Controls.Clear();
            computeParamControl.Model = SystemManager.Instance().CurrentModel;
            this.paramContainer.Panel1.Controls.Add(computeParamControl);
        }

        //private void lensCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    LogHelper.Debug(LoggerType.OpDebug, "ModellerPage - lensCheckerToolStripMenuItem_Click");

        //    VisionProbe probe = CreateVisionProbe();
        //    if (probe == null)
        //        return;

        //    ContactLensChecker lensChecker = new ContactLensChecker();
        //    probe.InspAlgorithm = lensChecker;

        //    AddVisionProbe(probe);
        //}

        private void RectCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - rectCheckerToolStripMenuItem_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;

            probe.InspAlgorithm = new RectChecker();

            AddVisionProbe(probe);
        }

        private void BlobCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - blobCheckerToolStripMenuItem_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;
            BlobChecker blobChecker = new BlobChecker();
            probe.InspAlgorithm = blobChecker;

            AddVisionProbe(probe);
        }

        //private void PadCheckerToolStripButton_Click(object sender, EventArgs e)
        //{
        //    LogHelper.Debug(LoggerType.OpDebug, "ModellerPage - PadCheckerToolStripButton_Click");

        //    VisionProbe probe = CreateVisionProbe();
        //    if (probe == null)
        //        return;
        //    PadChecker padChecker = new PadChecker();
        //    probe.InspAlgorithm = padChecker;

        //    AddVisionProbe(probe);
        //}

        private void CircleFinderToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - circleFinderToolStripMenuItem_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;
            CircleChecker circleChecker = new CircleChecker();
            probe.InspAlgorithm = circleChecker;

            AddVisionProbe(probe);
        }

        private void CornerCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - cornerCheckerToolStripMenuItem_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;
            CornerChecker cornerChecker = new CornerChecker();
            probe.InspAlgorithm = cornerChecker;

            AddVisionProbe(probe);
        }

        private void WidthCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - WidthCheckerToolStripButton_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;

            WidthChecker widthChecker = new WidthChecker();
            probe.InspAlgorithm = widthChecker;

            AddVisionProbe(probe);
        }

        private void LineCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - lineCheckerToolStripMenuItem_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;
            LineChecker lineChecker = new LineChecker();
            probe.InspAlgorithm = lineChecker;

            AddVisionProbe(probe);
        }

        private void BrightnessCheckerToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - BrightnessCheckerToolStripButton_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;

            BrightnessChecker brightnessChecker = new BrightnessChecker();
            probe.InspAlgorithm = brightnessChecker;

            AddVisionProbe(probe);
        }

        internal void Teach()
        {
            throw new NotImplementedException();
        }

        private void BinaryCounterToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - BinaryCounterToolStripButton_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;

            BinaryCounter binaryCounter = new BinaryCounter();
            probe.InspAlgorithm = binaryCounter;

            AddVisionProbe(probe);
        }

        private void PatternMatchinToolStripButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - patternMatchingToolStripMenuItem_Click");

            VisionProbe probe = CreateVisionProbe();
            if (probe == null)
                return;

            RotatedRect inspRegion = probe.BaseRegion;
            probe.BaseRegion = inspRegion;

            PatternMatching patternMatching = new PatternMatching();

            DynMvp.Vision.Pattern pattern = AlgorithmBuilder.CreatePattern();

            Bitmap clipImage = GetClipImage(probe.BaseRegion);
            pattern.Train(Image2D.FromBitmap(clipImage), null);

            probe.InspAlgorithm = patternMatching;
          
            AddVisionProbe(probe);

            // 보정프로브로 설정
            probe.Target.SelectFiducialProbe(probe.Id);
        }
    }
}
