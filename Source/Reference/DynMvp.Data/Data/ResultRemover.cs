//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.IO;
//using System.Globalization;

//using DynMvp.Base;

//namespace DynMvp.Data
//{
//    public interface IDataManager
//    {
//        string Message { get; }
//    }

//    public enum DataStoringType
//    {
//        Seq, Day_Seq, Month_Day_Seq, Year_Month_Day_Seq
//    }

//    public class DataRemover : ThreadHandler, IDataManager
//    {
//        DataStoringType dataStoringType;
//        string resultPath;
//        int resultStoringDays;
//        string dateFormat;
//        bool removeParentFolder;

//        string message = "";
//        bool notify = false;
//        public string Message
//        {
//            get
//            {
//                if (notify)
//                {
//                    notify = false;
//                    return message;
//                }
//                else
//                    return "";
//            }
//        }

//        public DataRemover(DataStoringType dataStoringType, string resultPath, int resultStoringDays, string dateFormat, bool removeParentFolder) : base("DataRemover")
//        {
//            ThreadManager.AddThread(this);

//            this.dataStoringType = dataStoringType;
//            this.resultPath = resultPath;
//            this.resultStoringDays = resultStoringDays;
//            this.dateFormat = dateFormat;
//            this.removeParentFolder = removeParentFolder;

//            this.workingThread = new Thread(new ThreadStart(DataRemoveProc));
//            this.workingThread.IsBackground = true;
//            this.requestStop = false;

//            this.Start();
//        }

//        private void DataRemoveProc()
//        {
//            while (RequestStop == false)
//            {
//                if (Directory.Exists(resultPath))
//                {
//                    string[] directoryNames = Directory.GetDirectories(resultPath);
//                    foreach (string dirName in directoryNames)
//                    {
//                        string path = Path.Combine(resultPath, dirName);

//                        switch (dataStoringType)
//                        {
//                            default:
//                            case DataStoringType.Seq:
//                                RemoveData_Seq(path);
//                                break;
//                            case DataStoringType.Day_Seq:
//                                RemoveData_Day(path);
//                                break;
//                            case DataStoringType.Month_Day_Seq:
//                                RemoveData_Month(path);
//                                break;
//                            case DataStoringType.Year_Month_Day_Seq:
//                                RemoveData_Year(path);
//                                break;
//                        }
//                    }
//                }
//#if DEBUG
//                Thread.Sleep(new TimeSpan(0, 0, 1));
//#else
//                Thread.Sleep(new TimeSpan(1, 0, 0));
//#endif
//            }
//        }

//        private bool IsOldData(string path)
//        {
//            if (resultStoringDays < 0)
//                return false;

//            string dirName = Path.GetFileName(path);

//            DateTime folderDate;
//            if (DateTime.TryParseExact(dirName, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out folderDate))
//            {
//                DateTime curTime = DateTime.Now;

//                TimeSpan timeSpan = curTime.Date - folderDate.Date;

//                return (timeSpan.TotalHours >= 24 * resultStoringDays);
//            }

//            return false;
//        }

//        private void RemoveData(string path)
//        {
//            this.message = string.Format("Start Remove: {0}", path);
//            this.notify = true;
//            try
//            {
//                LogHelper.Debug(LoggerType.DataRemover, string.Format("Start Delete / SrcPath: {0}", path));
//                FileHelper.ClearFolder(path);
//                Directory.Delete(path, false);
//                //RemoveData2(path);
//                LogHelper.Debug(LoggerType.DataRemover, string.Format("End Delete / SrcPath: {0}", path));
//            }
//            catch (Exception ex)
//            {
//                LogHelper.Debug(LoggerType.DataRemover, string.Format("Delete Fail: {0} / SrcPath : {1}", ex.Message, path));
//                this.message = string.Format("Remove Error: {0}, {1}", path, ex.Message);
//                this.notify = true;
//            }
//        }

//        private void RemoveData2(string path)
//        {
//            string[] files = Directory.GetFiles(path);
//            Array.ForEach(files, f => File.Delete(f));
//            string[] directories = Directory.GetDirectories(path);
//            Array.ForEach(directories, f => RemoveData2(f));
//            Directory.Delete(path);
//        }

//        private void RemoveData_Seq(string parentPath)
//        {
//            if (Directory.Exists(parentPath) == false)
//                return;

//            string[] directoryNames;
//            if (removeParentFolder == false)
//                directoryNames = Directory.GetDirectories(parentPath);
//            else
//                directoryNames = new string[] { parentPath };

//            foreach (string dirName in directoryNames)
//            {
//                string path = parentPath;

