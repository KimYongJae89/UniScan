using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace AutoCADFileViewer.Base.Object.Shape
{
    using PointList = List<List<List<double>>>;
    class Line : ObjectParser
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
            var lineInfo = (obj as netDxf.Entities.Line);
            Debug.Assert(lineInfo != null);
            PointList pointList = new PointList();
            double sx = lineInfo.StartPoint.X;
            double sy = lineInfo.StartPoint.Y;
            double ex = lineInfo.EndPoint.X;
            double ey = lineInfo.EndPoint.Y;
            var sp = new List<double>() { sx, sy };
            var ep = new List<double>() { ex, ey };
            var objPoint = new List<List<double>>() { sp, ep };
            double lineLength = Math.Round(Math.Sqrt(Math.Pow(ex - sx, 2.0) + Math.Pow(ey - sy, 2)), 4);
            pointList.Add(objPoint);

            base.SetOuterRect(sx, sy);
            base.SetOuterRect(ex, ey);

            base.ContaionsTwoKeyAdd(lineLength, objPoint.Count, pointList, objPoint);
        }
        public override void Clear()
        {

        }

        public List<(Point, Point)> GetLinePointList(double mul, PointF startCoord)
        {
            List<(Point, Point)> pointList = new List<(Point, Point)>();
            foreach (var layer in base.Layers)
            {
                foreach (var point in layer.Value)
                {
                    for (int i = 0; i < point.Value.Count; ++i)
                    {

                        Point sp = new Point();
                        sp.X = (int)(mul * (point.Value[i][0][0] - startCoord.X));
                        sp.Y = (int)(mul * (point.Value[i][0][1] - startCoord.Y));

                        Point ep = new System.Drawing.Point();
                        ep.X = (int)(mul * (point.Value[i][1][0] - startCoord.X));
                        ep.Y = (int)(mul * (point.Value[i][1][1] - startCoord.Y));
                        pointList.Add((sp, ep));
                    }
                }
            }
            return pointList;
        }
        #endregion
    }
}
