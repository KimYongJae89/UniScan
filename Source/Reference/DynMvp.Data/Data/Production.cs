using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using DynMvp.Base;
using DynMvp.InspData;
using System.Text.RegularExpressions;

namespace DynMvp.Data
{
    public delegate void OnLotChangedDelegate();

    public abstract class ProductionManagerBase
    {
        public static string XmlFileName => "ProductionList.xml";

        object savelock = new object();
        string defaultPath = "";
        protected ProductionBase curProduction = null;
        public ProductionBase CurProduction
        {
            get { return curProduction; }
        }

        protected List<ProductionBase> list = new List<ProductionBase>();
        public List<ProductionBase> List
        {
            get { return list; }
        }

        public event OnLotChangedDelegate OnLotChanged = null;

        public string DefaultPath { get => defaultPath; }

        public ProductionManagerBase(string defaultPath)
        {
            this.defaultPath = defaultPath;
            //this.Load(defaultPath);
        }

        public List<ProductionBase> FindAll(string namePattern, string lotPattern, DateTime startTime, DateTime endTime)
        {
            if (string.IsNullOrEmpty(namePattern))
                namePattern = "*";
            String patternName = "^" + Regex.Escape(namePattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
            Regex regexName = new Regex(patternName);

            if (string.IsNullOrEmpty(lotPattern))
                lotPattern = "*";
            String patternLot = "^" + Regex.Escape(lotPattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
            Regex regexLot = new Regex(patternLot);
            //return list.FindAll(f => regexName.IsMatch(f.Name) && regexLot.IsMatch(f.LotNo) && startTime < f.StartTime && f.StartTime <= endTime);
            return list.FindAll(f => 
            {
                bool a = regexName.IsMatch(f.Name);
                bool b = regexLot.IsMatch(f.LotNo);
                bool c = startTime <= f.StartTime.Date && f.StartTime.Date <= endTime;
                return a && b && c;
            });
        }

        public void Clear()
        {
            lock (this.list)
            {
                this.curProduction = null;
                list.Clear();
            }
        }

        public bool LotExist(Model model, string lotNo)
        {
            return GetProduction(model.ModelDescription, lotNo) != null;
        }

        public bool LotExist(Model model, DateTime date, string lotNo)
        {
            ProductionBase production = GetProduction(model.ModelDescription, date, lotNo);
            return production != null;
        }

        public virtual ProductionBase LotChange(Model model, string lotNo)
        {
            LogHelper.Debug(LoggerType.Operation, string.Format("ProductionManager::LotChange. {0}", lotNo));

            ProductionBase production = GetProduction(model.ModelDescription, lotNo);
            if (production == null)
                production = BuildProduction(model, lotNo);

            LotChange(production);

            return curProduction;
        }

        protected void LotChange(ProductionBase productionBase)
        {
            if (curProduction != productionBase)
            {
                curProduction = productionBase;
                OnLotChanged?.Invoke();
            }
        }

        public ProductionBase LotChange(Model model, DateTime date, string lotNo)
        {
            LogHelper.Debug(LoggerType.Operation, string.Format("ProductionManager::LotChange. {0}", lotNo));

            ProductionBase production = GetProduction(model.ModelDescription, date, lotNo);
            if (production == null)
                production = BuildProduction(model, lotNo);

            LotChange(production);

            return curProduction;
        }

        protected ProductionBase BuildProduction(Model model, string lotNo)
        {
            ProductionBase production = CreateProduction(model, lotNo);
            lock (this.list)
                this.list.Add(production);
            this.Save();

            return production;
        }

        public abstract ProductionBase CreateProduction(Model model, string lotNo);
        //{
        //    return BuildProduction(model, lotNo);
        //}

        public abstract ProductionBase CreateProduction(XmlElement productionElement);
        //{
        //    return BuildProduction(productionElement);
        //}

        public bool RemoveProduction(ProductionBase production)
        {
            bool removed;
            lock (this.list)
                removed = this.list.Remove(production);

            if (removed)
                this.Save();

            return removed;
        }

        public virtual ProductionBase[] GetProductions(ModelDescription modelDescription)
        {
            return list.FindAll(p => p.Name == modelDescription.Name).ToArray();
        }

        public virtual ProductionBase GetProduction(ModelDescription modelDescription, string lotNo)
        {
            return list.FindLast(p => p.Name == modelDescription.Name && p.LotNo == lotNo);
        }

        public virtual ProductionBase GetProduction(ModelDescription modelDescription, DateTime date, string lotNo)
        {
            return list.Find(p => p.Name == modelDescription.Name && p.StartTime.Date == date.Date && p.LotNo == lotNo);
        }

        public virtual ProductionBase GetLastProduction(Model model)
        {
            return list.LastOrDefault(p => p.Name == model.Name);
        }

        public virtual ProductionBase GetLastProduction()
        {
            return list.LastOrDefault();
        }

        public virtual ProductionBase GetLastProduction(Func<ProductionBase, bool> func)
        {
            return list.LastOrDefault(func);
        }

        public int GetTodayCount()
        {
            return list.Count(f=>f.StartTime.DayOfYear == DateTime.Today.DayOfYear);
        }

        public void Load(string filePath = "", string fileName = "ProductionList.xml")
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = this.defaultPath;

            list.Clear();

            string fullFileName = Path.Combine(filePath, fileName);

            if (File.Exists(fullFileName) == false)
            {
                string oldFileName = Path.Combine(filePath, "ProductionList.csv");
                if (File.Exists(oldFileName))
                    File.Move(oldFileName, fullFileName);
            }

            if (!File.Exists(fullFileName))
                return;

            try
            {
                XmlDocument xmlDocument = XmlHelper.Load(fullFileName);
                XmlElement productionListElement = xmlDocument.DocumentElement;
                XmlNodeList xmlNodeList = productionListElement.GetElementsByTagName("Production");
                foreach (XmlElement productionElement in xmlNodeList)
                {
                    ProductionBase production = CreateProduction(productionElement);
                    production?.OnLoaded();

                    if (list.Exists(f => f.Equals(production)) == false)
                        list.Add(production);
                }

                if (list.Count > 0)
                    list = list.OrderBy(f => f.StartTime).ToList();
            }
            catch (Exception ex)
            {
                // 로드 실패시 프로덕션 메니지 백업
                LogHelper.Error(LoggerType.Error, string.Format("ProductionManager::Load -[{0}] {1}", ex.GetType(), ex.Message));
                if (File.Exists(fullFileName))
                    Archive(fullFileName);
            }
        }

        public void Save(string filePath = "", string fileName = "ProductionList.xml")
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = this.defaultPath;

            string extention = Path.GetExtension(fileName).ToLower();
            if (extention != ".xml")
                throw new Exception("Extension is not [.xml]");

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement productionListElement = xmlDocument.CreateElement("", "ProductionList", "");
            xmlDocument.AppendChild(productionListElement);

            lock (list)
            {
                list.ForEach(f =>
                {
                    try
                    {
                        if (f != null)
                        {
                            lock (f)
                            {
                                XmlElement productionElement = xmlDocument.CreateElement("", "Production", "");
                                productionListElement.AppendChild(productionElement);

                                f.Save(productionElement);
                            }
                        }
                    }catch(Exception ex)
                    {
                        LogHelper.Error(LoggerType.Error, string.Format("ProductionManager::Save1 - {0}", ex.Message));
                    }
                });
            }

            string fullFileName = Path.Combine(filePath, fileName);
            string tempFileName = Path.Combine(filePath, Path.ChangeExtension(fileName, ".xm_"));
            string backupFileName = Path.Combine(filePath, Path.ChangeExtension(fileName, ".xml.bak"));

            lock (savelock)
            {
                try
                {
                    xmlDocument.Save(tempFileName);
                    FileHelper.SafeSave(tempFileName, backupFileName, fullFileName);
                }
                catch (Exception ex)
                {
                    // 저장 실패시 기존 파일 백업
                    LogHelper.Error(LoggerType.Error, string.Format("ProductionManager::Save2 - {0}", ex.Message));
                    Archive(fullFileName);
                }
            }
        }

