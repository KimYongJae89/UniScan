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
using UnieyeLauncher.Operation;

namespace UnieyeLauncher.UI
{
    public partial class ArchiveSelectForm : Form
    {
        public List<ArchiveItem> ArchiveItemList { get; } = new List<ArchiveItem>();

        public ArchiveItem SelectedArchiveItem { get; private set; }

        public ArchiveSelectForm(ArchiveOperator archiver)
        {
            InitializeComponent();
 
            this.ArchiveItemList.AddRange(archiver.ArchiveItems);
            this.ArchiveItemList.Sort((f, g) => g.DateTime.CompareTo(f.DateTime));

            //this.dataGridView.MultiSelect = false;
            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.AutoGenerateColumns = false;

            this.dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Date", DataPropertyName = "DateTime", DefaultCellStyle = new DataGridViewCellStyle() { Format = "yy-MM-dd HH:mm:ss" },  AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            this.dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Version", DataPropertyName = "Version", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            this.dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "File", DataPropertyName = "FileName", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            this.dataGridView.Columns.Add(new DataGridViewCheckBoxColumn() { HeaderText = "Bin", DataPropertyName = "ContainBin", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells});
            this.dataGridView.Columns.Add(new DataGridViewCheckBoxColumn() { HeaderText = "Config", DataPropertyName = "ContainConfig", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells});
            this.dataGridView.RowTemplate.ContextMenuStrip = this.contextMenuStrip;
        }

        private void ArchiveSelectForm_Load(object sender, EventArgs e)
        {
            this.dataGridView.DataSource = ArchiveItemList;
            this.dataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.Automatic;

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedRows.Count == 1)
            {
                this.SelectedArchiveItem = this.ArchiveItemList[this.dataGridView.SelectedRows[0].Index];
                this.DialogResult = DialogResult.OK;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ArchiveItem> deleteList = new List<ArchiveItem>();
            foreach(DataGridViewRow row in dataGridView.SelectedRows)
                deleteList.Add(this.ArchiveItemList[row.Index]);

            DialogResult dialogResult;
            if (deleteList.Count > 1)
                dialogResult = MessageBox.Show(string.Format("Delete {0} Items?", deleteList.Count), "Caption", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            else if (deleteList.Count == 1)
                dialogResult = MessageBox.Show(string.Format("Delete Archive File {0}?", deleteList[0].fileName), "Caption", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            else
                return;

            if (dialogResult == DialogResult.Cancel)
                return;

            deleteList.ForEach(f =>
            {
                FileInfo fileInfo = new FileInfo(f.FullPathName);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            });

            this.ArchiveItemList.RemoveAll(f => deleteList.Contains(f));

            dataGridView.DataSource = null;
            dataGridView.DataSource = this.ArchiveItemList;
            dataGridView.ClearSelection();
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
