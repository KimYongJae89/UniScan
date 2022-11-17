using AutoCADFileViewer.Base.Object;
using netDxf;
using netDxf.Header;
using netDxf.Tables;
using System.Collections.Generic;
using System.Drawing;

namespace AutoCADFileViewer.Base
{
    public class DxfHelper//: IDisposable
    {
        #region 필드
        #endregion

        #region 생성자 
        public DxfHelper(string fileName)
        {
            this.FileName = fileName;
            this.DxfVersion = DxfDocument.CheckDxfFileVersion(fileName, out this.isBinary);
            if (this.DxfVersion <= DxfVersion.AutoCad14)
            {
                this.LoadMessage = this.ErrorMessage = "Not supported version = [" + this.DxfVersion.ToString() + "]";
                this.LoadSuccess = false;
            }
            else
            {
                this.DxfDocument = DxfDocument.Load(this.FileName);
                this.DxfObjectList = GetLayerList(DxfDocument);
                this.LoadMessage = "Load Successed. version = [" + this.DxfVersion.ToString() + "]";
                this.LoadSuccess = true;
            }
        }
        #endregion

        #region 대리자 
        #endregion

        #region 속성
        public string FileName { get; } = "";

        public List<DxfObject> DxfObjectList { get; } = new List<DxfObject>();

        public DxfDocument DxfDocument { get; } = new DxfDocument();

        public DxfVersion DxfVersion { get; } = DxfVersion.Unknown;

        public string LoadMessage { get; } = "";

        public string ErrorMessage { get; set; } = "";

        public bool LoadSuccess { get; } = false;

        public Dictionary<ShapeType, ObjectParser> ObjectParser { get; } = new Dictionary<ShapeType, ObjectParser>();

        public RectangleF OuterMosetrect { get; set; } = new RectangleF();
        #endregion

        #region 인덱서
        public bool IsBinary { get => this.isBinary; }
        private bool isBinary = false;
        #endregion

        #region 이벤트	 
        #endregion

        #region 열거형
        public enum ShapeType
        {
            line, circle, lwpolyline, mlinestyle, arc, spline, point, insert, hatch
        }
        #endregion

        #region 메서드 
        public void AddObjectParser(ShapeType key, ObjectParser val)
        {
            this.ObjectParser.Add(key, val);
        }

        public Dictionary<ShapeType, ObjectParser> ClassifedAddObjectPaser()
        {
            foreach (var obj in DxfObjectList)
            {
                var result = (ShapeType)System.Enum.Parse(typeof(ShapeType), obj.CodeName.ToLower());
                if (ObjectParser.ContainsKey(result))
                {
                    ObjectParser[result].ClassifiedAdd(obj);
                }
            }
            return ObjectParser;
        }

        public bool TrySetOuterMosetrect(out RectangleF rect)
        {
            RectangleF outerMosetrect = new RectangleF();
            PointF endpoint = new PointF();
            foreach (var obj in ObjectParser)
            {
                outerMosetrect.X = outerMosetrect.X < obj.Value.outerRect.X ? outerMosetrect.X : obj.Value.outerRect.X;
                outerMosetrect.Y = outerMosetrect.Y < obj.Value.outerRect.Y ? outerMosetrect.Y : obj.Value.outerRect.Y;
                endpoint.X = endpoint.X > obj.Value.outerRect.Right ? endpoint.X : obj.Value.outerRect.Right;
                endpoint.Y = endpoint.Y > obj.Value.outerRect.Bottom ? endpoint.Y : obj.Value.outerRect.Bottom;
            }
            outerMosetrect.Width = endpoint.X - outerMosetrect.X;
            outerMosetrect.Height = endpoint.Y - outerMosetrect.Y;

            rect = OuterMosetrect = outerMosetrect;
            if (rect.Width == 0 || rect.Height == 0)
            {
                this.ErrorMessage = $"[ERROR] Width = {rect.Width}, Height = {rect.Height}";
                return false;
            }
            return true;
        }

        private List<DxfObject> GetLayerList(DxfDocument dxf)
        {
            var dst = new List<DxfObject>();
            foreach (Linetype lType in dxf.Linetypes)
            {
                foreach (DxfObject o in dxf.Linetypes.GetReferences(lType))
                {
                    if (o is Layer)
                    {
                        if (ReferenceEquals(lType, ((Layer)o).Linetype))
                        {
                            dst = ((Layer)o).Linetype.Owner.GetReferences("ByLayer");
                        }
                    }
                }
            }
            return dst;
        }
        #endregion
    }
}
