using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Settings;

namespace UniScanWPF.Table.Operation.Operators
{
    public class CanvasDefect
    {
        public IResultObject Defect { get => defect; }
        IResultObject defect;

        public Point[] RotateRectPointList { get => rotateRectPointList; set => rotateRectPointList = value; }
        Point[] rotateRectPointList;

        public CanvasDefect(IResultObject defect, ScanOperatorResult scanOperatorResult)
        {
            this.defect = defect;
            Point[] points = defect.GetPoints(1);

            if (scanOperatorResult != null)
            {
                float offsetX = scanOperatorResult.CanvasAxisPosition.Position[0] / Operator.DispResizeRatio;
                float offsetY = scanOperatorResult.CanvasAxisPosition.Position[1] / Operator.DispResizeRatio;
                for (int i = 0; i < points.Length; i++)
                    points[i].Offset(offsetX, offsetY);
                //boundingRect.Offset(offsetX, offsetY);
            }

            this.rotateRectPointList = new Point[points.Length];
            Array.Copy(points, rotateRectPointList, points.Length);
        }

        public CanvasDefect(string imagePath, XmlElement xmlElement)
        {
            this.defect = UniScanWPF.Table.Data.Defect.CreateDefect(imagePath, xmlElement);

            List<System.Windows.Point> pointList = new List<System.Windows.Point>();
            int j = 0;
            while (true)
            {
                XmlElement childNodeX = xmlElement[string.Format("X{0}", j)];
                XmlElement childNodeY = xmlElement[string.Format("Y{0}", j)];
                if (childNodeX != null && childNodeY != null)
                {
                    pointList.Add(new System.Windows.Point(
                      XmlHelper.GetValue(childNodeX, "", 0.0),
                      XmlHelper.GetValue(childNodeY, "", 0.0)));
                    j++;
                }
                else
                {
                    break;
                }
            }
            this.RotateRectPointList = pointList.ToArray();
        }

        public void Save(XmlElement xmlElement)
        {
            this.defect.Save(xmlElement);

            int length = this.rotateRectPointList.Length;
            for (int j = 0; j < length; j++)
            {
                XmlHelper.SetValue(xmlElement, string.Format("X{0}", j), this.rotateRectPointList[j].X);
                XmlHelper.SetValue(xmlElement, string.Format("Y{0}", j), this.rotateRectPointList[j].Y);
            }

        }

        public Shape GetShape()
        {
            Shape shape = null;
            if (rotateRectPointList.Length == 2)
            {
                Line lineShape = new Line();

                lineShape.X1 = rotateRectPointList[0].X;
                lineShape.Y1 = rotateRectPointList[0].Y;
                lineShape.X2 = rotateRectPointList[1].X;
                lineShape.Y2 = rotateRectPointList[1].Y;
                shape = lineShape;
            }
            else //if(rotateRectPointList.Length == 4)
            {
                Polygon polygon = new Polygon();

                polygon.Points = new PointCollection();
                Array.ForEach(rotateRectPointList, f => polygon.Points.Add(f));
                shape = polygon;
            }

            Brush brush = this.defect.GetBrush();
            shape.Fill = brush;
            shape.Stroke = brush;

            shape.Tag = this;
            return shape;
        }

        public Rect GetRect(int inflate = 0)
        {
            if (this.rotateRectPointList.Length == 0)
                return Rect.Empty;

            Rect[] rcects = this.rotateRectPointList.Select(f => new Rect(f.X, f.Y, 0, 0)).ToArray();
            Rect rect = rcects.Aggregate((f, g) => Rect.Union(f, g));
            rect.Inflate(inflate, inflate);
            return rect;
            //Point pt0 = this.rotateRectPointList[0];
            //Rect rect = new Rect(pt0.X, pt0.Y, 0, 0);
            //Array.ForEach(this.rotateRectPointList, f => rect = Rect.Union(rect, f));
            //rect.Inflate(inflate, inflate);
            return rect;
        }
    }

