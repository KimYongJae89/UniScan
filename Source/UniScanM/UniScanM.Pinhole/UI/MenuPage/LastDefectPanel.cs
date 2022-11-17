using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Data.UI;
using DynMvp.UI;
using UniScanM.Pinhole.Data;
using DynMvp.Base;
using DynMvp.InspData;
using UniScanM.Pinhole.Settings;

namespace UniScanM.Pinhole.UI.MenuPage
{
    public partial class LastDefectPanel : UserControl, IMultiLanguageSupport
    {
        CanvasPanel canvasPanel;
        private Data.InspectionResult lastInspectResult;
        IAsyncResult asyncUpdatePage = null;
        int index = 0;
        float resizeRatio;

        public LastDefectPanel()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitViewSetup();
            //defectList.RowTemplate.Height = (defectList.Height - defectList.ColumnHeadersHeight) / 4;
            resizeRatio = PinholeSettings.Instance().ResizeRatio;
            StringManager.AddListener(this);
        }

        public LastDefectPanel(int index)
        {
            InitializeComponent();
            
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.index = index;
            InitViewSetup();
            //defectList.RowTemplate.Height = (defectList.Height - defectList.ColumnHeadersHeight) / 4;


            StringManager.AddListener(this);
        }

        void InitViewSetup()
        {
            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.ShowCenterGuide = false;
            this.canvasPanel.ShowToolbar = false;
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.ReadOnly = true;
            this.canvasPanel.Margin = new Padding(0,0,0,0);
            resizeRatio = PinholeSettings.Instance().ResizeRatio;
            panelLastFovImage.Controls.Add(canvasPanel);

            if(index == 1)
                canvasPanel.HorizontalAlignment = HorizontalAlignment.Right;
            else 
                canvasPanel.HorizontalAlignment = HorizontalAlignment.Left;
        }
        

        public void Clear()
        {
            canvasPanel.Clear();
            canvasPanel.UpdateImage(null);
            labelLastDefectTime.Text = "";
        }

