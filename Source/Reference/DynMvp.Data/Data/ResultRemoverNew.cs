using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Globalization;

using DynMvp.Base;
using System.Threading.Tasks;

namespace DynMvp.Data
{
    public delegate void DataManagerLockerWork(string message);
    public class DataManagerLocker
    {
        public static string LockFileName = "DataManagerLockerWorkingList";
        public event DataManagerLockerWork OnWork = null;

        string workingPath;
        static List<Tuple<bool, string>> onWorkingPath = new List<Tuple<bool, string>>();

        static bool OnInitializing = false;

        public DataManagerLocker(string workingPath)
        {
            this.workingPath = workingPath;

            if (OnInitializing)
                return;

            string path = Path.Combine(this.workingPath, LockFileName);
            if (File.Exists(path))
            {
                lock (onWorkingPath)
                {
                    // 미완성 복사/삭제 목록 작성.
                    string[] lines = File.ReadAllLines(Path.Combine(this.workingPath, DataManagerLocker.LockFileName));
                    Array.ForEach(lines, f =>
                    {
                        string[] token = f.Split('*');
                        if (token.Length == 1)
                            onWorkingPath.Add(new Tuple<bool, string>(true, f));
                        else if (token.Length == 2)
                        {
                            bool remove = false;
                            bool isBoolian = bool.TryParse(token[0], out remove);
                            onWorkingPath.Add(new Tuple<bool, string>((!isBoolian || remove), token[1]));
                        }
                    });
                }

                OnInitializing = true;
                Task.Run(() =>
                {
                    // 목록에 있는 파일들 정리
                    lock (onWorkingPath)
                    {
                        onWorkingPath.ForEach(f =>
                        {
                            if (f.Item1)
                                FileHelper.ClearFolder(f.Item2, DataCopier.FlagFileName);
                        });
                        onWorkingPath.Clear();
                    }
                    File.Delete(path);
                    OnInitializing = false;
                });
            }
        }

        public bool StartWork(string path, bool removeAtReload)
        {
            if (OnInitializing)
                return false;

            lock (onWorkingPath)
            {
                if (onWorkingPath.Exists(f => f.Item2 == path))
                    return false;

                onWorkingPath.Add(new Tuple<bool, string>(removeAtReload, path));
                Write();

                return true;
            }
        }

        public void EndWork(string path)
        {
            if (OnInitializing)
                return;

            lock (onWorkingPath)
            {
                int idx = onWorkingPath.FindIndex(f => f.Item2 == path);
                if (idx >= 0)
                {
                    onWorkingPath.RemoveAt(idx);
                    Write();
                }
            }
        }

        private void Write()
        {
            if (string.IsNullOrEmpty(this.workingPath))
                return;

            lock (onWorkingPath)
            {
                StringBuilder sb = new StringBuilder();
                onWorkingPath.ForEach(f => sb.AppendLine(string.Format(@"{0}*{1}", f.Item1, f.Item2)));
                File.WriteAllText(Path.Combine(this.workingPath, DataManagerLocker.LockFileName), sb.ToString());
            }
        }
    }

    public class DataRemover : ThreadHandler
    {
        protected int resultStoringDays;
        protected int logStoringDays;
        protected float minimumFreeSpaceP;
        protected ProductionManagerBase productionManager;

        DataManagerLocker dataManagerLocker;
        DirectoryInfo logDataFolder;
        TimeSpan period;

        public DataRemover(int resultStoringDays, float minimumFreeSpaceP, ProductionManagerBase productionManager, DirectoryInfo logDataFolder) : base("DataRemover")
        {
            this.resultStoringDays = resultStoringDays;
            this.logStoringDays = MathHelper.Clipping(resultStoringDays, 7, 60); // 최소 7일. 최대 60일.
            this.minimumFreeSpaceP = minimumFreeSpaceP;
            this.productionManager = productionManager;
            this.logDataFolder = logDataFolder;
#if DEBUG
            this.period = new TimeSpan(0, 0, 10);
#else
            this.period = new TimeSpan(0, 30, 0);
#endif

            this.dataManagerLocker = new DataManagerLocker("");

            this.workingThread = new Thread(new ThreadStart(DataRemoveProc));
            this.workingThread.IsBackground = true;
            this.requestStop = false;

            this.Start();
        }

