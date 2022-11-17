using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace AutoCADFileViewer.Base.Object.Shape
{
    using PointList = List<List<List<double>>>;
    class Polygon : ObjectParser
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
            var pointList = new PointList();
            List<netDxf.Entities.LwPolylineVertex> vertexes = (obj as netDxf.Entities.LwPolyline)?.Vertexes;
            Debug.Assert(vertexes != null);

            int j = vertexes.Count - 1;
            double sum = 0.0;
            var objPoint = new List<List<double>>();
            for (int i = 0; i < vertexes.Count; ++i)
            {
                List<double> positionList = new List<double>();
                double x = vertexes[i].Position.X;
                double y = vertexes[i].Position.Y;
                positionList.Add(x);
                positionList.Add(y);
                objPoint.Add(positionList);

                sum += (vertexes[j].Position.X + x) * (vertexes[j].Position.Y - y);
                j = i;

                base.SetOuterRect(x, y);
            }
            pointList.Add(objPoint);

            double area = Math.Round(sum / 2, 4);
            base.ContaionsTwoKeyAdd(area, vertexes.Count, pointList, objPoint);
        }
        public override void Clear()
        {
            base.Layers.Clear();
        }
        public List<Point[]> GetPolygonPointList(double mul, PointF startCoord)
        {
            List<Point[]> pointList = new List<Point[]>();
            foreach (var layer in base.Layers)
            {
                foreach (var point in layer.Value)
                {
                    for (int i = 0; i < point.Value.Count; ++i)
                    {
                        var polyPoint = new Point[point.Value[i].Count];
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
