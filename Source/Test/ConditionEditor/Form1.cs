using ConditionEditor.Condition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace ConditionEditor
{
    public partial class Form1 : Form
    {
        ConditionManager conditionManager = new ConditionManager();

        Point cellIdx = new Point(-1, -1);
        public Form1()
        {
            InitializeComponent();

            dataGridView1.RowTemplate.Height = 100;
            dataGridView1.ColumnAdded += new DataGridViewColumnEventHandler((sender, e) =>
            {
                e.Column.HeaderText = e.Column.Index.ToString();
                e.Column.Width = 100;
            });
            dataGridView1.RowsAdded += new DataGridViewRowsAddedEventHandler((sender, e)=>
            {
            });


            dataGridView1.RowCount = 5;
            dataGridView1.ColumnCount = 5;
            Test();
            //int colWidth = 150;
            //dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Width = colWidth });
            //dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Width = colWidth });
            //dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Width = colWidth });
            //dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Width = colWidth });
            //dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Width = colWidth });

            //int rowHeight = 100;
            //dataGridView1.Rows.Add(new DataGridViewRow() { Height = rowHeight });
            //dataGridView1.Rows.Add(new DataGridViewRow() { Height = rowHeight });
            //dataGridView1.Rows.Add(new DataGridViewRow() { Height = rowHeight });
            //dataGridView1.Rows.Add(new DataGridViewRow() { Height = rowHeight });
            //dataGridView1.Rows.Add(new DataGridViewRow() { Height = rowHeight });

            //dataGridView1.Rows[0].Cells[0].ContextMenuStrip = this.contextMenuStrip1;
        }

        private void Test()
        {
            Expression expression1 = new Expression()
            {
                Calculater = new CalculaterDelegate((inputs) =>
                {
                    return new NumericValue(((NumericValue)inputs[0]) + ((NumericValue)inputs[1]));
                })
            };

            NumericValue value = new NumericValue(1);
            while (true)
            {
                value = (NumericValue)expression1.Calculate(value, value);
            }
        }

        private void dataGridView1_RowAdded(object sender, DataGridViewRowEventArgs e)
        {
            throw new NotImplementedException();
        }

        void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
      
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.cellIdx = new Point(e.RowIndex, e.ColumnIndex);

            int yOffset = 0;
            for (int y = 0; y < e.RowIndex; y++)
                yOffset += dataGridView1.Rows[y].Height;

            int xOffset = 0;
            for (int x = 0; x < e.ColumnIndex; x++)
                xOffset += dataGridView1.Columns[x].Width;

            DataGridViewCell dataGridViewCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            dataGridViewCell.Selected = true;
            if (e.Button == MouseButtons.Right)
            {
                Point loc = Point.Add(e.Location, new Size(xOffset, yOffset));
                contextMenuStrip1.Show(dataGridView1.PointToScreen(loc));
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Condition.Expression> conditionList = null;
            if (this.cellIdx.X < conditionManager.LineCount)
                conditionList = conditionManager.GetConditions(this.cellIdx.X);
            else
                conditionList = conditionManager.AddConditions();


            Condition.Expression prevCondition = null;
            Condition.Expression curCondition = null;
            if (this.cellIdx.Y < conditionList.Count)
            {
                prevCondition = this.cellIdx.Y == 0 ? null : conditionList[this.cellIdx.Y - 1];
                curCondition = conditionList[this.cellIdx.Y];
            }

            ConditionEditForm conditionEditForm = new ConditionEditForm();
            conditionEditForm.Initialize(prevCondition, curCondition);
            if(conditionEditForm.ShowDialog() == DialogResult.OK)
            {
                if (curCondition != null)
                    curCondition.CopyFrom(conditionEditForm.Condition);
                else
                    conditionList.Add(conditionEditForm.Condition);
            }
        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            int line = e.RowIndex;
            int step = e.ColumnIndex;
            if (conditionManager.IsExistCondition(line, step))
                e.Value = conditionManager.GetConditions(line)[step];
            else
                e.Value = "";
        }
    }
}
