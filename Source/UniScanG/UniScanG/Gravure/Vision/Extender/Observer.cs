using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Data;

namespace UniScanG.Gravure.Vision.Extender
{
    public class Observer : ClipAndSave
    {
        public Observer(ExtType extType) : base(extType, DynMvp.Base.LicenseManager.ELicenses.ExtObserve) { }

        protected override ExtItem CloneItem()
        {
            return new Observer(this.ExtType);
        }

        protected override void UpdateClipRectangle()
        {
            float l = Math.Max(this.MasterRectangleUm.Width, this.MasterRectangleUm.Height);
            SizeF inflate = new SizeF(l - this.MasterRectangleUm.Width, l - this.MasterRectangleUm.Height);
            this.ClipRectangleUm = RectangleF.Inflate(this.MasterRectangleUm, inflate.Width / 2, inflate.Height / 2);
        }
    }

    public abstract class ObserverCollection : ClipAndSaveCollection
    {
        public ObserverCollection(ExtType extType) : base(extType, false, DynMvp.Base.LicenseManager.ELicenses.ExtObserve) { }
        public ObserverCollection(ExtType extType, ExtParam param) : this(extType)
        {
            this.param = param.Clone();
        }

        protected abstract ExtCollection CreateCollection();
        //{
        //    return new ObserverCollection(this.ExtType, this.param);
        //}

        public override ExtItem CreateItem()
        {
            return new Observer(this.ExtType);
        }

        public override ExtCollection Clone()
        {
            ExtCollection extCollection = CreateCollection();
            this.items.ForEach(f => extCollection.Add(f.Clone()));
            return extCollection;
        }
    }

    public class ObserverChipCollection : ObserverCollection
    {
        public ObserverChipCollection() : base(ExtType.CHIP) { }
        public ObserverChipCollection(ExtParam param) : base(ExtType.CHIP, param) { }

        protected override ExtCollection CreateCollection()
        {
            return new ObserverChipCollection(this.param);
        }

        public override void Train(ExtCollectionTrainParam trainParam, Action ProgressUpdated, DebugContext debugContext)
        {
            if (!this.Param.Active)
                return;
        }
    }

    public class ObserverFPCollection : ObserverCollection
    {
        public ObserverFPCollection() : base(ExtType.FP) { }
        public ObserverFPCollection(ExtParam param) : base(ExtType.FP, param) { }

        protected override ExtCollection CreateCollection()
        {
            return new ObserverFPCollection(this.param);
        }

