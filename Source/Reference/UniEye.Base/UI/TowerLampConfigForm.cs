using System;
using System.Collections.Generic;
using System.Windows.Forms;

using UniEye.Base.Device;

namespace UniEye.Base.UI
{
    public partial class TowerLampConfigForm : System.Windows.Forms.Form
    {
        TowerLamp towerLamp = null;

        public TowerLamp TowerLamp
        {
            get { return towerLamp; }
            set { towerLamp = value; }
        }

        public TowerLampConfigForm()
        {
            InitializeComponent();
        }

        private void TowerLampConfigForm_Load(object sender, EventArgs e)
        {
           // towerLampStateView.Rows.Clear();
           // towerLampStateView.Columns.Clear();

           //towerLampStateView.Columns.Add("State", "State");
           // towerLampStateView.Columns.Add("Green On", "Green On");
           // towerLampStateView.Columns.Add("Green Blink", "Green Blink");
           // towerLampStateView.Columns.Add("Yellow On", "Yellow On");
           // towerLampStateView.Columns.Add("Yellow Blink", "Yellow Blink");
           // towerLampStateView.Columns.Add("Red On", "Red On");
           // towerLampStateView.Columns.Add("Red Blink", "Red Blink");
           // towerLampStateView.Columns.Add("Buzzer On", "Buzzer On");
           // towerLampStateView.Columns.Add("Buzzer Blink", "Buzzer Blink");

            List<TowerLampState> stateList = towerLamp.TowerLampStateList;
            foreach (TowerLampState state in stateList)
            {
                int i = towerLampStateView.Rows.Add(state.Type.ToString());
                towerLampStateView.Rows[i].Cells[1].Value = state.GreenLamp.Value;
                towerLampStateView.Rows[i].Cells[2].Value = state.GreenLamp.Blink;
                towerLampStateView.Rows[i].Cells[3].Value = state.YellowLamp.Value;
                towerLampStateView.Rows[i].Cells[4].Value = state.YellowLamp.Blink;
                towerLampStateView.Rows[i].Cells[5].Value = state.RedLamp.Value;
                towerLampStateView.Rows[i].Cells[6].Value = state.RedLamp.Blink;
                towerLampStateView.Rows[i].Cells[7].Value = state.Buzzer.Value;
                towerLampStateView.Rows[i].Cells[8].Value = state.Buzzer.Blink;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            towerLamp.TowerLampStateList.Clear();
            foreach (DataGridViewRow row in towerLampStateView.Rows)
            {
                TowerLampStateType type = (TowerLampStateType)Enum.Parse(typeof(TowerLampStateType), row.Cells[0].Value.ToString());

                bool greenOn = Convert.ToBoolean(row.Cells[1].Value.ToString());
                bool greenBlank = Convert.ToBoolean(row.Cells[2].Value.ToString());

                bool yellowOn = Convert.ToBoolean(row.Cells[3].Value.ToString());
                bool yellowBlank = Convert.ToBoolean(row.Cells[4].Value.ToString());

                bool redOn = Convert.ToBoolean(row.Cells[5].Value.ToString());
                bool redBlank = Convert.ToBoolean(row.Cells[6].Value.ToString());

                bool buzzerOn = Convert.ToBoolean(row.Cells[7].Value.ToString());
                bool buzzerBlank = Convert.ToBoolean(row.Cells[8].Value.ToString());

                towerLamp.TowerLampStateList.Add(new TowerLampState(type, new Lamp(redOn, redBlank), new Lamp(yellowOn, yellowBlank), new Lamp(greenOn, greenBlank), new Lamp(buzzerOn, buzzerBlank)));
            }
        }
    }
}
