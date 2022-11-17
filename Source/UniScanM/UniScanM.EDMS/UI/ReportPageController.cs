using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanM.Data;

namespace UniScanM.EDMS.UI
{
    public class ReportPageController : UniScanM.UI.ReportPageController
    {
        DataGridView dataGridView;
        public override void Initialize(DataGridView dataGridView)
        {
            this.dataGridView = dataGridView;

            base.Initialize(dataGridView);

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = StringManager.GetString(this.GetType().FullName, "Distance [m]"), AutoSizeMode = DataGridViewAutoSizeColumnMode.None, Width = 100, Resizable = DataGridViewTriState.True });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = StringManager.GetString(this.GetType().FullName, "Difference [um]"), AutoSizeMode = DataGridViewAutoSizeColumnMode.None,Width=100, Resizable = DataGridViewTriState.True });

            dataGridView.MultiSelect = true;
        }

        public override void BuildRowData(DataGridViewRow dataGridViewRow, Production production)
        {
            base.BuildRowData(dataGridViewRow, production);

            dataGridViewRow.Cells[4].Value = production.EndPosition - production.StartPosition;

            if(!float.IsNaN(production.Value))
                dataGridViewRow.Cells[5].Value = Math.Round(production.Value, 3);

            //foreach(DataGridViewColumn column in this.dataGridView.Columns)
            //    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }
    }
}
