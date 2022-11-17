using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanM.Data;

namespace UniScanM.UI
{
    public abstract class ReportPageController
    {
        public void Search(DateTime startTime, DateTime endTime, string lot, DataGridView dataGridView)
        {
            Initialize(dataGridView);
            dataGridView.Rows.Clear();
            ProductionManager productionManager = SystemManager.Instance().ProductionManager;
            List<Production> foundProductionList = SystemManager.Instance().ProductionManager.FindAll(null, null, startTime, endTime).ConvertAll(f => f as Production);
            if (string.IsNullOrEmpty(lot) == false)
                foundProductionList.RemoveAll(f => f.LotNo.ToUpper().Contains(lot) == false);

            foreach (Production production in foundProductionList)
            {
                if (production.Total == 0)
                    continue;

                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridView);

                BuildRowData(newRow, production);

                newRow.Tag = production;
                dataGridView.Rows.Add(newRow);
            }

            dataGridView.AutoResizeColumns();
        }

        public virtual void BuildRowData(DataGridViewRow dataGridViewRow, Production production)
        {
            dataGridViewRow.Cells[0].Value = production.StartTime;
            dataGridViewRow.Cells[1].Value = production.Name;
            dataGridViewRow.Cells[2].Value = production.LotNo;
            dataGridViewRow.Cells[3].Value = production.Mode;
            if (production.Mode.Length > 1)
                dataGridViewRow.Cells[3].Value = production.Mode.Substring(0, 1).ToUpper();
        }

        public virtual void Initialize(DataGridView dataGridView)
        {
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = StringManager.GetString(this.GetType().FullName, "Date"),Resizable= DataGridViewTriState.True, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = StringManager.GetString(this.GetType().FullName, "Model"), Resizable = DataGridViewTriState.True, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = StringManager.GetString(this.GetType().FullName, "Lot"), Resizable = DataGridViewTriState.True, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = StringManager.GetString(this.GetType().FullName, ""), Resizable = DataGridViewTriState.True, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells});
            dataGridView.Columns[0].DefaultCellStyle.Format = "yy.MM.dd HH:mm:ss";
        }
    }
}
