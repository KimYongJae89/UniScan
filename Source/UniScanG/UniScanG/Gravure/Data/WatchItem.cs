using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.Vision;

namespace UniScanG.Gravure.Data
{
    public enum ExtType { NONE, CHIP, FP, INDEX, StopIMG, Margin, PMVariance }
    public abstract class ExtItem : IComparable
    {
        public virtual bool IsFixed => false;
        public bool IsLicenseExist => DynMvp.Base.LicenseManager.Exist(this.LicenseKey);

        public string LicenseKey { get; private set; }

        public int Index { get; set; }

        public int ContainerIndex { get; set; }

        public bool Use { get => this.use && IsLicenseExist; set => this.use = value; }
        bool use;

        public string Name { get; set; }

        public ExtType ExtType { get; set; }

        public RectangleF MasterRectangleUm { get; protected set; }

        public RectangleF ClipRectangleUm { get; protected set; }

        public ImageD MasterImageD { get; set; }

        protected abstract ExtItem CloneItem();

        public static Color GetBgColor(ExtType watchType)
        {
            Color color = Color.Red;
            switch (watchType)
            {
                case ExtType.CHIP:
                    color = Color.Yellow;
                    break;

                case ExtType.FP:
                    color = Color.LightGreen;
                    break;

                case ExtType.INDEX:
                    color = Color.LightBlue;
                    break;

                case ExtType.StopIMG:
                    color = Color.Yellow;
                    break;

                case ExtType.Margin:
                    color = Color.Orange;
                    break;

                case ExtType.PMVariance:
                    color = Color.LightPink;
                    break;
            }
            return color;
        }

        public ExtItem(ExtType extType, DynMvp.Base.LicenseManager.ELicenses licenseKey) : this(extType, licenseKey.ToString()) { }

        public ExtItem(ExtType extType, string licenseKey)
        {
            this.ExtType = extType;
            this.LicenseKey = licenseKey;
            this.Name = this.ExtType.ToString();
            this.use = false;
            this.ContainerIndex = 0;
        }

        public void Dispose()
        {
            this.MasterImageD?.Dispose();
            this.MasterImageD = null;
        }

        public void SetMasterRectangleUm(RectangleF masterRectangleUm)
        {
            MasterRectangleUm = masterRectangleUm;
            UpdateClipRectangle();
        }

        public void Offset(PointF ptR)
        {
            this.ClipRectangleUm.Offset(ptR);
            this.MasterRectangleUm.Offset(ptR);
        }

        public void Offset(float x, float y)
        {
            this.ClipRectangleUm.Offset(x, y);
            this.MasterRectangleUm.Offset(x, y);
        }

        public ExtItem Clone()
        {
            ExtItem extItem = CloneItem();
            extItem.CopyFrom(this);
            return extItem;
        }

        public Figure GetFigure(OffsetStructSet offsetStructSet, Calibration calibration)
        {
            Pen pen = new Pen(GetBgColor(this.ExtType), 5);
            if (!IsLicenseExist)
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            RectangleF rectUm = !this.MasterRectangleUm.IsEmpty ? this.MasterRectangleUm : this.ClipRectangleUm;
            RectangleF rect = calibration.WorldToPixel(rectUm);
            Figure figure = new RectangleFigure(rect, pen);
            figure.Tag = this;

            if (offsetStructSet != null)
            {
                figure.Offset(offsetStructSet.PatternOffset.Offset);
                if (0 <= this.ContainerIndex && this.ContainerIndex < offsetStructSet.LocalCount)
                    figure.Offset(offsetStructSet.LocalOffsets[this.ContainerIndex].Offset);
            }
            return figure;
        }

        public Figure GetTempFigure(OffsetStructSet offsetStructSet)
        {
            RectangleFigure figure = new RectangleFigure(this.ClipRectangleUm, new Pen(GetBgColor(this.ExtType), 5) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot });
            if (offsetStructSet != null)
            {
                figure.Offset(offsetStructSet.PatternOffset.Offset);
                if (0 <= this.ContainerIndex && this.ContainerIndex < offsetStructSet.LocalCount)
                    figure.Offset(offsetStructSet.LocalOffsets[this.ContainerIndex].Offset);
            }
            return figure;
        }

        public bool BuildItem(AlgoImage trainImage, RegionInfoG regionInfoG, PointF position, Calibration calibration, DebugContext debugContext)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(trainImage);

