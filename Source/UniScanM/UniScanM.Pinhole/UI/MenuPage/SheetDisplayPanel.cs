using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanM.Pinhole.Data;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.UI;
using UniScanM.Pinhole.Settings;

namespace UniScanM.Pinhole.UI.MenuPanel
{
    public delegate void DefectSelectedDelegate(DefectInfo defectInfo);

    public partial class SheetDisplayPanel : UserControl
    {
        float fovWidth;
        float cameraOffset;
        float sheetLength;
        float sheetLeft;
        float sheetWidth;

        int leftMargin;
        int rightMargin;
        int topMargin;
        int bottomMargin;

        int numColumn = 5; // To Setting
        int numRow = 10; // To Setting

        int markerSize = 5;

        int scrollPos = 0;
        bool bZoomSheet = true;
        int displayLength = 10;

        bool bRefreshAll = true;

        //InspectPage inspectPage;
        //public InspectPage InspectPage { set => inspectPage = value; }

        FigureGroup defectMarkFigures = new FigureGroup();
        FigureGroup lastDefectMarkFigures = new FigureGroup();

        Data.InspectionResult rollInspectResult;
        public Data.InspectionResult RollInspectResult { get => rollInspectResult; set => rollInspectResult = value; }

        public DefectSelectedDelegate DefectSelected;

        public SheetDisplayPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            InitSetttings();
            this.DefectSelected += Selected;
            nudSheetLength.Value = displayLength;
        }

        private void InitSetttings()
        {
            fovWidth = UISettings.Instance().FovWidth;
            cameraOffset = UISettings.Instance().CameraOffset;
            sheetLength = UISettings.Instance().SheetLength;
            sheetLeft = UISettings.Instance().SheetLeft;
            sheetWidth = UISettings.Instance().SheetWidth;

            leftMargin = UISettings.Instance().LeftMargin;
            rightMargin = UISettings.Instance().RightMargin;
            topMargin = UISettings.Instance().TopMargin;
            bottomMargin = UISettings.Instance().BottomMargin;
        }

