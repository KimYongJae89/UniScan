using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.InspData;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniScanG.Data;
using UniScanG.Data.Model;
using UniScanG.Module.Monitor.Device;
using UniScanG.Module.Monitor.MachineIF;
using UniScanG.Module.Monitor.Processing;

namespace UniScanG.Module.Monitor.Inspect
{
    public class InspectRunner : DirectTriggerInspectRunner
    {
        SortedDictionary<InspectOption, ImageD> grabbedImageDic = new SortedDictionary<InspectOption, ImageD>(new InspectOptionComparer());
        DateTime lastGrabTime = DateTime.MinValue;
        List<double> termMsList = new List<double>();
        ThreadHandler threadHandler = null;

        MachineIfData machineIfData = null;

        public InspectRunner()
        {
            this.machineIfData = ((DeviceController)SystemManager.Instance().DeviceController).MachineIfMonitor.MachineIfData as MachineIfData;
        }

        public override bool EnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectRunner::EnterWaitInspection");
            //MachineIfData machineIfData = ((DeviceController)SystemManager.Instance().DeviceController).MachineIfMonitor.MachineIfData as MachineIfData;
            //string modelName = string.IsNullOrEmpty(machineIfData.GET_MODEL) ? "NoModel" : machineIfData.GET_MODEL;
            //string pasteName = string.IsNullOrEmpty(machineIfData.GET_MODEL) ? "NoPaste" : machineIfData.GET_PASTE;
            //string lotName = string.IsNullOrEmpty(machineIfData.GET_LOT) ? string.Format("{0}", DateTime.Now.ToString("yyMMdd_HHmmss")) : machineIfData.GET_LOT;

            //ModelDescription curModelDescription = SystemManager.Instance().CurrentModel?.ModelDescription;
            //if (curModelDescription == null || curModelDescription.Name != modelName || curModelDescription.Paste != pasteName)
            //{
            //    UniEye.Base.Data.ModelManager modelManager = SystemManager.Instance().ModelManager;
            //    ModelDescription modelDescription = modelManager.ModelDescriptionList.Find(f =>
            //    {
            //        ModelDescription g = f as ModelDescription;
            //        if (g == null)
            //            return false;

            //        return g.Name == modelName && g.Paste == pasteName;
            //    }) as ModelDescription;

            //    if (modelDescription == null)
            //    {
            //        modelDescription = modelManager.CreateModelDescription() as ModelDescription;
            //        modelDescription.Name = modelName;
            //        modelDescription.Paste = pasteName;
            //        modelDescription.IsTrained = true;

            //        modelManager.AddModel(modelDescription);
            //        modelManager.SaveModelDescription(modelDescription);
            //    }
            //    SystemManager.Instance().LoadModel(modelDescription);
            //    SystemManager.Instance().CurrentModel.Modified = true;
            //    SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
            //}

            this.processer = new TeachProcesser();

            return base.EnterWaitInspection();
        }

        public override bool PostEnterWaitInspection()
        {
            this.grabbedImageDic.Clear();
            this.termMsList.Clear();
            this.lastGrabTime = DateTime.MinValue;
            this.inspectionResultQ.Clear();

            this.threadHandler = new ThreadHandler("InspectRunnerUpdateThreadHandler", new Thread(UpdateProc), false);
            this.threadHandler.Start();

            return base.PostEnterWaitInspection();
        }

