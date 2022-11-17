using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;

namespace UnieyeLauncher.Operation
{
    [Serializable]
    public class ArchiveSettings : SubSettings
    {
        int maxSize;

        public ArchiveSettings() : base(true) { }

        protected override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.maxSize = Convert.ToInt32(xmlElement["MaxSize"].InnerText);
        }

        protected override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlElement subElementSize = (XmlElement)xmlElement.AppendChild(xmlElement.OwnerDocument.CreateElement("MaxSize"));
            subElementSize.InnerText = maxSize.ToString();
        }
    }

    public struct ArchiveItem
    {
        public DateTime DateTime => dateTime;
        DateTime dateTime;

        public string Version => this.version;
        public string version;

        public string FullPathName => this.fullPathName;
        string fullPathName;

        public string FileName => this.fileName;
        public string fileName;

        public bool ContainBin => this.containBin;
        public bool containBin;

        public bool ContainConfig => this.containConfig;
        public bool containConfig;

        public ArchiveItem(DateTime dateTime, string version, string fullPathName, string fileName, bool containBin, bool containConfig)
        {
            this.dateTime = dateTime;
            this.version = version;
            this.fullPathName = fullPathName;
            this.fileName = fileName;
            this.containBin = containBin;
            this.containConfig = containConfig;
        }

    }

    public class ArchiveOperator : Operator
    {
        ArchiveSettings archiverSetting;
        string archiveDirectory;

        public ArchiveItem[] ArchiveItems { get => this.archiveItemList.ToArray(); }
        List<ArchiveItem> archiveItemList = new List<ArchiveItem>();

        public override bool Use => this.archiverSetting.Use;

        public ArchiveOperator(ArchiveSettings archiverSetting) : base()
        {
            this.archiverSetting = archiverSetting;
            this.archiveDirectory = Path.Combine(this.workingDirectory, "Archive");

            //UpdateArchiveList();
        }

        public void UpdateArchiveList(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.ReportProgress(0, "");

            archiveItemList.Clear();
            if (!Directory.Exists(archiveDirectory))
                return;

            string[] files = Directory.GetFiles(archiveDirectory, "*.zip");
            List<string> errorOpenFiles = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                worker.ReportProgress((int)(i * 100f / files.Length), "");

                string file = files[i];
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Length == 0)
                {
                    fileInfo.Delete();
                    continue;
                }

                DateTime dateTime = fileInfo.LastWriteTime;
                bool containBin = false;
                bool containConfig = false;
                try
                {
                    //Debug.WriteLine("Zip Start");
                    using (ZipArchive zipArchive = ZipFile.OpenRead(file))
                    {
                        foreach (ZipArchiveEntry entry in zipArchive.Entries)
                        {
                            string directoryName = "";
                            string upperDirectory = Path.GetDirectoryName(entry.FullName).ToLower();
                            while (!string.IsNullOrEmpty(upperDirectory))
                            {
                                directoryName = upperDirectory;
                                upperDirectory = Path.GetDirectoryName(directoryName);
                            }
                            //string directoryName = entry.FullName.Substring(0, idx);
                            if (!string.IsNullOrEmpty(directoryName))
                            {
                                containBin |= directoryName == "bin";
                                containConfig |= directoryName == "config";
                            }
                        }
                    }
                    //Debug.WriteLine("Zip End");

                    //Debug.WriteLine("Info Start");
                    string versionBuild = "";
                    try
                    {
                        string infoFile = fileInfo.FullName.Replace(".zip", "");
                        if (File.Exists(infoFile))
                        {
                            using (FileStream fStream = new FileStream(infoFile, FileMode.Open))
                            {
                                byte[] bytes = new byte[fStream.Length];
                                fStream.Read(bytes, 0, bytes.Length);
                                string str = Encoding.Default.GetString(bytes);
                                string[] tokens = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                                if (tokens.Length >= 2)
                                    versionBuild = string.Format("{0} ({1})", tokens[0], tokens[1]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        versionBuild = "";
                    }
                    //Debug.WriteLine("Info End");

                    archiveItemList.Add(new ArchiveItem(dateTime, versionBuild, file, file.Replace(archiveDirectory, ""), containBin, containConfig));
                }
                catch
                {
                    string directory = Path.GetDirectoryName(file);
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string extension = Path.GetExtension(file);
                    string newPath = Path.Combine(directory, string.Format("{0}{1}", fileName, extension.Substring(0, extension.Length - 1)));
                    fileInfo.MoveTo(newPath);
                    errorOpenFiles.Add(file);
                }
            }

            if (errorOpenFiles.Count > 0)
                OnEvent(EventType.Message, string.Format("{0} Item(s) Fail to Read", errorOpenFiles.Count));
        }

        private List<string> GetFileList(string srcPath)
        {
            DirectoryInfo srcDirectoryInfo = new DirectoryInfo(srcPath);

            FileInfo[] fileInfos = srcDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            List<string> fileList = fileInfos.ToList().ConvertAll(f => f.FullName);

            //FileInfo[] fileInfos = srcDirectoryInfo.GetFiles();
            //List<string> fileList = fileInfos.ToList().ConvertAll(f => f.FullName);

            //DirectoryInfo[] directoryInfos = srcDirectoryInfo.GetDirectories();
            //foreach (DirectoryInfo directoryInfo in directoryInfos)
            //{
            //    List<string> subFileList = GetFileList(directoryInfo.FullName);
            //    fileList.AddRange(subFileList);
            //}

            return fileList;
        }


        private string GetZipPath()
        {
            return Path.GetFullPath(Path.Combine(this.workingDirectory, "Archive"));
        }

        private string[] GetSorucePath()
        {
            return new string[]
            {
                Path.GetFullPath(Path.Combine(this.workingDirectory, "Bin")),
                Path.GetFullPath(Path.Combine(this.workingDirectory, "Config"))
            };
        }

        internal void StartArchive(object sender, DoWorkEventArgs e)
        {
            string[] sourcePaths = GetSorucePath();
            string zipPathName = GetZipPath();
            string binFileName = e.Argument as string;
            Directory.CreateDirectory(zipPathName);

            BackgroundWorker worker = sender as BackgroundWorker;
            worker.ReportProgress(0, zipPathName);

            Archave(sourcePaths, zipPathName, binFileName, worker);
            if (worker != null)
                e.Cancel = worker.CancellationPending;
        }

        public void Archave(string[] archivePaths, string archiveFilePath, string binFileName, BackgroundWorker worker)
        {
            if (!this.archiverSetting.Use)
                throw new Exception("Archiver is disabled");

            this.IsActive = true;
            DateTime nowDateTime = DateTime.Now;
            string version = "";
            string build = "";
            List<Tuple<string, string>> tupleList = new List<Tuple<string, string>>();
            Array.ForEach(archivePaths, f =>
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(f);
                if (!directoryInfo.Exists)
                    return;

                string s = Path.GetFileName(f);
                FileInfo[] fileInfos = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo fileInfo in fileInfos)
                {
                    string dirName = Path.Combine(s, fileInfo.FullName.Substring(f.Length + 1));
                    tupleList.Add(new Tuple<string, string>(fileInfo.FullName, dirName));

                    if (Path.GetFileName(fileInfo.Name) == binFileName)
                    {
                        System.Diagnostics.FileVersionInfo fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(fileInfo.FullName);
                        version = fileVersionInfo.ProductVersion;
                        build = ConvertToBuildDate(version);
                    }
                }
            });

            string infoFileName = Path.Combine(archiveFilePath, string.Format("{0}", nowDateTime.ToString("yyyyMMdd-HHmmss")));
            if (!string.IsNullOrEmpty(version) && !string.IsNullOrEmpty(build))
            {
                using (FileStream fileStream = new FileStream(infoFileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("{0}", version));
                    sb.AppendLine(string.Format("{0}", build));
                    string info = sb.ToString();
                    byte[] bytes = Encoding.Default.GetBytes(sb.ToString());
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }

            string zipFileName = Path.Combine(archiveFilePath, string.Format("{0}.zip", nowDateTime.ToString("yyyyMMdd-HHmmss")));
            using (FileStream fileStream = new FileStream(zipFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                {
                    for (int i = 0; i < tupleList.Count; i++)
                    {
                        if (worker == null || !worker.CancellationPending)
                        {
                            Tuple<string, string> tuple = tupleList[i];
                            try
                            {
                                int progress = (int)Math.Max(1, Math.Round((i) * 100f / tupleList.Count));
                                worker?.ReportProgress(progress, Path.GetFileName(tuple.Item2));
                                zipArchive.CreateEntryFromFile(tuple.Item1, tuple.Item2, CompressionLevel.Optimal);
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }

            if (worker != null && worker.CancellationPending)
            {
                File.Delete(zipFileName);
                File.Delete(infoFileName);
            }

            this.IsActive = false;

            //UpdateArchiveList();
        }

        private string ConvertToBuildDate(string versionString)
        {
            if (versionString == "1.0.0.0")
                return "";

            string[] tokens = versionString.Split('.');
            int intDays = Convert.ToInt32(tokens[2]);
            DateTime refDate = new DateTime(2000, 1, 1);
            DateTime dtBuildDate = refDate.AddDays(intDays);

            int intSeconds = Convert.ToInt32(tokens[3]);
            intSeconds = intSeconds * 2;
            dtBuildDate = dtBuildDate.AddSeconds(intSeconds);

            //return dtBuildDate;
            return dtBuildDate.ToString("yyMMdd.HHmm");
        }

        public void StartRestore(object sender, DoWorkEventArgs e)
        {
            string path = "";
            if (e.Argument is ArchiveItem)
                path = ((ArchiveItem)e.Argument).FullPathName;

            BackgroundWorker worker = sender as BackgroundWorker;
            Restore(path, this.workingDirectory, worker);

            if (worker != null)
                e.Cancel = worker.CancellationPending;
        }

        public void Restore(string archiveFilePath, string restorePath, BackgroundWorker worker)
        {
            if (!this.archiverSetting.Use)
                throw new Exception("Archiver is disabled");

            this.IsActive = true;
            int totalPercent = 0;
            try
            {
                using (ZipArchive zipArchive = ZipFile.OpenRead(archiveFilePath))
                {
                    totalPercent = zipArchive.Entries.Count;
                    for (int i = 0; i < zipArchive.Entries.Count; i++)
                    {
                        ZipArchiveEntry zipArchiveEntry = zipArchive.Entries[i];
                        try
                        {
                            if (worker == null || !worker.CancellationPending)
                            {
                                string fullPath = Path.Combine(restorePath, "_", zipArchiveEntry.FullName);
                                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                                zipArchiveEntry.ExtractToFile(fullPath, true);
                                worker?.ReportProgress((int)Math.Round((i + 1) * 100f / totalPercent));
                            }
                        }
                        catch (PathTooLongException)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            DirectoryInfo srcInfo = new DirectoryInfo(Path.Combine(restorePath, "_"));
            DirectoryInfo dstInfo = new DirectoryInfo(Path.Combine(restorePath));
            if (worker == null || !worker.CancellationPending)
            {
                AppHandler.MoveAllDirectories(srcInfo, dstInfo, true);
            }
            Directory.Delete(srcInfo.FullName, true);
            this.IsActive = false;
        }

        //private void MoveDirectories(DirectoryInfo srcInfo, DirectoryInfo dstInfo)
        //{
        //    DirectoryInfo[] srcDirectoryInfo = srcInfo.GetDirectories();
        //    DirectoryInfo[] dstDirectoryInfo = dstInfo.GetDirectories();
        //    Array.ForEach(srcDirectoryInfo, f =>
        //    {
        //        DirectoryInfo directoryInfo = Array.Find(dstDirectoryInfo, g => g.Name == f.Name);
        //        if (directoryInfo != null)
        //        {
        //            Directory.Delete(directoryInfo.FullName, true);
        //            Directory.Move(f.FullName, directoryInfo.FullName);
        //        }
        //        else
        //        {
        //            Directory.Move(f.FullName, Path.Combine(dstInfo.FullName, f.Name));
        //        }
        //    });

        //}

        //private void MoveFiles(DirectoryInfo srcInfo, DirectoryInfo dstInfo)
        //{
        //    FileInfo[] srcFiles = srcInfo.GetFiles();
        //    FileInfo[] dstFiles = dstInfo.GetFiles();
        //    Array.ForEach(srcFiles, f =>
        //    {
        //        FileInfo findInfo = Array.Find(dstFiles, g => g.Name == f.Name);
        //        if (findInfo != null)
        //        {
        //            File.Delete(findInfo.FullName);
        //            File.Move(f.FullName, findInfo.FullName);
        //        }
        //        else
        //        {
        //            File.Move(f.FullName, Path.Combine(dstInfo.FullName, f.Name));
        //        }
        //    });
        //}
    }
}
