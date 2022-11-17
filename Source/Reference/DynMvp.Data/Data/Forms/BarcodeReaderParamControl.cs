using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

using DynMvp.Vision;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Base;
using DynMvp.Data.Forms;

namespace DynMvp.Data.Forms
{
    public partial class BarcodeReaderParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;

        BarcodeReader barcodeReader = null;

        VisionProbe selectedProbe = null;

        bool onValueUpdate = false;

        public BarcodeReaderParamControl()
        {
            InitializeComponent();

            desiredString.Text = StringManager.GetString(this.GetType().FullName,desiredString);
            desiredNum.Text = StringManager.GetString(this.GetType().FullName,desiredNum);
            searchRangeWidth.Text = StringManager.GetString(this.GetType().FullName,searchRangeWidth);
            searchRangeHeight.Text = StringManager.GetString(this.GetType().FullName,searchRangeHeight);
            fiducialProbe.Text = StringManager.GetString(this.GetType().FullName,fiducialProbe);
            addBarcodeTypeButton.Text = StringManager.GetString(this.GetType().FullName,addBarcodeTypeButton);
            deleteBarcodeTypeButton.Text = StringManager.GetString(this.GetType().FullName,deleteBarcodeTypeButton);
            labelDesiredString.Text = StringManager.GetString(this.GetType().FullName,labelDesiredString);
            labelDesiredNum.Text = StringManager.GetString(this.GetType().FullName,labelDesiredNum);
            labelBarcodeType.Text = StringManager.GetString(this.GetType().FullName,labelBarcodeType);
            buttonStringInsert.Text = StringManager.GetString(this.GetType().FullName,buttonStringInsert);
            labelBarcodeType.Text = StringManager.GetString(this.GetType().FullName,labelBarcodeType);
            labelDesiredNum.Text = StringManager.GetString(this.GetType().FullName,labelDesiredNum);
            labelDesiredString.Text = StringManager.GetString(this.GetType().FullName,labelDesiredString);
            buttonStringInsert.Text = StringManager.GetString(this.GetType().FullName,buttonStringInsert);
            labelSearchRange.Text = StringManager.GetString(this.GetType().FullName,labelSearchRange);
            offsetRange.Text = StringManager.GetString(this.GetType().FullName,offsetRange);

            labelW.Text = StringManager.GetString(this.GetType().FullName,labelW);
            labelH.Text = StringManager.GetString(this.GetType().FullName,labelH);
            labelRangeLeft.Text = StringManager.GetString(this.GetType().FullName,labelRangeLeft);
            labelRangeRight.Text = StringManager.GetString(this.GetType().FullName,labelRangeRight);
            labelRangeBottom.Text = StringManager.GetString(this.GetType().FullName,labelRangeBottom);
            labelRangeTop.Text = StringManager.GetString(this.GetType().FullName,labelRangeTop);

            AlgorithmStrategy barcodeAlgorithmStrategy = AlgorithmBuilder.GetStrategy(BarcodeReader.TypeName);
            if (barcodeAlgorithmStrategy != null)
            {
                comboBoxBarcodeType.Items.Add(BarcodeType.DataMatrix.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.Codabar.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.Code128.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.Code39.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.Code93.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.Interleaved2of5.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.Pharmacode.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.PLANET.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.POSTNET.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.FourStatePostal.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.QRCode.ToString());

                if (barcodeAlgorithmStrategy.LibraryType == ImagingLibrary.CognexVisionPro)
                {
                    comboBoxBarcodeType.Items.Add(BarcodeType.UPCEAN.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.EANUCCComposite.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.PDF417.ToString());
                }
                else
                {
                    comboBoxBarcodeType.Items.Add(BarcodeType.BC412.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.EAN8.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.EAN13.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.EAN14.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.UPC_A.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.UPC_E.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.GS1_128.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.GS1Databar.ToString());
                }
                }
            }

        public void SetupBarcodeType(List<BarcodeType> barcodeTypeList)
        {
            comboBoxBarcodeType.Items.Clear();
            foreach (BarcodeType barcodeType in barcodeTypeList)
            {
                comboBoxBarcodeType.Items.Add(barcodeType.ToString());
            }
        }

        public void ClearSelectedProbe()
        {
            barcodeReader = null;
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - SetSelectedProbe");

            VisionProbe visionProbe = (VisionProbe)probe;
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == BarcodeReader.TypeName)
            {
                selectedProbe = visionProbe;
                barcodeReader = (BarcodeReader)selectedProbe.InspAlgorithm;
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
            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - UpdateData");

            onValueUpdate = true;

            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)barcodeReader.Param;

