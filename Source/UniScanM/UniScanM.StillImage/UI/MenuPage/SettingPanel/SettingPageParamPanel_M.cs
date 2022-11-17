using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.UI.Touch;
using UniEye.Base;
using UniScanM.StillImage.Settings;
using UniScanM.StillImage.Data;
using UniScanM.StillImage.Operation;
using UniEye.Base.UI;
using DynMvp.Base;
using DynMvp.Device.Serial;
//using UniScanM.Settings;

namespace UniScanM.StillImage.UI.MenuPage.SettingPanel
{
    public partial class SettingPageParamPanel_M : UserControl, UniEye.Base.UI.IPage, IMultiLanguageSupport
    {
        private bool encoderSpeedMeasure = false;

        Dictionary<DateTime, float> dataSource = new Dictionary<DateTime, float>();
        Timer speedTimer;
        public SettingPageParamPanel_M()
        {
            InitializeComponent();

            InspectRunner inspectRunner = (InspectRunner)SystemManager.Instance().InspectRunner;

            chartSpeed.ChartAreas[0].AxisX.Minimum = -30;
            chartSpeed.ChartAreas[0].AxisX.Maximum = 0;
            chartSpeed.Series[0].XValueMember = "X";
            chartSpeed.Series[0].YValueMembers = "Y";

            speedTimer = new Timer();
            speedTimer.Interval = 300;
            speedTimer.Tick += SpeedTimer_Tick;

            speedTimer.Start();

            //inspectionMode.Items.AddRange(Enum.GetValues(typeof(EInspectionMode)));
            //operationMode.Items.AddRange(Enum.GetNames(typeof(EOperationMode)));

            StringManager.AddListener(this);
        }

        private void SettingPageParamPanel_M_Load(object sender, EventArgs e)
        {
            UpdateData(false);
        }

        private void SpeedTimer_Tick(object sender, EventArgs e)
        {
            speedTimer.Stop();

            //double umPerMs = inspectStarter.AvgVelosity;
            //double mmPerMs = umPerMs / 1000;
            //double mPerMin = mmPerMs * 60;
            double mPerMin = SystemManager.Instance().InspectStarter.GetLineSpeedPv();
            //////////////////////////////////////////////////////////////
            if (this.encoderSpeedMeasure) //todo zmsong
            {
                SerialEncoder serialEncoder = SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo.DeviceType == ESerialDeviceType.SerialEncoder) as SerialEncoder;
                if (serialEncoder != null)
                {
                    double spdPlsPerMs = serialEncoder.GetSpeedPlsPerMs();
                    if (spdPlsPerMs >= 0)
                    {
                        float umPerPls = Settings.StillImageSettings.Instance().EncoderResolution; ;
                        mPerMin = spdPlsPerMs * umPerPls / 1000 * 60;
                    }
                }
            }
            //////////////////////////////////////////////////////////////
            /// encoderSplitter.EncoderPulsePerUm;
            UpdateControl("SheetSpeed", mPerMin);

            //if (mPerMin == 0)
            //{
            //    WriteSpeedLog(dataSource);
            //    dataSource.Clear();
            //}
            //else
            {
                DateTime dateTime = DateTime.Now;
                dataSource.Add(dateTime, (float)mPerMin);

                //if (this.Visible)
                {
                    List<PointF> dataList = GetChartData(60000);
                    int xAxisMin = -30;
                    if (dataList.Count > 0)
                    {
                        float temp = dataList.Min(f => f.X);
                        if (temp > -30)
                            xAxisMin = -30;
                        else //if (temp > -60)
                            xAxisMin = -60;
                        //else if (temp > -600)
                        //    xAxisMin = -600;
                        //else if (temp > -1800)
                        //    xAxisMin = -1800;
                        //else
                        //    xAxisMin = -3600;
                    }
                    if (chartSpeed != null && chartSpeed.IsHandleCreated)
                    {
                        chartSpeed.ChartAreas[0].AxisX.Minimum = xAxisMin;

                        chartSpeed.ChartAreas[0].AxisY.Minimum = Math.Max(dataList.Min(f => f.Y) * 0.9, 0);
                        chartSpeed.ChartAreas[0].AxisY.Maximum = Math.Max(dataList.Max(f => f.Y) * 1.1, 60);

                        chartSpeed.DataSource = dataList;
                        chartSpeed.DataBind();
                    }
                }
            }

