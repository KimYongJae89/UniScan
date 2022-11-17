using DynMvp;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using DynMvp.Vision.OpenCv;
using RCITest.Controls;
using RCITest.Helper;
using RCITest.Processer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.RCI;
using UniScanG.Gravure.Vision.RCI.Calculator;
using UniScanG.Gravure.Vision.RCI.Trainer;
using UniScanG.Vision;

namespace RCITest
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();

        ImageControl srcImageControl;
        ImageControl dstImageControl;

        CustomChartControl chartControl;

        public Options Options { get; private set; }

        public UniScanG.Data.Model.Model Model { get; private set; }

        public CalculatorResultV3 ResultV3 { get; private set; }
        public SimpleBlobResult SimpleBlobResult { get; private set; }

        public CancellationTokenSource BatchCTS { get; private set; }

        public Form1()
        {
            InitializeComponent();

            this.srcImageControl = new ImageControl() { Dock = DockStyle.Fill };
            this.srcImageControl.OnZoomScaleChanged += new OnViewPortChangedDelegate((c) => ImageControl_OnZoomScaleChanged(this.srcImageControl, this.dstImageControl));
            this.tableLayoutPanel1.Controls.Add(this.srcImageControl, 0, 0);

            this.dstImageControl = new ImageControl() { Dock = DockStyle.Fill };
            this.dstImageControl.OnZoomScaleChanged += new OnViewPortChangedDelegate((c) => ImageControl_OnZoomScaleChanged(this.dstImageControl, this.srcImageControl));
            this.tableLayoutPanel1.Controls.Add(this.dstImageControl, 1, 0);
            this.dstImageControl.CanvasPanel.FigureMouseEnter += new FigureMouseOverDelegate(figure =>
            {
                this.toolStripStatusLabel1.Text = figure?.Tag?.ToString();
            });

            this.chartControl = new CustomChartControl() { Zoomable = true, Dock = DockStyle.Fill };
            //this.tableLayoutPanel2.Controls.Add(this.chartControl, 0, 0);

            this.buttonLoadSrc.Tag = this.buttonAnalyzeSrc.Tag = this.srcImageControl;
            this.buttonLoadDst.Tag = this.dstImageControl;

            this.Model = new UniScanG.Data.Model.Model()
            {
                RCITrainResult = new RCITrainResult(),
                RCIOptions = new RCIOptions()
            };

            this.Options = new Options()
            {
                RCIOptions = this.Model.RCIOptions
            };
        }

        private void ImageControl_OnZoomScaleChanged(ImageControl from, ImageControl to)
        {
            if (to.CanvasPanel.ViewPort != from.CanvasPanel.ViewPort)
                to.CanvasPanel.ViewPort = from.CanvasPanel.ViewPort;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Configuration.TrackerSize = 1;
            this.Options.Load(Environment.CurrentDirectory);

            this.propertyGridDebug.SelectedObject = this.Options;
            this.propertyGridDebug.PropertyValueChanged += new PropertyValueChangedEventHandler((s, args) => this.Options.Save(Environment.CurrentDirectory));

            if (Program.ImagingLibrary == ImagingLibrary.MatroxMIL)
                MatroxHelper.InitApplication();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Options.Save(Environment.CurrentDirectory);
            MatroxHelper.FreeApplication();
        }

        private async void buttonLoad_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImageControl imageControl = (ImageControl)button.Tag;
            string[] selectes = SelectImage(false);
            if (selectes.Length == 0)
                return;

            Program.ResetAllTimer();
            await LoadImage(selectes[0], imageControl, null);
        }

        private string[] SelectImage(bool canMultiSelect)
        {
            return SelectFiles("bitmap(*.bmp)|*.bmp|png(*.png)|*.png", canMultiSelect);
        }

        private string[] SelectFiles(string filter, bool canMultiSelect)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = this.Options.PATH;
            dlg.FileName = this.Options.SEARCH_PATTERN;
            dlg.Filter = filter;
            dlg.Multiselect = canMultiSelect;
            if (dlg.ShowDialog() != DialogResult.OK)
                return new string[0];

            return dlg.FileNames;
        }

        private async Task LoadImage(string file, ImageControl imageControl, CancellationTokenSource cts)
        {
            await ShowProgressForm("Load Image", new Action(() =>
            {
                Program.TimeCheck("LoadImage", file, new Action(() =>
                {
                    Image2D imageD = new Image2D(file);
                    imageControl.Set(Path.GetFileNameWithoutExtension(file), imageD);
                }));
            }), cts);
        }

        private async void buttonAnalyze_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImageControl imageControl = (ImageControl)button.Tag;
            Program.ResetAllTimer();

            await Analyze(imageControl.Key, imageControl.ImageD);
        }

        private async Task Analyze(string key, ImageD imageD, CancellationTokenSource cts = null)
        {
            if (imageD == null)
                return;

            await ShowProgressForm("Analyzing", new Action(() =>
            {
                Program.BeginTimeCheck("Analyze");
                using (AlgoImage algoImage = ImageBuilder.Build(Program.ImagingLibrary, imageD, ImageType.Grey))
                {
                    AlgorithmSetting.Instance().RCIGlobalOptions.CopyFrom(this.Options.RCIGlobalOptions);

                    RCITrainer Trainer = new RCITrainer();
                    DebugContextG debugContextG = new DebugContextG(new DebugContext(this.Options.SaveDebugImage, this.Options.PATH_DEBUG));
                    Trainer.TrainPattern(algoImage, this.Model, 0, 0, debugContextG);
                    Trainer.TrainRegion(algoImage, this.Model, 0, 0, debugContextG);
                }
                this.Model.RCITrainResult.ElapsedMs = Program.EndTimeCheck("Analyze");
                UpdateTeachFigure();
            }), cts);
        }

        private void SaveLogData(string[] keys, float[] refDatas, float[][] compDatas, string fileName1, string fileName2)
        {
            int cols = keys.Length;
            int rows = refDatas.Length;

            // Feature 위치 변화량.
            StringBuilder sbFeature = new StringBuilder();
            sbFeature.Append("Ref, ");
            sbFeature.AppendLine(string.Join(", ", keys));
            for (int row = 0; row < rows; row++)
            {
                float[] colDatas = new float[cols + 1];
                for (int col = -1; col < cols; col++)
                {
                    if (col < 0)
                    {
                        colDatas[col + 1] = refDatas.ElementAt(row);
                    }
                    else
                    {
                        colDatas[col + 1] = compDatas.ElementAt(col).ElementAt(row);
                    }
                }
                sbFeature.AppendLine(string.Join(", ", string.Join(", ", colDatas)));
            }
            System.IO.File.WriteAllText(fileName1, sbFeature.ToString());

            // Block 크기 변화량
            StringBuilder sbBlock = new StringBuilder();
            sbBlock.Append("Ref, ");
            sbBlock.AppendLine(string.Join(", ", keys));
            for (int row = 0; row < rows - 1; row++)
            {
                float[] colDatas = new float[cols + 1];
                for (int col = -1; col < cols; col++)
                {
                    if (col < 0)
                        colDatas[col + 1] = (refDatas.ElementAt(row + 1) - refDatas.ElementAt(row));
                    else
                        colDatas[col + 1] = (compDatas.ElementAt(col).ElementAt(row + 1) - compDatas.ElementAt(col).ElementAt(row));
                }

                sbBlock.AppendLine(string.Join(", ", string.Join(", ", colDatas)));
            }
            System.IO.File.WriteAllText(fileName2, sbBlock.ToString());
        }

        private async void buttonBatch_Click(object sender, EventArgs e)
        {
            this.BatchCTS = new CancellationTokenSource();

            string skipPath = this.Options.PATH_DEBUG_FULL;
            SearchOption searchOption = this.Options.SEARCH_INCLUDE_SUBFOLDER ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            List<string> fileList = Directory.GetFiles(Options.PATH, Options.SEARCH_PATTERN, searchOption).ToList();
            fileList.RemoveAll(f => f.Contains(skipPath));
            string srcImageFile = fileList.FirstOrDefault();
            string[] dstImageFile = fileList.ToArray();

            if (string.IsNullOrEmpty(srcImageFile) || !File.Exists(srcImageFile))
                return;

            if (dstImageFile == null || dstImageFile.Length == 0)
                return;

            try
            {
                await LoadImage(srcImageFile, this.srcImageControl, this.BatchCTS);
                await Analyze(this.srcImageControl.Key, this.srcImageControl.ImageD, this.BatchCTS);
                //await SaveTrainResult();
                //await SaveTrainDebug();

                await BatchRun(dstImageFile.Select(f => Path.Combine(Options.PATH, f)).ToArray(), this.BatchCTS);
                MessageBox.Show(this, "Done", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            { }
        }

        private async Task BatchRun(string[] selectes, CancellationTokenSource cts)
        {
            if (!this.Model.RCITrainResult.IsValid)
                throw new Exception("Model is not Trained");

            Size size = this.Model.RCITrainResult.ImageD.Size;
            using (ProcessBufferSetG3 bufferSetG3 = new ProcessBufferSetG3(this.Model, 1, false, size.Width, (int)(size.Height * 1.1)))
            {
                bufferSetG3.BuildBuffers(false);

                Calibration calibration = new ScaledCalibration(this.Options.RCIStandaloneOptions.PelSize);

                for (int i = 0; i < selectes.Length; i++)
                {
                    if (cts.IsCancellationRequested)
                        return;

                    string f = selectes[i];
                    await LoadImage(f, this.dstImageControl, cts);

                    if (cts.IsCancellationRequested)
                        return;

                    Image2D imageD = (Image2D)this.dstImageControl.ImageD.Clone();
                    string key = this.dstImageControl.Key;
                    using (AlgoImage algoImage = ImageBuilder.Build(Program.ImagingLibrary, imageD, ImageType.Grey))
                    {
                        bufferSetG3.Upload(algoImage, null);
                        DebugContext debugContext = new DebugContext(this.Options.SaveDebugImage, this.Options.PATH_DEBUG);
                        SheetInspectParam sheetInspectParam = new SheetInspectParam(this.Model, bufferSetG3, calibration, new DebugContextG(debugContext));

                        await Revision1(sheetInspectParam, key, cts);
                        if (!cts.IsCancellationRequested)
                        {
                            string basePath = this.Options.PATH.TrimEnd('\\');
                            string baseDebugPath = this.Options.PATH_DEBUG_FULL.TrimEnd('\\');
                            string debugPath = f.Replace(basePath, baseDebugPath);

                            await SaveResultDebug(debugPath);

                            string defectPath = Path.GetDirectoryName(debugPath);
                            defectPath = Path.Combine(defectPath, "Defects");
                            System.IO.Directory.CreateDirectory(defectPath);
                            FileHelper.ClearFolder(defectPath, $"{key}*.*", false);
                            Array.ForEach(this.SimpleBlobResult.DefectBlobs, g => g.Save(defectPath, key));
                        }
                    }
                }
            }
        }

        private async void buttonTrainSave_Click(object sender, EventArgs e)
        {
            await SaveTrainResult();
        }

        private async Task SaveTrainResult()
        {
            Program.BeginTimeCheck("SaveTrainResult");
            await ShowProgressForm("Save", new Action(() =>
            {
                System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();

                System.Xml.XmlElement xmlElement = xmlDocument.CreateElement("", "ROOT", "");
                xmlDocument.AppendChild(xmlElement);

                this.Model.RCITrainResult.Save(xmlElement, "TrainResult");
                XmlHelper.Save(xmlDocument, @"C:\temp\Model\Model.xml");

                this.Model.RCITrainResult.SaveImages(@"C:\temp\Model");
            }), null);
            Program.EndTimeCheck("SaveTrainResult");
        }

        private async void buttonTrainLoad_Click(object sender, EventArgs e)
        {
            await ShowProgressForm("Load", new Action(() =>
            {
                string path = @"C:\temp\Model";
                string xmlFile = Path.Combine(path, "Model.xml");
                if (File.Exists(xmlFile))
                {
                    System.Xml.XmlDocument xmlDocument = XmlHelper.Load(xmlFile);
                    System.Xml.XmlElement xmlElement = xmlDocument["ROOT"];

                    this.Model.RCITrainResult = RCITrainResult.Load(xmlElement, "TrainResult");

                    this.Model.RCITrainResult.LoadImages(path);
                }
                UpdateTeachFigure();
            }), null);
        }

        private void UpdateTeachFigure()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateTeachFigure));
                return;
            }

            this.srcImageControl.CanvasPanel.BackgroundFigures.Clear();
            this.srcImageControl.CanvasPanel.BackgroundFigures.AddFigure(this.Model.RCITrainResult.GetPatternFigure());
            this.srcImageControl.CanvasPanel.BackgroundFigures.AddFigure(this.Model.RCITrainResult.GetRegionFigure());
            this.srcImageControl.CanvasPanel.BackgroundFigures.Scale(1f / this.srcImageControl.ImageScale);
            this.srcImageControl.CanvasPanel.Invalidate(false);
        }

        private void UpdateResultFigure()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateResultFigure));
                return;
            }

            FigureGroup figureGroup = this.SimpleBlobResult.GetFigure();

            this.dstImageControl.CanvasPanel.WorkingFigures.Clear();
            this.dstImageControl.CanvasPanel.WorkingFigures.AddFigure(figureGroup.FigureList.ToArray());
            this.dstImageControl.CanvasPanel.WorkingFigures.Scale(1f / this.dstImageControl.ImageScale);
            this.dstImageControl.CanvasPanel.Invalidate(false);

            Bitmap[] bitmaps = this.SimpleBlobResult.DefectBlobs.Select(f => f.GrayImage.ToBitmap()).ToArray();
            int imageSize = 64;
            this.imageList1.ImageSize = new Size(imageSize, imageSize);
            this.imageList1.Images.Clear();
            this.imageList1.Images.AddRange(bitmaps);


            ListViewItem[] items = bitmaps.Select((f, i) => new ListViewItem(i.ToString(), i)).ToArray();
            this.listView1.Items.Clear();
            this.listView1.Items.AddRange(items);
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.View = View.LargeIcon;
        }

        private async Task ShowProgressForm(string caption, Action action, CancellationTokenSource cts = null)
        {
            this.toolStripStatusLabel1.Text = caption;

            this.toolStripProgressBar1.Value = 0;
            try
            {
                if (cts == null)
                    await Task.Run(action);
                else
                    await Task.Run(action, cts.Token);
            }
            catch (Exception ex)
            {
                this.toolStripStatusLabel2.Text = $"{ex.GetType().Name}::{ex.Message}";
            }
            finally
            {
                this.toolStripStatusLabel2.Text = "";
                this.toolStripProgressBar1.Value = this.toolStripProgressBar1.Maximum;
            }

            return;

            //SimpleProgressForm simpleProgressForm = new SimpleProgressForm(caption);
            //simpleProgressForm.Show(this, action, cts);
            //if (simpleProgressForm.Exception != null)
            //{
            //    Exception ex = simpleProgressForm.Exception;
            //    do
            //    {
            //        ConsoleEx.WriteLine(ex.StackTrace?.ToString());
            //        MessageForm.Show(this, $"[{ex.GetType()}]: {ex.Message}");
            //        ex = ex.InnerException;
            //    } while (ex != null);

            //    if (cts != null)
            //        cts.Cancel();
            //}
        }

        private delegate void ProgressDoneDelegate(Task t);
        private void ProgressDone(Task t)
        {
            if(this.InvokeRequired)
            {
                Invoke(new ProgressDoneDelegate(ProgressDone), t);
                return;
            }

            this.toolStripProgressBar1.Value = this.toolStripProgressBar1.Maximum;
            this.toolStripStatusLabel2.Text = "";
            if (t.Exception != null)
                this.toolStripStatusLabel2.Text = $"{t.Exception.GetType().Name}::{t.Exception.Message}";
        }

        private async void buttonRev1_Click(object sender, EventArgs e)
        {
            Program.ResetAllTimer();

            if (!this.Model.RCITrainResult.IsValid)
                return;

            Image2D imageD = (Image2D)this.dstImageControl.ImageD.Clone();
            using (ProcessBufferSetG3 bufferSetG3 = new ProcessBufferSetG3(this.Model, 1, false, imageD.Width, imageD.Height))
            {
                bufferSetG3.BuildBuffers(false);

                using (AlgoImage algoImage = ImageBuilder.Build(Program.ImagingLibrary, imageD, ImageType.Grey))
                    bufferSetG3.Upload(algoImage, null);

                Calibration calibration = new ScaledCalibration(this.Options.RCIStandaloneOptions.PelSize);
                DebugContext debugContext = new DebugContext(this.Options.SaveDebugImage, this.Options.PATH_DEBUG);
                SheetInspectParam sheetInspectParam = new SheetInspectParam(this.Model, bufferSetG3, calibration, new DebugContextG(debugContext));

                string key = this.dstImageControl.Key;
                await Revision1(sheetInspectParam, key, null);
                //SaveResult(Path.Combine(Options.TEMP_PATH, key));
            }
        }

        private async Task Revision1(SheetInspectParam sheetInspectParam, string key, CancellationTokenSource cts)
        {
            await ShowProgressForm("Comparing", new Action(() =>
            {
                try
                {
                    Program.BeginTimeCheck("Compare");
                    CalculatorV3 calculatorV3 = new CalculatorV3();
                    CalculatorResultV3 resultV3 = (CalculatorResultV3)calculatorV3.Inspect(sheetInspectParam);
                    Program.EndTimeCheck("Compare");

                    Size pxSize = Size.Round(sheetInspectParam.CameraCalibration.WorldToPixel(this.Options.RCIStandaloneOptions.SensitivitySz));
                    byte threshold = this.Options.RCIOptions.SensitiveOption.High;
                    Program.BeginTimeCheck("Blob");
                    SimpleBlobResult simpleBlobResult = SimpleBlob.Blob(this.Model.RCITrainResult, (ProcessBufferSetG3)sheetInspectParam.ProcessBufferSet, resultV3, threshold, pxSize);
                    Program.EndTimeCheck("Blob", $"Found {simpleBlobResult.DefectBlobs.Length} Defects");

                    this.ResultV3 = resultV3;
                    this.SimpleBlobResult = simpleBlobResult;
                }
                catch (Exception ex)
                {
                    ConsoleEx.WriteLine($"{ex.GetType().Name}:{ex.Message}");
                }
            UpdateResultFigure();
            }), cts);

        }

        public async Task SaveResultDebug(string path)
        {
            Program.BeginTimeCheck("SaveResultDebug");
            await ShowProgressForm("Save", new Action(() =>
            {
                this.ResultV3.SaveForDebug(path);
                this.SimpleBlobResult.SaveForDebug(path);
            }), null);
            Program.EndTimeCheck("SaveResultDebug");
        }

        public async Task SaveTrainDebug()
        {
            Program.BeginTimeCheck("SaveTrainDebug");
            await ShowProgressForm("Save", new Action(() =>
            {
                string key = this.srcImageControl.Key;
                List<Task> taskList = new List<Task>();
                taskList.Add(Task.Run(() =>
                {
                    string filePath = System.IO.Path.Combine(this.Options.PATH_DEBUG_FULL, "Analyze", $"{key}_U.png");
                    ImageD uImageD = this.Model.RCITrainResult.ImageD;
                    uImageD.SaveImage(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }));
                taskList.Add(Task.Run(() =>
                {
                    string filePath = System.IO.Path.Combine(this.Options.PATH_DEBUG_FULL, "Analyze", $"{key}_W.png");
                    ImageD weightImageD = this.Model.RCITrainResult.WeightImageD;
                    weightImageD?.SaveImage(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }));
                taskList.Add(Task.Run(() =>
                {
                    string filePath = System.IO.Path.Combine(this.Options.PATH_DEBUG_FULL, "Analyze", $"{key}_D.png");
                    ImageD debugImageD;
                    //ImageD debugImageD = this.Model.RCITrainResult.DebugImageD;
                    using (AlgoImage algoImage = ImageBuilder.Build(Program.ImagingLibrary, this.srcImageControl.ImageD, ImageType.Grey))
                        debugImageD = RCITrainer.BuildDebugBitmap(algoImage, this.Model.RCITrainResult);
                    debugImageD?.SaveImage(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }));
                taskList.Add(Task.Run(() =>
                {
                    string filePath = System.IO.Path.Combine(this.Options.PATH_DEBUG_FULL, "Analyze", $"{key}_R.png");
                    ImageD reconImageD = this.Model.RCITrainResult.ReconImageD;
                    reconImageD?.SaveImage(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }));

                taskList.ForEach(f => f.Wait());
            }), null);
            Program.EndTimeCheck("SaveTrainDebug");
        }

        private void buttonConsole_Click(object sender, EventArgs e)
        {
            if (ConsoleEx.IsAlloced)
                ConsoleEx.Free();
            else
                ConsoleEx.Alloc();
        }

        private async void buttonLoad_DragDrop(object sender, DragEventArgs e)
        {
            string[] datas = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (datas == null || datas.Length == 0)
                return;

            ImageControl imageCountrl = (ImageControl)(((Button)sender).Tag);
            await LoadImage(datas[0], imageCountrl, new CancellationTokenSource());
            //string key = Path.GetFileNameWithoutExtension(datas[0]);
            //Analyze(key, this.srcImageControl.ImageD);
        }

        private void buttonLoad_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private async void buttonResultDebugSave_Click(object sender, EventArgs e)
        {
            string key = this.dstImageControl.Key;
            string path = Path.Combine(this.Options.PATH_DEBUG_FULL, key);
            await SaveResultDebug(path);
            Process.Start(path);
        }

        private async void buttonTrainDebugSave_Click(object sender, EventArgs e)
        {
            await SaveTrainDebug();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            this.BatchCTS?.Cancel();
        }
    }
}
