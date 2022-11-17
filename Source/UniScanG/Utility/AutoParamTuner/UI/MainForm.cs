using AutoParamTuner.Base;
using DynMvp.UI;
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
using System.Windows.Input;
using UniScanG.Data.Inspect;

namespace AutoParamTuner.UI
{
    internal partial class MainForm : Form
    {
        string defaultPath = @"D:\UniScan\Gravure_Inspector\Model\NEWMODEL\1\1\Model.xml";

        CanvasPanel canvasPanel = null;

        MainFormViewModel viewModel = null;

        Command ModelButtonCommand { get; set; }
        Command StartCommand { get; set; }
        Command ShowCommand { get; set; }

        public MainForm(MainFormViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;

            this.canvasPanel = new CanvasPanel()
            {
                Dock = DockStyle.Fill,
                FastMode = true,
                ReadOnly = true
            };
            this.canvasPanel.SetPanMode();
            groupBoxImage.Controls.Add(this.canvasPanel);

            this.viewModel.PropertyChanged += this.PropertyChanged;

            this.textBoxModel.DataBindings.Add(new Binding("Text", this.viewModel, "ModelPath"));

            this.ModelButtonCommand = new Command(this.viewModel.ModelPathCommand);
            this.StartCommand = new Command(this.viewModel.StartCommand);
            this.ShowCommand = new Command(this.viewModel.ShowCommand);


            this.dataGridView1.DataError += new DataGridViewDataErrorEventHandler((s, e) => MessageBox.Show(e.Exception.Message));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewCheckBoxColumn() { DataPropertyName = "Use", HeaderText = "Use"},
                new DataGridViewComboBoxColumn() { DataPropertyName = "Name", HeaderText = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, DataSource= Enum.GetValues( typeof(Base.ParamName))},
                new DataGridViewTextBoxColumn() { DataPropertyName = "Unit", HeaderText = "Unit", DefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightGray } },
                new DataGridViewTextBoxColumn() { DataPropertyName = "Min", HeaderText = "Min", DefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightGray } },
                new DataGridViewTextBoxColumn() { DataPropertyName = "Max", HeaderText = "Max", DefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightGray } },

                new DataGridViewTextBoxColumn() { DataPropertyName = "Value", HeaderText = "Value" }
            });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            BindingList<Model.SwapParamItem> bindingList = new BindingList<Model.SwapParamItem>(this.viewModel.ParamItemList);
            this.dataGridView1.DataSource = bindingList;

            if (File.Exists(this.defaultPath))
                this.viewModel.Set("ModelPath", this.defaultPath);
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ModelPath":
                    this.textBoxModel.Text = this.viewModel.ModelPath;
                    break;

                case "ModelG":
                    Figure[] regionFigures = this.viewModel.ModelG.CalculatorModelParam.RegionInfoCollection.Select(f =>
                    {
                        Figure regionFigure = f.GetFigure(false);
                        regionFigure.Offset(f.Region.Location);
                        return regionFigure;
                    }).ToArray();

                    this.canvasPanel.BackgroundFigures.Clear();
                    this.canvasPanel.BackgroundFigures.AddFigure(regionFigures);
                    break;

                case "Image":
                    this.canvasPanel.UpdateImage(this.viewModel.Image.ToBitmap());
                    this.canvasPanel.ZoomFit();
                    break;

                //case "InspectionResult":
                //    InspectionResult result = this.viewModel.Get<InspectionResult>("Result");
                //    Figure defectFigure = result.GetDefectFigure();
                //    this.canvasPanel.WorkingFigures.Clear();
                //    this.canvasPanel.WorkingFigures.AddFigure(defectFigure);
                //    break;

                case "Progress100":
                    this.toolStripProgressBar1.Value = Math.Max(this.toolStripProgressBar1.Minimum, Math.Min(this.toolStripProgressBar1.Maximum, (int)Math.Round(viewModel.Progress100)));
                    this.toolStripStatusLabel1.Text = $"{viewModel.Progress100.ToString("F0")}%";
                    break;
            }
        }

        private void buttonModel_Click(object sender, EventArgs e)
        {
            Command.ExcuteCommand((Control)sender, this.ModelButtonCommand, this.textBoxModel.Text);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Command.ExcuteCommand((Control)sender, this.StartCommand);
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            Command.ExcuteCommand((Control)sender, this.ShowCommand);
        }
    }
}
