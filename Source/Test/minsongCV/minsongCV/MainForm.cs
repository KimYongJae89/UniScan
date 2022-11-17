using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DynMvp.Vision;
using DynMvp.Base;
using DynMvp.UI;
using System.Windows;
using DynMvp.Data.UI;
using System.Drawing.Imaging;
using DynMvp.Vision.OpenCv;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace minsongCV
{

  

    public partial class MainForm : Form
    {
        Image loaded_image;
        AlgoImage lastImage = null;
        AlgoImage OriginImage = null;

        AlgoImage DiffAbsImage = null; //Whole Area size

        AlgoImage ReferenceImage = null; //average : Electrode
        AlgoImage ReferenceEdgeImage = null; //edge , its wait
        AlgoImage ReferenceBinaryImage = null; //Binary

        CanvasPanel canvasPanel = null;
        BlobRectList foundedBlobList = new BlobRectList();
        public MainForm()
        {
            InitializeComponent();

            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.SetPanMode();
            this.canvasPanel.ShowCenterGuide = false;
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.SizeChanged += View_SizeChanged;
            //this.canvasPanel.NoneClickMode = false;
            this.canvasPanel.BackColor = Color.CornflowerBlue;
            this.canvasPanel.MouseWheel += new MouseEventHandler(canvasPanel_MouseWheel);
            panel1.Controls.Add(this.canvasPanel);


            //canvasPanel.ClearFigure();
            //canvasPanel.WorkingFigures.AddFigure(figureGroup);

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        private void button_ZoomIn_Click(object sender, EventArgs e)
        {
            canvasPanel.ZoomIn();
            canvasPanel.Invalidate();
        }

        private void button_ZoomOut_Click(object sender, EventArgs e)
        {
            canvasPanel.ZoomOut();
            canvasPanel.Invalidate();
        }
        private void canvasPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            int lines = e.Delta * SystemInformation.MouseWheelScrollLines / 120;

            if (lines > 0)
            {
                canvasPanel.ZoomIn();
                canvasPanel.Invalidate();
            }
            else if (lines < 0)
            {
                canvasPanel.ZoomOut();
                canvasPanel.Invalidate();
            }
        }
        private void View_SizeChanged(object sender, EventArgs e)
        {
            canvasPanel.ZoomFit();
        }
        private void ClearView()
        {
            canvasPanel.Clear();
            canvasPanel.UpdateImage(null);
            canvasPanel.Invalidate();
        }
        void DrawCanvasPanel(BlobRectList blobRectList, Bitmap bitmap)
        {
            FigureGroup figureGroup = new FigureGroup();
            DrawBlob(blobRectList, figureGroup, bitmap);
            DrawGetMostPrintImage(blobRectList.GetList(), figureGroup, bitmap);

            canvasPanel.Clear();
            canvasPanel.WorkingFigures.AddFigure(figureGroup);
            canvasPanel.UpdateImage(bitmap);
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdFile = new OpenFileDialog();
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                string ResultPath = ofdFile.FileName;
                //this.Visible = false; //폼 감추기 
                if (loaded_image != null)
                {
                    loaded_image.Dispose();
                    loaded_image = null;
                }


                //using (FileStream fs = new FileStream(ResultPath, FileMode.Open, FileAccess.Read))
                //{
                //    loaded_image = Image.FromStream(fs);

                //}
                loaded_image = Image.FromFile(ResultPath); //얘는 메모리 문제가 없네.
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }
                pictureBox1.Image = loaded_image;
            }
            //loaded_image = Image.FromFile(@"d:\\stopimage.bmp"); //얘는 메모리 문제가 없네.


            Image2D imageD = Image2D.FromBitmap((Bitmap)loaded_image);
            lastImage = ImageBuilder.Build(ImagingLibrary.OpenCv, imageD, ImageType.Grey);
            OriginImage = lastImage.Clone();
            //pictureBox1.Image = lastImage.ToImageD().ToBitmap();
            canvasPanel.UpdateImage(lastImage.ToImageD().ToBitmap());
        }
        private void button_OpenFixed_Click(object sender, EventArgs e)
        {

            loaded_image = Image.FromFile(@"d:\\stopimage_crop.bmp");


            Image2D imageD = Image2D.FromBitmap((Bitmap)loaded_image);
            lastImage = ImageBuilder.Build(ImagingLibrary.OpenCv, imageD, ImageType.Grey);
            OriginImage = lastImage.Clone();

            //var subimg = lastImage.GetSubImage(new Rectangle(50, 50, 40, 30));
            //subimg.Clear(255);

            pictureBox1.Image = lastImage.ToImageD().ToBitmap();
            canvasPanel.UpdateImage(lastImage.ToImageD().ToBitmap());
        }
        private double[] getMTF(Image image)
        {
            int w = image.Width;
            int h = image.Height;

            byte[] imgar = imageToByteArray(image);
            double[] mtf = new double[w];

            int step = 3 * 2;
            // byte[] calbuf = new byte[step];
            for (int i = 0; i < w; i++)
            {
                // byte[] temp= imgar[i];
            }
            return mtf;
        }
        //C# 이미지를 byte 배열로 바꾸는 법 
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            return ms.ToArray();
        }

        //C# byte 배열를 image 로 바꾸는 법 
        public Image ByteArrayToImage(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            Image recImg = Image.FromStream(ms);
            return recImg;
        }//출처: http://itnhappy.tistory.com/89 [즐거운 IT 연구개발을 위해]

        private void button_Binary_Click(object sender, EventArgs e)
        {
            //AlgoImage srcSubImage = algoImage.GetSubImage(searchRect);

            ImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage);
            AlgoImage algimage = lastImage.Clone();
            ipc.Binarize(algimage);
            ipc.Binarize(algimage, true);
            lastImage = algimage;
            pictureBox1.Image = lastImage.ToImageD().ToBitmap();
        }

        private void button_Median_Click(object sender, EventArgs e)
        {
            ImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage);
            AlgoImage algimage = lastImage.Clone();
            ipc.Median(lastImage, algimage, 3);
            lastImage = algimage;
            pictureBox1.Image = lastImage.ToImageD().ToBitmap();
        }

        private void button_Erode_Click(object sender, EventArgs e)
        {
            ImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage);
            ipc.Erode(lastImage, 1);
            pictureBox1.Image = lastImage.ToImageD().ToBitmap();
        }

        private void button_Dilate_Click(object sender, EventArgs e)
        {
            ImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage);
            ipc.Dilate(lastImage, 1);
            pictureBox1.Image = lastImage.ToImageD().ToBitmap();
        }

        private void button_Average_Click(object sender, EventArgs e)
        {
            ImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage);

            AlgoImage algimage = ImageBuilder.BuildSameTypeSize(lastImage);
            ipc.Average(lastImage, algimage);
            lastImage = algimage;
            pictureBox1.Image = lastImage.ToImageD().ToBitmap();
        }

        private void button_AND_Click(object sender, EventArgs e)
        {
            ImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage);
            AlgoImage algimage = ImageBuilder.BuildSameTypeSize(lastImage);

            ipc.And(OriginImage, lastImage, algimage);

            lastImage = algimage;
            pictureBox1.Image = lastImage.ToImageD().ToBitmap();
        }

        private void button_Mutiplication_Click(object sender, EventArgs e)
        {
            ImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage);
            AlgoImage algimage = ImageBuilder.BuildSameTypeSize(lastImage);

            ipc.Mul(lastImage, algimage, 2);

            lastImage = algimage;
            // pictureBox1.Image = lastImage.ToImageD().ToBitmap();
            canvasPanel.UpdateImage(lastImage.ToBitmap());
        }

        private void button_Edge_Click(object sender, EventArgs e)
        {
            ImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage);
            AlgoImage algimage = ImageBuilder.BuildSameTypeSize(lastImage);

            ipc.Sobel(lastImage, algimage); //
            //ipc.Sobel(lastImage);

            lastImage = algimage;
            //pictureBox1.Image = lastImage.ToBitmap();
            canvasPanel.UpdateImage(lastImage.ToBitmap());
        }

        private void button_Blob_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            // 블랍을 얻는다.
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage) as OpenCvImageProcessing;
            AlgoImage algimage = lastImage.Clone();
            AlgoImage blobimage = ImageBuilder.BuildSameTypeSize(lastImage);

            BlobParam blobParam = new BlobParam();
            blobParam.SelectArea = true;
            blobParam.SelectCenterPt = true;
            blobParam.SelectBoundingRect = true;


            DrawBlobOption drawBlobOption = new DrawBlobOption();
            drawBlobOption.SelectBlob = true;

            BlobRectList blobRectList = ipc.Blob(algimage, blobParam, blobimage);
            //ipc.DrawBlob(algimage, blobRectList, null, drawBlobOption);
            SaveTextBlob(blobRectList);
            DeleteEdgeBolb(blobRectList, algimage);

            BlobRectList blobRectList2 = new BlobRectList();
            //대표(중요) 인쇄형상 찾기
            blobRectList2.SetBlobRectList(FindOutMostPrint(blobRectList.GetList()));

            foundedBlobList.SetBlobRectList(blobRectList2.GetList());

            //대표(중요) 인쇄형상 평균 이미지 (내부 필터링 된)
            ReferenceImage = GetMostPrintImage(blobRectList2.GetList(), OriginImage);

            ReferenceEdgeImage = ImageBuilder.BuildSameTypeSize(ReferenceImage);

            ipc.Sobel(ReferenceImage, ReferenceEdgeImage); //
            //lastImage = algimage;

            pictureBox1.Image = ReferenceImage.ToBitmap();
            DrawCanvasPanel(blobRectList2, algimage.ToBitmap());

            //pictureBox1.Image = lastImage.ToBitmap();
            //DrawCanvasPanel(blobRectList2, algimage.ToBitmap());
            ////canvasPanel.UpdateImage(algimage.ToBitmap());
        }
        //public List<AlgoImage> GetImageList(AlgoImage srcImag, List<BlobRect> blobList) 
        //{
        //    List<AlgoImage> dstList = new List<AlgoImage>();

        //    foreach( var blob in blobList)
        //    {
        //        srcImag.GetSubImage()
        //    }
        //    return dstList;
        //}
        void SaveTextBlob(BlobRectList blobRectList, string filename = "")
        {
            if (filename == "") filename = "bloblist.csv";
            StreamWriter sw = new StreamWriter(new FileStream(filename, FileMode.Create));
            foreach (BlobRect blob in blobRectList)
            {
                sw.WriteLine(string.Join(",", blob.Area.ToString(), blob.CenterPt.ToString(), blob.BoundingRect.ToString()));

                //sw.WriteLine(int.MaxValue);
                //sw.WriteLine("Good morning!");
                //sw.WriteLine(uint.MaxValue);
                //sw.WriteLine("안녕하세요!");
                //sw.WriteLine(double.MaxValue);

            }
            sw.Close();
        }

        void DeleteEdgeBolb(BlobRectList blobRectList, AlgoImage algimage)
        {
            int eX = algimage.Width;// - 1; // 이상하다.. 왜??? 블랍 좌표가 안맞네...하 참....
            int eY = algimage.Height;// - 1;

            //var delist = blobRectList.GetList().FindAll(f =>
            // (f.BoundingRect.Left == 0 | f.BoundingRect.Right == eX | f.BoundingRect.Top == 0 | f.BoundingRect.Bottom == eY)
            //);

            blobRectList.SetBlobRectList(
                blobRectList.GetList().FindAll(f =>
             !(f.BoundingRect.Left == 0 | f.BoundingRect.Right == eX | f.BoundingRect.Top == 0 | f.BoundingRect.Bottom == eY)
            )
            );


        }
        void SortBlobs(BlobRectList blobRectList, AlgoImage algimage)
        {
            BlobRect temp = null;
            int eX = algimage.Width;// - 1; // 이상하다.. 왜??? 블랍 좌표가 안맞네...하 참....
            int eY = algimage.Height;// - 1;


            //1. 외각 블랍 제거
            List<BlobRect> list = blobRectList.GetList().FindAll(f =>
            !(f.BoundingRect.Left == 0 | f.BoundingRect.Right == eX | f.BoundingRect.Top == 0 | f.BoundingRect.Bottom == eY)
            );
            //2. 리스트의  Area별 분류 후 -> 가장 넓은 Area 면적을 차지하는(주전극 형상) 리스트만 취함.

            //3. 주전극 형상을 가로 사이즈로 정렬후 // 그룹핑
            //4. 

        }


        private void DrawBlob(BlobRectList blobRectList, FigureGroup figureGroup, Bitmap bitmap)
        {
            if (bitmap == null) return;

            Pen pen1 = new Pen(Color.FromArgb(128, 0, 255, 0), 1);
            Brush brush = new SolidBrush(Color.FromArgb(128, 0, 128, 0));
            Pen pen2 = new Pen(Color.Red, 2);


            foreach (var blob in blobRectList)
            {

                figureGroup.AddFigure(new RectangleFigure(blob.BoundingRect, pen1, brush));
                figureGroup.AddFigure(new LineFigure(
                    new PointF(blob.CenterPt.X - 2, blob.CenterPt.Y - 2),
                    new PointF(blob.CenterPt.X + 2, blob.CenterPt.Y + 2),
                    pen2));

                //figureGroup.AddFigure(new LineFigure(
                //    new PointF(blob.BoundingRect.Left, blob.BoundingRect.Top),
                //    new PointF(blob.BoundingRect.Right, blob.BoundingRect.Top),
                //    pen1));

                TextFigure text = new TextFigure(blob.LabelNumber.ToString(),
                    new Point((int)blob.BoundingRect.Left, (int)blob.BoundingRect.Top),
                    new Font(FontFamily.GenericSerif, 10), Color.Yellow); ;
                figureGroup.AddFigure(text);
            }
        }
        private void DrawGetMostPrintImage(List<BlobRect> blobRects, FigureGroup figureGroup, Bitmap bitmap)
        {
            AlgoImage MostImage = null;
            List<AlgoImage> listimage = new List<AlgoImage>();
            Pen pen1 = new Pen(Color.FromArgb(255, 255, 0, 0), 1);
            Brush brush = new SolidBrush(Color.FromArgb(128, 128, 0, 0));

            int bias = 20;
            int width = 0;
            int height = 0;
            int maxW = 0, maxH = 0;
            foreach (var blob in blobRects)
            {
                if (maxW < blob.BoundingRect.Width) maxW = (int)(blob.BoundingRect.Width + 0.5);
                if (maxH < blob.BoundingRect.Height) maxH = (int)(blob.BoundingRect.Height + 0.5);
            }
            width = maxW + bias * 2;
            height = maxH + bias * 2;

            int sX, sY, eX, eY;
            int ccc = 0;
            foreach (var blob in blobRects)
            {
                ccc++;
                sX = (int)(blob.CenterPt.X - (float)width / 2 + 0.5f);
                sY = (int)(blob.CenterPt.Y - (float)height / 2 + 0.5f);
                eX = sX + width;
                eY = sY + height;
                if (sX < 0 || sY < 0 || eX >= bitmap.Width || eY >= bitmap.Height)
                    continue;
                Rectangle rc = new Rectangle(sX, sY, width, height);
                //var cropImage = srcimg.GetSubImage(rc);
                //listimage.Add(cropImage);

                figureGroup.AddFigure(new RectangleFigure(rc, pen1, brush));
                //cropImage.Save("c:\\Test\\crop" + string.Format(ccc.ToString() + ".bmp"));
            }
        }
        private AlgoImage GetMostPrintImage(List<BlobRect> blobRects, AlgoImage srcimg)
        {
            AlgoImage MostImage = null;

            List<AlgoImage> listimage = new List<AlgoImage>();
            if (blobRects == null) return null;

            int bias = 20;
            int width = 0;
            int height = 0;
            int maxW = 0, maxH = 0;
            foreach (var blob in blobRects)
            {
                if (maxW < blob.BoundingRect.Width) maxW = (int)(blob.BoundingRect.Width + 0.5);
                if (maxH < blob.BoundingRect.Height) maxH = (int)(blob.BoundingRect.Height + 0.5);
            }
            width = maxW + bias * 2;
            height = maxH + bias * 2;

            int sX, sY, eX, eY;
            int ccc = 0;
            foreach (var blob in blobRects)
            {
                ccc++;
                sX = (int)(blob.CenterPt.X - (float)width / 2 + 0.5f);
                sY = (int)(blob.CenterPt.Y - (float)height / 2 + 0.5f);
                eX = sX + width;
                eY = sY + height;
                if (sX < 0 || sY < 0 || eX >= srcimg.Width || eY >= srcimg.Height)
                    continue;
                Rectangle rc = new Rectangle(sX, sY, width, height);
                var cropImage = srcimg.GetSubImage(rc);
                listimage.Add(cropImage);

                //cropImage.Save("c:\\Test\\crop" + string.Format(ccc.ToString() + ".bmp"));
            }
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage) as OpenCvImageProcessing;
            MostImage = ipc.GetAverageFilteredImage(listimage);
            MostImage?.Save("c:\\Test\\Most.bmp");

            return MostImage;
        }
        //find out most significant shape
        private List<BlobRect> FindOutMostPrint(List<BlobRect> blobRects)
        {
            int minCount = 1; //10개 이하는 삭제
            BlobRectList blobRectList = new BlobRectList();
            //1. Area 정렬
            List<List<BlobRect>> classfylist = ClassifyAfterSort(blobRects, new Func<BlobRect, double>(f => f.Area), 1000);
            classfylist.RemoveAll(f => f.Count <= minCount);
            classfylist.Sort((f, g) => (f.Count * f.Sum<BlobRect>(x => x.Area)).CompareTo(g.Count * g.Sum<BlobRect>(y => y.Area)));
            if (classfylist.Count() == 0) return null;
            List<BlobRect> mostList = classfylist.Last<List<BlobRect>>();
            blobRectList.SetBlobRectList(mostList);
            SaveTextBlob(blobRectList, "area.csv");

            //2. 주그룹 다시 Width 정렬
            classfylist = ClassifyAfterSort(mostList, new Func<BlobRect, double>(f => f.BoundingRect.Width), 10);
            classfylist.RemoveAll(f => f.Count <= minCount);
            classfylist.Sort((f, g) => (f.Count * f.Sum<BlobRect>(x => x.BoundingRect.Width)).CompareTo(g.Count * g.Sum<BlobRect>(y => y.BoundingRect.Width)));
            if (classfylist.Count() == 0) return null;
            mostList = classfylist.Last<List<BlobRect>>();
            blobRectList.SetBlobRectList(mostList);
            SaveTextBlob(blobRectList, "widht.csv");

            //3. 주 그룹 다시 Height 정렬
            classfylist = ClassifyAfterSort(mostList, new Func<BlobRect, double>(f => f.BoundingRect.Height), 10);
            classfylist.RemoveAll(f => f.Count <= minCount);
            classfylist.Sort((f, g) => (f.Count * f.Sum<BlobRect>(x => x.BoundingRect.Height)).CompareTo(g.Count * g.Sum<BlobRect>(y => y.BoundingRect.Height)));
            if (classfylist.Count() == 0) return null;
            mostList = classfylist.Last<List<BlobRect>>();
            blobRectList.SetBlobRectList(mostList);
            SaveTextBlob(blobRectList, "height.csv");

            //4. 주 그룹 다시 notch 정렬
            classfylist = ClassifyAfterSort(mostList, new Func<BlobRect, double>(f => f.BoundingRect.Size.Width), 10);
            classfylist.RemoveAll(f => f.Count <= minCount);
            classfylist.Sort((f, g) => (f.Count * f.Sum<BlobRect>(x => x.BoundingRect.Size.Width)).CompareTo(g.Count * g.Sum<BlobRect>(y => y.BoundingRect.Size.Width)));
            if (classfylist.Count() == 0) return null;
            mostList = classfylist.Last<List<BlobRect>>();
            blobRectList.SetBlobRectList(mostList);
            SaveTextBlob(blobRectList, "notch.csv");

            //
            return mostList; //most significant shape
        }

        //compareItem 으로 비교 정렬 후 
        //±seperation 값 이내에 있는것들 끼리 분류(그룹화)후 list of list 생성하여 반환
        private List<List<BlobRect>> ClassifyAfterSort(List<BlobRect> blobRects, Func<BlobRect, double> compareItem, double seperation)
        {
            //1.  정렬
            blobRects.Sort((f, g) => compareItem(f).CompareTo(compareItem(g)));

            List<List<BlobRect>> classList = new List<List<BlobRect>>();
            List<BlobRect> tempList = new List<BlobRect>();
            foreach (BlobRect blobrect in blobRects) //이미 사이즈 순으로 정렬되어있음.
            //blobRects.ForEach(f =>
            {
                if (tempList.Count > 0)
                {
                    double feature = compareItem(blobrect);

                    double average = tempList.Average(compareItem);
                    double lower = average - seperation;
                    double upper = average + seperation;

                    if (!(lower < feature && feature < upper)) //전에 그룹평균값과 다른!! 패턴이면.
                    {
                        classList.Add(tempList); //기존 패턴 등록
                        tempList = new List<BlobRect>(); //신규패턴으로 다시 시작.
                    }
                }
                tempList.Add(blobrect);
            };

            //마지막 그룹 넣고..
            if (tempList.Count > 0)
                classList.Add(tempList);

            return classList;
        }

        void Test()
        {
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage) as OpenCvImageProcessing;
            //AlgoImage maskimage = ImageBuilder.Build(lastImage);
            AlgoImage maskimage = lastImage.Clone();
            ipc.Binarize(maskimage, true);
            AlgoImage destImage = ImageBuilder.BuildSameTypeSize(lastImage);
            ipc.And(maskimage, OriginImage, destImage);
            double blackaverage = ipc.AverageMask(OriginImage, maskimage);

            ipc.Binarize(maskimage, true);
            ipc.And(maskimage, OriginImage, destImage);
            double whiteaverage = ipc.AverageMask(OriginImage, maskimage);

            //lastImage = algimage;
            pictureBox1.Image = OriginImage.ToImageD().ToBitmap();
            canvasPanel.UpdateImage(destImage.ToImageD().ToBitmap());

        }

        private void button_Test_Click(object sender, EventArgs e)
        {
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage) as OpenCvImageProcessing;
            int cw = 5;
            int ch = 200;
            lastImage = ipc.MakeDiffImage(lastImage, new Size(cw, ch));

            lastImage.Save(string.Format("D:\\test\\color\\{0},{1}.bmp", cw, ch));
            //pictureBox1.Image = lastImage.ToImageD().ToBitmap();
            canvasPanel.UpdateImage(lastImage.ToBitmap());

            var srcImage = lastImage;

            var imgsrc = srcImage.GetByte();
            var height = srcImage.Height;
            var width = srcImage.Width;
            var pitch = srcImage.Pitch;
            TDI tdic = new TDI();
        }


        private void InspectBlob(BlobRectList blobRectList)
        {
            DiffAbsImage = ImageBuilder.BuildSameTypeSize(OriginImage);  //Whole Area size
            DiffAbsImage.Clear(255);

            int Threshold = 10;
            //ReferenceImage = null; //average : Electrode
            ReferenceEdgeImage = null; //edge , its wait
            ReferenceBinaryImage = null; //Binary

            int sX, sY, eX, eY;
            int ccc = 0;

            int RoiW = ReferenceImage.Width;
            int RoiH = ReferenceImage.Height;

            foreach (var blob in blobRectList)
            {
                ccc++;
                sX = (int)(blob.CenterPt.X - (float)RoiW / 2 + 0.5f);
                sY = (int)(blob.CenterPt.Y - (float)RoiH / 2 + 0.5f);
                eX = sX + RoiW;
                eY = sY + RoiH;
                if (sX < 0 || sY < 0 || eX >= DiffAbsImage.Width || eY >= DiffAbsImage.Height)
                    continue;
                Rectangle rc = new Rectangle(sX, sY, RoiW, RoiH);
                var cropImage = DiffAbsImage.GetSubImage(rc);
                cropImage.Copy(ReferenceImage);
                //cropImage.Save("c:\\Test\\crop" + string.Format(ccc.ToString() + ".bmp"));
            }
        }

        private void MakeReferenceImage(BlobRectList blobRectList)
        {
            DiffAbsImage = ImageBuilder.BuildSameTypeSize(OriginImage);  //Whole Area size
            DiffAbsImage.Clear(255);

            int Threshold = 10;
            //ReferenceImage = null; //average : Electrode
            ReferenceEdgeImage = null; //edge , its wait
            ReferenceBinaryImage = null; //Binary

            int sX, sY, eX, eY;
            int ccc = 0;

            int RoiW = ReferenceImage.Width;
            int RoiH = ReferenceImage.Height;

            foreach (var blob in blobRectList)
            {
                ccc++;
                sX = (int)(blob.CenterPt.X - (float)RoiW / 2 + 0.5f);
                sY = (int)(blob.CenterPt.Y - (float)RoiH / 2 + 0.5f);
                eX = sX + RoiW;
                eY = sY + RoiH;
                if (sX < 0 || sY < 0 || eX >= DiffAbsImage.Width || eY >= DiffAbsImage.Height)
                    continue;
                Rectangle rc = new Rectangle(sX, sY, RoiW, RoiH);
                var cropImage = DiffAbsImage.GetSubImage(rc);


                cropImage.Copy(ReferenceImage);
                //cropImage.Save("c:\\Test\\crop" + string.Format(ccc.ToString() + ".bmp"));
            }
        }

        private void button_MakeReferenceImage_Click(object sender, EventArgs e)
        {
            MakeReferenceImage(foundedBlobList);
            canvasPanel.Clear();
            FigureGroup figureGroup = new FigureGroup();
            DrawGetMostPrintImage(foundedBlobList.GetList(),figureGroup, DiffAbsImage.ToBitmap());

            canvasPanel.Clear();
            canvasPanel.WorkingFigures.AddFigure(figureGroup);
            canvasPanel.UpdateImage(DiffAbsImage.ToBitmap());
        }

        private void button_PatMat_Click(object sender, EventArgs e)
        {
            if(ReferenceEdgeImage == null)            
                MessageBox.Show(this, "템플릿 영상없다.", "err");

            AlgoImage sobelimage = ImageBuilder.BuildSameTypeSize(OriginImage);
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage) as OpenCvImageProcessing;
            ipc.Sobel(OriginImage, sobelimage);
            AlgoImage matchImage=ipc.MatchTemplate(sobelimage, ReferenceEdgeImage);
            matchImage.Save("C:\\Test\\matchImage.bmp");
            //pictureBox1.Image = lastImage.ToBitmap();
            canvasPanel.UpdateImage(matchImage.ToBitmap());
        }

        private void SubtractionImage(BlobRectList blobRectList)
        {
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage) as OpenCvImageProcessing;

            DiffAbsImage = ImageBuilder.BuildSameTypeSize(OriginImage);  //Whole Area size
            DiffAbsImage.Clear(0);

            int Threshold = 10;
            //ReferenceImage = null; //average : Electrode
            ReferenceEdgeImage = null; //edge , its wait
            ReferenceBinaryImage = null; //Binary

            int sX, sY, eX, eY;
            int ccc = 0;

            int RoiW = ReferenceImage.Width;
            int RoiH = ReferenceImage.Height;

            foreach (var blob in blobRectList)
            {
                ccc++;
                sX = (int)(blob.CenterPt.X - (float)RoiW / 2 + 0.5f);
                sY = (int)(blob.CenterPt.Y - (float)RoiH / 2 + 0.5f);
                eX = sX + RoiW;
                eY = sY + RoiH;
                if (sX < 0 || sY < 0 || eX >= DiffAbsImage.Width || eY >= DiffAbsImage.Height)
                    continue;
                Rectangle rc = new Rectangle(sX, sY, RoiW, RoiH);
                var srcImage = OriginImage.GetSubImage(rc);
                var cropImage = DiffAbsImage.GetSubImage(rc);
                ipc.Subtract(srcImage, ReferenceImage, cropImage, true);

                //cropImage.Copy(ReferenceImage);
                //cropImage.Save("c:\\Test\\crop" + string.Format(ccc.ToString() + ".bmp"));
            }
            lastImage = DiffAbsImage;
            DiffAbsImage.Save("c:\\Test\\DiffAbsImage.bmp");
        }
        private void PatmatSubtractionImage(BlobRectList blobRectList)
        {
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage) as OpenCvImageProcessing;

            DiffAbsImage = ImageBuilder.BuildSameTypeSize(OriginImage);  //Whole Area size
            DiffAbsImage.Clear(0);
        
            int Threshold = 10;
            //ReferenceImage = null; //average : Electrode
            //ReferenceEdgeImage ; //edge , its wait
            //ReferenceBinaryImage = null; //Binary

            int sX, sY, eX, eY;
            int ccc = 0;
            int dX = 22; //search range x
            int dY = 22; //search range y
            var ReferenceEdgeImageBiynay= ReferenceEdgeImage.Clone();
            ipc.Binarize(ReferenceEdgeImageBiynay, true);


            int RoiW = ReferenceEdgeImage.Width+dX;
            int RoiH = ReferenceEdgeImage.Height+dY;

            foreach (var blob in blobRectList)
            {
                ccc++;
                sX = (int)(blob.CenterPt.X - (float)RoiW / 2 + 0.5f);
                sY = (int)(blob.CenterPt.Y - (float)RoiH / 2 + 0.5f);
                eX = sX + RoiW;
                eY = sY + RoiH;
                if (sX < 0 || sY < 0 || eX >= DiffAbsImage.Width || eY >= DiffAbsImage.Height)
                    continue;
                Rectangle rc = new Rectangle(sX, sY, RoiW, RoiH);
                var srcImage = OriginImage.GetSubImage(rc);
                var sobelimage = srcImage.Clone();
                ipc.Sobel(srcImage, sobelimage);
                var cropImage = DiffAbsImage.GetSubImage(rc);

                /////////////////////////////////////////////////////////////////////////////////////////////////////
                Point pt = ipc.MatchTemplatePos(sobelimage, ReferenceEdgeImage);
                //srcImage.Save("c:\\test\\0001\\src_sm.bmp");
                //sobelimage.Save("c:\\test\\0001\\sobel_sm.bmp");
                //ReferenceEdgeImage.Save("c:\\test\\0001\\refedge_sm.bmp");
                //matchImage.Save("c:\\test\\0001\\match_sm.bmp");

                sX += pt.X;
                sY += pt.Y;
                eX = sX + ReferenceEdgeImage.Width;
                eY = sY + ReferenceEdgeImage.Height;

                Rectangle rc2 = new Rectangle(sX, sY, ReferenceEdgeImage.Width, ReferenceEdgeImage.Height);
                srcImage = OriginImage.GetSubImage(rc2);
                cropImage = DiffAbsImage.GetSubImage(rc2);
                ///////////////////////////////////////////////////////////////////////////////////////////////////////
                ipc.Subtract(srcImage, ReferenceImage, cropImage, true);
                ipc.And(cropImage, ReferenceEdgeImageBiynay, cropImage);

                //cropImage.Copy(ReferenceImage);
                //cropImage.Save("c:\\Test\\crop" + string.Format(ccc.ToString() + ".bmp"));
            }
            
            DiffAbsImage.Save("c:\\Test\\PATMATDiffAbsImage.bmp");
        }
        private void button_Subtraction_Click(object sender, EventArgs e)
        {
            //SubtractionImage(foundedBlobList);

            PatmatSubtractionImage(foundedBlobList);
            lastImage = DiffAbsImage.Clone();

            canvasPanel.Clear();
            //FigureGroup figureGroup = new FigureGroup();
            //DrawGetMostPrintImage(foundedBlobList.GetList(), figureGroup, DiffAbsImage.ToBitmap());

            canvasPanel.Clear();
            //canvasPanel.WorkingFigures.AddFigure(figureGroup);
            canvasPanel.UpdateImage(lastImage.ToBitmap());
        }

        private void button_Threshold_Click(object sender, EventArgs e)
        {
            ImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage);
            AlgoImage algimage = lastImage.Clone();
            int threshold = Convert.ToInt32(tbThreshold.Text);
            ipc.Binarize(algimage, lastImage, threshold);

            canvasPanel.UpdateImage(lastImage.ToBitmap());
        }

        private void button_Remove_edge_Click(object sender, EventArgs e)
        {

        }
        private void txtInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void button_ImgThreshold_Click(object sender, EventArgs e)
        {
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(lastImage) as OpenCvImageProcessing;
            AlgoImage algimage = lastImage.Clone();

            //ipc.Add(algimage, 100);
            //algimage.Save("c:\\Test\\Added.bmp");
            //canvasPanel.UpdateImage(algimage.ToBitmap());

            AlgoImage thresholdimage = ipc.Binarize(lastImage, algimage, 10, 100);
            thresholdimage.Save("c:\\Test\\ThresholdImage.bmp");
            canvasPanel.UpdateImage(thresholdimage.ToBitmap());
        }

        private void button_TDI_Click(object sender, EventArgs e)
        {

            loaded_image = Image.FromFile(@"d:\\TDI.bmp");


            Image2D imageD = Image2D.FromBitmap((Bitmap)loaded_image);
            lastImage = ImageBuilder.Build(ImagingLibrary.OpenCv, imageD, ImageType.Grey);
            OriginImage = lastImage.Clone();

            var image = lastImage.GetByte();
            TDI tdi = new TDI();

            for(int i=2; i< 5; i++)
            {
                var tdiImage = tdi.ConvertTDIimage(image, lastImage.Width, lastImage.Height,i);

                //byte[] imageTest = new byte[256 * 256];
                //for(int i=0; i<256; i++)
                //{
                //    for(int j=0; j<256; j++)
                //    {
                //        imageTest[i * 256 + j] = (byte)(i + j);
                //    }
                //}        //var bitmap = CopyDataToBitmap(imageTest, 256, 256);
                Image2D imageDtest = new Image2D();
                imageDtest.Initialize(lastImage.Width, lastImage.Height, 1);
                imageDtest.SetData(tdiImage);

                var bitmap = imageDtest.ToBitmap();
                string path = string.Format("d:\\TDI128stage({0}).bmp", i);
                bitmap.Save(path);
            }


            //  var bitmap=CopyDataToBitmap(tdiImage, lastImage.Width, lastImage.Height);


            pictureBox1.Image = OriginImage.ToImageD().ToBitmap();
           // canvasPanel.UpdateImage(bitmap);
        }

        public Bitmap CopyDataToBitmap(byte[] data, int width, int height)
        {
            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(
                                 new Rectangle(0, 0, bmp.Width, bmp.Height),
                                 ImageLockMode.WriteOnly, bmp.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);


            //Unlock the pixels
            bmp.UnlockBits(bmpData);


            //Return the bitmap 
            return bmp;
        }
    }



    public class TDI
    {
        int _width; //(pitch)
        int _stage = 128;
        int _countSample;

        List<int[]> _queue =null ;

        public TDI()
        {


        }

        void Initialize(int width, int stage, int countSample)
        {
            _queue = new List<int[]>();

            _width = width; //(pitch)
            _stage = stage;
            _countSample = countSample;

            for (int i = 0; i < _stage; i++)
            {
                _queue.Add(new int[_width]);
            }
        }

        byte[] AddImage(byte[] scanImage) //한칸 이송된 이미지 width * stage * stage Line당 샘플링수
        {
            var outarray = _queue[0];
            byte[] retValue = new byte[outarray.Length];

            _queue.RemoveAt(0);
            _queue.Add(new int[_width]);

            for (int i = 0; i < _stage; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    for (int k = 0; k < _countSample; k++)
                    {
                        _queue[i][j] += scanImage[(i*_countSample + k) * _width + j];
                    }
                }
            }
            for (int j = 0; j < _width; j++)
            {
                retValue[j] = (byte)(outarray[j] / (_stage * _countSample));
            }

            return retValue;
        }


        public byte[] ConvertTDIimage(byte[] srcimage, int width, int height, int moveStepPix)
        {
            byte[] dstImage = new byte[width * height];
            this._countSample = 3;

            Initialize(width, this._stage, _countSample);

            int endY = height   -   ( _stage   *   _countSample   );

            byte[] partial = new byte[width * _stage * _countSample];
            int partialSize = width * _stage * _countSample;

            for (int i = 0; i < endY-1; i += moveStepPix)
            {
                int startIndex = i * width;
                //srcimage.CopyTo(partial, startIndex);

                Array.Copy(srcimage, startIndex,
                    partial, 0,
                    partialSize);

                var oneline = AddImage(partial);

                Array.Copy(oneline, 0,  dstImage, (i/ moveStepPix) * width,     oneline.Length);

            }
            return dstImage;
        }

    }
}
