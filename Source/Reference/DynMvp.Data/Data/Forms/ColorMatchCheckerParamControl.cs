using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DynMvp.UI;
using DynMvp.Base;
using DynMvp.Vision;
using System.Xml;

namespace DynMvp.Data.Forms
{
    public partial class ColorMatchCheckerParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;

        VisionProbe selectedProbe = null;
        ColorMatchChecker colorMatchChecker = null;        

        Image2D targetImage = null;
        public Image2D TargetImage
        {
            set { targetImage = value; }
        }

        bool onValueUpdate = false;

        public ColorMatchCheckerParamControl()
        {
            InitializeComponent();

            buttonAdd.Text = StringManager.GetString(this.GetType().FullName,buttonAdd.Text);
            buttonDelete.Text = StringManager.GetString(this.GetType().FullName,buttonDelete.Text);
            buttonReset.Text = StringManager.GetString(this.GetType().FullName,buttonReset.Text);
            labelScore.Text = StringManager.GetString(this.GetType().FullName,labelScore.Text);
            labelSmoothing.Text = StringManager.GetString(this.GetType().FullName,labelSmoothing.Text);
            labelMatchColor.Text = StringManager.GetString(this.GetType().FullName,labelMatchColor.Text);
            checkUseColorSet.Text = StringManager.GetString(this.GetType().FullName,checkUseColorSet.Text);
            buttonSave.Text = StringManager.GetString(this.GetType().FullName,buttonSave.Text);

        }

        public void ClearSelectedProbe()
        {
            selectedProbe = null;
            colorMatchChecker = null;
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorMatchCheckerParamControl - SetSelectedProbe");

            selectedProbe = (VisionProbe)probe;

            if (selectedProbe.InspAlgorithm.GetAlgorithmType() == ColorMatchChecker.TypeName)
            {
                ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)selectedProbe.InspAlgorithm.Param;

                UpdateData();
                if (String.IsNullOrEmpty(colorMatchCheckerParam.MatchColor))
                    return;
            }
            else
                throw new InvalidOperationException();
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            LogHelper.Debug(LoggerType.Operation, "ColorMatchCheckerParamControl - UpdateData");
            