        private void DataRemoveProc()
        {
            DateTime lastStartTime = DateTime.MinValue;
            while (RequestStop == false)
            {
                DateTime curTime = DateTime.Now;
                TimeSpan timeSpan = (curTime - lastStartTime);
                if (timeSpan < period)
                {
                    Thread.Sleep(period - timeSpan);
                    continue;
                }

                try
                {
                    //LogHelper.Debug(LoggerType.DataRemover, "DataRemover::DataRemoveProc - Run");
                    lastStartTime = curTime;
                    if (this.productionManager != null)
                        RemoveResult();

                    RemoveErrorReportData();

                    if (this.logDataFolder != null)
                        RemoveLogData();

                    RemoveAdditionalData();
                    //LogHelper.Debug(LoggerType.DataRemover, "DataRemover::DataRemoveProc - Wait");
                }catch(Exception ex)
                {
                    LogHelper.Error(LoggerType.DataRemover, string.Format("DataRemover::DataRemoveProc - {0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));
                }
            }
        }

        private List<string> GetSubDirectorys(DirectoryInfo directoryInfo)
        {
            DirectoryInfo[] subDirectoryInfos = directoryInfo.GetDirectories();
            List<string> subDirectoryList = new List<string>();
            Array.ForEach(subDirectoryInfos, f =>
            {
                subDirectoryList.Add(f.FullName);
                subDirectoryList.AddRange(GetSubDirectorys(f));
            });
            return subDirectoryList;
        }

        private void RemoveErrorReportData()
        {
            // 에러 리포트는 최대 365일 저장.
            int timeSpanDays = 365;
            DateTime limit = DateTime.Now - new TimeSpan(timeSpanDays, 0, 0, 0);
            Predicate<ErrorItem> predicate = new Predicate<ErrorItem>(f =>
            {
                return f.ErrorTime < limit && f.IsCleared;
            });

            object lockObject = ErrorManager.Instance().LockObject;
            lock (lockObject)
            {
                ErrorManager.Instance().ErrorItemList.RemoveAll(predicate);
                ErrorManager.Instance().SaveErrorList();
            }
        }

        private void RemoveLogData()
        {
            DirectoryInfo[] subDirectoryInfo = logDataFolder.GetDirectories();
            foreach (DirectoryInfo directortInfo in subDirectoryInfo)
            {
                bool ok = DateTime.TryParseExact(directortInfo.Name, LogHelper.BackupPathForamt, null, DateTimeStyles.None, out DateTime dateTime);
                if (ok == false)
                    continue;

                TimeSpan timeSpan = DateTime.Now - dateTime;
                if (timeSpan.TotalDays > this.logStoringDays)
                    RemoveLogData(directortInfo);
            }

            FileInfo[] fileInfos = logDataFolder.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                if (!DateTime.TryParse(fileInfo.Extension, out DateTime dateTime))
                    continue;

                TimeSpan timeSpan = DateTime.Now - dateTime;
                if (timeSpan.TotalDays > this.logStoringDays)
                    RemoveLogData(fileInfo);
            }
        }

        private void RemoveLogData(DirectoryInfo directoryInfo)
        {
            try
            {
                LogHelper.Debug(LoggerType.DataRemover, string.Format("DataRemover::RemoveLogData - {0}", directoryInfo.FullName));

                FileHelper.ClearFolder(directoryInfo.FullName);
                Directory.Delete(directoryInfo.FullName);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.DataRemover, $"DataRemover::RemoveLogData - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        private void RemoveLogData(FileInfo fileInfo)
        {
            try
            {
                LogHelper.Debug(LoggerType.DataRemover, string.Format("DataRemover::RemoveLogData - {0}", fileInfo.FullName));
                fileInfo.Delete();
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.DataRemover, $"DataRemover::RemoveLogData - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }

        }
        protected virtual void RemoveAdditionalData()
        {

        }

        private void RemoveResult()
        {
            if (this.productionManager.List.Count == 0)
                return;

            int fullRemoveCnt = 0;
            bool isFull;
            while (isFull = IsDriveFull() && fullRemoveCnt < 5)
            {
                Thread.Sleep(500);
                ProductionBase production = this.productionManager.List.FirstOrDefault();
                if (production == null)
                    break;

                if (production == this.productionManager.CurProduction)
                    break;

                if (RemoveAllData(production, "DriveFull"))
                    this.productionManager.RemoveProduction(production);
                fullRemoveCnt++;
            }

            List<ProductionBase> productionList = new List<ProductionBase>(this.productionManager.List);
            foreach (ProductionBase production in productionList)
            {
                if (production.Equals(this.productionManager.CurProduction))
                    continue;

                bool isCopied = IsCopied(production);
                if (isCopied)
                {   // Copier에 의해 복사되었으면 바로 제거.
                    RemoveLocalData(production);
                }

                bool isOldData = IsOldData(production);
                if (isOldData)
                {   // 오래된 파일 제거.
                    if (RemoveAllData(production, "OldData"))
                        this.productionManager.RemoveProduction(production);
                }

                bool userDelete = production.UserRemoveFlag;
                if (userDelete)
                {   // 사용자 제거
                    if (RemoveAllData(production, "UserDelete"))
                        this.productionManager.RemoveProduction(production);
                }
            }
        }

        private bool IsDriveFull()
        {
            if (this.minimumFreeSpaceP < 0)
                return false;

            DirectoryInfo directoryInfo = new DirectoryInfo(productionManager.DefaultPath);
            if (directoryInfo.Exists == false)
                return false;

            DriveInfo driveInfo = new DriveInfo(directoryInfo.Root.Name);
            float freeRate = driveInfo.AvailableFreeSpace * 100.0f / driveInfo.TotalSize;
            bool isFull = freeRate < this.minimumFreeSpaceP;
            if (isFull)
            {
                string message = string.Format("Local Drive: {0}, Total: {1}, Free: {2}, Rate: {3:0.00}%", driveInfo.Name, driveInfo.TotalSize, driveInfo.AvailableFreeSpace, freeRate);
                LogHelper.Debug(LoggerType.DataRemover, message);
            }
            return isFull;
        }

        private bool RemoveLocalData(ProductionBase production)
        {
            string targetPath = production.GetResultPath();

            try
            {
                if (this.dataManagerLocker.StartWork(targetPath, false) == false) // 동작중이면 작업 안 함
                    return false;

                LogHelper.Debug(LoggerType.DataRemover, string.Format("DataRemover::RemoveLocalData Start - SrcPath: {0}", targetPath), true);
                if (Directory.Exists(targetPath))
                    FileHelper.ClearFolder(targetPath, DataCopier.FlagFileName);
                LogHelper.Debug(LoggerType.DataRemover, "DataRemover::RemoveLocalData Done");

                this.dataManagerLocker.EndWork(targetPath);

                return true;
            }
            catch (Exception ex)
            {
                this.dataManagerLocker.EndWork(targetPath);

                LogHelper.Error(LoggerType.DataRemover, string.Format("DataRemover::RemoveLocalData Fail - {0}", ex.Message));
                return false;
            }
        }

        protected bool RemoveAllData(ProductionBase production, string reason)
        {
            ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Initialize.DataRemove, ErrorLevel.Info,
                "Data Remove Start - {0}, Date: {1}, Model: {2}, Lot: {3}", new string[] { reason, production.StartTime.ToString("yy-MM-dd"), production.Name, production.LotNo }, ""));
            string targetPath = production.GetResultPath();

            bool ok = true;
            if (Directory.Exists(targetPath))
                ok = RemoveAllData(targetPath);

            return ok;
        }

        protected bool RemoveAllData(string targetPath)
        {
            try
            {
                if (this.dataManagerLocker.StartWork(targetPath, false) == false) // 동작중이면 작업 안 함
                    return false;

                LogHelper.Debug(LoggerType.DataRemover, string.Format("DataRemover::RemoveAllData Start - SrcPath: {0}", targetPath));
                string copiedFlag = Path.Combine(targetPath, DataCopier.FlagFileName);
                if (File.Exists(copiedFlag))
                {
                    string copiedFile = File.ReadAllText(copiedFlag);
                    if (Directory.Exists(copiedFile))
                    {
                        FileHelper.ClearFolder(copiedFile);
                        RemoveUpperFolder(copiedFile);
                    }
                }

                RemoveCopiedDataForOldVersion(targetPath);

                if (Directory.Exists(targetPath))
                {
                    FileHelper.ClearFolder(targetPath);
                    RemoveUpperFolder(targetPath);
                }

                RemoveAllDataExtend(targetPath);

                LogHelper.Debug(LoggerType.DataRemover, "DataRemover::RemoveAllData End");
                this.dataManagerLocker.EndWork(targetPath);
                return true;
            }
            catch (Exception ex)
            {
                this.dataManagerLocker.EndWork(targetPath);
                LogHelper.Error(LoggerType.DataRemover, string.Format("DataRemover::RemoveAllData Fail - {0}", ex.Message));
                ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Initialize.Information, ErrorLevel.Info,
                    "Data Remove Fail. {0}", new string[] { ex.Message }, ""));
                return false;
            }
        }

