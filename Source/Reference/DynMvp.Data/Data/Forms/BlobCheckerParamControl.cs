using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Vision;

namespace DynMvp.Data.Forms
{
    public partial class BlobCheckeParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;
        public FiducialChangedDelegate FiducialChanged = null;

        List<BlobChecker> blobCheckerList = new List<BlobChecker>();
        VisionProbe selectedProbe;

        bool onValueUpdate = false;
        bool systemOption = false;

        public BlobCheckeParamControl(bool systemOption = false)
        {
            InitializeComponent();
            
            darkBlob.Text = StringManager.GetString(this.GetType().FullName,darkBlob.Text);
            useWholeImage.Text = StringManager.GetString(this.GetType().FullName,useWholeImage.Text);
            useFiducialProbe.Text = StringManager.GetString(this.GetType().FullName,useFiducialProbe.Text);
            this.systemOption = systemOption;

            if(systemOption)
            {
                offsetRangeY.Hide();
                offsetRangeX.Hide();
                label2.Hide();
                label1.Hide();
                labelOffsetRange.Hide();
            }
        }

        public void ClearSelectedProbe()
        {
            blobCheckerList.Clear();
            selectedProbe = null;
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - SetSelectedProbe");

            VisionProbe visionProbe = (VisionProbe)probe;
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == BlobChecker.TypeName)
            {
                blobCheckerList.Add((BlobChecker)visionProbe.InspAlgorithm);
                if (blobCheckerList.Count == 1)
                    selectedProbe = visionProbe;
                else
                    selectedProbe = null;

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
            if (blobCheckerList.Count == 0)
                return;

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - UpdateData");

            onValueUpdate = true;

            BlobCheckerParam blobCheckerParam = (BlobCheckerParam)blobCheckerList[0].Param;

            if (selectedProbe != null)
                useFiducialProbe.Checked = selectedProbe.ActAsFiducialProbe;
            else
                useFiducialProbe.Enabled = false;

            panelFiducial.Visible = useFiducialProbe.Checked;

            darkBlob.Checked = blobCheckerParam.DarkBlob;
            searchRangeWidth.Value = blobCheckerParam.SearchRangeWidth;
            searchRangeHeight.Value = blobCheckerParam.SearchRangeHeight;
            offsetRangeX.Value = (Decimal)blobCheckerParam.OffsetRangeX;
            offsetRangeY.Value = (Decimal)blobCheckerParam.OffsetRangeY;
            areaLowerPct.Value = blobCheckerParam.MinArea;
            areaUpperPct.Value = blobCheckerParam.MaxArea;

            useWholeImage.Checked = blobCheckerParam.UseWholeImage;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - ParamControl_ValueChanged");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, algorithm, newParam, true);
            }
        }

        public void ParamControl_FiducialChanged(bool useFiducialProbe)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - ParamControl_FiducialChanged");

                if (FiducialChanged != null)
                    FiducialChanged(useFiducialProbe);
            }
        }

        private void searchRangeWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - searchRangeWidth_ValueChanged");

            if (blobCheckerList.Count == 0)
                return;

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).SearchRangeWidth = (int)searchRangeWidth.Value;
                if (systemOption)
                    ((BlobCheckerParam)newParam).OffsetRangeX = (int)searchRangeWidth.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void searchRangeHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - searchRangeHeight_ValueChanged");

            if (blobCheckerList.Count == 0)
                return;

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).SearchRangeHeight = (int)searchRangeHeight.Value;
                if (systemOption)
                    ((BlobCheckerParam)newParam).OffsetRangeY = (int)searchRangeHeight.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void useWholeImage_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - useWholeImage_CheckedChanged");

            if (blobCheckerList.Count == 0)
                return;

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).UseWholeImage = useWholeImage.Checked;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void useFiducialProbe_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            if (blobCheckerList.Count != 1)
                return;

            BlobCheckerParam newParam = (BlobCheckerParam)blobCheckerList[0].Param.Clone();
            newParam.ActAsFiducial = useFiducialProbe.Checked;

            panelFiducial.Visible = useFiducialProbe.Checked;

            if (useFiducialProbe.Checked)
            {
                searchRangeWidth.Value = 100;
                searchRangeHeight.Value = 100;
                newParam.SearchRangeWidth = 100;
                newParam.SearchRangeHeight = 100;
            }
            else
            {
                searchRangeWidth.Value = 0;
                searchRangeHeight.Value = 0;
                newParam.SearchRangeWidth = 0;
                newParam.SearchRangeHeight = 0;
            }

            
            ParamControl_FiducialChanged(useFiducialProbe.Checked);
            ParamControl_ValueChanged(ValueChangedType.Position, blobCheckerList[0], newParam);
        }

        private void offsetRangeX_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - offsetRange_ValueChanged");

            if (blobCheckerList.Count == 0)
                return;

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).OffsetRangeX = (int)offsetRangeX.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void offsetRangeY_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - offsetRange_ValueChanged");

            if (blobCheckerList.Count == 0)
                return;

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).OffsetRangeY = (int)offsetRangeY.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void areaLowerPct_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - areaLowerPct_ValueChanged");

            if (blobCheckerList.Count == 0)
                return;

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).MinArea = (int)areaLowerPct.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void areaUpperPct_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - areaUpperPct_ValueChanged");

            if (blobCheckerList.Count == 0)
                return;

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).MaxArea = (int)areaUpperPct.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void darkBlob_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - areaUpperPct_ValueChanged");

            if (blobCheckerList.Count == 0)
                return;

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).DarkBlob = darkBlob.Checked;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return BlobChecker.TypeName;
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
