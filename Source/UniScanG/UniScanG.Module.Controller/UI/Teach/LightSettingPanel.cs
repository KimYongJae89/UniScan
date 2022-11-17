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
using UniScanG.Gravure.Device;
using UniScanG.MachineIF;
using UniScanG.Module.Controller.Device;
using DynMvp.Base;

namespace UniScanG.Module.Controller.UI.Teach
{
    public partial class LightSettingPanel : UserControl, IMultiLanguageSupport
    {
        LightCtrl lightCtrl;
        LightParam curLightParam;
        LightParamSet lightParamSet;

        List<Tuple<Label, TrackBar, NumericUpDown>> controlList;

        MachineIfData machineIfData;

        bool onUpdate = false;

        public LightSettingPanel(LightCtrl lightCtrl, LightParamSet lightParamSet)
        {
            InitializeComponent();

            this.lightCtrl = lightCtrl;
            this.curLightParam = new LightParam(lightCtrl.LastLightValue);
            this.lightParamSet = lightParamSet.Clone();
            this.machineIfData = (SystemManager.Instance().DeviceController as DeviceControllerG)?.MachineIfMonitor?.MachineIfData as MachineIfData;

            this.controlList = new List<Tuple<Label, TrackBar, NumericUpDown>>();

            this.controlList.Add(new Tuple<Label, TrackBar, NumericUpDown>(this.lightTopLeft, this.trackBarLightTopLeft, this.numericLightTopLeft));
            this.controlList.Add(new Tuple<Label, TrackBar, NumericUpDown>(this.lightTopMiddle, this.trackBarLightTopMiddle, this.numericLightTopMiddle));
            this.controlList.Add(new Tuple<Label, TrackBar, NumericUpDown>(this.lightTopRight, this.trackBarLightTopRight, this.numericLightTopRight));
            this.controlList.Add(new Tuple<Label, TrackBar, NumericUpDown>(this.lightBottom, this.trackBarLightBottom, this.numericLightBottom));

            if (this.lightCtrl.NumChannel == 3)
            {
                this.controlList.RemoveAt(1);
                this.dataGridView1.Columns.RemoveAt(2);
                this.numericLightTopMiddle.Enabled = this.trackBarLightTopMiddle.Enabled = this.lightTopMiddle.Enabled = false;
            }

            StringManager.AddListener(this);

            this.Disposed += LightSettingPanel_Disposed;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void LightSettingPanel_Disposed(object sender, EventArgs e)
        {
            lightCtrl.OnLigthValueChanged -= lightCtrl_OnLigthValueChanged;
        }

        private void LightSettingPanel_Load(object sender, EventArgs e)
        {
            lightCtrl.OnLigthValueChanged += lightCtrl_OnLigthValueChanged;
            UpdateData(true);
        }

        private void lightCtrl_OnLigthValueChanged()
        {
            if(this.InvokeRequired)
            {
                BeginInvoke(new OnLigthValueChangedDelegate(lightCtrl_OnLigthValueChanged));
                return;
            }

            int numLight = Math.Min(this.lightCtrl.NumChannel, 4);
            for (int i = 0; i < numLight; i++)
                this.controlList[i].Item1.Text = this.lightCtrl.GetLightValue().Value[i].ToString();
        }

        public void UpdateData(bool updateAll)
        {
            onUpdate = true;
            int numLight = Math.Min(this.lightCtrl.NumChannel, 4);

            for (int i = 0; i < numLight; i++)
            {
                this.controlList[i].Item3.Minimum = this.controlList[i].Item2.Minimum = 0;
                this.controlList[i].Item3.Maximum = this.controlList[i].Item2.Maximum = this.lightCtrl.GetMaxLightLevel();

                this.controlList[i].Item1.Text = this.lightCtrl.GetLightValue().Value[i].ToString();

                if (updateAll)
                    this.controlList[i].Item3.Value = this.controlList[i].Item2.Value = Math.Max(0, this.curLightParam.LightValue.Value[i]);
            }

            this.dataGridView1.Rows.Clear();
            foreach (LightParam lp in this.lightParamSet.LightParamList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(this.dataGridView1);

                row.Cells[0].Value = lp.Name;
                for (int i = 0; i < Math.Min(row.Cells.Count - 1, lp.LightValue.NumLight); i++)
                    row.Cells[i + 1].Value = lp.LightValue.Value[i];

                this.dataGridView1.Rows.Add(row);
            }
            onUpdate = false;
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            TrackBar trackBar = sender as TrackBar;
            if (trackBar != null)
            {
                int idx = this.controlList.FindIndex(f => f.Item2 == trackBar);
                if (idx < 0)
                    return;

                int newValue = (int)this.controlList[idx].Item2.Value;
                this.controlList[idx].Item3.Value = newValue;
                this.curLightParam.LightValue.Value[idx] = newValue;
                //TurnOn(idx, newValue, false);
            }
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (numericUpDown != null)
            {
                int idx = this.controlList.FindIndex(f => f.Item3 == numericUpDown);
                if (idx < 0)
                    return;

                int newValue = (int)numericUpDown.Value;
                this.controlList[idx].Item2.Value = newValue;
                this.curLightParam.LightValue.Value[idx] = newValue;
                //TurnOn(idx, newValue,false);
            }
        }

        private void TurnOn(LightParam lightParam)
        {
            //this.curLightParam = (LightParam)lightParam.Clone();
            lightCtrl.TurnOn(lightParam.LightValue);
            UpdateData(false);
        }

        private void TurnOff()
        {
            lightCtrl.TurnOff();
            UpdateData(false);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            float lineSpeed = this.machineIfData == null ? 0 : this.machineIfData.GET_TARGET_SPEED_REAL;
            //if (lineSpeed == 0)
            //    return;

            string name = lineSpeed.ToString("F1");
            LightParam oldLightParam = this.lightParamSet.LightParamList.Find(f => f.Name == name);
            if (oldLightParam == null)
            {
                LightParam newLightParam = (LightParam)this.curLightParam.Clone();
                newLightParam.Name = name;
                this.lightParamSet.LightParamList.Add(newLightParam);
                this.lightParamSet.LightParamList.Sort(new UniScanG.Data.Model.LightParamComparer());
            }
            else
            {
                int copyLen = Math.Min(this.curLightParam.LightValue.NumLight, oldLightParam.LightValue.NumLight);
                Array.Copy(this.curLightParam.LightValue.Value, oldLightParam.LightValue.Value, copyLen);
            }
            UpdateData(false);
        }

        private void buttonOn_Click(object sender, EventArgs e)
        {
            LightParam lightParam = (LightParam)this.curLightParam.Clone();
            if (DynMvp.Devices.FrameGrabber.CameraConfiguration.ConfigFlag == "FASTMODE")
            {
                for (int i = 0; i < lightParam.LightValue.Value.Length; i++)
                    lightParam.LightValue.Value[i] = lightParam.LightValue.Value[i] / 4;
            }
            TurnOn(lightParam);
        }
        
        private void buttonOff_Click(object sender, EventArgs e)
        {
            TurnOff();
        }

        private void buttonOnAuto_Click(object sender, EventArgs e)
        {
            LightParam lightParam = ((DeviceController)SystemManager.Instance().DeviceController).GetLightParam(this.lightParamSet);
            if (lightParam != null)
                TurnOn(lightParam);
            //if (lightParamSet.LightParamList.Count == 0)
            //{
            //    TurnOff();
            //    return;
            //}

            //float lineSpeed = this.machineIfData == null ? 0 : this.machineIfData.GET_TARGET_SPEED;
            //if (lineSpeed == 0)
            //{
            //    TurnOff();
            //    return;
            //}

            //LightParam sameLightParam = GetSameLightParam(lightParamSet.LightParamList, lineSpeed);
            //if(sameLightParam!=null)
            //{
            //    TurnOn(sameLightParam);
            //    return;
            //}

            //LightParam upperLightParam = GetUpperLightParam(lightParamSet.LightParamList, lineSpeed);
            //LightParam lowerLightParam = GetLowerLightParam(lightParamSet.LightParamList, lineSpeed);
            //LightParam lightParam = null;

            //if (upperLightParam != null && lowerLightParam != null)
            //// 두 부분 선형 보간
            //{
            //    lightParam = GetLightParam(lineSpeed, lowerLightParam, upperLightParam);
            //}
            //else if (lowerLightParam == null)
            //// 빠른 두 개로 선형보간
            //{
            //    LightParam upperLightParam2 = GetUpperLightParam(lightParamSet.LightParamList, float.Parse(upperLightParam.Name));
            //    if (upperLightParam2 != null)
            //        lightParam = GetLightParam(lineSpeed, upperLightParam, upperLightParam2);
            //    else
            //        lightParam = GetLightParam(lineSpeed, new LightParam(this.curLightParam.LightValue.NumLight, 0) { Name = "0" }, upperLightParam);
            //}
            //else if (upperLightParam == null)
            //// 느린 두 개로 선형보간
            //{
            //    LightParam lowerLightParam2 = GetLowerLightParam(lightParamSet.LightParamList, float.Parse(lowerLightParam.Name));
            //    if (lowerLightParam2 != null)
            //        lightParam = GetLightParam(lineSpeed, lowerLightParam2, lowerLightParam);
            //    else
            //        lightParam = GetLightParam(lineSpeed, new LightParam(this.curLightParam.LightValue.NumLight, 0) { Name = "0" }, lowerLightParam);
            //}

            //if (lightParam != null)
            //    TurnOn(lightParam);
        }

        private LightParam GetSameLightParam(List<LightParam> lightParamList, float lineSpeed)
        {
            return lightParamList.Find(f =>
            {
                float nameValue = 0;
                bool isFloat = float.TryParse(f.Name, out nameValue);
                if (!isFloat)
                    return false;
                return nameValue == lineSpeed;
            });
        }

        private LightParam GetUpperLightParam(List<LightParam> lightParamList, float lineSpeed)
        {
            return lightParamList.Find(f =>
            {
                float nameValue = 0;
                bool isFloat = float.TryParse(f.Name, out nameValue);
                if (!isFloat)
                    return false;
                return nameValue > lineSpeed;
            });
        }

        private LightParam GetLowerLightParam(List<LightParam> lightParamList, float lineSpeed)
        {
            return lightParamList.FindLast(f =>
            {
                float nameValue = 0;
                bool isFloat = float.TryParse(f.Name, out nameValue);
                if (!isFloat)
                    return false;
                return nameValue < lineSpeed;
            });
        }

        private LightParam GetLightParam(float lineSpeed, LightParam lowerLightParam, LightParam upperLightParam)
        {
            // 두 부분을 선형 보간
            float lowerSpeed = float.Parse(lowerLightParam.Name);
            float upperSpeed = float.Parse(upperLightParam.Name);
            int numLight = Math.Min(lowerLightParam.LightValue.NumLight, upperLightParam.LightValue.NumLight);
            LightParam newLightParam = new LightParam(numLight);
            newLightParam.Name = lineSpeed.ToString("F1");
            for (int i = 0; i < numLight; i++)
                newLightParam.LightValue.Value[i] = (int)Math.Round(lowerLightParam.LightValue.Value[i] + (upperLightParam.LightValue.Value[i] - lowerLightParam.LightValue.Value[i]) / (upperSpeed - lowerSpeed) * (lineSpeed - lowerSpeed));

            return newLightParam;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.onUpdate)
                return;

            if (dataGridView1.Rows.Count == 0)
                return;

            string newValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            if (e.ColumnIndex == 0)
                this.lightParamSet.LightParamList[e.RowIndex].Name = float.Parse(newValue).ToString("F1");
            else
                this.lightParamSet.LightParamList[e.RowIndex].LightValue.Value[e.ColumnIndex - 1] = int.Parse(newValue);

            UpdateData(true);
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (this.onUpdate)
                return;

            if (e.ColumnIndex == 0)
            {
                float f;
                e.Cancel = !float.TryParse(e.FormattedValue.ToString(), out f);
            }
            else
            {
                int i;
                e.Cancel = !int.TryParse(e.FormattedValue.ToString(), out i);
            }
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (onUpdate)
                return;

            this.lightParamSet.LightParamList.RemoveAt(e.RowIndex);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().CurrentModel.LightParamSet = this.lightParamSet.Clone();
            SystemManager.Instance().CurrentModel.Modified = true;
            SystemManager.Instance().ExchangeOperator.SaveModel();
        }
    }
}
