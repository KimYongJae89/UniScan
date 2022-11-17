using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using DynMvp.Base;
using DynMvp.UI;
using System.Threading.Tasks;
using System.IO;
using UniScanG.Data;
using DynMvp.UI.Touch;
using DynMvp.Data;
using System.Diagnostics;

namespace UniScanG.UI.Report
{
    public partial class ReportProgressForm : Form
    {
        Task loadTask;

        bool visibleOnShow = true;
        private string messageText;
        public string MessageText
        {
            set { messageText = value; }
        }


        SortedList<int, MergeSheetResult> sortedList;

        public int MaxCount { get; set; }

        bool isCancle = false;
        bool searchDone = false;

        public ReportProgressForm(string message, bool visibleOnShow)
        {
            InitializeComponent();
            messageText = message;
            this.visibleOnShow = visibleOnShow;

#if DEBUG
            this.ControlBox = true;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
#endif

            if (messageText != null)
                this.labelMessage.Text = StringManager.GetString(this.GetType().FullName, message);
        }

        public void SetLabelMessage(string message)
        {
            this.labelMessage.Text = StringManager.GetString(this.GetType().FullName, message);
        }

        public bool Show(IWin32Window parent, ref string resultPath, List<MergeSheetResult> mergeSheetResultList)
        {
            if (Directory.Exists(UniEye.Base.Settings.PathSettings.Instance().Result) == false)
                return false;

            string actualPath = GetActualPath(resultPath);
            if (string.IsNullOrEmpty(actualPath) || Directory.Exists(actualPath) == false)
            {
                MessageForm.Show(null, StringManager.GetString("Can not found Result Path"));
                return false;
            }
            resultPath = actualPath;

            this.sortedList = new SortedList<int, MergeSheetResult>(this.MaxCount);

            DirectoryInfo directoryInfo = new DirectoryInfo(actualPath);
            FileInfo cacheFileInfos = new FileInfo(Path.Combine(actualPath, Data.Inspect.DataExporterG.CacheFileName));

            DialogResult dialogResult = DialogResult.Abort;
            if (cacheFileInfos.Exists)
            // 캐시파일 있음
            {
                SetLabelMessage("Result Loading From Cache");
                dialogResult = WithCache(parent, cacheFileInfos);
                if (dialogResult == DialogResult.Abort)
                    cacheFileInfos.Delete();
            }
            
            if(dialogResult == DialogResult.Abort)
            // 캐시파일 없음
            {
                SetLabelMessage("Result Loading From Store");
                dialogResult = WithoutCache(parent, directoryInfo);
                if (dialogResult == DialogResult.OK)
                    BuildCache(cacheFileInfos);
            }

            mergeSheetResultList.AddRange(this.sortedList.Values.ToList());
            return dialogResult == DialogResult.OK;
        }

        private DialogResult WithoutCache(IWin32Window parent, DirectoryInfo directoryInfo)
        {
            this.DialogResult = DialogResult.None;
            this.isCancle = false;
            this.searchDone = false;
            this.sortedList.Clear();

            DirectoryInfo[] subDirectoryInfoList = directoryInfo.GetDirectories();
            Array.Sort(subDirectoryInfoList, (f, g) =>
            {
                int a, b;
                bool aa = int.TryParse(f.Name, out a);
                bool bb = int.TryParse(g.Name, out b);
                if (aa && bb)
                    return a.CompareTo(b);
                else
                    return f.Name.CompareTo(g.Name);
            });

            this.loadTask = Task.Run(() =>
            {
                try
                {
                    Search(subDirectoryInfoList);
                    searchDone = true;
                }
                catch
                {
                    isCancle = true;
                }
            });

            DialogResult dialogResult = base.ShowDialog(parent);
            return dialogResult;
        }

