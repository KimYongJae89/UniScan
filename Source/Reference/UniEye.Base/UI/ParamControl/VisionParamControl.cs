using System;
using System.Drawing;
using System.Windows.Forms;

using DynMvp.UI.Touch;
using DynMvp.Data;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Vision;
using DynMvp.Data.Forms;
using DynMvp.Vision.UI;
using System.Collections.Generic;
using DynMvp.Data.FilterForm;
using UniEye.Base.Settings;

namespace UniEye.Base.UI.ParamControl
{
    public partial class VisionParamControl : UserControl, IAlgorithmParamControl
    {
        List<VisionProbe> selectedProbeList = new List<VisionProbe>();

        public ValueChangedDelegate ValueChanged = null;

        Target selectedTarget;
        public Target SelectedTarget
        {
            set
            {
                selectedTarget = value;
                UpdateData();
            }
        }

        PositionAligner positionAligner;
        public PositionAligner PositionAligner
        {
            set { positionAligner = value; }
        }

        private CommandManager commandManager;
        public CommandManager CommandManager
        {
            set { commandManager = value; }
        }

        Camera selectedCamera;
        public Camera SelectedCamera
        {
            get { return selectedCamera; }
            set { selectedCamera = value; }
        }

        ImageD targetGroupImage;
        public ImageD TargetGroupImage
        {
            get { return targetGroupImage; }
            set
            {
                LogHelper.Debug(LoggerType.Operation, "Vision Param Control - Set Target Image");

                targetGroupImage = value;
                if (selectedAlgorithmParamControl != null)
                    selectedAlgorithmParamControl.SetTargetGroupImage(targetGroupImage);
            }
        }

        // 컨트롤의 값을 프로그램적으로 갱신하고 있는 동안, 부품 이미지의 갱신을 하지 않도록 하기 위해 사용하는 파라미터
        // 갑이 갱신될 때, 각 갱신 이벤트에 이미지 갱신을 하는 함수를 호출하고 있어, 이 플랙이 없을 경우 반복적으로 갱신이 수행됨.
        bool onValueUpdate = false;

        private IAlgorithmParamControl selectedAlgorithmParamControl = null;
        public IAlgorithmParamControl SelectedAlgorithmParamControl
        {
            get { return selectedAlgorithmParamControl; }
        }

        private IFilterParamControl selectedFilterParamControl = null;
        public IFilterParamControl SelectedFilterParamControl
        {
            get { return selectedFilterParamControl; }
        }

        private List<IAlgorithmParamControl> algorithmParamControlList = new List<IAlgorithmParamControl>();
        public List<IAlgorithmParamControl> AlgorithmParamControlList
        {
            get { return algorithmParamControlList; }
            set { algorithmParamControlList = value; }
        }
        private List<IFilterParamControl> filterParamControlList = new List<IFilterParamControl>();

        public VisionParamControl()
        {
            LogHelper.Debug(LoggerType.Operation, "Begin VisionParamControl-Ctor");
            
            InitializeComponent();
            
            this.SuspendLayout();

            // 개별 Project에서 추가 필요
            //AddParamControl(new DirtyCheckerParamControl());
            //AddParamControl(new SealingCheckerParamControl());
            //AddParamControl(new FogAlignCheckerParamControl());
            //AddParamControl(new ContactLensCheckerParamControl());
            //AddParamControl(new CalibrationCheckerParamControl());
            //AddParamControl(new CarAlignCheckerParamControl());
            //AddParamControl(new PadCheckerParamControl());

            // Filter Param Control
            AddFilterParamControl(new BinarizationFilterParamControl());
            AddFilterParamControl(new EdgeExtractionFilterParamControl());
            AddFilterParamControl(new MorphologyFilterParamControl());
            AddFilterParamControl(new NoParamFilterParamControl());
            AddFilterParamControl(new SubtractionFilterParamControl());
            AddFilterParamControl(new MaskFilterParamControl());

            this.ResumeLayout(false);
            this.PerformLayout();

            //change language begin
            labelPos.Text = StringManager.GetString(this.GetType().FullName, labelPos.Text);
            labelSize.Text = StringManager.GetString(this.GetType().FullName, labelSize.Text);
            inverseResult.Text = StringManager.GetString(this.GetType().FullName, inverseResult.Text);
            labelW.Text = StringManager.GetString(this.GetType().FullName, labelW.Text);
            labelH.Text = StringManager.GetString(this.GetType().FullName, labelH.Text);
            imageBandLabel.Text = StringManager.GetString(this.GetType().FullName, imageBandLabel.Text);
            labelFiducialProbe.Text = StringManager.GetString(this.GetType().FullName, labelFiducialProbe.Text);
            buttonAddFilter.Text = StringManager.GetString(this.GetType().FullName, buttonAddFilter.Text);
            buttonDeleteFilter.Text = StringManager.GetString(this.GetType().FullName, buttonDeleteFilter.Text);
            imageBandLabel.Text = StringManager.GetString(this.GetType().FullName, imageBandLabel.Text);
            //change language end

            contextMenuStripAddFilter.Items.Clear();
            string[] filterTypeNames = Enum.GetNames(typeof(FilterType));
            foreach (string filterTypeName in filterTypeNames)
            {
                ToolStripItem filterToolStripItem = contextMenuStripAddFilter.Items.Add(StringManager.GetString(this.GetType().FullName, filterTypeName));
                filterToolStripItem.Tag = filterTypeName;
                filterToolStripItem.Click += filterToolStripItem_Click;
            }

            UpdateLightTypeCombo();
            
            SystemManager.Instance().UiChanger.SetupVisionParamControl(this);

            LogHelper.Debug(LoggerType.Operation, "End VisionParamControl-Ctor");
        }

