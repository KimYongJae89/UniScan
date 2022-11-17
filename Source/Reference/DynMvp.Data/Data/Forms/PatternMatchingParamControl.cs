using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using DynMvp.Vision;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Data.UI;

namespace DynMvp.Data.Forms
{
    public partial class PatternMatchingParamControl : UserControl, IAlgorithmParamControl
    {
        PatternMatching patternMatching = null;
        VisionProbe selectedProbe = null;

        Image2D targetGroupImage = null;
        public Image2D TargetGroupImage
        {
            set
            {
                LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - Set Target Group Image");
                targetGroupImage = value;
            }
        }

        bool onValueUpdate = false;

        public AlgorithmValueChangedDelegate ValueChanged = null;

        public PatternMatchingParamControl()
        {
            InitializeComponent();

            // language change
            labelSize.Text = StringManager.GetString(this.GetType().FullName, labelSize.Text);
            labelScore.Text = StringManager.GetString(this.GetType().FullName, labelScore.Text);
            labelW.Text = StringManager.GetString(this.GetType().FullName, labelW.Text);
            labelH.Text = StringManager.GetString(this.GetType().FullName, labelH.Text);
            addPatternButton.Text = StringManager.GetString(this.GetType().FullName, addPatternButton.Text);
            deletePatternButton.Text = StringManager.GetString(this.GetType().FullName, deletePatternButton.Text);
            refreshPatternButton.Text = StringManager.GetString(this.GetType().FullName, refreshPatternButton.Text);
            editMaskButton.Text = StringManager.GetString(this.GetType().FullName, editMaskButton.Text);
            ColumnPatternImage.HeaderText = StringManager.GetString(this.GetType().FullName, ColumnPatternImage.HeaderText);
            labelAngle.Text = StringManager.GetString(this.GetType().FullName, labelAngle.Text);
            labelAngleMin.Text = StringManager.GetString(this.GetType().FullName, labelAngleMin.Text);
            labelAngleMax.Text = StringManager.GetString(this.GetType().FullName, labelAngleMax.Text);
            labelScale.Text = StringManager.GetString(this.GetType().FullName, labelScale.Text);
            labelScaleMax.Text = StringManager.GetString(this.GetType().FullName, labelScaleMax.Text);
            labelScaleMin.Text = StringManager.GetString(this.GetType().FullName, labelScaleMin.Text);
            fiducialProbe.Text = StringManager.GetString(this.GetType().FullName, fiducialProbe.Text);
            useWholeImage.Text = StringManager.GetString(this.GetType().FullName, useWholeImage.Text);
            centerOffset.Text = StringManager.GetString(this.GetType().FullName, centerOffset.Text);
            useWholeImage.Text = StringManager.GetString(this.GetType().FullName, useWholeImage.Text);
            labelW.Text = StringManager.GetString(this.GetType().FullName, labelW.Text);
            labelH.Text = StringManager.GetString(this.GetType().FullName, labelH.Text);

            AlgorithmStrategy algorithmStrategy = AlgorithmBuilder.GetStrategy(PatternMatching.TypeName);
            try
            {
                if (algorithmStrategy.LibraryType != ImagingLibrary.CognexVisionPro)
                {
                    //labelAngle.Visible = false;
                    //minAngle.Visible = false;
                    //maxAngle.Visible = false;
                    //labelAngleMin.Visible = false;
                    //labelAngleMax.Visible = false;
                    labelScale.Visible = false;
                    labelScaleMax.Visible = false;
                    labelScaleMin.Visible = false;
                    minScale.Visible = false;
                    maxScale.Visible = false;
                }
            }
            catch (ArgumentNullException e)
            {
                LogHelper.Debug(LoggerType.StartUp, "LibraryType is null. check your algorithm license key.");
                if (MessageBox.Show(StringManager.GetString(this.GetType().FullName, "LibraryType is null. Please check your algorithm license key.")) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }


            List<object> newCollection = new List<object>();
            foreach (object item in patternType.Items)
            {
                newCollection.Add(StringManager.GetString(this.GetType().FullName, item.ToString()));
            }
            patternType.Items.Clear();
            patternType.Items.AddRange(newCollection.ToArray());

            patternImageSelector.RowTemplate.Height = patternImageSelector.Height - 20;
        }

        public void ClearSelectedProbe()
        {
            selectedProbe = null;
            patternMatching = null;
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - SetSelectedProbe");

            VisionProbe visionProbe = (VisionProbe)probe;
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == PatternMatching.TypeName)
            {
                selectedProbe = visionProbe;
                patternMatching = (PatternMatching)visionProbe.InspAlgorithm;
                UpdateData();
            }
            else
                throw new InvalidOperationException();
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdatePatternImageSelector()
        {
            patternImageSelector.Rows.Clear();

            PatternMatchingParam patternMatchingParam = (PatternMatchingParam)patternMatching.Param;//.Clone();

            foreach (Pattern pattern in patternMatchingParam.PatternList)
            {
                //ImageD patternImage = pattern.GetMaskedImage();
                ImageD patternImage = pattern.PatternImage;
                int index = patternImageSelector.Rows.Add(patternImage.ToBitmap());
#if DEBUG
                patternImage.SaveImage(string.Format("{0}\\{1}.bmp", Configuration.TempFolder, "patternImage"));
                pattern.PatternImage.SaveImage(string.Format("{0}\\{1}.bmp", Configuration.TempFolder, "patternImage2"));
#endif
                patternImageSelector.Rows[index].Tag = pattern;


                patternImageSelector.Rows[index].Height = patternImageSelector.Rows[index].Cells[0].ContentBounds.Height;
                if (patternImageSelector.Rows[index].Height > patternImageSelector.Height - patternImageSelector.ColumnHeadersHeight)
                {
                    patternImageSelector.Rows[index].Height = (patternImageSelector.Height - patternImageSelector.ColumnHeadersHeight);
                }
            }

            if (patternImageSelector.Rows.Count > 0)
            {
                patternImageSelector.Rows[0].Selected = true;
                patternType.SelectedIndex = (int)((Pattern)patternImageSelector.Rows[0].Tag).PatternType;
            }
        }

        private void UpdateData()
        {
            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - UpdateData");

            onValueUpdate = true;

            UpdatePatternImageSelector();

            PatternMatchingParam patternMatchingParam = (PatternMatchingParam)patternMatching.Param;//.Clone();

            searchRangeWidth.Value = patternMatchingParam.SearchRangeWidth;
            searchRangeHeight.Value = patternMatchingParam.SearchRangeHeight;

            minAngle.Value = (int)patternMatchingParam.MinAngle;
            maxAngle.Value = (int)patternMatchingParam.MaxAngle;
            minScale.Value = (int)(patternMatchingParam.MinScale * 100);
            maxScale.Value = (int)(patternMatchingParam.MaxScale * 100);

            matchScore.Value = patternMatchingParam.MatchScore;
            fiducialProbe.Checked = selectedProbe.ActAsFiducialProbe;
            useWholeImage.Checked = patternMatchingParam.UseWholeImage;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - VisionParamControl_PositionUpdated");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, algorithm, newParam, true);
            }
        }

