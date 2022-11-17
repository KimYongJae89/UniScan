using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.UI;

namespace DynMvp.Data.Forms
{
    public partial class MilCharReaderParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;

        CharReader charReader = null;
        VisionProbe selectedProbe = null;

        Image2D targetGroupImage;
        public Image2D TargetGroupImage
        {
            get { return targetGroupImage; }
            set { targetGroupImage = value; }
        }

        bool onValueUpdate = false;

        public MilCharReaderParamControl()
        {
            InitializeComponent();

            //change language
            labelDesiredString.Text = StringManager.GetString(this.GetType().FullName, labelDesiredString);
            labelFontFileName.Text = StringManager.GetString(this.GetType().FullName, labelFontFileName);
            labelNumCharacter.Text = StringManager.GetString(this.GetType().FullName, labelNumCharacter);
            labelMinScore.Text = StringManager.GetString(this.GetType().FullName, labelMinScore);
            groupBoxFont.Text = StringManager.GetString(this.GetType().FullName, groupBoxFont);
            labelStringCalibration.Text = StringManager.GetString(this.GetType().FullName, labelStringCalibration);
            CalibrationButton.Text = StringManager.GetString(this.GetType().FullName, CalibrationButton);
            extractFontButton.Text = StringManager.GetString(this.GetType().FullName, extractFontButton);
            showFontbutton.Text = StringManager.GetString(this.GetType().FullName, showFontbutton);
        }

        public void ClearSelectedProbe()
        {
            selectedProbe = null;
            charReader = null;
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "MilCharReaderParamControl - SetSelectedProbe");

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
            LogHelper.Debug(LoggerType.Operation, "MilCharReaderParamControl - UpdateData");

            onValueUpdate = true;

            CharReaderParam param = (CharReaderParam)charReader.Param;

            fontFileName.Text = Path.GetFileName(param.FontFileName);
            desiredString.Text = param.DesiredString;
            minScore.Value = (int)param.MinScore;
            desiredNumCharacter.Value = param.DesiredNumCharacter;