        public void TabVisibleChange(bool general, bool filter, bool inspection)
        {
            tabControl1.TabPages.Clear();

            if (general == true)
                tabControl1.TabPages.Add(tabPageGeneral);

            if (filter == true)
                tabControl1.TabPages.Add(tabPageFilter);

            if (inspection == true)
                tabControl1.TabPages.Add(tabPageInspection);
        }

        public string GetTypeName()
        {
            return "Vision";
        }

        public void AddAlgorithmParamControl(IAlgorithmParamControl paramControl)
        {
            UserControl userControl = (UserControl)paramControl;

            userControl.Name = "algorithmParamControl";
            userControl.Location = new System.Drawing.Point(0, 0);
            userControl.Size = new System.Drawing.Size(10, 10);
            userControl.Dock = System.Windows.Forms.DockStyle.Fill;
            userControl.TabIndex = 26;
            userControl.Hide();
            paramControl.SetValueChanged(new AlgorithmValueChangedDelegate(VisionParamControl_AlgorithmValueChanged));

            algorithmParamControlList.Add(paramControl);
        }

        public void AddFilterParamControl(IFilterParamControl paramControl)
        {
            UserControl userControl = (UserControl)paramControl;

            userControl.Name = "filterParamControl";
            userControl.Location = new System.Drawing.Point(0, 0);
            userControl.Size = new System.Drawing.Size(10, 10);
            userControl.Dock = System.Windows.Forms.DockStyle.Fill;
            userControl.TabIndex = 26;
            userControl.Hide();
            paramControl.SetValueChanged(new FilterParamValueChangedDelegate(FilterParamControl_ValueChanged));

            filterParamControlList.Add(paramControl);
        }

        private void UpdateLightTypeCombo()
        {
            LogHelper.Debug(LoggerType.Operation, "InitLightTypeList");

            bool preOnValueUpdate = onValueUpdate;

            onValueUpdate = true;

            //int selectedIndex = lightTypeCombo.SelectedIndex;
            //lightTypeCombo.Items.Clear();
            int cc = 0;
            if (this.selectedTarget == null)
            {
                for (int i = 0; i < MachineSettings.Instance().NumLightType; i++)
                {
                    string text = String.Format(StringManager.GetString(this.GetType().FullName, "Light Type") + " {0}", i + 1);
                    if (i < lightTypeCombo.Items.Count)
                        lightTypeCombo.Items[i] = text;
                    else
                        lightTypeCombo.Items.Add(text);
                    cc++;
                }
            }
            else
            {
                LightParamSet lightParamSet = this.selectedTarget.TargetGroup.GetLightParamSet();
                //lightTypeCombo.Items.Clear();
                for (int i = 0; i < MachineSettings.Instance().NumLightType; i++)
                {
                    if(i < lightTypeCombo.Items.Count)
                        lightTypeCombo.Items[i] = lightParamSet.LightParamList[i].Name;
                    else
                        lightTypeCombo.Items.Add(lightParamSet.LightParamList[i].Name);                    
                    cc++;
                }
            }

            while(lightTypeCombo.Items.Count > cc)
            {
                lightTypeCombo.Items.RemoveAt(cc);
            }
            //onValueUpdate = false;
            //if (lightTypeCombo.Items.Count > selectedIndex && selectedIndex >= 0)
            //{
            //    lightTypeCombo.SelectedIndex = selectedIndex;
            //}

            if (lightTypeCombo.Items.Count > 0 && lightTypeCombo.SelectedIndex < 0)
                lightTypeCombo.SelectedIndex = 0;

            if (lightTypeCombo.Items.Count == 1)
                lightTypeCombo.Enabled = false;

            onValueUpdate = preOnValueUpdate;
        }

