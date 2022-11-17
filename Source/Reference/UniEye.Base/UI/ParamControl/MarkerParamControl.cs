using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Devices.Daq;
using DynMvp.Base;
using DynMvp.Data.Forms;

namespace UniEye.Base.UI.ParamControl
{
    public partial class MarkerParamControl : UserControl, IAlgorithmParamControl
    {
        public ValueChangedDelegate ValueChanged = null;

        Model model;
        public Model Model
        {
            get { return model; }
            set { model = value; }
        }

        private CommandManager commandManager;
        public CommandManager CommandManager
        {
            set { commandManager = value; }
        }

        MarkerProbe markerProbe;
        bool onValueUpdate = false;

        public MarkerParamControl()
        {
            LogHelper.Debug(LoggerType.Operation, "Begin MarkerParamControl-Ctor");

            InitializeComponent();

            labelMarkerType.Text = StringManager.GetString(this.GetType().FullName, labelMarkerType.Text);
            labelMergeSource.Text = StringManager.GetString(this.GetType().FullName, labelMergeSource.Text);
            label1.Text = StringManager.GetString(this.GetType().FullName, label1.Text);

            markerType.DataSource = Enum.GetNames(typeof(MarkerType));

            LogHelper.Debug(LoggerType.Operation, "End MarkerParamControl-Ctor");
        }

        public void UpdateProbeImage()
        {

        }

        public void ClearSelectedProbe()
        {
            markerProbe = null;
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - SetSelectedProbe");

            markerProbe = (MarkerProbe)probe;
            if (markerProbe != null)
                UpdateData();
        }

        private void UpdateData()
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - UpdateData");

            onValueUpdate = true;

            markerType.Text = markerProbe.MarkerType.ToString();
            mergeSource.Text = markerProbe.MergeSourceId;
            mergeOffsetX.Value = (Decimal)markerProbe.MergeOffset.X;
            mergeOffsetY.Value = (Decimal)markerProbe.MergeOffset.Y;
            mergeOffsetZ.Value = (Decimal)markerProbe.MergeOffset.Z;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - ParamControl_ValueChanged");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, true);
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            //if (sender == numSample)
            //    valueName = StringManager.GetString(this.GetType().FullName,labelNumSample.Text);


            UpDownControl.ShowControl(valueName, (Control)sender);
        }

        private void mergeSource_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - mergeSource_TextChanged");

            if (markerProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "MarkerParamControl - selectedProbe instance is null.");
                return;
            }

            try
            {
                markerProbe.MergeSourceId = mergeSource.Text;
                ParamControl_ValueChanged(ValueChangedType.None);
            }
            catch
            {
            }
        }

        private void mergeOffsetX_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - mergeOffsetX_ValueChanged");

            if (markerProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "MarkerParamControl - selectedProbe instance is null.");
                return;
            }

            try
            {
                markerProbe.MergeOffset.X = Convert.ToSingle(mergeOffsetX.Value);
                ParamControl_ValueChanged(ValueChangedType.None);
            }
            catch
            {
            }
        }

        private void mergeOffsetY_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - mergeSource_TextChanged");

            if (markerProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "MarkerParamControl - selectedProbe instance is null.");
                return;
            }

            try
            {
                markerProbe.MergeOffset.Y = Convert.ToSingle(mergeOffsetY.Value);
                ParamControl_ValueChanged(ValueChangedType.None);
            }
            catch
            {
            }
        }

        private void mergeOffsetZ_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - mergeSource_TextChanged");

            if (markerProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "MarkerParamControl - selectedProbe instance is null.");
                return;
            }

            try
            {
                markerProbe.MergeOffset.Z = Convert.ToSingle(mergeOffsetZ.Value);
                ParamControl_ValueChanged(ValueChangedType.None);
            }
            catch
            {
            }
        }

        private void markerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - markerType_SelectedIndexChanged");

            if (markerProbe == null)
            {
                LogHelper.Error(LoggerType.Error, "MarkerParamControl - selectedProbe instance is null.");
                return;
            }

            try
            {
                markerProbe.MarkerType = (MarkerType)Enum.Parse(typeof(MarkerType), markerType.Text);
                ParamControl_ValueChanged(ValueChangedType.None);
            }
            catch
            {
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return "Marker Probe";
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {

        }

        public void SetTargetGroupImage(ImageD image)
        {

        }
    }
}
