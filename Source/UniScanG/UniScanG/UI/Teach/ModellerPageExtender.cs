using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.Data;
using UniScanG.Data.Inspect;
using UniScanG.Data.Model;
using UniScanG.Data.Vision;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach;
using UniScanG.Vision;

namespace UniScanG.UI.Teach
{
    public delegate void OnProgressingDelegate(bool isProgressing);
    public delegate void ImageUpdatedDelegate(Size imageSize);
    public delegate void UpdateZoomDelegate(Rectangle viewPort);
    public delegate void UpdateFigureDelegate(FigureGroup fgFigureGroup, FigureGroup bgFigureGroup);
    public delegate void UpdateSheetResultDelegate(InspectionResult inspectionResult, DebugContext debugContext);
    public delegate void ExportDataDelegate(string path);
    public delegate void OnLineSpeedUpdatedDelegate();
    public delegate void ModelImageLoadDoneDelegate();
    public delegate void OnImageControllerViewPortChangedDelegate(CanvasPanel canvasPanel);

    public abstract class ModellerPageExtender : UniEye.Base.UI.HwTriggerModellerPageExtender, IModelListener
    {
        public event OnProgressingDelegate OnProgressing;
        public event ModelImageLoadDoneDelegate ModelImageLoadDone;
        public event OnImageControllerViewPortChangedDelegate OnImageControllerViewPortChanged;

        public RegionInfo TargetRegionInfo { get => this.targetRegionInfo; set => this.targetRegionInfo = value; }
        protected RegionInfo targetRegionInfo;

        private object currentImageLockObject = new object();
        public Image2D CurrentImage
        {
            get => this.currentImage;
            set
            {
                lock(currentImageLockObject)
                    this.currentImage = value;
            }
        }
        Image2D currentImage = null;

        public bool LineStartState
        {
            get => this.lineStartState;
            set
            {
                if (this.lineStartState != value)
                {
                    this.lineStartState = value;
                    OnLineSpeedUpdated?.Invoke();
                }
            }
        }
        protected bool lineStartState = false;

        public float LineSpeedMpm
        {
            get => this.lineSpeedMpm;
            set
            {
                if (this.lineSpeedMpm != value)
                {
                    this.lineSpeedMpm = value;
                    OnLineSpeedUpdated?.Invoke();
                }
            }
        }
        protected float lineSpeedMpm = 0;

        public bool IsModelImageLoading
        {
            get => this.isModelImageLoading;
            set
            {
                if (this.isModelImageLoading != value)
                {
                    this.isModelImageLoading = value;
                    OnProgressing?.Invoke(value);
                }
            }
        }
        bool isModelImageLoading = false;
        public bool IsGrabable => this.lineStartState || this.lineSpeedMpm > 0;

        public ImageUpdatedDelegate ImageUpdated;
        public UpdateZoomDelegate UpdateZoom;
        public UpdateFigureDelegate UpdateFigure;
        public UpdateSheetResultDelegate UpdateSheetResult;
        public ExportDataDelegate ExportData;
        public event OnLineSpeedUpdatedDelegate OnLineSpeedUpdated;
        //public AddDontcareRegionDelegate AddDontcareRegion;

        public ModellerPageExtender()
        {
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
#if DEBUG
            this.lineSpeedMpm = 30;
#endif
        }

        public void RequestLineSpeed()
        {
            if (SystemManager.Instance().ExchangeOperator.IsConnected)
                SystemManager.Instance().ExchangeOperator.SendCommand(Common.Exchange.ExchangeCommand.C_SPD);
        }

        public virtual void DataExport(string path)
        {
            ExportData?.Invoke(path);
        }

        public void SaveImage()
        {
            if (currentImage == null)
                return;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Supported Files(*.png,*.bmp)|*.png;*.bmp|PNG File(*.png)|*.png|BMP File(*.bmp)|*.bmp";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
                simpleProgressForm.Show(() =>
                {
                    //AlgorithmStrategy algorithmStrategy = AlgorithmBuilder.GetStrategy("ImageSaver");
                    //if (algorithmStrategy != null && algorithmStrategy.Enabled)
                    //{
                    //    using (AlgoImage algoImage = ImageBuilder.Build(algorithmStrategy.AlgorithmType, this.currentImage))
                    //        algoImage.Save(dlg.FileName);
                    //}
                    //else
                    {
                        currentImage.SaveImage(dlg.FileName);
                    }
                });
            }
        }

