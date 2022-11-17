using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace UniScanG.Common.Data
{
    class DataRemoverG : DataRemover
    {
        DirectoryInfo hwMonitoringDirectoryInfo;
        DirectoryInfo imageSaverDirectoryInfo;

        public DataRemoverG(int resultStoringDays, float minimumFreeSpaceP, ProductionManagerBase productionManager, DirectoryInfo log) 
            : base(resultStoringDays, minimumFreeSpaceP, productionManager, log)
        {
            this.hwMonitoringDirectoryInfo = new DirectoryInfo(PathSettings.Instance().HWMonitor);
            this.imageSaverDirectoryInfo = new DirectoryInfo(Path.Combine(@"\\127.0.0.1", "ImageSaver"));

            LogHelper.Debug(LoggerType.DataRemover, $"DataRemover::RemoveImageSaver - {imageSaverDirectoryInfo.FullName}");
            if (!this.imageSaverDirectoryInfo.Exists)
                LogHelper.Debug(LoggerType.DataRemover, $"DataRemover::RemoveImageSaver - NotExist - {imageSaverDirectoryInfo.FullName}");

        }

        protected override void RemoveAdditionalData()
        {
            if (this.hwMonitoringDirectoryInfo != null)
                RemoveHwMonData();

            if (this.imageSaverDirectoryInfo != null)
                RemoveImageSaver();
        }

        private void RemoveHwMonData()
        {
            if (!this.hwMonitoringDirectoryInfo.Exists)
                return;

            FileInfo[] fileInfos = this.hwMonitoringDirectoryInfo.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                DateTime dateTime;
                string name = fileInfo.Name.Replace(fileInfo.Extension, "");
                bool ok = DateTime.TryParseExact(name, HardwareMonitor.FileNameFormat, null, DateTimeStyles.None, out dateTime);
                if (ok == false)
                    continue;

                TimeSpan timeSpan = DateTime.Now - dateTime;
                if (timeSpan.TotalDays > this.logStoringDays)
                {
                    try
                    {
                        LogHelper.Debug(LoggerType.DataRemover, string.Format("DataRemover::RemoveHwMonData - Start - {0}", fileInfo.FullName));
                        fileInfo.Delete();
                        LogHelper.Debug(LoggerType.DataRemover, string.Format("DataRemover::RemoveHwMonData - End", fileInfo.FullName));
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(LoggerType.DataRemover, $"DataRemover::RemoveHwMonData - Error - {ex.Message}");
                    }
                }
            }
        }

        private void RemoveImageSaver()
        {
            if (!this.imageSaverDirectoryInfo.Exists)
                return;
            
            DateTime past = DateTime.Now.AddDays(-28);
            DirectoryInfo[] modleDirectories = this.imageSaverDirectoryInfo.GetDirectories();
            foreach (DirectoryInfo modelDirectory in modleDirectories)
            {
                DirectoryInfo[] lotDirectories = modelDirectory.GetDirectories();
                foreach (DirectoryInfo lotDirectorie in lotDirectories)
                {
                    if (lotDirectorie.LastAccessTime < past)
                    {
                        try
                        {
                            LogHelper.Debug(LoggerType.DataRemover, $"DataRemover::RemoveImageSaver - Start - {lotDirectorie.FullName}");
                            lotDirectorie.Delete(true);
                            LogHelper.Debug(LoggerType.DataRemover, "DataRemover::RemoveImageSaver - End");
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(LoggerType.DataRemover, $"DataRemover::RemoveImageSaver - Error - {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}
