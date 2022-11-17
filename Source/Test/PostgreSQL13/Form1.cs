using DynMvp.Data.Database.PostgreSQL13;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostgreSQL13
{
    public partial class Form1 : Form
    {
        public DynMvp.Data.Database.PostgreSQL13.Database Database { get; private set; } = null;
        Options options = new Options();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = options;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            this.Database = new Database(new DatabaseSettings() { Server = options.Server, DatabaseName = options.Database, UserName = options.Username, Password = options.Password });
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            TableInfo[] tables = this.Database.GetTables();

            this.dataGridViewTables.Rows.Clear();

            Array.ForEach(tables, f => {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(this.dataGridViewTables);
                row.Cells[0].Value = f.TableName;
                this.dataGridViewTables.Rows.Add(row);
            });
        }
    }
}
