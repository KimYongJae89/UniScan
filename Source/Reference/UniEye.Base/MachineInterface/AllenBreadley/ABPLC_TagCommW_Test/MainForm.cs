using ABPLCTagCommW;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABPLC_TagCommW_Test
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
        }

        private delegate void SetEnableDelegate(Control control, bool enable);
        private void SetEnable(Control control, bool enable)
        {
            if (control == null)
                return;

            if (control.InvokeRequired)
            {
                Invoke(new SetEnableDelegate(SetEnable), control, enable);
                return;
            }

            control.Enabled = enable;
        }

        private delegate void SetTextDelegate(Control control, string text);
        private void SetText(Control control, string text)
        {
            if (control == null)
                return;

            if (control.InvokeRequired)
            {
                Invoke(new SetTextDelegate(SetText), control, text);
                return;
            }

            control.Text = text;
        }

        private void ExcuteCommand(object sender, MethodInvoker f)
        {
            Task.Run(() =>
            {
                Control control = sender as Control;
                try
                {
                    SetEnable(control, false);

                    f.Invoke();
                    AddList("OK");
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    AddList(ex.Message);
                }
                finally
                {
                    SetEnable(control, true);
                }
            });
        }

        private delegate void AddListDelegate(string str);
        private void AddList(string str)
        {
            if (this.listBox1.InvokeRequired)
            {
                Invoke(new AddListDelegate(AddList), str);
                return;
            }

            bool autoScroll = (this.listBox1.SelectedIndex == this.listBox1.Items.Count - 1);
            int idx = this.listBox1.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] {str}");
            if (autoScroll)
                this.listBox1.SelectedIndex = idx;
        }

        private void buttonPlcInit_Click(object sender, EventArgs e)
        {
            AddList("Initialize PLC");
            ExcuteCommand(this.groupBoxInit, new MethodInvoker(() =>
             {
                 string PLC_IPADDRESS = "192.168.1.110"; // PLC IP Address
                 string CPU_TYPE = "LGX"; // CPU TYPE
                 string PLC_PATH = "1,0"; // PLC Path

                 ABPLCW.InitPLC(PLC_IPADDRESS, CPU_TYPE, PLC_PATH, 0);
             }));
        }

        private void buttonPlcDispose_Click(object sender, EventArgs e)
        {
            AddList("Dispose PLC");
            ExcuteCommand(this.groupBoxInit, new MethodInvoker(() =>
            {
                ABPLCW.ReleasePLC();
            }));
        }

        private void buttonDintInit_Click(object sender, EventArgs e)
        {
            AddList("Initialize DINT Tag");
            ExcuteCommand(this.groupBoxDInt, new MethodInvoker(() =>
            {
                ABPLCW.Register(0, "c_PLCtoUniEye", sizeof(Int32), 100);
            }));
        }

        private void buttonDintDispose_Click(object sender, EventArgs e)
        {
            AddList("Dispose DINT Tag");
            ExcuteCommand(this.groupBoxDInt, new MethodInvoker(() =>
            {
                ABPLCW.UnRegister(0);
            }));
        }

        private void buttonDintRead_Click(object sender, EventArgs e)
        {
            AddList("DINT Read");
            ExcuteCommand(this.groupBoxDInt, new MethodInvoker(() =>
            {
                TextBox[] textBoxes = new TextBox[]
                {
                    textBoxDint0,textBoxDint1,textBoxDint2,textBoxDint3,textBoxDint4,
                    textBoxDint5,textBoxDint6,textBoxDint7,textBoxDint8,textBoxDint9
                };

                Int32[] values = new Int32[100];
                ABPLCW.ReadData(values, 0, values.Length, 500);

                for (int i = 0; i < Math.Min(values.Length, textBoxes.Length); i++)
                    SetText(textBoxes[i], values[i].ToString());
            }));
        }

        private void buttonDintWrite_Click(object sender, EventArgs e)
        {
            AddList("DINT Write");
            ExcuteCommand(this.groupBoxDInt, new MethodInvoker(() =>
            {
                TextBox[] textBoxes = new TextBox[]
                {
                    textBoxDint0,textBoxDint1,textBoxDint2,textBoxDint3,textBoxDint4,
                    textBoxDint5,textBoxDint6,textBoxDint7,textBoxDint8,textBoxDint9
                };

                for (int i = 0; i < textBoxes.Length; i++)
                {
                    Int32 data = 0;
                    Int32.TryParse(textBoxes[i].Text, out data);
                    ABPLCW.SetTagValue(0, i, sizeof(Int32), data);
                }

                ABPLCW.WriteData(0, 500);
            }));
        }

        private void buttonDebugConsolInit_Click(object sender, EventArgs e)
        {
            AddList("Initialize Consol");
            ExcuteCommand(this.groupBoxDebugConsol, new MethodInvoker(() =>
            {
                ABPLCW.InitConsol();
            }));
        }

        private void buttonDebugConsolRelease_Click(object sender, EventArgs e)
        {
            AddList("Release Consol");
            ExcuteCommand(this.groupBoxDebugConsol, new MethodInvoker(() =>
            {
                ABPLCW.DisposeConsol();
            }));
        }

        private void buttonRealInit_Click(object sender, EventArgs e)
        {
            AddList("Initialize REAL Tag");
            ExcuteCommand(this.groupBoxReal, new MethodInvoker(() =>
            {
                ABPLCW.Register(1, "REAL", sizeof(float), 1);
            }));
        }

        private void buttonRealDispose_Click(object sender, EventArgs e)
        {
            AddList("Dispose REAL Tag");
            ExcuteCommand(this.groupBoxReal, new MethodInvoker(() =>
            {
                ABPLCW.UnRegister(1);
            }));
        }

        private void buttonRealRead_Click(object sender, EventArgs e)
        {
            AddList("REAL Read");
            ExcuteCommand(this.groupBoxReal, new MethodInvoker(() =>
            {
                double[] values = new double[1] {10};
                ABPLCW.ReadData(values, 1, values.Length, 500);

                SetText(textBoxReal, values[0].ToString());
            }));
        }

        private void buttonRealWrite_Click(object sender, EventArgs e)
        {
            AddList("REAL Write");
            ExcuteCommand(this.groupBoxReal, new MethodInvoker(() =>
            {
                double data = 0;
                double.TryParse(textBoxReal.Text, out data);
                ABPLCW.SetTagValue(1, 0, sizeof(float), data);
                ABPLCW.WriteData(1, 500);
            }));
        }

        private void buttonStringInit_Click(object sender, EventArgs e)
        {
            AddList("Initialize STRING Tag");
            ExcuteCommand(this.groupBoxString, new MethodInvoker(() =>
            {
                ABPLCW.RegisterStr(2, "STRING");
            }));
        }

        private void buttonStringDispose_Click(object sender, EventArgs e)
        {
            AddList("Dispose STRING Tag");
            ExcuteCommand(this.groupBoxString, new MethodInvoker(() =>
            {
                ABPLCW.UnRegister(2);
            }));
        }

        private void buttonStringRead_Click(object sender, EventArgs e)
        {
            AddList("STRING Read");
            ExcuteCommand(this.groupBoxString, new MethodInvoker(() =>
            {
                string str = ABPLCW.ReadString(2, 500);
                SetText(textBoxString, str);
            }));
        }

        private void buttonStringWrite_Click(object sender, EventArgs e)
        {
            AddList("STRING Write");
            ExcuteCommand(this.groupBoxString, new MethodInvoker(() =>
            {
                ABPLCW.WriteString(2, 500, textBoxString.Text);
            }));
        }
    }
}
