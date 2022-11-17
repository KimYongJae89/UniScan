using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace UniScanG.Module.Observer.Data
{
    class DataRemover : ThreadHandler
    {
        public DataRemover() : base("DataRemover")
        {
            this.workingThread = new Thread(ThreadProc) { IsBackground = true };
            this.requestStop = false;
        }

        private void ThreadProc()
        {
            DateTime lastWorkingTime = DateTime.Now;

            while (!this.requestStop)
            {
                DateTime dateTime = DateTime.Now;
                if (dateTime.Subtract(lastWorkingTime).TotalMinutes > 5)
                {
                    float storeResultHours = Math.Max(1, Properties.Settings.Default.ResultStoringDays * 24);

                    DirectoryInfo dInfoResult = new DirectoryInfo(PathSettings.Instance().Result);
                    DriveInfo driveInfoResult = new DriveInfo(dInfoResult.Root.Name);                    
                    DirectoryInfo[] dInfos = dInfoResult.GetDirectories();
                    Array.ForEach(dInfos, f =>
                    {
                        try
                        {
                            DirectoryInfo[] subDirectorys = f.GetDirectories(); // format: {HH}_{mm}_LOT
                            Array.ForEach(subDirectorys, g =>
                            {
                                DateTime dt = DateTime.ParseExact(f.Name, "yyyy_MM_dd", null);

                                string[] tokens = g.Name.Split('_');
                                if (tokens.Length > 2)
                                {
                                    if (int.TryParse(tokens[0], out int hour))
                                        dt.AddHours(hour);

                                    if (int.TryParse(tokens[1], out int min))
                                        dt.AddMinutes(min);
                                }

                                bool isTooOld = dateTime.Subtract(dt).TotalHours > storeResultHours;
                                bool isDriveFull = driveInfoResult.AvailableFreeSpace * 1.0 / driveInfoResult.TotalSize < 0.005;
                                if (isTooOld || isDriveFull)
                                {
                                    LogHelper.Debug(LoggerType.DataRemover, $"DataRemover::ThreadProc - Remove Data: {g.FullName}, isTooOld: {isTooOld}, isDriveFull: {isDriveFull} ");
                                    g.Delete(true);
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(LoggerType.DataRemover, $"DataRemover::ThreadProc - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                        }
                    });

                    float storeLogDays = 7;
                    DirectoryInfo dInfoLog = new DirectoryInfo(PathSettings.Instance().Result);
                    DriveInfo driveInfoLog = new DriveInfo(dInfoLog.Root.Name);
                    FileInfo[] fileInfoLogs = dInfoLog.GetFiles();
                    Array.ForEach(fileInfoLogs, f =>
                    {
                        try
                        {
                            //DateTime dt = DateTime.ParseExact(f.Extension, "yyyy-MM-dd", null);
                            if (!DateTime.TryParse(f.Extension, out DateTime dt))
                                return;
                            bool isTooOld = dateTime.Subtract(dt).Days > storeLogDays;
                            bool isDriveFull = driveInfoResult.AvailableFreeSpace * 1.0 / driveInfoResult.TotalSize < 0.005;

                            if (isTooOld || isDriveFull)
                            {
                                LogHelper.Debug(LoggerType.DataRemover, $"DataRemover::ThreadProc - Remove Log: {f.FullName}, isTooOld: {isTooOld}, isDriveFull: {isDriveFull} ");
                                f.Delete();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(LoggerType.DataRemover, $"DataRemover::ThreadProc - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                        }
                    });
                    lastWorkingTime = dateTime;
                }

                Thread.Sleep(1000);
            }
        }
    }
}
