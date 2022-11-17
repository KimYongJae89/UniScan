using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DynMvp.Vision;
using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Data.Forms
{
    public partial class ColorCheckerParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;

        VisionProbe selectedProbe;
        List<ColorChecker> colorCheckerList = new List<ColorChecker>();

        Image2D targetGroupImage = null;
        public Image2D TargetGroupImage
        {
            set {
                LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - Set Target Group Image");
                targetGroupImage = value;
            }
        }

        //bool selectColorMode = false;
        bool onValueUpdate = false;

        public ColorCheckerParamControl()
        {
            InitializeComponent();

            colorSpace.DataSource = Enum.GetNames(typeof(ColorSpace));

            columnR.MaskInput = "nnn";
            columnG.MaskInput = "nnn";
            columnB.MaskInput = "nnn";
        }

        private void AddColor(Color color)
        {
            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).ColorValueList.Add(new ColorValue(color));

                ParamControl_ValueChanged(ValueChangedType.None, colorChecker, newParam);
            }

            int rowIndex = colorGrid.Rows.Add("", color.R.ToString(), color.G.ToString(), color.B.ToString());
            colorGrid.Rows[rowIndex].Cells[0].Style.BackColor = color;
            colorGrid.Rows[rowIndex].Cells[0].Style.SelectionBackColor = color;

            for (int i = 1; i < 4; i++)
            {
                colorGrid.Rows[rowIndex].Cells[i].Style.BackColor = Color.White;
                colorGrid.Rows[rowIndex].Cells[i].Style.SelectionBackColor = Color.DeepSkyBlue;
            }
        }

        private void addColorButton_Click(object sender, EventArgs e)
        {
            RectangleF imageRegion = new RectangleF(0, 0, targetGroupImage.Width, targetGroupImage.Height);
            RectangleF probeRegion = selectedProbe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, imageRegion))
            {
                RotatedRect probeRotatedRect = selectedProbe.BaseRegion;

                ImageD clipImage = targetGroupImage.ClipImage(probeRotatedRect);

                AlgoImage algoImage = ImageBuilder.Build(ColorChecker.TypeName, clipImage, ImageType.Color);

                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
                Color color = imageProcessing.GetColorAverage(algoImage);

                AddColor(color);
            }
            else
            {
                MessageBox.Show(StringManager.GetString(this.GetType().FullName, "Probe region is invalid."));
            }
        }

        void IAlgorithmParamControl.ClearSelectedProbe()
        {
            selectedProbe = null;
            colorCheckerList.Clear();
        }

        void IAlgorithmParamControl.AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - SetSelectedProbe");

            selectedProbe = (VisionProbe)probe;
            if (selectedProbe.InspAlgorithm.GetAlgorithmType() == ColorChecker.TypeName)
            {
                colorCheckerList.Add((ColorChecker)selectedProbe.InspAlgorithm);
                UpdateData();
            }
            else
                throw new InvalidOperationException();
        }

        private void UpdateData()
        {
            if (colorCheckerList.Count == 0)
                return;

            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - UpdateData");

            onValueUpdate = true;

            ColorCheckerParam param = (ColorCheckerParam)colorCheckerList[0].Param;

            acceptanceScore.Value = param.AcceptanceScore;
            scoreWeightValue1.Value = (Decimal)param.ScoreWeightValue1;
            scoreWeightValue2.Value = (Decimal)param.ScoreWeightValue2;
            scoreWeightValue3.Value = (Decimal)param.ScoreWeightValue3;
            colorSpace.SelectedIndex = (int)param.ColorSpace;

            colorGrid.Rows.Clear();
            foreach (ColorValue colorValue in param.ColorValueList)
            {
                int rowIndex = colorGrid.Rows.Add("", colorValue.Value1.ToString(), colorValue.Value2.ToString(), colorValue.Value3.ToString());
                colorGrid.Rows[rowIndex].Cells[0].Style.BackColor = colorValue.ToColor();
                colorGrid.Rows[rowIndex].Cells[0].Style.SelectionBackColor = colorValue.ToColor();

                for (int i = 1; i < 4; i++)
                {
                    colorGrid.Rows[rowIndex].Cells[i].Style.BackColor = Color.White;
                    colorGrid.Rows[rowIndex].Cells[i].Style.SelectionBackColor = Color.DeepSkyBlue;
                }
            }

            useSegmentation.Checked = param.UseSegmentation;
            segmentCalcType.SelectedIndex = (int)param.SegmentCalcType;
            segmentScore.Value = param.SegmentScore;

            useGrid.Checked = param.GridParam.UseGrid;
            gridRowCount.Value = param.GridParam.RowCount;
            gridColumnCount.Value = param.GridParam.ColumnCount;
            gridCalcType.SelectedIndex = (int)param.GridParam.CalcType;
            gridScore.Value = param.GridParam.AcceptanceScore;

            EnableItems();

            onValueUpdate = false;
        }

        void EnableItems()
        {
            gridRowCount.Enabled = useGrid.Checked;
            gridColumnCount.Enabled = useGrid.Checked;
            gridCalcType.Enabled = useGrid.Checked;
            gridScore.Enabled = useGrid.Checked;

            segmentCalcType.Enabled = useSegmentation.Checked;
            segmentScore.Enabled = useSegmentation.Checked;
        }

        void IAlgorithmParamControl.UpdateProbeImage()
        {

        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - ParamControl_ValueChanged");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, algorithm, newParam, true);
            }
        }

        private void deleteColorButton_Click(object sender, EventArgs e)
        {
            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();

                foreach (DataGridViewRow row in colorGrid.SelectedRows)
                {
                    ((ColorCheckerParam)newParam).ColorValueList.Remove(new ColorValue(row.Cells[0].Style.BackColor));
                    colorGrid.Rows.RemoveAt(row.Index);
                }

                ParamControl_ValueChanged(ValueChangedType.None, colorChecker, newParam);
            }
        }

        private void colorSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - colorSpace_SelectedIndexChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).ColorSpace = (ColorSpace)colorSpace.SelectedIndex;

                ParamControl_ValueChanged(ValueChangedType.None, colorChecker, newParam);
            }
        }

        private void scoreWeightValue1_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - scoreWeightValue1_ValueChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).ScoreWeightValue1 = (float)scoreWeightValue1.Value;

                ParamControl_ValueChanged(ValueChangedType.None, colorChecker, newParam);
            }
        }

        private void scoreWeightValue2_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - scoreWeightValue2_ValueChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).ScoreWeightValue2 = (float)scoreWeightValue2.Value;

                ParamControl_ValueChanged(ValueChangedType.None, colorChecker, newParam);
            }
        }

        private void scoreWeightValue3_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - scoreWeightValue3_ValueChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).ScoreWeightValue3 = (float)scoreWeightValue3.Value;

                ParamControl_ValueChanged(ValueChangedType.None, colorChecker, newParam);
            }
        }

        private void acceptanceScore_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - acceptanceScore_ValueChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).AcceptanceScore = (int)acceptanceScore.Value;

                ParamControl_ValueChanged(ValueChangedType.ImageProcessing, colorChecker, newParam);
            }
        }

        private void colorGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (onValueUpdate == true || colorCheckerList.Count == 0)
                return;

            float value = Convert.ToSingle(colorGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            if (value > 255)
            {
                onValueUpdate = true;
                colorGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 255;
                onValueUpdate = false;
                value = 255;
            }

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                ColorCheckerParam newColorCheckerParam = (ColorCheckerParam)colorChecker.Param.Clone();

                ColorValue colorValue = newColorCheckerParam.ColorValueList[e.RowIndex];
                colorValue.SetValue(e.ColumnIndex - 1, value);

                newColorCheckerParam.ColorValueList[e.RowIndex] = colorValue;
                colorGrid.Rows[e.RowIndex].Cells[0].Style.BackColor = colorValue.ToColor();

                ParamControl_ValueChanged(ValueChangedType.None, colorChecker, newColorCheckerParam);
            }
        }

        private void segmentScore_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - segmentScore_ValueChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).SegmentScore = (int)segmentScore.Value;

                ParamControl_ValueChanged(ValueChangedType.ImageProcessing, colorChecker, newParam);
            }
        }

        private void segmentCalcType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - segmentCalcType_SelectedIndexChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).SegmentCalcType = (SegmentCalcType)segmentCalcType.SelectedIndex;

                ParamControl_ValueChanged(ValueChangedType.ImageProcessing, colorChecker, newParam);
            }
        }

        private void useSegmentation_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - useSegmentation_CheckedChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).UseSegmentation = useSegmentation.Checked;

                ParamControl_ValueChanged(ValueChangedType.ImageProcessing, colorChecker, newParam);
            }

            EnableItems();
        }

        private void useGrid_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - useGrid_CheckedChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).GridParam.UseGrid = useGrid.Checked;

                ParamControl_ValueChanged(ValueChangedType.ImageProcessing, colorChecker, newParam);
            }

            EnableItems();
        }

        private void gridCalcType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - gridAcceptance_ValueChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).GridParam.CalcType = (SegmentCalcType)gridCalcType.SelectedIndex;

                ParamControl_ValueChanged(ValueChangedType.None, colorChecker, newParam);
            }
        }

        private void gridScore_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - gridScore_ValueChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).GridParam.AcceptanceScore = (int)gridScore.Value;

                ParamControl_ValueChanged(ValueChangedType.None, colorChecker, newParam);
            }
        }

        private void gridColumnCount_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - gridColumnCount_ValueChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).GridParam.ColumnCount = (int)gridColumnCount.Value;

                ParamControl_ValueChanged(ValueChangedType.None, colorChecker, newParam);
            }
        }

        private void girdRowCount_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - girdRowCount_ValueChanged");

            if (colorCheckerList.Count == 0)
                return;

            foreach (ColorChecker colorChecker in colorCheckerList)
            {
                AlgorithmParam newParam = colorChecker.Param.Clone();
                ((ColorCheckerParam)newParam).GridParam.RowCount = (int)gridRowCount.Value;

                ParamControl_ValueChanged(ValueChangedType.None, colorChecker, newParam);
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
            if (selectColorButton.Checked)
            {
                float[] values = targetGroupImage.GetRangeValue(clickPos);

                AddColor(Color.FromArgb((int)values[2], (int)values[1], (int)values[0]));
                selectColorButton.Checked = false;

                processingCancelled = true;
            }
        }

        public string GetTypeName()
        {
            return ColorChecker.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }

        public void SetTargetGroupImage(ImageD targetGroupImage)
        {
            if (targetGroupImage is Image2D)
                this.targetGroupImage = (Image2D)targetGroupImage;
        }
    }
}
