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

namespace UniScanX.MPAlignment.Algo.UI
{
    public partial class MPParamControl : UserControl, ProbeContainable
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;

        MPAlgorithm mpAlgorithm = null;

        private Bitmap probeImage = null;

        public CanvasPanel probeImageView;

        bool onValueUpdate = false;

        public MPParamControl()
        {
            InitializeComponent();

            //change language
            label_MPAlgorithm.Text = StringManager.GetString(label_MPAlgorithm.Text);

            this.probeImageView = new CanvasPanel();
            this.probeImageView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.probeImageView.Location = new System.Drawing.Point(0, 0);
            this.probeImageView.Name = "MpProbeImage";
            this.probeImageView.Size = new System.Drawing.Size(10, 10);
            this.probeImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.probeImageView.TabIndex = 26;
            this.probeImageView.TabStop = false;
            this.probeImageView.RotationLocked = false;
            this.pnlProbeImage.Controls.Add(this.probeImageView);


        }

        public void SetSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Algorithm, "MPParamControl - SetSelectedProbe");

            VisionProbe selectedProbe = (VisionProbe)probe;
            if (selectedProbe.InspAlgorithm.GetAlgorithmType() == MPAlgorithm.TypeName)
            {
                mpAlgorithm = (MPAlgorithm)selectedProbe.InspAlgorithm;
                UpdateData(false);
            }
            else
                throw new InvalidOperationException();
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData(bool uiToData )
        {
            LogHelper.Debug(LoggerType.Algorithm, "MPParamControl - UpdateData");

            onValueUpdate = true;
            MPAlgorithmParam param = mpAlgorithm.Param as MPAlgorithmParam;

            if(uiToData) //UI -> Data
            {

            }
            else //data ->UI
            {
                //nudTh1_down.Value = param.Threshold_convexDown;
                //nudTh1_up.Value = mpAlgorithm.Param.UpperValue;
            }

            onValueUpdate = false;
        }
        
        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "MPParamControl - ParamControl_ValueChanged");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, algorithm, newParam, true);
            }
        }

        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Algorithm, "MPParamControl - lowerValue_ValueChanged");

            if (mpAlgorithm == null)
            {
                LogHelper.Error(LoggerType.Algorithm, "MPParamControl - mpAlgorithm instance is null.");
                return;
            }
            var ctrol = sender as NumericUpDown;
            UpdateData(true);     

            ParamControl_ValueChanged(ValueChangedType.None, this.mpAlgorithm, this.mpAlgorithm.Param);
        }

    }
}
