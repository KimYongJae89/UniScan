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
using System.IO;
using UniScanS.Screen.Data;

namespace UniScanS.UI.Report
{
    public partial class ReportProgressForm : Form
    {         
        private string messageText;
        public string MessageText
        {
          set { messageText = value; }
        }

        List<DataGridViewRow> sheetDataList;
        
        bool isCancle = false;
        bool searchDone = false;

        int total;

        public ReportProgressForm(string message)
        {
            InitializeComponent();
            messageText = message;

            this.TopMost = true;
            this.TopLevel = true;
            if (messageText != null)
            {
                this.labelMessage.Text = StringManager.GetString(this.GetType().FullName, message);           
            }
        }

        public void SetLabelMessage(string message)
        {
            this.labelMessage.Text = StringManager.GetString(this.GetType().FullName, messageText);
        }

        public void Show(int total, string resultPath, ref List<DataGridViewRow> sheetDataList)
        {
            this.total = total;
            this.sheetDataList = sheetDataList;

            DirectoryInfo directoryInfo = new DirectoryInfo(resultPath);
            DirectoryInfo[] directoryInfoList = directoryInfo.GetDirectories();
            
            progressBar.Value = sheetDataList.Count / total * 100;
            labelRatio.Text = string.Format("{0} / {1}", sheetDataList.Count, total);

            Task task = new Task(new Action(() => Search(directoryInfoList)));
            task.Start();

            base.ShowDialog();
        }

        private void Search(DirectoryInfo[] directoryInfoList)
        {
            Parallel.ForEach(directoryInfoList, subDirectory =>
            {
                if (isCancle == true)
                    return;

                bool parseResult = false;
                int index = -1;
                parseResult = int.TryParse(subDirectory.Name, out index);

                if (parseResult == false)
                    return;

                if (total < index)
                    return;

                MergeSheetResult mergeSheetResult = new MergeSheetResult(index, subDirectory.FullName);
                
                DataGridViewRow dataGridViewRow = new DataGridViewRow();

                DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell() { Value = mergeSheetResult.Index };
                DataGridViewTextBoxCell qtyCell = new DataGridViewTextBoxCell() { Value = mergeSheetResult.DefectNum };

                dataGridViewRow.Cells.Add(nameCell);
                dataGridViewRow.Cells.Add(qtyCell);

                dataGridViewRow.Tag = mergeSheetResult;

                lock (sheetDataList)
                    sheetDataList.Add(dataGridViewRow);
            });
            
            searchDone = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            isCancle = true;
        }

        private void taskCheckTimer_Tick(object sender, EventArgs e)
        {
            if (searchDone == true)
                Close();

            if (total != 0)
            {
                progressBar.Value = (int)((float)sheetDataList.Count / (float)total * 100.0f);
                labelRatio.Text = string.Format("{0} / {1}", sheetDataList.Count, total);
                progressBar.Invalidate();
            }
        }

        private void SimpleProgressForm_Load(object sender, EventArgs e)
        {
            taskCheckTimer.Start();
        }
    }
}
