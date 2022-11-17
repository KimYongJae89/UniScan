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
using DynMvp.Data.Forms;
using UniScanX.MPAlignment.Algo.UI;

namespace UniScanX.MPAlignment.Algo.UI
{
    public partial class PatternMatchingParamControl : UserControl, ProbeContainable
    {
        PatternMatching patternMatching = null;
        VisionProbe selectedProbe = null;

        Bitmap targetImage = null;
        public Bitmap TargetImage
        {
            set{ targetImage = value; }
        }

        bool onValueUpdate = false;

        public AlgorithmValueChangedDelegate ValueChanged = null;

        public PatternMatchingParamControl()
        {
            InitializeComponent();

            // language change
            labelSize.Text = StringManager.GetString(labelSize.Text);
            labelScore.Text = StringManager.GetString(labelScore.Text);
            labelW.Text = StringManager.GetString(labelW.Text);
            labelH.Text = StringManager.GetString(labelH.Text);
            addPatternButton.Text = StringManager.GetString(addPatternButton.Text);
            deletePatternButton.Text = StringManager.GetString(deletePatternButton.Text);
            refreshPatternButton.Text = StringManager.GetString(refreshPatternButton.Text);
            editMaskButton.Text = StringManager.GetString(editMaskButton.Text);
            ColumnPatternImage.HeaderText = StringManager.GetString(ColumnPatternImage.HeaderText);
            labelAngle.Text = StringManager.GetString(labelAngle.Text);
            labelAngleMin.Text = StringManager.GetString(labelAngleMin.Text);
            labelAngleMax.Text = StringManager.GetString(labelAngleMax.Text);
            labelScale.Text = StringManager.GetString(labelScale.Text);
            labelScaleMax.Text = StringManager.GetString(labelScaleMax.Text);
            labelScaleMin.Text = StringManager.GetString(labelScaleMin.Text);
            fiducialProbe.Text = StringManager.GetString(fiducialProbe.Text);
            ChangeVisibleControl();

            List<object> newCollection = new List<object>();
            foreach (object item in patternType.Items)
            {
                newCollection.Add(StringManager.GetString(item.ToString()));
            }
            patternType.Items.Clear();
            patternType.Items.AddRange(newCollection.ToArray());

            patternImageSelector.RowTemplate.Height = patternImageSelector.Height - 20; 
        }

        private void ChangeVisibleControl()
        {

        }
        public void SetSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Algorithm, ("PatternMatchingParamControl - SetSelectedProbe"));

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