    public delegate void CombineCompletedEventHandler(List<CanvasDefect> canvasDefectList);
    public delegate void CombineClearEventHandler();

    public class ResultCombiner : INotifyPropertyChanged
    {
        //List<CanvasDefect>[] canvasDefectListArray;
        List<CanvasDefect> overallCanvasDefectList;

        public LightTuneResult LightTuneResult { get => this.lightTuneResult; }
        LightTuneResult lightTuneResult;

        public ScanOperatorResult[] ScanOperatorResultArray { get => this.scanOperatorResultArray; }
        ScanOperatorResult[] scanOperatorResultArray;

        public ExtractOperatorResult[] ExtractOperatorResultArray { get => this.extractOperatorResultArray; }
        ExtractOperatorResult[] extractOperatorResultArray;

        public ObservableCollection<CanvasDefect> CombineDefectList { get => combineDefectList; }
        ObservableCollection<CanvasDefect> combineDefectList = new ObservableCollection<CanvasDefect>();

        public StoringOperator StoringOperator { get => storingOperator; }
        StoringOperator storingOperator = new StoringOperator();

        public List<ExtraMeasure> ExtraMeasureList => overallCanvasDefectList.FindAll(f => f.Defect is ExtraMeasure).ConvertAll(f => (ExtraMeasure)f.Defect);
        public string[] ExtraValues
        {
            get
            {
                string[] values = new string[6] { "","","","","",""};
                InspectOperatorSettings setting = SystemManager.Instance().OperatorManager.InspectOperator.Settings;

                if (setting == null)
                    return values;

                bool measureL = (setting.MarginMeasureParam.DesignedUm.Height > 0);
                if (setting.MarginMeasureParam.DesignedUm.Height == setting.MarginMeasureParam.DesignedUm.Width)
                    values[0] = $"{setting.MarginMeasureParam.DesignedUm.Width}";
                else
                    values[0] = $"W {setting.MarginMeasureParam.DesignedUm.Width}{Environment.NewLine}L {setting.MarginMeasureParam.DesignedUm.Height}";

                values[1] = $"{setting.MarginMeasureParam.JudgementSpecUm}";

                System.Drawing.SizeF[] exxtraValues = GetExtraValueList(out bool lExist);

                if (exxtraValues.All(f => f.IsEmpty))
                    return values;

                {
                    if (measureL && lExist)
                    {
                        values[2] = $"W {exxtraValues[0].Width:F02}{Environment.NewLine}L {exxtraValues[0].Height:F02}";
                        values[3] = $"W {exxtraValues[1].Width:F02}{Environment.NewLine}L {exxtraValues[1].Height:F02}";
                        values[4] = $"W {exxtraValues[2].Width:F02}{Environment.NewLine}L {exxtraValues[2].Height:F02}";
                    }
                    else
                    {
                        values[2] = $"W {exxtraValues[0].Width:F02}";
                        values[3] = $"W {exxtraValues[1].Width:F02}";
                        values[4] = $"W {exxtraValues[2].Width:F02}";
                    }
                    values[5] = "";

                    System.Drawing.SizeF designed = setting.MarginMeasureParam.DesignedUm;
                    double spec = setting.MarginMeasureParam.JudgementSpecUm;

                    if (measureL && lExist)
                        values[5] = 
                            (Math.Abs(designed.Width - exxtraValues[0].Width) > spec) || 
                            (Math.Abs(designed.Width - exxtraValues[1].Width) > spec) || 
                            (Math.Abs(designed.Height - exxtraValues[0].Height) > spec) ||
                            (Math.Abs(designed.Height - exxtraValues[1].Height) > spec) ? "NG" : "OK";
                    else
                        values[5] =
                            (Math.Abs(designed.Width - exxtraValues[0].Width) > spec) ||
                            (Math.Abs(designed.Width - exxtraValues[1].Width) > spec) ? "NG" : "OK";
                }
                return values;
            }
        }

