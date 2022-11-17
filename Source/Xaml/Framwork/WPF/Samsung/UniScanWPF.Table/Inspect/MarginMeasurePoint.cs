using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using UniScanWPF.Helper;
using UniScanWPF.Table.Data;
using WpfControlLibrary.Teach;

namespace UniScanWPF.Table.Inspect
{
    public class MarginMeasurePosList: System.Collections.ObjectModel.ObservableCollection<MarginMeasurePos>
    {
        public MarginMeasurePosList()        {        }

        public void EnableEvent()
        {
            this.CollectionChanged += MarginMeasurePosList_CollectionChanged;
        }

        private void MarginMeasurePosList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            InfoBox.Instance.CurrentModel.Modified = true;
            InfoBox.Instance.ModelChanged();
        }

        public MarginMeasurePosList FilterByFlow(int flowNo)
        {
            MarginMeasurePosList list = new MarginMeasurePosList();

            List < IGrouping<int, MarginMeasurePos>> ddd = this.GroupBy(f => f.FlowPosition).ToList();
            IGrouping<int, MarginMeasurePos> group = ddd.Find(f => f.Key == flowNo);
            if (group != null)
                Array.ForEach(group.ToArray(), f => list.Add(f));

            //var dd = this.GroupBy(f => f.FlowPosition).ToList();
            //for (int i = 0; i < this.Count; i++)
            //    if (this[i].FlowPosition == flowNo)
            //        list.Add(this[i]);
            return list;
        }
    }
    public class MarginMeasureParam
    {
        public bool ThresholdAuto { get; set; } = true;
        public int ThresholdM { get; set; } = 0;
        public int ThresholdA { get; set; } = 0;

        public System.Drawing.SizeF DesignedUm { get; set; } = System.Drawing.SizeF.Empty;
        public double JudgementSpecUm { get; set; } = 50;

        public int Threshold
        {
            get => ThresholdAuto ? ThresholdA : ThresholdM;
            set
            {
                if (!ThresholdAuto)
                    ThresholdM = value;
            }
        }

        internal void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "ThresholdAuto", this.ThresholdAuto);
            XmlHelper.SetValue(xmlElement, "ThresholdM", this.ThresholdM);
            XmlHelper.SetValue(xmlElement, "ThresholdA", this.ThresholdA);
            XmlHelper.SetValue(xmlElement, "DesignedUm", this.DesignedUm);
            XmlHelper.SetValue(xmlElement, "JudgementSpecUm", this.JudgementSpecUm);
        }

        internal void Load(XmlElement xmlElement)
        {
            this.ThresholdAuto = XmlHelper.GetValue(xmlElement, "ThresholdAuto", this.ThresholdAuto);
            this.ThresholdM = XmlHelper.GetValue(xmlElement, "ThresholdM", this.ThresholdM);
            this.ThresholdA = XmlHelper.GetValue(xmlElement, "ThresholdA", this.ThresholdA);
            this.DesignedUm = XmlHelper.GetValue(xmlElement, "DesignedUm", this.DesignedUm);
            this.JudgementSpecUm = XmlHelper.GetValue(xmlElement, "JudgementSpecUm", this.JudgementSpecUm);
        }

        internal void CoptFrom(MarginMeasureParam measureParam)
        {
            this.ThresholdAuto = measureParam.ThresholdAuto;
            this.ThresholdM = measureParam.ThresholdM;
            this.ThresholdA = measureParam.ThresholdA;
            this.DesignedUm = measureParam.DesignedUm;
            this.JudgementSpecUm = measureParam.JudgementSpecUm;
        }
    }

    public class MarginMeasurePos : ITeachModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsAlignable => !this.basePos[0].IsEmpty;

        public bool IsAdvAlignable => Array.TrueForAll(this.basePos, f => !f.IsEmpty);

        /// <summary>
        /// [0]: LB, [1]: LT, [2]: Loc
        /// </summary>
        public System.Drawing.Point[] BasePos { get => this.basePos; set => this.basePos = value; }
        System.Drawing.Point[] basePos;

        public string Name { get => this.name; set => this.name = value; }
        string name;

        public BitmapSource BgBitmapSource { get => this.bgBitmapSource; set => this.bgBitmapSource = value; }
        BitmapSource bgBitmapSource;

        public int FlowPosition { get => this.flowPosition; set => this.flowPosition = value; }
        int flowPosition;

        public System.Drawing.Rectangle Rectangle { get => this.rectangle; set => this.rectangle = value; }
        System.Drawing.Rectangle rectangle;

        public bool UseW { get => this.useW; set => this.useW = value; }
        bool useW;

        public bool UseL { get => this.useL; set => this.useL = value; }
        bool useL;

        public bool OverrideParam { get; set; }
        public MarginMeasureParam MeasureParam { get; private set; } = new MarginMeasureParam();

        public MarginMeasureSubPosCollection SubPosCollection => this.subPosCollection;
        MarginMeasureSubPosCollection subPosCollection = new MarginMeasureSubPosCollection();

        protected void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MarginMeasurePos()
        {
            this.basePos = new System.Drawing.Point[3];
            this.name = "";
            this.bgBitmapSource = null;
            this.flowPosition = -1;
            this.rectangle = System.Drawing.Rectangle.Empty;
            this.useW = true;
            this.useL = true;
            this.OverrideParam = false;
            this.MeasureParam = new MarginMeasureParam();
        }

        public MarginMeasurePos Clone(bool includeImage)
        {
            MarginMeasurePos marginMeasurePoint = new MarginMeasurePos();
            marginMeasurePoint.CopyFrom(this, includeImage);
            return marginMeasurePoint;
        }

        internal void Save(XmlElement xmlElement, string key = "")
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement, "");
                return;
            }

            XmlHelper.SetValue(xmlElement, "BasePos0", this.basePos[0]);
            XmlHelper.SetValue(xmlElement, "BasePos1", this.basePos[1]);
            XmlHelper.SetValue(xmlElement, "BasePos2", this.basePos[2]);
            XmlHelper.SetValue(xmlElement, "Name", this.name);
            XmlHelper.SetValue(xmlElement, "FlowPosition", this.flowPosition);
            XmlHelper.SetValue(xmlElement, "Rectangle", this.rectangle);
            XmlHelper.SetValue(xmlElement, "UseW", this.useW);
            XmlHelper.SetValue(xmlElement, "UseL", this.useL);

            XmlHelper.SetValue(xmlElement, "OverrideParam", this.OverrideParam);
            this.MeasureParam.Save(xmlElement);

            this.subPosCollection.Save(xmlElement, "SubPosCollection");

            if (this.bgBitmapSource != null)
            {
                string bitmapSourceString = Helper.WPFImageHelper.BitmapSoruceToBase64String(this.bgBitmapSource);
                XmlHelper.SetValue(xmlElement, "BitmapSource", bitmapSourceString);
            }
        }

        internal void Load(XmlElement xmlElement, string key = "")
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement[key];
                if (subElement != null)
                    Load(subElement);
                return;
            }

            if (XmlHelper.Exist(xmlElement, "BasePos"))
                this.basePos[0] = XmlHelper.GetValue(xmlElement, "BasePos", this.basePos[0]);
            else
            { 
                this.basePos[0] = XmlHelper.GetValue(xmlElement, "BasePos0", this.basePos[0]);
                this.basePos[1] = XmlHelper.GetValue(xmlElement, "BasePos1", this.basePos[1]);
                this.basePos[2] = XmlHelper.GetValue(xmlElement, "BasePos2", this.basePos[2]);
            }

            this.name = XmlHelper.GetValue(xmlElement, "Name", this.name);
            this.flowPosition = XmlHelper.GetValue(xmlElement, "FlowPosition", -1);
            this.rectangle = XmlHelper.GetValue(xmlElement, "Rectangle", System.Drawing.Rectangle.Empty);
            this.useW = XmlHelper.GetValue(xmlElement, "UseW", this.useW);
            this.useL = XmlHelper.GetValue(xmlElement, "UseL", this.useL);

            this.OverrideParam = XmlHelper.GetValue(xmlElement, "OverrideParam", this.OverrideParam);
            this.MeasureParam.Load(xmlElement);

            this.subPosCollection.Load(xmlElement, "SubPosCollection");

            string bitmapSourceString = XmlHelper.GetValue(xmlElement, "BitmapSource", "");
            if (!string.IsNullOrEmpty(bitmapSourceString))
                this.bgBitmapSource = Helper.WPFImageHelper.Base64StringToBitmapSoruce(bitmapSourceString);
        }

        internal static void Export(MarginMeasurePos marginMeasurePos, XmlElement xmlElement, string key = "")
        {
            marginMeasurePos.Save(xmlElement, key);
        }

        internal static MarginMeasurePos Import(XmlElement xmlElement, string key = "")
        {
            MarginMeasurePos marginMeasurePoint = new MarginMeasurePos();
            marginMeasurePoint.Load(xmlElement, key);
            return marginMeasurePoint;
        }

        internal void CopyFrom(MarginMeasurePos marginMeasurePos, bool includeImage)
        {
            this.name = marginMeasurePos.name;
            Array.Copy(marginMeasurePos.basePos, this.basePos, 3);
            if (includeImage)
            {
                this.bgBitmapSource = marginMeasurePos.bgBitmapSource.Clone();
                this.bgBitmapSource.Freeze();
            }

            this.flowPosition = marginMeasurePos.flowPosition;
            this.rectangle = marginMeasurePos.rectangle;

            this.OverrideParam = marginMeasurePos.OverrideParam;
            this.MeasureParam.CoptFrom(marginMeasurePos.MeasureParam);            

            this.subPosCollection = marginMeasurePos.subPosCollection.Clone();
        }

        public DrawObj[] GetDrawObjs()
        {
            DrawObj[] drawObjs = this.subPosCollection.Select(f => f.GetDrawObj()).ToArray();
            return drawObjs;
        }

        public ITeachProbe CreateProbe(Rect rect)
        {
            return new MarginMeasureSubPos(rect);
        }

        public ITeachProbe GetProbe(int idx)
        {
            return this.SubPosCollection[idx];
        }

        public void AddProbe(ITeachProbe probe)
        {
            this.SubPosCollection.Add((MarginMeasureSubPos)probe);
            OnPropertyChanged("SubPosCollection");
        }

        public void RemoveProbe(ITeachProbe probe)
        {
            this.SubPosCollection.Remove((MarginMeasureSubPos)probe);
            OnPropertyChanged("SubPosCollection");
        }

        public int GetAutoThresholdValue()
        {
            BitmapSource srcBitmapSource = this.BgBitmapSource;
            if (srcBitmapSource == null)
                return 0;

            System.Drawing.Size imageSize = new System.Drawing.Size(srcBitmapSource.PixelWidth, srcBitmapSource.PixelHeight);
            using (AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, imageSize))
            {
                byte[] bytes = WPFImageHelper.BitmapSourceToBytes(this.BgBitmapSource);
                algoImage.SetByte(bytes);
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
                int value = (int)ip.GetBinarizeValue(algoImage);
                return value;
            }
        }
    }

    public class MarginMeasureSubPosCollection: System.Collections.ObjectModel.ObservableCollection<MarginMeasureSubPos>
    {
        public MarginMeasureSubPosCollection Clone()
        {
            MarginMeasureSubPosCollection collection = new MarginMeasureSubPosCollection();
            for (int i = 0; i < this.Count; i++)
                collection.Add(this[i].Clone());
            return collection;
        }

        internal void Save(XmlElement xmlElement, string key = "")
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement, "");
                return;
            }

            for (int i = 0; i < this.Count; i++)
                this[i].Save(xmlElement, "SubPosition");
        }

        internal void Load(XmlElement xmlElement, string key = "")
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement[key];
                if (subElement != null)
                    Load(subElement);
                return;
            }

            XmlNodeList nodeList = xmlElement.GetElementsByTagName("SubPosition");
            foreach(XmlElement node in nodeList)
            {
                MarginMeasureSubPos marginMeasureSubPos = new MarginMeasureSubPos(Rect.Empty);
                marginMeasureSubPos.Load(node);
                this.Add(marginMeasureSubPos);
            }
        }

    }

    public class MarginMeasureSubPos : WpfControlLibrary.Teach.ITeachProbe
    {
        public Rect Rect
        {
            get => WpfControlLibrary.Helper.Converter.ToRect(this.rectangle);
            set => this.rectangle = WpfControlLibrary.Helper.Converter.ToRectangleF(value);
        }

        public System.Drawing.RectangleF Rectangle { get => this.rectangle; set => this.rectangle = value; }
        System.Drawing.RectangleF rectangle;

        public bool UseW { get => this.useW; set => this.useW = value; }
        bool useW;

        public bool UseL { get => this.useL; set => this.useL = value; }
        bool useL;

        public MarginMeasureSubPos(Rect rect)
        {
            this.rectangle = WpfControlLibrary.Helper.Converter.ToRectangleF(rect);
            this.useW = true;
            this.useL = true;
        }

        public MarginMeasureSubPos Clone()
        {
            MarginMeasureSubPos pos = new MarginMeasureSubPos(Rect.Empty);
            pos.CopyFrom(this);
            return pos;
        }

        public void CopyFrom(MarginMeasureSubPos pos)
        {
            this.rectangle = pos.rectangle;
            this.useW = pos.useW;
            this.useL = pos.useL;
        }

        internal void Save(XmlElement xmlElement, string key = "")
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement, "");
                return;
            }

            XmlHelper.SetValue(xmlElement, "Rectangle", this.rectangle);
            XmlHelper.SetValue(xmlElement, "UseW", this.useW);
            XmlHelper.SetValue(xmlElement, "UseL", this.useL);
        }
    
        internal void Load(XmlElement xmlElement, string key = "")
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement[key];
                if (subElement != null)
                    Load(subElement);
                return;
            }

            this.rectangle = XmlHelper.GetValue(xmlElement, "Rectangle", System.Drawing.RectangleF.Empty);
            this.useW = XmlHelper.GetValue(xmlElement, "UseW", this.useW);
            this.useL = XmlHelper.GetValue(xmlElement, "UseL", this.useL);
        }

        public DrawObj GetDrawObj()
        {
            Point pt = WpfControlLibrary.Helper.Converter.ToPoint(this.rectangle.Location);
            Size sz = WpfControlLibrary.Helper.Converter.ToSize(this.rectangle.Size);
            Rect rect = new Rect(pt, sz);

            DrawObj drawObj = new WpfControlLibrary.Teach.DrawObj(this);
            drawObj.GetGeometry += new GetGeometryDelegate(f =>
            {
                MarginMeasureSubPos marginMeasureSubPos = (MarginMeasureSubPos)f;
                double x = (f.Rect.Left + f.Rect.Right) / 2;
                double y = (f.Rect.Top + f.Rect.Bottom) / 2;
                double dx = (f.Rect.Width) / 4;
                double dy = (f.Rect.Height) / 4;
                double circleRad = Math.Min(f.Rect.Width, f.Rect.Height) / 8;
                GeometryGroup geometryGroup = new GeometryGroup();
                geometryGroup.Children.Add(new RectangleGeometry(marginMeasureSubPos.Rect));
                if (marginMeasureSubPos.useW)
                {
                    geometryGroup.Children.Add(new LineGeometry(new Point(f.Rect.Left + dx, y), new Point(f.Rect.Right - dx, y)));

                    geometryGroup.Children.Add(new LineGeometry(new Point(f.Rect.Left + dx, y - dy), new Point(f.Rect.Left + dx, y + dy)));
                    //geometryGroup.Children.Add(new LineGeometry(new Point(x, y - dy), new Point(x, y + dy)));
                    geometryGroup.Children.Add(new LineGeometry(new Point(f.Rect.Right - dx, y - dy), new Point(f.Rect.Right - dx, y + dy)));
                }

                if (marginMeasureSubPos.useL)
                {
                    geometryGroup.Children.Add(new LineGeometry(new Point(x, f.Rect.Top + dy), new Point(x, f.Rect.Bottom - dy)));

                    geometryGroup.Children.Add(new LineGeometry(new Point(x - dx, f.Rect.Top + dy), new Point(x + dx, f.Rect.Top + dy)));
                    //geometryGroup.Children.Add(new LineGeometry(new Point(x - dx, y), new Point(x + dx, y)));
                    geometryGroup.Children.Add(new LineGeometry(new Point(x - dx, f.Rect.Bottom - dy), new Point(x + dx, f.Rect.Bottom - dy)));
                }

                return geometryGroup;
            });
            return drawObj;
        }
    }
}
