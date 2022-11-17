using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Device.Device.Light.SerialLigth.iCore
{
    public partial class iCoreConfigForm : Form
    {
        SerialLightCtrlInfoIPulse serialLightInfoIPulse;

        public iCoreConfigForm(SerialLightCtrlInfoIPulse serialLightInfoIPulse)
        {
            InitializeComponent();

            DynMvp.UI.UiHelper.SetNumericMinMax(this.slaveId, byte.MinValue, byte.MaxValue);
            DynMvp.UI.UiHelper.SetNumericMinMax(this.maxVoltage, ushort.MinValue, ushort.MaxValue);

            this.opMode.DataSource = Enum.GetValues(typeof(OperationMode));

            this.serialLightInfoIPulse = serialLightInfoIPulse;
        }

        private void iCoreConfigForm_Load(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            DynMvp.UI.UiHelper.SetNumericValue(this.slaveId, serialLightInfoIPulse.SlaveId);
            opMode.SelectedItem = serialLightInfoIPulse.OperationMode;
            DynMvp.UI.UiHelper.SetNumericValue(this.maxVoltage, serialLightInfoIPulse.MaxVoltage);
            DynMvp.UI.UiHelper.SetNumericValue(this.timeDuration, (decimal)serialLightInfoIPulse.TimeDuration);
            DynMvp.UI.UiHelper.SetCheckBoxChecked(this.lowPassFilter, serialLightInfoIPulse.LPFMode);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            serialLightInfoIPulse.SlaveId = (byte)this.slaveId.Value;
            serialLightInfoIPulse.OperationMode = (OperationMode)opMode.SelectedItem;
            serialLightInfoIPulse.MaxVoltage = (ushort)this.maxVoltage.Value;
            serialLightInfoIPulse.TimeDuration = (float)this.timeDuration.Value;
            serialLightInfoIPulse.LPFMode = this.lowPassFilter.Checked;

            Close();
        }

    }
}
