using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace AutoCADFileViewer.Base.Object.Shape
{
    using PointList = List<List<List<double>>>;
    class Circle : ObjectParser
    {
        #region 필드 
        #endregion

        #region 생성자 
        #endregion

        #region 대리자
        #endregion

        #region 이벤트	 
        #endregion

        #region 속성 
        #endregion

        #region 메서드 
        public override void ClassifiedAdd(netDxf.DxfObject obj)
        {
            var c = (obj as netDxf.Entities.Circle);
            Debug.Assert(c != null);
            double area = Math.Round(Math.PI * c.Radius * c.Radius, 4);
            var xyr = new List<double>() { c.Center.X, c.Center.Y, c.Radius };
            var objPoint = new List<List<double>>() { xyr };
            var pointList = new PointList() { objPoint };

            base.SetOuterRect((c.Center.X - c.Radius), (c.Center.Y - c.Radius));

            base.ContaionsTwoKeyAdd(area, xyr.Count, pointList, objPoint);
        }

        public override void Clear()
        {

        }

        public List<Emgu.CV.Structure.RotatedRect> GetCircleRotatedRectList(double mul, PointF startCoord)
        {
            var circleRotatedRectList = new List<Emgu.CV.Structure.RotatedRect>();
            foreach (var layer in Layers)
            {
                foreach (var pt in layer.Value)
                {
                    for (int i = 0; i < pt.Value.Count; ++i)
                    {
                        var x = (int)(mul * (pt.Value[i][0][0] - startCoord.X));
                        var y = (int)(mul * (pt.Value[i][0][1] - startCoord.Y));
                        var r = (float)Math.Abs(mul * pt.Value[i][0][2]);
                        var center = new PointF(x, y);
                        var size = new SizeF(r, r);
                        Emgu.CV.Structure.RotatedRect rotateRect = new Emgu.CV.Structure.RotatedRect(center, size, 0.0f);
                        circleRotatedRectList.Add(rotateRect);
                    }
                }
            }
            return circleRotatedRectList;
        }
        #endregion
    }
}
