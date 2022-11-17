using System.Collections.Generic;
using System.Drawing;

namespace AutoCADFileViewer.Base.Object
{
    using PointList = List<List<List<double>>>;

    public abstract class ObjectParser
    {
        #region 필드 
        public ObjectParser()
        {
            rightBottom = new PointF();
        }
        public PointF rightBottom;
        public RectangleF outerRect;
        public void SetOuterRect(double x, double y) //만약 최외각 사각형의 점 좌표가 DXF파일안에 정의되어 있다면 아래처럼 찾을 필요는 없음.
        {
            outerRect.X = outerRect.X < (float)x ? outerRect.X : (float)x;
            outerRect.Y = outerRect.Y < (float)y ? outerRect.Y : (float)y;
            rightBottom.X = rightBottom.X > (float)x ? rightBottom.X : (float)x;
            rightBottom.Y = rightBottom.Y > (float)y ? rightBottom.Y : (float)y;
            outerRect.Width = outerRect.Width > (rightBottom.X - outerRect.X) ? outerRect.Width : (rightBottom.X - outerRect.X);
            outerRect.Height = outerRect.Height > (rightBottom.Y - outerRect.Y) ? outerRect.Height : (rightBottom.Y - outerRect.Y);
        }
        public void ContaionsTwoKeyAdd(double key1, int key2, PointList val, List<List<double>> objPoint)
        {
            if (!Layers.ContainsTwoKey(key1, key2))
            {
                Dictionary<int, PointList> ptDictionary = new Dictionary<int, PointList>();
                ptDictionary.Add(key2, val);
                Layers.Add(key1, ptDictionary);
            }
            else
            {
                Layers[key1][key2].Add(objPoint);
            }
        }
        #endregion

        #region 생성자 
        #endregion

        #region 속성 
        public ObjectDictionary Layers { get; } = new ObjectDictionary();
        #endregion

        #region 대리자 
        #endregion

        #region 이벤트	 
        #endregion

        #region 메서드 
        public abstract void ClassifiedAdd(netDxf.DxfObject obj);

        public abstract void Clear();

        public virtual ObjectDictionary GetObjectDictionary()
        {
            return Layers;
        }

        public virtual RectangleF GetouterRect()
        {
            return outerRect;
        }
        #endregion

        #region 클래스
        public class ObjectDictionary : Dictionary<double, Dictionary<int, PointList>> // double : area,
        {
            public bool ContainsTwoKey(double key1, int key2)
            {
                if (this.Count == 0)
                {
                    return false;
                }
                else
                {
                    if (this.ContainsKey(key1))
                    {
                        if (this[key1].ContainsKey(key2))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
        }
        #endregion
    }
}
