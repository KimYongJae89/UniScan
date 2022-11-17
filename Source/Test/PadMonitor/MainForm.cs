using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Base;
using System.Windows;
using DynMvp.Data.UI;
using DynMvp.Vision.OpenCv;
using System.Diagnostics;
using DynMvp.UI.Touch;
using System.Runtime.InteropServices;
using System.Threading;

namespace PadMonitor
{
    public partial class MainForm : Form
    {
        OverViewer m_overViewer = null;
        CanvasPanel[] canvasPanels = new CanvasPanel[3];
        CanvasPanel canvasPanelFull = null;

        AlgoImage SourceImage = null; //Whole Area size
        AlgoImage ReferenceImage = null;  //Whole Area size
        AlgoImage InspectionImage = null;  //Whole Area size

        AlgoImage ScaledSourceImage = null;
        AlgoImage ScaledReferenceImage = null;
        AlgoImage ScaledInspectionImage = null;



        public MainForm()
        { 
            InitializeComponent();
            CreateCanvasPanel();
            // this.KeyPreview = true;
        }

        private void CreateCanvasPanel()
        {
            for (int i = 0; i < canvasPanels.Length; i++)
                canvasPanels[i] = new CanvasPanel();

            foreach (var canvas in canvasPanels)
            {
                //canvas.NoneClickMode = true;
                canvas.SetPanMode();// = DragMode.Pan;
                canvas.UseZoom = false;
                canvas.ShowCenterGuide = false;
                canvas.Dock = DockStyle.None;
                //canvas.SizeChanged += View_SizeChanged;
                //canvas.NoneClickMode = false;
                canvas.BackColor = Color.CornflowerBlue;
                //canvas.MouseWheel += new MouseEventHandler(canvasPanel_MouseWheel);
                canvas.MouseClicked = CanvasPanel_MouseClicked;
                //canvas.FigureClicked = CanvasPanel_FigureFocused;
                canvas.FigureMouseEnter = CanvasPanel_FigureFocused;

                this.Controls.Add(canvas);

            }


            canvasPanelFull = new CanvasPanel();
            //canvasPanelFull.DragMode = DragMode.Pan;
            //canvasPanelFull.NoneClickMode = true;
            canvasPanelFull.ShowCenterGuide = false;
            canvasPanelFull.UseZoom = false;
            canvasPanelFull.Dock = DockStyle.None;
            //canvasPanelFull.SizeChanged += View_SizeChanged;
            //canvasPanelFull.NoneClickMode = false;
            canvasPanelFull.BackColor = Color.Aqua;
            //canvas.MouseWheel += new MouseEventHandler(canvasPanel_MouseWheel);
            canvasPanelFull.MouseClicked = CanvasPanel_MouseClicked;
            //canvasPanelFull.FigureClicked = CanvasPanel_FigureFocused;
            canvasPanelFull.FigureMouseEnter = CanvasPanel_FigureFocused;
            this.Controls.Add(canvasPanelFull);

 

        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            int headheight = panelHeader.Size.Height;
            int space = 2;
            Size imgsize = new Size(68000,3000);
            //Size monsize = new Size(3840, 2160);
            //Size monsize = new Size(1920, 1080);
            Size monsize = this.Size;

            double divRate = Math.Sqrt( (double)imgsize.Width * (double)monsize.Height / ((double)imgsize.Height * (double)monsize.Width) );
            double divCnt = Math.Floor(divRate);

            double newRate = imgsize.Width / divCnt / imgsize.Height;
            int newMonHeight = (int)Math.Floor(monsize.Width / newRate);

            Size ViewSize = this.Size;
            Debug.WriteLine("■ ViewSize = " + ViewSize.ToString());


            //int startY = (int)(ViewSize.Height - (newMonHeight* divCnt +space *(divCnt-1)));
            int startY = headheight + space;

            int fullHeight = (int)(monsize.Width * (double)imgsize.Height / (double)imgsize.Width);
            canvasPanelFull.Location = new Point(0, startY);
            canvasPanelFull.Size = new Size(ViewSize.Width, fullHeight);

            startY = startY + fullHeight + space;


            //this.StartPosition = FormStartPosition.Manual;
            //Rectangle fullScrenn_bounds = Rectangle.Empty;
            //foreach (var screen in Screen.AllScreens)
            //{
            //    fullScrenn_bounds = Rectangle.Union(fullScrenn_bounds, screen.Bounds);
            //}
            //this.ClientSize = new Size(fullScrenn_bounds.Width, fullScrenn_bounds.Height);
            //this.Location = new Point(fullScrenn_bounds.Left, fullScrenn_bounds.Top);

            for (int i = 0; i < canvasPanels.Length; i++)
            {
                if (canvasPanels[i] != null)
                {
                    canvasPanels[i].Location= new Point(0, startY + (newMonHeight + 2) * i);
                    canvasPanels[i].Size = new Size(ViewSize.Width, newMonHeight);
                }
            }

        }

