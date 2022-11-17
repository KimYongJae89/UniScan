using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.Inspect;
using UniScanWPF.Screen.PinHoleColor.PinHole.Data;

namespace UniScanWPF.Screen.PinHoleColor.PinHole.Inspect
{
    internal class PinHoleDetectorResult : DetectorResult
    {
        Rectangle interestRegion;
        List<float> edgeValueList = new List<float>();
        float average;

        public Rectangle InterestRegion { get => interestRegion; set => interestRegion = value; }
        public float Average { get => average; set => average = value; }
        public List<float> EdgeValueList { get => edgeValueList; set => edgeValueList = value; }

        public override void ExportResult(string resultPath, XmlElement detectorElement)
        {
            XmlHelper.SetValue(detectorElement, "InterestRegionX", interestRegion.X.ToString());
            XmlHelper.SetValue(detectorElement, "InterestRegionY", interestRegion.Y.ToString());
            XmlHelper.SetValue(detectorElement, "InterestRegionW", interestRegion.Width.ToString());
            XmlHelper.SetValue(detectorElement, "InterestRegionH", interestRegion.Height.ToString());
            XmlHelper.SetValue(detectorElement, "Average", average.ToString());

            for (int i = 0; i < edgeValueList.Count; i++)
                XmlHelper.SetValue(detectorElement, string.Format("Edge{0}", i), edgeValueList[i].ToString());

            base.ExportResult(resultPath, detectorElement);
        }

        public override void ImportResult(string resultPath, XmlElement detectorElement)
        {
            int x = Convert.ToInt32(XmlHelper.GetValue(detectorElement, "InterestRegionX", "0"));
            int y = Convert.ToInt32(XmlHelper.GetValue(detectorElement, "InterestRegionY", "0"));
            int width = Convert.ToInt32(XmlHelper.GetValue(detectorElement, "InterestRegionW", "0"));
            int height = Convert.ToInt32(XmlHelper.GetValue(detectorElement, "InterestRegionH", "0"));

            interestRegion = new Rectangle(x, y, width, height);

            average = Convert.ToSingle(XmlHelper.GetValue(detectorElement, "Average", "0"));

            int edgeCount = 0;
            foreach (XmlNode node in detectorElement.ChildNodes)
            {
                if (node.Name.Contains("Edge") == true)
                    edgeCount++;
            }

            for (int i = 0; i < edgeCount; i++)
                edgeValueList.Add(Convert.ToSingle(XmlHelper.GetValue(detectorElement, string.Format("Edge{0}", i), "0")));

            base.ImportResult(resultPath, detectorElement);
        }

        public override DetectorType GetDetectorType()
        {
            return DetectorType.PinHole;
        }

        public override List<UIElement> GetFigures()
        {
            List<UIElement> figureList = new List<UIElement>();

            foreach (PinHoleDefect defect in defectList)
            {
                System.Windows.Shapes.Rectangle rectFigure = new System.Windows.Shapes.Rectangle();
                switch (defect.Type)
                {
                    case PinHoleDefectType.PinHole:
                        rectFigure.Stroke = new SolidColorBrush(Colors.Red);
                        break;
                    case PinHoleDefectType.Dust:
                        rectFigure.Stroke = new SolidColorBrush(Colors.Yellow);
                        break;
                }

                rectFigure.StrokeThickness = 10;
                Canvas.SetLeft(rectFigure, defect.Rectangle.X);
                Canvas.SetTop(rectFigure, defect.Rectangle.Y);

                rectFigure.Width = defect.Rectangle.Width;
                rectFigure.Height = defect.Rectangle.Height;

                figureList.Add(rectFigure);
            }
            
            System.Windows.Shapes.Line lineStart = new System.Windows.Shapes.Line();
            lineStart.Stroke = new SolidColorBrush(Colors.Gold);
            lineStart.StrokeThickness = 10;
            lineStart.X1 = interestRegion.X;
            lineStart.Y1 = 0;
            lineStart.X2 = interestRegion.X;
            lineStart.Y2 = interestRegion.Height;
            
            System.Windows.Shapes.Line lineEnd = new System.Windows.Shapes.Line();
            lineEnd.Stroke = new SolidColorBrush(Colors.Gold);
            lineEnd.StrokeThickness = 10;
            lineEnd.X1 = (interestRegion.X + interestRegion.Width);
            lineEnd.Y1 = 0;
            lineEnd.X2 = (interestRegion.X + interestRegion.Width);
            lineEnd.Y2 = interestRegion.Height;
           
            figureList.Add(lineStart);
            figureList.Add(lineEnd);

            TextBlock textBlock = new TextBlock();
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.FontSize = 200;
            textBlock.Text = string.Format("{0:0.0}", average);

            Canvas.SetLeft(textBlock, (interestRegion.X + interestRegion.Width) / 2 - 100);
            Canvas.SetTop(textBlock, interestRegion.Height / 2 - 100);

            textBlock.Foreground = new SolidColorBrush(Colors.Green);

            figureList.Add(textBlock);

            if (edgeValueList.Count >= 2)
            {
                TextBlock textBlock1 = new TextBlock();
                textBlock1.FontSize = 150;
                textBlock1.Text = string.Format("{0:0.0}", edgeValueList[0]);

                Canvas.SetLeft(textBlock1, lineStart.X1 + 50);
                Canvas.SetTop(textBlock1, interestRegion.Height / 2 - 75);

                TextBlock textBlock2 = new TextBlock();
                textBlock2.FontSize = 150;
                textBlock2.Text = string.Format("{0:0.0}", edgeValueList[1]);

                Canvas.SetLeft(textBlock2, lineEnd.X1 - 450);
                Canvas.SetTop(textBlock2, interestRegion.Height / 2 - 75);

                textBlock1.Foreground = new SolidColorBrush(Colors.Gold);
                textBlock2.Foreground = new SolidColorBrush(Colors.Gold);

                figureList.Add(textBlock1);
                figureList.Add(textBlock2);
            }

            return figureList;
        }
    }
}