        private void searchRangeWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - searchRangeWidth_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Error, "PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

            AlgorithmParam newParam = patternMatching.Param.Clone();
            ((PatternMatchingParam)newParam).SearchRangeWidth = (int)searchRangeWidth.Value;

            ParamControl_ValueChanged(ValueChangedType.Position, patternMatching, newParam);
        }

        private void searchRangeHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - searchRangeHeight_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Error, "PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

            AlgorithmParam newParam = patternMatching.Param.Clone();
            ((PatternMatchingParam)newParam).SearchRangeHeight = (int)searchRangeHeight.Value;

            ParamControl_ValueChanged(ValueChangedType.Position, patternMatching, newParam);
        }

        private void matchScore_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - matchScore_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Error, "PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

            AlgorithmParam newParam = patternMatching.Param.Clone();
            ((PatternMatchingParam)newParam).MatchScore = (int)matchScore.Value;

            ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == searchRangeHeight)
                valueName = StringManager.GetString(this.GetType().FullName, "Search Range Height");
            else if (sender == searchRangeWidth)
                valueName = StringManager.GetString(this.GetType().FullName, "Search Range Width");
            else if (sender == matchScore)
                valueName = StringManager.GetString(this.GetType().FullName, "Match Score");

            UpDownControl.ShowControl(valueName, (Control)sender);
        }

        private void addPatternButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - addPatternButton_Click");

            AlgorithmParam newParam = patternMatching.Param.Clone();

            ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
            AddPattern((PatternMatchingParam)newParam);

            UpdatePatternImageSelector();
        }

        private void AddPattern(PatternMatchingParam patternMatchingParam)
        {
            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - AddPattern");

            Target selectedTarget = selectedProbe.Target;

            RectangleF imageRegion = new RectangleF(0, 0, targetGroupImage.Width, targetGroupImage.Height);

            RectangleF probeRegion = selectedProbe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, imageRegion))
            {
                RotatedRect probeRotatedRect = selectedProbe.BaseRegion;

                ImageD clipImage = targetGroupImage.ClipImage(probeRotatedRect);

                ImageD filterredImage = selectedProbe.InspAlgorithm.Filter(clipImage, 0);

                Pattern pattern = patternMatchingParam.AddPattern((Image2D)filterredImage);

                patternType.SelectedIndex = (int)pattern.PatternType;
            }
            else
            {
                MessageBox.Show(StringManager.GetString(this.GetType().FullName, "Probe region is invalid."));
            }
        }

        private void deletePatternButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - deletePatternButton_Click");

            if (patternImageSelector.SelectedRows.Count > 0)
            {
                int index = patternImageSelector.SelectedRows[0].Index;
                if (index > -1)
                {
                    Pattern pattern = (Pattern)patternImageSelector.Rows[index].Tag;

                    PatternMatchingParam newParam = (PatternMatchingParam)patternMatching.Param.Clone();
                    newParam.RemovePattern(index);

                    ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);

                    patternImageSelector.Rows.RemoveAt(index);
                }
            }
        }

        private void refreshPatternButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - refreshPatternButton_Click");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Error, "PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

            patternImageSelector.Rows.Clear();

            PatternMatchingParam newParam = (PatternMatchingParam)patternMatching.Param.Clone();

            ((PatternMatchingParam)patternMatching.Param).RemoveAllPatterns();
            newParam.RemoveAllPatterns();
            AddPattern((PatternMatchingParam)newParam);

            ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);

            UpdatePatternImageSelector();
        }

        private void editMaskButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - editMaskButton_Click");

            if (patternImageSelector.SelectedRows.Count > 0)
            {
                int index = patternImageSelector.SelectedRows[0].Index;
                if (index > -1)
                {
                    PatternMatchingParam newParam = (PatternMatchingParam)patternMatching.Param.Clone();

                    Pattern pattern = (Pattern)newParam.GetPattern(index);

                    MaskEditor maskEditor = new MaskEditor();
                    maskEditor.SetImage(pattern.PatternImage);
                    maskEditor.SetMaskFigures(pattern.MaskFigures);
                    if (maskEditor.ShowDialog(this) == DialogResult.OK)
                    {
                        pattern.UpdateMaskImage();
                        patternImageSelector.Rows[index].Cells[0].Value = pattern.GetMaskedImage();

                        ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
                    }
                }
            }
        }

        private void patternImageSelector_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - patternImageSelector_CellClick");

            if (e.RowIndex > -1)
            {
                Pattern pattern = (Pattern)patternImageSelector.Rows[e.RowIndex].Tag;
                if (pattern == null)
                {
                    LogHelper.Error(LoggerType.Error, "PatternMatchingParamControl - pattern image is null.");
                    return;
                }

                patternType.SelectedIndex = (int)pattern.PatternType;

                //                ParamControl_ValueChanged(ValueChangedType.None);
            }
        }

        private void patternType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - patternType_SelectedIndexChanged");

            if (patternImageSelector.SelectedRows.Count > 0)
            {
                int index = patternImageSelector.SelectedRows[0].Index;
                if (index > -1)
                {
                    PatternMatchingParam newParam = (PatternMatchingParam)patternMatching.Param.Clone();

                    Pattern pattern = (Pattern)newParam.GetPattern(index);
                    if (pattern == null)
                    {
                        LogHelper.Error(LoggerType.Error, "PatternMatchingParamControl - pattern image is null.");
                        return;
                    }

                    pattern.PatternType = (PatternType)patternType.SelectedIndex;

                    ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
                }
            }
        }

        private void minAngle_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - minAngle_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Error, "PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

            AlgorithmParam newParam = patternMatching.Param.Clone();
            ((PatternMatchingParam)newParam).MinAngle = (int)minAngle.Value;

            ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
        }

        private void maxAngle_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - maxAngle_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Error, "PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

            AlgorithmParam newParam = patternMatching.Param.Clone();
            ((PatternMatchingParam)newParam).MaxAngle = (int)maxAngle.Value;

            ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
        }

        private void minScale_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - minScale_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Error, "PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

            AlgorithmParam newParam = patternMatching.Param.Clone();
            ((PatternMatchingParam)newParam).MinScale = (float)minScale.Value / 100;

            ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
        }

        private void maxScale_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - maxScale_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Error, "PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

            AlgorithmParam newParam = patternMatching.Param.Clone();
            ((PatternMatchingParam)newParam).MaxScale = (float)maxScale.Value / 100;

            ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
        }

        private void fiducialProbe_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            if (fiducialProbe.Checked)
                selectedProbe.Target.SelectFiducialProbe(selectedProbe.Id);
            else
                selectedProbe.Target.DeselectFiducialProbe(selectedProbe.Id);
        }

        private void centerOffset_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - minScale_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Error, "PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

            AlgorithmParam newParam = patternMatching.Param.Clone();
            ((PatternMatchingParam)newParam).UseImageCenter = centerOffset.Checked;

            ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
        }

        private void useWholeImage_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            AlgorithmParam newParam = patternMatching.Param.Clone();
            ((PatternMatchingParam)newParam).UseWholeImage = useWholeImage.Checked;

            ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return PatternMatching.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }

        public void SetTargetGroupImage(ImageD targetGroupImage)
        {
            if (targetGroupImage is Image2D)
                this.targetGroupImage = (Image2D)targetGroupImage;
        }
    }
}