            desiredString.Text = barcodeReaderParam.DesiredString;
            desiredNum.Value = (int)barcodeReaderParam.DesiredNum;
            searchRangeWidth.Value = (int)barcodeReaderParam.SearchRangeWidth;
            searchRangeHeight.Value = (int)barcodeReaderParam.SearchRangeHeight;
            fiducialProbe.Checked = selectedProbe.ActAsFiducialProbe;
            offsetRange.Checked = barcodeReaderParam.OffsetRange;
            rangeThresholdLeft.Value = (int)barcodeReaderParam.RangeThresholdLeft;
            rangeThresholdBottom.Value = (int)barcodeReaderParam.RangeThresholdBottom;
            rangeThresholdTop.Value = (int)barcodeReaderParam.RangeThresholdTop;
            rangeThresholdRight.Value = (int)barcodeReaderParam.RangeThresholdRight;
            timeoutTime.Value = barcodeReaderParam.TimeoutTime;
            RefreshBarcodeTypeList();

            if (fiducialProbe.Checked)
            {
                selectedProbe.Target.SelectFiducialProbe(selectedProbe.Id);

                offsetRange.Enabled = false;
                EnableFiducialProbeItems(true);

                barcodeReaderParam.DesiredNum = (int)desiredNum.Value;
            }
            else
            {
                selectedProbe.Target.DeselectFiducialProbe(selectedProbe.Id);

                if (offsetRange.Checked == true)
                {
                    fiducialProbe.Enabled = false;
                }
                else
                {
                    offsetRange.Enabled = true;
                    fiducialProbe.Enabled = true;
                }
                EnableFiducialProbeItems(offsetRange.Checked);
                EnableOffSetRangeItems(offsetRange.Checked);
            }
            
            useAreaFilter.Checked = barcodeReaderParam.UseAreaFilter;
            minArea.Value = barcodeReaderParam.MinArea;
            maxArea.Value = barcodeReaderParam.MaxArea;

            labelMinArea.Enabled = useAreaFilter.Checked;
            labelMaxArea.Enabled = useAreaFilter.Checked;
            minArea.Enabled = useAreaFilter.Checked;
            maxArea.Enabled = useAreaFilter.Checked;

            closeNum.Value = barcodeReaderParam.CloseNum;

            useBlobing.Checked = barcodeReaderParam.UseBlobing;
            groupBoxBlobing.Enabled = barcodeReaderParam.UseBlobing;

            RefreshThresholdList();