        protected virtual void RemoveAllDataExtend(string reportPath) { }

        /// <summary>
        /// 알고리즘 개선 전 장비와 호환을 위함. 추후 제거 예정
        /// </summary>
        /// <param name="targetPath"></param>
        private void RemoveCopiedDataForOldVersion(string targetPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(targetPath);
            DriveInfo[] driveInfos = DataCopier.BackupDriveInfos;
            foreach (DriveInfo driveInfo in driveInfos)
            {
                string copiedName = targetPath.Replace(directoryInfo.Root.FullName, driveInfo.RootDirectory.FullName);
                copiedName = copiedName.Replace(@"\\", @"\");

                if (Directory.Exists(copiedName))
                {
                    FileHelper.ClearFolder(copiedName);
                    RemoveUpperFolder(copiedName);
                }
            }
        }

        protected void RemoveUpperFolder(string copiedFile)
        {
            DirectoryInfo di = new DirectoryInfo(copiedFile);
            while (di != null && di.Exists == false)
                di = di.Parent;

            if (di == null)
                return;

            int lowerItemCount = di.GetDirectories().Length + di.GetFiles().Length;
            while (lowerItemCount == 0)
            {
                Directory.Delete(di.FullName, false);
                Thread.Sleep(10);
                di = di.Parent;
                lowerItemCount = di.GetDirectories().Length + di.GetFiles().Length;
            }
        }

        private bool IsOldData(ProductionBase production)
        {
            if (resultStoringDays < 0)
                return false;

            DateTime currentDate = DateTime.Now;
            DateTime productionDate = production.StartTime;

            double days = (currentDate.Date - productionDate.Date).TotalDays;

            return (days >= resultStoringDays);
        }

        private bool IsCopied(ProductionBase production)
        {
            string resultPath = production.GetResultPath();
            if (Directory.Exists(resultPath) == false)
                return false;

            string file = Path.Combine(production.GetResultPath(), DataCopier.FlagFileName);

            bool exist = File.Exists(file);
            int files = Directory.GetFiles(resultPath).Length;
            int directories = Directory.GetDirectories(resultPath).Length;
            return exist && (files > 1 || directories > 0);
        }
    }

