using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using UniEye.Base.Settings;
using UniScanG.Module.Observer.Data;
using UniScanG.Module.Observer.Panel;
using UniScanG.Module.Observer.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.Extender;

namespace UniScanG.Module.Observer
{
    public partial class MainForm : Form, IMultiLanguageSupport
    {
        Dictionary<int, List<string>> pathList = new Dictionary<int, List<string>>();
        Task task = null;
        bool IsProcRunning => task != null && task.IsCompleted == false;

        ImagePanel[] imagePanelChip = new ImagePanel[9];
        ImagePanel[] imagePanelIndex = new ImagePanel[6];
        ImagePanel[] imagePanelFP = new ImagePanel[6];

        Result lastResult = null;
        DataRemover dataRemover;

        string CHIP_NAME_FORMAT => "Chip-{0}";
        string CHIP_NAME_FORMATL => StringManager.GetString(this.GetType().FullName, CHIP_NAME_FORMAT);

        string ALIGN_NAME_FORMAT => "Align-{0}";
        string ALIGN_NAME_FORMATL => StringManager.GetString(this.GetType().FullName, ALIGN_NAME_FORMAT);

        string FP_NAME_FORMAT => "FP-{0}";
        string FP_NAME_FORMATL => StringManager.GetString(this.GetType().FullName, FP_NAME_FORMAT);

        public MainForm()
        {
            InitializeComponent();

            if (true)
            {
                this.tableLayoutPanel.ColumnStyles[0].SizeType = SizeType.Absolute;
                this.tableLayoutPanel.ColumnStyles[0].Width = 0;

                this.tableLayoutPanel.ColumnStyles[1].SizeType = SizeType.Percent;
                this.tableLayoutPanel.ColumnStyles[1].Width = 50;

                this.tableLayoutPanel.ColumnStyles[2].SizeType = SizeType.Percent;
                this.tableLayoutPanel.ColumnStyles[2].Width = 50;

                this.tableLayoutPanel.ColumnStyles[3].SizeType = SizeType.Absolute;
                this.tableLayoutPanel.ColumnStyles[3].Width = 0;
            }

            for (int i = 0; i < imagePanelChip.Length; i++)
            {
                this.imagePanelChip[i] = new UniScanG.Module.Observer.Panel.ImagePanel();
                this.imagePanelChip[i].UpdateText(string.Format(CHIP_NAME_FORMATL, i + 1));
                tableLayoutPanelChip.Controls.Add(this.imagePanelChip[i], i / 3, i % 3);
            }

            for (int i = 0; i < imagePanelIndex.Length; i++)
            {
                this.imagePanelIndex[i] = new UniScanG.Module.Observer.Panel.ImagePanel();
                this.imagePanelIndex[i].UpdateText(string.Format(ALIGN_NAME_FORMATL, i + 1));
                tableLayoutPanelIndex.Controls.Add(this.imagePanelIndex[i], i / 3, i % 3);
            }

            for (int i = 0; i < imagePanelFP.Length; i++)
            {
                this.imagePanelFP[i] = new UniScanG.Module.Observer.Panel.ImagePanel();
                this.imagePanelFP[i].UpdateText(string.Format(FP_NAME_FORMATL, i + 1));
                tableLayoutPanelFP.Controls.Add(this.imagePanelFP[i], i / 3, i % 3);
            }

            LoadModuleList();

            this.dataRemover = new DataRemover();
            this.dataRemover.Start();
        }

        private void LoadModuleList()
        {
            try
            {
                XmlDocument xmlDocument = XmlHelper.Load(Path.Combine(PathSettings.Instance().Config, "ModuleList.xml"));
                XmlElement xmlHeadElement = xmlDocument["ModuleList"];

                XmlNodeList xmlNodeList = xmlHeadElement.GetElementsByTagName("Module");
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    XmlElement xmlElement = (XmlElement)xmlNodeList[i];

                    int group = XmlHelper.GetValue(xmlElement, "Group", -1);
                    string path = XmlHelper.GetValue(xmlElement, "Path", "");

                    if (group >= 0 && !string.IsNullOrEmpty(path))
                    {
                        if (!this.pathList.ContainsKey(group))
                            this.pathList.Add(group, new List<string>());
                        this.pathList[group].Add(path);
                    }
                }

                if (this.pathList.Keys.Count == 0)
                    throw new Exception("PathList is Empty");
            }
            catch
            {
                this.pathList.Clear();
                List<string> group0 = new List<string>();
                group0.Add(@"\\192.168.50.10\UniScan\Gravure_Inspector\Result\Monitoring");
                group0.Add(@"\\192.168.50.20\UniScan\Gravure_Inspector\Result\Monitoring");
                this.pathList.Add(0, group0);

                List<string> group1 = new List<string>();
                group1.Add(@"\\192.168.50.11\UniScan\Gravure_Inspector\Result\Monitoring");
                group1.Add(@"\\192.168.50.21\UniScan\Gravure_Inspector\Result\Monitoring");
                this.pathList.Add(1, group1);

                SaveModuleList();
            }
        }

