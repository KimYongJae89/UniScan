using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanG.Gravure.Settings;

namespace UniScanG.Module.Inspector.Inspect
{
    class DebugRawImageSaver : ThreadHandler
    {
        public ConcurrentQueue<ImageD> Queue { get; private set; }

        public string FilePath1 { get;private set; }
        public string FilePath2 { get;private set; }

        object locker = new object();

        int count;
        int iterate;

        int camIndex;
        int clientIndex;
        string lotNo;
        string modelName;
        DebugSaveRawImageSettings settings;

        public DebugRawImageSaver(string name, Thread workingThread = null, bool requestStop = false) : base(name, workingThread, requestStop)
        {
            if (workingThread == null)
                this.workingThread = new Thread(ThreadProc);
            this.requestStop = false;

            this.settings = Gravure.Settings.AdditionalSettings.Instance().DebugSaveRawImage;
            this.modelName = SystemManager.Instance().CurrentModel.Name;
            this.lotNo = SystemManager.Instance().ProductionManager.CurProduction.LotNo;
            this.camIndex = SystemManager.Instance().ExchangeOperator.GetCamIndex(); // C
            this.count = 0; // S
            this.clientIndex = SystemManager.Instance().ExchangeOperator.GetClientIndex(); // L
            
            this.FilePath1 = Path.Combine(@"\\192.168.50.1\", "ImageSaver");
            this.FilePath2 = Path.Combine(this.FilePath1, modelName, lotNo);

            this.Queue = new ConcurrentQueue<ImageD>();
            this.iterate = 0;
        }

        public void BeginSave(AlgoImage algoImage, object tag)
        {
            try
            {
                lock (locker)
                {
                    if (!this.settings.Use)
                        return;

                    int period = this.settings.Period;
                    if (period <= 0)
                        return;

                    if (this.requestStop)
                        return;

                    if (this.iterate % period == 0)
                    {
                        LogHelper.Debug(LoggerType.Operation, $"DebugRawImageSaver::BeginSave - Enqueue {tag}");
                        this.Queue.Enqueue(algoImage.ToImageD());
                        this.iterate = 0;
                    }
                    this.iterate++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex);
            }
        }
        
        public override bool Stop(int timeoutMs = -1)
        {
            if(base.Stop(timeoutMs))
            {
                while (this.Queue.TryDequeue(out ImageD imageD))
                    imageD.Dispose();

                return true;
            }
            return false;
        }

        private void ThreadProc()
        {
            NetworkDrive networkDrive = new NetworkDrive();

            if (!this.settings.Use)
            {
                Stop(0);
                return;
            }

            try
            {
                LogHelper.Debug(LoggerType.Operation, $"DebugRawImageSaver::ThreadProc - ConnectNetworkDrive - Path: [{this.FilePath1}]");
                LogHelper.Debug(LoggerType.Operation, $"DebugRawImageSaver::ThreadProc - ConnectNetworkDrive - ID: [{this.settings.CM_FileShareId}]");
                LogHelper.Debug(LoggerType.Operation, $"DebugRawImageSaver::ThreadProc - ConnectNetworkDrive - PW: [{this.settings.CM_FileSharePW}]");
                int code = networkDrive.ConnectNetworkDrive(null, this.FilePath1, this.settings.CM_FileShareId, this.settings.CM_FileSharePW);
                LogHelper.Debug(LoggerType.Operation, $"DebugRawImageSaver::ThreadProc - ConnectNetworkDrive - return:  [{code}]");

                LogHelper.Debug(LoggerType.Operation, $"DebugRawImageSaver::ThreadProc - CreateDirectory - Path:  [{this.FilePath2}]");
                Directory.CreateDirectory(this.FilePath2);
                if (!Directory.Exists(this.FilePath2))
                {
                    LogHelper.Debug(LoggerType.Operation, $"DebugRawImageSaver::ThreadProc - CreateDirectory - Not Exist. Stop.");
                    Stop(0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex);
                Stop(0);
                return;
            }

            while (!this.requestStop)
            {
                ImageD imageD = null;
                if (!this.Queue.TryDequeue(out imageD))
                {
                    Thread.Sleep(10);
                    continue;
                }

                try
                {
                    string fileName = SystemManager.Instance().CurrentModel.GetImageName(this.camIndex, this.count, this.clientIndex, "png");
                    string fullPathName = Path.Combine(this.FilePath2, fileName);
                    LogHelper.Debug(LoggerType.Operation, $"DebugRawImageSaver::ThreadProc - SaveImage Start - {fullPathName}");
                    System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                    imageD.SaveImage(fullPathName, System.Drawing.Imaging.ImageFormat.Png);
                    imageD.Dispose();
                    sw.Stop();
                    LogHelper.Debug(LoggerType.Operation, $"DebugRawImageSaver::ThreadProc - SaveImage End - {sw.ElapsedMilliseconds}ms");

                    this.count++;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, ex);
                }
            }

            LogHelper.Debug(LoggerType.Operation, $"DebugRawImageSaver::ThreadProc - DisconnectNetworkDrive");
            networkDrive?.DisconnectNetworkDrive();
            networkDrive = null;
        }
    }
}