            speedTimer.Start();
        }

        private List<PointF> GetChartData(int timeMs)
        {
            DateTime dateTime = DateTime.Now;

            //return dataSource.TakeWhile(f => (dateTime - f.Key).TotalSeconds < 10).ToList().ConvertAll<PointF>(f => new PointF((float)((f.Key - DateTime.Now).TotalSeconds), f.Value));

            IEnumerable<KeyValuePair<DateTime, float>> a = dataSource.Where(f => (dateTime - f.Key).TotalMilliseconds < timeMs);
            List<KeyValuePair<DateTime, float>> b = a.ToList();
            List<PointF> c = b.ConvertAll<PointF>(f => new PointF((float)((f.Key - DateTime.Now).TotalSeconds), f.Value));
            return c;
        }

        public void UpdateControl(string item, object value)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateControlDelegate(UpdateControl), item, value);
                return;
            }

            switch (item)
            {
                case "SheetLength":
                    this.lblSheetLength.Text = ((double)value).ToString("F1");
                    break;
                case "SheetSpeed":
                    this.lblSheetSpeed.Text = $"{(this.encoderSpeedMeasure ? "E" : "")}{((double)value).ToString("F1")}";
                    break;
            }
        }
        private void chbUseStopMode_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void DoDataSet(NumericUpDown ctrol, int value)
        {
            if (value < ctrol.Minimum) value = (int)ctrol.Minimum;
            if (value > ctrol.Maximum) value = (int)ctrol.Maximum;
            ctrol.Value = (Decimal)value;
        }

        private void UpdateData(bool update)
        {
            if(update == true) //Ctrol => Data  //////////////////////////////////////////////////////////////////////////////////////
            {
                Settings.StillImageSettings additionalSettings = StillImageSettings.Instance() as Settings.StillImageSettings;
                if (additionalSettings != null)
                {
                    additionalSettings.MinimumLineSpeed = (float)this.nudMinLineSpeed.Value;
                    additionalSettings.InspectionMode = (EInspectionMode)cbxInspectionMode.SelectedIndex;
                    additionalSettings.OperationMode = (EOperationMode)cbxOperationMode.SelectedIndex;
                    additionalSettings.UserStopMode = chbUseStopMode.Checked;
                    additionalSettings.StopDefectSheetCnt = (int)this.nudStopSheetCount.Value;
                    additionalSettings.RepeatInspectbyZone = (int)this.nudZoneRepeatCount.Value;
                    additionalSettings.Save();
                }

                SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;
                Model model = SystemManager.Instance().CurrentModel as Model;
                InspectParam inspectParam = (StillImage.Data.InspectParam)model?.InspectParam;

                bool existParam = inspectParam != null;
                if (existParam)
                {
                    this.cbxUnit.Text = (inspectParam.IsRelativeOffset) ? "Relative" : "Absolute";
                    if (inspectParam.IsRelativeOffset)
                        this.label10.Text = this.label11.Text = "[%]";
                    else
                        this.label10.Text = this.label11.Text = "[um]";

                    Feature feature = inspectParam.OffsetRange;
                    try
                    {
                        float mw = float.Parse(nudMarginW.Text);
                        float ml = float.Parse(nudMarginL.Text);
                        if (inspectParam.IsRelativeOffset == false)
                        {
                            mw /= pelSize.Width;
                            ml /= pelSize.Height;
                        }
                        float bw = float.Parse(nudBlotW.Text) / pelSize.Width;
                        float bl = float.Parse(nudBlotL.Text) / pelSize.Height;

                        feature.Margin = new SizeF(mw, ml);
                        feature.Blot = new SizeF(bw, bl);
                    }
                    catch
                    {
                        return;
                    }
                    inspectParam.OffsetRange = feature;
                    inspectParam.MatchRatio = (float)(this.nudMatchRatio.Value) / 100;

                    //inspectParam.PrintingLengthWarnLevelum = (double)this.nudPrintingWarnLevel.Value;
                    inspectParam.InspectionLevelMin = (int)this.nudInspectionLevelMin.Value;
                    inspectParam.InspectionLevelMax = (int)this.nudInspectionLevelMax.Value;
                    inspectParam.InspectionSizeMin = (int)this.nudInspectionSizeMin.Value;
                    inspectParam.InspectionSizeMax = (int)this.nudInspectionSizeMax.Value;
                }
            }
            else //Data => Ctrol  /////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                Settings.StillImageSettings additionalSettings = StillImageSettings.Instance() as Settings.StillImageSettings;
                if (additionalSettings != null)
                {
                    this.nudMinLineSpeed.Value = (decimal)additionalSettings.MinimumLineSpeed;
                    this.cbxInspectionMode.SelectedIndex = (int)additionalSettings.InspectionMode;
                    this.cbxOperationMode.SelectedIndex = (int)additionalSettings.OperationMode;
                    this.chbUseStopMode.Checked = additionalSettings.UserStopMode;
                    this.nudStopSheetCount.Value = (decimal)additionalSettings.StopDefectSheetCnt;
                    //additionalSettings.RepeatInspectbyZone = (int)this.nudZoneRepeatCount.Value;
                    DoDataSet(this.nudZoneRepeatCount, additionalSettings.RepeatInspectbyZone);
                }

                SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;
                Model model = SystemManager.Instance().CurrentModel as Model;
                InspectParam inspectParam = (StillImage.Data.InspectParam)model?.InspectParam;

                bool existParam = inspectParam != null;
                if (existParam)
                {
                    this.cbxUnit.Text = (inspectParam.IsRelativeOffset) ? "Relative" : "Absolute";
                    if (inspectParam.IsRelativeOffset)
                        this.label10.Text = this.label11.Text = "[%]";
                    else
                        this.label10.Text = this.label11.Text = "[um]";

                    DoDataSet(this.nudMarginW, (int)(inspectParam.OffsetRange.Margin.Width * (inspectParam.IsRelativeOffset ? 1 : pelSize.Width) ));
                    DoDataSet(this.nudMarginL, (int)(inspectParam.OffsetRange.Margin.Height * (inspectParam.IsRelativeOffset ? 1 : pelSize.Height)));
                    DoDataSet(this.nudBlotW, (int)(inspectParam.OffsetRange.Blot.Width * pelSize.Width));
                    DoDataSet(this.nudBlotL, (int)(inspectParam.OffsetRange.Blot.Height * pelSize.Height));

                    DoDataSet(this.nudMatchRatio, (int)(inspectParam.MatchRatio * 100));
                    //this.nudPrintingWarnLevel.Value = (decimal)inspectParam.PrintingLengthWarnLevelum;
                    DoDataSet(this.nudInspectionLevelMax, (int)(inspectParam.InspectionLevelMax));
                    DoDataSet(this.nudInspectionLevelMin, (int)(inspectParam.InspectionLevelMin));

                    DoDataSet(this.nudInspectionSizeMax, (int)(inspectParam.InspectionSizeMax));
                    DoDataSet(this.nudInspectionSizeMin, (int)(inspectParam.InspectionSizeMin));
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            UpdateData(true);

            if (SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel))
                MessageForm.Show(null, "Save Success.");
            else
                MessageForm.Show(null, "Save Fail.");
        }


        public void PageVisibleChanged(bool visibleFlag)
        {
 //           if (visibleFlag)
 //               UpdateData(false);
        }

        private void unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxUnit.Text == "Relative")
                label10.Text = label11.Text = "[%]";
            else
                label10.Text = label11.Text = "[um]";
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            cbxInspectionMode.Items.Clear();
            string[] inspModes = Enum.GetNames(typeof(EInspectionMode));
            Array.ForEach(inspModes, f => cbxInspectionMode.Items.Add(StringManager.GetString(this.GetType().FullName, f)));

            cbxOperationMode.Items.Clear();
            string[] opModes = Enum.GetNames(typeof(EOperationMode));
            Array.ForEach(opModes, f => cbxOperationMode.Items.Add(StringManager.GetString(this.GetType().FullName, f)));

            UpdateData(false);
        }

        private void SettingPageParamPanel_ControlRemoved(object sender, ControlEventArgs e)
        {
            speedTimer.Stop();
        }

        private void lblSheetSpeed_Click(object sender, EventArgs e)
        {
        }

        private void lblSheetSpeed_DoubleClick(object sender, EventArgs e)
        {
            this.encoderSpeedMeasure = !this.encoderSpeedMeasure;

        }
    }
}