            onValueUpdate = true;

            ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)selectedProbe.InspAlgorithm.Param;

            matchScore.Value = colorMatchCheckerParam.MatchScore;
            matchColor.Text = colorMatchCheckerParam.MatchColor;

            UpdateGirdData();
            
            colorMatchChecker.Train();

            onValueUpdate = false;
        }


        //public void ParamControl_ValueChanged(ValueChangedType valueChangedType)
        //{
        //    if (onValueUpdate == false)
        //    {
        //        LogHelper.Debug(LoggerType.OpDebug, "ColorMatchCheckerParamControl - ParamControl_ValueChanged");

        //        if (ValueChanged != null)
        //            ValueChanged(valueChangedType, true);
        //    }
        //}

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - ParamControl_ValueChanged");

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, algorithm, newParam, true);
            }
        }

        private void AddColorPattern()
        {
            LogHelper.Debug(LoggerType.Operation, "ColorMatchParamControl - AddPattern");

            Target selectedTarget = selectedProbe.Target;

            RectangleF targetRegion = selectedTarget.BaseRegion.GetBoundRect();
            RectangleF probeRegion = selectedProbe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
            {
                RotatedRect probeRotatedRect = selectedProbe.BaseRegion;

                probeRotatedRect.X -= targetRegion.Left;
                probeRotatedRect.Y -= targetRegion.Top;

                Image2D clipImage = (Image2D)targetImage.ClipImage(probeRotatedRect);

                AlgoImage algoImage = ImageBuilder.Build(colorMatchChecker.GetAlgorithmType(), clipImage, ImageType.Color, ImageBandType.Luminance);

                string tempColorName = "";
                tempColorName = GetColorName();
                if (tempColorName == "")
                    return;
                if (String.IsNullOrEmpty(tempColorName))
                    return;

                ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)selectedProbe.InspAlgorithm.Param;

                ColorPattern colorPattern = colorMatchCheckerParam.AddColorPattern(tempColorName, clipImage);
                colorPattern.Image = clipImage;
                ((ColorMatchCheckerParam)colorMatchChecker.Param).Smoothing = Convert.ToInt32(txtSmoothing.Value);
                colorMatchChecker.Train();
                
                colorPatternGrid.Rows.Add(colorPattern.Name, colorPattern.Image);
                colorPatternGrid.Rows[0].Tag = colorPattern;
                colorPatternGrid.Rows[0].Height = Math.Min(colorPattern.Image.Height, colorPatternGrid.RowTemplate.Height);
                colorPatternGrid.Rows[0].Selected = true;
            }
            else
            {
                MessageBox.Show(StringManager.GetString(this.GetType().FullName, "Probe region is invalid."));
            }
        }
        private string GetColorName()
        {
            string name;

            InputForm form = new InputForm("Input Color Name");
            Point point = new Point(Screen.PrimaryScreen.Bounds.Width - Screen.PrimaryScreen.Bounds.Width / 4, Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.Bounds.Height / 4);
            form.ChangeLocation(point);

            if (form.ShowDialog(this) == DialogResult.OK && form.InputText != "")
            {
                name = form.InputText;
                for (int i = 0; i < colorPatternGrid.Rows.Count; i++)
                {
                    if (colorPatternGrid.Rows[i].Cells[0].Value.ToString() == form.InputText)
                    {
                        MessageBox.Show("The colorname is already exist.");
                        name = "";
                    }
                }
            }
            else
                name = "";
            return name;
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddColorPattern();
            UpdateData();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (colorPatternGrid.SelectedRows.Count > 0)
            {
                string deleteColorName = colorPatternGrid.SelectedRows[0].Cells[0].Value.ToString();
                colorPatternGrid.Rows.Remove(colorPatternGrid.SelectedRows[0]);

                ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)selectedProbe.InspAlgorithm.Param;

                colorMatchCheckerParam.DeleteColorPattern(deleteColorName);
            }

            colorPatternGrid.Refresh();
            UpdateData();
        }

        private void buttonSelectColorPatternFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            { 
                colorPatternGrid.Controls.Clear();
            }
        }

        private void matchScore_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorMatchCheckerParamControl - matchScore_ValueChanged");

            if (colorMatchChecker == null)
            {
                LogHelper.Error(LoggerType.Error, "ColorMatchCheckerParamControl - colorMatchChecker instance is null.");
                return;
            }

            ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)colorMatchChecker.Param;

            AlgorithmParam newParam = colorMatchCheckerParam.Clone();
            ((ColorMatchCheckerParam)newParam).MatchScore = (int)matchScore.Value;

            ParamControl_ValueChanged(ValueChangedType.None, colorMatchChecker, newParam);
        }

        private void matchColor_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorMatchCheckerParamControl - matchColor_TextChanged");

            if (colorMatchChecker == null)
            {
                LogHelper.Error(LoggerType.Error, "ColorMatchCheckerParamControl - colorMatchChecker instance is null.");
                return;
            }

            ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)selectedProbe.InspAlgorithm.Param;

            AlgorithmParam newParam = colorMatchCheckerParam.Clone();
            ((ColorMatchCheckerParam)newParam).MatchColor = matchColor.Text;

            ParamControl_ValueChanged(ValueChangedType.None, colorMatchChecker, newParam);
        }

        private void useColorPatternFile_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        //private void colorPatternFileName_TextChanged(object sender, EventArgs e)
        //{
        //    LogHelper.Debug(LoggerType.OpDebug, "ColorMatchCheckerParamControl - matchColor_TextChanged");

        //    if (colorMatchChecker == null)
        //    {
        //        LogHelper.Error(LoggerType.Error, "ColorMatchCheckerParamControl - colorMatchChecker instance is null.");
        //        return;
        //    }

        //    ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)selectedProbe.InspAlgorithm.Param;

        //    AlgorithmParam newParam = colorMatchCheckerParam.Clone();
        //    ((ColorMatchCheckerParam)newParam).ColorPatternFileName = colorPatternFileName.Text;

        //    ParamControl_ValueChanged(ValueChangedType.None, colorMatchChecker, newParam);
        //}


        private void colorPatternGrid_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void ColorMatchCheckerParamControl_Load(object sender, EventArgs e)
        {
            if(colorMatchChecker != null)
            {
                UpdateData();
                
            }
        }
        
        private void colorPatternGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (colorPatternGrid.SelectedRows.Count > 0)
            {
                DeleteDuplicateMatchColor();
            }
            UpdateData();
        }
        private void DeleteDuplicateMatchColor()
        {
            string addTargetName = colorPatternGrid.SelectedRows[0].Cells[0].Value.ToString();
            if (String.IsNullOrEmpty(addTargetName) == false)
            {
                string[] text = this.matchColor.Text.Split(';');
                for(int i = 0; i < text.Length; i++)
                {
                    if (addTargetName == text[i])
                    {
                        MessageBox.Show("Alreay added target name");
                        return;
                    }
                        
                }
                matchColor.Text += addTargetName + ';';
            }
        }

        private void comboIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colorFolderPath = String.Format(@"{0}\..\Config", Environment.CurrentDirectory); 
            if (comboIndex.SelectedIndex == 0 || checkUseColorSet.Checked == false)
            {
                return;
            }
            else
            {
                string fileName = String.Format(@"{0}\ColorSet{1}.xml", colorFolderPath, comboIndex.SelectedItem.ToString());

                if (File.Exists(fileName))
                {
                    if (MessageBox.Show("Do you want to change the color list?", "Caution", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        LoadData(fileName);
                        UpdateGirdData();
                    }
                    else
                        return;
                }
            }
        }

        private void LoadData(string fileName)
        {
            ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)selectedProbe.InspAlgorithm.Param;

            //칼라 데이터 초기화
            this.colorPatternGrid.Rows.Clear();
            colorMatchCheckerParam.RemoveAllColorPattern();

            //칼라 데이터 추가
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement colorElement = xmlDocument.CreateElement("", "Color", "");
            xmlDocument.AppendChild(colorElement);
            string tempName = fileName + ".xml";
            colorMatchCheckerParam.ColorPatternFileName = fileName;
            colorMatchCheckerParam.LoadParam(colorElement);
        }

        private void UpdateGirdData()
        {
            ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)selectedProbe.InspAlgorithm.Param;

            colorPatternGrid.Rows.Clear();
            if(colorMatchCheckerParam.ColorPatternList.Count > 0)
                foreach (ColorPattern colorPattern in colorMatchCheckerParam.ColorPatternList)
                {
                    if (String.IsNullOrEmpty(colorPattern.Name) == false)
                        colorPatternGrid.Rows.Add(colorPattern.Name, colorPattern.Image);
                }
        }

        private void comboIndex_Validating(object sender, CancelEventArgs e)
        {
            if(e.Cancel == true)
            {
                MessageBox.Show("Color load Cancel");
            }

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string colorFolderPath = String.Format(@"{0}\..\Config", Environment.CurrentDirectory);
            string fileName = String.Format(@"{0}\ColorSet{1}", colorFolderPath, comboIndex.SelectedItem.ToString());
            if (comboIndex.SelectedIndex == 0 || checkUseColorSet.Checked == false)
            {
                return;
            }
            else
            {
                if (colorPatternGrid.Rows.Count >= 0)
                {
                    if (MessageBox.Show("Do you want save the color list?", "Caution", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)selectedProbe.InspAlgorithm.Param;

                        XmlDocument xmlDocument = new XmlDocument();

                        XmlElement colorElement = xmlDocument.CreateElement("", "Color", "");
                        xmlDocument.AppendChild(colorElement);
                        string tempName = fileName + ".xml";
                        colorMatchCheckerParam.UseColorPatternFile = true;
                        colorMatchCheckerParam.ColorPatternFileName = tempName;
                        colorMatchCheckerParam.SaveParam(colorElement);
                    }
                    else
                        return;
                }
                
            }
        }

        private void checkUseColorSet_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorMatchCheckerParamControl - matchColor_TextChanged");

            if (colorMatchChecker == null)
            {
                LogHelper.Error(LoggerType.Error, "ColorMatchCheckerParamControl - colorMatchChecker instance is null.");
                return;
            }

            ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)selectedProbe.InspAlgorithm.Param;

            AlgorithmParam newParam = colorMatchCheckerParam.Clone();
            ((ColorMatchCheckerParam)newParam).UseColorPatternFile = checkUseColorSet.Checked;

            ParamControl_ValueChanged(ValueChangedType.None, colorMatchChecker, newParam);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            ResetColor();
        }

        private void ResetColor()
        {
            this.colorPatternGrid.Rows.Clear();
            ((ColorMatchCheckerParam)colorMatchChecker.Param).RemoveAllColorPattern();
            this.matchColor.Text = "";
            this.txtSmoothing.Value = 0;
            this.matchScore.Value = 0;
            if (colorMatchChecker != null)
            {
                ((ColorMatchCheckerParam)colorMatchChecker.Param).RemoveAllColorPattern();
                colorMatchChecker.Clear();
            }
            colorMatchChecker = (ColorMatchChecker)selectedProbe.InspAlgorithm;
            
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return ColorMatchChecker.TypeName;
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
