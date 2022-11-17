using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using DynMvp.UI.Touch;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlphaMotionMove
{
    public partial class MainForm : Form
    {
        Settings configuration;
        Motion motion = null;

        public int AutoStopCntTimes { get; set; } = 0;

        bool autoStartReq = false;
        bool autoStopReq = false;
        TimeSpan runningTimeSpan = TimeSpan.Zero;
        Stopwatch runningStopwatch = null;
        Stopwatch pauseStopwatch = null;
        int runningTimes = 0;
        float runningDist = 0;

        ThreadHandler workThreadHandler = null;

        public MainForm()
        {
            InitializeComponent();

            this.runningStopwatch = new Stopwatch();
            this.pauseStopwatch = new Stopwatch();

            this.configuration = new Settings();
            this.configuration.Load();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                MotionInfo motionInfo = this.configuration.MotionInfo;
                this.motion = MotionFactory.Create(motionInfo);
                bool ok = motion.Initialize(motionInfo);
                if (!ok)
                {
                    MessageBox.Show("Motion Initialize Fail!!");
                    Close();
                    return;
                }

                this.groupBox1.Text = motionInfo.Type.ToString();

                if(this.motion is IDigitalIo)
                {
                    IDigitalIo digitalIo = (IDigitalIo)this.motion;
                    int numOutPort = digitalIo.GetNumOutPort();
                    string format = string.Format("D0{0}", numOutPort / 10 + 1);

                    string[] strings = new string[numOutPort];
                    for (int i = 0; i < numOutPort; i++)
                        strings[i] = string.Format("DO {0}", i.ToString(format));

                    this.infoIoList.Items.Clear();
                    this.infoIoList.Items.AddRange(strings);

                    WriteIoH(false);
                    WriteIoL(false);
                }
                
                UpdatePicture(Properties.Resources.Idle);
            }
            catch (DllNotFoundException ex)
            {
                string message = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
                MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            UpdateControls();
            this.timerUiUpdate.Start();
        }

        private void UpdateControls()
        {
            SetBinding(this.startPosition, int.MinValue, int.MaxValue, new Binding("Value", this.configuration.Departure, "PositionMm", false, DataSourceUpdateMode.OnPropertyChanged));
            SetBinding(this.startDelay, 0, int.MaxValue, new Binding("Value", this.configuration.Departure, "DelayMs", false, DataSourceUpdateMode.OnPropertyChanged));
            SetBinding(this.startIo, new Binding("Checked", this.configuration.Departure, "IoActive", false, DataSourceUpdateMode.OnPropertyChanged));

            SetBinding(this.forwardSpd, 0, int.MaxValue, new Binding("Value", this.configuration.Forward, "VelocityMmps", false, DataSourceUpdateMode.OnPropertyChanged));
            SetBinding(this.forwardAcc, 0, int.MaxValue, new Binding("Value", this.configuration.Forward, "AccelationMs", false, DataSourceUpdateMode.OnPropertyChanged));
            SetBinding(this.forwardIo, new Binding("Checked", this.configuration.Forward, "IoActive", false, DataSourceUpdateMode.OnPropertyChanged));

            SetBinding(this.endPosition, int.MinValue, int.MaxValue, new Binding("Value", this.configuration.Arrival, "PositionMm", false, DataSourceUpdateMode.OnPropertyChanged));
            SetBinding(this.endDelay, int.MinValue, int.MaxValue, new Binding("Value", this.configuration.Arrival, "DelayMs", false, DataSourceUpdateMode.OnPropertyChanged));
            SetBinding(this.endIo, new Binding("Checked", this.configuration.Arrival, "IoActive", false, DataSourceUpdateMode.OnPropertyChanged));

            SetBinding(this.backwardSpd, 0, int.MaxValue, new Binding("Value", this.configuration.Backward, "VelocityMmps", false, DataSourceUpdateMode.OnPropertyChanged));
            SetBinding(this.backwardAcc, 0, int.MaxValue, new Binding("Value", this.configuration.Backward, "AccelationMs", false, DataSourceUpdateMode.OnPropertyChanged));
            SetBinding(this.backwardIo, new Binding("Checked", this.configuration.Backward, "IoActive", false, DataSourceUpdateMode.OnPropertyChanged));

            SetBinding(this.useStopIn, new Binding("Checked", this.configuration.StopIn, "Use", false, DataSourceUpdateMode.OnPropertyChanged));
            SetBinding(this.stopInTime, 0, int.MaxValue, new Binding("Value", this.configuration.StopIn, "TimeSec", false, DataSourceUpdateMode.OnPropertyChanged));

            SetBinding(this.useRunIn, new Binding("Checked", this.configuration.RunIn, "Use", false, DataSourceUpdateMode.OnPropertyChanged));
            SetBinding(this.runInTime, 0, int.MaxValue, new Binding("Value", this.configuration.RunIn, "TimeSec", false, DataSourceUpdateMode.OnPropertyChanged));

            this.autoStopCnt.DataBindings.Add(new Binding("Text", this, "AutoStopCntTimes", false, DataSourceUpdateMode.OnPropertyChanged));
        }

        private void SetBinding(NumericUpDown numericUpDown, decimal min, decimal max, Binding binding)
        {
            numericUpDown.Minimum = min;
            numericUpDown.Maximum = max;
            numericUpDown.DataBindings.Clear();
            numericUpDown.DataBindings.Add(binding);

            // new Binding("Value", this.configuration.Departure, "DelayMs")
        }

        private void SetBinding(CheckBox checkBox, Binding binding)
        {
            checkBox.DataBindings.Clear();
            checkBox.DataBindings.Add(binding);
        }

        private void timerUiUpdate_Tick(object sender, EventArgs e)
        {
            MotionStatus motionState = motion.GetMotionStatus(0);
            if (motionState.alarm)
                this.buttonEMG.BackColor = Color.Red;
            else
                this.buttonEMG.BackColor = Control.DefaultBackColor;

            if (motionState.servoOn)
                this.buttonServo.BackColor = Color.LightGreen;
            else
                this.buttonServo.BackColor = Control.DefaultBackColor;

            if (workThreadHandler != null)
                buttonMove.Text = "Stop";
            else
                buttonMove.Text = "Move";

            uint dioValue = 0;
            string dioValueFormat = "X0";
            if (motion is IDigitalIo)
            {
                IDigitalIo digitalIo = (IDigitalIo)motion;
                dioValue = digitalIo.ReadOutputGroup(0);
                int numOutPort = digitalIo.GetNumOutPort();
                dioValueFormat = string.Format("X{0}", numOutPort / 4);

                for (int i = 0; i < numOutPort; i++)
                {
                    bool value = ((dioValue >> i) & 0x1) == 0x1;
                    infoIoList.SetItemChecked(i, value);
                }

                this.infoIo.Text = string.Format("0x {0}", dioValue.ToString(dioValueFormat));
            }

            TimeSpan ts = this.runningStopwatch.IsRunning ? this.runningStopwatch.Elapsed : this.runningTimeSpan;
            this.infoTime.Text = string.Format("{0:00}H {1:00}m {2:00}s", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
            this.infoTimes.Text = this.runningTimes.ToString();
            this.infoDistance.Text = this.runningDist.ToString("F03");
            this.infoSpeed.Text = (this.runningDist / ts.TotalMinutes).ToString("F03");

            if (this.configuration.StopIn.Use)
            {
                double totalRunningSec = this.runningStopwatch.Elapsed.TotalSeconds;
                if (totalRunningSec >= this.configuration.StopIn.TimeSec)
                {
                    if (!autoStopReq)
                    {
                        autoStopReq = true;
                        autoStartReq = false;
                        Stop();
                        this.AutoStopCntTimes++;
                        this.autoStopCnt.DataBindings[0].ReadValue();
                    }
                }
            }

            if (this.configuration.RunIn.Use)
            {
                double totalPauseSec = this.pauseStopwatch.Elapsed.TotalSeconds;
                if (totalPauseSec >= this.configuration.RunIn.TimeSec)
                {
                    if (!autoStartReq)
                    {
                        autoStartReq = true;
                        autoStopReq = false;
                        Run();
                    }
                }
            }

            if (this.pauseStopwatch.IsRunning || this.runningStopwatch.IsRunning)
            {
                labelRunning.Visible = true;
                int sec = this.pauseStopwatch.Elapsed.Seconds + this.runningStopwatch.Elapsed.Seconds;
                labelRunning.ForeColor = (sec % 2 == 0) ? Color.Red : Color.LightPink;
            }
            else
            {
                labelRunning.ForeColor = SystemColors.Control;
                labelRunning.Visible = false;
            }
        }

        private void buttonServo_Click(object sender, EventArgs e)
        {
            MotionStatus motionState = motion.GetMotionStatus(0);
            motion.TurnOnServo(!motionState.servoOn);
        }

        private HomeParam GetHomingParam()
        {
            HomeParam homeParam = new HomeParam();
            homeParam.HomeDirection = MoveDirection.CCW;
            homeParam.HomeMode = HomeMode.HomeSensor;
            homeParam.HighSpeed = new MovingParam("", 100, 100, 100, 10000, 0);
            homeParam.MediumSpeed = new MovingParam("", 100, 100, 100, 5000, 0);
            homeParam.FineSpeed = new MovingParam("", 100, 100, 100, 1000, 0);

            return homeParam;
        }

        private delegate uint GetDioValueDelegate(ComboBox comboBox);
        private uint GetDioValue(ComboBox comboBox)
        {
            if(InvokeRequired)
            {
                return (uint)Invoke(new GetDioValueDelegate(GetDioValue), comboBox);
            }
            return (uint)(comboBox.SelectedIndex == 0 ? 0xffff : 0x0000);
        }

        private void buttonMove_Click(object sender, EventArgs e)
        {
            if (workThreadHandler == null)
            {
                this.AutoStopCntTimes = 0;
                this.autoStopCnt.DataBindings[0].ReadValue();
                autoStartReq = true;
                autoStopReq = false;
                Run();
            }
            else
            {
                Stop();
                this.pauseStopwatch.Reset();
                this.runningStopwatch.Reset();
            }
        }

        private void Run()
        {
            if (workThreadHandler != null)
                return;

            MotionStatus motionState = motion.GetMotionStatus(0);
            if (motionState.homeOk == false)
            {
                bool ok = Homeing(true);
                motion.StopMove();
                if (ok == false)
                    return;
            }

            workThreadHandler = new ThreadHandler("WorkThread", new Thread(WorkThreadProc));
            workThreadHandler.Start();
        }

        private void Stop()
        {
            workThreadHandler?.Stop();
            workThreadHandler = null;
        }

        private void WorkThreadProc()
        {
            this.pauseStopwatch.Reset();

            this.runningTimes = 0;
            this.runningDist = 0;
            this.runningTimeSpan = TimeSpan.Zero;

            this.runningStopwatch.Start();
            int timedelay = 350; // 초기 거리가 짧을때 거리에 따라 슬립이 더작아야하지만 무시...
            try
            {
                WriteIoH(true);
                while (true)
                {
                    // 역방향  
                    UpdatePicture(Properties.Resources.Backward); //역방향 움직이는 중.
                    MoveMotion(this.configuration.Departure, this.configuration.Backward);//------------------
                    WriteIoL(this.configuration.Backward.IoActive);
                    //Thread.Sleep(this.configuration.Backward.AccelationMs);

                    // 도착 기다림
                    WaitMotionDone();

                    Thread.Sleep(timedelay);

                    //출발지 도착
                    UpdatePicture(Properties.Resources.Departure);
                    WriteIoL(this.configuration.Departure.IoActive);
                    Thread.Sleep(this.configuration.Departure.DelayMs);                    

                    if (workThreadHandler.RequestStop)
                        break;

                    //정방향
                    UpdatePicture(Properties.Resources.Forward);
                    MoveMotion(this.configuration.Arrival, this.configuration.Forward);//------------------------
                    WriteIoL(this.configuration.Forward.IoActive);
                    //Thread.Sleep(this.configuration.Forward.AccelationMs);

                    Thread.Sleep(timedelay);

                    WaitMotionDone();

                    //WriteIoL(this.configuration.Arrival.IoActive);

                    this.runningDist += (float)Math.Abs(this.configuration.Arrival.PositionMm - this.configuration.Departure.PositionMm) / 1000;
                    this.runningTimes++;

                    //목적지 도착
                    UpdatePicture(Properties.Resources.Arrival);
                    WriteIoL(this.configuration.Arrival.IoActive);
                    Thread.Sleep(this.configuration.Arrival.DelayMs);
                }
                WriteIoH(false);
            }
            finally
            {
                UpdatePicture(Properties.Resources.Idle);
                this.runningTimeSpan = this.runningStopwatch.Elapsed;
                this.runningStopwatch.Reset();
                this.pauseStopwatch.Start();
            }
        }

        private void UpdatePicture(Image image)
        {
            if (InvokeRequired)
                this.pictureBox1.BeginInvoke(new MethodInvoker(() => UpdatePicture(image)));
            else
                this.pictureBox1.Image = image;
        }

        private void MoveMotion(_Position position, _Direction direction)
        {
            this.motion.StartMove(0, position.PositionMm * 1000, direction.GetMovingParam());
        }

        private void WaitMotionDone()
        {
            while (this.motion.IsMoveDone(0) == false)
                Thread.Sleep(10);
        }

        private void ClearIo()
        {
            if (motion is IDigitalIo)
            {
                IDigitalIo digitalIo = (IDigitalIo)motion;
                digitalIo.WriteOutputGroup(0, 0x0000);
            }
        }

        private void WriteIoL(bool active)
        {
            if (motion is IDigitalIo)
            {
                IDigitalIo digitalIo = (IDigitalIo)motion;
                int numOutPort = digitalIo.GetNumOutPort() / 2;

                uint curValue = digitalIo.ReadOutputGroup(0);
                uint newValue;
                string data = new string('0', numOutPort / 4);
                if(active)
                {
                    data = new string('0', numOutPort / 4) + new string('F', numOutPort / 4);
                    newValue = curValue | uint.Parse(data, System.Globalization.NumberStyles.AllowHexSpecifier);
                }
                else
                {
                    data = new string('F', numOutPort / 4) + new string('0', numOutPort / 4);
                    newValue = curValue & uint.Parse(data, System.Globalization.NumberStyles.AllowHexSpecifier);
                }

                digitalIo.WriteOutputGroup(0, newValue);
            }
        }


        private void WriteIoH(bool active)
        {
            if (motion is IDigitalIo)
            {
                IDigitalIo digitalIo = (IDigitalIo)motion;
                int numOutPort = digitalIo.GetNumOutPort() / 2;

                uint curValue = digitalIo.ReadOutputGroup(0);
                uint newValue;
                string data;
                if (active)
                {
                    data = new string('F', numOutPort / 4) + new string('0', numOutPort / 4);
                    newValue = curValue | uint.Parse(data, System.Globalization.NumberStyles.AllowHexSpecifier);
                }
                else
                {
                    data = new string('0', numOutPort / 4) + new string('F', numOutPort / 4);
                    newValue = curValue & uint.Parse(data, System.Globalization.NumberStyles.AllowHexSpecifier);
                }

                digitalIo.WriteOutputGroup(0, newValue);
            }
        }


        private bool Homeing(bool userQuary)
        {
            bool moveHome = true;
            if (userQuary)
            {
                DialogResult dialogResult = MessageForm.Show(null, "Homeing?", MessageFormType.YesNo);
                moveHome = (dialogResult == DialogResult.Yes);
            }

            if (moveHome)
            {
                HomeParam homeParam = GetHomingParam();
                SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                bool result = false;
                simpleProgressForm.Show(() =>
                {
                    Stopwatch sw = new Stopwatch();
                    motion.StartHomeMove(0, homeParam);
                    while (true)
                    {
                        bool isHomeDone = motion.IsHomeDone(0);
                        bool isMoveDone = motion.IsMoveDone(0);
                        if (isHomeDone)
                        {
                            result = true;
                            break;
                        }
                        else if (isMoveDone)
                        {
                            if (sw.IsRunning == false)
                                sw.Restart();
                            else if (sw.ElapsedMilliseconds > 1000)
                            {
                                sw.Stop();
                                result = isHomeDone;
                                break;
                            }
                        }

                        if (cancellationTokenSource.IsCancellationRequested)
                        {
                            result = false;
                            break;
                        }
                        Thread.Sleep(100);
                    }
                }, cancellationTokenSource);
                    motion.StopMove(0);
                return result;
            }
            else
            {
                return false;
            }

        }

        private void buttonEMG_Click(object sender, EventArgs e)
        {
            EMG();

            this.pauseStopwatch.Reset();
            this.runningStopwatch.Reset();
        }

        private void EMG()
        {
            if (workThreadHandler != null)
            {
                this.runningStopwatch.Stop();
                workThreadHandler.Abort();
                workThreadHandler = null;
            }

            motion.StopMove();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.workThreadHandler?.Stop();
            this.configuration.Save();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int index = e.Index;
            bool isChecked = e.NewValue == CheckState.Checked;

            IDigitalIo digitalIo = (IDigitalIo)this.motion;
            digitalIo.WriteOutputPort(0, index, isChecked);
        }
    }
}