            // Bar 영역 이미지
            Rectangle patRect = regionInfoG.Region;
            AlgoImage barImage = trainImage.Clip(patRect);
            barImage.Save("barImage.bmp", debugContext);

            // 이진화 후 Blob
            ip.Binarize(barImage, barImage, true);
            barImage.Save("Binarize.bmp", debugContext);

            List<BlobRect> blobList;
            using (BlobRectList blobRectList = ip.Blob(barImage, new BlobParam() { EraseBorderBlobs = true }))
                blobList = blobRectList.GetList();

            // Blob 평균 면적의 0.5배 이하인 Blob은 제거
            float average = blobList.Average(f => f.Area);
            blobList.RemoveAll(f => f.Area < average * 0.5);

            // Bar의 가로/세로 pt지점과 가장 가까운 Blob 선택
            PointF centerPt = new PointF(barImage.Width * position.X, barImage.Height * position.Y);
            blobList = blobList.OrderBy(f => MathHelper.GetLength(centerPt, f.CenterPt)).ToList();
            BlobRect centerBlobRect = blobList.FirstOrDefault();
            RectangleF boundRect = centerBlobRect.BoundingRect;
            BlobRect[] neighbor = new BlobRect[]
            {
                blobList.FindAll(f=>f.BoundingRect.IntersectsWith(RectangleF.FromLTRB(0,boundRect.Top,boundRect.Left,boundRect.Bottom))).OrderBy(f=>f.BoundingRect.Right).LastOrDefault(),
                blobList.FindAll(f=>f.BoundingRect.IntersectsWith(RectangleF.FromLTRB(boundRect.Left,0,boundRect.Right,boundRect.Top))).OrderBy(f=>f.BoundingRect.Bottom).LastOrDefault(),
                blobList.FindAll(f=>f.BoundingRect.IntersectsWith(RectangleF.FromLTRB(boundRect.Right,boundRect.Top, barImage.Width,boundRect.Bottom))).OrderBy(f=>f.BoundingRect.Left).FirstOrDefault(),
                blobList.FindAll(f=>f.BoundingRect.IntersectsWith(RectangleF.FromLTRB(boundRect.Left,boundRect.Bottom,boundRect.Right,barImage.Height))).OrderBy(f=>f.BoundingRect.Top).FirstOrDefault()
            };

            float[] dxs = new float[] { neighbor[0] == null ? 0 : centerBlobRect.CenterPt.X - neighbor[0].CenterPt.X, neighbor[2] == null ? 0 : neighbor[2].CenterPt.X - centerBlobRect.CenterPt.X };
            float[] dys = new float[] { neighbor[1] == null ? 0 : centerBlobRect.CenterPt.Y - neighbor[1].CenterPt.Y, neighbor[3] == null ? 0 : neighbor[3].CenterPt.Y - centerBlobRect.CenterPt.Y };
            SizeF masterRectSize = new SizeF(dxs.Max() * 2, dys.Max() * 2);
            if (masterRectSize.Width == 0 || masterRectSize.Height == 0)
                return false;

            Rectangle masterRect = Rectangle.Round(DrawingHelper.FromCenterSize(centerBlobRect.CenterPt, masterRectSize));

            //모니터링 영역으로 등록
            masterRect.Offset(regionInfoG.Region.Location);

            ImageD imageD;
            using (AlgoImage monAlgoImage = trainImage.GetSubImage(masterRect))
                imageD = monAlgoImage.ToImageD();

            this.Use = true;
            this.Index = -1;
            this.ContainerIndex = -1;
            //watchItem.ClipRectangle = clipRect;
            //this.MasterRectangleR = masterRect;
            this.MasterImageD = imageD;

            RectangleF rectUm = calibration.PixelToWorld(masterRect);
            SetMasterRectangleUm(rectUm);

