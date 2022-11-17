using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniScanS.Common.Data;

namespace UniScanS.Common.Settings.Monitor.UI
{
    public partial class FovSettingForm : Form
    {
        List<InspectorFovPanel> inspectorFovPanelList = new List<InspectorFovPanel>();
        CanvasPanel canvasPanel = new CanvasPanel();

        public List<InspectorInfo> inspectorInfoList = null;

        public FovSettingForm(List<InspectorInfo> inspectorInfoList)
        {
            InitializeComponent();

            this.inspectorInfoList = inspectorInfoList;

            layoutInspector.ColumnStyles.Clear();
            foreach (InspectorInfo inspectorInfo in inspectorInfoList)
            {
                InspectorFovPanel inspectorFovPanel = new InspectorFovPanel(inspectorInfo);
                inspectorFovPanelList.Add(inspectorFovPanel);

                layoutInspector.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                layoutInspector.Controls.Add(inspectorFovPanel);
                inspectorFovPanel.UpdateData();
            }

            layoutInspector.ColumnCount = inspectorInfoList.Count;

            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.SetPanMode();
            canvasPanel.ShowCenterGuide = false;
            panelImage.Controls.Add(canvasPanel);
            
            MatroxHelper.InitApplication();
            //foreach (ColumnStyle columnStyle in layoutInspector.ColumnStyles)
            //{
            //    columnStyle.SizeType = SizeType.Percent;
            //    columnStyle.Width = 50.0f;
            //}
        }

        ~FovSettingForm()
        {
            MatroxHelper.FreeApplication();
        }

        private void UpdateMonitorFov()
        {
            width.Value = (decimal)SystemTypeSettings.Instance().MonitorFov.Width;
            height.Value = (decimal)SystemTypeSettings.Instance().MonitorFov.Height;
        }

        private void matchingWidth_ValueChanged(object sender, EventArgs e)
        {
            foreach (InspectorFovPanel inspectorFovPanel in inspectorFovPanelList)
            {
                if (inspectorFovPanel.Image != null)
                    inspectorFovPanel.UpdateRegion(new Rectangle(inspectorFovPanel.Image.Width - (int)matchingWidth.Value, (int)matchingY.Value, (int)matchingWidth.Value, (int)matchingHeight.Value));
            }
        }

        private void matchingHeight_ValueChanged(object sender, EventArgs e)
        {
            foreach (InspectorFovPanel inspectorFovPanel in inspectorFovPanelList)
            {
                if (inspectorFovPanel.Image != null)
                    inspectorFovPanel.UpdateRegion(new Rectangle(inspectorFovPanel.Image.Width - (int)matchingWidth.Value, (int)matchingY.Value, (int)matchingWidth.Value, (int)matchingHeight.Value));
            }
        }

        private void buttonOverlap_Click(object sender, EventArgs e)
        {
            CalcOverlap();

            foreach (InspectorFovPanel inspectorFovPanel in inspectorFovPanelList)
            {
                if (inspectorFovPanel.Image != null)
                    inspectorFovPanel.UpdateData();
            }
        }

        private void buttonBasic_Click(object sender, EventArgs e)
        {
            ClacBasic();
            foreach (InspectorFovPanel inspectorFovPanel in inspectorFovPanelList)
            {
                if (inspectorFovPanel.Image != null)
                    inspectorFovPanel.UpdateData();
            }
        }

        private void ClacBasic()
        {
            for (int i = 0; i < inspectorFovPanelList.Count; i++)
            {
                if (inspectorFovPanelList[i].Image == null)
                    continue;

                AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, inspectorFovPanelList[i].Image, ImageType.Grey);
                int width = algoImage.Width;

                //관심영역 구간
                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
                int thValue = (int)Math.Round(imageProcessing.Li(algoImage));

                AlgoImage binImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, algoImage.Width, algoImage.Height);
                imageProcessing.Binarize(algoImage, binImage, thValue, true);

                float[] vArray = imageProcessing.Projection(binImage, Direction.Vertical, ProjectionType.Mean);
                Tuple<int, int> verticalPos = FindVerticalPos(vArray);

                //첫 구간, 마지막 구간
                if (i == 0 || i == inspectorFovPanelList.Count - 1)
                {
                    float[] hArray = imageProcessing.Projection(binImage, Direction.Horizontal, ProjectionType.Mean);
                    int hPos = FindHorizentalPos(hArray);

                    if (width / 2 > hPos)
                    {
                        inspectorFovPanelList[i].InspectorInfo.Fov = new RectangleF(hPos, verticalPos.Item1, width - hPos, verticalPos.Item2 - verticalPos.Item1);
                    }
                    else
                    {
                        inspectorFovPanelList[i].InspectorInfo.Fov = new RectangleF(0, verticalPos.Item1, width - hPos, verticalPos.Item2 - verticalPos.Item1);
                    }
                }
                //가운데
                else
                {
                    inspectorFovPanelList[i].InspectorInfo.Fov = new RectangleF(0, verticalPos.Item1, width, verticalPos.Item2 - verticalPos.Item1);
                }

