using AutoCADFileViewer.Base;
using AutoCADFileViewer.Base.MatExtension;
using Emgu.CV;
using netDxf;
using netDxf.Blocks;
using netDxf.Entities;
using netDxf.Objects;
using netDxf.Tables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DxfObject = netDxf.DxfObject;
using Image = netDxf.Entities.Image;
using Line = netDxf.Entities.Line;
using ShapeType = AutoCADFileViewer.Base.DxfHelper.ShapeType;

namespace AutoCADFileViewer
{
    public partial class Form1 : Form
    {
        #region 필드 
        DxfHelper dxfHelper;
        RectangleF maxRect;
        #endregion

        #region 생성자 
        public Form1()
        {
            InitializeComponent();
            textBoxScale.Text = "50.0";
        }
        #endregion

        #region 메서드 
        private void ContoursAnalysis(Mat img)
        {
            Mat hierarchy = new Mat();
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
            CvInvoke.FindContours(img, contours, hierarchy, Emgu.CV.CvEnum.RetrType.Tree, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxNone);
            var tp = contours.ToArrayOfArray();
            Dictionary<int, List<System.Drawing.Point[]>> tpPT = new Dictionary<int, List<System.Drawing.Point[]>>();
            var splitHierarchy = hierarchy.Split();
            for (int i = 0; i < hierarchy.Cols; ++i)
            {
                var col = hierarchy.Col(i);
                var next = MatExtension.GetValue(splitHierarchy[0], 0, i);
                var previos = MatExtension.GetValue(splitHierarchy[1], 0, i);
                var firstChild = MatExtension.GetValue(splitHierarchy[2], 0, i);
                var parent = MatExtension.GetValue(splitHierarchy[3], 0, i);
                if (tpPT.ContainsKey(firstChild))
                {
                    System.Drawing.Point[] pts = new System.Drawing.Point[tp[i].Length];
                    tpPT[firstChild].Add(pts);
                }
                else
                {
                    System.Drawing.Point[] pts = new System.Drawing.Point[tp[i].Length];
                    List<System.Drawing.Point[]> ptsList = new List<System.Drawing.Point[]>() { pts };
                    //tpPT[firstChild].Add(pts);
                    tpPT.Add(firstChild, ptsList);
                }

            }
        }
        private void ClassifedAddDxfHelper(DxfHelper dxfHelper)
        {
            dxfHelper.AddObjectParser(ShapeType.line, new Base.Object.Shape.Line());
            dxfHelper.AddObjectParser(ShapeType.lwpolyline, new Base.Object.Shape.Polygon());
            dxfHelper.AddObjectParser(ShapeType.circle, new Base.Object.Shape.Circle());
            dxfHelper.AddObjectParser(ShapeType.spline, new Base.Object.Shape.Spline());
            dxfHelper.AddObjectParser(ShapeType.point, new Base.Object.Shape.Dot());
            Dictionary<ShapeType, Base.Object.ObjectParser> paser = dxfHelper.ClassifedAddObjectPaser();
        }
        private Mat ImageSet(DxfHelper dxfHelper, RectangleF maxRect, float mul = 10.0f)
        {
            maxRect.Width *= mul;
            maxRect.Height *= mul;
            int w = (int)Math.Ceiling(maxRect.Width);
            int h = (int)Math.Ceiling(maxRect.Height);
            Rectangle roiRect = new Rectangle(0, 0, w, h);
            Emgu.CV.Mat dstImage = new Emgu.CV.Mat(h, w, Emgu.CV.CvEnum.DepthType.Cv8U, 1);
            dstImage.SetTo(new Emgu.CV.Structure.MCvScalar(0));

            var polygonPointList = (dxfHelper.ObjectParser[ShapeType.lwpolyline] as Base.Object.Shape.Polygon)?.GetPolygonPointList(mul, new PointF(maxRect.X, maxRect.Y));
            foreach (var pt in polygonPointList)
            {
                CvInvoke.Polylines(dstImage, pt, true, new Emgu.CV.Structure.MCvScalar(255));
            }

            var splinePointList = (dxfHelper.ObjectParser[ShapeType.spline] as Base.Object.Shape.Spline)?.GetSplinePointList(mul, new PointF(maxRect.X, maxRect.Y));
            foreach (var pt in splinePointList)
            {
                CvInvoke.Polylines(dstImage, pt, true, new Emgu.CV.Structure.MCvScalar(255));
            }

            var circleRotatedRectList = (dxfHelper.ObjectParser[ShapeType.circle] as Base.Object.Shape.Circle)?.GetCircleRotatedRectList(mul, new PointF(maxRect.X, maxRect.Y));
            foreach (var rect in circleRotatedRectList)
            {
                CvInvoke.Ellipse(dstImage, rect, new Emgu.CV.Structure.MCvScalar(255));
            }

            var lineList = (dxfHelper.ObjectParser[ShapeType.line] as Base.Object.Shape.Line)?.GetLinePointList(mul, new PointF(maxRect.X, maxRect.Y));
            foreach (var pt in lineList)
            {
                CvInvoke.Line(dstImage, pt.Item1, pt.Item2, new Emgu.CV.Structure.MCvScalar(255));
            }

            var dotList = (dxfHelper.ObjectParser[ShapeType.point] as Base.Object.Shape.Dot)?.GetDotPointList(mul, new PointF(maxRect.X, maxRect.Y));
            foreach (var dot in dotList)
            {
                CvInvoke.Line(dstImage, dot, dot, new Emgu.CV.Structure.MCvScalar(255), 10);
            }

            CvInvoke.Flip(dstImage, dstImage, Emgu.CV.CvEnum.FlipType.Vertical);
            return dstImage;
        }
        private void SetThumbImage(Mat image)
        {
            Mat thumbImage = new Mat();
            System.Drawing.Size drawingSize = new System.Drawing.Size();
            float ratio = (float)image.Width / image.Height;
            if (ratio > 1.0f)
            {
                drawingSize.Width = pictureBoxCADImage.Width;
                drawingSize.Height = (int)(pictureBoxCADImage.Height / ratio);
            }
            else
            {
                drawingSize.Width = (int)(pictureBoxCADImage.Width * ratio);
                drawingSize.Height = pictureBoxCADImage.Height;
            }
            CvInvoke.Resize(image, thumbImage, drawingSize);
            pictureBoxCADImage.Image = thumbImage.Bitmap;
        }
        private static void CheckReferences()
        {
            DxfDocument dxf = new DxfDocument();

            Layer layer1 = new Layer("Layer1");
            layer1.Color = AciColor.Blue;
            layer1.Linetype = Linetype.Center;

            Layer layer2 = new Layer("Layer2");
            layer2.Color = AciColor.Red;

            LwPolyline poly = new LwPolyline();
            poly.Vertexes.Add(new LwPolylineVertex(0, 0));
            poly.Vertexes.Add(new LwPolylineVertex(10, 10));
            poly.Vertexes.Add(new LwPolylineVertex(20, 0));
            poly.Vertexes.Add(new LwPolylineVertex(30, 10));
            poly.Layer = layer1;
            dxf.AddEntity(poly);

            Ellipse ellipse = new Ellipse(new Vector3(2, 2, 0), 5, 3);
            ellipse.Rotation = 30;
            ellipse.Layer = layer1;
            dxf.AddEntity(ellipse);

            Line line = new Line(new Vector2(10, 5), new Vector2(-10, -5));
            line.Layer = layer2;
            line.Linetype = Linetype.DashDot;
            dxf.AddEntity(line);

            dxf.Save("test.dxf");

            dxf = DxfDocument.Load("test.dxf");

            foreach (ApplicationRegistry registry in dxf.ApplicationRegistries)
            {
                foreach (DxfObject o in dxf.ApplicationRegistries.GetReferences(registry))
                {
                    if (o is EntityObject)
                    {
                        foreach (KeyValuePair<string, XData> data in ((EntityObject)o).XData)
                        {
                            if (data.Key == registry.Name)
                                if (!ReferenceEquals(registry, data.Value.ApplicationRegistry))
                                    Console.WriteLine("Application registry {0} not equal entity to {1}", registry.Name, o.CodeName);
                        }
                    }
                }
            }

            foreach (Block block in dxf.Blocks)
            {
                foreach (DxfObject o in dxf.Blocks.GetReferences(block))
                {
                    if (o is Insert)
                        if (!ReferenceEquals(block, ((Insert)o).Block))
                            Console.WriteLine("Block {0} not equal entity to {1}", block.Name, o.CodeName);
                }
            }

            foreach (ImageDefinition def in dxf.ImageDefinitions)
            {
                foreach (DxfObject o in dxf.ImageDefinitions.GetReferences(def))
                {
                    if (o is Image)
                        if (!ReferenceEquals(def, ((Image)o).Definition))
                            Console.WriteLine("Image definition {0} not equal entity to {1}", def.Name, o.CodeName);
                }
            }

            foreach (DimensionStyle dimStyle in dxf.DimensionStyles)
            {
                foreach (DxfObject o in dxf.DimensionStyles.GetReferences(dimStyle))
                {
                    if (o is Dimension)
                        if (!ReferenceEquals(dimStyle, ((Dimension)o).Style))
                            Console.WriteLine("Dimension style {0} not equal entity to {1}", dimStyle.Name, o.CodeName);
                }
            }

            foreach (Group g in dxf.Groups)
            {
                foreach (DxfObject o in dxf.Groups.GetReferences(g))
                {
                    // no references
                }
            }

            foreach (UCS u in dxf.UCSs)
            {
                foreach (DxfObject o in dxf.UCSs.GetReferences(u))
                {
                }
            }

            foreach (TextStyle style in dxf.TextStyles)
            {
                foreach (DxfObject o in dxf.TextStyles.GetReferences(style))
                {
                    if (o is Text)
                        if (!ReferenceEquals(style, ((Text)o).Style))
                            Console.WriteLine("Text style {0} not equal entity to {1}", style.Name, o.CodeName);

                    if (o is MText)
                        if (!ReferenceEquals(style, ((MText)o).Style))
                            Console.WriteLine("Text style {0} not equal entity to {1}", style.Name, o.CodeName);

                    if (o is DimensionStyle)
                        if (!ReferenceEquals(style, ((DimensionStyle)o).TextStyle))
                            Console.WriteLine("Text style {0} not equal entity to {1}", style.Name, o.CodeName);
                }
            }

            foreach (Layer layer in dxf.Layers)
            {
                foreach (DxfObject o in dxf.Layers.GetReferences(layer))
                {
                    if (o is Block)
                        if (!ReferenceEquals(layer, ((Block)o).Layer))
                            Console.WriteLine("Layer {0} not equal entity to {1}", layer.Name, o.CodeName);
                    if (o is EntityObject)
                        if (!ReferenceEquals(layer, ((EntityObject)o).Layer))
                            Console.WriteLine("Layer {0} not equal entity to {1}", layer.Name, o.CodeName);
                }
            }

            foreach (Linetype lType in dxf.Linetypes)
            {
                foreach (DxfObject o in dxf.Linetypes.GetReferences(lType))
                {
                    if (o is Layer)
                        if (!ReferenceEquals(lType, ((Layer)o).Linetype))
                            Console.WriteLine("Line type {0} not equal to {1}", lType.Name, o.CodeName);
                    if (o is MLineStyle)
                    {
                        foreach (MLineStyleElement e in ((MLineStyle)o).Elements)
                        {
                            if (!ReferenceEquals(lType, e.Linetype))
                                Console.WriteLine("Line type {0} not equal to {1}", lType.Name, o.CodeName);
                        }
                    }
                    if (o is EntityObject)
                        if (!ReferenceEquals(lType, ((EntityObject)o).Linetype))
                            Console.WriteLine("Line type {0} not equal entity to {1}", lType.Name, o.CodeName);
                }
            }

            Console.WriteLine("Press a key to continue...");
            Console.ReadKey();
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
        private void buttonImageFileLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "bmp";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;

            ImageDefinition imageDefinition = new ImageDefinition("MyImage", dlg.FileName);
            string fileFormatChange = dlg.SafeFileName.Replace(".bmp", ".dxf");
            string savePath = dlg.FileName.Replace(dlg.SafeFileName, "") + fileFormatChange;
            Image image = new Image(imageDefinition, Vector2.Zero, imageDefinition.Width, imageDefinition.Height);
            DxfDocument doc = new DxfDocument();
            doc.AddEntity(image);
            doc.Save(savePath);
        }
        private void textBoxScale_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBoxScale.Text, out float scale))
            {
                float width = 440.987f;
                float mm = scale / 1000.0f; //(1pixel/mm)
                float mul = width / mm / maxRect.Width;
                labelSaveInfo.Text = "[SaveImage]\r\n";
                labelSaveInfo.Text += $"Width:{Math.Ceiling(mul * maxRect.Width)}\r\n";
                labelSaveInfo.Text += $"Height:{Math.Ceiling(mul * maxRect.Height)}\r\n";
            }
        }
        private void buttonCadFileLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = "dxf"
            };

            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;

            dxfHelper = new DxfHelper(dlg.FileName);
            if (!dxfHelper.LoadSuccess)
            {
                MessageBox.Show(dxfHelper.ErrorMessage);
                return;
            }

            ClassifedAddDxfHelper(dxfHelper);
            if (!dxfHelper.TrySetOuterMosetrect(out maxRect))
            {
                MessageBox.Show(dxfHelper.ErrorMessage);
            }
            else
            {
                if (float.TryParse(textBoxScale.Text, out float mul))
                {
                    Mat dstImage = ImageSet(dxfHelper, maxRect);
                    SetThumbImage(dstImage);
                }

            }

            labelRect.Text = $"Width:{maxRect.Width}, Height:{maxRect.Height}";

            //440.987mm -> 290.2 (vector)
            //1Pixel -> 3.8um (0.0038mm)
            if (float.TryParse(textBoxScale.Text, out float scale))
            {
                float width = 440.987f;
                float mm = scale / 1000.0f; //(1pixel/mm)
                float mul = width / mm / maxRect.Width;
                labelSaveInfo.Text = "[ImageSize]\r\n";
                labelSaveInfo.Text += $"Width:{Math.Ceiling(mul * maxRect.Width)}\r\n";
                labelSaveInfo.Text += $"Height:{Math.Ceiling(mul * maxRect.Height)}\r\n";

            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (float.TryParse(textBoxScale.Text, out float mul))
            {
                float width = 440.987f;
                float mm = mul / 1000.0f; //(1pixel/mm)
                float reaulMul = width / mm / maxRect.Width;
                Mat ExportImage = ImageSet(dxfHelper, maxRect, reaulMul);
                ExportImage.Save("D:/ExportImage.bmp");
                MessageBox.Show("SaveOK");
            }
        }
        #endregion


    }
}
