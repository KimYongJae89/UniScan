using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanG.Data;
using UniScanG.Gravure.Device;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.MachineIF;

namespace UniScanG.Gravure.Data
{
    public class DefectCountGroup
    {
        int[] array;

        public string Key { get; private set; }

        public int Total => this.array.Sum();
        public int Transform => Get(DefectType.Transform);
        public int Sticker => Get(DefectType.Sticker);
        public int SheetAttack => Get(DefectType.Attack);
        public int NoPrint => Get(DefectType.Noprint);
        public int Dielectric => Get(DefectType.Coating);
        public int PinHole => Get(DefectType.PinHole);
        public int Spread => Get(DefectType.Spread);
        public int Margin => Get(DefectType.Margin);

        public DefectCountGroup(string key)
        {
            this.Key = key;

            int length = Enum.GetValues(typeof(DefectType)).Length - 2;
            this.array = new int[length];
            
            Clear();
        }

        public void Clear()
        {
            Array.Clear(this.array, 0, this.array.Length);
        }

        public void Add(DefectType type, int count)
        {
            int index = (int)type;
            if (index < 0 || index >= this.array.Length)
            {
                LogHelper.Error(LoggerType.Error, $"DefectCountGroup::Add - Out of range. type - {type} ({index})");
                return;
            }

            this.array[index] += count;
        }

        public int Get(DefectType type)
        {
            int index = (int)type;
            if (index < 0 || index >= this.array.Length)
            {
                LogHelper.Error(LoggerType.Error, $"DefectCountGroup::Get - Out of range. type - {type} ({index})");
                return 0;
            }

            return this.array[index];
        }

        public void Save(XmlElement xmlElement, string key = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            XmlHelper.SetValue(xmlElement, "NoPrint", this.NoPrint);
            XmlHelper.SetValue(xmlElement, "PinHole", this.PinHole);
            XmlHelper.SetValue(xmlElement, "Spread", this.Spread);
            XmlHelper.SetValue(xmlElement, "SheetAttack", this.SheetAttack);
            XmlHelper.SetValue(xmlElement, "Dielectric", this.Dielectric);
            XmlHelper.SetValue(xmlElement, "Sticker", this.Sticker);
            XmlHelper.SetValue(xmlElement, "Margin", this.Margin);
            XmlHelper.SetValue(xmlElement, "Transform", this.Transform);
        }

        public bool Load(XmlElement xmlElement, string key = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                return Load(subElement);
            }

            if (xmlElement == null)
                return false;