        public Brush ExtraJudgementBrush => ExtraValues[5] == "NG" ? new SolidColorBrush(Colors.OrangeRed) : new SolidColorBrush(Colors.White);

        public event CombineClearEventHandler CombineClear;
        public event CombineCompletedEventHandler CombineCompleted;
        public event PropertyChangedEventHandler PropertyChanged;

        public ResultCombiner()
        {
            scanOperatorResultArray = new ScanOperatorResult[DeveloperSettings.Instance.ScanNum];
            extractOperatorResultArray = new ExtractOperatorResult[DeveloperSettings.Instance.ScanNum];
            overallCanvasDefectList = new List<CanvasDefect>();
            //canvasDefectListArray = new List<CanvasDefect>[DeveloperSettings.Instance.ScanNum];

            //for (int i = 0; i < DeveloperSettings.Instance.ScanNum; i++)
            //    canvasDefectListArray[i] = new List<CanvasDefect>();
        }
        
        public System.Drawing.SizeF[] GetExtraValueList(out bool lExist)
        {
            List<float> wList = new List<float>();
            List<float> lList = new List<float>();

            this.ExtraMeasureList.ForEach(f =>
            {
                if (float.TryParse(f.Width, out float w) && w > 0)
                    wList.Add(w);

                if (float.TryParse(f.Height, out float h) && h > 0)
                    lList.Add(h);
            });

            System.Drawing.SizeF[] values = new System.Drawing.SizeF[3];
            if (wList.Count > 0)
            {
                values[0].Width = wList.Min();
                values[1].Width = wList.Max();
                values[2].Width = wList.Average();
            }

            lExist = false;
            if (lList.Count > 0)
            {
                values[0].Height= lList.Min();
                values[1].Height = lList.Max();
                values[2].Height = lList.Average();
                lExist = true;
            }
            return values;
        }

        public void AddResult(OperatorResult operatorResult)
        {
            switch (operatorResult.Type)
            {
                case ResultType.LightTune:
                    lightTuneResult = operatorResult as LightTuneResult;
                    OnPropertyChanged("LightTuneResult");
                    break;

                case ResultType.Scan:
                    scanOperatorResultArray[((ScanOperatorResult)operatorResult).FlowPosition] = operatorResult as ScanOperatorResult;
                    OnPropertyChanged("ScanOperatorResultArray");
                    break;

                case ResultType.Extract:
                    extractOperatorResultArray[((ExtractOperatorResult)operatorResult).ScanOperatorResult.FlowPosition] = operatorResult as ExtractOperatorResult;
                    OnPropertyChanged("ExtractOperatorResultArray");
                    break;

                case ResultType.Inspect:
                case ResultType.InspectLump:
                    InspectOperatorResult inspectOperatorResult = (InspectOperatorResult)operatorResult;
                    List<CanvasDefect> list = GetCanvasDefectList(inspectOperatorResult);
                    this.overallCanvasDefectList.AddRange(list);
                    //if (list.Count > 500)
                    //{
                    //    list.Sort((f, g) => g.Defect.DefectBlob.Area.CompareTo(f.Defect.DefectBlob.Area));
                    //    list.RemoveRange(500, list.Count - 500);
                    //}
                    
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Array temp = list.ToArray();
                        
                        lock (combineDefectList)
                        {
                            foreach (CanvasDefect canvasDefect in temp)
                            {
                                combineDefectList.Add(canvasDefect);
                            }
                        }

                        OnPropertyChanged("CombineDefectList");
                        OnPropertyChanged("ExtraMeasureList");
                        OnPropertyChanged("ExtraValues");
                        OnPropertyChanged("ExtraJudgementBrush");                        
                    }));

                    CombineCompleted(list);
                    break;

                //InspectOperatorResult inspectOperatorResult = (InspectOperatorResult)operatorResult;
                //List<CanvasDefect> canvasDefectList = RemoveIntersectDefect(inspectOperatorResult);

