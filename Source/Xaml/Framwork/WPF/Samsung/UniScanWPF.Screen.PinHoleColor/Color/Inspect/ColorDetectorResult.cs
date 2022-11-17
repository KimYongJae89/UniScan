using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using UniScanWPF.Helper;
using UniScanWPF.Screen.PinHoleColor.Color.Data;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.Color.Inspect
{
    public enum ColorDefectType
    {
        Blot, NoPrint
    }
    
    public class ColorDetectorResult : DetectorResult
    {
        BitmapSource sheetImage;
        float average;
        float widthRatio;
        float heightRatio;

        public BitmapSource SheetImage { get => sheetImage; set => sheetImage = value; }
        public float Average { get => average; set => average = value; }
        public float WidthRatio { get => widthRatio; set => widthRatio = value; }
        public float HeightRatio { get => heightRatio; set => heightRatio = value; }

        public override void ExportResult(string resultPath, XmlElement detectorElement)
        {
            XmlHelper.SetValue(detectorElement, "Average", average.ToString());
            XmlHelper.SetValue(detectorElement, "WidthRatio", widthRatio.ToString());
            XmlHelper.SetValue(detectorElement, "HeightRatio", heightRatio.ToString());

            if (sheetImage != null)
            {
                string sheetPath = Path.Combine(resultPath, "SheetImage.jpg");
                WPFImageHelper.SaveBitmapSource(sheetPath, sheetImage);
            }

            base.ExportResult(resultPath, detectorElement);
        }

        public override void ImportResult(string resultPath, XmlElement detectorElement)
        {
            average = Convert.ToSingle(XmlHelper.GetValue(detectorElement, "Average", "0"));
            widthRatio = Convert.ToSingle(XmlHelper.GetValue(detectorElement, "WidthRatio", "0"));
            heightRatio = Convert.ToSingle(XmlHelper.GetValue(detectorElement, "HeightRatio", "0"));

            string sheetPath = Path.Combine(resultPath, "SheetImage.jpg");
            if (File.Exists(sheetPath) == true)
                sheetImage = WPFImageHelper.LoadBitmapSource(sheetPath);

            base.ImportResult(resultPath, detectorElement);
        }

        public override List<UIElement> GetFigures()
        {
            List<UIElement> figureList = new List<UIElement>();

            if (sheetImage != null)
            {
                foreach (ColorDefect defect in defectList)
                {
                    System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle();
                    rectangle.StrokeThickness = 10;
                    
                    TextBlock textBlock = new TextBlock();
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.FontSize = defect.Rectangle.Width * 0.2f;
                    textBlock.Text = string.Format("{0:0.0}", defect.DiffValue);

                    switch (defect.Type)
                    {
                        case ColorDefectType.Blot:
                            rectangle.Stroke = new SolidColorBrush(Colors.Red);
                            textBlock.Foreground = new SolidColorBrush(Colors.Crimson);
                            break;
                        case ColorDefectType.NoPrint:
                            rectangle.Stroke = new SolidColorBrush(Colors.Yellow);
                            textBlock.Foreground = new SolidColorBrush(Colors.Yellow);
                            break;
                    }

                    Canvas.SetLeft(rectangle, defect.Rectangle.X);
                    Canvas.SetTop(rectangle, defect.Rectangle.Y);
                    Canvas.SetLeft(textBlock, defect.Rectangle.X);
                    Canvas.SetTop(textBlock, defect.Rectangle.Y + ((defect.Rectangle.Height - textBlock.FontSize) / 2));

                    rectangle.Width = defect.Rectangle.Width;
                    rectangle.Height = defect.Rectangle.Height;
                    textBlock.Width = defect.Rectangle.Width;

                    figureList.Add(rectangle);
                    if (textBlock.Foreground != null)
                        figureList.Add(textBlock);
                }

                TextBlock avgTextBlock = new TextBlock();
                avgTextBlock.FontSize = 100;
                avgTextBlock.Text = string.Format("{0:0.0}", average);
                avgTextBlock.Foreground = new SolidColorBrush(Colors.Green);

                Canvas.SetLeft(avgTextBlock, sheetImage.Width / 2 - 100);
                Canvas.SetTop(avgTextBlock, sheetImage.Height / 2 - 50);

                TextBlock widthRatioTextBlock = new TextBlock();
                widthRatioTextBlock.FontSize = 75;
                widthRatioTextBlock.Text = string.Format("{0:0.0}", widthRatio * 100.0f);
                widthRatioTextBlock.Foreground = new SolidColorBrush(Colors.Gold);

                Canvas.SetLeft(widthRatioTextBlock, sheetImage.Width / 2 - 100);
                Canvas.SetTop(widthRatioTextBlock, sheetImage.Height - 150);

                TextBlock heightRatioTextBlock = new TextBlock();
                heightRatioTextBlock.FontSize = 75;
                heightRatioTextBlock.Text = string.Format("{0:0.0}", heightRatio * 100.0f);
                heightRatioTextBlock.Foreground = new SolidColorBrush(Colors.Gold);

                Canvas.SetLeft(heightRatioTextBlock, sheetImage.Width - 200);
                Canvas.SetTop(heightRatioTextBlock, sheetImage.Height / 2 - 50);

                figureList.Add(avgTextBlock);
                figureList.Add(widthRatioTextBlock);
                figureList.Add(heightRatioTextBlock);
            }

            return figureList;
        }

        public override DetectorType GetDetectorType()
        {
            return DetectorType.Color;
        }
    }
}
