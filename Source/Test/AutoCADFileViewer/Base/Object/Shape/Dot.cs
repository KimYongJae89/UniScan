using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADFileViewer.Base.Object.Shape
{
    using PointList = List<List<List<double>>>;
    class Dot : ObjectParser
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
            var dotInfo = (obj as netDxf.Entities.Point);
            Debug.Assert(dotInfo != null);
            double dotX = dotInfo.Position.X;
            double dotY = dotInfo.Position.Y;
            var dot = new List<double>() { dotX, dotY };
            var objPoint = new List<List<double>>() { dot };
            var pointList = new PointList();
            pointList.Add(objPoint);
            base.SetOuterRect(dotX, dotY);
            base.ContaionsTwoKeyAdd(0, objPoint.Count, pointList, objPoint);
        }
        public override void Clear()
        {

        }

        public List<Point> GetDotPointList(double mul, PointF startCoord)
        {
            List<Point> pointList = new List<Point>();
            foreach (var layer in base.Layers)
            {
                foreach (var point in layer.Value)
                {
                    for (int i = 0; i < point.Value.Count; ++i)
                    {
                        Point sp = new Point();
                        sp.X = (int)(mul * (point.Value[i][0][0] - startCoord.X));
                        sp.Y = (int)(mul * (point.Value[i][0][1] - startCoord.Y));

                        pointList.Add(sp);
                    }
                }
            }
            return pointList;
        }
        #endregion

    }
}