            Add(DefectType.Noprint, XmlHelper.GetValue(xmlElement, "NoPrint", 0));
            Add(DefectType.PinHole, XmlHelper.GetValue(xmlElement, "PinHole", 0));
            Add(DefectType.Spread, XmlHelper.GetValue(xmlElement, "Spread", 0));
            Add(DefectType.Attack, XmlHelper.GetValue(xmlElement, "SheetAttack", 0));
            Add(DefectType.Coating, XmlHelper.GetValue(xmlElement, "Dielectric", 0));
            Add(DefectType.Sticker, XmlHelper.GetValue(xmlElement, "Sticker", 0));
            Add(DefectType.Margin, XmlHelper.GetValue(xmlElement, "Margin", 0));
            Add(DefectType.Transform, XmlHelper.GetValue(xmlElement, "Transform", 0));
            return true;
        }
    }

    public class ProductionG : UniScanG.Data.Production
    {
        public int SubIndex { get; private set; }

        public int CriticalPatternNum { get; private set; }
        public DateTime CriticalPatternNumClear { get; private set; }

        public DefectCountGroup Patterns { get; } = new DefectCountGroup("Patterns");
        //public int TransformPatternNum { get; private set; }
        //public int StickerPatternNum { get; private set; }
        //public int SheetAttackPatternNum { get; private set; }
        //public int NoPrintPatternNum { get; private set; }
        //public int DielectricPatternNum { get; private set; }
        //public int PinHolePatternNum { get; private set; }
        //public int SpreadPatternNum { get; private set; }
        //public int MarginPatternNum { get; private set; }

        public DefectCountGroup Defects { get; } = new DefectCountGroup("Defects");
        //public int TransformNum { get; private set; }
        //public int StickerNum { get; private set; }
        //public int SheetAttackNum { get; private set; }
        //public int NoPrintNum { get; private set; }
        //public int DielectricNum { get; private set; }
        //public int PinHoleNum { get; private set; }
        //public int SpreadNum { get; private set; }
        //public int MarginNum { get; private set; }

        public DefectCountGroup Erased { get; } = new DefectCountGroup("Erased");

        public int EraseReq { get; private set; }
        public int EraseNum { get; private set; }
        public int EraseGood { get; private set; }

        public float SpecBlackUm { get; private set; }
        public float SpecWhiteUm { get; private set; }

        public string NgGrade { get; private set; }
        public string NoPrintGrade { get; private set; }
        public string PinHoleGrade { get; private set; }

        public int EraseDuplicated => Math.Max(0, this.Erased.Total - EraseNum);
        //public int EraseDuplicated => this.Erased.Total - EraseNum;

        public float NoPrintPatternRatio => this.Total == 0 ? 0 : this.Patterns.NoPrint * 100f / this.Total;
        public float PinholePatternRatio => this.Total == 0 ? 0 : this.Patterns.PinHole * 100f / this.Total;
        public float EraseGoodRatio => this.EraseNum == 0 ? 0 : this.EraseGood * 100f / this.EraseNum;
        public float EraseNgRatio => this.EraseNum == 0 ? 0 : (this.EraseNum - this.EraseGood) * 100f / this.EraseNum;

        public bool UpdateRequire { get; set; }

        public override string ResultPath => GetResultPath(PathSettings.Instance().Result);

        public ProductionG(string name, DateTime dateTime, string lotNo, RewinderZone rewinderZone, float thickness, string paste, int subIndex, float lineSpeedMpm)
            : base(name, dateTime, lotNo, rewinderZone, thickness, paste, lineSpeedMpm)
        {
            this.SubIndex = subIndex;

            this.CriticalPatternNumClear = dateTime;
            this.SpecBlackUm = float.NaN;
            this.SpecWhiteUm = float.NaN;

            if (lineSpeedMpm < 0)
                UpdateLineSpeedMpm();
        }

        public ProductionG(XmlElement productionElement) : base(productionElement) { }

        public void ResetCriticalPatternNum()
        {
            this.CriticalPatternNum = 0;
            this.CriticalPatternNumClear = DateTime.Now;
        }

        public override void Reset()
        {
            base.Reset();

            ResetCriticalPatternNum();

            this.Patterns.Clear();
            //this.TransformPatternNum = 0;
            //this.StickerPatternNum = 0;
            //this.SheetAttackPatternNum = 0;
            //this.NoPrintPatternNum = 0;
            //this.DielectricPatternNum = 0;
            //this.PinHolePatternNum = 0;
            //this.SpreadPatternNum = 0;
            //this.MarginPatternNum = 0;

            this.Defects.Clear();
            //this.TransformNum = 0;
            //this.StickerNum = 0;
            //this.SheetAttackNum = 0;
            //this.NoPrintNum = 0;
            //this.DielectricNum = 0;
            //this.PinHoleNum = 0;
            //this.SpreadNum = 0;
            //this.MarginNum = 0;

            this.Erased.Clear();

            this.EraseReq = 0;
            this.EraseNum = 0;
            this.EraseGood = 0;

            this.SpecBlackUm = float.NaN;
            this.SpecWhiteUm = float.NaN;

            this.NgGrade = "";
            this.NoPrintGrade = "";
            this.PinHoleGrade = "";
            //this.lineSpeedMpm = 0;
        }

        //public override bool Equals(object obj)
        //{
        //    bool eq = base.Equals(obj);
        //    if (eq)
        //    {
        //        ProductionG productionG = obj as ProductionG;
        //        eq = (this.thickness == productionG.Thickness && this.paste == productionG.Paste);
        //    }
        //    return eq;
        //}

        //public override int GetHashCode()
        //{
        //    return this.thickness.GetHashCode() ^ this.paste.GetHashCode();
        //}

        public override void Load(XmlElement productionElement)
        {
            base.Load(productionElement);

            this.SubIndex = XmlHelper.GetValue(productionElement, "SubIndex", 0);
            this.UpdateRequire = XmlHelper.GetValue(productionElement, "UpdateRequire", this.UpdateRequire);

            this.CriticalPatternNum = XmlHelper.GetValue(productionElement, "CriticalPatternNum", this.CriticalPatternNum);
            this.CriticalPatternNumClear = XmlHelper.GetValue(productionElement, "CriticalPatternNum", this.StartTime);

            if (!this.Patterns.Load(productionElement, this.Patterns.Key))
            {
                this.Patterns.Add(DefectType.Transform, XmlHelper.GetValue(productionElement, "TransformPatternNum", 0));
                this.Patterns.Add(DefectType.Sticker, XmlHelper.GetValue(productionElement, "StickerPatternNum", 0));
                this.Patterns.Add(DefectType.Attack, XmlHelper.GetValue(productionElement, "SheetAttackPatternNum", 0));
                this.Patterns.Add(DefectType.Noprint, XmlHelper.GetValue(productionElement, "NoPrintPatternNum", 0));
                this.Patterns.Add(DefectType.Coating, XmlHelper.GetValue(productionElement, "DielectricPatternNum", 0));
                this.Patterns.Add(DefectType.PinHole, XmlHelper.GetValue(productionElement, "PinHolePatternNum", 0));
                this.Patterns.Add(DefectType.Spread, XmlHelper.GetValue(productionElement, "SpreadPatternNum", 0));
                this.Patterns.Add(DefectType.Margin, XmlHelper.GetValue(productionElement, "MarginPatternNum", 0));
            }

            if (!this.Defects.Load(productionElement, this.Defects.Key))
            {
                this.Defects.Add(DefectType.Transform, XmlHelper.GetValue(productionElement, "TransformNum", 0));
                this.Defects.Add(DefectType.Sticker, XmlHelper.GetValue(productionElement, "StickerNum", 0));
                this.Defects.Add(DefectType.Attack, XmlHelper.GetValue(productionElement, "SheetAttackNum", 0));
                this.Defects.Add(DefectType.Noprint, XmlHelper.GetValue(productionElement, "NoPrintNum", 0));
                this.Defects.Add(DefectType.Coating, XmlHelper.GetValue(productionElement, "DielectricNum", 0));
                this.Defects.Add(DefectType.PinHole, XmlHelper.GetValue(productionElement, "PinHoleNum", 0));
                this.Defects.Add(DefectType.Spread, XmlHelper.GetValue(productionElement, "SpreadNum", 0));
                this.Defects.Add(DefectType.Margin, XmlHelper.GetValue(productionElement, "MarginNum", 0));
            }

            this.Erased.Load(productionElement, this.Erased.Key);
            this.EraseReq = XmlHelper.GetValue(productionElement, "EraseReq", 0);
            this.EraseNum = XmlHelper.GetValue(productionElement, "EraseNum", 0);
            this.EraseGood = XmlHelper.GetValue(productionElement, "EraseGood", 0);

            this.NgGrade = XmlHelper.GetValue(productionElement, "NgGrade", this.NgGrade);
            this.NoPrintGrade = XmlHelper.GetValue(productionElement, "NoPrintGrade", this.NoPrintGrade);
            this.PinHoleGrade = XmlHelper.GetValue(productionElement, "PinHoleGrade", this.PinHoleGrade);

            this.SpecBlackUm = XmlHelper.GetValue(productionElement, "SpecBlackUm", float.NaN);
            if (this.SpecBlackUm < 0)
                this.SpecBlackUm = float.NaN;

            this.SpecWhiteUm = XmlHelper.GetValue(productionElement, "SpecWhiteUm", float.NaN);
            if (this.SpecWhiteUm < 0)
                this.SpecWhiteUm = float.NaN;
        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);

            XmlHelper.SetValue(productionElement, "SubIndex", this.SubIndex);
            XmlHelper.SetValue(productionElement, "UpdateRequire", this.UpdateRequire);

            XmlHelper.SetValue(productionElement, "CriticalPatternNum", this.CriticalPatternNum);
            XmlHelper.SetValue(productionElement, "CriticalPatternNumClear", this.CriticalPatternNumClear);

            this.Defects.Save(productionElement, this.Defects.Key);
            //XmlHelper.SetValue(productionElement, "TransformNum", this.TransformNum);
            //XmlHelper.SetValue(productionElement, "StickerNum", this.Defects.Sticker.ToString());
            //XmlHelper.SetValue(productionElement, "SheetAttackNum", this.Defects.SheetAttack.ToString());
            //XmlHelper.SetValue(productionElement, "NoPrintNum", this.Defects.NoPrint.ToString());
            //XmlHelper.SetValue(productionElement, "DielectricNum", this.Defects.Dielectric.ToString());
            //XmlHelper.SetValue(productionElement, "PinHoleNum", this.Defects.PinHole.ToString());
            //XmlHelper.SetValue(productionElement, "SpreadNum", this.Defects.Spread.ToString());
            //XmlHelper.SetValue(productionElement, "MarginNum", this.Defects.MarginNum.ToString());

            this.Patterns.Save(productionElement, this.Patterns.Key);
            //XmlHelper.SetValue(productionElement, "TransformPatternNum", this.TransformPatternNum);
            //XmlHelper.SetValue(productionElement, "StickerPatternNum", this.Patterns.Sticker.ToString());
            //XmlHelper.SetValue(productionElement, "SheetAttackPatternNum", this.SheetAttackPatternNum.ToString());
            //XmlHelper.SetValue(productionElement, "NoPrintPatternNum", this.Patterns.NoPrint.ToString());
            //XmlHelper.SetValue(productionElement, "DielectricPatternNum", this.Patterns.Dielectric.ToString());
            //XmlHelper.SetValue(productionElement, "PinHolePatternNum", this.Patterns.PinHole.ToString());
            //XmlHelper.SetValue(productionElement, "SpreadPatternNum", this.Patterns.Spread.ToString());

            this.Erased.Save(productionElement, this.Erased.Key);

            XmlHelper.SetValue(productionElement, "EraseReq", this.EraseReq);
            XmlHelper.SetValue(productionElement, "EraseNum", this.EraseNum);
            XmlHelper.SetValue(productionElement, "EraseGood", this.EraseGood);

            XmlHelper.SetValue(productionElement, "NgGrade", this.NgGrade);
            XmlHelper.SetValue(productionElement, "NoPrintGrade", this.NoPrintGrade);
            XmlHelper.SetValue(productionElement, "PinHoleGrade", this.PinHoleGrade);

            XmlHelper.SetValue(productionElement, "SpecBlackUm", this.SpecBlackUm);
            XmlHelper.SetValue(productionElement, "SpecWhiteUm", this.SpecWhiteUm);
        }

        public override void Save(string xmlFilePath)
        {
            string copiedFIle = Path.Combine(xmlFilePath, DataCopier.FlagFileName);
            bool isCopied = File.Exists(copiedFIle);
            if (isCopied)
                xmlFilePath = File.ReadAllText(copiedFIle);

            base.Save(xmlFilePath);
        }

        public override void OnLoaded()
        {
            if (!this.localBackupDone)
            {
                // Production의 결과 경로(백업된 위치 포함)
                string resultPath = this.ResultPath;

                string copiedFIle = Path.Combine(resultPath, DataCopier.FlagFileName);
                bool isCopied = File.Exists(copiedFIle);
                if (isCopied)
                    resultPath = File.ReadAllText(copiedFIle);

                string productionFile = Path.Combine(resultPath, ProductionBase.XmlFileName);
                FileInfo fileInfo = new FileInfo(productionFile);

                // Production.xml 파일이 없으면 작성.
                // Production.xml 파일의 날짜가 안맞으면 새로 작성
                bool write = !fileInfo.Exists || (this.StartTime != fileInfo.CreationTime || this.lastUpdateTime != fileInfo.LastWriteTime);
                if (write)
                    Save(resultPath);
                this.localBackupDone = true;
            }
        }

        public void UpdateFrom(List<MergeSheetResult> mergeSheetResultList)
        {
            mergeSheetResultList.ForEach(f =>
            {
                AddTotal();
                if (!f.IsNG)
                {
                    AddGood();
                }
                else
                {
                    AddNG();
                    this.Update(f);
                }
            });

            string path = GetResultPath();
            DateTime createTime = Directory.GetCreationTime(path);

            if (mergeSheetResultList.Count > 0)
            {
                DateTime minDateTime = mergeSheetResultList.Min(f => f.StartTime);
                if (this.StartTime.Date == minDateTime.Date)
                {
                    this.StartTime = minDateTime;
                }
                else if (this.StartTime.Date == createTime.Date)
                {
                    this.StartTime = createTime;
                }

                DateTime maxDateTime = mergeSheetResultList.Max(f => f.StartTime + f.SpandTime);
                if (this.lastUpdateTime.Date == maxDateTime.Date)
                    this.lastUpdateTime = maxDateTime;
            }

            this.UpdateRequire = false;
        }

        public override void Update(InspectionResult inspectionResult)
        {
            if (inspectionResult == null)
            {
                AddPass();
                return;
            }

            lock (this)
            {
                if (inspectionResult.Judgment == Judgment.Accept)
                {
                    AddGood();
                }
                else
                {
                    AddNG();

                    if (inspectionResult.AlgorithmResultLDic.ContainsKey(SheetCombiner.TypeName))
                    // Controller
                    {
                        MergeSheetResult mergeSheetResult = inspectionResult.AlgorithmResultLDic[SheetCombiner.TypeName] as MergeSheetResult;
                        Update(mergeSheetResult);

                        if (mergeSheetResult.IsCritical)
                            this.CriticalPatternNum++;
                    }
                    else
                    // Inspector
                    {
                        if (inspectionResult.AlgorithmResultLDic.ContainsKey(CalculatorBase.TypeName))
                        {
                            CalculatorResult calculatorResult = inspectionResult.AlgorithmResultLDic[CalculatorBase.TypeName] as CalculatorResult;
                            Update(calculatorResult);
                        }

                        if (inspectionResult.AlgorithmResultLDic.ContainsKey(Detector.TypeName))
                        {
                            DetectorResult detectorResult = inspectionResult.AlgorithmResultLDic[Detector.TypeName] as DetectorResult;
                            Update(detectorResult);
                        }

                        if (inspectionResult.AlgorithmResultLDic.ContainsKey(Watcher.TypeName))
                        {
                            WatcherResult watcherResult = inspectionResult.AlgorithmResultLDic[Watcher.TypeName] as WatcherResult;
                            Update(watcherResult);
                        }
                    }
                }
                this.lastSequenceNo = Math.Max(this.lastSequenceNo, int.Parse(inspectionResult.InspectionNo));
            }
        }

        private void Update(AlgorithmResultG sheetResult)
        {
            // BuildInspectionResult 할 때 Add 했음.
            //AddTotal();

            List<FoundedObjInPattern> defectObjList = sheetResult.SheetSubResultList.FindAll(f => f.IsDefect);

            var groups = defectObjList.GroupBy(f => f.GetDefectType());
            foreach (var group in groups)
            {
                var key = group.Key;
                int count = group.Count();

                //LogHelper.Debug(LoggerType.Inspection, $"ProductionG::Update - key: {key}");
                //LogHelper.Debug(LoggerType.Inspection, $"ProductionG::Update - Patterns");
                this.Patterns.Add(key, 1);

                //LogHelper.Debug(LoggerType.Inspection, $"ProductionG::Update - Defects: {count}");
                this.Defects.Add(key, count);

                if (sheetResult.PostProcessed)
                {
                    //LogHelper.Debug(LoggerType.Inspection, $"ProductionG::Update - Erased");
                    this.Erased.Add(key, 1);
                }
            }

            //int transformNum = defectObjList.Count(f => f.GetDefectType() == DefectType.Transform);
            //if (transformNum > 0)
            //{
            //    this.Defects.Transform += transformNum;
            //    this.Patterns.Transform++;
            //}

            //int stickerNum = defectObjList.Count(f => f.GetDefectType() == DefectType.Sticker);
            //if (stickerNum > 0)
            //{
            //    this.Defects.Sticker += stickerNum;
            //    this.Patterns.Sticker++;
            //}

            //int sheetAttackNum = defectObjList.Count(f => f.GetDefectType() == DefectType.Attack);
            //if (sheetAttackNum > 0)
            //{
            //    this.Defects.SheetAttack += sheetAttackNum;
            //    this.Patterns.SheetAttack++;
            //}

            //int noPrintNum = defectObjList.Count(f => f.GetDefectType() == DefectType.Noprint);
            //if (noPrintNum > 0)
            //{
            //    this.Defects.NoPrint += noPrintNum;
            //    this.Patterns.NoPrint++;
            //}

            //int dielectricNum = defectObjList.Count(f => f.GetDefectType() == DefectType.Coating);
            //if (dielectricNum > 0)
            //{
            //    this.Defects.Dielectric += dielectricNum;
            //    this.Patterns.Dielectric++;
            //}

            //int pinHoleNum = defectObjList.Count(f => f.GetDefectType() == DefectType.PinHole);
            //if (pinHoleNum > 0)
            //{
            //    this.Defects.PinHole += pinHoleNum;
            //    this.Patterns.PinHole++;
            //}

            //int spreadNum = defectObjList.Count(f => f.GetDefectType() == DefectType.Spread);
            //if (spreadNum > 0)
            //{
            //    this.Defects.Spread += spreadNum;
            //    this.Patterns.Spread++;
            //}

            //int marginNum = defectObjList.Count(f => f.GetDefectType() == DefectType.Margin);
            //if (marginNum > 0)
            //{
            //    this.Defects.Margin += marginNum;
            //    this.Patterns.Margin++;
            //}
        }

        public void UpdateSpec(string[] filePaths)
        {
            List<SizeF> sizeList = new List<SizeF>();

            // W: BalckSpec, H: WhiteSpec
            foreach (string filePath in filePaths)
            {
                if (!File.Exists(filePath))
                    continue;

                DateTime lastWriteTime = File.GetLastWriteTime(filePath);
                bool sameDate = this.StartTime.Date == lastWriteTime.Date;
                if (sameDate && lastWriteTime < this.StartTime)
                    this.StartTime = lastWriteTime;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                XmlElement root = (XmlElement)xmlDoc["Root"];
                if (root == null)
                    continue;

                XmlElement detElement = root[Detector.TypeName];
                if (detElement == null)
                    continue;

                DetectorParam param = new DetectorParam(true);
                param.LoadParam(detElement);

                //DateTime teachTime = param.ModifiedDateTime;
                //if (teachTime < this.StartTime)
                //    this.StartTime = teachTime;

                sizeList.Add(new SizeF(param.MinBlackDefectLength, param.MinWhiteDefectLength));
            }

            if (sizeList.Count > 0)
            {
                this.SpecBlackUm = float.IsNaN(this.SpecBlackUm) ? sizeList.Min(f => f.Width) : Math.Min(this.SpecBlackUm, sizeList.Min(f => f.Width));
                this.SpecWhiteUm = float.IsNaN(this.SpecWhiteUm) ? sizeList.Min(f => f.Height) : Math.Min(this.SpecWhiteUm, sizeList.Min(f => f.Height));
            }

            this.lastUpdateTime = DateTime.Now;
        }

        public void UpdateLineSpeedMpm(float lineSpeedMpm)
        {
            this.LineSpeedMpm = lineSpeedMpm;
        }

        public void UpdateLineSpeedMpm()
        {
            // Get Line Speed
            MachineIfData machineIfData = (SystemManager.Instance().DeviceController as DeviceControllerG)?.MachineIfMonitor?.MachineIfData as MachineIfData;
            this.LineSpeedMpm = machineIfData == null ? 0 : machineIfData.GET_TARGET_SPEED_REAL;
        }

        public string GetResultPathOld()
        {
            return GetResultPathOld(PathSettings.Instance().Result);
            //string resultPath = Path.Combine(
            //    PathSettings.Instance().Result,
            //    this.StartTime.ToString("yy-MM-dd"),
            //    this.Name,
            //    this.Thickness.ToString(),
            //    this.Paste,
            //    this.LotNo);
            //return resultPath;
        }

        public string GetResultPathOld(string root)
        {
            string resultPath = Path.Combine(
                root,
                this.StartTime.ToString("yy-MM-dd"),
                this.Name,
                this.Thickness.ToString(),
                this.Paste,
                this.LotNo);

            return resultPath;
        }

        public override string GetModelPath()
        {
            ModelDescription md = SystemManager.Instance().ModelManager.GetModelDescription(this.name, this.thickness.ToString(), this.paste);
            if (md != null)
                return SystemManager.Instance().ModelManager.GetModelPath(md);
            return null;
        }

        public override string GetResultPath()
        {
            string resultPath = GetResultPath(PathSettings.Instance().Result);
            if (Directory.Exists(resultPath))
                return resultPath;

            return GetResultPathOld();
        }

        public override string GetResultPath(string root)
        {
            string resultPath = Path.Combine(
                root,
                this.StartTime.ToString("yy-MM-dd"),
                this.Name,
                this.Thickness.ToString(),
                this.Paste,
                this.SubIndex == 0 ? string.Format("{0}_{1}", this.LotNo, this.rewinderZone) : string.Format("{0}({1})_{2}", this.LotNo, this.SubIndex, this.rewinderZone));

            return resultPath;
        }

        public void IncreseEraseNum()
        {
            this.EraseNum++;
        }

        public void IncreseEraseGood()
        {
            this.EraseGood++;
        }

        public void IncreseEraseReq()
        {
            this.EraseReq++;
        }

        public void UpdateGrade()
        {
            UpdateGrade(
                Settings.AdditionalSettings.Instance().Grade,
                Settings.AdditionalSettings.Instance().GradeNP,
                Settings.AdditionalSettings.Instance().GradePH
                );
        }

        public void UpdateGrade(GradeSetting grade, GradeSetting gradeNP, GradeSetting gradePH)
        {
            if (!DynMvp.Base.LicenseManager.Exist("DecGrade"))
            {
                this.NgGrade = "";
                this.NoPrintGrade = "";
                this.PinHoleGrade = "";
                return;
            }

            if (this.Total == 0)
            {
                this.NgGrade = "-";
                this.NoPrintGrade = "-";
                this.PinHoleGrade = "-";
                return;
            }

            this.NgGrade = GetGrade(this.NgRatio, grade);
            this.NoPrintGrade = GetGrade(this.NoPrintPatternRatio, gradeNP);
            this.PinHoleGrade = GetGrade(this.PinholePatternRatio, gradePH);
        }

        private string GetGrade(double ratio, GradeSetting setting)
        {
            if (!setting.Use)
                return "-";

            if (ratio < setting.ScoreA)
                return "A";
            else if (ratio < setting.ScoreB)
                return "B";
            else if (ratio < setting.ScoreC)
                return "C";
            else
                return "D";
        }
    }
}
