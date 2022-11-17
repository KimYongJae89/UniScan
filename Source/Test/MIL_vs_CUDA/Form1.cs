using DynMvp.Base;
using DynMvp.Vision;
using DynMvp.Vision.OpenCv;
using MIL_vs_CUDA.Data;
using MIL_vs_CUDA.Processer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using UniScanG.Data.Inspect;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;

namespace MIL_vs_CUDA
{
    public partial class Form1 : Form
    {
        BindingList<LogDataStruct> logDataList = new BindingList<LogDataStruct>();
        BindingList<ProcessOutput> processOutputList = new BindingList<ProcessOutput>();

        Processer.Processer processer = null;

        public bool ModelSelected { get => !string.IsNullOrEmpty(curAlgorithmPoosXmlPath); }
        string curAlgorithmPoosXmlPath = "";
        Image2D image2D = null;
        Task runningTask = null;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        bool onInitialize = false;

        public Form1()
        {
            onInitialize = true;

            InitializeComponent();

            this.repeatCount.Maximum = int.MaxValue;
            this.splitCount.Maximum = int.MaxValue;

            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewCell dataGridViewCell = new DataGridViewTextBoxCell() { Style = new DataGridViewCellStyle() { Format = "F3" } };
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "DateTIme", DataPropertyName = "DateTime", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, CellTemplate = new DataGridViewTextBoxCell() { Style = new DataGridViewCellStyle() { Format = "yyyyMMdd HH:mm:ss.fff" } } });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Message", DataPropertyName = "Message", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Spend[ms]", DataPropertyName = "SpendTimeMs", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, CellTemplate = dataGridViewCell });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "TotalSpend[ms]", DataPropertyName = "TotalSpendTimeMs", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, CellTemplate = dataGridViewCell });
            this.dataGridView1.DataSource = this.logDataList;

            this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "StartDateTime", DataPropertyName = "StartDateTime", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, CellTemplate = new DataGridViewTextBoxCell() { Style = new DataGridViewCellStyle() { Format = "yyyyMMdd HH:mm:ss.fff" } } });
            this.dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Name", DataPropertyName = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            this.dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Build[s]", DataPropertyName = "BuildTimeS", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, CellTemplate = dataGridViewCell });
            this.dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Transfer[s]", DataPropertyName = "TransferTimeS", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, CellTemplate = dataGridViewCell });
            this.dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Calculate[s]", DataPropertyName = "ProcessCalcTimeS", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, CellTemplate = dataGridViewCell });
            this.dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Detect[s]", DataPropertyName = "ProcessDetectTimeS", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, CellTemplate = dataGridViewCell });
            this.dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Process[s]", DataPropertyName = "ProcessTimeS", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, CellTemplate = dataGridViewCell });
            this.dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Save[s]", DataPropertyName = "SaveTimeS", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, CellTemplate = dataGridViewCell });
            this.dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Total[s]", DataPropertyName = "TotalTimeS", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, CellTemplate = dataGridViewCell });
            this.dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Detects[EA]", DataPropertyName = "DetectsCount", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells});
            this.dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Success", DataPropertyName = "IsSuccess", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells});
            this.dataGridView2.DataSource = this.logDataList;
            this.dataGridView2.DataSource = this.processOutputList;

            this.calculatorVersion.DataSource = Enum.GetValues(typeof(ECalculatorVersion));

            onInitialize = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.processer = new Processer.Processer() { OnLogAdded = AddLog };
            this.processer.SaveResultImage = includeSave.Checked;

            UpdateButtonState(false);
            this.calculatorVersion.SelectedItem = AlgorithmSetting.Instance().CalculatorVersion;
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            string fullModelPath = @"D:\UniScan\Gravure_Inspector\Model\440-32BMJE502-GL08SC\1\1\AlgorithmPool.xml";
            bool fullModelExist = File.Exists(fullModelPath);

            DialogResult dialogResult = DialogResult.No;
            StringBuilder sb = new StringBuilder();

            if (fullModelExist)
            {
                sb.AppendLine(string.Format("Yes: {0}", fullModelPath));
                sb.AppendLine(string.Format("No: Open File Dialog"));

                dialogResult = MessageBox.Show(this, sb.ToString(), "Load Default Model?", MessageBoxButtons.YesNo);
            }

            string modelPath = "";
            switch (dialogResult)
            {
                case DialogResult.Yes:
                    modelPath = fullModelPath;
                    break;
                case DialogResult.No:
                    {
                        OpenFileDialog dlg = new OpenFileDialog();
                        dlg.Filter = "XML files(*.xml)|*.xml";
                        if (dlg.ShowDialog() == DialogResult.OK)
                            modelPath = dlg.FileName;
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(modelPath))
                ModelSelect(modelPath);
        }

        private void ModelReselect()
        {
            if (this.ModelSelected)
                ModelSelect(this.curAlgorithmPoosXmlPath);
        }

        private void ModelSelect(string algorithmPoolXmlfileName)
        {
            if (string.IsNullOrEmpty(algorithmPoolXmlfileName))
                return;

            new DynMvp.UI.Touch.SimpleProgressForm().Show(this, () =>
            {
                string xmlPath = Path.GetDirectoryName(algorithmPoolXmlfileName);
                LoadParam(xmlPath);

                string imagePath = Path.Combine(xmlPath, "Image");
                LoadImage(imagePath);

            });

            UpdateButtonState(true);
            UpdateTitle(algorithmPoolXmlfileName);
            this.curAlgorithmPoosXmlPath = algorithmPoolXmlfileName;
        }

        private delegate void UpdateTitleDelegate(string title);
        private void UpdateTitle(string title)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateTitleDelegate(UpdateTitle), title);
                return;
            }
            this.Text = title;
        }

        private delegate void UpdateButtonStateDelegate(bool v);
        private void UpdateButtonState(bool v)
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateButtonStateDelegate(UpdateButtonState), v);
                return;
            }
            this.buttonMilStart.Enabled = v;
            this.buttonEmguStart.Enabled = v;
            this.buttonAllStart.Enabled = v;

            this.buttonMilTestSerial.Enabled = v;
            this.buttonMilTestParallel.Enabled = v;
            this.buttonCudaTestSerial.Enabled = v;
            this.buttonCudaTestParallel.Enabled = v;
        }

        private void LoadParam(string xmlPath)
        {
            string xmlFile = Path.Combine(xmlPath, "AlgorithmPool.xml");
            if (File.Exists(xmlFile) == false)
                return;

            UniScanG.Gravure.AlgorithmArchiver algorithmArchiver = new UniScanG.Gravure.AlgorithmArchiver();
            AlgorithmPool.Instance().Initialize(algorithmArchiver);
            AlgorithmPool.Instance().Load(xmlFile);

            //XmlDocument xmlDocument = new XmlDocument();
            //xmlDocument.Load(xmlFile);
            //foreach (XmlElement xmlElement in xmlDocument.DocumentElement)
            //{
            //    string algorithmType = XmlHelper.GetValue(xmlElement, "AlgorithmType", "");
            //    if (algorithmType == Calculator.TypeName)
            //    {
            //        this.calculatorParam = new CalculatorParam();
            //        this.calculatorParam.LoadParam(xmlElement);
            //    }
            //    else if (algorithmType == Detector.TypeName)
            //    {
            //        this.detectorParam = new DetectorParam();
            //        this.detectorParam.LoadParam(xmlElement);
            //    }
            //}
        }

        private void LoadImage(string imagePath)
        {
            string[] imageFile = Directory.GetFiles(imagePath, "Image_*.bmp");
            if (imageFile.Length > 0)
                this.image2D = new Image2D(imageFile[0]);
        }

        private void buttonLogSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "TXT files(*.txt)|*.txt";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            SaveLog(dlg.FileName);
        }

        private void SaveLog(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(textBox1.Text);
            sb.AppendLine("Time,Name,Build[s],Process[s],Save[s],Total[s]");
            foreach (ProcessOutput processOutput in this.processOutputList)
                sb.AppendLine(processOutput.ToString());
            File.WriteAllText(fileName, sb.ToString());
        }

        private void buttonLogClear_Click(object sender, EventArgs e)
        {
            this.ClearLog();
            this.ClearHistory();
        }

        private void ClearLog()
        {
            this.logDataList.Clear();
        }

        private void ClearHistory()
        {
            this.processOutputList.Clear();
        }

        private delegate void AddLogDelegate(LogItemType logItemType, string message, DateTime dateTime, double spandTime, bool isStartLog);
        private void AddLog(LogItemType logItemType, string message, DateTime dateTime, bool isStartLog)
        {
            AddLog(logItemType, message, dateTime, 0, isStartLog);
        }
        private void AddLog(LogItemType logItemType, string message, DateTime dateTime, double spandTime, bool isStartLog)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AddLogDelegate(AddLog), logItemType, message, dateTime, spandTime, isStartLog);
                return;
            }

            if (this.logDataList.Count == 0)
                isStartLog = true;

            double totalSpandTime = 0;

            if (!isStartLog)
            {
                LogDataStruct firstLogDataStruct = this.logDataList.LastOrDefault(f => f.IsStart);
                LogDataStruct lastLogDataStruct = this.logDataList.LastOrDefault();

                if (spandTime == 0)
                    spandTime = (dateTime - lastLogDataStruct.Datetime).TotalMilliseconds;
                totalSpandTime = (dateTime - firstLogDataStruct.Datetime).TotalMilliseconds;
            }

            lock(this.logDataList)
                this.logDataList.Add(new LogDataStruct(dateTime, message, spandTime, totalSpandTime, isStartLog));
            this.dataGridView1.FirstDisplayedScrollingRowIndex = this.logDataList.Count - 1;
        }

        private delegate void AddHistoryDelegate(Processer.ProcessOutput processOutput);
        private void AddHistory(Processer.ProcessOutput processOutput)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AddHistoryDelegate(AddHistory), processOutput);
                return;
            }

            lock (this.processOutputList)
                processOutputList.Add(processOutput);
            this.dataGridView2.FirstDisplayedScrollingRowIndex = this.processOutputList.Count - 1;
        }


        public delegate void SetProgressBarDelegate(int i, int count);
        private void SetProgressBar(int i, int count)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SetProgressBarDelegate(SetProgressBar), i, count);
                return;
            }

            this.labelProgress.Text = string.Format("{0}/{1}", i, count);
            this.progressBar1.Maximum = count;
            this.progressBar1.Value = Math.Min(i, count);
        }

        private void buttonMilStart_Click(object sender, EventArgs e)
        {
            ClearLog();
            ClearHistory();
            StartMil(multiLayerBuffer.Checked, usePinnedMem.Checked, useGPU.Checked, useParallel.CheckState);
        }

        private void buttonEmguStart_Click(object sender, EventArgs e)
        {
            ClearLog();
            ClearHistory();
            StartEmgu(multiLayerBuffer.Checked, usePinnedMem.Checked, useParallel.CheckState);
        }

        private void buttonAllStart_Click(object sender, EventArgs e)
        {
            ClearLog();
            ClearHistory();

            StartAll(multiLayerBuffer.Checked, useParallel.CheckState);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.cancellationTokenSource.Cancel();
        }

        private bool IsBusy()
        {
            bool isBusy = (this.runningTask != null && this.runningTask.IsCompleted == false);
            bool isCanceled = this.cancellationTokenSource.IsCancellationRequested;
            return isBusy;
        }

        private bool CheckBusy()
        {
            if (IsBusy())
            {
                MessageBox.Show("Process is busy.");
                return false;
            }
            return true;
        }

        private delegate bool InitializeMilDelegate(bool useNonpagedMem, bool useGpu);
        private bool InitializeMil(bool useNonpagedMem, bool useGpu)
        {
            if (InvokeRequired)
            {
                return (bool)Invoke(new InitializeMilDelegate(InitializeMil), useNonpagedMem, useGpu);
            }

            if (!DynMvp.Devices.MatroxHelper.InitApplication(useNonpagedMem, useGpu))
            {
                MessageBox.Show("MIL Application Alloc Fail.");
                return false;
            }

            if (!DynMvp.Devices.MatroxHelper.LicenseExist())
            {
                MessageBox.Show("MIL License is not founded.");
                FinalizeMil();
                return false;
            }

            return true;
        }

        private delegate void ProcessDoneDelegate(string message);
        private void ShowMessageBox(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new ProcessDoneDelegate(ShowMessageBox), message);
                return;
            }
            MessageBox.Show("Done");
        }

        private delegate void FinalizeMilDelegate();
        private void FinalizeMil()
        {
            if (InvokeRequired)
            {
                Invoke(new FinalizeMilDelegate(FinalizeMil));
                return;
            }
            DynMvp.Devices.MatroxHelper.FreeApplication();
        }

        private ProcessBufferSetG Prepare(ProcesserType processerType, bool useMultiLayerBuffer, bool useGPU)
        {
            try
            {
                AlgorithmBuilder.ClearStrategyList();
                AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Detector.TypeName, ImagingLibrary.MatroxMIL, "", ImageType.Grey));
                if (processerType == ProcesserType.MIL)
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CalculatorBase.TypeName, ImagingLibrary.MatroxMIL, "", useGPU ? ImageType.Gpu : ImageType.Grey));
                else
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CalculatorBase.TypeName, ImagingLibrary.OpenCv, "", useGPU ? ImageType.Gpu : ImageType.Grey));

                CalculatorBase calculator = (CalculatorBase)AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName);
                Algorithm detector = (Algorithm)AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName);

                calculator.PrepareInspection();
                detector.PrepareInspection();

                ProcessBufferSetG processBufferSetG = calculator.CreateProcessingBuffer(1, useMultiLayerBuffer, this.image2D.Width, (int)(this.image2D.Height));
                processBufferSetG.BuildBuffers(false);
                return processBufferSetG;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Operation, ex.Message);
                return null;
            }
        }


        private void Clear(ProcessBufferSetG processBufferSetG)
        {
            AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName).ClearInspection();
            AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName).ClearInspection();

            processBufferSetG?.Dispose();
        }

        private void StartMil(bool useMultiLayerBuffer, bool useNonpagedMem, bool useGPU, CheckState useParallel)
        {
            if (CheckBusy() == false)
                return;

            this.cancellationTokenSource = new CancellationTokenSource();
            int loop = (int)repeatCount.Value;
            this.runningTask = Task.Run(() =>
            {
                Process(ProcesserType.MIL, useMultiLayerBuffer, useNonpagedMem, useGPU, useParallel, loop);
                ShowMessageBox("Done");
            });
        }


        private void StartEmgu(bool useMultiLayerBuffer, bool useNonpagedMem, CheckState useParallel)
        {
            if (CheckBusy() == false)
                return;

            if (!DynMvp.Vision.ImageBuilder.OpenCvImageBuilder.IsCudaAvailable())
            {
                MessageBox.Show("CUDA device not founded.");
                return;
            }

            this.cancellationTokenSource = new CancellationTokenSource();
            int loop = (int)repeatCount.Value;
            this.runningTask = Task.Run(() =>
            {
                Process(ProcesserType.EMGU, useMultiLayerBuffer, useNonpagedMem,false, useParallel,loop);
                ShowMessageBox("Done");
            });
        }

        private void StartAll(bool useMultiLayerBuffer, CheckState useParallel)
        {
            if (CheckBusy() == false)
                return;

            if (!DynMvp.Vision.ImageBuilder.OpenCvImageBuilder.IsCudaAvailable())
            {
                MessageBox.Show("CUDA device not founded.");
                return;
            }

            this.cancellationTokenSource = new CancellationTokenSource();
            int loop = (int)repeatCount.Value;
            this.runningTask = Task.Run(() =>
            {
                Process(ProcesserType.MIL, useMultiLayerBuffer, false, false, useParallel, loop);
                Process(ProcesserType.MIL, useMultiLayerBuffer, true, false, useParallel, loop);
                Process(ProcesserType.EMGU, useMultiLayerBuffer, true, false, useParallel, loop);
                ShowMessageBox("Done");
            });
        }

        private void Process(ProcesserType processerType, bool useMultiLayerBuffer, bool useNonpagedMem, bool useGPU, CheckState useParallel, int loop)
        {
            if (InitializeMil(useNonpagedMem, useGPU) == false)
                return;

            string message =string.Format("{0} {1} {2}", processerType,(useNonpagedMem ? "Pinned" : "Paged"), (useMultiLayerBuffer ? "Multi" : "Single"));
            SetProgressBar(0, loop);

            ProcessBufferSetG processBufferSetG = Prepare(processerType, useMultiLayerBuffer, useGPU);
            for (int i = 0; i < loop; i++)
            {
                if (this.cancellationTokenSource.IsCancellationRequested)
                    break;

                processBufferSetG?.Clear();
                Processer.ProcessOutput processOutput = processer.Process(new ProcessInput(image2D, processBufferSetG, message, processerType, useParallel));
                AddHistory(processOutput);
                SetProgressBar(i + 1, loop);
            }

            Clear(processBufferSetG);
            FinalizeMil();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DynMvp.Devices.MatroxHelper.FreeApplication();
        }

        private void buttonMilTestSerial_Click(object sender, EventArgs e)
        {
            ClearLog();
            ClearHistory();
            ProcessingTest(ImagingLibrary.MatroxMIL, ImageType.Grey, false);
        }

        private void buttonMilTestParallel_Click(object sender, EventArgs e)
        {
            ClearLog();
            ClearHistory();
            ProcessingTest(ImagingLibrary.MatroxMIL, ImageType.Grey, true);
            ShowMessageBox("Done");
        }

        private void buttonCudaTestSerial_Click(object sender, EventArgs e)
        {
            ClearLog();
            ClearHistory();
            ProcessingTest(ImagingLibrary.OpenCv, ImageType.Gpu,false);
            ShowMessageBox("Done");
        }

        private void buttonCudaTestParallel_Click(object sender, EventArgs e)
        {
            ClearLog();
            ClearHistory();
            ProcessingTest(ImagingLibrary.OpenCv, ImageType.Gpu, true);
            ShowMessageBox("Done");
        }

        private void buttonTransferTest_Click(object sender, EventArgs e)
        {
            ClearLog();
            ClearHistory();
            //ProcessingTest(ImagingLibrary.MatroxMIL, ImageType.Grey, false);
            //ProcessingTest(ImagingLibrary.MatroxMIL, ImageType.Grey, true);
            //MessageBox.Show("Done");
            SpeedTest();
        }

        private void SpeedTest()
        {
            if (InitializeMil(false,false) == false)
                return;

            Task.Run(() =>
            {
                StringBuilder sb = new StringBuilder();
                int count = (int)splitCount.Value;
                int loop = (int)repeatCount.Value;
                int length = count * loop;
                Image2D[] images = new Image2D[length];
                SetProgressBar(0, length);

                for (int i = 0; i < loop; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        int idx = i * count + j;
                        images[idx] = images[j];
                        if (images[idx] == null)
                            images[idx] = (Image2D)this.image2D.Clone();
                        SetProgressBar(idx, length);
                    }
                }

                TransferTester transferTester = new TransferTester();
                transferTester.SetProgressBar = SetProgressBar;

                //long uploadtestwithallocms = transferTester.UploadTest(true, images);
                //sb.AppendLine(string.Format("upload with alloc: {0}[ms]", uploadtestwithallocms / length));

                long uploadtestwithoutallocms = transferTester.UploadTest(false, images);
                sb.AppendLine(string.Format("upload without alloc: {0}[ms]", uploadtestwithoutallocms / length));

                AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.OpenCv, images[0], ImageType.Gpu);
                long downloadTestMs = transferTester.DownloadTest(algoImage, length);
                sb.AppendLine(string.Format(string.Format("Download: {0}[ms]", downloadTestMs / length)));

                algoImage.Dispose();
                Array.ForEach(images, f => f.Dispose());

                FinalizeMil();
                ShowMessageBox(sb.ToString());
                ShowMessageBox("Done");
            });
        }

        private void ProcessingTest(ImagingLibrary imagingLibrary, ImageType imageType, bool isParallel)
        {
            if (processer.IsBusy)
            {
                ShowMessageBox("Process is busy");
                return;
            }

            if (imagingLibrary == ImagingLibrary.MatroxMIL&& !DynMvp.Devices.MatroxHelper.InitApplication(false,false))
            {
                MessageBox.Show("MIL Application Alloc Fail.");
                return;
            }

            if (!DynMvp.Devices.MatroxHelper.LicenseExist())
            {
                MessageBox.Show("MIL License is not founded.");
                return;
            }

            if (imageType== ImageType.Gpu && ImageBuilder.GetInstance(imagingLibrary).IsCudaAvailable() == false)
            {
                MessageBox.Show(string.Format("The {0} ImageBuilder is not support {1}", imagingLibrary, imageType));
                return;
            }

            int count = (int)this.repeatCount.Value;
            int split = (int)this.splitCount.Value;
            string title = string.Format("{0}/{1}/{2}", imagingLibrary, imageType, isParallel ? "Parallel" : "Serial");

            Task task = Task.Run(() =>
            {
                //ImageD image2D = this.image2D;
                //if (true)
                //{
                //    AlgoImage originalImage = ImageBuilder.Build(ImagingLibrary.OpenCv, this.image2D, ImageType.Grey);
                //    AlgoImage resizeImage = ImageBuilder.Build(ImagingLibrary.OpenCv, ImageType.Grey, new Size(this.image2D.Width / 2, this.image2D.Height / 2));
                //    AlgorithmBuilder.GetImageProcessing(ImagingLibrary.OpenCv).Resize(originalImage, resizeImage);
                //    image2D = resizeImage.ToImageD();
                //    originalImage.Dispose();
                //    resizeImage.Dispose();
                //}

                MIL_vs_CUDA.Processer.ProcessOutput processOutput = new ProcessOutput(string.Format("{0} - {1}", imagingLibrary, imageType));
                List<Rectangle> roiList = new List<Rectangle>();
                double roiW = image2D.Width * 1.0 / split;
                double roiH = image2D.Height * 1.0 / split;
                if (Math.Min(roiW, roiH) < 1)
                    return;

                int srcY = 0;
                for (int y = 0; y < split; y++)
                {
                    int srcX = 0;
                    int dstY = (int)(roiH * (y + 1));
                    for (int x = 0; x < split; x++)
                    {
                        int dstX = (int)(roiW * (x + 1));
                        //roiList.Add(Rectangle.FromLTRB(srcX, srcY, dstX, dstY));
                        roiList.Add(new Rectangle(srcX, srcY, (int)roiW, (int)roiH));
                        srcX = dstX;
                    }
                    srcY = dstY;
                }                
                
                AddLog(LogItemType.Build, string.Format("{0} - Build - Start", title), DateTime.Now, true);
                processOutput.AddLog(LogItemType.Start, "Start", DateTime.Now);
                AlgoImage algoImage = ImageBuilder.Build(imagingLibrary, image2D, imageType);
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);                
                AddLog(LogItemType.Build, string.Format("{0} - Build - End", title), DateTime.Now, false);

                // test 1
                if (true)
                {
                    StringBuilder sb = new StringBuilder();
                    double elipsedTimeMsAcc = 0;
                    AlgoImage[] bufferAlgoImages = new AlgoImage[] { ImageBuilder.BuildSameTypeSize(algoImage)};
                    AlgoImage resultAlgoImages = ImageBuilder.BuildSameTypeSize(algoImage);

                    SetProgressBar(0, count);
                    AddLog(LogItemType.Process_Calc, string.Format("{0} - Test1 - Start", title), DateTime.Now, true);
                    for (int j = 0; j < count; j++)
                    {
                        double elipsedTimeMs = DoTest(algoImage, isParallel, roiList, bufferAlgoImages, resultAlgoImages, TestSubRoutine1);
                        sb.AppendLine(string.Format("SubRoutine Test1 {0:000} End. {1:0.000}", j, elipsedTimeMs));
                        elipsedTimeMsAcc += elipsedTimeMs;
                        SetProgressBar(j + 1, count);
                        AddLog(LogItemType.Process_Calc, string.Format("{0} - Test1 - Sub{1} - End", title, j), DateTime.Now, false);
                    }
                    sb.AppendLine(string.Format("SubRoutine Test1 Total End. {0:0.000}", elipsedTimeMsAcc));

                    AddLog(LogItemType.Process_Calc, string.Format("{0} - Test1 - End", title), DateTime.Now, false);
                    Array.ForEach(bufferAlgoImages, f => f.Dispose());
                    resultAlgoImages.Dispose();

                    Debug.Write(sb.ToString());
                }

                // test 2
                if(true)
                {
                    StringBuilder sb = new StringBuilder();
                    double elipsedTimeMsAcc = 0;
                    AlgoImage[] bufferAlgoImages = new AlgoImage[] { ImageBuilder.BuildSameTypeSize(algoImage), ImageBuilder.BuildSameTypeSize(algoImage) };
                    AlgoImage resultAlgoImages = ImageBuilder.BuildSameTypeSize(algoImage);

                    SetProgressBar(0, count);
                    AddLog(LogItemType.Process_Calc, string.Format("{0} - Test2 - Start", title), DateTime.Now, true);
                    for (int j = 0; j < count; j++)
                    {
                        double elipsedTimeMs = DoTest(algoImage, isParallel, roiList, bufferAlgoImages, resultAlgoImages, TestSubRoutine2);
                        sb.AppendLine(string.Format("SubRoutine Test2 {0:000} End. {1:0.000}", j, elipsedTimeMs));
                        elipsedTimeMsAcc += elipsedTimeMs;
                        SetProgressBar(j + 1, count);
                        AddLog(LogItemType.Process_Calc, string.Format("{0} - Test2 - Sub{1} - End", title, j), DateTime.Now, false);
                    }
                    sb.AppendLine(string.Format("SubRoutine Test2 Total End. {0:0.000}", elipsedTimeMsAcc));

                    AddLog(LogItemType.Process_Calc, string.Format("{0} - Test2 - End", title), DateTime.Now, false);
                    Array.ForEach(bufferAlgoImages, f => f.Dispose());
                    resultAlgoImages.Dispose();

                    //Debug.Write(sb.ToString());
                }

                algoImage.Dispose();

                if(imagingLibrary == ImagingLibrary.MatroxMIL)
                    FinalizeMil();

                ShowMessageBox("Done");
            });


            while (task.Wait(100) == false)
            {
                Application.DoEvents();
            }   
        }

        private delegate double TestSubRoutineDelegate(ImageProcessing imageProcessing, AlgoImage algoImage, Rectangle[] rectangles, AlgoImage[] bufferImage, AlgoImage resultImage);
        private double DoTest(AlgoImage algoImage, bool isParallel, List<Rectangle> rectangleList, AlgoImage[] bufferImage, AlgoImage resultImage, TestSubRoutineDelegate TestSubRoutine)
        {
            double elipsedTimeMsAcc = 0;
            if (isParallel)
            {
                Parallel.For(0, rectangleList.Count - 1, i =>
                {
                    ImageProcessing imageProcessing = ImageProcessing.Create(algoImage.LibraryType);

                    Rectangle[] rectangles = new Rectangle[] { rectangleList[i], rectangleList[i + 1] };
                    double elipsedTimeMs = TestSubRoutine(imageProcessing, algoImage, rectangles, bufferImage, resultImage);
                    elipsedTimeMsAcc += elipsedTimeMs;

                    imageProcessing.WaitStream();
                });
            }
            else
            {
                //ImageProcessing imageProcessing = ImageProcessing.Create(algoImage.LibraryType);
                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage.LibraryType);

                for (int i = 0; i < rectangleList.Count - 1; i++)
                {
                    Rectangle[] rectangles = new Rectangle[] { rectangleList[i], rectangleList[i + 1] };
                    double elipsedTimeMs = TestSubRoutine(imageProcessing, algoImage, rectangles, bufferImage, resultImage);
                    elipsedTimeMsAcc += elipsedTimeMs;
                }

                imageProcessing.WaitStream();
            }

            return elipsedTimeMsAcc;
        }

        private double TestSubRoutine1(ImageProcessing imageProcessing, AlgoImage algoImage, Rectangle[] rectangles, AlgoImage[] bufferImage, AlgoImage resultImage)
        {
            Stopwatch sw = Stopwatch.StartNew();

            AlgoImage[] srcSubImages = new AlgoImage[] { algoImage.GetSubImage(rectangles[0]), algoImage.GetSubImage(rectangles[1]) };
            AlgoImage[] bufSubImages = new AlgoImage[] { bufferImage[0].GetSubImage(rectangles[0]), resultImage.GetSubImage(rectangles[0]) };
            AlgoImage[] dstSubImages = new AlgoImage[] { resultImage.GetSubImage(rectangles[0]) };

            imageProcessing.Subtract(srcSubImages[0], srcSubImages[1], bufSubImages[0]);
            imageProcessing.Subtract(srcSubImages[1], srcSubImages[0], bufSubImages[1]);

            imageProcessing.Add(bufSubImages[0], bufSubImages[1], dstSubImages[0]);

            Array.ForEach(srcSubImages, f => f.Dispose());
            Array.ForEach(bufSubImages, f => f.Dispose());
            Array.ForEach(dstSubImages, f => f.Dispose());

            sw.Stop();
            return sw.Elapsed.TotalMilliseconds;
        }

        private double TestSubRoutine2(ImageProcessing imageProcessing, AlgoImage algoImage, Rectangle[] rectangles, AlgoImage[] bufferImage, AlgoImage resultImage)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            AlgoImage[] srcSubImages = new AlgoImage[] { algoImage.GetSubImage(rectangles[0]), algoImage.GetSubImage(rectangles[1]) };
            AlgoImage[] bufSubImages = new AlgoImage[] { bufferImage[0].GetSubImage(rectangles[0]), bufferImage[1].GetSubImage(rectangles[0]) };
            AlgoImage[] dstSubImages = new AlgoImage[] { resultImage.GetSubImage(rectangles[0]) };

            imageProcessing.Subtract(srcSubImages[0], srcSubImages[1], bufSubImages[0]);
            imageProcessing.Subtract(srcSubImages[1], srcSubImages[0], bufSubImages[1]);

            imageProcessing.Add(bufSubImages[0], bufSubImages[1], dstSubImages[0]);

            Array.ForEach(srcSubImages, f => f.Dispose());
            Array.ForEach(bufSubImages, f => f.Dispose());
            Array.ForEach(dstSubImages, f => f.Dispose());

            sw.Stop();
            return sw.Elapsed.TotalMilliseconds;
        }

        private void GetSubImageTest(ImagingLibrary imagingLibrary, ImageType imageType)
        {
            if (processer.IsBusy)
            {
                MessageBox.Show("Process is busy.");
                return;
            }

            if (!DynMvp.Devices.MatroxHelper.InitApplication(false,false))
            {
                MessageBox.Show("MIL Application Alloc Fail.");
                return;
            }

            if (!DynMvp.Devices.MatroxHelper.LicenseExist())
            {
                MessageBox.Show("MIL License is not founded.");
                return;
            }

            ClearLog();
            ClearHistory();
            int count = (int)this.repeatCount.Value;
            int mopCount = 1;

            Task task = Task.Run(() =>
            {
                List<Rectangle> roiList = new List<Rectangle>();
                double roiW = this.image2D.Width * 1.0 / count;
                double roiH = this.image2D.Height * 1.0 / count;
                int srcY = 0;
                for (int y = 0; y < count; y++)
                {
                    int srcX = 0;
                    int dstY = (int)(roiH * (y + 1));
                    for (int x = 0; x < count; x++)
                    {
                        int dstX = (int)(roiW * (x + 1));
                        //roiList.Add(Rectangle.FromLTRB(srcX, srcY, dstX, dstY));
                        roiList.Add(new Rectangle(srcX, srcY, (int)roiW, (int)roiH));
                        srcX = dstX;
                    }
                    srcY = dstY;
                }

                AddLog(LogItemType.Build, string.Format("{0} - {1} - Build - Start", imagingLibrary, imageType), DateTime.Now, true);
                AlgoImage algoImage = ImageBuilder.Build(imagingLibrary, this.image2D, imageType);
                AlgoImage algoImageTh = ImageBuilder.BuildSameTypeSize(algoImage);
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
                ip.Binarize(algoImage, algoImageTh, true);
                AddLog(LogItemType.Build, string.Format("{0} - {1} - Build - End", imagingLibrary, imageType), DateTime.Now, false);

                // test 1
                {
                    AlgoImage[] testAlgoImages = new AlgoImage[] { ImageBuilder.BuildSameTypeSize(algoImage), ImageBuilder.BuildSameTypeSize(algoImage) };
                    AddLog(LogItemType.Process_Calc, string.Format("{0} - {1} - Test1 - Start", imagingLibrary, imageType), DateTime.Now, true);

                    for (int j = 0; j < 100; j++)
                    {
                        Parallel.For(0, roiList.Count - 1, i =>
                        {
                            Rectangle[] rectangles = new Rectangle[] { roiList[i], roiList[i + 1] };

                            AlgoImage[] srcSubImages = new AlgoImage[] { algoImage.GetSubImage(rectangles[0]), algoImage.GetSubImage(rectangles[1]) };
                            AlgoImage[] bufSubImages = new AlgoImage[] { testAlgoImages[0].GetSubImage(rectangles[0]), testAlgoImages[1].GetSubImage(rectangles[0]) };
                            AlgoImage[] dstSubImages = new AlgoImage[] { testAlgoImages[0].GetSubImage(rectangles[0]) };

                            Array.ForEach(srcSubImages, f => f.Dispose());
                            Array.ForEach(bufSubImages, f => f.Dispose());
                            Array.ForEach(dstSubImages, f => f.Dispose());
                        });
                        SetProgressBar(j + 1, 100);
                    }
                    AddLog(LogItemType.Process_Calc, string.Format("{0} - {1} - Test1 - End", imagingLibrary, imageType), DateTime.Now, false);
                    ip.WaitStream();
                    AddLog(LogItemType.Process_Calc, string.Format("{0} - {1} - Test1 - End", imagingLibrary, imageType), DateTime.Now, false);

                    Array.ForEach(testAlgoImages, f => f.Dispose());
                }

                algoImageTh.Dispose();
                algoImage.Dispose();

                FinalizeMil();
                ShowMessageBox("Done");
            });


            while (task.Wait(100) == false)
            {
                Application.DoEvents();
            }
        }

        private void includeSave_CheckedChanged(object sender, EventArgs e)
        {
            this.processer.SaveResultImage = includeSave.Checked;
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            InspectionResult inspectionResult = processOutputList.LastOrDefault()?.InspectionResult;
            if (inspectionResult != null)
            {
                if (string.IsNullOrEmpty(inspectionResult.ResultPath))
                {
                    inspectionResult.ResultPath = @"C:\Temp\Result";
                    Directory.CreateDirectory(inspectionResult.ResultPath);
                    FileHelper.ClearFolder(inspectionResult.ResultPath);
                }
                DataExporterG inspectorDataExporterG = new DataExporterG();
                inspectorDataExporterG.Export(inspectionResult);
                System.Diagnostics.Process.Start(inspectionResult.ResultPath);
            }
        }

        private void buttonConvertTest_Click(object sender, EventArgs e)
        {
            if (CheckBusy() == false)
                return;

            ClearLog();
            ClearHistory();
            this.cancellationTokenSource = new CancellationTokenSource();
            InitializeMil(false, false);
            this.runningTask = Task.Run(() =>
            {
                int count = (int)this.repeatCount.Value;
                AlgoImage srcImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, this.image2D, ImageType.Grey);

                AddLog(LogItemType.Build, "Start Transfer Test", DateTime.Now, true);

                for (int i = 0; i < count; i++)
                {
                    if (this.cancellationTokenSource.IsCancellationRequested)
                        break;

                    Rectangle imageRect = new Rectangle(Point.Empty, srcImage.Size);
                    imageRect.Inflate(-srcImage.Width / 4, -srcImage.Height / 4);
                    AlgoImage srcSubImage = srcImage.GetSubImage(imageRect);

                    AlgoImage uploaded = DynMvp.Vision.ImageConverter.Convert(srcSubImage, ImagingLibrary.OpenCv, ImageType.Gpu);
                    AddLog(LogItemType.Transfer, "Upload", DateTime.Now, false);

                    Rectangle subImageRect = new Rectangle(Point.Empty, uploaded.Size);
                    subImageRect.Inflate(-subImageRect.Width / 4, -subImageRect.Height / 4);
                    AlgoImage subUploaded = uploaded.GetSubImage(subImageRect);

                    AlgoImage donwload = DynMvp.Vision.ImageConverter.Convert(subUploaded, ImagingLibrary.MatroxMIL, ImageType.Grey);
                    AddLog(LogItemType.Transfer, "Donwload", DateTime.Now, false);

                    if (i == 0)
                    {
                        srcSubImage.Save(@"C:\temp\Upload1.bmp");
                        uploaded.Save(@"C:\temp\Uploaded2.bmp");
                        subUploaded.Save(@"C:\temp\Donwload1.bmp");
                        donwload.Save(@"C:\temp\Donwload2.bmp");
                        AddLog(LogItemType.Save, "Save", DateTime.Now, false);
                    }

                    donwload.Dispose();
                    subUploaded.Dispose();
                    uploaded.Dispose();
                    srcSubImage.Dispose();
                }

                srcImage.Dispose();

                FinalizeMil();

                ShowMessageBox("Done");
            });
        }

        private void buttonChildImageTest_Click(object sender, EventArgs e)
        {
            if (CheckBusy()==false)
                return;

            ClearLog();

            this.cancellationTokenSource = new CancellationTokenSource();

            if(false)
            {
                this.runningTask = Task.Run(() =>
                {
                    Point wRange = new Point(1000, 1000);
                    Point hRange = new Point(1000, 1000);
                    int range = (wRange.Y - wRange.X + 1) * (hRange.Y - hRange.X + 1);
                    int step = 0;
                    int lenght = 1000;

                    this.InitializeMil(false,false);
                    AlgoImage srcImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, this.image2D, ImageType.Grey);
                    this.SetProgressBar(0, range);

                    AddLog(LogItemType.Build, "Image Builded", DateTime.Now, true);

                    Random random = new Random(DateTime.Now.Millisecond);
                    Rectangle[] subRect = new Rectangle[lenght];
                    AlgoImage[] subImage = new AlgoImage[lenght];
                    for (int xAlign = -1; xAlign < 4; xAlign++)
                    {
                        for (int h = hRange.X; h <= hRange.Y; h++)
                        {
                            for (int w = wRange.X; w <= wRange.Y; w++)
                            {
                                Size subRectSize = new Size(w, h);
                                for (int i = 0; i < subRect.Length; i++)
                                {
                                    int l = random.Next(0, srcImage.Width - w);
                                    if (xAlign >= 0)
                                        l = ((l - 3) / 4) * 4 + xAlign;
                                    int t = random.Next(0, srcImage.Height - h);
                                    Rectangle rectangle =
                                    subRect[i] = new Rectangle(new Point(l, t), subRectSize);
                                }

                                List<double> miliSeconds = new List<double>();
                                for (int k = 0; k < 5; k++)
                                {
                                    Stopwatch sw = Stopwatch.StartNew();
                                    for (int j = 0; j < lenght; j++)
                                        subImage[j] = srcImage.GetSubImage(subRect[j]);
                                    sw.Stop();
                                    miliSeconds.Add(sw.Elapsed.TotalMilliseconds);
                                    Array.ForEach(subImage, f => f.Dispose());
                                }

                                string messageTime = string.Join(",", miliSeconds.ConvertAll(f => f.ToString("F3")));
                                string message = string.Format("GetChindImage,{0},{1},{2},{3}", xAlign, subRectSize.Width, subRectSize.Height, messageTime);
                                AddLog(LogItemType.Build, message, DateTime.Now, false);

                                SetProgressBar(++step, range);

                                if (this.cancellationTokenSource.IsCancellationRequested)
                                    break;
                            }
                        }
                    }
                    srcImage.Dispose();
                    AddLog(LogItemType.End, "Image Disposed", DateTime.Now, false);

                    FinalizeMil();
                    ShowMessageBox("Done");
                });
            }
            else
            {
                this.runningTask = Task.Run(() =>
                {
                    bool useNonpagedMem = this.usePinnedMem.Checked;
                    int splitCount = (int)this.splitCount.Value;
                    int repeatCount = (int)this.repeatCount.Value;
                    this.SetProgressBar(0, repeatCount);
                    AddLog(LogItemType.Start, "Start", DateTime.Now, true);

                    this.InitializeMil(useNonpagedMem, false);

                    AlgoImage srcImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, this.image2D, ImageType.Grey);
                    AddLog(LogItemType.Build, "Image Builded", DateTime.Now, false);

                    Rectangle[] subRect = new Rectangle[splitCount];
                    AlgoImage[] subImages = new AlgoImage[splitCount];
                    Random random = new Random((int)DateTime.Now.Ticks);

                    Size subRectSize = new Size(1024, 1024);
                    for (int i = 0; i < subRect.Length; i++)
                    {
                        int l = random.Next(0, srcImage.Width - subRectSize.Width);
                        int t = random.Next(0, srcImage.Height - subRectSize.Height);
                        subRect[i] = new Rectangle(new Point(l, t), subRectSize);
                    }

                    for (int r = 0; r < repeatCount; r++)
                    {
                        Stopwatch sw = Stopwatch.StartNew();

                        for (int j = 0; j < splitCount; j++)
                        {
                            AlgoImage subImage = srcImage.GetSubImage(subRect[j]);
                            subImage.Dispose();
                        }

                        //for (int j = 0; j < splitCount; j++)
                        //    subImages[j] = srcImage.GetSubImage(subRect[j]);

                        //Parallel.For(0, splitCount, j =>
                        //{
                        //    AlgoImage subImage = srcImage.GetSubImage(subRect[j]);
                        //    subImage.Dispose();
                        //});

                        sw.Stop();

                        string message = string.Format("GetChindImage({0}) {1}Times", useNonpagedMem ? "Pinned" : "Paged", splitCount);
                        AddLog(LogItemType.Build, message, DateTime.Now, sw.Elapsed.TotalMilliseconds, false);

                        //Array.ForEach(subImages, f => f?.Dispose());
                        SetProgressBar(r + 1, repeatCount);

                        if (this.cancellationTokenSource.IsCancellationRequested)
                            break;
                    }

                    srcImage.Dispose();
                    AddLog(LogItemType.Dispose, "Image Dispose", DateTime.Now, false);

                    FinalizeMil();
                    ShowMessageBox("Done");
                });
            }
        }

        private void calculatorVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onInitialize)
                return;

            bool same = AlgorithmSetting.Instance().CalculatorVersion.Equals(calculatorVersion.SelectedItem);
            if (!same)
            {
                bool change = (!ModelSelected || MessageBox.Show("Reload Model?", "Message", MessageBoxButtons.YesNo) == DialogResult.Yes);
                if (change)
                {
                    AlgorithmSetting.Instance().CalculatorVersion = (ECalculatorVersion)calculatorVersion.SelectedItem;
                    AlgorithmSetting.Instance().Save();
                    this.ModelReselect();
                }

                calculatorVersion.SelectedItem = AlgorithmSetting.Instance().CalculatorVersion;
            }
        }

        private void calculatorVersion_DataSourceChanged(object sender, EventArgs e)
        {
            this.calculatorVersion.SelectedItem = AlgorithmSetting.Instance().CalculatorVersion;
        }
    }
}