        public void UpdateLastDefect(Data.InspectionResult inspectResult)
        {
            if (inspectResult.Judgment == Judgment.Accept)
                return;

            //if (inspectResult.GetTotalDefectCount() > 0)
            //{
            //    lastInspectResult = inspectResult;
            //    //defectList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            //    for (int i=0; i<PinholeSettings.Instance().MaxDefect; i++)
            //    {
            //        if (inspectResult.LastDefectInfoList.Count() <= i)
            //            break;

            //        DefectInfo defectInfo = inspectResult.LastDefectInfoList[i];

            //        if (defectList.Rows.Count <= i)
            //        {
            //            defectList.Rows.Add(defectInfo.SectionIndex, String.Format("{0:0.0}", defectInfo.RealPosition.X), String.Format("{0:0.0}", defectInfo.RealPosition.Y),
            //                        String.Format("{0:0.0}", defectInfo.BoundingRect.Width), String.Format("{0:0.0}", defectInfo.BoundingRect.Height), defectInfo.DefectType.ToString());
            //        }
            //        else
            //        {
            //            defectList.Rows[i].Cells[0].Value = defectInfo.SectionIndex.ToString();
            //            defectList.Rows[i].Cells[1].Value = String.Format("{0:0.0}", defectInfo.RealPosition.X);
            //            defectList.Rows[i].Cells[2].Value = String.Format("{0:0.0}", defectInfo.RealPosition.Y);
            //            defectList.Rows[i].Cells[3].Value = String.Format("{0:0.0}", defectInfo.BoundingRect.Width);
            //            defectList.Rows[i].Cells[4].Value = String.Format("{0:0.0}", defectInfo.BoundingRect.Height);
            //            defectList.Rows[i].Cells[5].Value = defectInfo.DefectType.ToString();
            //            //defectList.Rows[i].Cells[6].Value = defectInfo.ClipImage;
            //        }
            //    }
            //    //defectList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //}
            Bitmap bitmap = inspectResult.DisplayBitmap;
            if (canvasPanel.Image == null)
            {
                canvasPanel.UpdateImage(bitmap);
                canvasPanel.ZoomFit();
            }
            else
            {
                canvasPanel.UpdateImage(bitmap);
            }

            int imageHeight = bitmap.Height;
            int halfWidth = bitmap.Width;
            int halfHeight = bitmap.Height;
            Font font = new Font(FontFamily.GenericSansSerif, 6);
            float pelSize = PinholeSettings.Instance().PixelResolution;
            Point imgCenterPt = new Point(halfWidth, halfHeight);

            int index = 1;

            FigureGroup figureGroup = new FigureGroup();

            string defectTime = inspectResult.InspectionStartTime.ToString("yyyy-MM-dd, HH:mm:ss.fff");

            labelLastDefectTime.Text = string.Format("{0}-{1}:{2}", inspectResult.LotNo, inspectResult.SectionIndex, defectTime);

            foreach (DefectInfo defectInfo in inspectResult.LastDefectInfoList)
            {
                RectangleF boundingRect = defectInfo.BoundingRect;
                float realWidth = boundingRect.Width * pelSize;
                float realHeight = boundingRect.Height * pelSize;
                Color defectColor = (defectInfo.DefectType == Data.DefectType.Dust ? Color.Red : Color.Orange);

                Figure defectMark = defectInfo.GetDefectMark(defectColor, resizeRatio);

                PointF centerPt = DrawingHelper.CenterPoint(boundingRect);
                //StringAlignment alignment = (centerPt.X > halfWidth ? StringAlignment.Near : StringAlignment.Far);
                //StringAlignment lineAlignment = (centerPt.Y > halfHeight ? StringAlignment.Far : StringAlignment.Near);
                //PointF numPt = new PointF();
                //numPt.X = (centerPt.X > halfWidth ? boundingRect.Right + 5 : boundingRect.Left - 5);
                //numPt.Y = (centerPt.Y > halfHeight ? boundingRect.Bottom : boundingRect.Top);

                PointF numPt = new PointF(2, (index - 1) * 10 + imageHeight);
                StringAlignment alignment = StringAlignment.Near;
                StringAlignment lineAlignment = StringAlignment.Near;
                String caption = String.Format("{0}:W{1}/H{2}", index, realWidth, realHeight);

                TextFigure infoTextFigure = new TextFigure(caption, Point.Truncate(numPt), font, defectColor, alignment, lineAlignment);
                //LineFigure lineFigure = new LineFigure(numPt, centerPt, new Pen(defectColor, 1));
                TextFigure numTextFigure = new TextFigure(index.ToString(), Point.Truncate(centerPt), font, defectColor, StringAlignment.Center, StringAlignment.Center);

                figureGroup.AddFigure(defectMark);
                figureGroup.AddFigure(numTextFigure);
                figureGroup.AddFigure(infoTextFigure);

                index++;
            }


            canvasPanel.WorkingFigures.Clear();
            canvasPanel.WorkingFigures.AddFigure(figureGroup.FigureList.ToArray());
            canvasPanel.Invalidate();
        }

        //public void UpdateLastDefect(Data.InspectionResult inspectResult)
        //{
        //    if (inspectResult.Judgment == Judgment.Accept)
        //        return;

        //    if (inspectResult.GetTotalDefectCount() > 0)
        //    {
        //        lastInspectResult = inspectResult;
        //        //defectList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

        //        for (int i = 0; i < PinholeSettings.Instance().MaxDefect; i++)
        //        {
        //            if (inspectResult.LastDefectInfoList.Count() <= i)
        //                break;

        //            DefectInfo defectInfo = inspectResult.LastDefectInfoList[i];