        // Flickering 을 없애기 위해 아래 함수 필요... 없애지 말것
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // base.OnPaintBackground(e);
        }

        public void RefreshPanel()
        {
            if (rollInspectResult == null)
                return;

            defectMarkFigures.Clear();
            AddResult(rollInspectResult, false);

            bRefreshAll = true;
        }

        public void AddResult(Data.InspectionResult inspectResult, bool bDrawMark)
        {
            if (inspectResult == null)
                return;

            PointF lastPosition = new PointF();

            lock (inspectResult)
            {
                int xMargin = leftMargin + rightMargin;
                int yMargin = topMargin + bottomMargin;

                int startPos = (int)(scrollPos * displayLength);
                int endPos = (int)((scrollPos + 1) * displayLength);

                bool posValid = false;
                //Parallel.ForEach(inspectResult.DefectInfoMap, (KeyValuePair<int, DefectInfo> keyValue) =>
                Parallel.For(0, inspectResult.NumDefect, (i) =>
                {
                    DefectInfo defectInfo = inspectResult[i];
                    if (defectInfo != null)
                    {
                        PointF defectPtPel = new PointF();

                        lastPosition = defectInfo.RealPosition;

                        if (defectInfo.CameraIndex == 0)
                            defectPtPel.X = (defectInfo.RealPosition.X / fovWidth) * (panelDefectMap.Width - xMargin) + leftMargin;
                        else
                            defectPtPel.X = (((defectInfo.RealPosition.X + cameraOffset) / fovWidth) * (panelDefectMap.Width - xMargin)) + rightMargin;

                        if (bZoomSheet)
                        {
                            if (defectInfo.RealPosition.Y >= startPos*1000 && defectInfo.RealPosition.Y <= endPos*1000)
                            {
                                defectPtPel.Y = (((defectInfo.RealPosition.Y / 1000 - startPos) / (float)displayLength) * (panelDefectMap.Height - yMargin) + topMargin * 2);
                                posValid = true;
                            }
                            else
                            {
                                posValid = false;
                            }
                        }
                        else
                        {
                            defectPtPel.Y = (defectInfo.RealPosition.Y / 1000 / sheetLength) * (panelDefectMap.Height - yMargin) + topMargin;
                            posValid = true;
                        }

                        if (posValid)
                        {
                            Figure defectMark;
                            if (defectInfo.DefectType == Data.DefectType.Dust)
                                defectMark = new XRectFigure(defectPtPel, markerSize, new Pen(Color.Red));
                            else
                                defectMark = new EllipseFigure(defectPtPel, markerSize, new Pen(Color.LightGreen));

                            defectMark.Tag = defectInfo;

                            lock (lastDefectMarkFigures)
                                lastDefectMarkFigures.AddFigure(defectMark);

                            lock (defectMarkFigures)
                                defectMarkFigures.AddFigure(defectMark);
                        }
                    }
                });
            }

            if (lastDefectMarkFigures.NumFigure == 0 && bZoomSheet)
            {
                Invoke(new Action(() =>
                {
                    vsbDefectMap.Value = scrollPos = (int)(lastPosition.Y / 1000 / displayLength);
                    //RefreshPanel();
                }));

                return;
            }
            else
                Invalidate();
        }

        private void panelDefectMap_Paint(object sender, PaintEventArgs e)
        {
            if (bRefreshAll)
            {
                BufferedGraphics myBuffer = BufferedGraphicsManager.Current.Allocate(e.Graphics, this.DisplayRectangle);

                myBuffer.Graphics.Clear(SystemColors.Control);

                DrawGrid(myBuffer.Graphics);

                lock (defectMarkFigures)
                    defectMarkFigures.Draw(myBuffer.Graphics, new CoordTransformer(), false);

                myBuffer.Render();

                bRefreshAll = false;
            }
            else
            {
                lock (lastDefectMarkFigures)
                {
                    lastDefectMarkFigures.Draw(e.Graphics, new CoordTransformer(), false);
                    lastDefectMarkFigures.Clear();
                }
            }
        }

        private void DrawGrid(Graphics g)
        {
            float xStep = fovWidth / numColumn;

            float yStartPos = (float)(scrollPos * displayLength);
            float yEndPos = (float)((scrollPos + 1) * displayLength);

            if (bZoomSheet)
            {
                yStartPos = (float)(scrollPos * displayLength);
                yEndPos = (float)((scrollPos + 1) * displayLength);
            }
            else
            {
                yStartPos = (float)0;
                yEndPos = (float)sheetLength;
            }

            float yLength = yEndPos - yStartPos;
            float tempYStep = yLength / numRow;
            int order = (int)Math.Exp((int)Math.Log10(tempYStep)-1);
            float yStep;
            if (order > 0)
                yStep = (int)(tempYStep / order) * order;
            else
                yStep = tempYStep;

            int xMargin = leftMargin + rightMargin;
            int yMargin = topMargin + bottomMargin;

            Pen gridPen = new Pen(Color.LightGray);

            for (float x = 0; x < fovWidth; x += xStep)
            {
                float posX = (x / fovWidth) * (panelDefectMap.Width - xMargin) + leftMargin;
                g.DrawLine(gridPen, posX, topMargin, posX, panelDefectMap.Height - bottomMargin);
            }

            Pen sheetPen = new Pen(Color.Green);

            float posSheetLeft = (sheetLeft / fovWidth) * (panelDefectMap.Width - xMargin) + leftMargin;
            g.DrawLine(sheetPen, posSheetLeft, topMargin, posSheetLeft, panelDefectMap.Height - bottomMargin);

            float posSheetRight = ((sheetLeft + sheetWidth) / fovWidth) * (panelDefectMap.Width - xMargin) + leftMargin;
            g.DrawLine(sheetPen, posSheetRight, topMargin, posSheetRight, panelDefectMap.Height - bottomMargin);

            Font font = new Font(FontFamily.GenericSansSerif, 8);
            Brush brush = new SolidBrush(Color.DarkGray);

            for (float y = yStartPos; y < yEndPos; y += yStep)
            {
                float posY = ((y - yStartPos) / yLength) * (panelDefectMap.Height - yMargin) + topMargin;
                g.DrawLine(gridPen, leftMargin, posY, panelDefectMap.Width - rightMargin, posY);
                g.DrawString(y.ToString(), font, brush, leftMargin, posY);
            }
        }

        private void vsbDefectMap_Scroll(object sender, ScrollEventArgs e)
        {
            scrollPos = vsbDefectMap.Value;
            RefreshPanel();
        }

        private void panelDefectMap_MouseClick(object sender, MouseEventArgs e)
        {
            Figure figureSelected = defectMarkFigures.Select(new PointF(e.X, e.Y));
            if (figureSelected != null && DefectSelected != null)
            {
                DefectSelected((DefectInfo)figureSelected.Tag);
            }
        }

        private void Selected(DefectInfo defectInfo)
        {
            // 추적 하는 기능 추가
        }

        private void SheetDisplayPanel_Load(object sender, EventArgs e)
        {
            Invalidate();
        }

        void LoadOption()
        {

        }

        private void nudSheetLength_ValueChanged(object sender, EventArgs e)
        {
            displayLength = (int)nudSheetLength.Value;
            RefreshPanel();
        }

        private void chkZoomSheet_CheckedChanged(object sender, EventArgs e)
        {
            bZoomSheet = chkZoomSheet.Checked;
            RefreshPanel();
        }

        private void SheetDisplayPanel_SizeChanged(object sender, EventArgs e)
        {
            RefreshPanel();
        }

        private void panelDefectMap_SizeChanged(object sender, EventArgs e)
        {
            bRefreshAll = true;
        }
    }
}