            calibrationString.Clear();
            fontGrid.Columns.Clear();
            fontGrid.Rows.Clear();

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

            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);

            desiredNumCharacter.Value = desiredString.Text.Length;
        }

        private void selectFontFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Mil Font File (*.mfo) | *.mfo";
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    CharReaderParam param = (CharReaderParam)charReader.Param;

                    AlgorithmParam newParam = charReader.Param.Clone();
                    ((CharReaderParam)newParam).FontFileName = dlg.FileName;

                    ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);

                    fontFileName.Text = dlg.SafeFileName;
                }
            }
        }

        private void minScore_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - minScore_ValueChanged");

            if (charReader == null)
            {
                LogHelper.Error(LoggerType.Error, "CharReaderParamControl - CharReader instance is null.");
                return;
            }

            AlgorithmParam newParam = charReader.Param.Clone();
            ((CharReaderParam)newParam).MinScore = (float)minScore.Value;

            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
        }

        private void ShowFontList()
        {
            Target selectedTarget = selectedProbe.Target;

            RectangleF targetRegion = selectedTarget.BaseRegion.GetBoundRect();
            RectangleF probeRegion = selectedProbe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
            {
                fontGrid.Columns.Clear();
                fontGrid.Rows.Clear();

                RotatedRect probeRegionInFov = selectedProbe.BaseRegion;

                ImageD clipImage = targetGroupImage.ClipImage(probeRegionInFov);

                AlgoImage algoImage = ImageBuilder.Build(charReader.GetAlgorithmType(), clipImage, ImageType.Grey, ImageBandType.Luminance);

                RectangleF clipRect = new RectangleF(0, 0, probeRegionInFov.Width, probeRegionInFov.Height);

                DebugContext debugContext = new DebugContext(false, "");
                AlgorithmResult algorithmResult = charReader.Read(algoImage, clipRect, debugContext);

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
                        ImageD charImage = targetGroupImage.ClipImage(position);
                        fontGrid.Rows[0].Cells[i].Value = charImage.ToBitmap();
                        fontGrid.Rows[0].Cells[i].Tag = charPositionList[i];
                    }
                }

                if (charReaderResult.Good == false)
                {
                    charReaderResult.ErrorMessage = charReaderResult.ErrorMessage;

                    String pathName = Path.Combine(Configuration.TempFolder, "OcrFailed");
                    DebugContext ocrDebugContext = new DebugContext(true, pathName);

                    string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    algoImage.Save(timeStamp + ".bmp", ocrDebugContext);

                    clipImage.SaveImage(Path.Combine(pathName, timeStamp + "_0.bmp"), ImageFormat.Bmp);
                }

                algoImage.Dispose();
            }
        }

        private void showFontbutton_Click(object sender, EventArgs e)
        {
            FontView fontView = new FontView();
            fontView.Initialize(charReader);
            fontView.ShowDialog();
        }

        private void extractFontButton_Click(object sender, EventArgs e)
        {
            Target selectedTarget = selectedProbe.Target;

            RectangleF targetRegion = selectedTarget.BaseRegion.GetBoundRect();
            RectangleF probeRegion = selectedProbe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
            {
                fontGrid.Columns.Clear();
                fontGrid.Rows.Clear();

                RotatedRect probeRegionInFov = selectedProbe.BaseRegion;

                ImageD probeClipImage = targetGroupImage.ClipImage(probeRegionInFov);

                AlgoImage algoImage = ImageBuilder.Build(charReader.GetAlgorithmType(), probeClipImage, ImageType.Grey, ImageBandType.Luminance);

                RectangleF clipRect = new RectangleF(0, 0, probeRegionInFov.Width, probeRegionInFov.Height);
                DebugContext debugContext = new DebugContext(true, Configuration.TempFolder);
                AlgorithmResult algorithmResult = charReader.Extract(algoImage, clipRect, 0, debugContext);

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
                        ImageD charImage = targetGroupImage.ClipImage(position);
                        fontGrid.Rows[0].Cells[i].Value = charImage.ToBitmap();
                        fontGrid.Rows[0].Cells[i].Tag = charPositionList[i];
                    }
                }

                if (charReaderResult.Good == false)
                {
                    charReaderResult.ErrorMessage = charReaderResult.ErrorMessage;

                    String pathName = Path.Combine(Configuration.TempFolder, "OcrFailed");
                    DebugContext ocrDebugContext = new DebugContext(true, pathName);

                    string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    algoImage.Save(timeStamp + ".bmp", ocrDebugContext);
                }

                algoImage.Dispose();
            }
        }

        private void fontGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            InputForm form = new InputForm("Charactor");
            if (form.ShowDialog() == DialogResult.OK)
            {
                Bitmap charImage = (Bitmap)fontGrid.Rows[0].Cells[e.ColumnIndex].Value;

                AlgoImage algoImage = ImageBuilder.Build(charReader.GetAlgorithmType(), Image2D.FromBitmap(charImage), ImageType.Grey, ImageBandType.Luminance);
                charReader.AddCharactor(algoImage, form.InputText[0].ToString());

                algoImage.Dispose();
            }
        }

        private void calibrationButton_Click(object sender, EventArgs e)
        {
            CharReaderParam param = (CharReaderParam)charReader.Param;

            if (File.Exists(param.FontFileName))
            {
                Target selectedTarget = selectedProbe.Target;

                RectangleF targetRegion = selectedTarget.BaseRegion.GetBoundRect();
                RectangleF probeRegion = selectedProbe.BaseRegion.GetBoundRect();
                if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
                {
                    fontGrid.Columns.Clear();
                    fontGrid.Rows.Clear();

                    RotatedRect probeRegionInFov = selectedProbe.BaseRegion;

                    RotatedRect searchRegionInFov = new RotatedRect(probeRegionInFov);

                    RectangleF imageRegionInFov = searchRegionInFov.GetBoundRect();

                    RectangleF imageClipRegion = imageRegionInFov;
                    imageClipRegion.X -= targetRegion.Left;
                    imageClipRegion.Y -= targetRegion.Top;

                    ImageD probeClipImage = targetGroupImage.ClipImage(Rectangle.Ceiling(imageClipRegion));

                    AlgoImage algoImage = ImageBuilder.Build(charReader.GetAlgorithmType(), probeClipImage, ImageType.Grey, ImageBandType.Luminance);

                    selectedProbe.InspAlgorithm.Filter(algoImage);
                    int count = charReader.CalibrateFont(algoImage, calibrationString.Text);

                    algoImage.Dispose();

                    if (calibrationString.Text.Count() == count)
                    {
                        MessageBox.Show(String.Format("Success!! (Desired Calibration String Num : {0}, Blob String Num : {1})", calibrationString.Text.Length, count));
                    }
                    else
                    {
                        MessageBox.Show(String.Format("Fail.. (Desired Calibration String Num : {0}, Blob String Num : {1})", calibrationString.Text.Length, count));
                    }
                }
            }
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

        }
    }
}