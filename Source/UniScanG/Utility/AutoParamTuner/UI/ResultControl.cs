using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoParamTuner.Base;
using UniScanG.Data;
using DynMvp.UI;

namespace AutoParamTuner.UI
{
    internal partial class ResultControl : UserControl
    {
        ResultControlViewModel viewModel;

        Dictionary<GroupBox, string> groupTextDic = null;

        Command gridSelectionChangedCommand;
        Command listViewSelectionChangedCommand;
        Command listViewDragDropCommand;

        BindingSource bindingSource;

        public ResultControl(ResultControlViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.viewModel.PropertyChanged += this.PropertyChanged;

            this.gridSelectionChangedCommand = new Command(this.viewModel.GridSelectionChangedCommand);
            this.listViewSelectionChangedCommand = new Command(this.viewModel.ListViewSelectionChangedCommand);
            this.listViewDragDropCommand = new Command(this.viewModel.ListViewDragDropCommand);

            this.dataGridView.DataError += new DataGridViewDataErrorEventHandler((s, e) => { });
            this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn() { DataPropertyName = "Item1", HeaderText = "Value", ReadOnly=true, AutoSizeMode=DataGridViewAutoSizeColumnMode.Fill},
                new DataGridViewTextBoxColumn() { DataPropertyName = "Item2", HeaderText = "Total", ReadOnly=true},
                new DataGridViewTextBoxColumn() { DataPropertyName = "Item3", HeaderText = "TrueNG", ReadOnly=true},
                new DataGridViewTextBoxColumn() { DataPropertyName = "Item4", HeaderText = "FalseNG", ReadOnly=true}
            });

            this.groupTextDic = new Dictionary<GroupBox, string>();
            this.groupTextDic.Add(this.groupBoxTR, "&True NG ({0})");
            this.groupTextDic.Add(this.groupBoxNC, "&Unknown ({0})");
            this.groupTextDic.Add(this.groupBoxFR, "&False NG ({0})");

