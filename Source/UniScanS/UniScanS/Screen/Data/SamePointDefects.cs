using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanS.Screen.Vision;

namespace UniScanS.Screen.Data
{
    public class SamePointDefects : List<PointF>
    {
        PointF centerPt;
        List<SheetSubResult> subResultList;

        public SamePointDefects(SheetSubResult subResult)
        {
            subResultList = new List<SheetSubResult>();

            PointF point = DrawingHelper.CenterPoint(subResult.Region);
            centerPt = new PointF(point.X, point.Y);

            this.Add(point);

            subResultList.Add(subResult);
        }

        public bool IsContains(PointF point)
        {
            float length = MathHelper.GetLength(point, centerPt);
            return (length <= AlgorithmSetting.Instance().DefectDistance);
        }

        public bool TryAdd(SheetSubResult subResult)
        {
            PointF point = DrawingHelper.CenterPoint(subResult.Region);

            if (IsContains(point))
            {
                this.Add(point);
                subResultList.Add(subResult);
                centerPt = new PointF(this.Average(p => p.X), this.Average(p => p.Y));

                return true;
            }

            return false;
        }

        public Figure GetCircleFigure(Color color)
        {
            return new EllipseFigure(new RectangleF(
                new PointF(centerPt.X - AlgorithmSetting.Instance().DefectDistance / 2, centerPt.Y - AlgorithmSetting.Instance().DefectDistance / 2),
                new SizeF(AlgorithmSetting.Instance().DefectDistance, AlgorithmSetting.Instance().DefectDistance)), new Pen(Color.Ivory), new SolidBrush(color));
        }
    }
}