        private void SaveModuleList()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlHeadElement = (XmlElement)xmlDocument.AppendChild(xmlDocument.CreateElement("ModuleList"));

            foreach (KeyValuePair<int, List<string>> pair in this.pathList)
            {
                pair.Value.ForEach(f =>
                {
                    XmlElement xmlElement = (XmlElement)xmlHeadElement.AppendChild(xmlDocument.CreateElement("Module"));
                    XmlHelper.SetValue(xmlElement, "Group", pair.Key);
                    XmlHelper.SetValue(xmlElement, "Path", f);
                });
            }

            xmlDocument.Save(Path.Combine(PathSettings.Instance().Config, "ModuleList.xml"));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (IsProcRunning)
                return;

            task = Task.Run(new Action(TaskProc));
        }

        private void TaskProc()
        {
            try
            {
                int group = IsWriteDone();
                if (group >= 0)
                {
                    Clear();
                    LoadAndUpdate(group);
                    SaveLastResult();
                    ClearWriteDone(group);
                    UpdateLastInfo();
                }
            }
            catch { }
            finally { }
        }

        private void UpdateLastInfo()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateLastInfo));
                return;
            }

            if (this.lastResult != null)
            {
                this.lastUpdateTime.Text = this.lastResult.InspectTime.ToString("yyyy-MM-dd HH:mm:ss");
                this.lastUpdateLot.Text = this.lastResult.Lot;
                this.lastUpdateCount.Text = (this.lastResult.No + 1).ToString();
            }
        }

        private void Clear()
        {
            Array.ForEach(imagePanelFP, f => f.Clear());
            Array.ForEach(imagePanelIndex, f => f.Clear());
            Array.ForEach(imagePanelChip, f => f.Clear());
        }

        private void ClearWriteDone(int groupNo)
        {
            List<string> list = this.pathList[groupNo];
            foreach (string path in list)
            {
                string file = Path.Combine(path, Watcher.LockFileName);
                File.Delete(file);
            }
        }

        private void LoadAndUpdate(int groupNo)
        {
            ClearRow();

            Result result = new Result(this.lastResult);
            
            List<string> list = this.pathList[groupNo];
            
            int chipCnt = 0, fpCnt = 0, indexCnt = 0;
            for (int i = 0; i < list.Count; i++)
            {
                string path = list[i];
                // time, lot, margin
                Tuple<DateTime, string, SizeF> infoTuple = GetInfo(path);
                if(result.Lot != infoTuple.Item2)
                {
                    result.StartTime = DateTime.Now;
                    result.Lot = infoTuple.Item2;
                    result.No = 0;
                }

                string[] files = Directory.GetFiles(path, "*.bmp");
                foreach (string file in files)
                {
                    FileInfo fInfo = new FileInfo(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));
                    if (!fInfo.Exists)
                        continue;

                    // Image_C{0:00}_S{1:000}_L{2:00}.bmp
                    string[] token = Path.GetFileNameWithoutExtension(file).Split('_');
                    if (token.Length != 4)
                        continue;

                    int cam = -1;
                    int index = -1;
                    ExtType type = ExtType.NONE;
                    try
                    {
                        cam = int.Parse(token[1].Substring(1));
                        type = (ExtType)int.Parse(token[2].Substring(1));
                        index = int.Parse(token[3].Substring(1));
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    if (cam < 0 || index < 0 || type == ExtType.NONE)
                        continue;

                    LogHelper.Debug(LoggerType.Operation, $"MainForm::LoadAndUpdate - Load Image: {file}");
                    Image2D image2D = new Image2D(file);

                    string title;
                    bool processed = true;
                    switch (type)
                    {
                        default:
                            processed = false;
                            break;

                        case ExtType.CHIP:
                            if (chipCnt >= imagePanelChip.Length)
                                continue;

                            title = string.Format(CHIP_NAME_FORMATL, chipCnt + 1);
                            if (image2D != null)
                            {
                                result.List.Add(string.Format(CHIP_NAME_FORMAT, chipCnt + 1), image2D.ToBitmap());
                                MarginCheckerResult marginCheckerResult = MarginChecker.Inspect(title, image2D.ToBitmap(), infoTuple.Item3);
                                float marginL = marginCheckerResult.Margin.Left * infoTuple.Item3.Width;
                                float marginT = marginCheckerResult.Margin.Top * infoTuple.Item3.Height;
                                float marginR = marginCheckerResult.Margin.Right * infoTuple.Item3.Width;
                                float marginB = marginCheckerResult.Margin.Bottom * infoTuple.Item3.Height;

                                string resultPath = Path.Combine(PathSettings.Instance().Result, result.GetPath());
                                DataExporter dataExporter = new DataExporter();
                                dataExporter.ExportData(resultPath, "Margins.csv", marginCheckerResult);
                                //imagePanelChip[chipCnt].UpdateResult(marginL, marginT, marginR, marginB);

                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataGridView);
                                row.DefaultCellStyle.Font = new Font("Malgun Gothic", 12);
                                row.Cells[0].Value = title;
                                row.Cells[1].Value = Math.Min(marginL, marginR);
                                row.Cells[2].Value = Math.Min(marginT, marginB);
                                AddRow(row);
                            }
                            imagePanelChip[chipCnt].UpdateImage(image2D.ToBitmap());
                            imagePanelChip[chipCnt].UpdateText(title);
                            chipCnt++;
                            break;

                        case ExtType.FP:
                            if (fpCnt >= imagePanelFP.Length)
                                continue;

                            title = string.Format(FP_NAME_FORMATL, fpCnt + 1); 
                            if (image2D != null)
                                result.List.Add(string.Format(FP_NAME_FORMAT, fpCnt + 1), image2D.ToBitmap());

                            imagePanelFP[fpCnt].UpdateImage(image2D.ToBitmap());
                            imagePanelFP[fpCnt].UpdateText(title);
                            fpCnt++;
                            break;

                        case ExtType.INDEX:
                            if (indexCnt >= imagePanelIndex.Length)
                                continue;

                            title = string.Format(ALIGN_NAME_FORMATL, indexCnt + 1);
                            if (image2D != null)
                                result.List.Add(string.Format(ALIGN_NAME_FORMAT, indexCnt + 1), image2D.ToBitmap());

                            imagePanelIndex[indexCnt].UpdateImage(image2D.ToBitmap());
                            imagePanelIndex[indexCnt].UpdateText(title);
                            indexCnt++;
                            break;
                    }

                    if (processed)
                    {
                        fInfo.Delete();
                        //File.Delete(file);
                    }
                }
            }
            UpdateTitle();
            this.lastResult = result;
        }

        private void SaveLastResult()
        {
            string fullPath = Path.Combine(PathSettings.Instance().Result, this.lastResult.GetPath());

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            foreach (KeyValuePair<string, Bitmap> pair in this.lastResult.List)
            {
                try
                {
                    System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                    switch(Properties.Settings.Default.ImageSaveFormat)
                    {
                        case "PNG":
                            imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                            break;
                        case "JPG":
                            imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                            break;
                    }

                    string ext = imageFormat.ToString();
                    FileInfo fInfo = new FileInfo(Path.Combine(fullPath, $"{this.lastResult.No:000}_{pair.Key}.{ext.ToLower()}"));

                    LogHelper.Debug(LoggerType.Operation, $"MainForm::SaveLastResult - Save Image: {fInfo.FullName}");
                    Bitmap bitmap = (Bitmap)pair.Value.Clone();
                    bitmap.Save(fInfo.FullName, imageFormat);
                }
                catch(Exception ex)
                {
                    LogHelper.Error(LoggerType.Operation, $"MainForm::SaveLastResult - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                }
            }
        }

        private void ClearRow()
        {
            if (dataGridView.InvokeRequired)
            {
                Invoke(new MethodInvoker(ClearRow));
                return;
            }

            dataGridView.Rows.Clear();
        }

        private delegate void AddRowDelegate(DataGridViewRow row);
        private void AddRow(DataGridViewRow row)
        {
            if (dataGridView.InvokeRequired)
            {
                Invoke(new AddRowDelegate(AddRow), row);
                return;
            }

            dataGridView.Rows.Add(row);
        }

        private Tuple<DateTime, string, SizeF> GetInfo(string path)
        {
            DateTime dateTime = DateTime.Now;
            string lot = "";
            SizeF pelSize = SizeF.Empty;

            string file = Path.Combine(path, Watcher.LockFileName);
            FileStream fileStream = new FileStream(file, FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream);
            while (streamReader.EndOfStream == false)
            {
                string line = streamReader.ReadLine();
                string[] tokens = line.Split(',');
                WatcherLockFile e;
                bool ok = Enum.TryParse<WatcherLockFile>(tokens[0], out e);
                if (ok == false)
                    continue;

                try
                {
                    switch (e)
                    {
                        case WatcherLockFile.DateTime:
                            //dateTime = DateTime.ParseExact(tokens[1], "yyyy-MM-dd HH:mm:ss", null);
                            dateTime = DateTime.Parse(tokens[1]);
                            break;
                        case WatcherLockFile.Lot:
                            lot = tokens.Length > 1 ? tokens[1] : "";
                            break;
                        case WatcherLockFile.PelWidth:
                            pelSize.Width = Convert.ToSingle(tokens[1]);
                            break;
                        case WatcherLockFile.PelHeight:
                            pelSize.Height = Convert.ToSingle(tokens[1]);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, $"GetInfo - {path} - {e} - {ex.GetType().Name} - {ex.Message}");
                }
            }
            streamReader.Close();
            fileStream.Close();
            return new Tuple<DateTime, string, SizeF>(dateTime, lot, pelSize);
        }

        private int IsWriteDone()
        {
            Tuple<int, bool>[] isDones = this.pathList.AsParallel().Select((f, i) => new Tuple<int, bool>(i, f.Value.TrueForAll(g => File.Exists(Path.Combine(g, Watcher.LockFileName))))).ToArray();

            Tuple<int, bool> isDone = Array.Find(isDones, f => f.Item2);
            if (isDone == null)
                return -1;
            return isDone.Item1;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitGridView();

            UpdateImageFormatTools();
            //string[] badPath = CheckPath();
            //if (badPath.Length > 0)
            //    MessageForm.Show(null, string.Format("Cannot find path {0}", badPath[0]));

            StringManager.AddListener(this);
            UpdateTitle();
            this.WindowState = FormWindowState.Maximized;
            timer1.Interval = 500;
            timer1.Start();
        }

        private void UpdateImageFormatTools()
        {
            string imageSaveFormat = Properties.Settings.Default.ImageSaveFormat;
            this.bMPToolStripMenuItem.Checked = imageSaveFormat == "BMP";
            this.pNGToolStripMenuItem.Checked = imageSaveFormat == "PNG";
            this.jPGToolStripMenuItem.Checked = imageSaveFormat == "JPG";
        }

        private void InitGridView()
        {
            this.dataGridView.Columns.Clear();

            this.dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "columnTarget", HeaderText = "Target", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            this.dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "columnMarginW", HeaderText = "Width", AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader });
            this.dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "columnMarginH", HeaderText = "Height", AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader });

            for (int i = 1; i < 3; i++)
            {
                DataGridViewColumn column = this.dataGridView.Columns[i];
                column.DefaultCellStyle.Format = "F0";
                column.DefaultCellStyle.Font = new Font("Malgun Gothic", 12);
            }
        }

        private delegate void UpdateTitleDelegate();
        private void UpdateTitle()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateTitleDelegate(UpdateTitle));
                return;
            }

            string title = CustomizeSettings.Instance().ProgramTitle;
            string copyright = CustomizeSettings.Instance().Copyright;
            this.Text = string.Format("{0} @ {1}, Version {2} Build {3}", "Gravure Watcher", "2019 UniEye", VersionHelper.Instance().VersionString, VersionHelper.Instance().BuildString);

            //this.Text = string.Format("{0} - Build@{1} - Update@{2}", "Gravure Monitoring", VersionHelper.GetBuildDateTime().ToString("yyMMdd.HHmm"), DateTime.Now.ToString("yyMMdd.HHmmss"));
        }

        private string[] CheckPath()
        {
            List<string> badPathList = new List<string>();
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() =>
            {
                List<string> stringList = this.pathList.SelectMany(f => f.Value).ToList();
                var va = stringList.GroupBy(f => Directory.Exists(f));

                foreach (IGrouping<bool, string> group in va)
                {
                    if (!group.Key)
                        badPathList = group.ToList();
                }
                //for (int i = 0; i < this.pathList.Length && string.IsNullOrEmpty(badString); i++)
                //{
                //    for (int j = 0; j < pathList[i].Count && string.IsNullOrEmpty(badString); j++)
                //    {
                //        string path = pathList[i][j];
                //        if ( == false)
                //            badString = path;
                //    }
                //}
            });
            return badPathList.ToArray();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void moduleSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            new SimpleProgressForm().Show(() =>
            {
                while (IsProcRunning)
                    Thread.Sleep(500);
            });

            Dictionary<int, List<string>> clone = new Dictionary<int, List<string>>();
            foreach (var pair in this.pathList)
            {
                clone.Add(pair.Key, new List<string>());
                pair.Value.ForEach(f => clone[pair.Key].Add((string)f.Clone()));
            }

            ModuleSettingForm form = new ModuleSettingForm()
            {
                ModulePathList = clone
            };

            if (form.ShowDialog() == DialogResult.OK)
            {
                this.pathList = form.ModulePathList;
                SaveModuleList();
            }

            timer1.Start();
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(PathSettings.Instance().Result);
        }

        private void ImageFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
            Properties.Settings.Default.ImageSaveFormat = toolStripMenuItem.Text;
            Properties.Settings.Default.Save();

            UpdateImageFormatTools();
        }
    }
}
