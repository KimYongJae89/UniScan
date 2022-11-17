using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using UniScanWPF.Helper;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Operation.Operators;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;
using System.Drawing;

namespace UniScanWPF.Table.Data
{
    public class Model : DynMvp.Data.Model
    {
        public new ModelDescription ModelDescription => (ModelDescription)this.modelDescription;

        public int LightValueTop { get => lightValueTop; set => lightValueTop = value; }
        int lightValueTop;

        public int BinarizeValueTop { get => binarizeValueTop; set => binarizeValueTop = value; }
        int binarizeValueTop;

        public int BinarizeValueBack { get => binarizeValueBack; set => binarizeValueBack = value; }
        int binarizeValueBack;

        public ObservableCollection<PatternGroup> InspectPatternList { get => inspectPatternList; }
        ObservableCollection<PatternGroup> inspectPatternList;

        public ObservableCollection<PatternGroup> CandidatePatternList { get => candidatePatternList; }
        ObservableCollection<PatternGroup> candidatePatternList;

        public MarginMeasurePosList MarginMeasurePosList { get => marginMeasurePosList; }
        MarginMeasurePosList marginMeasurePosList;

        // 이게 최선입니까.....
        public MarginMeasurePosList MarginMeasurePosList0 { get => marginMeasurePosList.FilterByFlow(0); }
        public MarginMeasurePosList MarginMeasurePosList1 { get => marginMeasurePosList.FilterByFlow(1); }
        public MarginMeasurePosList MarginMeasurePosList2 { get => marginMeasurePosList.FilterByFlow(2); }
        public MarginMeasurePosList MarginMeasurePosList3 { get => marginMeasurePosList.FilterByFlow(3); }
        public MarginMeasurePosList MarginMeasurePosList4 { get => marginMeasurePosList.FilterByFlow(4); }
        public MarginMeasurePosList MarginMeasurePosList5 { get => marginMeasurePosList.FilterByFlow(5); }
        public MarginMeasurePosList MarginMeasurePosList6 { get => marginMeasurePosList.FilterByFlow(6); }
        public MarginMeasurePosList MarginMeasurePosList7 { get => marginMeasurePosList.FilterByFlow(7); }
        public MarginMeasurePosList MarginMeasurePosList8 { get => marginMeasurePosList.FilterByFlow(8); }
        public MarginMeasurePosList MarginMeasurePosList9 { get => marginMeasurePosList.FilterByFlow(9); }
        public MarginMeasurePosList MarginMeasurePosList10 { get => marginMeasurePosList.FilterByFlow(10); }
        public MarginMeasurePosList MarginMeasurePosList11 { get => marginMeasurePosList.FilterByFlow(11); }
        public MarginMeasurePosList MarginMeasurePosList12 { get => marginMeasurePosList.FilterByFlow(12); }
        public MarginMeasurePosList MarginMeasurePosList13 { get => marginMeasurePosList.FilterByFlow(13); }
        public MarginMeasurePosList MarginMeasurePosList14 { get => marginMeasurePosList.FilterByFlow(14); }

        public Model()
        {
            inspectPatternList = new ObservableCollection<PatternGroup>();
            candidatePatternList = new ObservableCollection<PatternGroup>();
            marginMeasurePosList = new MarginMeasurePosList();
        }

        public override bool IsTaught()
        {
            return inspectPatternList.Count + candidatePatternList.Count == 0 ? false : true;
        }

        public void SortPatternGroup()
        {
            List<PatternGroup> list = this.inspectPatternList.OrderByDescending(f => f.Count).ToList();
            for (int i = 0; i < list.Count; i++)
                this.inspectPatternList.Move(this.inspectPatternList.IndexOf(list[i]), i);

            List<PatternGroup> list2 = this.candidatePatternList.OrderByDescending(f => f.Count).ToList();
            for (int i = 0; i < list2.Count; i++)
                this.candidatePatternList.Move(this.candidatePatternList.IndexOf(list2[i]), i);

            //this.inspectPatternList = new ObservableCollection<PatternGroup>(this.inspectPatternList.OrderByDescending(f => f.Count));

            //this.candidatePatternList = new ObservableCollection<PatternGroup>(this.candidatePatternList.OrderByDescending(f => f.Count));
        }

