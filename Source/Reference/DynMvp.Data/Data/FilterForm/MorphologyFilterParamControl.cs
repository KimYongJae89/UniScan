using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DynMvp.Vision;
using DynMvp.UI.Touch;
using DynMvp.Base;
using DynMvp.Vision.UI;

namespace DynMvp.Data.FilterForm
{
    public partial class MorphologyFilterParamControl : UserControl, IFilterParamControl
    {
        public FilterParamValueChangedDelegate ValueChanged = null;

        List<MorphologyFilter> morphologyFilterList = new List<MorphologyFilter>();

        bool onValueUpdate = false;

        public MorphologyFilterParamControl()
        {
            InitializeComponent();

            labelMorphologyType.Text = StringManager.GetString(this.GetType().FullName,labelMorphologyType);
            labelNumIteration.Text = StringManager.GetString(this.GetType().FullName,labelNumIteration);


            morphologyType.Items.Clear();
            string[] typeNames = Enum.GetNames(typeof(MorphologyType));
            foreach (string typeName in typeNames)
                morphologyType.Items.Add(StringManager.GetString(this.GetType().FullName, typeName));

            UpdateData(new MorphologyFilter());

            //change language
            labelMorphologyType.Text = StringManager.GetString(this.GetType().FullName,labelMorphologyType);
        }

        public FilterType GetFilterType()
        {
            return FilterType.Morphology;
        }

        public void ClearSelectedFilter()
        {
            morphologyFilterList.Clear();
        }

        public void AddSelectedFilter(IFilter filter)
        {
            LogHelper.Debug(LoggerType.Operation, "MorphologyFilterParamControl - SetSelectedFilter");

            if (filter is MorphologyFilter)
            {
                morphologyFilterList.Add((MorphologyFilter)filter);
                UpdateData(morphologyFilterList[0]);
            }
        }

        public IFilter CreateFilter()
        {
            return new MorphologyFilter((MorphologyType)morphologyType.SelectedIndex, (int)numIteration.Value);
        }

        private void UpdateData(MorphologyFilter morphologyFilter)
        {
            LogHelper.Debug(LoggerType.Operation, "MorphologyFilterParamControl - UpdateData");

            onValueUpdate = true;

            morphologyType.SelectedIndex = (int)morphologyFilter.MorphologyType;
            numIteration.Value = morphologyFilter.NumIteration;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged()
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "MorphologyFilterParamControl - ParamControl_ValueChanged");

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
            if (sender == morphologyType)
                valueName = "Morphology Type";
            else if (sender == numIteration)
                valueName = "Num Iteration";

            UpDownControl.ShowControl(StringManager.GetString(this.GetType().FullName,valueName), (Control)sender);
        }

        private void morphologyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MorphologyFilterParamControl - morphologyType_SelectedIndexChanged");

            if (morphologyFilterList.Count == 0)
                return;

            foreach (MorphologyFilter morphologyFilter in morphologyFilterList)
            {
                morphologyFilter.MorphologyType = (MorphologyType)morphologyType.SelectedIndex;

                ParamControl_ValueChanged();
            }
        }

        private void numIteration_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MorphologyFilterParamControl - numIteration_ValueChanged");

            if (morphologyFilterList.Count == 0)
                return;

            foreach (MorphologyFilter morphologyFilter in morphologyFilterList)
            {
                morphologyFilter.NumIteration = (int)numIteration.Value;

                ParamControl_ValueChanged();
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