            this.listViewTrue.Tag = "TrueNgList";
            this.listViewUnknown.Tag = "UnknownList";
            this.listViewFalse.Tag = "FalseNgList";
        }

        private void ResultControl_Load(object sender, EventArgs e)
        {
            this.bindingSource = new BindingSource();
            this.dataGridView.DataSource = this.bindingSource;
            UpdateDatagrid();
            UpdateChart();
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TrueNgList":
                    UpdateListview(this.listViewTrue, this.viewModel.TrueNgList);
                    break;

                case "UnknownList":
                    UpdateListview(this.listViewUnknown, this.viewModel.UnknownList);
                    break;

                case "FalseNgList":
                    UpdateListview(this.listViewFalse, this.viewModel.FalseNgList);
                    break;

                case "Chart":
                    UpdateChart();
                    break;

                case "DataGrid":
                    UpdateDatagrid();
                    break;
            }
        }

        private void UpdateDatagrid()
        {
            this.bindingSource.DataSource = this.viewModel.GetDataSource();
            this.bindingSource.Sort = "Item1";
            //this.bindingSource.DataSource = this.viewModel.Pairs;

            //UiHelper.SuspendDrawing(this.dataGridView);
            //this.dataGridView.DataSource = this.viewModel.GetDataSource();
            //UiHelper.ResumeDrawing(this.dataGridView);
        }

        private void UpdateListview(ListView listView, List<UniScanG.Data.FoundedObjInPattern> list)
        {
            if (listView.LargeImageList == null)
                listView.LargeImageList = new ImageList() { ImageSize = new Size(56, 56) };

            int firstSelected = int.MaxValue;
            if (listView.SelectedIndices.Count > 0)
                firstSelected = listView.SelectedIndices[0];

            listView.BeginUpdate();

            listView.LargeImageList.Images.Clear();
            listView.LargeImageList.Images.AddRange(list.Select(f => f.Image).ToArray());
            listView.Items.Clear();
            listView.Items.AddRange(list.Select((f, i) => new ListViewItem(f.GetDefectType().ToString(), i) { Tag = f }).ToArray());

            listView.EndUpdate();

            if (listView.Focused && listView.Items.Count > firstSelected)
            {
                listView.Items[firstSelected].Selected = true;
                listView.EnsureVisible(firstSelected);
            }

            if (listView.Parent is GroupBox)
            {
                GroupBox groupBox = (GroupBox)listView.Parent;
                if (this.groupTextDic.ContainsKey(groupBox))
                    groupBox.Text = string.Format(this.groupTextDic[groupBox], list.Count);
            }
        }

        private void UpdateChart()
        {
            chart1.Series["TrueNg"].Points.Clear();
            chart1.Series["FalseNg"].Points.Clear();
            chart1.Series["Unknown"].Points.Clear();

            foreach(KeyValuePair<object, TunerResult> pair in  this.viewModel.Pairs)
            {
                object x = pair.Key;
                chart1.Series["TrueNg"].Points.AddXY(x, pair.Value.TrueNgList.Count);
                chart1.Series["FalseNg"].Points.AddXY(x, pair.Value.FalseNgList.Count);
                chart1.Series["Unknown"].Points.AddXY(x, pair.Value.UnknownList.Count);
            }
            
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
                return;

            Command.ExcuteCommand(this, this.gridSelectionChangedCommand, dataGridView.SelectedRows[0].DataBoundItem);
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItems.Count == 0)
                return;

            FoundedObjInPattern item = null;
            if (listView.FocusedItem != null)
                item = listView.FocusedItem.Tag as FoundedObjInPattern;
            else
                item = listView.SelectedItems[0].Tag as FoundedObjInPattern;

            if (item == null)
                return;

            Command.ExcuteCommand(this, this.listViewSelectionChangedCommand, item);
        }

        private void listViewNone_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //Console.WriteLine($"{((ListView)sender).Name}_ItemDrag");

            ListView listView = (ListView)sender;
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (ListViewItem item in listView.SelectedItems)
                list.Add(item);

            DragDropEffects eff = DoDragDrop(list.ToArray(), DragDropEffects.Move);
        }

        private void listViewNone_DragEnter(object sender, DragEventArgs e)
        {
            //Console.WriteLine($"{((ListView)sender).Name}_DragEnter");
            if (e.Data.GetDataPresent(typeof(ListViewItem[])))
                e.Effect = DragDropEffects.Move;
        }

        private void listViewNone_DragDrop(object sender, DragEventArgs e)
        {
            //Console.WriteLine($"{((ListView)sender).Name}_DragDrop");

            ListViewItem[] items = (ListViewItem[])e.Data.GetData(typeof(ListViewItem[]));
            string sourceListPropName = (string)items[0].ListView.Tag;
            string targetListPropName = (string)((ListView)sender).Tag;

            Tuple<ListViewItem[], string, string> param = new Tuple<ListViewItem[], string, string>(items, sourceListPropName, targetListPropName);
            Command.ExcuteCommand(this, this.listViewDragDropCommand, param);
        }
        
        private void listViewNone_DragLeave(object sender, EventArgs e)
        {
            //Console.WriteLine($"{((ListView)sender).Name}_DragLeave");
        }

        private void listView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            ListView listView = (ListView)sender;

            if(e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in listView.Items)
                    item.Selected = true;
                return;
            }

            string sourceListPropName = (string)listView.Tag;
            string targetListPropName = null;
            switch (e.KeyCode)
            {
                case Keys.T:
                    targetListPropName = (string)this.listViewTrue.Tag;
                    break;
                case Keys.F:
                    targetListPropName = (string)this.listViewFalse.Tag;
                    break;
                case Keys.U:
                    targetListPropName = (string)this.listViewUnknown.Tag;
                    break;
            }

            if (string.IsNullOrEmpty(targetListPropName))
                return;

            if (targetListPropName == sourceListPropName)
                return;

            
            ListViewItem[] items = new ListViewItem[listView.SelectedItems.Count];
            listView.SelectedItems.CopyTo(items, 0);
            
            //List<ListViewItem> list = new List<ListViewItem>();
            //foreach (ListViewItem item in listView.SelectedItems)
            //    list.Add(item);

            Tuple<ListViewItem[], string, string> param = new Tuple<ListViewItem[], string, string>(items, sourceListPropName, targetListPropName);
            Command.ExcuteCommand(this, this.listViewDragDropCommand, param);
        }

        private void listView_SizeChanged(object sender, EventArgs e)
        {
            return;
            ListView listView = (ListView)sender;
            int size = listView.Width / 10;
            listView.LargeImageList.ImageSize = new Size(size, size);
        }
    }
}
