using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

using DynMvp.Vision;
using DynMvp.UI;
using DynMvp.Base;
using DynMvp.Vision.UI;

namespace DynMvp.Data.FilterForm
{
    public partial class SubtractionFilterParamControl : UserControl, IFilterParamControl
    {
        public FilterParamValueChangedDelegate ValueChanged = null;

        List<SubtractionFilter> subtractionFilterList = new List<SubtractionFilter>();
        Image2D targetGroupImage;
        bool onValueUpdate = false;

        public SubtractionFilterParamControl()
        {
            InitializeComponent();

            subtractionType.Items.Clear();
            string[] typeNames = Enum.GetNames(typeof(SubtractionType));
            foreach (string typeName in typeNames)
                subtractionType.Items.Add(StringManager.GetString(this.GetType().FullName, typeName));

            //UpdateData();

            //change language
            labelNegativeValueHandle.Text = StringManager.GetString(this.GetType().FullName, labelNegativeValueHandle);
        }

        public FilterType GetFilterType()
        {
            return FilterType.Subtraction;
        }

        public void ClearSelectedFilter()
        {

        }

        public void AddSelectedFilter(IFilter filter)
        {
            LogHelper.Debug(LoggerType.Operation, "SubtractionFilterParamControl - SetSelectedFilter");

            if (filter is SubtractionFilter)
            {
                subtractionFilterList.Add((SubtractionFilter)filter);
                UpdateData(subtractionFilterList[0]);
            }
        }

        public IFilter CreateFilter()
        {
            return new SubtractionFilter(SubtractionType.SetZero);
        }

        private void UpdateData(SubtractionFilter subtractionFilter)
        {
            LogHelper.Debug(LoggerType.Operation, "SubtractionFilterParamControl - UpdateData");
            if (subtractionFilter == null)
            {
                return;
            }

            onValueUpdate = true;

            subtractionType.SelectedIndex = (int)subtractionFilter.SubtractionType;
            checkBoxInvert.Checked = subtractionFilter.UseInvert;

            if (subtractionFilter.IsTrained)
            {
                pictureBoxTrain.Image = subtractionFilter.SubtractionImage.ToImageD().ToBitmap();
            }
            else
            {
                pictureBoxTrain.Image = null;
            }
            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged()
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "SubtractionFilterParamControl - ParamControl_ValueChanged");

                if (ValueChanged != null)
                    ValueChanged();
            }
        }
        
        private void subtractionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "SubtractionFilterParamControl - subtractionType_SelectedIndexChanged");

            if (subtractionFilterList.Count == 0)
                return;

            foreach(SubtractionFilter subtractionFilter in subtractionFilterList)
            {
                subtractionFilter.SubtractionType = (SubtractionType)Enum.Parse(typeof(SubtractionType), subtractionType.Text);

                ParamControl_ValueChanged();
            }
        }

        private void checkBoxInvert_CheckedChanged(object sender, EventArgs e)
        {
            if (subtractionFilterList.Count == 0)
                return;

            foreach (SubtractionFilter subtractionFilter in subtractionFilterList)
            {
                subtractionFilter.UseInvert = (sender as CheckBox).Checked;
                ParamControl_ValueChanged();
            }            
        }

        private void buttonTrain_Click(object sender, EventArgs e)
        {
            if (subtractionFilterList.Count != 1)
                return;

            if (targetGroupImage != null)
            {
                VisionProbe selecetedVisionProbe = null; // visionParamControl.SelectedProbe;

                RotatedRect filterRegion = selecetedVisionProbe.BaseRegion;
                RectangleF clipRegion = filterRegion.GetBoundRect();
                //if (useTargetCoordinate)
                //    clipRegion.Offset(-Target.Region.X, -Target.Region.Y);
                Rectangle clipRect = Rectangle.Round(clipRegion);

                ImageD clipImage = targetGroupImage.ClipImage(clipRect);
                //AlgoImage algoImage = ImageBuilder.Build(subtractionFilter.GetFilterType().ToString(), clipImage, ImageType.Grey, ImageBandType.Luminance);
                AlgoImage algoImage = ImageBuilder.Build(FilterType.Subtraction.ToString(), clipImage, ImageType.Grey, ImageBandType.Luminance);


                subtractionFilterList[0].Filter(algoImage);

                subtractionFilterList[0].SetMasterImage(algoImage);

                ParamControl_ValueChanged();
            }
        }

        private void SubtractionFilterParamControl_VisibleChanged(object sender, EventArgs e)
        {
            if (subtractionFilterList.Count == 0)
                return;

            if (this.Visible)
            {
                UpdateData(subtractionFilterList[0]);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (subtractionFilterList.Count == 0)
                return;

            foreach (SubtractionFilter subtractionFilter in subtractionFilterList)
            {
                ParamControl_ValueChanged();
            }

            UpdateData(subtractionFilterList[0]);
        }

        public void SetValueChanged(FilterParamValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }

        public void SetTargetGroupImage(ImageD image)
        {
            if (image is Image2D)
                targetGroupImage = (Image2D)image;
        }
    }
}
