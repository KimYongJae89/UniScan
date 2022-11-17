using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using DynMvp.UI;
using DynMvp.Base;
using DynMvp.Vision;
using DynMvp.Data;

namespace DynMvp.InspData
{
    public class ResultCount
    {
        public int numAccept;
        public int numReject;
        public int numFalseReject;
        public Judgment Judgment
        {
            get
            {
                if (numReject > 0)
                    return Judgment.Reject;
                else if (numFalseReject > 0)
                    return Judgment.FalseReject;
                else
                    return Judgment.Accept;
            }
        }

        public Dictionary<string, int> numTargetTypeDefects = new Dictionary<string, int>();
        public Dictionary<string, int> numTargetDefects = new Dictionary<string, int>();
    }

    public delegate void TargetInspectedDelegate(Target target, InspectionResult objectInspectionResult);
    public delegate void TargetGroupInspectedDelegate(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult);
    public delegate void InspectionStepInspectedDelegate(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult);

    public class TargetImage
    {
        string targetId;
        public string TargetId
        {
            get { return targetId; }
        }

        int lightTypeIndex;
        public int LightTypeIndex
        {
            get { return lightTypeIndex; }
        }

        Image2D image;
        public Image2D Image
        {
            get { return image; }
        }

        public TargetImage(string targetId, int lightTypeIndex, Image2D image)
        {
            this.targetId = targetId;
            this.lightTypeIndex = lightTypeIndex;
            this.image = image;
        }

        public void Clear()
        {
            if (image != null)
            {
                image.Dispose();
                image = null;
            }
        }
    }

    public class InspectionResult : IDisposable
    {
        string machineName;
        public string MachineName
        {
            get { return machineName; }
            set { machineName = value; }
        }

        string modelName;
        public string ModelName { get => modelName; set => modelName = value; }

        string lotNo;
        public string LotNo { get => lotNo; set => lotNo = value; }
        
        string inspectionNo;
        public string InspectionNo
        {
            get { return inspectionNo; }
            set { inspectionNo = value; }
        }

        //string inputBarcode;
        //public string InputBarcode
        //{
        //    get { return inputBarcode; }
        //    set { inputBarcode = value; }
        //}

        //string outputBarcode;
        //public string OutputBarcode
        //{
        //    get { return outputBarcode; }
        //    set { outputBarcode = value; }
        //}

        TimeSpan inspectionTime;
        public TimeSpan InspectionTime
        {
            get { return inspectionTime; }
            set { inspectionTime = value; }
        }

        DateTime inspectionStartTime;
        public DateTime InspectionStartTime
        {
            get { return inspectionStartTime; }
            set { inspectionStartTime = value; }
        }

        DateTime inspectionEndTime;
        public DateTime InspectionEndTime
        {
            get { return inspectionEndTime; }
            set { inspectionEndTime = value; }
        }

        TimeSpan exportTime;
        public TimeSpan ExportTime
        {
            get { return exportTime; }
            set { exportTime = value; }
        }
        
        public TimeSpan GetInspectionProcessTime()
        {
            return (inspectionEndTime - inspectionStartTime);
        }

        public static string GetInspectionNo()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        string jobOperator;
        public string JobOperator
        {
            get { return jobOperator; }
            set { jobOperator = value; }
        }

        string resultPath;
        public string ResultPath
        {
            get { return resultPath; }
            set { resultPath = value; }
        }

        string dbPath;
        public string DbPath
        {
            get { return dbPath; }
            set { dbPath = value; }
        }

        bool retryInspection;
        public bool RetryInspection
        {
            get { return retryInspection; }
            set { retryInspection = value; }
        }

        int repeatCount;
        public int RepeatCount
        {
            get { return repeatCount; }
            set { repeatCount = value; }
        }

        bool machineAlarm = false;
        public bool MachineAlarm
        {
            get { return machineAlarm; }
            set { machineAlarm = value; }
        }

        protected Judgment judgment = Judgment.Accept;
        public Judgment Judgment
        {
            get { return judgment; }
            set { judgment = value; }
        }

        int resultViewIndex;
        public int ResultViewIndex
        {
            get { return resultViewIndex; }
            set { resultViewIndex = value; }
        }

        bool resultSent = false;
        public bool ResultSent
        {
            get { return resultSent; }
            set { resultSent = value; }
        }

        protected List<ImageD> grabImageList = new List<ImageD>();
        public List<ImageD> GrabImageList
        {
            get { return grabImageList; }
            set { grabImageList = value; }
        }

        protected List<TargetImage> targetImageList = new List<TargetImage>();
        public List<TargetImage> TargetImageList
        {
            get { return targetImageList; }
            set { targetImageList = value; }
        }

