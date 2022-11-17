using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DynMvp.Vision;
using DynMvp.UI.Touch;
using DynMvp.Base;
using DynMvp.Vision.UI;

namespace DynMvp.Data.FilterForm
{
    public partial class NoParamFilterParamControl : UserControl, IFilterParamControl
    {
        public FilterParamValueChangedDelegate ValueChanged = null;

        List<IFilter> filterList = new List<IFilter>();

        public NoParamFilterParamControl()
        {
            InitializeComponent();

            //change language
            labelNoParameter.Text = StringManager.GetString(this.GetType().FullName, labelNoParameter);
        }

        public FilterType GetFilterType()
        {
            return FilterType.Morphology;
        }

        public void ClearSelectedFilter()
        {
            filterList.Clear();
        }

        public void AddSelectedFilter(IFilter filter)
        {
            LogHelper.Debug(LoggerType.Operation, "NoParamFilterParamControl - SetSelectedFilter");

            this.filterList.Add(filter);
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {

        }

        public IFilter CreateFilter()
        {
            return null;
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
