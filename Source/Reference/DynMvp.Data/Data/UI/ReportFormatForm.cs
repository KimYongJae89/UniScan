using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Data;
using DynMvp.Base;

namespace DynMvp.Data.UI
{
    public partial class ReportFormatForm : Form
    {
        Model model;
        public Model Model
        {
            get { return model; }
            set { model = value; }
        }

        public ReportFormatForm()
        {
            InitializeComponent();

            buttonAdd.Text = StringManager.GetString(this.GetType().FullName,buttonAdd.Text);
            buttonDelete.Text = StringManager.GetString(this.GetType().FullName,buttonDelete.Text);
            buttonOk.Text = StringManager.GetString(this.GetType().FullName,buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            ExportPacketFormat reportPacketFormat = model.ModelDescription.ReportPacketFormat;

            reportPacketFormat.ValueDataList.Clear();

            foreach(DataGridViewRow row in gridViewValueData.Rows)
            {
                reportPacketFormat.ValueDataList.Add(new ValueData(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString()));
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            SelectResultValueForm form = new SelectResultValueForm();
            form.Model = model;
            if (form.ShowDialog() == DialogResult.OK)
            {
                string valueName = form.ValueName.Replace("ResultValue.", "");
                gridViewValueData.Rows.Add(form.ObjectName, valueName);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (gridViewValueData.SelectedRows.Count > 0)
            {
                int index = gridViewValueData.SelectedRows[0].Index;
                gridViewValueData.Rows.RemoveAt(index);
            }
        }

        private void ReportFormatForm_Load(object sender, EventArgs e)
        {
            ExportPacketFormat packetFormat = model.ModelDescription.ReportPacketFormat;
            foreach(ValueData valueData in packetFormat.ValueDataList)
            {
                gridViewValueData.Rows.Add(valueData.ObjectName, valueData.ValueName);
            }
        }
    }
}