        //키보드 메시지가 폼의 컨트롤에 도달하기 전에 폼이 이 메시지를 수신하도록 폼의 KeyPreview 속성을 true로 설정
        private void MainForm_KeyDown(object sender, KeyEventArgs e) //키를 누를 때  vs Keyup : 키를 뗄 때.
        {
            if (e.Control || e.Alt || e.Shift) return;
            switch (e.KeyCode)
            {
                case Keys.F2: //load & show Reference
                    if (openImageFile(ref ReferenceImage))
                    {
                        RescaleFullImage(ReferenceImage, ref ScaledReferenceImage);//축소된 이미지 만들기
                        DrawFullView(ScaledReferenceImage);
                        DrawZoomViews(ReferenceImage);
                        //MessageBox.Show("F2 pressed !");
                    }
                    break;

                case Keys.F3: //load & show Source(Scan)
                    if (openImageFile(ref SourceImage))
                    {
                        RescaleFullImage(SourceImage, ref ScaledSourceImage); //축소된 이미지 만들기
                        DrawFullView(ScaledSourceImage);
                        DrawZoomViews(SourceImage);
                    }
                    break;

                case Keys.F4: //Calc Inspection  & Show (InspectionImage)
                    Inspection2(ReferenceImage, SourceImage);
                    RescaleFullImage(InspectionImage, ref ScaledInspectionImage);//축소된 이미지 만들기
                    DrawFullView(ScaledInspectionImage);
                    DrawZoomViews(InspectionImage);
                    break;

                case Keys.F5:
                    DrawFullView(ScaledReferenceImage);
                    DrawZoomViews(ReferenceImage);
                    break;

                case Keys.F6:
                    DrawFullView(ScaledSourceImage);
                    DrawZoomViews(SourceImage);
                    break;

                case Keys.F7:
                    if(ScaledInspectionImage ==null)
                        RescaleFullImage(InspectionImage, ref ScaledInspectionImage);//축소된 이미지 만들기
                    DrawFullView(ScaledInspectionImage);
                    DrawZoomViews(InspectionImage);
                    break;

            }
            return;
        }

        // This event occurs after the KeyDown event and can be used to prevent
        // characters from entering the control.
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e) //키를 계속 누르고 있을때..
        {
            // Stop the character from being entered into the control since it is non-numerical.
            e.Handled = true;
        }

        private void DrawFullView(AlgoImage scaledImage)
        {
            if (scaledImage == null)
                MessageBox.Show("image is null");
            Bitmap bitmap = scaledImage.ToBitmap();

            System.Drawing.Imaging.ColorPalette cp = bitmap.Palette;
            for (int i = 0; i < 256; i++)
            {
                if (i == 0)
                    cp.Entries[i] = Color.FromArgb(50, 255, 50);
                else if (i == 255)
                    cp.Entries[i] = Color.FromArgb(255, 0, 0);
                else
                    cp.Entries[i] = Color.FromArgb(i, i, i);
            }
            bitmap.Palette = cp;

            canvasPanelFull.UpdateImage(bitmap);
        }

