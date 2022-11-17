using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;
using UniScanM.Operation;
using DynMvp.UI;
using DynMvp.Data;
using UniScanM.Data;

namespace UniScanM.UI
{
    public partial class RollInfoControl : UserControl, IMultiLanguageSupport
    {
        MachineState state = null;
        Timer timer = null;

        public RollInfoControl()
        {
            InitializeComponent();
            //if (SystemManager.Instance().InspectStarter is PLCInspectStarter)
            //((PLCInspectStarter)SystemManager.Instance().InspectStarter).MelsecMonitor.MelsecInfoMationChanged += InfoChanged;
            UpdateLanguage();

            if (SystemManager.Instance().InspectStarter is PLCInspectStarter)
            {
                state = ((PLCInspectStarter)SystemManager.Instance().InspectStarter).MelsecMonitor?.State;
                this.timer = new Timer();
                timer.Interval = 1000;
                timer.Tick += Timer_Tick;
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Production production = SystemManager.Instance().ProductionManager.CurProduction;
            if (production != null)
            {
                UpdateLotNo(production.LotNo);
                UpdateWorker(production.Worker);
                UpdatePaste(production.Paste);
                UpdateModel(production.Name);
            }
        }

        public void UpdateLotNo(string lotNo)
        {
            //if (string.IsNullOrEmpty(lotNo))
            //    lotNo = "EMPTY";
            string label = StringManager.GetString(this.GetType().FullName, "Lot No");
            labelLotNo.Text = string.Format("{0} : {1}", label, lotNo);
        }

        public void UpdateWorker(string workerName)
        {
            //if (string.IsNullOrEmpty(workerName))
            //    workerName = "Op";
            labelOpName.Text = workerName;
        }

        public void UpdatePaste(string paste)
        {
            //if (string.IsNullOrEmpty(paste))
            //    paste = "EMPTY";
            string label = StringManager.GetString(this.GetType().FullName, "Paste");
            labelPaste.Text = string.Format("{0} : {1}", label, paste);
        }

        public void UpdateModel(string modelName)
        {
            //if (string.IsNullOrEmpty(modelName))
            //    modelName = "DEFAULT";
            string label = StringManager.GetString(this.GetType().FullName, "Model");
            labelModelName.Text = string.Format("{0} : {1}", label, modelName);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void labelModelName_Click(object sender, EventArgs e)
        {
            //bool? IsVirtual = SystemManager.Instance().DeviceBox.MachineIf?.IsVirtual;
            //if (IsVirtual.HasValue && IsVirtual.Value)
            //{
            //    InputForm form = new InputForm(StringManager.GetString(GetType().FullName, "Input Model Name"), state.ModelName);
            //    if (form.ShowDialog() == DialogResult.OK)
            //        state.ModelName = form.InputText;
            //}
        }

        private void labelPaste_Click(object sender, EventArgs e)
        {
            //bool? IsVirtual = SystemManager.Instance().DeviceBox.MachineIf?.IsVirtual;
            //if (IsVirtual.HasValue && IsVirtual.Value)
            //{
            //    InputForm form = new InputForm(StringManager.GetString(GetType().FullName, "Input Paste Name"), state.Paste);
            //    if (form.ShowDialog() == DialogResult.OK)
            //        state.Paste = form.InputText;
            //}
        }

        private void labelLotNo_Click(object sender, EventArgs e)
        {
            //bool? IsVirtual = SystemManager.Instance().DeviceBox.MachineIf?.IsVirtual;
            //if (IsVirtual.HasValue && IsVirtual.Value)
            //{
            //    InputForm form = new InputForm(StringManager.GetString(GetType().FullName, "Input Lot No."), state.LotNo);
            //    if (form.ShowDialog() == DialogResult.OK)
            //        state.LotNo = form.InputText;
            //}
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string[] localeList = new string[] { "ko-kr", "en-us", "zh-cn" };
            string curLocale = string.IsNullOrEmpty(StringManager.LocaleCode) ? "en-us" : Array.Find(localeList, f => f == StringManager.LocaleCode);
            string nextLocale = localeList[((Array.FindIndex(localeList, f => f == curLocale) + 1) % localeList.Length)];
            StringManager.ChangeLanguage(nextLocale);
        }
    }
}
