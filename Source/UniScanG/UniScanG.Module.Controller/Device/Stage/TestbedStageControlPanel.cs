using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Devices.Light;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.Dio;
using DynMvp.UI;
using UniScanG;

namespace UniScanG.Module.Controller.Device.Stage
{
    public partial class TestbedStageControlPanel : UserControl
    {
        Timer updateTimer = null;
        List<Tuple<IoPort, Control, Control>> portVisibleMap = null;
        bool onUpdate = false;

        public TestbedStageControlPanel(PortMapBase portMapBase)
        {
            InitializeComponent();
            
            this.portVisibleMap = new List<Tuple<IoPort, Control, Control>>();
            ;
            List<IoPort> ioPortList = portMapBase.InPortList.IoPortList;
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetInPort(PortMap.IoPortName.InEmergency),this.labelIoInputEmgPush, this.labelIoInputEmgRelease));
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetInPort(PortMap.IoPortName.InDoorOpenL),this.labelIoInputDoorOpenL, null));
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetInPort(PortMap.IoPortName.InDoorOpenR),this.labelIoInputDoorOpenR, null));
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetInPort(PortMap.IoPortName.InAirPressure),this.labelIoInputAirGood, this.labelIoInputAirNotgood));

            List<IoPort> outPortList = portMapBase.OutPortList.IoPortList;
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetOutPort(PortMap.IoPortName.OutDoorOpen),this.buttonIoOutputDoorLock, this.buttonIoOutputDoorUnlock));
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetOutPort(PortMap.IoPortName.OutIonizer),this.buttonIoOutputIonizerOn, this.buttonIoOutputIonizerOff));
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetOutPort(PortMap.IoPortName.OutIonizerSol),this.buttonIoOutputIonizerSolOn, this.buttonIoOutputIonizerSolOff));
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetOutPort(PortMap.IoPortName.OutVaccumOn),this.buttonIoOutputVaccumOn, this.buttonIoOutputVaccumOff));
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetOutPort(PortMap.IoPortName.OutAirFan),this.buttonIoOutputFanOn, this.buttonIoOutputFanOff));
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetOutPort(PortMap.IoPortName.OutRoomLight),this.buttonIoOutputLightOn, this.buttonIoOutputLightOff));
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetOutPort(PortMap.IoPortName.OutTowerRed),this.buttonIoOutputTowerRed, null));
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetOutPort(PortMap.IoPortName.OutTowerYellow),this.buttonIoOutputTowerYellow, null));
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetOutPort(PortMap.IoPortName.OutTowerGreen),this.buttonIoOutputTowerGreen, null));
            portVisibleMap.Add(new Tuple<IoPort, Control, Control>(portMapBase.GetOutPort(PortMap.IoPortName.OutTowerBuzzer),this.buttonIoOutputTowerBuzzer, null));

            portVisibleMap.RemoveAll(f => f.Item1 == null);
        }

        private void LightSettingPanel_Load(object sender, EventArgs e)
        {
            this.onUpdate = true;
            float[] baseSpeed = new float[] { 10, 20, 40, 60, 80, 100, 120 };
            Array.ForEach(baseSpeed, f => convayorSpeed.Items.Add(f));
            convayorSpeed.FormatString = "F1";

            TestbedStage testbedStage = ((DeviceController)SystemManager.Instance().DeviceController).TestbedStage;
            if (testbedStage != null)
            {
                float curVelMpm = testbedStage.GetCommandVelMpm();
                convayorSpeed.Text = curVelMpm.ToString("F1");
            }
            this.onUpdate = false;

            this.updateTimer = new Timer();
            this.updateTimer.Interval = 200;
            this.updateTimer.Tick += UpdateTimer_Tick;
            this.updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            this.updateTimer.Stop();
            if (this.Disposing || this.IsDisposed)
                return;

            UpdateData();
            this.updateTimer.Start();
        }

        public void UpdateData()
        {
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.Convayor;
            bool useable = (axisHandler != null && Array.TrueForAll(axisHandler.IsAmpFault(), f => !f));

            convayorSpeed.Enabled = radioConvayorForward.Enabled = radioConvayorBackward.Enabled = useable;
            buttonConvayorRun.Enabled = buttonConvayorStop.Enabled = useable;
            if (useable)
            {
                TestbedStage testbedStage = ((DeviceController)SystemManager.Instance().DeviceController).TestbedStage;

                float actualVelMpm = testbedStage.GetActualVelMpm();
                float commandVelMpm = testbedStage.GetCommandVelMpm();

                labelConvayorSpeedUnit.Text = actualVelMpm.ToString("F1");
                convayorSpeed.Text = commandVelMpm.ToString("F1");
            }
            //convayor.GetActualPos
            //AxmStatusReadVel 

            DigitalIoHandler digitalIoHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;

            foreach (Tuple<IoPort, Control, Control> pair in this.portVisibleMap)
            {
                Color color1, color2;
                string ioPortName = pair.Item1.Name;
                IoPort ioInPort = SystemManager.Instance().DeviceBox.PortMap.GetInPort(ioPortName);
                if (ioInPort != null)
                {
                    if (ioInPort.PortNo == IoPort.UNUSED_PORT_NO)
                    {
                        color1 = color2 = SystemColors.Control;
                    }
                    else
                    {
                        bool isActive = SystemManager.Instance().DeviceController.IoMonitor.CheckInput(ioInPort);
                        if (isActive)
                        {
                            color1 = Color.LightGreen;
                            color2 = SystemColors.Control;
                        }
                        else
                        {
                            color1 = SystemColors.Control;
                            color2 = Color.LightGreen;
                        }
                    }
                    UpdateControl(pair.Item2, color1);
                    UpdateControl(pair.Item3, color2);
                }

                IoPort ioOutPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(ioPortName);
                if (ioOutPort != null)
                {
                    if (ioOutPort.PortNo == IoPort.UNUSED_PORT_NO)
                    {
                        color1 = color2 = SystemColors.Control;
                    }
                    else
                    {
                        bool isActive = SystemManager.Instance().DeviceController.IoMonitor.CheckOutput(ioOutPort);
                        if (isActive)
                        {
                            color1 = Color.LightGreen;
                            color2 = SystemColors.Control;
                        }
                        else
                        {
                            color1 = SystemColors.Control;
                            color2 = Color.LightGreen;
                        }
                    }
                    UpdateControl(pair.Item2, color1);
                    UpdateControl(pair.Item3, color2);
                }

            }
        }

        private delegate void UpdateControlDelegate(Control control, Color bgColor);
        private void UpdateControl(Control control, Color bgColor)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateControlDelegate(UpdateControl), control, bgColor);
                return;
            }

            if (control != null)
                control.BackColor = bgColor;
        }

        private void buttonIoOutputDoor_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            IoPort ioPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort("OutDoorOpen");
            SetOutput(ioPort, button.Name == buttonIoOutputDoorLock.Name);
        }

        private void buttonIoOutputIonizer_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            IoPort ioPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort("OutIonizer");
            SetOutput(ioPort, button.Name == buttonIoOutputIonizerOn.Name);
        }

        private void buttonIoOutputIonizerSol_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            IoPort ioPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort("OutIonizerSol");
            SetOutput(ioPort, button.Name == buttonIoOutputIonizerSolOn.Name);
        }

        private void buttonIoOutputVaccum_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            IoPort ioPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort("OutVaccumOn");
            SetOutput(ioPort, button.Name == buttonIoOutputVaccumOn.Name);
        }

        private void buttonIoOutputFan_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            IoPort ioPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort("OutAirFan");
            SetOutput(ioPort, button.Name == buttonIoOutputFanOn.Name);
        }

        private void buttonIoOutputLight_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            IoPort ioPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort("OutRoomLight");
            SetOutput(ioPort, button.Name == buttonIoOutputLightOn.Name);
        }

        private void buttonIoOutputTower_Click(object sender, EventArgs e)
        {

        }

        private void SetOutput(IoPort ioPort, bool setActive)
        {
            if (setActive)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputActive(ioPort);
            else
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputDeactive(ioPort);
        }

        private void buttonConvayorRun_Click(object sender, EventArgs e)
        {
            ((DeviceController)SystemManager.Instance().DeviceController).TestbedStage?.UpdateAxisHandler(true);

            //AxisHandler convayor = SystemManager.Instance().DeviceController.Convayor;
            //convayor.ContinuousMove(null, radioConvayorBackward.Checked);
        }

        private void buttonConvayorStop_Click(object sender, EventArgs e)
        {
            ((DeviceController)SystemManager.Instance().DeviceController).TestbedStage?.UpdateAxisHandler(false);
            //AxisHandler convayor = SystemManager.Instance().DeviceController.Convayor;
            //convayor.StopMove();
        }

        private void MachineSettingPanel_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void convayorSpeed_TextChanged(object sender, EventArgs e)
        {
            if (this.onUpdate)
                return;

            float mpm = 0;
            if (float.TryParse(convayorSpeed.Text, out mpm))
            {
                if (mpm > 0)
                {
                    ((DeviceController)SystemManager.Instance().DeviceController).TestbedStage?.SetConvSpeed(mpm);
                }
            }
        }
    }
}