            onValueUpdate = false;
        }

        private void RefreshBarcodeTypeList()
        {
            barcodeTypeList.Items.Clear();

            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)barcodeReader.Param;
            foreach (BarcodeType barcodeType in barcodeReaderParam.BarcodeTypeList)
            {
                barcodeTypeList.Items.Add(barcodeType.ToString());
            }
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - ParamControl_ValueChanged");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, algorithm, newParam, true);
            }
        }

        private void desiredString_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - desiredString_TextChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).DesiredString = desiredString.Text;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        private void desiredNum_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - desiredNum_ValueChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).DesiredNum = (int)desiredNum.Value;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        private void fiducialProbe_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - fiducialProbe_CheckedChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)newParam;

            if (fiducialProbe.Checked)
            {
                selectedProbe.Target.SelectFiducialProbe(selectedProbe.Id);

                offsetRange.Enabled = false;
                barcodeReaderParam.DesiredNum = (int)desiredNum.Value;
            }
            else
            {
                selectedProbe.Target.DeselectFiducialProbe(selectedProbe.Id);

                offsetRange.Enabled = true;
                desiredNum.Enabled = true;
            }

            UpdateData();

            EnableFiducialProbeItems(fiducialProbe.Checked);
        }

        private void searchRangeWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - searchRangeWidth_ValueChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).SearchRangeWidth = (int)searchRangeWidth.Value;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        private void searchRangeHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - searchRangeWidth_ValueChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).SearchRangeHeight = (int)searchRangeHeight.Value;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        private void addBarcodeTypeButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - addBarcodeTypeButton_Click");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)newParam;

            barcodeReaderParam.BarcodeTypeList.Remove((BarcodeType)Enum.Parse(typeof(BarcodeType), (string)comboBoxBarcodeType.SelectedItem));
            barcodeReaderParam.BarcodeTypeList.Add((BarcodeType)Enum.Parse(typeof(BarcodeType), (string)comboBoxBarcodeType.SelectedItem));

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);

            RefreshBarcodeTypeList();
        }

        private void deleteBarcodeTypeButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - deleteBarcodeTypeButton_Click");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)newParam;

            barcodeReaderParam.BarcodeTypeList.Remove((BarcodeType)Enum.Parse(typeof(BarcodeType), (string)barcodeTypeList.SelectedItem));

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);

            RefreshBarcodeTypeList();
        }

        private void useRange_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - useRange_CheckedChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)newParam;

            if (offsetRange.Checked)
            {
                barcodeReaderParam.OffsetRange = true;
                fiducialProbe.Enabled = false;

                barcodeReaderParam.DesiredNum = (int)desiredNum.Value;
            }
            else
            {
                barcodeReaderParam.OffsetRange = false;
                fiducialProbe.Enabled = true;

                desiredNum.Enabled = true;
            }

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);

            EnableFiducialProbeItems(offsetRange.Checked);
            EnableOffSetRangeItems(offsetRange.Checked);

            UpdateData();
        }

        private void EnableOffSetRangeItems(bool enable)
        {
            onValueUpdate = true;

            if (enable)
                desiredNum.Value = 1;
            else
            {
                rangeThresholdLeft.Value = 0;
                rangeThresholdRight.Value = 0;
                rangeThresholdBottom.Value = 0;
                rangeThresholdTop.Value = 0;

                BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)barcodeReader.Param;

                barcodeReaderParam.RangeThresholdLeft = 0;
                barcodeReaderParam.RangeThresholdRight = 0;
                barcodeReaderParam.RangeThresholdBottom = 0;
                barcodeReaderParam.RangeThresholdTop = 0;
            }

            desiredNum.Enabled = !enable;

            labelRangeLeft.Enabled = enable;
            labelRangeRight.Enabled = enable;
            labelRangeTop.Enabled = enable;
            labelRangeBottom.Enabled = enable;
            
            rangeThresholdLeft.Enabled = enable;
            rangeThresholdRight.Enabled = enable;
            rangeThresholdBottom.Enabled = enable;
            rangeThresholdTop.Enabled = enable;

            onValueUpdate = false;
        }

        private void EnableFiducialProbeItems(bool enable)
        {
            onValueUpdate = true;

            if (enable)
            {
                desiredNum.Value = 1;
            }
            else
            {
                searchRangeWidth.Value = 0;
                searchRangeHeight.Value = 0;
            }

            onValueUpdate = false;

            desiredNum.Enabled = !enable;
            labelSearchRange.Enabled = enable;
            labelH.Enabled = enable;
            labelW.Enabled = enable;
            searchRangeWidth.Enabled = enable;
            searchRangeHeight.Enabled = enable;
        }

        private void rangeThresholdLeft_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - rangeThresholdLeft_ValueChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).RangeThresholdLeft = (int)rangeThresholdLeft.Value;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        private void rangeThresholdRight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - rangeThresholdRight_ValueChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).RangeThresholdRight = (int)rangeThresholdRight.Value;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        private void rangeThresholdBottom_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - rangeThresholdBottom_ValueChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).RangeThresholdBottom = (int)rangeThresholdBottom.Value;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        private void rangeThresholdTop_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - rangeThresholdTop_ValueChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).RangeThresholdTop = (int)rangeThresholdTop.Value;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        private void closeNum_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - comboBoxAutoThresholdType_SelectedIndexChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).CloseNum = (int)closeNum.Value;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        private void minArea_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - minArea_ValueChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).MinArea = (int)minArea.Value;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        private void maxArea_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - maxArea_ValueChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).MaxArea = (int)maxArea.Value;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        private void useAreaFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - comboBoxAutoThresholdType_SelectedIndexChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).UseAreaFilter = useAreaFilter.Checked;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);

            labelMinArea.Enabled = useAreaFilter.Checked;
            labelMaxArea.Enabled = useAreaFilter.Checked;
            minArea.Enabled = useAreaFilter.Checked;
            maxArea.Enabled = useAreaFilter.Checked;
        }

        private void buttonStringInsert_Click(object sender, EventArgs e)
        {
            if (barcodeReader == null)
                return;

            desiredString.Text = barcodeReader.LastReadString;
        }

        private void thresholdPercent_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - thresholdPercent_ValueChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - selectedProbe instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).ThresholdPercent = (int)thresholdPercent.Value;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        private void addThresholdButton_Click(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - addThresholdButton_Click");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - BarcodeReader instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();

            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)newParam;
            int index = barcodeReaderParam.ThresholdPercentList.IndexOf((int)thresholdPercent.Value);
            if (index == -1)
                barcodeReaderParam.ThresholdPercentList.Add((int)thresholdPercent.Value);

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);

            RefreshThresholdList();
        }

        private void deleteThresholdButton_Click(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - addThresholdButton_Click");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - BarcodeReader instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).ThresholdPercentList.Remove((int)thresholdList.SelectedItem);

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);

            RefreshThresholdList();
        }

        private void RefreshThresholdList()
        {
            thresholdList.Items.Clear();

            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)barcodeReader.Param;

            foreach (int ThresholdPercent in barcodeReaderParam.ThresholdPercentList)
            {
                thresholdList.Items.Add(ThresholdPercent);
            }
        }

        private void useBlobing_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - useBlobing_CheckedChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - BarcodeReader instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).UseBlobing = useBlobing.Checked;

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);

            groupBoxBlobing.Enabled = useBlobing.Checked;
        }

        private void timeoutTime_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - useBlobing_CheckedChanged");

            if (barcodeReader == null)
            {
                LogHelper.Error(LoggerType.Error, "BarcodeReaderParamControl - BarcodeReader instance is null.");
                return;
            }

            AlgorithmParam newParam = barcodeReader.Param.Clone();
            ((BarcodeReaderParam)newParam).TimeoutTime = Convert.ToInt32(timeoutTime.Value);

            ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return BarcodeReader.TypeName;
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