                //overallCanvasDefectList.AddRange(canvasDefectList);
                //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    combineDefectList = new ObservableCollection<CanvasDefect>(overallCanvasDefectList);
                //    OnPropertyChanged("CombineDefectList");
                //}));

                //CombineCompleted(canvasDefectList);
                //break;
                case ResultType.Train:
                    break;
            }
        }

        public void SaveInspectOperatorResult()
        {
            List<ExtractOperatorResult> extractOperatorResultList = this.extractOperatorResultArray.Cast<ExtractOperatorResult>().ToList();
            List<CanvasDefect> canvasDefectList = new List<CanvasDefect>(this.overallCanvasDefectList);
            storingOperator.Save(extractOperatorResultList, canvasDefectList);
        }

        private List<CanvasDefect> GetCanvasDefectList(InspectOperatorResult inspectOperatorResult)
        {
            int? currentPosition = inspectOperatorResult.ExtractOperatorResult?.ScanOperatorResult?.FlowPosition;
            //int prevPosition = currentPosition - 1;
            //int nextPosition = currentPosition + 1;

            List<CanvasDefect> temp = inspectOperatorResult.DefectList.ConvertAll(f => new CanvasDefect(f, inspectOperatorResult.ExtractOperatorResult?.ScanOperatorResult));
            return temp;
            //List<CanvasDefect> temp = new List<CanvasDefect>();
            //foreach (Defect defect in inspectOperatorResult.DefectList)
            //    temp.Add(new CanvasDefect(defect, inspectOperatorResult.ExtractOperatorResult.ScanOperatorResult));
            //temp.AddRange(.ToArray());

            //if (prevPosition >= 0)
            //    RemoveIntersectDefect(inspectOperatorResult.DefectList, inspectOperatorResult.ExtractOperatorResult.ScanOperatorResult, prevPosition, ref temp);

            //if (nextPosition < DeveloperSettings.Instance.ScanNum)
            //    RemoveIntersectDefect(inspectOperatorResult.DefectList, inspectOperatorResult.ExtractOperatorResult.ScanOperatorResult, nextPosition, ref temp);

            //lock (canvasDefectListArray[currentPosition])
            //    canvasDefectListArray[currentPosition].AddRange(temp);
        }

        private void RemoveIntersectDefect(int position, ref List<CanvasDefect> temp)
        {

            //temp.RemoveAll(canvasDefectListArray[position].Any(intersectDefect => defect.BoundingRect.IntersectsWith(intersectDefect.BoundingRect)));
            //foreach (CanvasDefect defect in temp)
            //{
            //    if ()
            //    {

            //        continue;
            //    }
            //        continue;

            //    temp.Add(canvasDefect);
            //}
        }

        public void Alloc(int scanCount)
        {
            scanOperatorResultArray = new ScanOperatorResult[scanCount];
            for (int i = 0; i < scanCount; i++)
                scanOperatorResultArray[i] = null;
        }

        public void Clear(bool clearImage = true)
        {
            if (clearImage == true)
            {
                Array.Clear(scanOperatorResultArray, 0, scanOperatorResultArray.Length);
                Array.Clear(extractOperatorResultArray, 0, extractOperatorResultArray.Length);
                //for (int i = 0; i < DeveloperSettings.Instance.ScanNum; i++)
                //{
                //    scanOperatorResultArray[i] = null;
                //    extractOperatorResultArray[i] = null;
                //}

                lightTuneResult = null;
            }

            overallCanvasDefectList.Clear();

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                combineDefectList.Clear();
            }));

            CombineClear();
            
            OnPropertyChanged("ScanOperatorResultArray");
            OnPropertyChanged("LightTuneResult");
            OnPropertyChanged("CombineDefectList");
            OnPropertyChanged("ExtraValues");
            OnPropertyChanged("ExtraJudgementBrush");
        }

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}