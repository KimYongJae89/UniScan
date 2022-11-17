using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;

namespace UniScanG.Module.Observer
{
    public partial class ModuleSettingForm : Form, IMultiLanguageSupport
    {
        public Dictionary<int, List<string>> ModulePathList { get; set; }

        public ModuleSettingForm()
        {
            InitializeComponent();

            StringManager.AddListener(this);
        }

        private void ModuleSettingForm_Load(object sender, EventArgs e)
        {
            this.resultPath.Text = PathSettings.Instance().Result;
            this.storeIn.Value = Properties.Settings.Default.ResultStoringDays;

            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Group", AutoSizeMode= DataGridViewAutoSizeColumnMode.AllCells});
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Path", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Test Result", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

            UpdateList(null);            
        }

        private void UpdateList(Dictionary<int, List<string>> list)
        {
            this.dataGridView1.Rows.Clear();

            if (list == null)
                list = this.ModulePathList;

            foreach (var pair in list)
            {
                int moduleGroup = pair.Key;
                DataGridViewRow[] rows = pair.Value.Select(f =>
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(this.dataGridView1);
                    row.Cells[0].Value = pair.Key.ToString();
                    row.Cells[1].Value = f;
                    row.Cells[2].Value = "";
                    return row;
                }).ToArray();

                this.dataGridView1.Rows.AddRange(rows);
            }
        }

        private Dictionary<int, List<string>> ApplyList()
        {
            Dictionary<int, List<string>> newList = new Dictionary<int, List<string>>();

            foreach(DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    int group = int.Parse(row.Cells[0].Value.ToString());
                    string path = row.Cells[1].Value.ToString();

                    if (!newList.ContainsKey(group))
                        newList.Add(group, new List<string>());

                    newList[group].Add(path);
                }
            }

            return newList;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                PathSettings.Instance().Result = this.resultPath.Text;
                PathSettings.Instance().Save();

                Properties.Settings.Default.ResultStoringDays = (int)this.storeIn.Value;
                Properties.Settings.Default.Save();

                this.ModulePathList = ApplyList();
                this.DialogResult =  DialogResult.OK;
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<int, List<string>> list = ApplyList();
                UpdateList(list);

                bool[] results = new bool[0];

                new SimpleProgressForm().Show(() => results = CheckPath(list));
                for (int i = 0; i < results.Length; i++)
                    this.dataGridView1.Rows[i].Cells[2].Value = results[i] ? "OK" : "NG";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool[] CheckPath(Dictionary<int, List<string>> list)
        {
            List<string> stringList = list.SelectMany(f => f.Value).ToList();
            bool[] results = stringList.Select(f =>
            {
                bool exist = false;
                Task task = Task.Run(() => exist = Directory.Exists(f));
                bool isDone = task.Wait(1000);
                return isDone && exist;
            }).ToArray();

            return results;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