        private void Archive(string fileName)
        {
            if (File.Exists(fileName))
            {
                string path = Path.GetDirectoryName(fileName);
                string name = Path.GetFileNameWithoutExtension(fileName);
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                string ext = Path.GetExtension(fileName);
                string copyFileName = Path.Combine(path, string.Format("{0}.{1}.{2}", name, date, ext));
                if (File.Exists(copyFileName))
                    File.Delete(copyFileName);
                File.Copy(fileName, copyFileName);
            }
        }
    }
    
    public abstract class ProductionBase
    {
        public static string XmlFileName => "Production.xml";

        protected string name;
        public string Name
        {
            get { return name; }
            //set { name = value; }
        }

        protected string lotNo;
        public string LotNo
        {
            get { return lotNo; }
            //set { lotNo = value; }
        }

        protected int lastSequenceNo = -1;
        public int LastSequenceNo
        {
            get { return lastSequenceNo; }
            set { lastSequenceNo = value; }
        }

        public bool UserRemoveFlag { get => this.userRemoveFlag; set => this.userRemoveFlag = value; }
        protected bool userRemoveFlag = false;

        public bool LocalBackupDone { get => this.localBackupDone; set => this.localBackupDone = value; }
        protected bool localBackupDone = false;

        public double NgRatio
        {
            get { return Done == 0 ? 0 : (ng * 100.0f / Done); }
        }

