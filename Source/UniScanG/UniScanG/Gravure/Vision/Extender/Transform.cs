using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision.Calculator.V2;
using UniScanG.Vision;

namespace UniScanG.Gravure.Vision.Extender
{
    public class Transform : ExtItem
    {
        protected override ExtItem CloneItem()
        {
            return new Transform();
        }

        public Transform() : base(ExtType.PMVariance, DynMvp.Base.LicenseManager.ELicenses.ExtTransfrom) { }

        protected override void UpdateClipRectangle()
        {
            this.ClipRectangleUm = RectangleF.Inflate(this.MasterRectangleUm, this.MasterRectangleUm.Width / 2, this.MasterRectangleUm.Height / 2);
        }
    }

    public class TransformParam : ExtParam
    {
        public override bool Use => AdditionalSettings.Instance().TransformUse;

        public float MatchingScore { get => this.matchingScore; set => this.matchingScore = value; }
        float matchingScore;

        public Size Count { get => this.count; set => this.count = value; }
        private Size count;

        public SizeF SizeUm { get => this.sizeUm; set => this.sizeUm = value; }
        private SizeF sizeUm;

        public TransformParam(bool available) : base(available)
        {
            this.matchingScore = 55.0f;
            this.count = new Size(2, 3);
            this.sizeUm = new Size(3000, 3000);
        }