        private void DrawZoomViews(AlgoImage wholeImage)//, CanvasPanel[] Panels)
        {
            if (wholeImage == null || canvasPanels.Length == 0) return;

            Size size = wholeImage.Size;

            int split = 3;
            int cropWidth = size.Width / split;

            AlgoImage subImage = null;
            for (int i = 0; i < split; i++)
            {
                Rectangle rc = new Rectangle(i * cropWidth, 0, cropWidth, size.Height);
                subImage = wholeImage.GetSubImage(rc);
                canvasPanels[i].UpdateImage(subImage.ToBitmap());
            }
        }

        private void RescaleFullImage(AlgoImage fullimage,ref AlgoImage scaledImage)
        {
            if (fullimage == null)
            {
                MessageBox.Show("RescaleFullImage() arg == null ");
                return;
            }
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(fullimage) as OpenCvImageProcessing;
            //Build(ImagingLibrary libraryType, ImageType imageType, int width, int height, ImageBandType imageBand = ImageBandType.Luminance)
            Size size = canvasPanelFull.Size;
            AlgoImage destImage = ImageBuilder.Build(fullimage.LibraryType, fullimage.ImageType, size.Width, size.Height);
            ipc.Resize(fullimage, destImage);
            scaledImage = destImage;
        }

        private bool openImageFile(ref AlgoImage destImage)
        {
            OpenFileDialog ofdFile = new OpenFileDialog();
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                string ResultPath = ofdFile.FileName;
                //this.Visible = false; //폼 감추기 
                if (destImage != null)
                {
                    destImage.Dispose();
                    destImage = null;
                }

                var loaded_image = Image.FromFile(ResultPath); //얘는 메모리 문제가 없네.
                Image2D imageD = Image2D.FromBitmap((Bitmap)loaded_image);
                destImage = ImageBuilder.Build(ImagingLibrary.OpenCv, imageD, ImageType.Grey);

                //for (int i = 0; i < canvasPanels.Length; i++)
                //    if (canvasPanels[i] != null)
                //        canvasPanels[i].UpdateImage(destImage.ToBitmap());               
                return true;
            }
            return false;
        }

        public delegate void delegateInspection(AlgoImage refImage, AlgoImage scanImage);
        private void Inspection(AlgoImage refImage, AlgoImage scanImage)
        {
            if (InvokeRequired)
            {
                Invoke(new delegateInspection(Inspection), refImage, scanImage);
                return;
            }

            if (refImage == null || scanImage == null)
            {
                MessageBox.Show("Image is null");
                return;
            }
            if(refImage.Size != scanImage.Size)
            {
                MessageBox.Show("Image Size is different");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;

            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(refImage) as OpenCvImageProcessing;
            Size size = canvasPanelFull.Size;
            if(InspectionImage== null)
                InspectionImage = ImageBuilder.BuildSameTypeSize(refImage);

            //버퍼 생성
            AlgoImage binMask = ImageBuilder.BuildSameTypeSize(refImage);
            AlgoImage edgeMask = ImageBuilder.BuildSameTypeSize(refImage);

            //엣지제거용 엣지마스크 생성
            ipc.Sobel(refImage, edgeMask);
            ipc.Binarize(edgeMask, binMask, 66,true); //inverse threshold binarize...
            ipc.Erode(binMask, 3);
            binMask.Save(System.Environment.CurrentDirectory + "BinMask.bmp");

            //차이 계산시 일부 영역별로 최적위치를 찾고
            //그 차이값을 적산한다.
            //일단 간단히 차이값 구함
            ipc.Subtract(refImage, scanImage, InspectionImage, true);

            //엣지 제거
            ipc.And(InspectionImage, binMask, InspectionImage);
            InspectionImage.Save(System.Environment.CurrentDirectory + "Inspection.bmp");

            //이진화
            ipc.Binarize(InspectionImage, 50);

            if (ScaledInspectionImage != null) ScaledInspectionImage.Dispose();
            RescaleFullImage(InspectionImage, ref ScaledInspectionImage);//축소된 이미지 만들기

            BlobParam blobParam = new BlobParam();
            {
                blobParam.SelectArea = true;
                blobParam.SelectCenterPt = true;
                blobParam.SelectBoundingRect = true;
            }

            BlobRectList blobList = ipc.Blob(InspectionImage, blobParam);

            DrawDefectScaled(blobList, InspectionImage, canvasPanelFull);


            int split = 3;
            int cropWidth = InspectionImage.Size.Width / split;

            AlgoImage []subImages = new AlgoImage[3];
            AlgoImage[] SrcSubImages = new AlgoImage[3];
            BlobRectList[] blobLists = new BlobRectList[3];
            for (int i = 0; i < split; i++)
            {
                Rectangle rc = new Rectangle(i * cropWidth, 0, cropWidth, InspectionImage.Size.Height);
                subImages[i] = InspectionImage.GetSubImage(rc);
                SrcSubImages[i] =  scanImage.GetSubImage(rc);

                blobLists[i] = ipc.Blob(subImages[i], blobParam);
                DrawDefect(blobLists[i], SrcSubImages[i], canvasPanels[i]);
            }

            listup(blobList, dataGridView_LEFT);
            listup(blobList, dataGridView_CENTER);
            listup(blobList, dataGridView_RIGHT);
            //버퍼 제거
            binMask.Dispose();
            edgeMask.Dispose();
        }
        private void Inspection2(AlgoImage refImage, AlgoImage scanImage)
        {
            if (InvokeRequired)
            {
                Invoke(new delegateInspection(Inspection), refImage, scanImage);
                return;
            }

            if (refImage == null || scanImage == null)
            {
                MessageBox.Show("Image is null");
                return;
            }
            if (refImage.Size != scanImage.Size)
            {
                MessageBox.Show("Image Size is different");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;

            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(refImage) as OpenCvImageProcessing;
            Size size = canvasPanelFull.Size;
            if (InspectionImage == null)
                InspectionImage = ImageBuilder.BuildSameTypeSize(refImage);

            //버퍼 생성
            AlgoImage binMask = ImageBuilder.BuildSameTypeSize(refImage);
            AlgoImage edgeMask = ImageBuilder.BuildSameTypeSize(refImage);

            //엣지제거용 엣지마스크 생성
            ipc.Sobel(refImage, edgeMask);
            ipc.Binarize(edgeMask, binMask, 66, true); //inverse threshold binarize...
            ipc.Erode(binMask, 3);
            binMask.Save(System.Environment.CurrentDirectory + "BinMask.bmp");

            //차이 계산시 일부 영역별로 최적위치를 찾고
            //그 차이값을 적산한다.
            //일단 간단히 차이값 구함
            //ipc.Subtract(refImage, scanImage, InspectionImage, true);
            //InspectSubtraction(refImage, scanImage, InspectionImage, 50, 30);
            InspectSubtraction222(refImage, scanImage, InspectionImage, 50, 30);

            //엣지 제거
            ipc.And(InspectionImage, binMask, InspectionImage);
            InspectionImage.Save(System.Environment.CurrentDirectory + "Inspection.bmp");

            //이진화
            ipc.Binarize(InspectionImage, 64);

            if (ScaledInspectionImage != null) ScaledInspectionImage.Dispose();
            RescaleFullImage(InspectionImage, ref ScaledInspectionImage);//축소된 이미지 만들기

            BlobParam blobParam = new BlobParam();
            {
                blobParam.SelectArea = true;
                blobParam.SelectCenterPt = true;
                blobParam.SelectBoundingRect = true;
            }

            BlobRectList blobList = ipc.Blob(InspectionImage, blobParam);

            DrawDefectScaled(blobList, InspectionImage, canvasPanelFull);


            int split = 3;
            int cropWidth = InspectionImage.Size.Width / split;

            AlgoImage[] subImages = new AlgoImage[3];
            AlgoImage[] SrcSubImages = new AlgoImage[3];
            BlobRectList[] blobLists = new BlobRectList[3];
            for (int i = 0; i < split; i++)
            {
                Rectangle rc = new Rectangle(i * cropWidth, 0, cropWidth, InspectionImage.Size.Height);
                subImages[i] = InspectionImage.GetSubImage(rc);
                SrcSubImages[i] = scanImage.GetSubImage(rc);

                blobLists[i] = ipc.Blob(subImages[i], blobParam);
                DrawDefect(blobLists[i], SrcSubImages[i], canvasPanels[i]);
            }

            listup(blobList, dataGridView_LEFT);
            listup(blobList, dataGridView_CENTER);
            listup(blobList, dataGridView_RIGHT);
            //버퍼 제거
            binMask.Dispose();
            edgeMask.Dispose();
        }

        private void InspectSubtraction(AlgoImage refImage, AlgoImage inspImage, AlgoImage dstImage, int inspWidth, int margin)
        {
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(inspImage) as OpenCvImageProcessing;
   
   
            dstImage.Clear(0);

            int sX, sY, eX, eY;
            int ccc = 0;
            int dX = margin; //patmat search range x  //todo param
            int dY = margin; //patmat search range y  //todo param


;

            int imageWidth = refImage.Width;
            int imageHeight = refImage.Height;

            int i = 0; 
            for(i = margin; i< imageWidth;    i+=inspWidth)
            {
                ccc++;
                sX =i;
                sY = 0 + margin;
                eX = sX + inspWidth  ;
                eY = imageHeight - margin ;

                if (sX < 0 || sY < 0 || eX >= inspImage.Width || eY >= inspImage.Height)//영역을 벗어나면 스킵
                    continue;

                Rectangle rc = new Rectangle(sX, sY, eX-sX, eY -sY);
                var refImageROI = refImage.GetSubImage(rc);

                sX = i- margin;
                sY = 0;
                eX = sX + inspWidth + margin;
                eY = imageHeight;
                Rectangle rc2 = new Rectangle(sX, sY, eX - sX, eY - sY);
                var inspImageROI = inspImage.GetSubImage(rc2);


                ///////////////위치보정후////////////////////////////////////////////////////////////////////////////////

                Point pt = ipc.MatchTemplatePos(inspImageROI, refImageROI);

                sX += pt.X;
                sY += pt.Y;
                eX = sX + refImageROI.Width;
                eY = sY + refImageROI.Height-1;
                if (sX < 0 || sY < 0 || eX >= inspImage.Width || eY >= inspImage.Height) //영역을 벗어나면 스킵 // 여긴 논리적으로 걸릴수 없다.
                    continue;
                /////////////밝기 보정/////////////////////////////////////////////////////////////////////////////////
                Rectangle rc3 = new Rectangle(sX, sY, refImageROI.Width, refImageROI.Height);
                inspImageROI = inspImage.GetSubImage(rc3);
                var destImageROI = dstImage.GetSubImage(rc3);
                //float inspAvg = ipc.GetGreyAverage(inspImageROI);
                //float refAvg = ipc.GetGreyAverage(refImageROI);
                //var CalRefImage = ReferenceImage.Clone();
                //ipc.Mul(ReferenceImage, CalRefImage, inspAvg / refAvg);
                ////////////차이값 계산/////////////////////////////////////////////////////////////////////////////////
                ipc.Subtract(inspImageROI, refImageROI, destImageROI, true);
                // ipc.And(destImage, ReferenceEdgeImageBiynay, destImage);
                ///////////동적문턱값 //////////////////////////////////////////////////////////////////////////////////
                //var roi = InspThresholdImage.GetSubImage(rc2);
                //roi.Copy(RefThresholdImage);

            }//for(i = margin; i< imageWidth;    i+=inspWidth/2)

            dstImage.Save(System.Environment.CurrentDirectory + "InspectSubtractionPM.bmp");
        }

        private void InspectSubtraction222(AlgoImage refImage, AlgoImage inspImage, AlgoImage dstImage, int inspWidth, int margin)
        {
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(inspImage) as OpenCvImageProcessing;
            dstImage.Clear(0);

            int sX, sY, eX, eY;
            int ccc = 0;
            int dX = margin; //patmat search range x  //todo param
            int dY = margin; //patmat search range y  //todo param
            int imageWidth = refImage.Width;
            int imageHeight = refImage.Height;

            int i = 0;
            int j = 0;
            int inspHeight = imageHeight / 4;
            for (i = margin; i < imageWidth; i += inspWidth)
            {
                for ( j = margin; j < imageHeight; j += inspHeight/2)
                {
                    ccc++;
                    sX = i;
                    sY = j ;
                    eX = sX + inspWidth;
                    eY = sY + inspHeight;

                    if (sX < 0 || sY < 0 || eX >= inspImage.Width || eY >= inspImage.Height)//영역을 벗어나면 스킵
                        continue;

                    Rectangle rc = new Rectangle(sX, sY, eX - sX, eY - sY);
                    var refImageROI = refImage.GetSubImage(rc);

                    sX = i - margin;
                    sY = j - margin;
                    eX = sX + inspWidth + margin;
                    eY = sY + inspHeight + margin;
                    Rectangle rc2 = new Rectangle(sX, sY, eX - sX, eY - sY);
                    var inspImageROI = inspImage.GetSubImage(rc2);

                    ///////////////위치보정후////////////////////////////////////////////////////////////////////////////////

                    Point pt = ipc.MatchTemplatePos(inspImageROI, refImageROI);

                    sX += pt.X;
                    sY += pt.Y;
                    eX = sX + refImageROI.Width;
                    eY = sY + refImageROI.Height - 1;
                    if (sX < 0 || sY < 0 || eX >= inspImage.Width || eY >= inspImage.Height) //영역을 벗어나면 스킵 // 여긴 논리적으로 걸릴수 없다.
                        continue;
                    /////////////밝기 보정/////////////////////////////////////////////////////////////////////////////////
                    Rectangle rc3 = new Rectangle(sX, sY, refImageROI.Width, refImageROI.Height);
                    inspImageROI = inspImage.GetSubImage(rc3);
                    var destImageROI = dstImage.GetSubImage(rc3);
                    //float inspAvg = ipc.GetGreyAverage(inspImageROI);
                    //float refAvg = ipc.GetGreyAverage(refImageROI);
                    //var CalRefImage = ReferenceImage.Clone();
                    //ipc.Mul(ReferenceImage, CalRefImage, inspAvg / refAvg);
                    ////////////차이값 계산/////////////////////////////////////////////////////////////////////////////////
                    ipc.Subtract(inspImageROI, refImageROI, destImageROI, true);
                    // ipc.And(destImage, ReferenceEdgeImageBiynay, destImage);
                    ///////////동적문턱값 //////////////////////////////////////////////////////////////////////////////////
                    //var roi = InspThresholdImage.GetSubImage(rc2);
                    //roi.Copy(RefThresholdImage);

                }//for(i = margin; i< imageWidth;    i+=inspWidth/2)
            }
            dstImage.Save(System.Environment.CurrentDirectory + "InspectSubtractionPM.bmp");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }



        private void listup(BlobRectList blobList, DataGridView datagridview)
        {
            
            datagridview.Rows.Clear();
            int i = 1;
            foreach(var blob in blobList)
            {
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow.CreateCells(datagridview, i,blob.CenterPt, blob.Area);
                datagridview.Rows.Add(dataGridViewRow);
                //dataGridViewRow.DefaultCellStyle.BackColor = Color.Red;
                //dataGridViewRow.DefaultCellStyle.ForeColor = Color.White;
                i++;
            }
            // dataGridViewRow.Tag = inspectionResult;
        }
        public void CanvasPanel_MouseClicked(CanvasPanel canvasPanel, PointF point, ref bool processingCancelled)
        {
            m_overViewer?.Hide();
        }
        public void CanvasPanel_FigureFocused(Figure figure)
        {
            //figure.Tag as
            //MousePosition.

            if (figure == null) return;
            if(m_overViewer == null)
                m_overViewer = new OverViewer();

            m_overViewer.Location = Point.Subtract(MousePosition, new Size(-5, -10));

            ZoomDataInfo zinfo = figure.Tag as ZoomDataInfo;

            if (zinfo != null)
            {
                int x = (int)zinfo.blobrect.CenterPt.X;
                int y = (int)zinfo.blobrect.CenterPt.Y;
                Size imgsize = zinfo.image.Size;

                int sx = x - 320; sx = sx > 0 ? sx : 0;
                int ex = x + 320; ex = ex < imgsize.Width ? ex : imgsize.Width - 1;
                int sy = y - 240; sy = sy > 0 ? sy : 0;
                int ey = y + 240; ey = ey < imgsize.Height ? ey : imgsize.Height - 1;

                Rectangle rect = new Rectangle(sx, sy, ex - sx, ey - sy);
                m_overViewer.UpdateImage( zinfo.image.GetSubImage(rect).ToBitmap());

                m_overViewer.Show();
            }

        }
        private void DrawDefectScaled(BlobRectList blobList, AlgoImage image,CanvasPanel viewPanel)
        {
            Size size = canvasPanelFull.Size;
            double rate = (double)size.Width / (double)image.Size.Width;
            //FigureGroup figureGroup = new FigureGroup();
            Pen pen = new Pen(Color.Red, 2);
            viewPanel.Clear();
            foreach (var blob in blobList)
            {
                RectangleF rc = blob.BoundingRect;

                int sx = (int)(rc.Left * rate);  
                int sy = (int)(rc.Top * rate);
                int dx = (int)(rc.Width * rate);
                int dy = (int)(rc.Height * rate);
                Rectangle rect = new Rectangle(sx, sy, dx, dy);

                RectangleFigure figRect = new RectangleFigure(rect, pen);
                figRect.Tag = new ZoomDataInfo(SourceImage, blob);
                //figureGroup.AddFigure(figRect);
                viewPanel.WorkingFigures.AddFigure(figRect);
            }

            //////////////////////////////////////
     
            
           // viewPanel.Invalidate();

        }

        private void DrawDefect(BlobRectList blobList, AlgoImage image,CanvasPanel viewPanel)
        {
            //FigureGroup figureGroup = new FigureGroup();
            Pen pen = new Pen(Color.Red, 5);
            viewPanel.Clear();
            foreach (var blob in blobList)
            {
                RectangleFigure rect = new RectangleFigure(blob.BoundingRect, pen);
                rect.Tag = new ZoomDataInfo(image, blob);
                //figureGroup.AddFigure(rect);
                viewPanel.WorkingFigures.AddFigure(rect);
            }
                            
            //////////////////////////////////////

            
            //viewPanel.Invalidate();

        }

        private void button_Scan_Click(object sender, EventArgs e)
        {
            var progressForm = new SimpleProgressForm();
            progressForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            progressForm.TopMost = true;
            progressForm.Text = "Scan";
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            progressForm.Show(() =>
            {
                 ThreadFlow();
                //Thread.Sleep(10000);


            }, cancellationTokenSource);

            Debug.WriteLine("---------------");

            if (cancellationTokenSource.IsCancellationRequested)
                return;
        }
        private void ThreadFlow()
        {

            var loaded_image = Image.FromFile(@"C:\uniscan\디스플레이 패널\unieye_0423\Monitor\Reference_68vs3.bmp"); //얘는 메모리 문제가 없네.
            Image2D imageD = Image2D.FromBitmap((Bitmap)loaded_image);
            ReferenceImage = ImageBuilder.Build(ImagingLibrary.OpenCv, imageD, ImageType.Grey);
            RescaleFullImage(ReferenceImage, ref ScaledReferenceImage);//축소된 이미지 만들기

            loaded_image = Image.FromFile(@"C:\uniscan\디스플레이 패널\unieye_0423\Monitor\defect_68vs3.bmp"); //얘는 메모리 문제가 없네.
            imageD = Image2D.FromBitmap((Bitmap)loaded_image);
            SourceImage = ImageBuilder.Build(ImagingLibrary.OpenCv, imageD, ImageType.Grey);
            RescaleFullImage(SourceImage, ref ScaledSourceImage);//축소된 이미지 만들기

            ///
            //Inspection(ReferenceImage, SourceImage);
            //
            Inspection2(ReferenceImage, SourceImage);

            DrawFullView(ScaledSourceImage);
            DrawZoomViews(SourceImage);
        }
    }
}