    public class DataCopier : ThreadHandler
    {
        public static string FlagFileName = "Copied";

        DataManagerLocker dataManagerLocker;

        public static DriveInfo[] BackupDriveInfos
        {
            get
            {
                if (backupDriveInfos == null)
                {
                    List<DriveInfo> driveInfoList = DriveInfo.GetDrives().ToList();
#if DEBUG
                    backupDriveInfos = driveInfoList.FindAll(f => f.IsReady && f.VolumeLabel.ToLower() == "backup" && f.DriveFormat == "NTFS").ToArray();
#else
                    backupDriveInfos = driveInfoList.ToList().FindAll(f => f.DriveType == DriveType.Fixed && f.IsReady && f.VolumeLabel.ToLower() == "backup" && f.DriveFormat == "NTFS").ToArray();
#endif
                }
                return backupDriveInfos;
            }
        }
        static DriveInfo[] backupDriveInfos;

        public static long MinimumDiskTotalSize
        {
            get
            {
                if (minimumDiskTotalSize < 0)
                {
                    string rootPath = Path.GetPathRoot(Environment.CurrentDirectory);
                    DriveInfo d = new DriveInfo(rootPath);

                    if (BackupDriveInfos.Length > 0)
                        minimumDiskTotalSize = Math.Min(d.TotalSize, BackupDriveInfos.Min(f => f.TotalSize));
                    else
                        minimumDiskTotalSize = d.TotalSize;
                }
                return minimumDiskTotalSize;
            }
        }
        static long minimumDiskTotalSize = -1;