        public void ModelTraind(OperatorResult operatorResult)
        {
            if (operatorResult.Type != ResultType.Train)
                return;

            TeachOperatorResult teachOperatorResult = (TeachOperatorResult)operatorResult;

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.inspectPatternList.Clear();
                this.candidatePatternList.Clear();

                foreach (PatternGroup pg in teachOperatorResult.InspectPatternList)
                    this.inspectPatternList.Add(pg);

                foreach (PatternGroup pg in teachOperatorResult.CandidatePatternList)
                    this.candidatePatternList.Add(pg);

                // 티칭이 끝나면 마진측정 영역을 새 좌표로 업데이트한다.
                ResultCombiner resultCombiner = SystemManager.Instance().OperatorManager.ResultCombiner;
                System.Drawing.Point newBasePos = resultCombiner.ExtractOperatorResultArray[0].VertexPoints[3];
                foreach (MarginMeasurePos pos in this.marginMeasurePosList)
                {
                    System.Drawing.Point diff = System.Drawing.Point.Subtract(newBasePos, new System.Drawing.Size(pos.BasePos[0]));
                    pos.BasePos[0] = DrawingHelper.Add(pos.BasePos[0], diff);
                    pos.BasePos[1] = DrawingHelper.Add(pos.BasePos[1], diff);
                    pos.BasePos[2] = DrawingHelper.Add(pos.BasePos[2], diff);
                    pos.Rectangle = DrawingHelper.Offset(pos.Rectangle, diff);

                    AlgoImage topImage = resultCombiner.ExtractOperatorResultArray[pos.FlowPosition].ScanOperatorResult.TopLightImage;
                    Rectangle clipRect = Rectangle.Intersect(pos.Rectangle, new Rectangle(System.Drawing.Point.Empty, topImage.Size));
                    if (clipRect.Width > 0 && clipRect.Height > 0)
                    {
                        using (AlgoImage clipImage = topImage.GetSubImage(clipRect))
                            pos.BgBitmapSource = clipImage.ToBitmapSource();
                    }
                    else
                        pos.BgBitmapSource = null;
                }
                //this.prevImage = teachOperatorResult.BitmapSource;

                ModelDescription.IsTeached = true;
                modified = true;
                //SimpleProgressWindow teachWindow = new SimpleProgressWindow(LocalizeHelper.GetString("Save"));
                //teachWindow.Show(() =>
                //{
                //    SystemManager.Instance().ModelManager.SaveModel(this);
                //});
            }));
        }

        public override void LoadModel(XmlElement xmlElement)
        {
            base.LoadModel(xmlElement);

            lightValueTop = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "LightValueTop", "0"));
            binarizeValueTop = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "BinarizeValueTop", "100"));
            binarizeValueBack = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "BinarizeValueBack", "100"));
            if (xmlElement.GetElementsByTagName("LightValue").Count > 0)
                lightValueTop = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "LightValue", "0"));

            if (binarizeValueTop == 0) binarizeValueTop = 100;
            if (binarizeValueBack == 0) binarizeValueBack = 100;

            // Load Margin Measure Point
            LoadMarginMeasureList(xmlElement);

            int index = 0;
            string inspectPath = Path.Combine(ModelPath, "Inspect");
            if (Directory.Exists(inspectPath))
            {
                foreach (XmlElement childElement in xmlElement)
                {
                    if (childElement.Name.Contains("Inspect"))
                    {
                        PatternGroup patternGroup = new PatternGroup();
                        patternGroup.Load(inspectPath, index++, childElement);

                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            inspectPatternList.Add(patternGroup);
                        }));
                    }
                }
            }

            index = 0;
            string candidatePath = Path.Combine(ModelPath, "Candidate");
            if (Directory.Exists(candidatePath))
            {
                XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("Candidate");
                foreach (XmlElement childElement in xmlElement)
                {
                    if (childElement.Name.Contains("Candidate"))
                    {
                        PatternGroup patternGroup = new PatternGroup();
                        patternGroup.Load(candidatePath, index++, childElement);

                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            candidatePatternList.Add(patternGroup);
                        }));
                    }
                }
            }
            SystemManager.Instance().OperatorManager.ResultCombiner.Clear();
        }


        private void LoadMarginMeasureList(XmlElement xmlElement)
        {
            //this.prevImage = WPFImageHelper.LoadBitmapSource(Path.Combine(ModelPath, "Prev.jpg"));

            this.marginMeasurePosList.Clear();
            if (xmlElement == null)
            {
                //this.marginMeasurePosList.Add(new MarginMeasurePos(ReferencePos.LT, new Point(30, 30)));
                //this.marginMeasurePosList.Add(new MarginMeasurePos(ReferencePos.RT, new Point(30, 30)));
                //this.marginMeasurePosList.Add(new MarginMeasurePos(ReferencePos.RB, new Point(30, 30)));
                //this.marginMeasurePosList.Add(new MarginMeasurePos(ReferencePos.LB, new Point(30, 30)));
                return;
            }

            XmlNodeList subElementList = xmlElement.GetElementsByTagName("Margin");
            foreach (XmlElement subElement in subElementList)
            {
                MarginMeasurePos marginMeasurePos = MarginMeasurePos.Import(subElement);
                if (marginMeasurePos.IsAdvAlignable)
                    this.marginMeasurePosList.Add(marginMeasurePos);
            }

            this.marginMeasurePosList.EnableEvent();
        }

        public override void SaveModel(XmlElement xmlElement)
        {
            base.SaveModel(xmlElement);

            //if (this.prevImage != null)
            //    WPFImageHelper.SaveBitmapSource(Path.Combine(ModelPath, "Prev.jpg"), this.prevImage);

            string imagePath = Path.Combine(ModelPath, "Inspect");

            XmlHelper.SetValue(xmlElement, "LightValueTop", lightValueTop.ToString());
            XmlHelper.SetValue(xmlElement, "BinarizeValueTop", binarizeValueTop.ToString());
            XmlHelper.SetValue(xmlElement, "BinarizeValueBack", binarizeValueBack.ToString());

            string inspectPath = Path.Combine(ModelPath, "Inspect");
            if (Directory.Exists(inspectPath) == false)
                Directory.CreateDirectory(inspectPath);

            for (int i = 0; i < this.inspectPatternList.Count; i++)
            {
                XmlElement patternGroupElement = xmlElement.OwnerDocument.CreateElement(string.Format("Inspect{0}", i));
                xmlElement.AppendChild(patternGroupElement);
                inspectPatternList[i].Save(inspectPath, i, patternGroupElement);
            }

            string candidatePath = Path.Combine(ModelPath, "Candidate");
            if (Directory.Exists(candidatePath) == false)
                Directory.CreateDirectory(candidatePath);

            for (int i = 0; i < this.candidatePatternList.Count; i++)
            {
                XmlElement patternGroupElement = xmlElement.OwnerDocument.CreateElement(string.Format("Candidate{0}", i));
                xmlElement.AppendChild(patternGroupElement);
                candidatePatternList[i].Save(candidatePath, i, patternGroupElement);
            }

            for (int i = 0; i < this.marginMeasurePosList.Count; i++)
            {
                XmlElement marginElement = xmlElement.OwnerDocument.CreateElement("Margin");
                xmlElement.AppendChild(marginElement);
                this.marginMeasurePosList[i].Save(marginElement);
            }
        }
    }

    public class ModelManager : UniEye.Base.Data.ModelManager
    {
        public override DynMvp.Data.Model CreateModel()
        {
            return new Model();
        }

        public ModelManager() : base()
        {
            Init(modelPath);
        }

        public override void Init(string modelPath)
        {
            base.Init(modelPath);

            this.modelPath = modelPath;
            try
            {
                this.Refresh();
            }
            catch (IOException ex)
            { }
        }

        public override DynMvp.Data.ModelDescription CreateModelDescription()
        {
            return new ModelDescription();
        }

        public bool IsModelExist(DynMvp.Data.ModelDescription modelDescription)
        {
            ModelDescription modelDescriptionG = (ModelDescription)modelDescription;

            foreach (ModelDescription m in modelDescriptionList)
            {
                if (m.Name == modelDescriptionG.Name &&
                    m.ScreenName == modelDescriptionG.ScreenName &&
                    m.MarginUm == modelDescriptionG.MarginUm &&
                    m.Thickness == modelDescriptionG.Thickness &&
                    m.Paste == modelDescriptionG.Paste)
                    return true;
            }

            return false;
        }

        public override string GetModelPath(DynMvp.Data.ModelDescription modelDescription)
        {
            ModelDescription modelDescriptionG = (ModelDescription)modelDescription;

            return Path.Combine(modelPath, modelDescription.Name, modelDescriptionG.Thickness.ToString(), modelDescriptionG.Paste);
        }

        public override void Refresh(string modelPath = null)
        {
            if (modelPath == null)
                modelPath = this.modelPath;

            bool exist = Directory.Exists(modelPath);
            DirectoryInfo modelRootDir = new DirectoryInfo(modelPath);
            if (modelRootDir.Exists == false)
            {
                Directory.CreateDirectory(modelPath);
                return;
            }

            modelDescriptionList.Clear();

            foreach (DirectoryInfo nameDirectory in modelRootDir.GetDirectories())
            {
                foreach (DirectoryInfo thicknessDir in nameDirectory.GetDirectories())
                {
                    foreach (DirectoryInfo pasteDir in thicknessDir.GetDirectories())
                    {
                        ModelDescription modelDescription = (ModelDescription)LoadModelDescription(pasteDir.FullName);
                        if (modelDescription == null)
                            continue;

                        modelDescriptionList.Add(modelDescription);

                        if (String.IsNullOrEmpty(modelDescription.Category) == false)
                            CategoryList.Add(modelDescription.Category);
                    }
                }
            }
        }

        public override void DeleteModel(DynMvp.Data.ModelDescription modelDescription)
        {
            ModelDescription modelDescriptionG = (ModelDescription)modelDescription;

            ModelDescription realMD = null;
            foreach (ModelDescription md in modelDescriptionList)
            {
                if (md.Name == modelDescriptionG.Name && md.Thickness == modelDescriptionG.Thickness && md.Paste == modelDescriptionG.Paste)
                    realMD = md;
            }

            if (realMD == null)
                return;

            modelDescriptionList.Remove(realMD);

            string firstPath = String.Format("{0}\\{1}", modelPath, realMD.Name);
            string middlePath = String.Format("{0}\\{1}", firstPath, realMD.Thickness);
            string lastPath = String.Format("{0}\\{1}", middlePath, realMD.Paste);

            if (Directory.Exists(lastPath) == true)
            {
                Directory.Delete(lastPath, true);

                DirectoryInfo middleInfo = new DirectoryInfo(middlePath);
                if (middleInfo.GetFiles().Length + middleInfo.GetDirectories().Length == 0)
                    Directory.Delete(middlePath, true);

                DirectoryInfo firstInfo = new DirectoryInfo(firstPath);
                if (firstInfo.GetFiles().Length + firstInfo.GetDirectories().Length == 0)
                    Directory.Delete(firstPath, true);
            }

            Refresh();
        }

        public override bool SaveModel(DynMvp.Data.Model model)
        {
            if (model == null)
                return false;

            model.ModelPath = GetModelPath((ModelDescription)model.ModelDescription);
            bool ok = base.SaveModel(model);
            if (ok)
                SystemManager.Instance().OperatorManager.SaveAll();
            return ok;
        }
    }

    public class ModelDescription : DynMvp.Data.ModelDescription
    {
        public string ScreenName { get => this.screenName; set => this.screenName = value; }
        string screenName;

        public SizeF MarginUm { get => this.marginUm; set => this.marginUm = value; }
        SizeF marginUm;
        
        public string MarginUmStr => $"W {marginUm.Width:F01}, L {marginUm.Height:F01}";

        public float MarginW { get => this.marginUm.Width; set => this.marginUm.Width = value; }
        public float MarginH { get => this.marginUm.Height; set => this.marginUm.Height = value; }

        float thickness;
        public float Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }

        string paste;
        public string Paste
        {
            get { return paste; }
            set { paste = value; }
        }

        bool isTeached;
        public bool IsTeached
        {
            get { return isTeached; }
            set { isTeached = value; }
        }

        public override bool Equals(object obj)
        {
            bool same = base.Equals(obj);
            if (same == false)
                return false;

            ModelDescription md = obj as ModelDescription;
            return thickness == md.thickness && paste == md.paste;
        }

        public override void Load(XmlElement modelDescElement)
        {
            base.Load(modelDescElement);

            name = XmlHelper.GetValue(modelDescElement, "Name", "DEFAULT NAME");
            screenName = XmlHelper.GetValue(modelDescElement, "ScreenName", "DEFAULT SCREEN");
            marginUm = XmlHelper.GetValue(modelDescElement, "MarginUm", SizeF.Empty);
            paste = XmlHelper.GetValue(modelDescElement, "Paste", "DEFAULT PASTE");
            thickness = XmlHelper.GetValue(modelDescElement, "Thickness", 0f);
            isTeached = XmlHelper.GetValue(modelDescElement, "IsTeached", false);

            if (marginUm.IsEmpty)
            {
                float margin = XmlHelper.GetValue(modelDescElement, "Margin", 0f);
                if (margin > 0)
                    marginUm = new SizeF(margin, margin);
            }
        }

        public override void Save(XmlElement modelDescElement)
        {
            base.Save(modelDescElement);

            XmlHelper.SetValue(modelDescElement, "Name", name);
            XmlHelper.SetValue(modelDescElement, "ScreenName", screenName);
            XmlHelper.SetValue(modelDescElement, "MarginUm", marginUm);
            XmlHelper.SetValue(modelDescElement, "Thickness", thickness);
            XmlHelper.SetValue(modelDescElement, "Paste", paste);
            XmlHelper.SetValue(modelDescElement, "IsTeached", isTeached);
        }

        public override DynMvp.Data.ModelDescription Clone()
        {
            ModelDescription discription = new ModelDescription();

            discription.Copy(this);

            return discription;
        }

        public override void Copy(DynMvp.Data.ModelDescription srcDesc)
        {
            base.Copy(srcDesc);
            ModelDescription md = (ModelDescription)srcDesc;
            screenName = md.screenName;
            marginUm = md.marginUm;
            thickness = md.thickness;
            paste = md.paste;
        }
    }
}
