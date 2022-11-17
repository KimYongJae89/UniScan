using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using UniScanG.Common.Data;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.SheetFinder.SheetBase;
using UniScanG.Gravure.Vision.Trainer;
using UniScanG.UI.Teach;
using UniScanG.UI.Teach.Inspector;

namespace UniScanG.Gravure.UI.Teach.Inspector
{
    public partial class TeachToolBarG : UserControl, IModellerControl, IMultiLanguageSupport
    {
        ModellerPageExtender modellerPageExtender;

        public TeachToolBarG()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;
        }

        public void SetModellerExtender(UniScanG.UI.Teach.ModellerPageExtender modellerPageExtender)
        {
            this.modellerPageExtender = (ModellerPageExtender)modellerPageExtender;
        }

        public void UpdateData() { }

        private void buttonSheetGrab_Click(object sender, System.EventArgs e)
        {
            if (!this.modellerPageExtender.IsGrabable)
            {
                if (MessageForm.Show(this, StringManager.GetString("Machine is not running. Continue Anyway?"), MessageFormType.YesNo) == DialogResult.No)
                    return;
            }

            int grabCount = 1;
            if (UserHandler.Instance().CurrentUser.IsMasterAccount)
            {
                bool ok = false;
                while (!ok)
                {
                    InputForm inputForm = new InputForm(StringManager.GetString("How many grab?"), "1");
                    if (inputForm.ShowDialog() == DialogResult.Cancel)
                        return;

                    ok = int.TryParse(inputForm.InputText, out grabCount);
                }
            }
            
            float minBrightnessStdDevBak = 0;
            SheetFinderBaseParam sheetFinder = SheetFinderBase.SheetFinderBaseParam;
            if (sheetFinder != null)
                minBrightnessStdDevBak = sheetFinder.MinBrightnessStdDev;

            try
            {
                if (sheetFinder != null)
                    sheetFinder.MinBrightnessStdDev = 1;

                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn();
                CancellationTokenSource token = new CancellationTokenSource();
                modellerPageExtender.GrabSheet(grabCount, token);
            }
            catch (Exception ex)
            {
                MessageForm.Show(null, string.Format("{0}{1}{2}", StringManager.GetString("Grab Fail."), Environment.NewLine, ex.Message));
            }
            finally
            {
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
                if (sheetFinder != null)
                    sheetFinder.MinBrightnessStdDev = minBrightnessStdDevBak;
            }
        }