        public DriveInfo BackupDrive { get => backupDrive; }
        DriveInfo backupDrive = null;

        public ProductionBase BackupProduction { get => backupProduction; }
        ProductionBase backupProduction = null;

        ProductionManagerBase productionManager;
        float srcStoringDays;
        float minFreeSizeP;
        TimeSpan period;

        public DataCopier(ProductionManagerBase productionManager, float srcStoringDays, float minFreeSizeP) : base("ResultCopier")
        {
            this.productionManager = productionManager;
            this.srcStoringDays = srcStoringDays;
            this.minFreeSizeP = minFreeSizeP;
            this.dataManagerLocker = new DataManagerLocker(productionManager.DefaultPath);

#if DEBUG
            this.period = new TimeSpan(0, 0, 10);
#else
            this.period = new TimeSpan(0, 25, 0);
#endif

            this.workingThread = new Thread(new ThreadStart(DataCopyProc));
            this.workingThread.IsBackground = true;
            this.requestStop = false;

            if(BackupDriveInfos.Length>0)
                this.Start();
        }

        public static float GetMinSizeGb(float minFreeSizeP)
        {
            if (minFreeSizeP < 0)
                return 0;
            return (minFreeSizeP / 100f * MinimumDiskTotalSize) / 1024f / 1024f / 1024f;
        }

        //public string GetActualPath(string resultPath)
        //{
        //    // 로컬 드라이브에 존재함. 백업중일 수 있음.
        //    if (Directory.Exists(resultPath))
        //    {
        //        bool copied = File.Exists(Path.Combine(resultPath, "Copied"));
        //        if (copied == false)    // 파일이 백업 드라이브로 복사되지 않음.
        //            return resultPath;
        //    }

        //    // 로컬 드라이브에 존재하지 않음. 백업 후 삭제되었음.
        //    DirectoryInfo virtualDirInfo = new DirectoryInfo(resultPath);
        //    foreach (DriveInfo driveInfo in this.VolumeList)
        //    {
        //        string tempPath = resultPath.Replace(virtualDirInfo.Root.FullName, driveInfo.RootDirectory.FullName);
        //        if (Directory.Exists(tempPath))
        //            return tempPath;
        //    }
        //    return "";
        //}

