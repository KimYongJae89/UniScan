using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Vision;
using DynMvp.Base;
using DynMvp.Vision.Planbss;
using System.Drawing;

namespace DynMvp.Data.Forms
{
    public partial class RectCheckerParamControl: UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;

        List<RectChecker> rectCheckerList = new List<RectChecker>();

        bool onValueUpdate = false;

        public RectCheckerParamControl()
        {
            InitializeComponent();

            labelSearchRange.Text = StringManager.GetString(this.GetType().FullName,labelSearchRange.Text);
            labelSearchLength.Text = StringManager.GetString(this.GetType().FullName,labelSearchLength.Text);
            labelEdgeType.Text = StringManager.GetString(this.GetType().FullName,labelEdgeType.Text);
            labelEdgeThickWidth.Text = StringManager.GetString(this.GetType().FullName,labelEdgeThickWidth.Text);
            labelEdgeThickHeight.Text = StringManager.GetString(this.GetType().FullName,labelEdgeThickHeight.Text);
            labelGrayValue.Text = StringManager.GetString(this.GetType().FullName,labelGrayValue.Text);
            labelScan.Text = StringManager.GetString(this.GetType().FullName,labelScan.Text);
            labelPassRate.Text = StringManager.GetString(this.GetType().FullName,labelPassRate.Text);
            labelCardinalPoint.Text = StringManager.GetString(this.GetType().FullName,labelCardinalPoint.Text);
            labelConvexShape.Text = StringManager.GetString(this.GetType().FullName,labelConvexShape.Text);
            outToIn.Text = StringManager.GetString(this.GetType().FullName,outToIn.Text);
        }

        public void ClearSelectedProbe()
        {
            rectCheckerList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - SetSelectedProbe");

            VisionProbe visionProbe = (VisionProbe)probe;
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == RectChecker.TypeName)
            {
                rectCheckerList.Add((RectChecker)visionProbe.InspAlgorithm);
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
            if (rectCheckerList.Count == 0)
                return;

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - UpdateData");

            onValueUpdate = true;

            RectCheckerParam param = (RectCheckerParam)rectCheckerList[0].Param;

            searchRange.Value = (int)param.SearchRange;
            edgeType.SelectedIndex = (int)param.EdgeType;
            edgeThickWidth.Value = (int)param.EdgeThickWidth;
            edgeThickHeight.Value = (int)param.EdgeThickHeight;
            grayValue.Value = (int)param.GrayValue;
            projectionHeight.Value = (int)param.ProjectionHeight;
            passRate.Value = (int)param.PassRate;
            cardinalPoint.SelectedIndex = (int)param.CardinalPoint;
            outToIn.Checked = param.OutToIn;
            searchLength.Value = (int)param.SearchLength;
            convexShape.SelectedIndex = (int)param.ConvexShape;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "CircleCheckeParamControl - ParamControl_ValueChanged");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, algorithm, newParam, true);
            }
        }

        private void searchRange_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - searchRange_ValueChanged");

            if (rectCheckerList.Count == 0)
                return;

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).SearchRange = Convert.ToInt32(searchRange.Text);

                ParamControl_ValueChanged(ValueChangedType.Position, rectChecker, newParam);
            }
        }

        private void RectCheckerParamControl_Load(object sender, EventArgs e)
        {

        }

        private void edgeThickWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - edgeThickWidth_ValueChanged");

            if (rectCheckerList.Count == 0)
                return;

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).EdgeThickWidth = Convert.ToInt32(edgeThickWidth.Text);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void edgeThickHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - edgeThickHeight_ValueChanged");

            if (rectCheckerList.Count == 0)
                return;

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).EdgeThickHeight = Convert.ToInt32(edgeThickHeight.Text);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void grayValue_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - grayValue_ValueChanged");

            if (rectCheckerList.Count == 0)
                return;

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).GrayValue = Convert.ToInt32(grayValue.Text);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void projectionHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - projectionHeight_ValueChanged");

            if (rectCheckerList.Count == 0)
                return;

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).ProjectionHeight = Convert.ToInt32(projectionHeight.Text);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void passRate_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - passRate_ValueChanged");

            if (rectCheckerList.Count == 0)
                return;

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).PassRate = Convert.ToInt32(passRate.Text);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void cardinalPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - cardinalPoint_SelectedIndexChanged");

            if (rectCheckerList.Count == 0)
                return;

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).CardinalPoint = (CardinalPoint)cardinalPoint.SelectedIndex;

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void convexShape_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - convexShape_SelectedIndexChanged");

            if (rectCheckerList.Count == 0)
                return;

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).ConvexShape = (ConvexShape)convexShape.SelectedIndex;

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void edgeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - edgeType_SelectedIndexChanged");

            if (rectCheckerList.Count == 0)
                return;

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).EdgeType = (EdgeType)(edgeType.SelectedIndex);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void searchLength_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - searchLength_ValueChanged");

            if (rectCheckerList.Count == 0)
                return;

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).SearchLength = Convert.ToInt32(searchLength.Text);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return RectChecker.TypeName;
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