        private void UpdateData()
        {
            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - UpdateData");

            //onValueUpdate = true;

            //patternImageSelector.Rows.Clear();

            //foreach (Pattern pattern in patternMatching.PatternList)
            //{
            //    Bitmap patternImage = pattern.GetMaskOverlayImage();
            //    int index = patternImageSelector.Rows.Add(patternImage);
            //    patternImageSelector.Rows[index].Tag = pattern;
            //    //patternImageSelector.Rows[index].Height = Math.Min(patternImage.Height, patternImageSelector.RowTemplate.Height);

            //    patternImageSelector.Rows[index].Height = (patternImageSelector.Height - patternImageSelector.ColumnHeadersHeight) / 2;
            //    //patternImageSelector.Rows[index].Height = patternImageSelector.Rows[index].Cells[1].ContentBounds.Height;
            //    if (patternImageSelector.Rows[index].Height > patternImageSelector.Height - patternImageSelector.ColumnHeadersHeight)
            //    {
            //        patternImageSelector.Rows[index].Height = (patternImageSelector.Height - patternImageSelector.ColumnHeadersHeight);
            //    }
            //}

            //if (patternImageSelector.Rows.Count > 0)
            //{
            //    patternImageSelector.Rows[0].Selected = true;
            //    patternType.SelectedIndex = (int)((Pattern)patternImageSelector.Rows[0].Tag).PatternType;
            //}

            //PatternMatchingParam param = patternMatching.Param;

            //searchRangeWidth.Value = param.SearchRangeWidth;
            //searchRangeHeight.Value = param.SearchRangeHeight;

            //minAngle.Value = (int)param.MinAngle;
            //maxAngle.Value = (int)param.MaxAngle;
            //minScale.Value = (int)(param.MinScale * 100);
            //maxScale.Value = (int)(param.MaxScale * 100);

            //matchScore.Value = param.MatchScore;
            //fiducialProbe.Checked = selectedProbe.ActAsFiducialProbe;

            //onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - VisionParamControl_PositionUpdated");

                //if (ValueChanged != null)
                //    ValueChanged(valueChangedType, true);
            }
        }

        private void searchRangeWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - searchRangeWidth_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Algorithm,"PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

   //         patternMatching.Param.SearchRangeWidth = (int)searchRangeWidth.Value;

            ParamControl_ValueChanged(ValueChangedType.Position);
        }

        private void searchRangeHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - searchRangeHeight_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Algorithm,"PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

       //     patternMatching.Param.SearchRangeHeight = (int)searchRangeHeight.Value;

            ParamControl_ValueChanged(ValueChangedType.Position);
        }

        private void matchScore_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - matchScore_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Algorithm,"PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

       //     patternMatching.Param.MatchScore = (int)matchScore.Value;

            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == searchRangeHeight)
                valueName = StringManager.GetString("Search Range Height");
            else if (sender == searchRangeWidth)
                valueName = StringManager.GetString("Search Range Width");
            else if (sender == matchScore)
                valueName = StringManager.GetString("Match Score");

            UpDownControl.ShowControl(valueName, (Control)sender);
        }

        private void addPatternButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - addPatternButton_Click");
            AddPattern();

            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void AddPattern()
        {
            //LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - AddPattern");

            //Target selectedTarget = selectedProbe.Target;

            //RectangleF targetRegion = selectedTarget.Region.GetBoundRect();
            //RectangleF probeRegion = selectedProbe.Region.GetBoundRect();
            //if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
            //{
            //    RotatedRect probeRotatedRect = selectedProbe.Region;

            //    probeRotatedRect.X -= targetRegion.Left;
            //    probeRotatedRect.Y -= targetRegion.Top;

            //    //Bitmap clipImage = ImageHelper.ClipImage(targetImage, probeRotatedRect);
            //    var image2d = Image2D.ToImage2D(targetImage);
            //    Bitmap clipImage = image2d.ClipImage(probeRotatedRect).ToBitmap();

            //    AlgoImage algoImage = ImageBuilder.Build(patternMatching.GetAlgorithmType(), clipImage, ImageType.Grey, ImageBandType.Luminance);

            //    selectedProbe.InspAlgorithm.Filter(algoImage);

            //    Pattern pattern = patternMatching.AddPattern(algoImage);

            //    patternImageSelector.Rows.Insert(0, pattern.GetMaskOverlayImage());
            //    patternImageSelector.Rows[0].Tag = pattern;
            //    patternImageSelector.Rows[0].Height = Math.Min(algoImage.Height, patternImageSelector.RowTemplate.Height);

            //    patternImageSelector.Rows[0].Height = (patternImageSelector.Height - patternImageSelector.ColumnHeadersHeight) / 2;
            //    //patternImageSelector.Rows[index].Height = patternImageSelector.Rows[index].Cells[1].ContentBounds.Height;
            //    if (patternImageSelector.Rows[0].Height > patternImageSelector.Height - patternImageSelector.ColumnHeadersHeight)
            //    {
            //        patternImageSelector.Rows[0].Height = (patternImageSelector.Height - patternImageSelector.ColumnHeadersHeight);
            //    }

            //    patternImageSelector.Rows[0].Selected = true;

            //    patternType.SelectedIndex = (int)pattern.PatternType;
            //}
            //else
            //{
            //    MessageBox.Show(StringManager.GetString("Probe region is invalid."));
            //}
        }

        private void deletePatternButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - deletePatternButton_Click");

            if (patternImageSelector.SelectedRows.Count > 0)
            {
                int index = patternImageSelector.SelectedRows[0].Index;
                if (index > -1)
                {
                    Pattern pattern = (Pattern)patternImageSelector.Rows[index].Tag;
                    patternMatching.RemovePattern(pattern);

                    patternImageSelector.Rows.RemoveAt(index);

                    ParamControl_ValueChanged(ValueChangedType.None);
                }
            }
        }

        private void refreshPatternButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - refreshPatternButton_Click");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Algorithm,"PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

            patternImageSelector.Rows.Clear();
            patternMatching.RemoveAllPatterns();
            AddPattern();

            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void editMaskButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - editMaskButton_Click");

            //if (patternImageSelector.SelectedRows.Count > 0)
            //{
            //    int index = patternImageSelector.SelectedRows[0].Index;
            //    if (index > -1)
            //    {
            //        Pattern pattern = (Pattern)patternImageSelector.Rows[index].Tag;

            //        MaskEditor maskEditor = new MaskEditor();
            //        maskEditor.SetImage(pattern.GetPatternImage());
            //        maskEditor.SetMaskFigures(pattern.MaskFigures);
            //        if (maskEditor.ShowDialog(this) == DialogResult.OK)
            //        {
            //            pattern.UpdateMaskImage();
            //            patternImageSelector.Rows[index].Cells[0].Value = pattern.GetMaskOverlayImage();

            //            ParamControl_ValueChanged(ValueChangedType.None);
            //        }
            //    }
            //}
        }

        private void patternImageSelector_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - patternImageSelector_CellClick");

            if (e.RowIndex > -1)
            {
                Pattern pattern = (Pattern)patternImageSelector.Rows[e.RowIndex].Tag;
                if (pattern == null)
                {
                    LogHelper.Error(LoggerType.Algorithm,"PatternMatchingParamControl - pattern image is null.");
                    return;
                }

                patternType.SelectedIndex = (int)pattern.PatternType;

                ParamControl_ValueChanged(ValueChangedType.None);
            }
        }

        private void patternType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - patternType_SelectedIndexChanged");

            if (patternImageSelector.SelectedRows.Count > 0)
            {
                int index = patternImageSelector.SelectedRows[0].Index;
                if (index > -1)
                {
                    Pattern pattern = (Pattern)patternImageSelector.Rows[index].Tag;
                    if (pattern == null)
                    {
                        LogHelper.Error(LoggerType.Algorithm,"PatternMatchingParamControl - pattern image is null.");
                        return;
                    }

                    pattern.PatternType = (PatternType)patternType.SelectedIndex;

                    ParamControl_ValueChanged(ValueChangedType.None);
                }
            }
        }

        private void minAngle_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - minAngle_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Algorithm, "PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

  //          patternMatching.Param.MinAngle = (int)minAngle.Value;

            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void maxAngle_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - maxAngle_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Algorithm, "PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

   //         patternMatching.Param.MaxAngle = (int)maxAngle.Value;

            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void minScale_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - minScale_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Algorithm,"PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

     //       patternMatching.Param.MinScale = (float)minScale.Value / 100;

            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void maxScale_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - maxScale_ValueChanged");

            if (patternMatching == null)
            {
                LogHelper.Error(LoggerType.Algorithm,"PatternMatchingParamControl - patternMatching instance is null.");
                return;
            }

     //       patternMatching.Param.MaxScale = (float)maxScale.Value / 100;

            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void fiducialProbe_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            if (fiducialProbe.Checked)
                //selectedProbe.Target.SetFiducialProbe(selectedProbe.Id);
                selectedProbe.ActAsFiducialProbe = true;
            else
                selectedProbe.ActAsFiducialProbe = false;
            //selectedProbe.Target.SetFiducialProbe(0);
        }

        private void PatternMatchingParamControl_Load(object sender, EventArgs e)
        {

        }

        private void buttonSavePattern_Click(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Algorithm, "PatternMatchingParamControl - patternType_SelectedIndexChanged");

            if (patternImageSelector.SelectedRows.Count > 0)
            {
                int index = patternImageSelector.SelectedRows[0].Index;
                if (index > -1)
                {
                    Pattern pattern = (Pattern)patternImageSelector.Rows[index].Tag;
                    if (pattern == null)
                    {
                        LogHelper.Error(LoggerType.Algorithm,"PatternMatchingParamControl - pattern image is null.");
                        return;
                    }

                    //SaveFileDialog dialog = new SaveFileDialog();
                    //if (dialog.ShowDialog() == DialogResult.OK)
                    //    pattern.SavePattern(dialog.FileName);
                }
            }
        }
    }
}