        private DialogResult WithCache(IWin32Window parent, FileInfo cacheFileInfos)
        {
            this.DialogResult = DialogResult.None;
            this.isCancle = false;
            this.searchDone = false;
            this.sortedList.Clear();

            Stopwatch sw = new Stopwatch();
            this.loadTask = Task.Run(() =>
            {
                sw.Start();
                try
                {
                    MergeSheetResult mergeSheetResult = null;
                    List<string> lineList = new List<string>();

                    Stopwatch swLoad = new Stopwatch();
                    using (FileStream fs = new FileStream(cacheFileInfos.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            LogHelper.Debug(LoggerType.Operation, $"ReportProgressForm::Show - Load Start.", true);
                            swLoad.Start();
                            while (!sr.EndOfStream)
                                lineList.Add(sr.ReadLine());
                            swLoad.Stop();
                            LogHelper.Debug(LoggerType.Operation, $"ReportProgressForm::Show - Load Done. {swLoad.ElapsedMilliseconds}[ms]", true);
                        }
                    }

                    lineList.RemoveAll(f => string.IsNullOrEmpty(f));

                    LogHelper.Debug(LoggerType.Operation, $"ReportProgressForm::Show - Parse Start.", true);
                    Stopwatch swParse = new Stopwatch();

                    swParse.Start();
                    if (true)
                    // Parall
                    {
                        int src = 0;
                        int last = lineList.Count;
                        List<Task> taskList = new List<Task>();
                        while (src < last)
                        {
                            int dst = lineList.FindIndex(src + 1, f => MergeSheetResult.IsHeader(f));
                            if (dst < 0)
                                dst = last;

                            int length = dst - src;
                            if (length > 0)
                            {
                                string[] lines = lineList.GetRange(src, length).ToArray();
                                Task task = ParseLines(cacheFileInfos.DirectoryName, lines);
#if DEBUG
                                task.Wait();
#endif
                                taskList.Add(task);
                            }
                            src = dst;
                        }
                        taskList.ForEach(f => f.Wait());
                    }
                    else
                    // Serial
                    {
                        lineList.ForEach(f =>
                                {
                                    MergeSheetResult temp = new MergeSheetResult(-1, 0, null, false);
                                    if (temp.ImportHeader(f))
                                    {
                                        temp.ResultPath = Path.Combine(cacheFileInfos.DirectoryName, temp.Index.ToString());
                                        mergeSheetResult = temp;

                                        this.sortedList.Add(mergeSheetResult.Index, mergeSheetResult);
                                    }
                                    else
                                    {
                                        mergeSheetResult.ImportLine(f);
                                    }
                                });
                    }
                    swParse.Stop();
                    LogHelper.Debug(LoggerType.Operation, $"ReportProgressForm::Show - Parse Done. {swParse.ElapsedMilliseconds}[ms]", true);

                    searchDone = true;
                }
                catch (Exception ex)
                {
                    isCancle = true;
                    LogHelper.Error(LoggerType.Error, ex);
                }
                finally
                {
                    sw.Stop();
                }
            });

            DialogResult dialogResult = base.ShowDialog(parent);
            LogHelper.Debug(LoggerType.Operation, $"ReportProgressForm::WithCache - Count: {this.sortedList.Count}[EA], Result: {dialogResult}, Elapsed: {sw.ElapsedMilliseconds}[ms]");
            return dialogResult;
        }

        private Task ParseLines(string baseResultPath, string[] lines)
        {
            return Task.Run(() =>
            {
                MergeSheetResult result = new MergeSheetResult(-1, 0, null, false);
                result.ImportHeader(lines[0]);
                result.ResultPath = Path.Combine(baseResultPath, result.Index.ToString());

                List<IEnumerable<float>> list = new List<IEnumerable<float>>();
                for (int i = 1; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("PartialProjection"))
                    {
                        string[] tokens = lines[i].Split(',');
                        switch (tokens.Length)
                        {
                            case 1:
                                list.Add(new float[0]);
                                break;
                            case 2:
                                list.Add(tokens[1].Split(';').Select(f => float.Parse(f)));
                                break;
                            default:
                                IEnumerable<string> e = tokens.Skip(1);
                                list.Add(e.Select(f => float.Parse(f)));
                                break;
                        }
                    }
                    else
                    {
                        result.ImportLine(lines[i]);
                    }
                }
                result.PartialProjection = list.Select(f => f.ToArray()).ToArray();
                //result.IsImported = true;

                lock (this.sortedList)
                    this.sortedList.Add(result.Index, result);
            });
        }