        public void ImageController_OnViewPortChanged(CanvasPanel canvasPanel)
        {
            this.OnImageControllerViewPortChanged?.Invoke(canvasPanel);
        }

        public void LoadImage()
        {
            string defaultPath = SystemManager.Instance().CurrentModel?.GetImagePath();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Supported Files(*.png,*.bmp)|*.png;*.bmp|PNG File(*.png)|*.png|BMP File(*.bmp)|*.bmp";
            //dlg.InitialDirectory = defaultPath;

            if (dlg.ShowDialog() == DialogResult.OK)
                LoadImage(dlg.FileName);
        }

        public void LoadImage(string fileName)
        {
            //if (this.currentImage != null)
            //    this.currentImage.Dispose();

            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();

            Image2D image2D = null;
            simpleProgressForm.Show(ConfigHelper.Instance().MainForm, () =>
            {
                image2D = new Image2D(fileName);
                //image2D.ConvertFromData();
            });

            if (image2D != null && UpdateImage != null)
            {
                SystemManager.Instance().CurrentModel.ImageModified = true;
                this.CurrentImage = image2D;
                UpdateImage?.Invoke(image2D, false);
                ModelImageLoadDone?.Invoke();
                IsModelImageLoading = false;
            }
        }

        public abstract void AddCriticalPoint(Rectangle rectangle);
        public abstract void RemoveCriticalPoint(Rectangle rectangle);

        public abstract void AddDontcareRegion(Rectangle rectangle);
        public abstract void GrabSheet(int count, CancellationTokenSource cts);
        public abstract void GrabFrame(CancellationTokenSource cts);

        public delegate void TeachDelegate(object arg);
        public void Teach(object arg = null)
        {
            Form mainForm = ConfigHelper.Instance().MainForm;
            if (mainForm.InvokeRequired)
            {
                mainForm.Invoke(new TeachDelegate(Teach), arg);
                return;
            }

            if (SystemManager.Instance().CurrentModel == null)
                throw new Exception("There is no Model Selected");

            Etc.ProgressForm progressForm = new Etc.ProgressForm();
            progressForm.StartPosition = FormStartPosition.CenterScreen;
            //progressForm.StartPosition = FormStartPosition.Manual;

            progressForm.TitleText = StringManager.GetString(this.GetType().FullName, "Auto Teach");
            progressForm.MessageText = StringManager.GetString(this.GetType().FullName, "Start");

            progressForm.BackgroundWorker.DoWork += TeachBackgroundWorker_DoWork;
            progressForm.RunWorkerCompleted += TeachRunWorkerCompleted;
            progressForm.Argument = arg;

            DialogResult dialogResult = progressForm.ShowDialog(mainForm);
            if (dialogResult != DialogResult.OK)
            {
                Exception ex = progressForm.LastException;
                if (ex != null)
                    throw ex;
            }
        }

        protected abstract void TeachBackgroundWorker_DoWork(object sender, DoWorkEventArgs e);
        protected abstract void TeachRunWorkerCompleted(object result);
        protected abstract Image2D GetMarkedPrevImage();