        private DriveInfo SelectTargetVolume()
        {
            //Array.ForEach(BackupDriveInfos, driveInfo =>
            //{
            //    float rate = driveInfo.AvailableFreeSpace * 100.0f / driveInfo.TotalSize;
            //    string message = string.Format("Drive: {0}, Total: {1}, Free: {2}, Rate: {3:0.00}%", driveInfo.Name, driveInfo.TotalSize, driveInfo.AvailableFreeSpace, rate);
            //    LogHelper.Debug(LoggerType.DataRemover, message);
            //});

            if (minFreeSizeP < 0)
            {
                DriveInfo maxFreeDriveInfo = null;
                float maxFreeSpace = 0;
                Array.ForEach(BackupDriveInfos, f =>
                {
                    float freeSpace = GetFreeSpace(f);
                    if (freeSpace > maxFreeSpace)
                    {
                        maxFreeDriveInfo = f;
                        maxFreeSpace = freeSpace;
                    }
                });
                return maxFreeDriveInfo;
            }
            else
            {
                return Array.Find(BackupDriveInfos, f => GetFreeSpace(f) > minFreeSizeP);
            }
        }

        private float GetFreeSpace(DriveInfo driveInfo)
        {
            // 왜 위에꺼로 안하고 아래꺼로 했었을까..?
            float freeSpace = driveInfo.AvailableFreeSpace * 100.0f / driveInfo.TotalSize;
            //float freeSpace = driveInfo.AvailableFreeSpace * 100.0f / MinimumDiskTotalSize;
            return freeSpace;
        }

        private void DataCopyProc()
        {
            DateTime lastStartTime = DateTime.MinValue;
            while (RequestStop == false)
            {
                DateTime curTime = DateTime.Now;
                TimeSpan timeSpan = (curTime - lastStartTime);
                if (timeSpan < period)
                {
                    Thread.Sleep(period - timeSpan);
                    continue;
                }
                lastStartTime = curTime;

                if (BackupDriveInfos.Length > 0)
                {
                    //LogHelper.Debug(LoggerType.DataRemover, "DataCopier::DataCopyProc - Run");
                    List<ProductionBase> productionList = new List<ProductionBase>(this.productionManager.List);
                    foreach (ProductionBase production in productionList)
                    {
                        bool skip = (production == this.productionManager.CurProduction);
                        if (skip)
                            continue;

                        bool copiable = IsCopiable(production);
                        if (copiable)
                        {
                            string srcPath = production.GetResultPath();
                            LogHelper.Debug(LoggerType.DataRemover, string.Format("Src Path [{0}] is copiable.", srcPath));

                            DriveInfo dstInfo = SelectTargetVolume();
                            if (dstInfo != null)
                            {
                                float rate = dstInfo.AvailableFreeSpace * 100.0f / dstInfo.TotalSize;
                                string message = string.Format("Target Drive: {0}, Total: {1}, Free: {2}, Rate: {3:0.00}%", dstInfo.Name, dstInfo.TotalSize, dstInfo.AvailableFreeSpace, rate);
                                LogHelper.Debug(LoggerType.DataRemover, message);
                                CopyData(production, dstInfo);
                            }
                            else
                            {
                                string message = string.Format("Copy Fail: Cannot find target drive or Target drive has no free space / SrcPath: {0}", srcPath);
                                LogHelper.Error(LoggerType.DataRemover, message);
                                break;
                            }
                        }
                    }
                    //LogHelper.Debug(LoggerType.DataRemover, "DataCopier::DataCopyProc - End");
                }
            }
        }

        private void SetWork(DriveInfo dstDrvInfo, ProductionBase production)
        {
            lock (this)
            {
                this.backupDrive = dstDrvInfo;
                this.backupProduction = production;
            }
        }

        private bool IsCopiable(ProductionBase production)
        {
            if (srcStoringDays < 0)
                return false;

            string path = production.GetResultPath();
            if (Directory.Exists(path) == false)
                return false;

            bool copied = File.Exists(Path.Combine(path, DataCopier.FlagFileName));
            if (copied) // 이미 복사되었음.
                return false;

            if (production.UserRemoveFlag)
                return false;

            DateTime curTime = DateTime.Now;
            DateTime procTime = production.LastUpdateTime;
            double totalDays = (curTime - procTime).TotalDays;
            return (totalDays >= this.srcStoringDays);
        }

