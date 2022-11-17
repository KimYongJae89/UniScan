using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using UniScanWPF.Helper;
using WpfControlLibrary.UI;

namespace TastApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Result : TestTargetResult
        {
            BitmapSource source;
            float angle;

            public BitmapSource Source { get => source; set => source = value; }
            
            public float Angle { get => angle; set => angle = value; }

            public override List<UIElement> GetFigureList()
            {
                return null;
            }

            protected override BitmapSource GetBitmapImage()
            {
                return source;
            }
        }

        TestCanvasPanel testPanel;

        public MainWindow()
        {
            InitializeComponent();

            MatroxHelper.InitApplication();

            testPanel = new TestCanvasPanel(false);
            canvas.Navigate(testPanel);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Result result = new Result();
            result.Source = WPFImageHelper.LoadBitmapSource(@"D:\Data\100.bmp");

            testPanel.TargetResult = result;
        }

        private void Inspect_Click(object sender, RoutedEventArgs e)
        {
            Image2D image2D = new Image2D(@"D:\Data\100.bmp");

            AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, image2D, ImageType.Grey);
            AlgoImage processImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(algoImage.Width, algoImage.Height));

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            //WPFImageHelper.SaveBitmapSource(@"D:\Data\1.bmp", algoImage2.ToBitmapSource());
            BlobParam blobParam = new BlobParam();
            blobParam.SelectRotateRect = true;
            float avg =  imageProcessing.GetGreyAverage(algoImage);

            imageProcessing.Binarize(algoImage, processImage, (int)Math.Round(avg), true);

            imageProcessing.FillHoles(processImage, processImage);

            BlobRectList blobRectList =  imageProcessing.Blob(processImage, blobParam);

            List<BlobRect> list = blobRectList.GetList();

            foreach (BlobRect blobRect in list)
            {
                StripeCheckerResult stripeCheckerResult = StripeChecker.Check(processImage, blobRect.RotateCenterPt, blobRect.RotateWidth + 10, 1, blobRect.RotateAngle - 90);
                blobRect.MeasureData = new object[] { stripeCheckerResult.MeanWidth, stripeCheckerResult.Point1, stripeCheckerResult.Point2 };
            }
            
            BitmapSource source = algoImage.ToBitmapSource();
            Result result = new Result();
            result.Source = source;
            result.BlobRectList = list;

            float left = list.Min(blobRect => blobRect.BoundingRect.Left);
            float top = list.Min(blobRect => blobRect.BoundingRect.Top);
            float right = list.Max(blobRect => blobRect.BoundingRect.Right);
            float bottom = list.Max(blobRect => blobRect.BoundingRect.Bottom);

            float scanLength =  bottom - top;
            float asdasd = left + scanLength;
            
            list = list.OrderByDescending(blobRect => blobRect.RotateAngle).ToList();

            int count = list.Count / 3;

            list = list.Skip(count).Take(count).ToList();

            float avgAngle = list.Average(blobRect => blobRect.RotateAngle);
            
            blobRectList.Dispose();
            processImage.Dispose();
            algoImage.Dispose();

            testPanel.TargetResult = result;

            //ImageProcessing imageProcessing = ImageBuilder.Build()
        }
    }
}