        public virtual void SaveModel()
        {
            Form mainForm = ConfigHelper.Instance().MainForm;
            if (mainForm.InvokeRequired)
            {
                mainForm.Invoke(new MethodInvoker(SaveModel));
                return;
            }

            UniScanG.Data.Model.Model currentModel = SystemManager.Instance().CurrentModel;
            if (currentModel == null)
                return;

            LogHelper.Debug(LoggerType.Operation, $"ModellerPageExtender::SaveModel - {currentModel.Name}");
            string imagePath = currentModel.GetImagePath();
            if (Directory.Exists(imagePath) == false)
                Directory.CreateDirectory(imagePath);

            new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Save Model")).Show(mainForm, new Action(() =>
            {
                while (this.IsModelImageLoading)
                    Thread.Sleep(500);

                List<Task> taskList = new List<Task>();

                string fileName = GetModelImageName();
                string previewFileName = currentModel.GetPreviewImagePath("");
                string preview0FileName = currentModel.GetPreviewImagePath("0");

                if (currentModel.ImageModified)
                //  썸네일 이미지 삭제. 원본 이미지 저장.
                {
                    File.Delete(previewFileName);
                    File.Delete(preview0FileName);
                    currentModel.Modified = true;
                    taskList.Add(Task.Run(() => 
                    {
                        string fullName = Path.Combine(imagePath, fileName);
                        string tempName = Path.ChangeExtension(fullName, "tmp");
                        currentImage?.SaveImage(tempName, ImageFormat.Png);
                        try
                        {
                            FileHelper.Move(tempName, fullName);
                        }
                        catch(Exception ex)
                        {
                            LogHelper.Error(LoggerType.Error, $"ModellerPageExtender::SaveModel - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                        }
                        //Array.ForEach(Directory.GetFiles(imagePath, "*.bmp"), f => File.Delete(f));

                        currentModel.RCITrainResult.SaveImages(Path.Combine(imagePath, "New"));
                    }));
                    currentModel.ImageModified = false;
                }

                if (!File.Exists(previewFileName))
                // 썸네일 이미지 저장
                {
                    taskList.Add(Task.Run(() =>
                    {
                        using (ImageD imageD = SheetCombiner.CreatePrevImage(currentImage))
                            imageD?.SaveImage(previewFileName);
                    }));
                }

                if (currentModel.Modified)
                {
                    Task task = Task.Run(() =>
                    {
                        SystemManager.Instance().ModelManager.SaveModel(currentModel);

                        using (Image2D prevImage = GetMarkedPrevImage())
                            prevImage?.SaveImage(preview0FileName);

                        UniEye.Base.Settings.AdditionalSettings.Instance().Save();
                        UniEye.Base.Settings.OperationSettings.Instance().Save();
                    });
                    taskList.Add(task);
                }

                taskList.ForEach(f => f.Wait());
            }));
        }

        public abstract void Inspect();
        public virtual void Clear()
        {
            UpdateSheetResult?.Invoke(null, null);
        }

        public abstract string GetModelImageName();

        public void ModelChanged()
        {
            IsModelImageLoading = true;
            UpdateImage?.Invoke(null, true);
            Clear();

            this.currentImage?.Dispose();
            this.currentImage = null;

            Task t = Task.Run(() =>
            {
                string imagePath = SystemManager.Instance().CurrentModel.GetImagePath();

                string filePathPng = Path.Combine(imagePath, GetModelImageName());
                string filePathTmp = Path.ChangeExtension(filePathPng, "tmp");
                string filePathBmp = Path.ChangeExtension(filePathPng, "bmp");

                Image2D image2D = null;
                string filePath = "";
                try
                {
                    if(File.Exists(filePathTmp))
                        FileHelper.Move(filePathTmp, filePathPng);

                    if (File.Exists(filePathPng))
                    {
                        image2D = new Image2D(filePath = filePathPng);
                        if (File.Exists(filePathBmp))
                            File.Delete(filePathBmp);
                    }

                    if (File.Exists(filePathBmp))
                    {
                        image2D = new Image2D(filePath = filePathBmp);
                    }

                    SystemManager.Instance().CurrentModel.RCITrainResult.LoadImages(Path.Combine(imagePath, "New"));
                    //using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    //    Thread.Sleep(99999999);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, $"ModellerPageExtender::ModelChanged - {filePath} {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                    image2D = null;
                }

                if (IsModelImageLoading)
                {
                    this.currentImage = image2D;
                    UpdateImage?.Invoke(image2D, true);
                    ModelImageLoadDone?.Invoke();

                    IsModelImageLoading = false;
                }
            });
            t.Wait();
        }


        public void ModelTeachDone(int camId) { }

        public void ModelRefreshed() { }
    }
}