        private void BuildCache(FileInfo cacheFileInfos)
        {
            List<MergeSheetResult> list = this.sortedList.Values.ToList();

            using (StreamWriter sw = cacheFileInfos.CreateText())
            {
                list.ForEach(f => sw.WriteLine(f.GetExportString()));
            }
        }

        private string GetActualPath(string resultPath)
        {
            return DataCopier.GetCopiedPath(resultPath);
            string actualPath = resultPath;

            if (File.Exists(Path.Combine(resultPath, DataCopier.FlagFileName)))
            {
                actualPath = File.ReadAllText(Path.Combine(resultPath, DataCopier.FlagFileName));
            }
            else if (Directory.Exists(resultPath) == false)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(resultPath);
                DriveInfo[] driveInfos = DataCopier.BackupDriveInfos;
                foreach (DriveInfo driveInfo in driveInfos)
                {
                    string ss = driveInfo.Name;
                    string resultPath2 = ss + resultPath.Substring(ss.Length);
                    if (Directory.Exists(resultPath2))
                        return resultPath2;
                }
            }
            return actualPath;
        }

        private void Search(DirectoryInfo[] directoryInfoList)
        {
            Stopwatch sw = new Stopwatch();
            try
            {
                sw.Start();
                int total = directoryInfoList.Length;
                int element = 100;
                int group = (int)Math.Ceiling(total * 1.0 / element);
                for (int i = 0; i < group; i++)
                //foreach (DirectoryInfo subDirectory in directoryInfoList)
                {
                    if (isCancle == true)
                        return;

                    int src = i * element;
                    int dst = Math.Min(total, (i + 1) * element);
                    ParallelOptions parallelOptions = new ParallelOptions();
#if DEBUG
                    parallelOptions.MaxDegreeOfParallelism = 1;
#endif
                    Parallel.For(src, dst, parallelOptions, j =>
                    {
                        DirectoryInfo subDirectory = directoryInfoList[j];

                        bool parseResult = false;
                        int index = -1;
                        parseResult = int.TryParse(subDirectory.Name, out index);

                        if (parseResult == false)
                        {
                            LogHelper.Debug(LoggerType.Operation, string.Format("ReportProgressForm::Search - index parse fail. dirName: {0}", subDirectory.Name));
                            return;
                        }

                        MergeSheetResult mergeSheetResult = new MergeSheetResult(index, 0, subDirectory.FullName);
                        //if (mergeSheetResult.IsImported)
                        {
                            lock (this.sortedList)
                                this.sortedList.Add(index, mergeSheetResult);
                        }
                    });
                }
            }
            finally
            {
                sw.Stop();
                //Debug.WriteLine("Timer: {0}[ms]", sw.ElapsedMilliseconds);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            isCancle = true;
        }

        private void taskCheckTimer_Tick(object sender, EventArgs e)
        {
            if (isCancle)
            {
                loadTask?.Wait();
                this.DialogResult = DialogResult.Abort;
            }
            else if (searchDone == true)
            {
                loadTask?.Wait();
                this.DialogResult = DialogResult.OK;
            }
            else if (MaxCount != 0)
            {
                UiHelper.SetProgressBarValue(this.progressBar, this.sortedList.Count);
                UiHelper.SetControlText(this.labelRatio, string.Format("{0} / {1}", this.sortedList.Count, this.MaxCount));
            }
            else
            {
                UiHelper.SetProgressBarValue(this.progressBar, this.sortedList.Count);
                UiHelper.SetControlText(this.labelRatio, this.sortedList.Count.ToString());
            }
        }

        private void ReportProgressForm_Load(object sender, EventArgs e)
        {
            UiHelper.SetProgressBarMinMax(this.progressBar, 0, this.MaxCount);
            taskCheckTimer.Start();
            if (!this.visibleOnShow)
                this.WindowState = FormWindowState.Minimized;
        }

        private void ReportProgressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            taskCheckTimer.Stop();
        }
    }
}
