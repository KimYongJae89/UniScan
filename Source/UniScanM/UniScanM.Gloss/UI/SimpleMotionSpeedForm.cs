using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.UI
{
    public partial class SimpleMotionSpeedForm : Form, IMultiLanguageSupport
    {
        private AxisConfiguration axisConfiguration;
        public AxisConfiguration AxisConfiguration { get => axisConfiguration; set => axisConfiguration = value; }

        public SimpleMotionSpeedForm()
        {
            InitializeComponent();

            StringManager.AddListener(this);
        }

        public void Intialize(AxisConfiguration axisConfiguration)
        {
            this.AxisConfiguration = axisConfiguration;
        }

        private void SimpleMotionSpeedForm_Load(object sender, EventArgs e)
        {
            AxisParam axisParam = axisConfiguration[0][0].AxisParam;

            homeStartSpeed.Value = (decimal)(Math.Max(axisParam.HomeSpeed.HighSpeed.MaxVelocity / 1000 * axisParam.MicronPerPulse, 1));
            homeEndSpeed.Value = (decimal)(Math.Max(axisParam.HomeSpeed.FineSpeed.MaxVelocity / 1000 * axisParam.MicronPerPulse, 1));
            jogSpeed.Value = (decimal)(Math.Max(axisParam.JogParam.MaxVelocity / 1000 * axisParam.MicronPerPulse, 1));
            movingSpeed.Value = (decimal)(Math.Max(axisParam.MovingParam.MaxVelocity / 1000 * axisParam.MicronPerPulse, 1));
            refMovingSpeed.Value = (decimal)/*(Math.Max(axisParam.MovingParam.MaxVelocity / 1000 * axisParam.MicronPerPulse, 1) -*/ GlossSettings.Instance().ReferenceMovingSpeed;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AxisParam axisParam = axisConfiguration[0][0].AxisParam;

            axisParam.HomeSpeed.HighSpeed.MaxVelocity = Convert.ToSingle((Convert.ToDouble(homeStartSpeed.Value) * 1000 / axisParam.MicronPerPulse));
            axisParam.HomeSpeed.FineSpeed.MaxVelocity = Convert.ToSingle((Convert.ToDouble(homeEndSpeed.Value) * 1000 / axisParam.MicronPerPulse));
            axisParam.JogParam.MaxVelocity = Convert.ToSingle((Convert.ToDouble(jogSpeed.Value) * 1000 / axisParam.MicronPerPulse));
            axisParam.MovingParam.MaxVelocity = Convert.ToSingle((Convert.ToDouble(movingSpeed.Value) * 1000 / axisParam.MicronPerPulse));
            axisParam.MovingParam.AccelerationTimeMs = 200;
            axisParam.MovingParam.DecelerationTimeMs = 200;

            axisConfiguration.SaveConfiguration();

            GlossSettings.Instance().HomeStartSpeed = Convert.ToSingle(homeStartSpeed.Value);
            GlossSettings.Instance().HomeEndSpeed = Convert.ToSingle(homeEndSpeed.Value);
            GlossSettings.Instance().JogSpeed = Convert.ToSingle(jogSpeed.Value);
            GlossSettings.Instance().MovingSpeed = Convert.ToSingle(movingSpeed.Value);
            GlossSettings.Instance().ReferenceMovingSpeed = Convert.ToSingle(refMovingSpeed.Value);

            GlossSettings.Instance().Save();
            GlossSettings.Instance().Load();

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            this.Close();
        }
    }
}
