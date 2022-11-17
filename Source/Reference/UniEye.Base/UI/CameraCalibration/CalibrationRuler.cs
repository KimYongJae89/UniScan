using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Vision;

namespace UniEye.Base.UI.CameraCalibration
{
    public partial class CalibrationRuler : UserControl, CameraCalibrationPanel
    {
        private float rulerWidth = 0.8f;
        private float rulerHeight = 0.1f;
        private float rulerPartial = 0.2f;
        private float rulerScale = 1.0f;
        private int rulerReagionNum = 10;
        private int rulerThreshold = 110;
        private bool rulerThresholdAbs = false;

        public CalibrationRuler()
        {
            InitializeComponent();
        }

        public CalibrationResult Calibrate(Calibration calibration, ImageD imageD)
        {
            ApplyData();
            System.Drawing.Point center = System.Drawing.Point.Round(DrawingHelper.CenterPoint(new System.Drawing.Rectangle(System.Drawing.Point.Empty, imageD.Size)));
            Size size = new Size((int)Math.Round(imageD.Width * rulerWidth), (int)Math.Round(imageD.Height * rulerHeight));
            Rectangle rect = DrawingHelper.FromCenterSize(center, size);
            rect.Intersect(new Rectangle(Point.Empty, imageD.Size));
            CalibrationResult result = calibration.Calibrate(imageD, rect, rulerScale, this.rulerThresholdAbs ? rulerThreshold : -1);
            return result;
        }

        public void ChangeLanguage()
        {
 
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public delegate void ApplyDataDelegate();
        public void ApplyData()
        {
            if(InvokeRequired)
            {
                Invoke(new ApplyDataDelegate(ApplyData));
                return;
            }

            this.rulerWidth = (float)propertyWidth.Value / 100f;
            this.rulerHeight = (float)propertyHeight.Value / 100f;
            this.rulerScale = (float)propertyScale.Value;
            this.rulerReagionNum = (int)regionNum.Value;
            this.rulerThreshold = (int)propertyThreshold.Value;
            this.rulerThresholdAbs = checkThreshold.Checked;
            this.rulerPartial = (float)propertyPartial.Value / 100f;
        }

        public void UpdateData(Calibration calibration)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(UpdateData));
                return;
            }
            propertyWidth.Value = (decimal)(this.rulerWidth * 100f);
            propertyHeight.Value = (decimal)(this.rulerHeight * 100f);
            propertyScale.Value = (decimal)(this.rulerScale);
            regionNum.Value = (decimal)(this.rulerReagionNum);
            propertyThreshold.Value = (decimal)(this.rulerThreshold);
            checkThreshold.Checked = (bool)(this.rulerThresholdAbs);
            propertyPartial.Value = (decimal)(this.rulerPartial * 100f);
        }

        public void UpdateResult(CalibrationResult result, int subResultId)
        {
            Label[] labelList = null;
            PictureBox picBox = null;
            switch (subResultId)
            {
                case -1:
                    UpdateChart(result.projectionData);
                    return;
                    break;
                case 0:
                    picBox = this.pictureBox1;
                    labelList = new Label[5] { label26, label20, label24, label19, label15 };
                    break;
                case 1:
                    picBox = this.pictureBox2;
                    labelList = new Label[5] { label37, label31, label35, label30, label27 };
                    break;
                case 2:
                    picBox = this.pictureBox3;
                    labelList = new Label[5] { label48, label42, label46, label41, label38 };
                    break;
            }

            System.Diagnostics.Debug.Assert(labelList != null);

            if (picBox != null)
                SetPictureBoxImage(picBox, result.clipImage?.ToBitmap());
            
            SetLabelText(labelList[0], result.avgBrightness.ToString("0.000"));
            SetLabelText(labelList[1], result.minBrightness.ToString("0.000"));
            SetLabelText(labelList[2], result.maxBrightness.ToString("0.000"));
            //SetResultLabel(labelList[3], string.Format("Avg {0}(E2E {1})", result.resolution.Width.ToString("0.000"), result.resolution.Height.ToString("0.000")));
            SetLabelText(labelList[3], result.pelSize.Width.ToString("0.000"));
            SetLabelText(labelList[4], result.focusValue.ToString("0.000"));
        }

        private delegate void UpdateChartDelegate(List<float> dataSource);
        private void UpdateChart(float[] projectionData)
        {
            List<float> dataList = null;
            if (checkBoxUpdateLineProfile.Checked)
                dataList = projectionData.ToList();

            chart1.Invoke(new UpdateChartDelegate(UpdateChart2), dataList);            
        }

        private void UpdateChart2(List<float> dataSource)
        {
            chart1.Series.Clear();
            if (dataSource != null)
            {
                chart1.DataBindTable(dataSource);
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
                chart1.Series[0].BorderWidth = 2;
                chart1.Series[0].Color = Color.Black;
                chart1.ChartAreas[0].AxisX.Maximum = dataSource.Count;
                chart1.ChartAreas[0].AxisY.Maximum = Math.Min(255, dataSource.Max() * 1.1);
            }
        }

        private void SetLabelText(Label label, string message)
        {
            if (InvokeRequired)
            {
                Invoke(new SetLabelTextDelegate(SetLabelText), label, message);
                return;
            }

            label.Text = message;
        }

        private void SetPictureBoxImage(PictureBox pictureBox, Image image)
        {
            if (InvokeRequired)
            {
                Invoke(new SetPictureBoxImageDelegate(SetPictureBoxImage), pictureBox, image);
                return;
            }
            
            pictureBox.Image = image;
        }
    }
}