        //            if (defectList.Rows.Count <= i)
        //            {
        //                defectList.Rows.Add(defectInfo.SectionIndex, String.Format("{0:0.0}", defectInfo.RealPosition.X), String.Format("{0:0.0}", defectInfo.RealPosition.Y),
        //                            String.Format("{0:0.0}", defectInfo.BoundingRect.Width), String.Format("{0:0.0}", defectInfo.BoundingRect.Height), defectInfo.DefectType.ToString());
        //            }
        //            else
        //            {
        //                defectList.Rows[i].Cells[0].Value = defectInfo.SectionIndex.ToString();
        //                defectList.Rows[i].Cells[1].Value = String.Format("{0:0.0}", defectInfo.RealPosition.X);
        //                defectList.Rows[i].Cells[2].Value = String.Format("{0:0.0}", defectInfo.RealPosition.Y);
        //                defectList.Rows[i].Cells[3].Value = String.Format("{0:0.0}", defectInfo.BoundingRect.Width);
        //                defectList.Rows[i].Cells[4].Value = String.Format("{0:0.0}", defectInfo.BoundingRect.Height);
        //                defectList.Rows[i].Cells[5].Value = defectInfo.DefectType.ToString();
        //                //defectList.Rows[i].Cells[6].Value = defectInfo.ClipImage;
        //            }
        //        }
        //        //defectList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        //    }

        //    if (canvasPanel.Image == null)
        //    {
        //        canvasPanel.UpdateImage(inspectResult.DisplayBitmap);
        //        canvasPanel.ZoomFit();
        //    }
        //    else
        //    {
        //        canvasPanel.UpdateImage(inspectResult.DisplayBitmap);
        //    }

        //    FigureGroup figureGroup = new FigureGroup();

        //    foreach (DefectInfo defectInfo in inspectResult.LastDefectInfoList)
        //    {
        //        Figure defectMark;
        //        RectangleF boundingRect = defectInfo.BoundingRect;
        //        boundingRect.X = boundingRect.X * resizeRatio;
        //        boundingRect.Y = boundingRect.Y * resizeRatio;
        //        boundingRect.Width = boundingRect.Width * resizeRatio;
        //        boundingRect.Height = boundingRect.Height * resizeRatio;
        //        PointF point = DrawingHelper.CenterPoint(boundingRect);
        //        defectMark = new CrossFigure(DrawingHelper.CenterPoint(boundingRect), 12, new Pen(Color.Red, 2));

        //        //defectMark.Tag = defectInfo;

        //        figureGroup.AddFigure(defectMark);
        //    }


        //    canvasPanel.WorkingFigures.Clear();
        //    //canvasPanel.WorkingFigures.AddFigure(figureGroup);
        //    canvasPanel.WorkingFigures = figureGroup;
        //    canvasPanel.Invalidate();
        //}

        private void defectList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            //lock (lastInspectResult)
            //{
            //    int curIndex = e.RowIndex;
            //    DefectInfo defectInfo = null;

            //    if (lastInspectResult.LastDefectInfoList.Count() == 0)
            //        return;

            //    if (lastInspectResult.GetTotalDefectCount() <= curIndex)
            //        return;

            //    defectInfo = lastInspectResult[curIndex];

            //    if (defectInfo == null)
            //        return;

            //    switch (e.ColumnIndex)
            //    {
            //        case 0:
            //            e.Value = defectInfo.SectionIndex;
            //            break;
            //        case 1:
            //            e.Value = String.Format("{0:0.0}", defectInfo.RealPosition.X);
            //            break;
            //        case 2:
            //            e.Value = String.Format("{0:0.0}", defectInfo.RealPosition.Y);
            //            break;
            //        case 3:
            //            e.Value = defectInfo.DefectType.ToString();
            //            break;
            //        case 4:
            //            e.Value = String.Format("{0:0.0}", defectInfo.BoundingRect.Width);
            //            break;
            //        case 5:
            //            e.Value = String.Format("{0:0.0}", defectInfo.BoundingRect.Height);
            //            break;
            //        case 6:
            //            e.Value = defectInfo.ClipImage.Clone();
            //            break;
            //    }
            //}
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