                algoImage.Dispose();
                binImage.Dispose();
            }
        }

        private void CalcOverlap()
        {
            float refSumOverlap = 0;

            for (int i = 0; i < inspectorFovPanelList.Count - 1; i++)
            {
                if (inspectorFovPanelList[i].Image == null || inspectorFovPanelList[i + 1].Image == null)
                    continue;

                if (matchingWidth.Value <= 0 || matchingWidth.Value > inspectorFovPanelList[i].Image.Width || matchingWidth.Value > inspectorFovPanelList[i + 1].Image.Width)
                    continue;

                if (matchingHeight.Value <= 0 || matchingY.Value + matchingHeight.Value > inspectorFovPanelList[i].Image.Height || matchingY.Value + matchingHeight.Value > inspectorFovPanelList[i + 1].Image.Height)
                    continue;

                AlgoImage algoImage1 = ImageBuilder.Build(ImagingLibrary.MatroxMIL, inspectorFovPanelList[i].Image, ImageType.Grey);
                AlgoImage algoImage2 = ImageBuilder.Build(ImagingLibrary.MatroxMIL, inspectorFovPanelList[i + 1].Image, ImageType.Grey);
                
                //겹치는 부분 구간
                MilPattern milPattern = new MilPattern();

                PatternMatchingParam patternMatchingParam = new PatternMatchingParam();

                patternMatchingParam.MatchScore = (int)score.Value;
                patternMatchingParam.SpeedType = 0;
                patternMatchingParam.MinAngle = 0;
                patternMatchingParam.MaxAngle = 0;
                patternMatchingParam.IgnorePolarity = false;
                patternMatchingParam.UseAngle = false;
                patternMatchingParam.NumToFind = 0;

                int patternWidth = (int)matchingWidth.Value;
                int patternHeight = (int)matchingHeight.Value;

                int patternX = algoImage1.Width - patternWidth;
                int patternY = (int)matchingY.Value;
                AlgoImage patternImage = algoImage1.GetSubImage(new Rectangle(patternX, patternY, patternWidth, patternHeight));

                milPattern.Train(patternImage, patternMatchingParam);
                PatternResult result = milPattern.Inspect(algoImage2, patternMatchingParam, new DebugContext(false, ""));
 
                if (result.Good == true)
                {
                    MatchPos pos = result.MatchPosList.OrderByDescending(p => p.Pos.X).Last();

                    inspectorFovPanelList[i + 1].InspectorInfo.Fov =
                        new RectangleF(
                            pos.Rect.X + patternWidth,
                            inspectorFovPanelList[i + 1].InspectorInfo.Fov.Y - (patternY - pos.Rect.Y) - refSumOverlap,
                            inspectorFovPanelList[i + 1].InspectorInfo.Fov.Width - (pos.Rect.X + patternWidth),
                            inspectorFovPanelList[i + 1].InspectorInfo.Fov.Height
                            );

                    refSumOverlap += patternY - pos.Rect.Y;
                }

                

                milPattern.Dispose();

                algoImage1.Dispose();
                algoImage2.Dispose();
            }
        }

        private List<Tuple<int, int>> GetDielectricSpaceList(float[] array)
        {
            double avgValue = array.Average() / 2.0;

            List<Tuple<int, int>> edgeList = new List<Tuple<int, int>>();

            bool isDielectricSpace = false;

            int startPos = 0;
            int endPos = 0;

            //처음에 255..
            if (array[0] < avgValue)
                isDielectricSpace = true;

            for (int i = 1; i < array.Count(); i++)
            {
                //검은색 -> 흰색
                if (array[i - 1] >= avgValue && array[i] < avgValue)
                {
                    isDielectricSpace = true;
                    startPos = i;
                }

                //반대
                if (array[i - 1] < avgValue && array[i] >= avgValue)
                {
                    if (isDielectricSpace == true)
                    {
                        isDielectricSpace = false;
                        endPos = i;
                        edgeList.Add(new Tuple<int, int>(startPos, endPos));
                    }
                }
            }

            if (isDielectricSpace == true)
            {
                isDielectricSpace = false;
                endPos = array.Count() - 1;
                edgeList.Add(new Tuple<int, int>(startPos, endPos));
            }

            return edgeList;
        }

        private int FindHorizentalPos(float[] hArray)
        {
            List<Tuple<int, int>> edgeList = GetDielectricSpaceList(hArray);

            Tuple<int, int> leftEdge = edgeList.First();
            Tuple<int, int> rightEdge = edgeList.Last();

            return leftEdge.Item2 - leftEdge.Item1 > rightEdge.Item2 - rightEdge.Item1 ? leftEdge.Item2 : rightEdge.Item1;
        }

        private Tuple<int, int> FindVerticalPos(float[] vArray)
        {
            List<Tuple<int, int>> edgeList = GetDielectricSpaceList(vArray);

            Tuple<int, int> topEdge = edgeList.First();
            Tuple<int, int> bottomEdge = edgeList.Last();

            return new Tuple<int, int>(topEdge.Item2, bottomEdge.Item1);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            int imageWidth = 0;
            int imageHeight = 0;

            foreach (InspectorFovPanel inspectorFovPanel in inspectorFovPanelList)
            {
                foreach (InspectorInfo inspectorInfo in inspectorInfoList)
                {
                    if (inspectorFovPanel.InspectorInfo == inspectorInfo)
                    {
                        inspectorInfo.Fov = inspectorFovPanel.InspectorInfo.Fov;
                        imageWidth += (int)inspectorInfo.Fov.Width;
                        imageHeight = Math.Max(imageHeight, (int)inspectorInfo.Fov.Height);
                    }
                }
            }
            
            SystemTypeSettings.Instance().MonitorFov = new RectangleF(0, 0, imageWidth, imageHeight);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void matchingX_ValueChanged(object sender, EventArgs e)
        {
            foreach (InspectorFovPanel inspectorFovPanel in inspectorFovPanelList)
            {
                if (inspectorFovPanel.Image != null)
                    inspectorFovPanel.UpdateRegion(new Rectangle(inspectorFovPanel.Image.Width - (int)matchingWidth.Value, (int)matchingY.Value, (int)matchingWidth.Value, (int)matchingHeight.Value));
            }
        }

        private void matchingY_ValueChanged(object sender, EventArgs e)
        {
            foreach (InspectorFovPanel inspectorFovPanel in inspectorFovPanelList)
            {
                if (inspectorFovPanel.Image != null)
                    inspectorFovPanel.UpdateRegion(new Rectangle(inspectorFovPanel.Image.Width - (int)matchingWidth.Value, (int)matchingY.Value, (int)matchingWidth.Value, (int)matchingHeight.Value));
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            int imageWidth = 0;
            int imageHeight = 0;
            foreach (InspectorInfo inspectorInfo in inspectorInfoList)
            {
                imageWidth += (int)inspectorInfo.Fov.Width;
                imageHeight = Math.Max(imageHeight, (int)inspectorInfo.Fov.Height);
            }

            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;

            int resizeImageWidth = (int)(imageWidth * resizeRatio);
            int resizeImageHeight = (int)(imageHeight * resizeRatio);

            if (resizeImageWidth == 0 || resizeImageHeight == 0)
                return;

            Image2D image = new Image2D(resizeImageWidth, resizeImageHeight, 1);

            int copiedPos = 0;
            foreach (InspectorFovPanel inspectorFovPanel in inspectorFovPanelList)
            {
                if (inspectorFovPanel.Image == null)
                    continue;

                int resizeFovX = (int)(inspectorFovPanel.InspectorInfo.Fov.X * resizeRatio);
                int resizeFovY = (int)(inspectorFovPanel.InspectorInfo.Fov.Y * resizeRatio);
                int resizeFovWidth = (int)(inspectorFovPanel.InspectorInfo.Fov.Width * resizeRatio);
                int resizeFovHeight = (int)(inspectorFovPanel.InspectorInfo.Fov.Height * resizeRatio);
                
                if (resizeFovWidth == 0 || resizeFovHeight == 0)
                    continue;
                
                Rectangle srcRect = new Rectangle(resizeFovX, resizeFovY, resizeFovWidth, resizeFovHeight);
                Image2D resizeImage = Image2D.FromBitmap(ImageHelper.Resize(inspectorFovPanel.Image.ToBitmap(), resizeRatio, resizeRatio));
                
                image.CopyFrom(resizeImage, srcRect, resizeImage.Width, new Point(copiedPos, 0));

                copiedPos += resizeFovWidth;
            }

            canvasPanel.UpdateImage(image.ToBitmap());
            canvasPanel.Invalidate(false);
        }

        private void buttonSaveImage_Click(object sender, EventArgs e)
        {
            foreach (InspectorFovPanel inspectorFovPanel in inspectorFovPanelList)
            {
                if (inspectorFovPanel.Image == null)
                    return;
            }

            int imageWidth = 0;
            int imageHeight = 0;
            foreach (InspectorInfo inspectorInfo in inspectorInfoList)
            {
                imageWidth += (int)inspectorInfo.Fov.Width;
                imageHeight = Math.Max(imageHeight, (int)inspectorInfo.Fov.Height);
            }
            
            if (imageWidth == 0 || imageHeight == 0)
                return;
            
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                Image2D image = new Image2D(imageWidth, imageHeight, 1);

                int copiedPos = 0;
                foreach (InspectorFovPanel inspectorFovPanel in inspectorFovPanelList)
                {
                    if (inspectorFovPanel.Image == null)
                        continue;
                
                    Rectangle srcRect = new Rectangle(
                        (int)inspectorFovPanel.InspectorInfo.Fov.X, (int)inspectorFovPanel.InspectorInfo.Fov.Y,
                        (int)inspectorFovPanel.InspectorInfo.Fov.Width, (int)inspectorFovPanel.InspectorInfo.Fov.Height);
                
                    image.CopyFrom(inspectorFovPanel.Image, srcRect, inspectorFovPanel.Image.Width, new Point(copiedPos, 0));

                    copiedPos += (int)inspectorFovPanel.InspectorInfo.Fov.Width;
                }
                
                image.SaveImage(dialog.FileName, ImageFormat.Bmp);
            }
        }
    }
}