        public override ExtParam Clone()
        {
            return new TransformParam(this.Available)
            {
                matchingScore = this.matchingScore,
                count = this.count,
                sizeUm = this.sizeUm
            };
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "Score", this.matchingScore);
            XmlHelper.SetValue(xmlElement, "Count", this.count);
            XmlHelper.SetValue(xmlElement, "SizeUm", this.sizeUm);
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.matchingScore = XmlHelper.GetValue(xmlElement, "Score", this.matchingScore);
            this.count = XmlHelper.GetValue(xmlElement, "Count", this.count);
            this.sizeUm = XmlHelper.GetValue(xmlElement, "SizeUm", this.sizeUm);
        }
    }

    public class TransformCollection : ExtCollection
    {
        public TransformParam Param => (TransformParam)this.param;

        public TransformCollection() : base(ExtType.PMVariance)
        {
            this.param = new TransformParam(DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.ExtTransfrom));
        }

        public override ExtItem CreateItem()
        {
            return new Transform();
        }

        public override ExtCollection Clone()
        {
            TransformCollection transformCollection = new TransformCollection();
            transformCollection.param = this.param.Clone();
            this.items.ForEach(f => transformCollection.Add(f.Clone()));
            return transformCollection;
        }

        public override void Train(ExtCollectionTrainParam trainParam, Action ProgressUpdated, DebugContext debugContext)
        {
            Size count = this.Param.Active ? this.Param.Count : Size.Empty;
            List<RegionInfoG> useRegionInfoList = trainParam.RegionInfoList.FindAll(f => f.Use);

            if (count.Width <= 0 || count.Height <= 0)
            {
                Clear();
                return;
            }

            Rectangle validRect = new Rectangle(Point.Empty, trainParam.TrainImage.Size);
            Clear();

            int top = trainParam.BasePos.Y;
            int bottom = SheetFinder.SheetFinderBase.FindBasePosition(trainParam.TrainImage, null, Direction.Vertical, 100, true);
            int patterhLengthPx = bottom - top;
            if (patterhLengthPx <= 0)
                throw new Exception($"Cannot find pattern bottom edge");

            float estimatePatternLengthUm = trainParam.Calibration.PixelToWorld(patterhLengthPx);

            //float estimateSheetLengthMm2 = trainParam.Calibration.PixelToWorld(trainParam.TrainImage.Height) / 1000;

            int srcX = trainParam.BasePos.X;
            int dstX = (trainParam.BaseXSearchDir == SheetFinder.BaseXSearchDir.Left2Right) ? trainParam.TrainImage.Width : 0;
            int srcY = trainParam.BasePos.Y;
            int dstY = (int)(srcY + trainParam.Calibration.WorldToPixel(new PointF(0, estimatePatternLengthUm)).Y);

            SizeF sizeUm = this.Param.SizeUm;
            Size sizePx = Size.Round(trainParam.Calibration.WorldToPixel(this.Param.SizeUm));

            int[] centerX = new int[count.Width];
            for (int i = 0; i < count.Width; i++)
                centerX[i] = (int)Math.Round(i * (dstX - srcX) * 1f / count.Width) + srcX;
            int[] centerY = new int[count.Height];
            for (int i = 0; i < count.Height; i++)
                centerY[i] = (int)Math.Round(i * (dstY - srcY) * 1f / (count.Height - 1)) + srcY;

            int itemIdx = 0;
            for (int x = 0; x < count.Width; x++)
            {
                for (int y = 0; y < count.Height; y++)
                {
                    ProgressUpdated();
                    string name = string.Format("[X{0}/Y{1}]", x, y);

                    if (Exist(name))
                    {
                        ExtItem existItem = Get(name);
                        existItem.Offset(trainParam.BaseOffset);
                        continue;
                    }

                    Point centerPt = new Point(centerX[x], centerY[y]);
                    Rectangle rect = DrawingHelper.FromCenterSize(centerPt, sizePx);
                    if (Rectangle.Intersect(validRect, rect) != rect)
                        continue;

                    ExtItem watchItem = CreateItem();
                    using (AlgoImage masterAlgoImage = trainParam.TrainImage.GetSubImage(rect))
                    {
                        watchItem.Use = true;
                        watchItem.Index = itemIdx;
                        watchItem.ExtType = ExtType.PMVariance;
                        watchItem.Name = name;
                        watchItem.MasterImageD = masterAlgoImage.ToImageD();
                    }

                    RectangleF rectUm = trainParam.Calibration.PixelToWorld(rect);
                    watchItem.SetMasterRectangleUm(rectUm);

                    if ((x == 0) || (y == 0 || y == count.Height - 1))
                    {
                        Add(watchItem);
                        itemIdx++;
                    }

                    //curStep++;
                    //worker.ReportProgress((int)Math.Round(start + (step * curStep)),
                    //    string.Format(StringManager.GetString(this.GetType().FullName, "Monitoring Point ({0}/{1})"), curStep, totalSteps));
                    //Thread.Sleep(sleepMs);
                }
                Sort();
            }
            //curStep = monChipCount + marginCollection.Param.Count + (transformCollection.Param.Count.Width * transformCollection.Param.Count.Height);
        }

        public override FoundedObjInPattern[] Inspect(SheetInspectParam inspectParam)
        {
            if (!this.param.Active)
                return new FoundedObjInPattern[0];

            Calibration calibration = inspectParam.CameraCalibration;
            AlgoImage algoImage = inspectParam.AlgoImage;
            Point patternOffset = Point.Empty;
            MeasureShrinkAndExtend measureShrinkAndExtend = null;
            DebugContextG debugContextG = inspectParam.DebugContext as DebugContextG;

            ProcessBufferSetG2 processBufferSetG = inspectParam.ProcessBufferSet as ProcessBufferSetG2;
            bool disposeNeeded;
            if (processBufferSetG != null)
            {
                patternOffset = processBufferSetG.OffsetStructSet.PatternOffset.Offset;
                algoImage = processBufferSetG.AlgoImage;
                measureShrinkAndExtend = processBufferSetG.MeasureShrinkAndExtend;
                disposeNeeded = false;
            }
            else
            {
                measureShrinkAndExtend = new MeasureShrinkAndExtend();
                measureShrinkAndExtend.Initialize(this.items.ToArray(), calibration);
                disposeNeeded = true;
            }

            Stopwatch sw = Stopwatch.StartNew();
            OffsetObj[] watcherResultElements = measureShrinkAndExtend.Measure(algoImage, patternOffset, this.Param.MatchingScore, calibration, debugContextG);
            sw.Stop();
            debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Extend_Transform, sw.ElapsedMilliseconds);

            if (disposeNeeded)
                measureShrinkAndExtend.Dispose();
            return watcherResultElements;
        }
    }
}
