using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using DynMvp.Base;
using DynMvp.UI;
using System.Threading.Tasks;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using UniScanM.Algorithm;
using UniEye.Base.Data;

namespace UniScanM.UI.MenuPage.AutoTune
{
    public partial class AutoTuneForm : Form, IMultiLanguageSupport
    {
        bool autoSet = false;

        AutoTuner autoTuner;
        List<AutoTunePanel> tunePanelList = new List<AutoTunePanel>();
        public delegate void UpdateDataDelegate(List<ImageD> tuneDoneImageList);

        UpdateDataDelegate UpdateData;
        public AutoTuneForm(UpdateDataDelegate UpdateData, AutoTuneType type, object obj, bool autoSet = false)
        {
            this.autoSet = autoSet;
            autoTuner = new AutoTuner(type, obj);

            this.UpdateData = UpdateData;

            InitializeComponent();
            
            this.TopMost = true;
            this.TopLevel = true;
            
            layoutPanel.ColumnStyles.Clear();

            layoutPanel.ColumnCount = SystemManager.Instance().DeviceBox.ImageDeviceHandler.Count;

            foreach (ImageDevice device in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
            {
                AutoTunePanel autoTunePanel = new AutoTunePanel(device.Index);
                
                layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                layoutPanel.Controls.Add(autoTunePanel);

                tunePanelList.Add(autoTunePanel);
            }

            autoTuner.TuneProcessDelegate = TuneProcess;
            autoTuner.TuneDoneDelegate = TuneDone;

            StringManager.AddListener(this);
        }
     
        
        private void TuneDone(List<ImageD> imageList)
        {
            if (InvokeRequired)
            {
                Invoke(new TuneDoneDelegate(TuneDone), imageList);
                return;
            }

            progressBar.Value = 100;
            int lightValue = autoTuner.GetMaxTuneLightValue();

            if (lightValue == 0)
                lightValue = 255;

            this.lightValue.Text = lightValue.ToString();

            SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue.Value[0] = lightValue;

            buttonStart.Enabled = true;
            buttonApply.Enabled = true;
            if (autoSet == true)
            {
                if (UpdateData != null)
                    UpdateData(imageList);

                this.Close();
            }
        }
        
        private void TuneProcess()
        {
            if (InvokeRequired)
            {
                Invoke(new TuneProcessDelegate(TuneProcess));
                return;
            }

            progress.Text = string.Format("{0} / 255", autoTuner.CurLightValue);
            progressBar.Value = (int)autoTuner.GetProgressPercent();

            foreach (AutoTunePanel tunePanel in tunePanelList)
            {
                foreach (TuneValue value in autoTuner.TuneValueList)
                {
                    if (tunePanel.DeviceIndex == value.DeviceIndex)
                    {
                        tunePanel.UpdateData(autoTuner.CurLightValue, value.ValueList.Find(v => v.Item1 == autoTuner.CurLightValue).Item2);
                    }
                }
            }

            
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue.Value[0] = autoTuner.GetMaxTuneLightValue();
            
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            autoTuner.Stop();

            SystemManager.Instance().InspectRunner.ExitWaitInspection();
            SystemState.Instance().SetIdle();
            SystemState.Instance().SetInspectState(InspectState.Ready);
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonApply.Enabled = false;
            buttonStart.Enabled = false;

            foreach (AutoTunePanel tunePanel in tunePanelList)
                tunePanel.Clear();

            autoTuner.Start();
        }

        private void AutoTuneForm_Load(object sender, EventArgs e)
        {
            if (autoSet == true)
            {
                buttonApply.Enabled = false;
                buttonStart.Enabled = false;
                foreach (AutoTunePanel tunePanel in tunePanelList)
                    tunePanel.Clear();

                autoTuner.Start();
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }

    public static class LightValueHelper
    {
        static double maxValue = 255;
        public static int Get_PercentageToLightValue(int percent)
        {
            double result = maxValue * (double)percent / 100.0 ;
            if (result >= maxValue)
                result = maxValue;
            return (int)(result+0.5);
        }

        public static int Get_LightValueToPercentage (int lightValue)
        {
            if (lightValue == 0)
                return 0;
            double result =   (double)lightValue/ maxValue * 100.0 +0.5;
            if (result >= 100)
                result = 100;
            return (int)result;
        }
    }
}