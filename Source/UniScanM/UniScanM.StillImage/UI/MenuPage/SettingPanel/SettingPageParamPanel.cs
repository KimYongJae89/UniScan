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
    public partial class SettingPageParamPanel : UserControl, UniEye.Base.UI.IPage,IMultiLanguageSupport
    {
        bool onUpdate = false;
        Dictionary<DateTime, float> dataSource = new Dictionary<DateTime, float>();
        Timer speedTimer;


        public SettingPageParamPanel()
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
            marginW.Maximum = decimal.MaxValue;
            marginL.Maximum = decimal.MaxValue;
            blotW.Maximum = decimal.MaxValue;
            blotL.Maximum = decimal.MaxValue;
            //defectMaxArea.Maximum = decimal.MaxValue;
            StringManager.AddListener(this);
            UpdateData();
        }

        private void SpeedTimer_Tick(object sender, EventArgs e)
        {
            speedTimer.Stop();

            //double umPerMs = inspectStarter.AvgVelosity;
            //double mmPerMs = umPerMs / 1000;
            //double mPerMin = mmPerMs * 60;
            double mPerMin = SystemManager.Instance().InspectStarter.GetLineSpeedSv();
            //////////////////////////////////////////////////////////////
            if (StillImageSettings.Instance().AsyncMode == false)//todo zmsong
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

            switch(item)
            {
                case "SheetLength":
                    this.sheetLength.Text = ((double)value).ToString("F1");
                    break;
                case "SheetSpeed":
                    this.sheetSpeed.Text = ((double)value).ToString("F1");
                    break;
            }
        }

        private void UpdateData()
        {
            onUpdate = true;

            Settings.StillImageSettings additionalSettings = StillImageSettings.Instance() as Settings.StillImageSettings;
            if (additionalSettings != null)
            {
                this.minLineSpeed.Value = (decimal)additionalSettings.MinimumLineSpeed;
                this.inspectionMode.SelectedIndex = (int)additionalSettings.InspectionMode;
                this.operationMode.SelectedIndex = (int)additionalSettings.OperationMode;
                this.userStopMode.SelectedIndex = additionalSettings.UserStopMode == false ? 0 : 1;
                this.stopSheetCount.Value = (decimal)additionalSettings.StopDefectSheetCnt;
                //this.printingWarnLevel = (decimal)additionalSettings.print;
            }

            SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;
            Model model = SystemManager.Instance().CurrentModel as Model;
            InspectParam inspectParam = (StillImage.Data.InspectParam)model?.InspectParam;

            bool enable = inspectParam != null;
            this.unit.Enabled = enable;
            this.marginL.Enabled = this.marginW.Enabled = enable;
            this.blotW.Enabled = this.blotL.Enabled = enable;
            //this.defectMaxArea.Enabled = enable;
            this.matchRatio.Enabled = enable;

            if (enable)
            {
                this.unit.Text = (inspectParam.IsRelativeOffset) ? "Relative" : "Absolute";
                this.marginW.Value = (decimal)(inspectParam.OffsetRange.Margin.Width * (inspectParam.IsRelativeOffset?1:pelSize.Width));
                this.marginL.Value = (decimal)(inspectParam.OffsetRange.Margin.Height * (inspectParam.IsRelativeOffset ? 1 : pelSize.Height));
                this.blotW.Value = (decimal)(inspectParam.OffsetRange.Blot.Width* pelSize.Width);
                this.blotL.Value = (decimal)(inspectParam.OffsetRange.Blot.Height* pelSize.Height);
                //this.defectMaxArea.Value = (decimal)inspectParam.MaxDefectSize;
                //this.defectMinLength.Value = (decimal)inspectParam.MinDefectLength;

                this.matchRatio.Value = (decimal)inspectParam.MatchRatio * 100;
                //this.printingWarnLevel.Value = (decimal)inspectParam.PrintingLengthWarnLevelum;
                this.InspectionLevel.Value = inspectParam.InspectionLevelMin < InspectionLevel.Minimum ? InspectionLevel.Minimum : inspectParam.InspectionLevelMin;
                this.InspectionSize.Value = inspectParam.InspectionSizeMin;

                if (inspectParam.IsRelativeOffset)
                    this.label10.Text = this.label11.Text = "[%]";
                else
                    this.label10.Text = this.label11.Text = "[um]";
            }
            onUpdate = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Apply();
            
            if (SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel))
                MessageForm.Show(null, "Save Success.");
            else
                MessageForm.Show(null, "Save Fail.");
        }

        private bool Apply()
        {
            if (onUpdate)
                return false;

            Model model = SystemManager.Instance().CurrentModel as Model;

            Settings.StillImageSettings additionalSettings = StillImageSettings.Instance() as Settings.StillImageSettings;
            additionalSettings.MinimumLineSpeed = (float)this.minLineSpeed.Value;
            additionalSettings.InspectionMode = (EInspectionMode)inspectionMode.SelectedIndex;
            additionalSettings.OperationMode = (EOperationMode)operationMode.SelectedIndex;
            additionalSettings.UserStopMode = userStopMode.SelectedIndex == 0 ? false :true;
            additionalSettings.StopDefectSheetCnt = (int)this.stopSheetCount.Value;
            additionalSettings.Save();

            InspectParam inspectParam = (StillImage.Data.InspectParam)model.InspectParam;
            if (inspectParam != null)
            {
                inspectParam.IsRelativeOffset = unit.Text == "Relative";

                Feature feature = inspectParam.OffsetRange;
              
                try
                {
                    SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;

                    float mw = float.Parse(marginW.Text);
                    float ml = float.Parse(marginL.Text);
                    if (inspectParam.IsRelativeOffset == false)
                    {
                        mw /= pelSize.Width;
                        ml /= pelSize.Height;
                    }
                    float bw = float.Parse(blotW.Text) / pelSize.Width;
                    float bl = float.Parse(blotL.Text) / pelSize.Height;

                    feature.Margin = new SizeF(mw, ml);
                    feature.Blot = new SizeF(bw, bl);
                    //defectSize = (int)float.Parse(defectMaxArea.Text);

                }
                catch
                {
                    return false;
                }

                inspectParam.OffsetRange = feature;
                inspectParam.MatchRatio = (float)(this.matchRatio.Value) / 100;
                //inspectParam.PrintingLengthWarnLevelum = (double)this.printingWarnLevel.Value;
                inspectParam.InspectionLevelMin = (int)this.InspectionLevel.Value;
                inspectParam.InspectionSizeMin = (int)this.InspectionSize.Value;
            }

            return true;
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag)
                UpdateData();
        }

        private void unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (unit.Text == "Relative")
                label10.Text = label11.Text = "[%]";
            else
                label10.Text = label11.Text = "[um]";
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            inspectionMode.Items.Clear();
            string[] inspModes = Enum.GetNames(typeof(EInspectionMode));
            Array.ForEach(inspModes, f => inspectionMode.Items.Add(StringManager.GetString(this.GetType().FullName, f)));

            operationMode.Items.Clear();
            string[] opModes = Enum.GetNames(typeof(EOperationMode));
            Array.ForEach(opModes, f => operationMode.Items.Add(StringManager.GetString(this.GetType().FullName, f)));

            UpdateData();
        }

        private void SettingPageParamPanel_ControlRemoved(object sender, ControlEventArgs e)
        {
            speedTimer.Stop();
        }
    }
}
