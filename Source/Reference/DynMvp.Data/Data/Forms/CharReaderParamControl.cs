using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.UI;

namespace DynMvp.Data.Forms
{
    public partial class CharReaderParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;

        CharReader charReader = null;
        VisionProbe selectedProbe = null;

        bool onValueUpdate = false;

        Image2D targetGroupImage = null;
        public Image2D TargetGroupImage
        {
            get { return targetGroupImage; }
            set { targetGroupImage = value; }
        }

        public CharReaderParamControl()
        {
            InitializeComponent();

            groupBoxCharactorSize.Text = StringManager.GetString(this.GetType().FullName, groupBoxCharactorSize.Text);
            labelWidth.Text = StringManager.GetString(this.GetType().FullName, labelWidth.Text);
            labelHeight.Text = StringManager.GetString(this.GetType().FullName, labelHeight.Text);
            labelXOverlap.Text = StringManager.GetString(this.GetType().FullName, labelXOverlap.Text);
            labelThreshold.Text = StringManager.GetString(this.GetType().FullName, labelThreshold.Text);
            labelPolarity.Text = StringManager.GetString(this.GetType().FullName, labelPolarity.Text);
            addThresholdButton.Text = StringManager.GetString(this.GetType().FullName, addThresholdButton.Text);
            deleteThresholdButton.Text = StringManager.GetString(this.GetType().FullName, deleteThresholdButton.Text);
            labelNumCharacter.Text = StringManager.GetString(this.GetType().FullName, labelNumCharacter.Text);
            labelDesiredString.Text = StringManager.GetString(this.GetType().FullName, labelDesiredString.Text);
            autoTuneButton.Text = StringManager.GetString(this.GetType().FullName, autoTuneButton.Text);
            groupBoxFont.Text = StringManager.GetString(this.GetType().FullName, groupBoxFont.Text);
            labelFontFile.Text = StringManager.GetString(this.GetType().FullName, labelFontFile.Text);
            trainButton.Text = StringManager.GetString(this.GetType().FullName, trainButton.Text);
            extractFontButton.Text = StringManager.GetString(this.GetType().FullName, extractFontButton.Text);
            showFontbutton.Text = StringManager.GetString(this.GetType().FullName, showFontbutton.Text);
            thresholdList.Text = StringManager.GetString(this.GetType().FullName, thresholdList.Text);
        }

        public void ClearSelectedProbe()
        {
            charReader = null;
            selectedProbe = null;
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "CharParamControl - SetSelectedProbe");

