using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

using DynMvp.Vision;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Base;
using DynMvp.Vision.UI;

namespace DynMvp.Data.FilterForm
{
    public partial class BinarizationFilterParamControl : UserControl, IFilterParamControl
    {
        public FilterParamValueChangedDelegate ValueChanged = null;

        List<BinarizeFilter> binarizeFilterList = new List<BinarizeFilter>();

        bool onValueUpdate = false;

        public BinarizationFilterParamControl()
        {
            InitializeComponent();

            labelBinarizationType.Text = StringManager.GetString(this.GetType().FullName, labelBinarizationType);
            labelThresholdRange.Text = StringManager.GetString(this.GetType().FullName, labelThresholdRange);

            binarizationType.Items.Clear();
            string[] typeNames = Enum.GetNames(typeof(BinarizationType));
            foreach (string typeName in typeNames)
                binarizationType.Items.Add(StringManager.GetString(this.GetType().FullName, typeName));

            UpdateData(new BinarizeFilter());

            //change language
            labelBinarizationType.Text = StringManager.GetString(this.GetType().FullName, labelBinarizationType.Text);
        }

        public FilterType GetFilterType()
        {
            return FilterType.Binarize;
        }

        public void ClearSelectedFilter()
        {
            binarizeFilterList.Clear();
        }

        public void AddSelectedFilter(IFilter filter)
        {
            LogHelper.Debug(LoggerType.Operation, "BinarizationFilterParamControl - SetSelectedFilter");

            if (filter is BinarizeFilter)
            {
                binarizeFilterList.Add((BinarizeFilter)filter);
                UpdateData(binarizeFilterList[0]);
            }
        }

        public IFilter CreateFilter()
        {
            return new BinarizeFilter((BinarizationType)binarizationType.SelectedIndex, (int)thresholdLower.Value, (int)thresholdUpper.Value);
        }

        public IFilter CreateFilter(BinarizationType binarizationType, int thresholdLower, int thresholdUpper)
        {
            return new BinarizeFilter(binarizationType, thresholdLower, thresholdLower);
        }

        private void UpdateData(BinarizeFilter binarizeFilter)
        {
            LogHelper.Debug(LoggerType.Operation, "BinarizationFilterParamControl - UpdateData");

            onValueUpdate = true;

            binarizationType.SelectedIndex = (int)binarizeFilter.BinarizationType;
            thresholdLower.Value = binarizeFilter.ThresholdLower;
            thresholdUpper.Value = binarizeFilter.ThresholdUpper;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged()
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "BinarizationFilterParamControl - ParamControl_ValueChanged");

                if (ValueChanged != null)
                    ValueChanged();
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == binarizationType)
                valueName = "Binarization Type";
            else if (sender == thresholdLower)
                valueName = "Threshold Lower";
            else if (sender == thresholdUpper)
                valueName = "Threshold Upper";

            UpDownControl.ShowControl(StringManager.GetString(this.GetType().FullName,valueName), (Control)sender);
        }

        private void binarizationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinarizationFilterParamControl - binarizationType_SelectedIndexChanged");

            if (binarizeFilterList.Count == 0)
                return;

            foreach (BinarizeFilter binarizeFilter in binarizeFilterList)
            {
                binarizeFilter.BinarizationType = (BinarizationType)Enum.Parse(typeof(BinarizationType), binarizationType.Text);
            }

            ParamControl_ValueChanged();
        }

        private void thresholdLower_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinarizationFilterParamControl - thresholdLower_ValueChanged");

            if (binarizeFilterList.Count == 0)
                return;

            foreach (BinarizeFilter binarizeFilter in binarizeFilterList)
            {
                binarizeFilter.ThresholdLower = (int)thresholdLower.Value;
            }

            ParamControl_ValueChanged();
        }

        private void thresholdUpper_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinarizationFilterParamControl - thresholdUpper_ValueChanged");

            if (binarizeFilterList.Count == 0)
                return;

            foreach (BinarizeFilter binarizeFilter in binarizeFilterList)
            {
                binarizeFilter.ThresholdUpper = (int)thresholdUpper.Value;
            }

            ParamControl_ValueChanged();
        }

        private void thresholdLower_Validating(object sender, CancelEventArgs e)
        {
            if (binarizationType.SelectedIndex != (int)BinarizationType.DoubleThreshold)
                return;

            if (thresholdUpper.Value <= thresholdLower.Value)
            {
                e.Cancel = true;
                thresholdLower.Select(0, thresholdLower.Text.Length);

                // Set the ErrorProvider error with the text to display. 
                this.errorProvider.SetError(thresholdLower, StringManager.GetString(this.GetType().FullName, "Threshold lower value must be less than threshold upper value"));
            }
        }

        private void thresholdUpper_Validating(object sender, CancelEventArgs e)
        {
            if (thresholdUpper.Value <= thresholdLower.Value)
            {
                e.Cancel = true;
                thresholdUpper.Select(0, thresholdUpper.Text.Length);

                // Set the ErrorProvider error with the text to display. 
                this.errorProvider.SetError(thresholdUpper, StringManager.GetString(this.GetType().FullName, "Threshold upper value must be greater than threshold lower value"));
            }
        }

        public void SetValueChanged(FilterParamValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }

        public void SetTargetGroupImage(ImageD image)
        {

        }
    }
}