        private void FilterParamControl_ValueChanged()
        {
            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
        }


        private void buttonAddFilter_Click(object sender, EventArgs e)
        {
            Point pt = tabPageFilter.PointToScreen(new Point(buttonAddFilter.Bounds.Left, buttonAddFilter.Bounds.Bottom));
            contextMenuStripAddFilter.Show(pt);
        }

        private void filterToolStripItem_Click(object sender, EventArgs e)
        {
            if (selectedProbeList.Count == 0)
                return;

            Algorithm inspAlgorithm = selectedProbeList[0].InspAlgorithm;

            ToolStripItem filterToolStripItem = (ToolStripItem)sender;
            FilterType filterType = (FilterType)Enum.Parse(typeof(FilterType), (string)filterToolStripItem.Tag);
            IFilter filter = null;

            foreach(IFilterParamControl filterParamControl in filterParamControlList)
            {
                if (filterType == filterParamControl.GetFilterType())
                {
                    filter = filterParamControl.CreateFilter();
                    break;
                }
            }

            if (filter != null)
            {
                AddFilter(filter);
                UpdateData();
            }
        }

        public void ClearSelectedProbe()
        {
            selectedProbeList.Clear();
            selectedTarget = null;

            if (selectedAlgorithmParamControl != null)
            {
                ((UserControl)selectedAlgorithmParamControl).Visible = false;
                selectedAlgorithmParamControl.ClearSelectedProbe();
                selectedAlgorithmParamControl = null;
            }
        }

        public void AddSelectedProbe(Probe probe)
        {
            if (ShowAlgorithmParamControl((VisionProbe)probe) == true)
            {
                selectedProbeList.Clear();
            }

            selectedProbeList.Add((VisionProbe)probe);

            if (selectedTarget == null && selectedProbeList.Count == 1)
                selectedTarget = ((VisionProbe)probe).Target;
            else if (selectedTarget != ((VisionProbe)probe).Target)
                selectedTarget = null;

            if (selectedProbeList.Count != 0)
            {
                UpdateData();
            }
            else
            {
               EnableControls(false);
            }
        }

        public void UpdateProbeImage()
        {

        }

        public void UpdateTargetGroupImage(ImageD targetGroupImage)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - UpdateTargetGroupImage");

            this.targetGroupImage = targetGroupImage;
            foreach (IAlgorithmParamControl algorithmParamControl in algorithmParamControlList)
            {
                algorithmParamControl.SetTargetGroupImage(targetGroupImage);
            }

            LogHelper.Debug(LoggerType.Operation, "End VisionParamControl - UpdateTargetGroupImage");
        }