        public override void PreExitWaitInspection()
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectRunner::PreExitWaitInspection");
            base.PreExitWaitInspection();
            this.threadHandler.Stop();
        }

        protected override void PreInspect()
        {
            Processer processer = this.processer as Processer;
            SystemState.Instance().SetInspectState(processer.InspectState);

            //base.PreInspect();
        }

        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ImageGrabbed");

            ImageD imageD = imageDevice.GetGrabbedImage(ptr);
            Debug.Assert(imageD != null);

            CameraVirtualBufferTag cameraVirtualBufferTag = imageD.Tag as CameraVirtualBufferTag;
            int index = GetImageIndex(cameraVirtualBufferTag.FileName);
            DynMvp.ConsoleEx.WriteLine(string.Format("ImageGrabbed - DevName: {0}, FileName: {1}, index: {2}", imageDevice.Name, cameraVirtualBufferTag.FileName, index));
            InspectOption inspectionOption = this.inspectRunnerExtender.BuildInspectOption(imageDevice, (IntPtr)index) as InspectOption;

            DateTime now = DateTime.Now;
            if (this.processer.InspectState == InspectState.Run)
            {
                if (this.lastGrabTime != DateTime.MinValue)
                {
                    lock (this.termMsList)
                        this.termMsList.Add((now - this.lastGrabTime).TotalMilliseconds);
                }
            }
            this.lastGrabTime = now;

            lock (grabbedImageDic)
            {
                if (grabbedImageDic.Keys.ToList().Exists(f => f.ImageIndex == inspectionOption.ImageIndex))
                    StartInspect();

                grabbedImageDic.Add(inspectionOption, imageD);
                //Debug.WriteLine(string.Format("ImageGrabbed - grabbedImageDic Added. {0}", grabbedImageDic.Count));

                if (grabbedImageDic.Count >= 6)
                    StartInspect();
            }
        }

        private void StartInspect()
        {
            List<KeyValuePair<InspectOption, ImageD>> imageList = grabbedImageDic.ToList();
            grabbedImageDic.Clear();
            imageList.ForEach(f => DynMvp.ConsoleEx.WriteLine(string.Format("InspectRunner::StartInspect - index: {0}", f.Key.ImageIndex)));

            Task.Run(() =>
            {
                imageList.ForEach(f =>
                {
                    InspectionResult inspectionResult = BuildInspectionResult() as InspectionResult;
                    if (inspectionResult == null)
                        return;
                    //f.Key.TeachData = ((Model)SystemManager.Instance().CurrentModel).TeachData.Clone();
                    Inspect(f.Value, inspectionResult, f.Key);
                });
            });
        }

        private int GetImageIndex(string fileName)
        {
            string[] tokens = Path.GetFileNameWithoutExtension(fileName).Split('_');
            int cam = int.Parse(tokens[1].Substring(1));
            int type = int.Parse(tokens[2].Substring(1));
            int index = int.Parse(tokens[3].Substring(1));
            return cam * 3 + index;
            //return index * 2 + cam;
        }

        public void Inspect(ImageD imageD, InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectRunner::Inspect");
            inspectionResult.GrabImageList.Add(imageD.Clone());

            inspectRunnerExtender.OnPreInspection();

            this.PreInspect();
            this.processer.Process(imageD, inspectionResult, inspectionOption);
            this.PostInspect();

            inspectRunnerExtender.OnPostInspection();

            ProductInspected(inspectionResult);
        }

        protected override void PostInspect()
        {

        }

        Queue<DynMvp.InspData.InspectionResult> inspectionResultQ = new Queue<DynMvp.InspData.InspectionResult>();
        public override void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ProductInspected");
            Processer processer = this.processer as Processer;
            if (processer.InspectState == InspectState.Scan)
                inspectionResult.SetSkip();

            inspectionResultQ.Enqueue(inspectionResult);
            DynMvp.ConsoleEx.WriteLine(string.Format("InspectRunner::ProductInspected - ZoneIndex: {0}, inspectionResultQ.Count: {1}", ((InspectionResult)inspectionResult).ZoneIndex, inspectionResultQ.Count));

            this.Processer = this.Processer.GetNextProcesser();
        }

        private void UpdateProc()
        {
            while (!this.threadHandler.RequestStop)
            {
                if (inspectionResultQ.Count == 0)
                    continue;

                DynMvp.InspData.InspectionResult inspectionResult = inspectionResultQ.Dequeue();
                DynMvp.ConsoleEx.WriteLine(string.Format("InspectRunner::UpdateProc - inspectionResultQ.Count={0}", inspectionResultQ.Count));

                Stopwatch sw = Stopwatch.StartNew();

                base.ProductInspected(inspectionResult);

                double averageTermMs = 500;
                lock (this.termMsList)
                {
                    while (termMsList.Count > 50)
                    {
                        termMsList.RemoveAt(0);
                        //termMsList.Sort();
                        //double average = termMsList.Average();
                        //double a = average - termMsList.FirstOrDefault();
                        //double b = termMsList.LastOrDefault() - average;
                        //if (a > b)
                        //    termMsList.RemoveAt(0);
                        //else
                        //    termMsList.RemoveAt(termMsList.Count - 1);
                    }
                    averageTermMs = termMsList.Count > 0 ? termMsList.Average() * (1 - (this.inspectionResultQ.Count / 5) * 0.1) : 500;
                }

                sw.Stop();
                DynMvp.ConsoleEx.WriteLine(string.Format("InspectRunner::UpdateProc - SpandTime={0:F2}[ms]", sw.ElapsedMilliseconds));
                int sleepTime = Math.Max(0, (int)(averageTermMs - sw.ElapsedMilliseconds));
                DynMvp.ConsoleEx.WriteLine(string.Format("InspectRunner::UpdateProc - SleepTime={0:F2}[ms]", sleepTime));
                Thread.Sleep(sleepTime);
            }
        }
    }
}
