using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.Fiducial;

namespace UniScanG.Gravure.Vision.Extender
{
    internal class MeasureShrinkAndExtend
    {
        PatternMatchingParam patternMatchingParam;

        public FiducialFinder[] AlignComponents => this.fiducialFinders;
        FiducialFinder[] fiducialFinders;

        public MeasureShrinkAndExtend()
        {
            this.fiducialFinders = null;
            this.patternMatchingParam = new PatternMatchingParam()
            {
                SpeedType = 0,
            };
        }

        internal void Initialize(ExtItem[] watchItem, Calibration calibration)
        {
            List<FiducialFinder> alignComponentList = new List<FiducialFinder>();
            for (int i = 0; i < watchItem.Length; i++)
            {
                ExtItem f = watchItem[i];
                if (f.Use)
                {
                    string position = f.Name;
                    ImageD masterImageD = f.MasterImageD;
                    //masterImageD.SaveImage(@"C:\temp\masterImageD.bmp");

                    RectangleF masterRectUm = f.MasterRectangleUm;
                    Rectangle masterRect = Rectangle.Round(calibration.WorldToPixel(masterRectUm));

                    RectangleF searchRectUm = f.ClipRectangleUm;
                    Rectangle searchRect = Rectangle.Round(calibration.WorldToPixel(searchRectUm));

                    AlignInfo inBarAlignElement = new AlignInfo(position, masterImageD, masterRect, searchRect);
                    FiducialFinder alignComponent = new FiducialFinder();
                    alignComponent.Initialize(inBarAlignElement, this.patternMatchingParam);
                    alignComponentList.Add(alignComponent);
                }
            }

            this.fiducialFinders = alignComponentList.ToArray();
        }

        internal OffsetObj[] Measure(AlgoImage algoImage, Point patternOffset, float minScore, Calibration calibration, DebugContextG debugContextG)
        {
            //List<OffsetStruct> offsetStructList = this.alignComponents.Select(f => f.Align(algoImage, patternOffset, minScore, debugContextG)).ToList();
            //return offsetStructList.ToArray();

            List<OffsetObj> resultElementList = new List<OffsetObj>();
            for (int i = 0; i < this.fiducialFinders.Length; i++)
            {
                FiducialFinder alignComponent = this.fiducialFinders[i];
                OffsetStruct offsetStruct = alignComponent.Align(algoImage, patternOffset, minScore, true, debugContextG);

                PointF refPointPx = DrawingHelper.CenterPoint(alignComponent.SearchRect);
                SizeF refSizePx = alignComponent.SearchRect.Size;
                SizeF refSizeUm = calibration.PixelToWorld(alignComponent.SearchRect.Size);

                PointF adjPointPx = DrawingHelper.Add(refPointPx, patternOffset);
                SizeF matchingOffsetPx = new SizeF(offsetStruct.OffsetF);

                PointF adjPointUm = calibration.PixelToWorld(adjPointPx);
                SizeF matchingOffsetUm = calibration.PixelToWorld(matchingOffsetPx);

                RectangleF region = DrawingHelper.FromCenterSize(adjPointPx, refSizePx);
                RectangleF realRegion = DrawingHelper.FromCenterSize(adjPointUm, refSizeUm);
                //Rectangle region2 = Rectangle.Round(calibration.WorldToPixel(realRegion));

                OffsetObj offsetObj = new OffsetObj()
                {
                    Region = region,
                    RealRegion = realRegion
                };

                //변형량 측정은 항상 GOOD 처리 -> 레포트 화면에서 템플릿매칭 결과 안보이게??
                //offsetObj.Set(alignComponent.Position, refPointPx, patternOffset, matchingOffsetPx, matchingOffsetUm, offsetStruct.Score, false, offsetStruct.ImageD?.ToBitmap());
                offsetObj.Set(alignComponent.Position, refPointPx, patternOffset, matchingOffsetPx, matchingOffsetUm, offsetStruct.Score, !offsetStruct.IsGood, offsetStruct.ImageD?.ToBitmap());
                resultElementList.Add(offsetObj);
            }

            return resultElementList.ToArray();
        }

        internal void Dispose()
        {
            if (this.fiducialFinders != null)
            {
                Array.ForEach(this.fiducialFinders, f => f.Dispose());
                this.fiducialFinders = null;
            }
        }
    }
}
