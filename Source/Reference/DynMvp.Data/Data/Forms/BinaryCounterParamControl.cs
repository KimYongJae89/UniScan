using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

using DynMvp.Vision;
using DynMvp.Vision.Planbss;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Base;

namespace DynMvp.Data.Forms
{
    public partial class BinaryCounterParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;

        List<BinaryCounter> binaryCounterList = new List<BinaryCounter>();

        bool onValueUpdate = false;

        public BinaryCounterParamControl()
        {
            InitializeComponent();

            //change language
            radioCountWhitePixel.Text = StringManager.GetString(this.GetType().FullName,radioCountWhitePixel.Text);
            radioCountGreyPixel.Text = StringManager.GetString(this.GetType().FullName,radioCountGreyPixel.Text);
            radioCountBlackPixel.Text = StringManager.GetString(this.GetType().FullName,radioCountBlackPixel.Text);
            labelPixelRatio.Text = StringManager.GetString(this.GetType().FullName,labelPixelRatio.Text);
            tapeInspection.Text = StringManager.GetString(this.GetType().FullName,tapeInspection.Text);
        }

        public void ClearSelectedProbe()
        {
            binaryCounterList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - SetSelectedProbe");

            VisionProbe selectedProbe = (VisionProbe)probe;
            if (selectedProbe.InspAlgorithm.GetAlgorithmType() == BinaryCounter.TypeName)
            {
                binaryCounterList.Add((BinaryCounter)selectedProbe.InspAlgorithm);
                UpdateData();
            }
            else
                throw new InvalidOperationException();
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            if (binaryCounterList.Count == 0)
                return;

            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - UpdateData");

            onValueUpdate = true;

            BinaryCounterParam param = (BinaryCounterParam)binaryCounterList[0].Param;

            radioCountBlackPixel.Checked = param.CountBlackPixel;
            radioCountWhitePixel.Checked = param.CountWhitePixel;
            radioCountGreyPixel.Checked = param.CountGreyPixel;
            minPixelRatio.Value = param.MinPixelRatio;
            maxPixelRatio.Value = param.MaxPixelRatio;
            tapeInspection.Checked = param.TapeInspection;

            useGrid.Checked = param.GridParam.UseGrid;
            gridRowCount.Value = param.GridParam.RowCount;
            gridColumnCount.Value = param.GridParam.ColumnCount;
            gridCalcType.SelectedIndex = (int)param.GridParam.CalcType;
            gridScore.Value = param.GridParam.AcceptanceScore;

            onValueUpdate = false;

            EnableItems();
        }

        void EnableItems()
        {
            gridRowCount.Enabled = useGrid.Checked;
            gridColumnCount.Enabled = useGrid.Checked;
            gridCalcType.Enabled = useGrid.Checked;
            gridScore.Enabled = useGrid.Checked;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - ParamControl_ValueChanged");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, algorithm, newParam, true);
            }
        }

        private void minPixelRatio_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - minPixelRatio_ValueChanged");

            if (binaryCounterList.Count == 0)
                return;

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).MinPixelRatio = (int)minPixelRatio.Value;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            if (sender != minPixelRatio)
                return;

            UpDownControl.ShowControl(labelPixelRatio.Text, (Control)sender);
        }

        private void tapeInspection_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - boltInspection_CheckedChanged");

            if (binaryCounterList.Count == 0)
                return;

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).TapeInspection = tapeInspection.Checked;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void radioCountWhitePixel_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - countWhitePixel_CheckedChanged");

            if (binaryCounterList.Count == 0)
                return;

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).CountWhitePixel = radioCountWhitePixel.Checked;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void radioCountBlackPixel_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - countBlackPixel_CheckedChanged");

            if (binaryCounterList.Count == 0)
                return;

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).CountBlackPixel = radioCountBlackPixel.Checked;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void radioCountGreyPixel_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - radioCountGreyPixel_CheckedChanged");

            if (binaryCounterList.Count == 0)
                return;

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).CountGreyPixel = radioCountGreyPixel.Checked;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        private void maxPixelRatio_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - maxPixelRatio_ValueChanged");

            if (binaryCounterList.Count == 0)
                return;

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).MaxPixelRatio = (int)maxPixelRatio.Value;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void useGrid_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - useGrid_CheckedChanged");

            if (binaryCounterList.Count == 0)
                return;

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).GridParam.UseGrid = useGrid.Checked;

                ParamControl_ValueChanged(ValueChangedType.ImageProcessing, binaryCounter, newParam);
            }

            EnableItems();
        }

        private void gridRowCount_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - girdRowCount_ValueChanged");

            if (binaryCounterList.Count == 0)
                return;

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).GridParam.RowCount = (int)gridRowCount.Value;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void gridColumnCount_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - gridColumnCount_ValueChanged");

            if (binaryCounterList.Count == 0)
                return;

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).GridParam.ColumnCount = (int)gridColumnCount.Value;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void gridCalcType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - gridAcceptance_ValueChanged");

            if (binaryCounterList.Count == 0)
                return;

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).GridParam.CalcType = (SegmentCalcType)gridCalcType.SelectedIndex;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void gridScore_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - gridScore_ValueChanged");

            if (binaryCounterList.Count == 0)
                return;

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).GridParam.AcceptanceScore = (int)gridScore.Value;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        public string GetTypeName()
        {
            return BinaryCounter.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }

        public void SetTargetGroupImage(ImageD targetGroupImage)
        {

        }
    }
}