        private void CopyData(ProductionBase production, DriveInfo dstInfo)
        {
            string srcPath = production.GetResultPath();

            DirectoryInfo srcInfo = new DirectoryInfo(srcPath);
            if (srcInfo.Exists == false)
                return;

            string destPath = srcInfo.FullName.Replace(srcInfo.Root.FullName, dstInfo.RootDirectory.Name);
            destPath = destPath.Replace(@"\\", @"\");

            string doneFile = Path.Combine(srcInfo.FullName, DataCopier.FlagFileName);
            try
            {
                if (!this.dataManagerLocker.StartWork(srcInfo.FullName, false))
                    return;
                
                if(!this.dataManagerLocker.StartWork(destPath, true))
                {
                    this.dataManagerLocker.EndWork(srcInfo.FullName);
                    return;
                }
                
                SetWork(dstInfo, production);

                // 복사 시작
                LogHelper.Debug(LoggerType.DataRemover, string.Format("DataCopier::CopyData Copy Start - From {0} - To {1}", srcInfo.FullName, dstInfo.Name));
                ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Initialize.DataRemove, ErrorLevel.Info,
                "Data Copy Start - Date: {0}, Model: {1}, Lot: {2}", new string[] { production.StartTime.ToString("yy-MM-dd"), production.Name, production.LotNo }, ""));

                FileHelper.CopyDirectory(srcInfo.FullName, destPath, true, true);

                // 복사 끝
                File.WriteAllText(doneFile, destPath);
                this.dataManagerLocker.EndWork(srcInfo.FullName);
                this.dataManagerLocker.EndWork(destPath);
                LogHelper.Debug(LoggerType.DataRemover, "DataCopier::CopyData Copy Done");
            }
            catch (Exception ex)
            {
                this.dataManagerLocker.EndWork(srcInfo.FullName);
                this.dataManagerLocker.EndWork(destPath);
                LogHelper.Debug(LoggerType.DataRemover, string.Format("DataCopier::CopyData Copy Fail - {0}", ex.Message));
                return;
            }
            finally
            {
                SetWork(null, null);
            }

            try
            {
                // 삭제 시작
                if (this.dataManagerLocker.StartWork(srcInfo.FullName, false) == false) // 동작중이면 작업 안 함
                    return;

                LogHelper.Debug(LoggerType.DataRemover, string.Format("DataCopier::CopyData Delete Start - From {0}", srcInfo.FullName));
                FileHelper.ClearFolder(srcInfo.FullName, DataCopier.FlagFileName);

                // 삭제 끝
                this.dataManagerLocker.EndWork(srcInfo.FullName);
                LogHelper.Debug(LoggerType.DataRemover, "DataCopier::CopyData Delete End");
            }
            catch (Exception ex)
            {
                this.dataManagerLocker.EndWork(destPath);
                LogHelper.Debug(LoggerType.DataRemover, string.Format("DataCopier::CopyData Delete Fail - {0}", ex.Message));
            }
        }

        public static string GetCopiedPath(string path)
        {
            string actualPath = path;

            if (File.Exists(Path.Combine(path, DataCopier.FlagFileName)))
            {
                actualPath = File.ReadAllText(Path.Combine(path, DataCopier.FlagFileName));
            }
            else if (Directory.Exists(path) == false)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                DriveInfo[] driveInfos = DataCopier.BackupDriveInfos;
                foreach (DriveInfo driveInfo in driveInfos)
                {
                    string ss = driveInfo.Name;
                    string resultPath2 = ss + path.Substring(ss.Length);
                    if (Directory.Exists(resultPath2))
                        return resultPath2;
                }
            }
            return actualPath;
        }
    }
}