        protected List<ProbeResult> probeResultList = new List<ProbeResult>();
        public ProbeResult this[int index]
        {
            get
            {
                if (index >= probeResultList.Count)
                    return null;

                return probeResultList[index];
            }

            //set { probeResultList[index] = value; }
        }

        public List<ProbeResult> ProbeResultList
        {
            get { return probeResultList; }
        }

        protected Dictionary<string, AlgorithmResult> algorithmResultDic = new Dictionary<string, AlgorithmResult>();
        public Dictionary<string, AlgorithmResult> AlgorithmResultLDic
        {
            get { return algorithmResultDic; }
        }

        protected List<int> targetGroupInspected = new List<int>();
        public List<int> TargetGroupInspected
        {
            get { return targetGroupInspected; }
            set { targetGroupInspected = value; }
        }

        protected Dictionary<string, object> extraResult = new Dictionary<string, object>();
        public Dictionary<string, object> ExtraResult
        {
            get { return extraResult; }
            set { extraResult = value; }
        }

        public virtual bool IsGood()
        {
            if (machineAlarm == true)
                return false;

            return this.judgment == Judgment.Accept || this.judgment == Judgment.FalseReject;

            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Judgment == Judgment.Reject)
                        return false;
                }
            }

            return true;
        }

        public virtual bool IsPass()
        {
            if (machineAlarm == true)
                return false;

            if (IsGood() == true)
                return false;

            lock(probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Judgment == Judgment.Reject)
                        return false;
                }
            }

            return true;
        }

        public bool DifferentProductDetected
        {
            get
            {
                lock(probeResultList)
                {
                    foreach (ProbeResult probeResult in probeResultList)
                    {
                        if (probeResult.DifferentProductDetected == true)
                            return true;
                    }
                }

                return false;
            }
        }


        bool stepBlockRequired;
        public bool StepBlockRequired
        {
            set { stepBlockRequired = value; }
            get
            {
                if (stepBlockRequired == true)
                    return true;

                lock(probeResultList)
                {
                    foreach (ProbeResult probeResult in probeResultList)
                    {
                        if (probeResult.Probe.StepBlocker == true && probeResult.Judgment == Judgment.Reject)
                            return true;
                    }
                }

                return false;
            }
        }

        protected bool imageDisposible = true;
        public bool ImageDisposible
        {
            get { return imageDisposible; }
            set { imageDisposible = value; }
        }

        ~InspectionResult()
        {
            Clear(false);
        }

        public void AddProbeResult(ProbeResult probeResult)
        {
            lock (probeResultList)
            {
                //ProbeResult oldProbeResult = GetProbeResult(probeResult.Probe);
                //if (oldProbeResult != null)
                //{
                //    probeResultList.Remove(oldProbeResult);
                //}

                probeResultList.Add(probeResult);
            }

            if (probeResult.Judgment == Judgment.Reject)
            {
                judgment = Judgment.Reject;
            }
        }

        public void AddProbeResult(InspectionResult inspectionResult)
        {
            lock(probeResultList)
            {
                foreach (ProbeResult probeResult in inspectionResult)
                {
                    AddProbeResult(probeResult);
                }
            }

            inspectionResult.ImageDisposible = false;
            foreach (TargetImage targetImage in inspectionResult.TargetImageList)
                targetImageList.Add(targetImage);
        }

        public virtual void SetGood()
        {
            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Judgment == Judgment.Reject)
                        probeResult.Judgment = Judgment.FalseReject;
                }
            }

            if (judgment == Judgment.Reject)
                judgment = Judgment.FalseReject;
        }

        public virtual void SetDefect()
        {
            judgment = Judgment.Reject;

            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Judgment == Judgment.FalseReject)
                        probeResult.Judgment = Judgment.Reject;
                }
            }
        }

        public virtual void SetSkip()
        {
            judgment = Judgment.Skip;
        }

        public virtual void AppendResultFigures(FigureGroup figureGroup, FigureDrawOption option = null)
        {
            if (option == null)
                option = FigureDrawOption.Default;

            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    probeResult.AppendResultFigures(figureGroup, false);
                }
            }
        }

        public void AddTargetImage(string targetId, int lightTypeIndex, Image2D image)
        {
            TargetImage targetImage = new TargetImage(targetId, lightTypeIndex, image);
            targetImageList.Add(targetImage);
        }

        public Image2D GetTargetImage(string targetId, int lightTypeIndex = 0)
        {
            foreach (TargetImage targetImage in targetImageList)
            {
                if (targetImage.TargetId == targetId && targetImage.LightTypeIndex == lightTypeIndex)
                //if (targetImage.TargetId == targetId && targetImage.LightTypeIndex == 1) //OLTT용
                    return targetImage.Image;
            }

            return null;
        }

        public virtual void Clear(bool clearImage = true)
        {
            probeResultList.Clear();
            extraResult.Clear();

            //if (clearImage)
            //{
            //    foreach (ImageD grabImage in grabImageList)
            //    {
            //        if (grabImage != null)
            //            grabImage.Dispose();
            //    }
            //}

            //if (imageDisposible)
            //{
            //    foreach (TargetImage targetImage in targetImageList)
            //    {
            //        targetImage.Clear();
            //    }
            //    targetImageList.Clear();
            //}
        }

        public int Count()
        {
            return probeResultList.Count;
        }

        public IEnumerator<ProbeResult> GetEnumerator()
        {
            return probeResultList.GetEnumerator();
        }

        public void BuildResultMessage(MessageBuilder totalResultMessage)
        {
            lock(probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    probeResult.BuildResultMessage(totalResultMessage);
                }
            }
        }

        public void GetTargetGroupResult(string inspectionStepName, int targetGroupId, InspectionResult targetGroupInspectionResult, bool includeAccept = false)
        {
            lock(probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Probe.Target.TargetGroup.InspectionStep.StepName == inspectionStepName &&
                        probeResult.Probe.Target.TargetGroup.GroupId == targetGroupId && (includeAccept || probeResult.Judgment != Judgment.Accept))
                    {
                        targetGroupInspectionResult.AddProbeResult(probeResult);
                    }
                }
            }
        }

        public bool IsDefected(Target target)
        {
            lock(probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Probe.Target == target)
                    {
                        if (probeResult.Judgment == Judgment.Reject)
                            return true;
                    }
                }
            }

            return false;
        }

        public bool IsDefected(Probe probe)
        {
            lock(probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Probe == probe)
                    {
                        if (probeResult.Judgment == Judgment.Reject)
                            return true;
                    }
                }
            }

            return false;
        }

        public bool IsPass(Target target)
        {
            int defectCount = 0;
            bool isPass = true;

            lock(probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Probe.Target == target)
                    {
                        if (probeResult.Judgment == Judgment.Accept)
                            continue;

                        if (probeResult.Judgment == Judgment.Reject)
                        {
                            isPass = false;
                        }

                        defectCount++;
                    }
                }
            }

            if (defectCount == 0)
                return false;

            return isPass;
        }

        public bool IsDefected()
        {
            return this.judgment == Judgment.Reject;

            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Judgment == Judgment.Reject)
                        return true;
                }
            }

            return false;
        }

        public bool IsDefected(string stepName, int targetGroupId)
        {
            lock(probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    TargetGroup targetGroup = probeResult.Probe.Target.TargetGroup;
                    if (targetGroup.GroupId == targetGroupId && targetGroup.InspectionStep.StepName == stepName)
                    {
                        if (probeResult.Judgment == Judgment.Reject)
                            return true;
                    }
                }
            }

            return false;
        }

        public bool IsInspected(string stepName, int targetGroupId)
        {
            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    TargetGroup targetGroup = probeResult.Probe.Target.TargetGroup;
                    if (targetGroup.GroupId == targetGroupId && targetGroup.InspectionStep.StepName == stepName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool GetResult(string name, out object value)
        {
            value = null;

            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Probe.Target.Name == name)
                    {
                        InspectionResult targetResult = GetTargetResult(name);
                        value = targetResult.IsGood();
                        return true;
                    }
                    else if (probeResult.Probe.FullId == name)
                    {
                        value = (probeResult.Judgment == Judgment.Accept);
                        return true;
                    }
                    else if (name.Contains(probeResult.Probe.FullId) == true)
                    {
                        string[] words = name.Split(new char[] { '.' });
                        string resultValueName = words[words.Count() - 1];

                        ProbeResultValue probeResultValue = probeResult.ResultValueList.Find(x => { return x.Name == resultValueName; });
                        if (probeResultValue != null)
                        {
                            value = probeResultValue.Value;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void ClearExtraResult()
        {
            extraResult.Clear();
        }

        public void AddExtraResult(string name, object value)
        {
            extraResult[name] = value;
        }

        public object GetExtraResult(string name)
        {
            object value;
            if (extraResult.TryGetValue(name, out value) == false)
                return null;

            return value;
        }

        public InspectionResult GetTargetResult(Target target)
        {
            InspectionResult targetResult = new InspectionResult();

            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Probe.Target == target)
                    {
                        targetResult.AddProbeResult(probeResult);
                    }
                }
            }

            return targetResult;
        }

        public InspectionResult GetTargetResult(string targetIdOrName)
        {
            InspectionResult targetResult = new InspectionResult();

            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Probe.Target.Name == targetIdOrName || probeResult.Probe.Target.FullId == targetIdOrName)
                    {
                        targetResult.AddProbeResult(probeResult);
                    }
                }
            }

            return targetResult;
        }

        public ProbeResult GetProbeResult(int probeIndex)
        {
            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Probe.Id == probeIndex)
                    {
                        return probeResult;
                    }
                }
            }

            return null;
        }

        public ProbeResult GetProbeResult(Probe probe)
        {
            if (probe == null)
                return null;

            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Probe == probe)
                    {
                        return probeResult;
                    }
                }
            }

            return null;
        }

        public ProbeResult GetProbeResult(string probeIdOrName)
        {
            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Probe.FullId == probeIdOrName || probeResult.Probe.Name == probeIdOrName)
                    {
                        return probeResult;
                    }
                }
            }

            return null;
        }

        public ProbeResult GetProbeResult(string probeIdOrName, bool OrLogic)
        {
            if (OrLogic == false)
                return null;
            List<ProbeResult> tempProbeResultList = new List<ProbeResult>();

            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Probe.FullId == probeIdOrName || probeResult.Probe.Name == probeIdOrName)
                    {
                        tempProbeResultList.Add(probeResult);
                    }
                }
            }

            if (tempProbeResultList.Count < 1)
            {
                return null;
            }

            if (tempProbeResultList.Count < 2)
            {
                ProbeResult probeResult = ProbeResultList[0];
                return probeResult;
            }


            foreach (ProbeResult probeResult in tempProbeResultList)
            {
                if (probeResult.Judgment == Judgment.Accept)
                    return probeResult;
            }
            foreach (ProbeResult probeResult in tempProbeResultList)
            {
                if (probeResult.Judgment == Judgment.Reject)
                    return probeResult;
            }
            tempProbeResultList.Clear();
            return null;            
        }

        public ProbeResult GetProbeResult(string targetIdOrName, int probeIndex)
        {
            InspectionResult targetInspectionResult = GetTargetResult(targetIdOrName);
            if (targetInspectionResult.Count() > 0)
            {
                return targetInspectionResult.GetProbeResult(probeIndex);
            }

            return null;
        }

        public void GetResultCount(ResultCount resultCount)
        {
            lock(probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Judgment == Judgment.Reject)
                    {
                        string typeName = probeResult.TargetType;
                        if (string.IsNullOrEmpty(typeName) == false)
                        {
                            int count;
                            if (resultCount.numTargetTypeDefects.TryGetValue(typeName, out count))
                            {
                                resultCount.numTargetTypeDefects[typeName]++;
                            }
                            else
                            {
                                resultCount.numTargetTypeDefects[typeName] = 1; ;
                            }
                        }

                        string targetName = probeResult.TargetName;
                        if (string.IsNullOrEmpty(targetName) == false)
                        {
                            int count;
                            if (resultCount.numTargetDefects.TryGetValue(targetName, out count))
                            {
                                resultCount.numTargetDefects[targetName]++;
                            }
                            else
                            {
                                resultCount.numTargetDefects[targetName] = 1;
                            }
                        }
                        resultCount.numReject++;
                    }
                    else if (probeResult.Judgment == Judgment.FalseReject)
                    {
                        resultCount.numFalseReject++;
                    }
                    else
                    {
                        resultCount.numAccept++;
                    }
                }
            }
        }

        public int GetTargetDefectCount()
        {
            ResultCount resultCount = new ResultCount();
            GetResultCount(resultCount);

            return resultCount.numTargetDefects.Count();
        }

        public int GetProbeDefectCount()
        {
            ResultCount resultCount = new ResultCount();
            GetResultCount(resultCount);

            return resultCount.numReject;
        }

        public virtual string GetSummaryMessage()
        {
            ResultCount resultCount = new ResultCount();
            GetResultCount(resultCount);

            string summaryMessage = "";

            if (machineAlarm == true)
            {
                summaryMessage += StringManager.GetString(this.GetType().FullName, "Machine Alarm Occurred.");
            }

            if (resultCount.numReject == 0)
            {
                summaryMessage = StringManager.GetString(this.GetType().FullName, "The product is Good");
            }
            else
            {
                summaryMessage = String.Format("Defect Targets : {0}", resultCount.numTargetDefects.Count());

                if (DifferentProductDetected)
                {
                    summaryMessage += " / " + StringManager.GetString(this.GetType().FullName, "Different Product is Detected");
                }

                if (StepBlockRequired)
                {
                    summaryMessage += " / " + StringManager.GetString(this.GetType().FullName, "Step Blocked. Check the product.");
                }
            }

            return summaryMessage;
        }

        public string GetResultString()
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateJudgement()
        {
            bool rejectFlag = false;
            Judgment = Judgment.Accept;

            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Judgment == Judgment.Reject)
                        rejectFlag = true;
                }
            }

            if (rejectFlag)
                Judgment = Judgment.Reject;
        }

        public virtual void Dispose()
        {
          
        }
    }
}