//                string copiedFlag = Path.Combine(path, "Copied");
//                bool isCopied = File.Exists(copiedFlag);
//                if (isCopied)
//                {
//                    string[] subDir = Directory.GetDirectories(path);
//                    Array.ForEach(subDir, f => RemoveData(f));

//                    string copiedPath = File.ReadAllText(copiedFlag);
//                    if (string.IsNullOrEmpty(copiedPath) == false)
//                    {
//                        if (Directory.Exists(copiedPath))
//                        {
//                            if (IsOldData(copiedPath))
//                            {
//                                RemoveData(copiedPath);
//                                RemoveData(path);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        RemoveData(path);
//                    }
//                }
//                else if (IsOldData(path))
//                {
//                    RemoveData(path);
//                }
//            }
//        }



//        private void RemoveData_Day(string parentPath)
//        {
//            if (Directory.Exists(parentPath) == false)
//                return;

//            string[] seqDirectoryNames = Directory.GetDirectories(parentPath);
//            foreach (string seqDirectoryName in seqDirectoryNames)
//            {
//                RemoveData_Seq(seqDirectoryName);
//            }
//        }

//        private void RemoveData_Month(string parentPath)
//        {
//            if (Directory.Exists(parentPath) == false)
//                return;

//            string[] dayDirectoryNames = Directory.GetDirectories(parentPath);
//            foreach (string dayDirectoryName in dayDirectoryNames)
//            {
//                RemoveData_Day(dayDirectoryName);
//            }
//        }

//        private void RemoveData_Year(string parentPath)
//        {
//            if (Directory.Exists(parentPath) == false)
//                return;

//            string[] monthDirectoryNames = Directory.GetDirectories(parentPath);
//            foreach (string monthirectoryName in monthDirectoryNames)
//            {
//                RemoveData_Month(monthirectoryName);
//            }
//        }
//    }

//    public class DataCopier : ThreadHandler, IDataManager
//    {
//        public static string FlagFileName = "Copied";

//        DriveInfo curBackupDrive = null;
//        public DriveInfo CurBackupDrive { get => curBackupDrive; }

//        DataStoringType dataStoringType;
//        string srcPath;
//        string dateFormat;
//        int srcStoringDays;
//        float minFreeSpace;

//        string message = "";
//        bool notify = false;
//        public string Message
//        {
//            get
//            {
//                if (notify)
//                {
//                    notify = false;
//                    return message;
//                }
//                else
//                    return "";
//            }
//        }

//        public static List<DriveInfo> GetTargetDriveInfoList()
//        {
//            return DriveInfo.GetDrives().ToList().FindAll(f => f.VolumeLabel == "BackUp" && f.DriveFormat == "NTFS");
//        }

//        public DataCopier(DataStoringType dataStoringType, string srcPath, int srcStoringDays, string dateFormat, float minFreeSpace) : base("ResultCopier")
//        {
//            this.dataStoringType = dataStoringType;
//            this.srcPath = srcPath;
//            this.dateFormat = dateFormat;
//            this.srcStoringDays = srcStoringDays;
//            this.minFreeSpace = minFreeSpace;

//            this.workingThread = new Thread(new ThreadStart(DataCopyProc));
//            this.workingThread.IsBackground = true;
//            this.requestStop = false;

//            this.Start();
//        }

//        //public string GetActualPath(string resultPath)
//        //{
//        //    // 로컬 드라이브에 존재함. 백업중일 수 있음.
//        //    if (Directory.Exists(resultPath))
//        //    {
//        //        bool copied = File.Exists(Path.Combine(resultPath, "Copied"));
//        //        if (copied == false)    // 파일이 백업 드라이브로 복사되지 않음.
//        //            return resultPath;
//        //    }

//        //    // 로컬 드라이브에 존재하지 않음. 백업 후 삭제되었음.
//        //    DirectoryInfo virtualDirInfo = new DirectoryInfo(resultPath);
//        //    foreach (DriveInfo driveInfo in this.VolumeList)
//        //    {
//        //        string tempPath = resultPath.Replace(virtualDirInfo.Root.FullName, driveInfo.RootDirectory.FullName);
//        //        if (Directory.Exists(tempPath))
//        //            return tempPath;
//        //    }
//        //    return "";
//        //}

//        private DriveInfo SelectTargetVolume()
//        {
//            DriveInfo[] driveInfos = Array.FindAll(DriveInfo.GetDrives(), f => f.VolumeLabel == "BackUp");
//            Array.ForEach(driveInfos, driveInfo =>
//            {
//                float rate = driveInfo.AvailableFreeSpace * 100.0f / driveInfo.TotalSize;
//                string message = string.Format("Drive: {0}, Total: {1}, Free: {2}, Rate: {3:0.00}%", driveInfo.Name, driveInfo.TotalSize, driveInfo.AvailableFreeSpace, rate);
//                LogHelper.Debug(LoggerType.DataRemover, message);
//                //System.Diagnostics.Debug.WriteLine(message);
//            });

