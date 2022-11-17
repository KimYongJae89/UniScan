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
    public partial class OutputFormatForm : Form
    {
        Model model;
        public Model Model
        {
            get { return model; }
            set { model = value; }
        }

        public OutputFormatForm()
        {
            InitializeComponent();

            this.Text = StringManager.GetString(this.GetType().FullName,this.Text);
            buttonOk.Text = StringManager.GetString(this.GetType().FullName,buttonOk);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel);
            
            startTypeAscii.Text = StringManager.GetString(this.GetType().FullName,startTypeAscii);
            startTypeHex.Text = StringManager.GetString(this.GetType().FullName,startTypeHex);
            endTypeAscii.Text = StringManager.GetString(this.GetType().FullName,endTypeAscii);
            endTypeHex.Text = StringManager.GetString(this.GetType().FullName,endTypeHex);
            separatorTypeAscii.Text = StringManager.GetString(this.GetType().FullName,separatorTypeAscii);
            separatorTypeHex.Text = StringManager.GetString(this.GetType().FullName,separatorTypeHex);
            useChecksum.Text = StringManager.GetString(this.GetType().FullName,useChecksum);

            ColumnProbeId.HeaderText = StringManager.GetString(this.GetType().FullName, ColumnProbeId);
            ColumnValueName.HeaderText = StringManager.GetString(this.GetType().FullName, ColumnValueName);

            buttonAdd.Text = StringManager.GetString(this.GetType().FullName,buttonAdd.Text);
            buttonDelete.Text = StringManager.GetString(this.GetType().FullName,buttonDelete.Text);


        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            ExportPacketFormat packetFormat = model.ModelDescription.ExportPacketFormat;
            packetFormat.PacketStart = textBoxStart.Text;
            packetFormat.PacketEnd = textBoxEnd.Text;
            packetFormat.Separator = textBoxSeparator.Text;
            packetFormat.UseCheckSum = useChecksum.Checked;
            if (useChecksum.Checked == true)
                packetFormat.ChecksumSize = Convert.ToInt32(checksumSize.Text);
            else
                packetFormat.ChecksumSize = 0;

            packetFormat.PacketStartType = (startTypeAscii.Checked ? DelimiterType.Ascii : DelimiterType.Hex);
            packetFormat.PacketEndType = (endTypeAscii.Checked ? DelimiterType.Ascii : DelimiterType.Hex);
            packetFormat.SeparatorType = (separatorTypeAscii.Checked ? DelimiterType.Ascii : DelimiterType.Hex);

            packetFormat.ValueDataList.Clear();

            foreach(DataGridViewRow row in gridViewValueData.Rows)
            {
                packetFormat.ValueDataList.Add(new ValueData(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString()));
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

        private void OutputFormatForm_Load(object sender, EventArgs e)
        {
            ExportPacketFormat packetFormat = model.ModelDescription.ExportPacketFormat;
            startTypeAscii.Checked = (packetFormat.PacketStartType == DelimiterType.Ascii);
            startTypeHex.Checked = (packetFormat.PacketStartType == DelimiterType.Hex);
            endTypeAscii.Checked = (packetFormat.PacketEndType == DelimiterType.Ascii);
            endTypeHex.Checked = (packetFormat.PacketEndType == DelimiterType.Hex);
            separatorTypeAscii.Checked = (packetFormat.SeparatorType == DelimiterType.Ascii);
            separatorTypeHex.Checked = (packetFormat.SeparatorType == DelimiterType.Hex);

            textBoxStart.Text = packetFormat.PacketStart;
            textBoxEnd.Text = packetFormat.PacketEnd;
            textBoxSeparator.Text = packetFormat.Separator.ToString();
            useChecksum.Checked = packetFormat.UseCheckSum;
            checksumSize.Text = packetFormat.ChecksumSize.ToString();

            foreach(ValueData valueData in packetFormat.ValueDataList)
            {
                gridViewValueData.Rows.Add(valueData.ObjectName, valueData.ValueName);
            }
        }

        
    }
}