            barImage.Dispose();
            return true;
        }

        public virtual void PrepareInspection() { }

        public virtual void Export(XmlElement element)
        {
            XmlHelper.SetValue(element, "Index", this.Index.ToString());
            XmlHelper.SetValue(element, "ContainerIndex", ContainerIndex);
            XmlHelper.SetValue(element, "Use", this.use);
            XmlHelper.SetValue(element, "Name", this.Name);
            XmlHelper.SetValue(element, "WatchType", this.ExtType.ToString());
            XmlHelper.SetValue(element, "MasterImageD", this.MasterImageD);
            XmlHelper.SetValue(element, "MasterRectangleUm", this.MasterRectangleUm);
            XmlHelper.SetValue(element, "ClipRectangleUm", this.ClipRectangleUm);
        }

        public virtual bool Import(XmlElement element)
        {
            this.Index = XmlHelper.GetValue(element, "Index", this.Index);
            this.ContainerIndex = XmlHelper.GetValue(element, "ContainerIndex", -1);
            this.use = XmlHelper.GetValue(element, "Use", this.use);
            this.Name = XmlHelper.GetValue(element, "Name", this.Name);
            this.ExtType = XmlHelper.GetValue(element, "WatchType", this.ExtType);
            this.MasterImageD = XmlHelper.GetValue(element, "MasterImageD", this.MasterImageD);
            this.ClipRectangleUm = XmlHelper.GetValue(element, "ClipRectangleUm", this.ClipRectangleUm);
            this.MasterRectangleUm = XmlHelper.GetValue(element, "MasterRectangleUm", this.MasterRectangleUm);

            if (this.MasterRectangleUm.IsEmpty || this.ClipRectangleUm.IsEmpty)
            {
                LogHelper.Debug(LoggerType.Operation, $"ItemExt::Import - Name: {this.Name}");
                Calibration calibration = new ScaledCalibration(14);
                Rectangle masterRectanglePx = XmlHelper.GetValue(element, "MasterRectangle", Rectangle.Empty);
                if (!masterRectanglePx.IsEmpty)
                {
                    RectangleF masterRectangleUm = calibration.PixelToWorld(masterRectanglePx);
                    LogHelper.Debug(LoggerType.Operation, $"ItemExt::Import - masterRectanglePx: {masterRectanglePx}");
                    LogHelper.Debug(LoggerType.Operation, $"ItemExt::Import - masterRectangleUm: {masterRectangleUm}");
                    SetMasterRectangleUm(masterRectangleUm);
                }
            }
            
            return true;
        }

        public virtual void CopyFrom(ExtItem watchItem)
        {
            this.LicenseKey = (string)watchItem.LicenseKey.Clone();
            this.Index = watchItem.Index;
            this.ContainerIndex = watchItem.ContainerIndex;
            this.use = watchItem.use;
            this.Name = watchItem.Name;
            this.ExtType = watchItem.ExtType;
            this.ClipRectangleUm = watchItem.ClipRectangleUm;
            this.MasterRectangleUm = watchItem.MasterRectangleUm;
            this.MasterImageD = watchItem.MasterImageD.Clone();
        }

        public string GetFileName()
        {
            //return string.Format("{0}.{1}.{2}.bmp", watchType.ToString(), this.index, this.name);

            //"Image_C{0:00}_S{1:000}_L{2:00}.{3}";
            int camIndex = SystemManager.Instance().ExchangeOperator.GetCamIndex();
            return DynMvp.Devices.ImageBuffer.GetImage2dFileName(camIndex, (int)ExtType, this.Index, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        protected virtual void UpdateClipRectangle()
        {
            // 폭,너비가 1.5배
            PointF centerPointR = DrawingHelper.CenterPoint(this.MasterRectangleUm);
            RectangleF inflateRectR = RectangleF.Inflate(this.MasterRectangleUm, this.MasterRectangleUm.Width / 2, this.MasterRectangleUm.Height / 2);

            // 가로:세로 비율을 16:9
            float unit = Math.Max(inflateRectR.Width / 16f, inflateRectR.Height / 9f);
            RectangleF clipRectangle = DrawingHelper.FromCenterSize(centerPointR, new SizeF(unit * 16, unit * 9));

            this.ClipRectangleUm = clipRectangle;
        }

        public int CompareTo(object obj)
        {
            ExtItem extItem = obj as ExtItem;
            if (extItem == null)
                throw new ArgumentException();
            return Index.CompareTo(extItem.Index);
        }
    }

    public abstract class ExtParam
    {
        public bool Available { get; private set; }
        public abstract bool Use { get; }
        //public virtual bool Available => true;

        public abstract ExtParam Clone();

        public bool Active => Use && Available;

        public ExtParam(bool available)
        {
            this.Available = available;
        }

        public void Save(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            Save(xmlElement);
        }

        public void Load(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                if (subElement != null)
                    Load(subElement);
                return;
            }

            Load(xmlElement);
        }

        public virtual void Save(XmlElement xmlElement) { }

        public virtual void Load(XmlElement xmlElement) { }
    }

    public class ExtCollectionTrainParam
    {
        public AlgoImage TrainImage { get; set; }
        public List<RegionInfoG> RegionInfoList { get; set; }
        public Point BasePos { get; set; }
        public Point BaseOffset { get; set; }
        public Vision.SheetFinder.BaseXSearchDir BaseXSearchDir { get; set; }
        public Calibration Calibration { get; set; }

        public ExtCollectionTrainParam() { }
    }

    public abstract class ExtCollection
    {
        protected List<ExtItem> items;
        protected ExtParam param;

        public int Count => this.items.Count;

        public ExtType ExtType => this.extType;
        ExtType extType = ExtType.NONE;

        public List<ExtItem> Items => new List<ExtItem>(this.items);

        public abstract ExtItem CreateItem();
        public abstract FoundedObjInPattern[] Inspect(SheetInspectParam inspectParam);
        public abstract ExtCollection Clone();

        public abstract void Train(ExtCollectionTrainParam trainParam, Action ProgressUpdated, DebugContext debugContext);

        public ExtCollection(ExtType extType)
        {
            this.extType = extType;
            this.items = new List<ExtItem>();
        }

        public void Add(ExtItem extItem)
        {
            if (this.extType != extItem.ExtType)
                throw new ArgumentException();

            this.items.Add(extItem);
        }

        public void Remove(ExtItem extItem)
        {
            if (this.extType != extItem.ExtType)
                throw new ArgumentException();

            this.items.Remove(extItem);
        }

        public void UpdateIndex()
        {
            for (int i = 0; i < this.items.Count; i++)
            {
                ExtItem item = this.items[i];
                if (!item.IsFixed)
                {
                    item.Index = this.items.IndexOf(item);
                    if (string.IsNullOrEmpty(item.Name))
                        item.Name = item.Index.ToString();
                }
            }
        }

        public void RemoveAll(Predicate<ExtItem> prod)
        {
            this.items.RemoveAll(prod);
        }

        public void RemoveAt(int index)
        {
            this.items.RemoveAll(f => f.Index == index);
        }

        public bool Exist(string name)
        {
            return this.items.Exists(f => f.Name == name);
        }

        public ExtItem Get(string name)
        {
            return this.items.Find(f => f.Name == name);
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public void Sort()
        {
            this.items.Sort();
        }

        public void Offset(Point point)
        {
            this.items.ForEach(f => f.Offset(point));
        }

        public Figure GetFigure(OffsetStructSet offsetStructSet, Calibration calibration)
        {
            FigureGroup fg = new FigureGroup();
            this.items.ForEach(f => fg.AddFigure(f.GetFigure(offsetStructSet, calibration)));
            return fg;
        }

        public virtual void PrepareInspection()
        {
            this.items.ForEach(f => f.PrepareInspection());
            //foreach (ExtItem extItem in this.items)
            //    extItem.PrepareInspection();
        }

        public void Save(XmlElement xmlElement, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Save(xmlElement);
                return;
            }

            XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
            xmlElement.AppendChild(subElement);
            Save(subElement);
        }

        public virtual void Save(XmlElement xmlElement)
        {
            this.param.Save(xmlElement, "Param");

            foreach (ExtItem watchItem in this.items)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement("Item");
                watchItem.Export(subElement);
                xmlElement.AppendChild(subElement);
            }
        }

        public void Load(XmlElement xmlElement, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Load(xmlElement);
                return;
            }

            XmlElement subElement = xmlElement[key];
            Load(subElement);
        }

        public virtual void Load(XmlElement xmlElement)
        {
            if (xmlElement == null)
                return;

            this.param.Load(xmlElement, "Param");

            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("Item");
            foreach (XmlElement subElement in xmlNodeList)
            {
                ExtItem watchItem = CreateItem();
                watchItem.Import(subElement);

                if (this.extType == watchItem.ExtType)
                    this.items.Add(watchItem);
            }

            //if (!DynMvp.Base.LicenseManager.Exist(this.licenseKey))
            //    this.items.ForEach(f => f.Use = false);
        }

    }
}