        private void UpdateData()
        {
            if (selectedFilterParamControl != null)
                ((UserControl)selectedFilterParamControl).Hide();

            if (selectedProbeList.Count == 0)
            {
                if (selectedAlgorithmParamControl != null)
                {
                    ((UserControl)selectedAlgorithmParamControl).Visible = false;
                    selectedAlgorithmParamControl.ClearSelectedProbe();
                    selectedAlgorithmParamControl = null;
                }
                EnableControls(false);
                return;
            }

            EnableControls(true);

            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - UpdateData");

            onValueUpdate = true;

            VisionProbe selectedProbe = selectedProbeList[0];

            probeHeight.Maximum = Math.Max(probeHeight.Maximum, (int)selectedProbe.BaseRegion.Height);
            probeWidth.Maximum = Math.Max(probeWidth.Maximum, (int)selectedProbe.BaseRegion.Width);

            probePosX.Maximum = Math.Max(probePosX.Maximum, (int)selectedProbe.BaseRegion.Height);
            probePosY.Maximum = Math.Max(probePosY.Maximum, (int)selectedProbe.BaseRegion.Width);

            probePosX.Value = (int)selectedProbe.BaseRegion.X;
            probePosY.Value = (int)selectedProbe.BaseRegion.Y;

            probePosR.Maximum = 360;
            probePosR.Value = (int)selectedProbe.BaseRegion.Angle;
            
            probeWidth.Value = (int)selectedProbe.BaseRegion.Width;
            probeHeight.Value = (int)selectedProbe.BaseRegion.Height;

            UpdateFilterListBox();
            //filterListBox.Items.Clear();
            Algorithm inspAlgorithm = selectedProbe.InspAlgorithm;
            //filterListBox.Items.AddRange(inspAlgorithm.FilterList.ToArray());

            //selectedProbe.PreviewFilterResult(targetGroupImage, 0, false);
            UpdateLightTypeCombo();
            if (lightTypeCombo.Items.Count > 0)
                lightTypeCombo.SelectedIndex = selectedProbe.LightTypeIndex;

            comboFiducialProbe.Items.Clear();
            comboFiducialProbe.Items.Add("None");

            if (selectedProbe.Target != null)
            {
                foreach (Probe probe in selectedProbe.Target)
                {
                    if (probe.ActAsFiducialProbe == true)
                        comboFiducialProbe.Items.Add(probe.Id);
                }
            }

            if (selectedProbe.FiducialProbeId == 0)
                comboFiducialProbe.SelectedIndex = 0;
            else
            {
                comboFiducialProbe.Text = selectedProbe.FiducialProbeId.ToString();
            }
            inverseResult.Checked = selectedProbe.InverseResult;

            imageBand.Text = inspAlgorithm.Param.ImageBand.ToString();

            if (inspAlgorithm.Param.SourceImageType == ImageType.Grey)
            {
                imageBand.SelectedIndex = 0;
                imageBand.Enabled = false;
            }
            else
            {
                imageBand.Text = inspAlgorithm.Param.ImageBand.ToString();
                imageBand.Enabled = true;
            }

            onValueUpdate = false;

            VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing, false);
        }

        private void EnableControls(bool enable)
        {
            probePosX.Enabled = enable;
            probePosY.Enabled = enable;
            probeWidth.Enabled = enable;
            probeHeight.Enabled = enable;
            buttonAddFilter.Enabled = enable;
            buttonDeleteFilter.Enabled = enable;
            inverseResult.Enabled = enable;
            imageBand.Enabled = enable;
            comboFiducialProbe.Enabled = enable;

            if (enable == false)
            {
                if (selectedAlgorithmParamControl != null)
                {
                    ((UserControl)selectedAlgorithmParamControl).Hide();
                    selectedAlgorithmParamControl.ClearSelectedProbe();
                    selectedAlgorithmParamControl = null;
                }
            }
        }

        private bool ShowAlgorithmParamControl(VisionProbe visionProbe)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - ShowAlgorithmParamControl");

            IAlgorithmParamControl preSelectedAlgorithmParamControl = selectedAlgorithmParamControl;

            string algorithmType = visionProbe.InspAlgorithm.GetAlgorithmType();

            bool paramTypeChanged = false;

            foreach(IAlgorithmParamControl algorithmParamControl in algorithmParamControlList)
            {
                if (algorithmType == algorithmParamControl.GetTypeName())
                {
                    selectedAlgorithmParamControl = algorithmParamControl;
                    break;
                }
            }

            if (selectedAlgorithmParamControl != preSelectedAlgorithmParamControl)
            {
                UserControl userControl = (UserControl)selectedAlgorithmParamControl;

                if (preSelectedAlgorithmParamControl != null)
                {
                    ((UserControl)preSelectedAlgorithmParamControl).Hide();
                    preSelectedAlgorithmParamControl.ClearSelectedProbe();
                }

                this.algorithmParamPanel.Controls.Clear();
                this.algorithmParamPanel.Controls.Add(userControl);

                selectedAlgorithmParamControl.SetTargetGroupImage(targetGroupImage);

                userControl.Show();


                paramTypeChanged = true;
            }

            if (selectedAlgorithmParamControl != null)
                ((IAlgorithmParamControl)selectedAlgorithmParamControl).AddSelectedProbe(visionProbe);

            return paramTypeChanged;
        }

        public void VisionParamControl_ValueChanged(ValueChangedType valueChangedType, bool modified = true)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "VisionParamControl - VisionParamControl_PositionUpdated");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, modified);
            }
        }

        public void VisionParamControl_AlgorithmValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam, bool modified = true)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "VisionParamControl - VisionParamControl_AlgorithmValueChanged");

                if (algorithm != null)
                {
                    AlgorithmParam oldParam = algorithm.Param.Clone();
                    commandManager.Execute(new ChangeParameterCommand(algorithm, oldParam, newParam));
                    SystemManager.Instance().CurrentModel.Modified |= modified;
                }

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, modified);
            }
        }

        public void VisionParamControl_FiducialChanged(bool useFiducialProbe)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "VisionParamControl - VisionParamControl_FiducialChanged");

                commandManager.Execute(new ChangeFiducialCommand(selectedProbeList[0], useFiducialProbe));
            }
        }

        private void inverseResult_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - inverseResult_CheckedChanged");

            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                selectedProbe.InverseResult = inverseResult.Checked;

                VisionParamControl_ValueChanged(ValueChangedType.None);
            }
       }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == probePosX)
                valueName = StringManager.GetString(this.GetType().FullName, "Position X");
            else if (sender == probePosY)
                valueName = StringManager.GetString(this.GetType().FullName, "Position Y");
            else if (sender == probeWidth)
                valueName = StringManager.GetString(this.GetType().FullName, "Width");
            else if (sender == probeHeight)
                valueName = StringManager.GetString(this.GetType().FullName, "Height");

            UpDownControl.ShowControl(valueName, (Control)sender);
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void colorBand_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - colorBand_SelectedIndexChanged");

            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                selectedProbe.InspAlgorithm.Param.ImageBand = (ImageBandType)Enum.Parse(typeof(ImageBandType), imageBand.Text);

                VisionParamControl_ValueChanged(ValueChangedType.ImageProcessing);
            }
        }

        private void probePosX_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - probePosX_ValueChanged");

            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                selectedProbe.X = (int)probePosX.Value;

                if (positionAligner != null)
                    selectedProbe.AlignedRegion = positionAligner.AlignFov(selectedProbe.BaseRegion);

                VisionParamControl_ValueChanged(ValueChangedType.Position);
            }
        }

        private void probePosY_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - probePosY_ValueChanged");

            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                selectedProbe.Y = (int)probePosY.Value;

                if (positionAligner != null)
                    selectedProbe.AlignedRegion = positionAligner.AlignFov(selectedProbe.BaseRegion);

                VisionParamControl_ValueChanged(ValueChangedType.Position);
            }
        }

        private void probePosR_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - probePosR_ValueChanged");

            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                selectedProbe.Angle = (int)probePosR.Value;

                if (positionAligner != null)
                    selectedProbe.AlignedRegion = positionAligner.AlignFov(selectedProbe.BaseRegion);

                VisionParamControl_ValueChanged(ValueChangedType.Position);
            }
        }

        private void probeWidth_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - probeWidth_ValueChanged");

            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                selectedProbe.Width = (int)probeWidth.Value;

                if (positionAligner != null)
                    selectedProbe.AlignedRegion = positionAligner.AlignFov(selectedProbe.BaseRegion);

                VisionParamControl_ValueChanged(ValueChangedType.Position);
            }
        }

        private void probeHeight_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - probeHeight_ValueChanged");

            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                selectedProbe.Height = (int)probeHeight.Value;

                if (positionAligner != null)
                    selectedProbe.AlignedRegion = positionAligner.AlignFov(selectedProbe.BaseRegion);

                VisionParamControl_ValueChanged(ValueChangedType.Position);
            }
        }

        private void comboFiducialProbe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - comboFiducialProbe_SelectedIndexChanged");

            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                if (comboFiducialProbe.SelectedIndex == 0)
                    selectedProbe.FiducialProbeId = 0;
                else
                    selectedProbe.FiducialProbeId = Convert.ToInt32(comboFiducialProbe.Text);

                VisionParamControl_ValueChanged(ValueChangedType.None);
            }
        }

        private void filterListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - ShowAlgorithmParamControl");

            if (selectedFilterParamControl != null)
            {
                ((UserControl)selectedFilterParamControl).Hide();
                selectedFilterParamControl.ClearSelectedFilter();
            }

            IFilter filter = (IFilter)filterListBox.SelectedItem;
            if (filter == null)
                return;

            foreach (IFilterParamControl filterParamControl in filterParamControlList)
            {
                if (filter.GetFilterType() == filterParamControl.GetFilterType())
                {
                    selectedFilterParamControl = filterParamControl;
                    break;
                }
            }

            this.panelFilterParamControl.Controls.Clear();
            this.panelFilterParamControl.Controls.Add((UserControl)selectedFilterParamControl);

            ((UserControl)selectedFilterParamControl).Show();
            selectedFilterParamControl.SetTargetGroupImage(targetGroupImage);

            selectedFilterParamControl.AddSelectedFilter(filter);
        }

        private void buttonDeleteFilter_Click(object sender, EventArgs e)
        {
            IFilter filter = (IFilter)filterListBox.SelectedItem;
            if (filter == null)
                return;

            RemoveFilter(filter);
            UpdateData();
        }

        private void lightTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate)
                return;

            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                selectedProbe.LightTypeIndex = lightTypeCombo.SelectedIndex;

                VisionParamControl_ValueChanged(ValueChangedType.Light);
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                ((IAlgorithmParamControl)selectedAlgorithmParamControl).PointSelected(clickPos, ref processingCancelled);
            }
        }

        private void AddFilter(IFilter filter)
        {
            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                selectedProbe.InspAlgorithm.FilterList.Add(filter);
            }
            UpdateFilterListBox();
        }

        private void RemoveFilter(IFilter filter)
        {
            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                selectedProbe.InspAlgorithm.FilterList.Remove(filter);
            }
            UpdateFilterListBox();
        }

        private void RemoveFilter(int index)
        {
            if (selectedProbeList.Count == 0)
                return;

            foreach (VisionProbe selectedProbe in selectedProbeList)
            {
                selectedProbe.InspAlgorithm.FilterList.RemoveAt(index);
            }
            UpdateFilterListBox();
        }

        private void UpdateFilterListBox()
        {
            if (selectedProbeList.Count == 0)
                return;

            VisionProbe selectedProbe = selectedProbeList[0];

            filterListBox.Items.Clear();

            filterListBox.Items.AddRange(selectedProbe.InspAlgorithm.FilterList.ToArray());
        }

        private void buttonFilterUp_Click(object sender, EventArgs e)
        {
            int selectedFilterIndex = filterListBox.SelectedIndex;
            if (selectedFilterIndex <0)
                return;

            int targetFilterIndex = selectedFilterIndex - 1;

            SwapFilter(selectedFilterIndex, targetFilterIndex);
            UpdateData();
        }

        private void buttonFilterDown_Click(object sender, EventArgs e)
        {
            int selectedFilterIndex = filterListBox.SelectedIndex;
            if (selectedFilterIndex < 0)
                return;

            int targetFilterIndex = selectedFilterIndex + 1;

            SwapFilter(selectedFilterIndex, targetFilterIndex);
            UpdateData();
        }

        private void SwapFilter(int selectedFilterIndex, int targetFilterIndex)
        {
            if (selectedProbeList.Count == 0)
                return;

            VisionProbe selectedProbe = selectedProbeList[0];

            List<IFilter> filterList = selectedProbe.InspAlgorithm.FilterList;
            if (selectedFilterIndex < 0 || filterList.Count <= selectedFilterIndex
                || targetFilterIndex < 0 || filterList.Count <= targetFilterIndex)
            {
                return;
            }

            IFilter buffer = filterList[selectedFilterIndex];
            filterList[selectedFilterIndex] = filterList[targetFilterIndex];
            filterList[targetFilterIndex] = buffer;

            UpdateFilterListBox();
        }

        private void lightTypeCombo_DropDown(object sender, EventArgs e)
        {
            UpdateLightTypeCombo();
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            // No need implement
        }

        public void SetTargetGroupImage(ImageD image)
        {
            // No need implement
        }

        private void VisionParamControl_Load(object sender, EventArgs e)
        {
            //SystemManager.Instance().UiChanger.SetupVisionParamControl(this);
        }
    }
}