        public double GoodRatio
        {
            get { return Done == 0 ? 0 : (good * 100.0f / Done); }
        }

        public double PassRatio
        {
            get { return Done == 0 ? 0 : (pass * 100.0f / Done); }
        }

        private int total;
        public int Total
        {
            get { return total; }
            //set { ng = value; }
        }

        public int Done { get => (good + ng + pass); }
        private int ng;
        public int Ng
        {
            get { return ng; }
            //set { ng = value; }
        }

        private int pass;
        public int Pass
        {
            get { return pass; }
            //set { pass = value; }
        }

        private int good;
        public int Good
        {
            get { return good; }
            //set { good = value; }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = lastUpdateTime = value; }
        }

        protected DateTime lastUpdateTime;
        public DateTime LastUpdateTime
        {
            get { return lastUpdateTime; }
            set { lastUpdateTime = value; }
        }

        public abstract string ResultPath { get; }

        public ProductionBase(string name, DateTime dateTime, string lotNo)
        {
            this.name = name;
            this.lotNo = lotNo;

            StartTime = dateTime;
            //Reset();
        }

        public ProductionBase(XmlElement productionElement)
        {
            Load(productionElement);
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode() ^ this.lotNo.GetHashCode() ^ this.startTime.Date.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            ProductionBase production = obj as ProductionBase;
            if (production == null)
                return false;

            //return this.name == production.name &&
            //    this.lotNo == production.lotNo &&
            //    (this.startTime == DateTime.MinValue ? true : this.startTime.Date == production.startTime.Date);
            return this.name == production.name && this.lotNo == production.lotNo;
        }

