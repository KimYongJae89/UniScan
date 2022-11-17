using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.Vision.UI;
using DynMvp.Vision;

namespace DynMvp.Data.FilterForm
{
    public partial class MaskFilterParamControl : UserControl, IFilterParamControl
    {
        public FilterParamValueChangedDelegate ValueChanged;

        Image2D targetGroupImage;

        List<MaskFilter> maskFilterList = new List<MaskFilter>();

        bool onValueUpdate = false;

        public MaskFilterParamControl()
        {
            InitializeComponent();
        }

        public FilterType GetFilterType()
        {
            return FilterType.Mask;
        }

        public IFilter CreateFilter()
        {
            return new MaskFilter();
        }

        public void ClearSelectedFilter()
        {
            maskFilterList.Clear();
            targetGroupImage = null;
        }

        public void AddSelectedFilter(IFilter filter)
        {
            LogHelper.Debug(LoggerType.Operation, "MaskFilterParamControl - SetSelectedFilter");

            if (filter is MaskFilter)
            {
                maskFilterList.Add((MaskFilter)filter);
                UpdateData();
            }
        }

        private void UpdateData()
        {
            if (maskFilterList.Count == 0)
                return;

            MaskFilter maskFilter = maskFilterList[0];
            if (maskFilter.IsTrain)
            {
                pictureBoxTrain.Image = maskFilter.MaskImage.ToImageD().ToBitmap();
            }
            else
            {
                pictureBoxTrain.Image = null;
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (maskFilterList.Count == 0)
                return;

            MaskFilter maskFilter = maskFilterList[0];

            if (!maskFilter.IsTrain)
            {
                string debugMessage = "There is no Trained image.";
                LogHelper.Debug(LoggerType.Operation, debugMessage);
                MessageBox.Show(debugMessage);
                return;
            }

            MaskEditor maskEditor = new MaskEditor();
            maskEditor.SetImage(Image2D.FromBitmap(maskFilter.MaskImage.ToImageD().ToBitmap()));
            maskEditor.SetMaskFigures(maskFilter.MaskFigure);
            maskFilter.MaskFigure.Clear();

            if (maskEditor.ShowDialog(this) == DialogResult.OK)
            {
                ImageD imageD = maskEditor.GetImage();
                AlgoImage algoImage = ImageBuilder.Build("", imageD, imageD.NumBand == 1 ? ImageType.Grey : ImageType.Color);
                maskFilter.SetMasterImage(algoImage);
                ImageHelper.SaveImage(algoImage.ToImageD().ToBitmap(), @"D:\UpdatedFilterImage.bmp");
                ParamControl_ValueChanged();
                UpdateData();
            }
        }

        public void ParamControl_ValueChanged()
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "MaskFilterParamControl - ParamControl_ValueChanged");

                if (ValueChanged != null)
                    ValueChanged();
            }
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            if (targetGroupImage != null)
            {
                VisionProbe selecetedVisionProbe = null; // visionParamControl.SelectedProbe;

                RotatedRect filterRegion = selecetedVisionProbe.BaseRegion;
                RectangleF clipRegion = filterRegion.GetBoundRect();
                //if (useTargetCoordinate)
                //    clipRegion.Offset(-Target.Region.X, -Target.Region.Y);
                Rectangle clipRect = Rectangle.Round(clipRegion);

                MaskFilter maskFilter = maskFilterList[0];

                ImageD clipImage = targetGroupImage.ClipImage(clipRect);
                AlgoImage algoImage = ImageBuilder.Build(maskFilter.GetFilterType().ToString(), clipImage, clipImage.NumBand == 1 ? ImageType.Grey : ImageType.Color);

                foreach (IFilter filter in selecetedVisionProbe.InspAlgorithm.FilterList)
                {
                    if (filter.Equals(maskFilter))
                    {
                        break;
                    }
                    filter.Filter(algoImage);
                }

                maskFilter.SetMasterImage(algoImage);

                UpdateData();
            }
        }

        private void MaskFilterParamControl_VisibleChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            MaskFilter maskFilter = maskFilterList[0];

            if (maskFilter != null)
            {
                maskFilter.ClearMasterImage();
                UpdateData();
                ParamControl_ValueChanged();
            }
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
