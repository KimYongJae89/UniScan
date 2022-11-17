using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanS.Data;
using UniScanS.Screen.Vision.Detector;

namespace UniScanS.Screen.Data
{
    public class MergeSheetResult : SheetResult
    {
        bool importResult = false;
        public bool ImportResult
        {
            get { return importResult; }
        }

        int index;
        public int Index
        {
            get { return index; }
        }
        
        public bool IsNG
        {
            get { return SheetSubResultList.Count != 0; }
        }

        private string resultPath;
        public string ResultPath
        {
            get { return resultPath; }
        }

        public MergeSheetResult(int index, string path, bool import = true)
        {
            this.index = index;
            resultPath = path;
            if (import == true)
                this.Import(SheetInspector.TypeName, path);
        }

        public MergeSheetResult(int index, SheetResult sheetResult)
        {
            this.index = index;
            this.Copy(sheetResult);
        }

        public void AdjustSizeFilter(float minSize, float maxSize)
        {
            sheetAttackList = sheetAttackList.FindAll(s => s.RealLength >= minSize && s.RealLength <= maxSize);
            poleList = poleList.FindAll(p => p.RealLength >= minSize && p.RealLength <= maxSize);
            dielectricList = dielectricList.FindAll(d => d.RealLength >= minSize && d.RealLength <= maxSize);
            pinHoleList = pinHoleList.FindAll(p => p.RealLength >= minSize && p.RealLength <= maxSize);
            shapeList = shapeList.FindAll(s => s.RealLength >= minSize && s.RealLength <= maxSize);
        }

        public override void Export(string key, string path)
        {
            string fileName = Path.Combine(path, string.Format("{0}.csv", key));
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(AppendPostString());
            stringBuilder.AppendFormat("{0}\t{1}", SpandTime, SheetErrorType);
            stringBuilder.AppendLine();

            //if (prevImage != null)
            //{
            //    ImageHelper.SaveImage(prevImage, Path.Combine(path, string.Format("Prev.Jpg")));

            //    Bitmap colorImage = ImageHelper.MakeColor(prevImage);
            //    Graphics graphics = Graphics.FromImage(colorImage);

            //    DynMvp.UI.FigureGroup figureGroup = new DynMvp.UI.FigureGroup();
            //    foreach (SheetSubResult subResult in SheetSubResultList)
            //    {
            //        DynMvp.UI.Figure figure = subResult.GetFigure(50, 0.1f);
            //        figureGroup.AddFigure(figure);

            //        string text = string.Format("{0}-{1}", subResult.CamIndex, subResult.Index);

            //        int yPos = (int)figure.GetRectangle().Y - 75;
            //        if (yPos < 0)
            //            yPos = (int)figure.GetRectangle().Y + 75;

            //        Point point = new Point((int)figure.GetRectangle().X, yPos);
            //        DynMvp.UI.TextFigure textFigure = new DynMvp.UI.TextFigure(text, point, new Font(FontFamily.GenericSerif, 30), figure.FigureProperty.Pen.Color);
            //        figureGroup.AddFigure(textFigure);
            //    }
                    

            //    figureGroup.Draw(graphics, null, false);

            //    ImageHelper.SaveImage(colorImage, Path.Combine(path, string.Format("Map.Jpg")));
            //}
            
            foreach (SheetSubResult subResult in SheetSubResultList)
            {
                //ImageHelper.SaveImage(subResult.Image, Path.Combine(path, string.Format("{0}-{1}.bmp", subResult.CamIndex, subResult.Index)));

                stringBuilder.AppendFormat(subResult.ToExportString());
                stringBuilder.AppendLine();
            }

            File.WriteAllText(fileName, stringBuilder.ToString());
        }

        public void ImportPrevImage()
        {
            if (this.PrevImage == null)
                SheetCombiner.CombineImage(this);
        }

        public override void Import(string key, string path)
        {
            string fileName = Path.Combine(path, string.Format("{0}.csv", key));

            if (File.Exists(fileName) == false)
            {
                fileName = Path.Combine(path, string.Format("{0}.csv", this.index));
                if (File.Exists(fileName) == false)
                    return;
            }

            string[] lines = File.ReadAllLines(fileName);
            
            bool result = TimeSpan.TryParse(lines[0], out spandTime);
            
            if (result == false)
            {
                //SheetResultType = SheetResultType.Good;

                lines = lines.Skip(4).ToArray();
                result = TimeSpan.TryParse(lines[0], out spandTime);
                if (result == false)
                {
                    string[] splitLine = lines[0].Split('\t');
                    TimeSpan.TryParse(splitLine[0], out spandTime);
                    Enum.TryParse(splitLine[1], out sheetErrorType);
                }
            }

            if (sheetErrorType == SheetErrorType.None)
            {
                lines = lines.Skip(1).ToArray();

                foreach (string line in lines)
                {
                    string[] splitLine = line.Split('\t');

                    DefectType defectType = SheetSubResult.GetDefectType(splitLine[2]);

                    SheetSubResult sheetSubResult;
                    if (defectType == DefectType.Shape)
                        sheetSubResult = new ShapeResult();
                    else
                        sheetSubResult = new SheetSubResult();

                    sheetSubResult.CamIndex = Convert.ToInt32(splitLine[1]);
                    sheetSubResult.FromExportData(splitLine);

                    //sheetSubResult.RealLength = Math.Max(sheetSubResult.RealRegion.Width, sheetSubResult.RealRegion.Height) * 1000.0f;

                    switch (sheetSubResult.DefectType)
                    {
                        case DefectType.SheetAttack:
                            sheetAttackList.Add(sheetSubResult);
                            break;
                        case DefectType.Pole:
                            poleList.Add(sheetSubResult);
                            break;
                        case DefectType.Dielectric:
                            dielectricList.Add(sheetSubResult);
                            break;
                        case DefectType.PinHole:
                            pinHoleList.Add(sheetSubResult);
                            break;
                        case DefectType.Shape:
                            shapeList.Add((ShapeResult)sheetSubResult);
                            break;
                    }
                }
            }

            importResult = true;
        }
    }
}