        private void buttonFrameGrab_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!this.modellerPageExtender.IsGrabable)
                {
                    if (MessageForm.Show(this, "Machine is not running. Continue Anyway?", MessageFormType.YesNo) == DialogResult.No)
                        return;
                }
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn();
                CancellationTokenSource cts = new CancellationTokenSource();
                modellerPageExtender.GrabFrame(cts);
            }
            catch (Exception ex)
            {
                MessageForm.Show(null, string.Format("{0}{1}{2}", StringManager.GetString("Grab Fail."), Environment.NewLine, ex.Message));
            }
            finally
            {
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
            }
        }

        private void buttonAutoTeach_Click(object sender, System.EventArgs e)
        {
            try
            {
                modellerPageExtender.Teach(new TrainerArgument(false, true, true, true));
                modellerPageExtender.Clear();
            }
            catch (Exception ex)
            {
                MessageForm.Show(this, ex.Message);
            }
        }

        private void buttonInspect_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.TargetRegionInfo = null;
            modellerPageExtender.Inspect();
        }

        private void buttonSave_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.SaveModel();

            int camIdx = SystemManager.Instance().ExchangeOperator.GetCamIndex();
            SystemManager.Instance().ExchangeOperator.ModelTeachDone(camIdx);
        }

        private void buttonLoadImage_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.LoadImage();
        }

        private void buttonExportData_Click(object sender, System.EventArgs e)
        {
            //this.modellerPageExtender.AutoTeachProcess(-1);
            //return;

            Model curModel = SystemManager.Instance().CurrentModel;
            if (curModel == null)
                return;

            ExportData();
        }

        private void ExportData()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"C:\temp";
            if (fbd.ShowDialog() == DialogResult.Cancel)
                return;

            Model curModel = SystemManager.Instance().CurrentModel;
            if (curModel == null)
                return;
            string path = Path.Combine(fbd.SelectedPath, $"{curModel.Name}_{DateTime.Now.ToString("yy-MM-dd HH-mm-ss")}");

            ProgressForm progressForm = new ProgressForm()
            {
                StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen,
                TitleText = StringManager.GetString("Export"),
                MessageText = "",
                Argument = path,
            };
            progressForm.BackgroundWorker.DoWork += ExportDataDoWorkProc;
            progressForm.ShowDialog(this);

            System.Diagnostics.Process.Start(path);
        }

        private void ExportDataDoWorkProc(object sender, DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = (System.ComponentModel.BackgroundWorker)sender;
            string path = (string)e.Argument;

            ModellerPageExtenderG modellerPageExtenderG = (ModellerPageExtenderG)this.modellerPageExtender;
            modellerPageExtenderG.DataExport(worker, path);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void buttonClear_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.Clear();
        }

        private void buttonSaveImage_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.SaveImage();
        }

        private void ButtonIterationTest_Click(object sender, EventArgs e)
        {
            Model curModel = SystemManager.Instance().CurrentModel;
            ImageListForm imageListForm = new ImageListForm(curModel?.Name);
            if (imageListForm.ShowDialog() == DialogResult.Cancel)
                return;

            CancellationTokenSource cts = new CancellationTokenSource();
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(this, () =>
            {
                string modelPath = curModel.ModelPath;
                string[] xmlFiles = Directory.GetFiles(modelPath, "*.xml");
                Array.ForEach(xmlFiles, f =>
                {
                    FileInfo fileInfo = new FileInfo(f);
                    string src = fileInfo.FullName;
                    string dst = Path.Combine(imageListForm.ResultPath, fileInfo.Name);
                    FileHelper.CopyFile(src, dst, true);
                });

                //Vision.AlgorithmSetting.Instance().CalculatorParam.ModelParam.SaveParam();
                FileInfo[] fileInfos = imageListForm.GetFiles();
                foreach (FileInfo fileInfo in fileInfos)
                {
                    string file = fileInfo.FullName;//  Path.Combine(defaultPath, string.Format("{0}.bmp", i));
                    if (!File.Exists(file))
                        continue;
                    try
                    {
                        int index = Array.IndexOf(fileInfos, fileInfo);
                        simpleProgressForm.MessageText = $"{index + 1}/{fileInfos.Length}";

                        // Load
                        cts.Token.ThrowIfCancellationRequested();
                        modellerPageExtender.LoadImage(file);

                        // AutoTeach
                        cts.Token.ThrowIfCancellationRequested();
                        if (imageListForm.UseAutoTeach)
                        {
                            modellerPageExtender.Teach(new TrainerArgument(false, true, true, true) { DoNotSave = true });
                            modellerPageExtender.Clear();
                        }

                        // Inspect
                        cts.Token.ThrowIfCancellationRequested();
                        modellerPageExtender.TargetRegionInfo = null;
                        modellerPageExtender.Inspect();

                        // Save
                        cts.Token.ThrowIfCancellationRequested();
                        int tryId = GetTryId(imageListForm.ResultPath);
                        modellerPageExtender.InspectionResult.InspectionNo = tryId.ToString();
                        this.modellerPageExtender.DataExport(imageListForm.ResultPath);

                        //ModellerPageExtenderG modellerPageExtenderG = (ModellerPageExtenderG)this.modellerPageExtender;
                        //AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, modellerPageExtenderG.CurrentImage, ImageType.Grey);
                        //AlgoImage diffImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, modellerPageExtenderG.DiffImageD, ImageType.Grey);
                        //AlgoImage binnImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, modellerPageExtenderG.BinalImageD, ImageType.Grey);

                        //SaveTestInspection.Save(path, tryId,
                        //    algoImage, diffImage, binnImage,
                        //    (UniScanG.Data.Inspect.InspectionResult)this.modellerPageExtender.InspectionResult);

                        //binnImage.Dispose();
                        //diffImage.Dispose();
                        //algoImage.Dispose();

                        //System.Diagnostics.Process.Start(path);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(LoggerType.Inspection, ex);
                        MessageBox.Show(ex.Message);
                        break;
                    }
                }
            }, cts);
        }

        private int GetTryId(string resultPath)
        {
            int tryId = 0;
            FileInfo[] findInfos = new DirectoryInfo(resultPath).GetFiles();
            while (Array.Exists(findInfos, f => f.Name == tryId.ToString()))
                tryId++;

            return tryId;
        }
    }
}
