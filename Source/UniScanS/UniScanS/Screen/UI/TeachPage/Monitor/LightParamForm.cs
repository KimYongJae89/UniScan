using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using DynMvp.Devices.Light;

namespace UniScanG.Screen.UI.Teach.Monitor
{
    public partial class LightParamForm : Form, IMultiLanguageSupport
    {
        LightCtrlHandler lightCtrlHandler;
        LightParam lightParam;

        bool onValueUpdate = false;

        public LightParamForm()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            applyLightButton.Text = StringManager.GetString(this.GetType().FullName, applyLightButton.Text);
        }

        public void Initialize(LightCtrlHandler lightCtrlHandler, LightParam lightParam)
        {
            this.lightCtrlHandler = lightCtrlHandler;
            this.lightParam = lightParam;
        }

        private void LightParamPanel_Load(object sender, EventArgs e)
        {
            UpdateLightValue();
        }

        private void UpdateLightValue()
        {
            lightValue.Value = lightParam.LightValue.Value[0];
        }

        private void applyLightButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lightParam.LightValue.Value.Length; i++)
            {
                lightParam.LightValue.Value[i] = (int)lightValue.Value;
            }

            lightCtrlHandler.TurnOn(lightParam);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonLightPlus_Click(object sender, EventArgs e)
        {
            lightValue.Value = Math.Min(lightValue.Value + 5, 255);
        }

        private void buttonLightMinus_Click(object sender, EventArgs e)
        {
            lightValue.Value = Math.Max(lightValue.Value - 5, 0);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            //StringManager.UpdateString(this.GetType().FullName, labelLightValue);
            //StringManager.UpdateString(this.GetType().FullName, applyLightButton);
            //StringManager.UpdateString(this.GetType().FullName, buttonCancel);
        }
    }
}
