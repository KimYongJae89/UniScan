using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace AutoCADFileViewer.Base.Object.Shape
{
    using PointList = List<List<List<double>>>;
    class Spline : ObjectParser //TODO : knots(매듭)을 통한 곡선을 표현하는데 이것에 대한 추가 수정이 필요함.
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
            IReadOnlyList<netDxf.Entities.SplineVertex> splineVertices = (obj as netDxf.Entities.Spline)?.ControlPoints;
            Debug.Assert(splineVertices != null);
            var pointList = new PointList();

            int j = splineVertices.Count - 1;
            double sum = 0.0;
            var objPoint = new List<List<double>>();
            for (int i = 0; i < splineVertices.Count; ++i)
            {
                List<double> positionList = new List<double>();
                double x = splineVertices[i].Position.X;
                double y = splineVertices[i].Position.Y;
                positionList.Add(x);
                positionList.Add(y);
                objPoint.Add(positionList);

                sum += (splineVertices[j].Position.X + x) * (splineVertices[j].Position.Y - y);
                j = i;

                base.SetOuterRect(x, y);
            }
            pointList.Add(objPoint);

            double area = Math.Round(sum / 2, 4);
            base.ContaionsTwoKeyAdd(area, splineVertices.Count, pointList, objPoint);
        }
        public override void Clear()
        {

        }

        public List<Point[]> GetSplinePointList(double mul, PointF startCoord)
        {
            List<Point[]> pointList = new List<Point[]>();
            foreach (var layer in base.Layers)
            {
                foreach (var point in layer.Value)
                {
                    for (int i = 0; i < point.Value.Count; ++i)
                    {
                        Point[] polyPoint = new Point[point.Value[i].Count];
                        for (int j = 0; j < point.Value[i].Count; ++j)
                        {
                            polyPoint[j].X = (int)(mul * (point.Value[i][j][0] - startCoord.X));
                            polyPoint[j].Y = (int)(mul * (point.Value[i][j][1] - startCoord.Y));
                        }
                        pointList.Add(polyPoint);
                    }
                }
            }
            return pointList;
        }
        #endregion
    }
}