        public virtual void Reset()
        {
            total = good = ng = pass = 0;
            lastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 비동기 검사에서, 검사 진행 중 다음 검사 요청시 InspectionNo가 중복될 수 있음 -> Total 카운트 따로 관리해야 함.
        /// </summary>
        public void AddTotal()
        {
            if (IsDateChanged() && Configuration.AutoResetProductionCount)
            {
                Reset();
            }

            total++;

            lastUpdateTime = DateTime.Now;
        }

        public void AddNG()
        {
            if (IsDateChanged() && Configuration.AutoResetProductionCount)
            {
                Reset();
            }

            ng++;

            lastUpdateTime = DateTime.Now;
        }

        public void AddPass()
        {
            if (IsDateChanged() && Configuration.AutoResetProductionCount)
            {
                Reset();
            }

            pass++;

            lastUpdateTime = DateTime.Now;
        }

        public void AddGood()
        {
            if (IsDateChanged() && Configuration.AutoResetProductionCount)
            {
                Reset();
            }

            good++;

            lastUpdateTime = DateTime.Now;
        }

        public bool IsDateChanged()
        {
            return (lastUpdateTime.Day != DateTime.Now.Day);
        }

        public virtual void Load(XmlElement productionElement)
        {
            this.name = XmlHelper.GetValue(productionElement, "Name", "");
            this.lotNo = XmlHelper.GetValue(productionElement, "LotNo", "");

            this.startTime = XmlHelper.GetValue(productionElement, "StartTime", DateTime.Now);
            this.userRemoveFlag = XmlHelper.GetValue(productionElement, "WillBeDelete", this.userRemoveFlag);
            this.localBackupDone = XmlHelper.GetValue(productionElement, "LocalBackupDone", this.localBackupDone);

            this.total = XmlHelper.GetValue(productionElement, "Total", 0);
            this.good = XmlHelper.GetValue(productionElement, "Good", 0);
            this.ng = XmlHelper.GetValue(productionElement, "Ng", 0);
            this.pass = XmlHelper.GetValue(productionElement, "Pass", 0);
            this.lastUpdateTime = XmlHelper.GetValue(productionElement, "LastUpdateTime", DateTime.Now);
            this.lastSequenceNo = XmlHelper.GetValue(productionElement, "LastSequenceNo", this.total);

            if (this.lastSequenceNo < 0)
                this.lastSequenceNo = total;
        }

        public virtual void Save(XmlElement productionElement)
        {
            XmlHelper.SetValue(productionElement, "Name", name);
            XmlHelper.SetValue(productionElement, "LotNo", lotNo);
            XmlHelper.SetValue(productionElement, "StartTime", startTime);
            XmlHelper.SetValue(productionElement, "WillBeDelete", userRemoveFlag);
            XmlHelper.SetValue(productionElement, "LocalBackupDone", this.localBackupDone);

            XmlHelper.SetValue(productionElement, "Total", total.ToString());
            XmlHelper.SetValue(productionElement, "Good", good.ToString());
            XmlHelper.SetValue(productionElement, "Ng", ng.ToString());
            XmlHelper.SetValue(productionElement, "Pass", pass.ToString());
            XmlHelper.SetValue(productionElement, "LastUpdateTime", lastUpdateTime);
            XmlHelper.SetValue(productionElement, "LastSequenceNo", lastSequenceNo.ToString());
        }

        public void Save()
        {
            Save(this.ResultPath);
        }

        public virtual void Save(string xmlFilePath)
        {
            if (!Directory.Exists(xmlFilePath))
                return;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlElement xmlElement = XmlHelper.RootElement(xmlDocument, "Production");

                lock (this)
                    Save(xmlElement);

                string xmlFile = Path.Combine(xmlFilePath, "Production.xml");
                xmlDocument.Save(xmlFile);
                File.SetCreationTime(xmlFile, this.startTime);
                File.SetLastWriteTime(xmlFile, this.lastUpdateTime);
                File.SetLastAccessTime(xmlFile, this.lastUpdateTime);

                this.localBackupDone = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("ProductionBase::Save - Model {0}, Lot {1}, Message {2}", this.name, this.lotNo, ex.Message));
            }
        }

        public void Load(string xmlFilePath)
        {
            if (!File.Exists(xmlFilePath))
                return;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);

            XmlElement rootElement = XmlHelper.RootElement(xmlDocument, "Production");

            Load(rootElement);
            LocalBackupDone = true;
        }

        public virtual void OnLoaded()
        {

        }

        public abstract string GetModelPath();
        public abstract string GetResultPath();
        public abstract void Update(InspectionResult inspectionResult);
    }
}

