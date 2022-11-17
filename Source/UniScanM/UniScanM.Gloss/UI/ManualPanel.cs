using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Device.Daq.Sensor.UsbSensorGloss_60;
using DynMvp.Device.Device.Serial.Sensor.CD22_15_485;
using DynMvp.Device.Serial;
using DynMvp.Devices.MotionController;
using DynMvp.UI.Touch;
using Infragistics.Win.UltraWinEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScanM.Gloss.Data;
using UniScanM.Gloss.Operation;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.UI
{
    public partial class ManualPanel : UserControl, ITeachPage
    {
        #region 필드
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private System.Windows.Forms.Timer positionTimer = null;
        private ManualDataExporter manualExporter = null;
        private SerialSensorCD22_15_485 serialSensor;
        private List<float> startSensorValueList = new List<float>();
        private List<float> endSensorValueList = new List<float>();
        private bool isLongRunProc = true;
        private bool isClear = false;
        private int repeatCnt = 0;
        private double maxGlossValue = 0.0;
        #endregion

        #region 생성자
        public ManualPanel()
        {
            InitializeComponent();

            InitializeComboBox();

            comboSensor.SelectedIndex = 0;

            lblPath.Text = CurrentPath;
            manualExporter = new ManualDataExporter();

            positionTimer = new System.Windows.Forms.Timer();
            positionTimer.Tick += PositionTimer_Tick;
            positionTimer.Interval = 100;

            //if (CheckDeviceExist())
            //    IsOpen = true;

            //SerialDeviceHandler serialDeviceHandler = SystemManager.Instance().DeviceBox.SerialDeviceHandler;
            //SerialDevice serialDevice = serialDeviceHandler.Find(x => x.DeviceInfo.DeviceType == ESerialDeviceType.SerialSensor);
            //if (serialDevice is SerialSensorCD22_15_485)
            //{
            //    SerialSensor serialSensor = (SerialSensor)serialDevice;

            //    if (serialSensor != null && serialSensor.SerialPortEx != null)
            //    {
            //        //serialSensor.SerialPortEx.PacketHandler.PacketParser.OnDataReceived += Sensor_OnDataReceived;
            //        this.SerialSensor = (SerialSensorCD22_15_485)serialSensor;
            //    }
            //}
        }
        #endregion


        #region 대리자
        delegate void SetLaserTextCallback(string text);
        delegate void SetGlossTextCallback(string text);
        delegate void SetGlossMaxTextCallback(string text);
        #endregion


        #region 이벤트
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (IsOpen == false)
            {
                List<GlossSensorInfo> deviceList = UsbSensorGloss60.ListDevices();

                if (deviceList == null || deviceList.Count() == 0)
                {
                    MessageBox.Show("Device를 찾을 수 없습니다.");
                    return;
                }

                foreach (var device in deviceList)
                {
                    UsbSensorGloss60.Open(device.PortNo);
                    MessageBox.Show("Com [" + device.PortNo.ToString() + "] : Open Success");
                }
            }
            else
            {
                MessageBox.Show("이미 오픈되어있습니다..");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (IsOpen == false)
            {
                MessageBox.Show("Device를 Open 해주세요");
                return;
            }
            else if (IsOpen)
            {
                UsbSensorGloss60.Close();
                MessageBox.Show("Close Success");
            }
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            UsbSensorGloss60.SetDateTime();

            UsbSensorGloss60.Calibrate();

            string calStatusFlag = UsbSensorGloss60.GetCalibrateStatusFlag();
            string calDate = UsbSensorGloss60.GetCalibrationDate();

            if (calStatusFlag == "0")
                MessageBox.Show("Calibration Success !!\n\n" + $"LastCalibrationDate: {calDate}\n");
            else
                MessageBox.Show($"Calibration Error : {calStatusFlag}\n\n" + $"LastCalibrationDate: {calDate}\n");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            UsbSensorGloss60.Reset();
        }


        private void comboSensor_SelectedValueChanged(object sender, EventArgs e)
        {
            serialSensor = SerialDeviceHandler[comboSensor.SelectedIndex] as SerialSensorCD22_15_485;
        }

        private void btnLaserInit_Click(object sender, EventArgs e)
        {
            serialSensor.Initialize();
        }

        private void btnLaserMeasure_Click(object sender, EventArgs e)
        {
            float[] datas = new float[1];

            if (!serialSensor.GetData(1, datas))
                MessageBox.Show("유효한 거리가 아닙니다.");
            else
                lblLaserValue.Text = datas[0].ToString();
        }

        private void btnLaserOn_Click(object sender, EventArgs e)
        {
            serialSensor.LaserOn();
        }

        private void btnLaserOff_Click(object sender, EventArgs e)
        {
            serialSensor.LaserOff();
        }

        private void btnExecuteZeroSet_Click(object sender, EventArgs e)
        {
            serialSensor.ExecuteZeroReset();
        }

        private void btnReleaseZeroSet_Click(object sender, EventArgs e)
        {
            serialSensor.ReleaseZeroReset();
        }


        private void btnClear_Click(object sender, EventArgs e)
        {

            isClear = true;
            lblGlossMax.Text = "0";
        }


        private void numericBlackMirror_ValueChanged(object sender, EventArgs e)
        {
            GlossSettings.BlackMirrorValue = Convert.ToSingle(numericBlackMirror.Value);
            GlossSettings.Save();
        }

        private void buttonCalibration_Click(object sender, EventArgs e)
        {
            if (MessageForm.Show(null, "Calibration Setting will be Changed, Are you Sure?", MessageFormType.YesNo) == DialogResult.Yes)
            {
                var InspectRunner = SystemManager.Instance().InspectRunner as InspectRunner;
                InspectRunner.CalibrationProcess();
                InspectRunner.GlossMotionController.MoveSafetyPos();

                var calData = InspectRunner.CalibrationData;
                GlossSettings.AxisXDiffDegree = calData.AxisXDiffDegree;
                GlossSettings.AxisYDiffDegree = calData.AxisYDiffDegree;
                GlossSettings.FarFromMirrorSensorValue = calData.FarFromMirrorSensorValue;
                GlossSettings.NearToMirrorSensorLeftSideValue = calData.NearToMirrorSensorLeftSideValue;
                GlossSettings.NearToMirrorSensorRightSideValue = calData.NearToMirrorSensorRightSideValue;
                GlossSettings.BlackMirrorValue = calData.AfterBlackMirrorValue;
                GlossSettings.Save();

                labelXDegree.Text = calData.AxisXDiffDegree.ToString("F3");
                labelYDegree.Text = calData.AxisYDiffDegree.ToString("F3");
                labelFarFromMirror.Text = calData.FarFromMirrorSensorValue.ToString("F3");
                labelNearToMirrorLeft.Text = calData.NearToMirrorSensorLeftSideValue.ToString("F3");
                labelNearToMirrorRight.Text = calData.NearToMirrorSensorRightSideValue.ToString("F3");
                labelBeforeMirror.Text = calData.BeforeBlackMirrorValue.ToString("F3");
                labelAfterMirror.Text = calData.AfterBlackMirrorValue.ToString("F3");

                MessageForm.Show(null, "Calibration Complete", MessageFormType.OK);
            }
        }


        private void numericTopLeftPos_ValueChanged(object sender, EventArgs e)
        {
            GlossSettings.FarFromMirrorPosition = Convert.ToSingle(numericFarFromMirrorPos.Value);
            GlossSettings.Save();
        }
        
        private void numericBottomLeftPos_ValueChanged(object sender, EventArgs e)
        {
            GlossSettings.NearToMirrorLeftPosition = Convert.ToSingle(numericNearToMirrorLeftPos.Value);
            GlossSettings.Save();
        }

        private void numericBottomRightPos_ValueChanged(object sender, EventArgs e)
        {
            GlossSettings.NearToMirrorRightPosition = Convert.ToSingle(numericNearToMirrorRightPos.Value);
            GlossSettings.Save();
        }

        private void numericCalPos_ValueChanged(object sender, EventArgs e)
        {
            GlossSettings.CalibrationPosition = Convert.ToSingle(numericCalPos.Value)/* * 1000*/;
            GlossSettings.Save();
        }

        private void numericPositionOffset_ValueChanged(object sender, EventArgs e)
        {
            GlossSettings.PositionOffset = Convert.ToSingle(numericPositionOffset.Value);
            GlossSettings.Save();
        }


        private void buttonResetAlarm_MouseDown(object sender, MouseEventArgs e)
        {
            AxisHandler.ResetAlarmOn(true);
        }

        private void buttonResetAlarm_MouseUp(object sender, MouseEventArgs e)
        {
            AxisHandler.ResetAlarmOn(false);
            ErrorManager.Instance().ResetAllAlarm();
        }

        private void buttonMotionSpeed_Click(object sender, EventArgs e)
        {
            AxisConfiguration axisConfiguration = new AxisConfiguration();
            axisConfiguration = SystemManager.Instance().DeviceBox.AxisConfiguration;

            if (axisConfiguration.Count == 0)
                return;

            SimpleMotionSpeedForm form = new SimpleMotionSpeedForm();

            form.Intialize(axisConfiguration);

            if (form.ShowDialog() == DialogResult.OK)
            {
                axisConfiguration.LoadConfiguration();
            }
        }

        private void buttonServo_Click(object sender, EventArgs e)
        {
            AxisHandler.TurnOnServo(!(AxisHandler.IsServoOn()[0]));
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            if (AxisHandler == null)
                return;

            SimpleProgressForm form = new SimpleProgressForm("Homming...");
            form.Show(new Action(() =>
            {
                AxisHandler.HomeMove(cancellationTokenSource);
            }));
        }

        private void buttonMove_Click(object sender, EventArgs e)
        {
            AxisPosition movingPos = CalData.GetOffsetMoveAxisPosition(Convert.ToSingle(numericMotionPosition.Value));
            SimpleProgressForm form = new SimpleProgressForm(StringManager.GetString(GetType().FullName, "Move to Position..."));
            form.Show(new Action(() =>
            {
                AxisHandler.Move(0, movingPos, cancellationTokenSource);
            }));

            if (AxisHandler.AxisList[0].Motion.MotionType == MotionType.FastechEziMotionPlusR)
            {
                float position = CalData.GetOffsetPositionFromActualPosition(AxisHandler.GetActualPos().GetPosition()[0] / 1000f);
                ultraLabelPosition.Text = String.Format("{0:0.000}", position);
            }
        }

        private void buttonJogP_MouseDown(object sender, MouseEventArgs e)
        {
            if (GlossSettings.RevesePosition)
            {
                AxisHandler.StartContinuousMove(0, true);
            }
            else
            {
                AxisHandler.StartContinuousMove(0, false);
            }
        }

        private void buttonJogP_MouseUp(object sender, MouseEventArgs e)
        {
            AxisHandler.StopMove();

            if (AxisHandler.AxisList[0].Motion.MotionType == MotionType.FastechEziMotionPlusR)
            {
                float position = CalData.GetOffsetPositionFromActualPosition(AxisHandler.GetActualPos().GetPosition()[0] / 1000);
                ultraLabelPosition.Text = string.Format("{0:0.000}", position);
            }

            numericMotionPosition.Value = (decimal)Convert.ToSingle(ultraLabelPosition.Text);
        }

        private void buttonJogN_MouseDown(object sender, MouseEventArgs e)
        {
            if (GlossSettings.RevesePosition)
            {
                AxisHandler.StartContinuousMove(0, false);
            }
            else
            {
                AxisHandler.StartContinuousMove(0, true);
            }
        }

        private void buttonJogN_MouseUp(object sender, MouseEventArgs e)
        {
            AxisHandler.StopMove();

            if (AxisHandler.AxisList[0].Motion.MotionType == MotionType.FastechEziMotionPlusR)
            {
                float position = CalData.GetOffsetPositionFromActualPosition(AxisHandler.GetActualPos().GetPosition()[0] / 1000);
                ultraLabelPosition.Text = string.Format("{0:0.000}", position);
            }

            numericMotionPosition.Value = (decimal)Convert.ToSingle(ultraLabelPosition.Text);
        }

        private void comboBoxMotionPosition_ValueChanged(object sender, EventArgs e)
        {
            var control = sender as UltraComboEditor;
            switch ((GlossMotionPosition)Enum.Parse(typeof(GlossMotionPosition), control.SelectedItem.ToString()))
            {
                case GlossMotionPosition.Far: numericMotionPosition.Value = (decimal)GlossSettings.FarFromMirrorPosition; break;
                case GlossMotionPosition.NearLeft: numericMotionPosition.Value = (decimal)GlossSettings.NearToMirrorLeftPosition; break;
                case GlossMotionPosition.NearRight: numericMotionPosition.Value = (decimal)GlossSettings.NearToMirrorRightPosition; break;
                case GlossMotionPosition.Calibration: numericMotionPosition.Value = (decimal)GlossSettings.CalibrationPosition; break;
            }
        }


        private void btnIoSetting_Click(object sender, EventArgs e)
        {
            IOSettingForm iOSettingForm = new IOSettingForm();
            iOSettingForm.Show();
        }


        // Test
        private void btnOnceMeasure_Click(object sender, EventArgs e)
        {
            if (CheckingSpecialText(txtSampleName2.Text))
            {
                MessageBox.Show("특수문자를 제외해주세요");
                return;
            }

            if (IsOpen == false)
            {
                MessageBox.Show("Device를 Open 해주세요");
                return;
            }

            float[] datas = new float[1];
            serialSensor.GetData(1, datas);
            lblLaserValue.Text = datas[0].ToString();

            string result = UsbSensorGloss60.Measure().ToString();
            lblValue.Text = result;

            manualExporter.OpenFiles(CurrentPath);
            manualExporter.AppendHeader(ManualDataExporter.ManualType.OnceMeasure);
            manualExporter.AppendCSV(ManualDataExporter.ManualType.OnceMeasure, result, datas[0].ToString());
        }

        private void btnRepeat_Click(object sender, EventArgs e)
        {
            if (CheckingSpecialText(txtSampleName2.Text))
            {
                MessageBox.Show("특수문자를 제외해주세요");
                return;
            }

            repeatCnt = 0;
            if (IsOpen == false)
            {
                MessageBox.Show("Device를 Open 해주세요");
                return;
            }

            Task task = new Task(new Action(RepeatProc));
            task.Start();
        }

        private void btnLongRunStart_Click(object sender, EventArgs e)
        {
            //if (!CheckDeviceExist())
            //    return;

            if (isLongRunProc == true)
            {
                isLongRunProc = false;
            }
            else
            {
                isLongRunProc = true;
                Task task = new Task(new Action(ContinuousProc));
                task.Start();
            }
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                CurrentPath = folderBrowserDialog.SelectedPath;
                lblPath.Text = CurrentPath;
            }
        }

        private void Move_Click(object sender, EventArgs e)
        {
            if (AxisHandler == null)
                return;

            var inspectRunner = SystemManager.Instance().InspectRunner as InspectRunner;

            isLongRunProc = true;
            manualExporter.OpenFiles(CurrentPath);
            manualExporter.AppendHeader(ManualDataExporter.ManualType.Degree);

            while (isLongRunProc)
            {
                AxisHandler.HomeMove(null);

                startSensorValueList.Clear();
                endSensorValueList.Clear();

                startSensorValueList = inspectRunner.MeasureFarFromMirrorSensor();
                endSensorValueList = inspectRunner.MeasureNearToMirrorSensor();

                if (startSensorValueList == null || endSensorValueList == null)
                    return;

                float xDiff = Math.Max(startSensorValueList[0], endSensorValueList[0]) - Math.Min(startSensorValueList[0], endSensorValueList[0]);
                float yDiff = Math.Max(endSensorValueList[0], endSensorValueList[1]) - Math.Min(endSensorValueList[0], endSensorValueList[1]);

                float distance = Math.Max(GlossSettings.NearToMirrorLeftPosition, GlossSettings.NearToMirrorRightPosition) - Math.Min(GlossSettings.NearToMirrorLeftPosition, GlossSettings.NearToMirrorRightPosition);

                GlossSettings.AxisXDiffDegree = (float)((180.0 / Math.PI) * Math.Atan(xDiff / 124.2));
                GlossSettings.AxisYDiffDegree = (float)((180.0 / Math.PI) * Math.Atan(yDiff / distance));

                double MirrorValue = 0;

                if (chBoxMirrorMearue.Checked)
                    MirrorValue = inspectRunner.MeasureBlackMirror();

                inspectRunner.GlossMotionController.ChangeMovingSpeed(Convert.ToSingle(GlossSettings.MovingSpeed));
                manualExporter.AppendCSV(ManualDataExporter.ManualType.Degree, MirrorValue.ToString(), "", txtDegree.Text);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isLongRunProc = false;
        }
        #endregion


        #region 속성
        public string CurrentPath { get; set; } = @"D:\";

        private SerialDeviceHandler SerialDeviceHandler { get => SystemManager.Instance().DeviceBox.SerialDeviceHandler; }

        private AxisHandler AxisHandler { get => SystemManager.Instance().DeviceController.RobotStage; }

        private bool IsOpen { get => SystemManager.Instance().DeviceBox.DaqChannelList.Count > 0 ? (SystemManager.Instance().DeviceBox.DaqChannelList[0] != null ? true : false) : false; }

        private bool UseGloss { get => this.checkBoxGLoss.Checked; }

        private bool UseDistance { get => this.checkBoxDistance.Checked; }

        private bool UseUpdate { get => this.checkBoxUpdate.Checked; }

        private GlossSettings GlossSettings => GlossSettings.Instance();
        #endregion


        #region 메서드
        public void EnableControls(UserType userType)
        {
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            this.Visible = visibleFlag;
            if (visibleFlag == true)
            {
                positionTimer.Start();
                UpdateParameter();
            }
            else
            {
                positionTimer.Stop();
            }
        }

        public void UpdateControl(string item, object value)
        {
        }

        public bool CheckingSpecialText(string txt)
        {
            string str = @"[~!@\#$%^&*\()\=+|\\/:;?""<>']";
            System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(str);
            return rex.IsMatch(txt);
        }

        private void InitializeComboBox()
        {
            comboBoxMotionPosition.Items.Clear();
            comboBoxMotionPosition.DataSource = Enum.GetNames(typeof(GlossMotionPosition));
        }

        private void UpdateParameter()
        {
            numericBlackMirror.Value = Convert.ToDecimal(GlossSettings.BlackMirrorValue);
            numericFarFromMirrorPos.Value = Convert.ToDecimal(GlossSettings.FarFromMirrorPosition);
            numericNearToMirrorLeftPos.Value = Convert.ToDecimal(GlossSettings.NearToMirrorLeftPosition);
            numericNearToMirrorRightPos.Value = Convert.ToDecimal(GlossSettings.NearToMirrorRightPosition);
            numericCalPos.Value = Convert.ToDecimal(GlossSettings.CalibrationPosition);
            numericPositionOffset.Value = Convert.ToDecimal(GlossSettings.PositionOffset);
        }

        private void PositionTimer_Tick(object sender, EventArgs e)
        {
            if (AxisHandler != null)
            {
                if (AxisHandler.AxisList[0].Motion.MotionType != MotionType.FastechEziMotionPlusR)
                {
                    float position = CalData.GetOffsetPositionFromActualPosition(AxisHandler.GetActualPos().GetPosition()[0] / 1000);
                    ultraLabelPosition.Text = String.Format("{0:0.000}", position);
                }

                if (AxisHandler.IsOnError() == true)
                {
                    labelAlarm.Appearance.BackColor = Color.Red;
                }
                else
                {
                    labelAlarm.Appearance.BackColor = Color.Gainsboro;
                }

                if (AxisHandler.IsAllServoOn() == true)
                {
                    labelServo.Appearance.BackColor = Color.Lime;

                    buttonHome.Enabled = true;

                    if (AxisHandler.IsAllHomeDone() == true)
                    {
                        buttonMove.Enabled = true;
                        buttonJogP.Enabled = true;
                        buttonJogN.Enabled = true;
                    }
                    else
                    {
                        buttonMove.Enabled = false;
                        buttonJogP.Enabled = false;
                        buttonJogN.Enabled = false;
                    }
                }
                else
                {
                    labelServo.Appearance.BackColor = Color.Gainsboro;

                    buttonHome.Enabled = false;
                    buttonMove.Enabled = false;
                    buttonJogP.Enabled = false;
                    buttonJogN.Enabled = false;
                }
            }
        }

        private void RepeatProc()
        {
            string result = "";
            string name = txtSampleName.Text;

            manualExporter.OpenFiles(CurrentPath);
            manualExporter.AppendHeader(ManualDataExporter.ManualType.Repeat);

            while (true)
            {
                repeatCnt++;

                float[] datas = new float[1];
                serialSensor.GetData(1, datas);
                SetLaserText(datas[0].ToString());

                result = UsbSensorGloss60.Measure().ToString();
                SetGlossText(result);


                manualExporter.AppendCSV(ManualDataExporter.ManualType.Repeat, result, datas[0].ToString(), name, this.MeasureCnt.Value.ToString());

                //MeasureInfo measureInfo = new MeasureInfo(result, DateTime.Now, txtSampleName.Text, datas[0].ToString(), txtTilting.Text);
                //MeasureResultHandler.Instance.AddMeasure(measureInfo);

                if (repeatCnt == this.MeasureCnt.Value)
                {
                    MessageBox.Show("Measure Finish");
                    return;
                }
            }
        }

        private void SetLaserText(string text)
        {
            if (this.lblLaserValue.InvokeRequired)
            {
                SetLaserTextCallback d = new SetLaserTextCallback(SetLaserText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lblLaserValue.Text = text;
            }
        }

        private void SetGlossText(string text)
        {
            if (this.lblValue.InvokeRequired)
            {
                SetLaserTextCallback d = new SetLaserTextCallback(SetGlossText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lblValue.Text = text;
            }
        }

        private void SetGlossMaxText(string text)
        {
            if (this.lblGlossMax.InvokeRequired)
            {
                SetGlossMaxTextCallback d = new SetGlossMaxTextCallback(SetGlossMaxText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lblGlossMax.Text = text;
            }
        }

        private void ContinuousProc()
        {
            manualExporter.OpenFiles(CurrentPath);
            manualExporter.AppendHeader(ManualDataExporter.ManualType.LongRun);

            while (isLongRunProc == true)
            {
                float[] datas = new float[1];
                if (UseDistance)
                {
                    this.serialSensor.GetData(1, datas);
                    if (UseUpdate)
                        SetLaserText(datas[0].ToString());
                }

                string result = "0";
                if (UseGloss)
                {
                    result = UsbSensorGloss60.Measure().ToString();
                    if (UseUpdate)
                        SetGlossText(result);
                }

                if (UseUpdate)
                {
                    if (isClear)
                    {
                        maxGlossValue = 0.0;
                        isClear = false;
                    }
                    else
                        maxGlossValue = Math.Max(Convert.ToDouble(maxGlossValue), Convert.ToDouble(result));

                    SetGlossMaxText(maxGlossValue.ToString());
                }

                manualExporter.AppendCSV(ManualDataExporter.ManualType.LongRun, result, datas[0].ToString());
            }
        }

        private bool CheckDeviceExist()
        {
            if (UsbSensorGloss60.ListDevices() != null /*|| UsbSensorGloss60.Instance().ListDevices()?.Count() !=0*/)
                return true;
            else
                return false;
        }

        private bool CheckSampleName()
        {
            if (String.IsNullOrEmpty(txtSampleName2.Text))
            {
                MessageBox.Show("Sample이름을 입력해 주세요.");
                return false;
            }
            else
                return true;
        }
        #endregion
    }

    public enum GlossMotionPosition
    {
        Far, NearLeft, NearRight, Calibration
    }
}