        public override void Train(ExtCollectionTrainParam trainParam, Action ProgressUpdated, DebugContext debugContext)
        {
            if (!this.Param.Active)
                return;

            //Clear();

            SizeF sizeUm = new SizeF(9100, 9100 * 2); // 650px @ 14um/px
            Size sizePx = Size.Round(trainParam.Calibration.WorldToPixel(sizeUm));

            List<Rectangle> rectList = new List<Rectangle>();
            Point lt = Point.Empty;
            Point lb = new Point(0, trainParam.TrainImage.Height);
            Point rt = new Point(trainParam.TrainImage.Width, 0);
            Point rb = new Point(trainParam.TrainImage.Width, trainParam.TrainImage.Height);

            RegionInfoG regionInfoLT = trainParam.RegionInfoList.OrderBy(f => MathHelper.GetLength(lt, f.Region.Location)).FirstOrDefault();
            if (regionInfoLT != null)
            {
                Point ltLoc = new Point(regionInfoLT.Region.Left, regionInfoLT.Region.Top);
                rectList.Add(new Rectangle(ltLoc, sizePx));
            }

            RegionInfoG regionInfoLB = trainParam.RegionInfoList.OrderBy(f => MathHelper.GetLength(lb, f.Region.Location)).FirstOrDefault();
            if (regionInfoLB != null)
            {
                Point lbLoc = new Point(regionInfoLB.Region.Left, regionInfoLB.Region.Bottom - sizePx.Height);
                rectList.Add(new Rectangle(lbLoc, sizePx));
            }

            if (trainParam.BaseXSearchDir == SheetFinder.BaseXSearchDir.Right2Left)
            {
                RegionInfoG regionInfoRT = trainParam.RegionInfoList.OrderBy(f => MathHelper.GetLength(rt, f.Region.Location)).FirstOrDefault();
                if (regionInfoRT != null)
                {
                    Point rtLoc = new Point(regionInfoRT.Region.Right - sizePx.Width, regionInfoRT.Region.Top);
                    rectList.Add(new Rectangle(rtLoc, sizePx));
                }

                RegionInfoG regionInfoRB = trainParam.RegionInfoList.OrderBy(f => MathHelper.GetLength(rb, f.Region.Location)).FirstOrDefault();
                if (regionInfoRB != null)
                {
                    Point rbLoc = new Point(regionInfoRB.Region.Right - sizePx.Width, regionInfoRB.Region.Bottom - sizePx.Height);
                    rectList.Add(new Rectangle(rbLoc, sizePx));
                }
            }

            for (int i = 0; i < rectList.Count; i++)
            {
                Rectangle rect = rectList[i];
                rect.Intersect(new Rectangle(Point.Empty, trainParam.TrainImage.Size));
                if (rect.Width == 0 || rect.Height == 0)
                    continue;

                string name = i.ToString();
                if (Exist(name))
                    continue;

                ExtItem watchItem = CreateItem();
                using (AlgoImage masterAlgoImage = trainParam.TrainImage.GetSubImage(rect))
                {
                    watchItem.Use = true;
                    watchItem.Index = i;
                    watchItem.ExtType = ExtType.FP;
                    watchItem.Name = name;
                    watchItem.MasterImageD = masterAlgoImage.ToImageD();
                }

                RectangleF rectUm = trainParam.Calibration.PixelToWorld(rect);
                watchItem.SetMasterRectangleUm(rectUm);

                Add(watchItem);
            }
            UpdateIndex();
        }
    }

    public class ObserverIndexCollection : ObserverCollection
    {
        public ObserverIndexCollection() : base(ExtType.INDEX) { }
        public ObserverIndexCollection(ExtParam param) : base(ExtType.INDEX, param) { }

        protected override ExtCollection CreateCollection()
        {
            return new ObserverIndexCollection(this.param);
        }

        public override void Train(ExtCollectionTrainParam trainParam, Action ProgressUpdated, DebugContext debugContext)
        {
            if (!this.Param.Active)
                return;

            //Clear();

            SizeF sizeUm = new SizeF(9100, 9100); // 650px @ 14um/px
            Size sizePx = Size.Round(trainParam.Calibration.WorldToPixel(sizeUm));

            float gapUm = 4500;
            int gapPx = (int)trainParam.Calibration.WorldToPixel(gapUm);

            int centerX = trainParam.BasePos.X;
            if (trainParam.BaseXSearchDir == SheetFinder.BaseXSearchDir.Left2Right)
                centerX -= gapPx;
            else
                centerX += gapPx;

            int[] centerY = new int[]
            {
                (int)(trainParam.TrainImage.Size.Height*0.1),
                (int)(trainParam.TrainImage.Size.Height*0.5),
                (int)(trainParam.TrainImage.Size.Height*0.9),
            };

            for (int i = 0; i < centerY.Length; i++)
            {
                Rectangle rect = DrawingHelper.FromCenterSize(new Point(centerX, centerY[i]), sizePx);
                rect.Intersect(new Rectangle(Point.Empty, trainParam.TrainImage.Size));
                if (rect.Width == 0 || rect.Height == 0)
                    continue;

                string name = i.ToString();
                if (Exist(name))
                    continue;

                ExtItem watchItem = CreateItem();
                using (AlgoImage masterAlgoImage = trainParam.TrainImage.GetSubImage(rect))
                {
                    watchItem.Use = true;
                    watchItem.Index = i;
                    watchItem.ExtType = ExtType.INDEX;
                    watchItem.Name = name;
                    watchItem.MasterImageD = masterAlgoImage.ToImageD();
                }

                RectangleF rectUm = trainParam.Calibration.PixelToWorld(rect);
                watchItem.SetMasterRectangleUm(rectUm);

                Add(watchItem);
            }
        }
    }
}