//            return Array.Find(driveInfos, f => (f.AvailableFreeSpace * 100.0f / f.TotalSize) > minFreeSpace);
//        }

//        private bool IsBackupNeed(string path)
//        {
//            if (srcStoringDays < 0)
//                return false;

//            bool copied = File.Exists(Path.Combine(path, "Copied"));
//            if (copied) // 이미 복사되었음.
//                return false;

//            string dirName = Path.GetFileName(path);
//            DateTime folderDate;
//            if (DateTime.TryParseExact(dirName, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out folderDate))
//            {
//                DateTime curTime = DateTime.Now;
//                TimeSpan timeSpan = curTime.Date - folderDate.Date;
//                return (timeSpan.TotalHours >= 24 * this.srcStoringDays);
//            }

//            return false;
//        }

//        private void DataCopyProc()
//        {
//            while (RequestStop == false)
//            {
//                if (Directory.Exists(srcPath) == false)
//                {
//                    Thread.Sleep(10000);
//                    continue;
//                }

//                DirectoryInfo resultInfo = new DirectoryInfo(srcPath);
//                foreach (DirectoryInfo directory in resultInfo.GetDirectories())
//                {
//                    switch (dataStoringType)
//                    {
//                        default:
//                        case DataStoringType.Seq:
//                            CopyData_Seq(directory);
//                            break;
//                        case DataStoringType.Day_Seq:
//                            CopyData_Days(directory);
//                            break;
//                        case DataStoringType.Month_Day_Seq:
//                            CopyData_Month(directory);
//                            break;
//                        case DataStoringType.Year_Month_Day_Seq:
//                            CopyData_Year(directory);
//                            break;
//                    }
//                }
//#if DEBUG
//                Thread.Sleep(new TimeSpan(0, 0, 10));
//#else
//                Thread.Sleep(new TimeSpan(1, 0, 0));
//#endif
//            }
//        }

//        private void CopyData_Seq(DirectoryInfo srcInfo)
//        {
//            if (IsBackupNeed(srcInfo.FullName) == true)
//            {
//                DriveInfo driveInfo = SelectTargetVolume();
//                if (driveInfo == null)
//                {
//                    LogHelper.Debug(LoggerType.DataRemover, string.Format("Copy Fail /  Backup Drive has no free space / SrcPath: {0}", srcInfo.FullName));
//                    return;
//                }

//                string destPath = srcInfo.FullName.Replace(srcInfo.Root.FullName, driveInfo.RootDirectory.Name);
//                destPath = destPath.Replace(@"\\", @"\");
//                lock (this)
//                    curBackupDrive = driveInfo;

//                try
//                {
//                    this.message = string.Format("Start Copy: {0} -> {1}", srcInfo.Name, driveInfo.Name);
//                    this.notify = true;
//                    LogHelper.Debug(LoggerType.DataRemover, string.Format("Start Copy / SrcPath: {0} / DestPath: {1}", srcInfo.FullName, destPath));
//                    FileHelper.CopyDirectory(srcInfo.FullName, destPath, true, false);
//                    File.WriteAllText(Path.Combine(srcInfo.FullName, "Copied"), destPath);
//                    LogHelper.Debug(LoggerType.DataRemover, string.Format("End Copy / SrcPath: {0} / DestPath: {1}", srcInfo.FullName, destPath));
//                }
//                catch (Exception ex)
//                {
//                    LogHelper.Debug(LoggerType.DataRemover, string.Format("Copy Fail: {0} / SrcPath : {1} / DestPath : {2}", ex.Message, srcInfo.FullName, destPath));
//                    this.message = string.Format("Copy Error: {0} -> {1}, {2}", srcInfo.Name, driveInfo.Name, ex.Message);
//                    this.notify = true;
//                }

//                lock (this)
//                    curBackupDrive = null;
//            }
//        }

//        private void CopyData_Days(DirectoryInfo srcInfo)
//        {
//            foreach (DirectoryInfo dailyDir in srcInfo.GetDirectories())
//                CopyData_Seq(dailyDir);
//        }


//        private void CopyData_Month(DirectoryInfo srcInfo)
//        {
//            foreach (DirectoryInfo monthDir in srcInfo.GetDirectories())
//                CopyData_Days(monthDir);
//        }

//        private void CopyData_Year(DirectoryInfo srcInfo)
//        {
//            foreach (DirectoryInfo yearDir in srcInfo.GetDirectories())
//                CopyData_Month(yearDir);
//        }

//    }
//}