            selectedProbe = (VisionProbe)probe;
            if (selectedProbe.InspAlgorithm.GetAlgorithmType() == CharReader.TypeName)
            {
                charReader = (CharReader)selectedProbe.InspAlgorithm;
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
            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - UpdateData");

            onValueUpdate = true;

            CharReaderParam param = (CharReaderParam)charReader.Param;

            maxHeight.Value = param.CharacterMaxHeight;
            minHeight.Value = param.CharacterMinHeight;
            maxWidth.Value = param.CharacterMaxWidth;
            minWidth.Value = param.CharacterMinWidth;
            xOverlapRatio.Value = param.XOverlapRatio;
            polarity.SelectedIndex = (int)param.CharactorPolarity;
            desiredString.Text = param.DesiredString;
            desiredNumCharacter.Value = param.DesiredNumCharacter;

            fontFileName.Text = Path.GetFileName(param.FontFileName);
            charReader.Train(param.FontFileName);

            RefreshThresholdList();

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - ParamControl_ValueChanged");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, algorithm, newParam, true);
            }
        }

        private void minWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - minWidth_ValueChanged");

            if (charReader == null)
            {
                LogHelper.Error(LoggerType.Error, "CharParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = charReader.Param.Clone();
            ((CharReaderParam)newParam).CharacterMinWidth = (int)minWidth.Value;

            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
        }

        private void maxWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - maxWidth_ValueChanged");

            if (charReader == null)
            {
                LogHelper.Error(LoggerType.Error, "CharParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = charReader.Param.Clone();
            ((CharReaderParam)newParam).CharacterMaxWidth = (int)maxWidth.Value;

            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
        }

        private void minHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - minHeight_ValueChanged");

            if (charReader == null)
            {
                LogHelper.Error(LoggerType.Error, "CharParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = charReader.Param.Clone();
            ((CharReaderParam)newParam).CharacterMinHeight = (int)minHeight.Value;

            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
        }

        private void maxHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - maxHeight_ValueChanged");

            if (charReader == null)
            {
                LogHelper.Error(LoggerType.Error, "CharParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = charReader.Param.Clone();
            ((CharReaderParam)newParam).CharacterMaxHeight = (int)maxHeight.Value;

            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
        }

        private void polarity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - polarity_SelectedIndexChanged");

            if (charReader == null)
            {
                LogHelper.Error(LoggerType.Error, "CharReaderParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = charReader.Param.Clone();
            ((CharReaderParam)newParam).CharactorPolarity = (CharactorPolarity)polarity.SelectedIndex;

            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
        }

        private void desiredString_TextChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - desiredString_TextChanged");

            if (charReader == null)
            {
                LogHelper.Error(LoggerType.Error, "CharReaderParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = charReader.Param.Clone();
            ((CharReaderParam)newParam).DesiredString = desiredString.Text;
            desiredNumCharacter.Value = desiredString.Text.Length;

            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
        }

        private void selectFontFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    try
                    {
                        charReader.Train(dlg.FileName);

                        AlgorithmParam newParam = charReader.Param.Clone();
                        ((CharReaderParam)newParam).FontFileName = dlg.FileName;

                        ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);

                        fontFileName.Text = Path.GetFileName(dlg.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fail to CharReader training." + ex.Message);
                    }
                }
            }
        }

        private void trainButton_Click(object sender, EventArgs e)
        {
            CharReaderParam param = (CharReaderParam)charReader.Param;

            charReader.Train(param.FontFileName);
        }

        private void autoTuneButton_Click(object sender, EventArgs e)
        {
            if (desiredString.Text == "")
            {
                MessageBox.Show("Desired String is empty.");
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - autoTuneButton_Click");

            Target selectedTarget = selectedProbe.Target;

            RectangleF targetRegion = selectedTarget.BaseRegion.GetBoundRect();
            RectangleF probeRegion = selectedProbe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
            {
                RotatedRect probeRotatedRect = selectedProbe.BaseRegion;

                probeRotatedRect.X -= targetRegion.Left;
                probeRotatedRect.Y -= targetRegion.Top;
                AlgoImage algoImage = ImageBuilder.Build(charReader.GetAlgorithmType(), targetGroupImage, ImageType.Grey, ImageBandType.Luminance);

                charReader.AutoSegmentation(algoImage, probeRotatedRect, desiredString.Text);

                algoImage.Dispose();
            }
            else
            {
                MessageBox.Show(StringManager.GetString(this.GetType().FullName, "Probe region is invalid."));
            }
        }

        private void xOverlapRatio_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - xOverlapRatio_ValueChanged");

            if (charReader == null)
            {
                LogHelper.Error(LoggerType.Error, "CharReaderParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = charReader.Param.Clone();
            ((CharReaderParam)newParam).XOverlapRatio = (int)xOverlapRatio.Value;

            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
        }

        private void extractFontButton_Click(object sender, EventArgs e)
        {
            if (thresholdList.SelectedIndex == -1)
                return;

            int threshold = Convert.ToInt32(thresholdList.SelectedItem.ToString());

            Target selectedTarget = selectedProbe.Target;

            RectangleF targetRegion = selectedTarget.BaseRegion.GetBoundRect();
            RectangleF probeRegion = selectedProbe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
            {
                fontGrid.Columns.Clear();
                fontGrid.Rows.Clear();

                RotatedRect probeRegionInFov = selectedProbe.BaseRegion;

                ImageD probeClipImage = targetGroupImage.ClipImage(probeRegionInFov);

                DebugContext debugContext = new DebugContext(true, Configuration.TempFolder);
                AlgorithmResult algorithmResult = charReader.Extract(probeClipImage, probeRegionInFov, probeRegionInFov, threshold, debugContext);

                algorithmResult.Offset(-targetRegion.X, -targetRegion.Y);

                CharReaderResult charReaderResult = (CharReaderResult)algorithmResult;
                List<CharPosition> charPositionList = charReaderResult.CharPositionList;

                if (charPositionList.Count > 0)
                {
                    for (int i = 0; i < charPositionList.Count; i++)
                    {
                        DataGridViewImageColumn column = new DataGridViewImageColumn();
                        column.Width = 100;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        column.ImageLayout = DataGridViewImageCellLayout.Zoom;
                        column.DefaultCellStyle.NullValue = null;
                        column.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
                        fontGrid.Columns.Add(column);
                    }

                    fontGrid.Rows.Add(1);

                    for (int i = 0; i < charPositionList.Count; i++)
                    {
                        RotatedRect position = charPositionList[i].Position;
                        position.Offset(-targetRegion.X, -targetRegion.Y);
                        //ImageD image = targetGroupImage.ClipImage(position);
                        Image image = targetGroupImage.ClipImage(position).ToBitmap();
                        fontGrid.Rows[0].Cells[i].Value = image;
                        fontGrid.Rows[0].Cells[i].Tag = charPositionList[i];
                    }
                }

                if (charReaderResult.Good == false)
                {
                    charReaderResult.ErrorMessage = charReaderResult.ErrorMessage;

                    String pathName = Path.Combine(Configuration.TempFolder, "OcrFailed");
                    DebugContext ocrDebugContext = new DebugContext(true, pathName);

                    string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    DebugHelper.SaveImage(probeClipImage, timeStamp + ".bmp", ocrDebugContext);
                }
            }
        }

        private void fontGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            InputForm form = new InputForm("Charactor");
            if (form.ShowDialog() == DialogResult.OK)
            {
                CharPosition charPosition = (CharPosition)fontGrid.Rows[0].Cells[e.ColumnIndex].Tag;
                charReader.AddCharactor(charPosition, form.InputText[0]);

                CharReaderParam param = (CharReaderParam)charReader.Param;

                if (param.FontFileName == "")
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    if (dlg.ShowDialog() == DialogResult.Cancel)
                        return;

                    param.FontFileName = dlg.FileName;
                }

                charReader.SaveFontFile(param.FontFileName);
            }
        }

        private void RefreshThresholdList()
        {
            thresholdList.Items.Clear();

            CharReaderParam param = (CharReaderParam)charReader.Param;

            foreach (int threshold in param.ThresholdList)
            {
                thresholdList.Items.Add(threshold);
            }
        }

        private void addThresholdButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - addThresholdButton_Click");

            if (charReader == null)
            {
                LogHelper.Error(LoggerType.Error, "CharReaderParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = charReader.Param.Clone();
            ((CharReaderParam)newParam).ThresholdList.Add((int)threshold.Value);

            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);

            RefreshThresholdList();
        }

        private void deleteThresholdButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - deleteThresholdButton_Click");

            if (charReader == null)
            {
                LogHelper.Error(LoggerType.Error, "CharReaderParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = charReader.Param.Clone();
            ((CharReaderParam)newParam).ThresholdList.Remove((int)threshold.Value);

            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            RefreshThresholdList();
        }

        private void thresholdList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (thresholdList.SelectedIndex > -1)
                threshold.Value = Convert.ToInt32(thresholdList.Items[thresholdList.SelectedIndex].ToString());
        }

        private void desiredNumCharacter_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - desiredNumCharacter_ValueChanged");

            if (charReader == null)
            {
                LogHelper.Error(LoggerType.Error, "CharReaderParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = charReader.Param.Clone();
            ((CharReaderParam)newParam).DesiredNumCharacter = (int)desiredNumCharacter.Value;

            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
        }

        private void showFontButton_Click(object sender, EventArgs e)
        {
            FontView fontView = new FontView();
            fontView.Initialize(charReader);
            fontView.ShowDialog();
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return CharReader.TypeName;
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
