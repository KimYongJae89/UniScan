using AutoParamTuner.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoParamTuner.UI
{
    internal partial class ResultForm : Form
    {
        ResultFormViewModel viewModel;

        CanvasPanel canvasPanel;

        Command saveCommand;
        Command loadCommand;

        public ResultForm(ResultFormViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.viewModel.PropertyChanged += this.PropertyChanged;

            this.canvasPanel = new CanvasPanel()
            {
                Dock = DockStyle.Fill,
                FastMode = true,
                ReadOnly = true
            };
            this.canvasPanel.MouseDoubleClick += new MouseEventHandler((sender, e) =>
            {
                CanvasPanel canvasPanel = sender as CanvasPanel;
                if (canvasPanel == null)
                    return;

                if (e.Button == MouseButtons.Left)
                    canvasPanel.ZoomFit();
            });
            this.canvasPanel.SetPanMode();
            groupBoxImage.Controls.Add(this.canvasPanel);

            this.saveCommand = new Command(this.viewModel.SaveCommand);
            this.loadCommand = new Command(this.viewModel.LoadCommand);
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            PropertyChanged(this, new PropertyChangedEventArgs("ModelImage"));
            PropertyChanged(this, new PropertyChangedEventArgs("ResultDictionary"));        
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ModelImage":
                    this.canvasPanel.UpdateImage(this.viewModel.ModelImage?.ToBitmap());
                    this.canvasPanel.ZoomFit();
                    break;

                case "ResultDictionary":
                    this.tabControl1.TabPages.Clear();
                    if (this.viewModel.ResultDictionary != null)
                    {
                        for (int i = 0; i < this.viewModel.ResultDictionary.Keys.Count; i++)
                        {
                            ParamName key = this.viewModel.ResultDictionary.Keys.ElementAt(i);
                            this.tabControl1.TabPages.Add(key.ToString(), key.ToString());

                            var v2 = this.viewModel.ResultDictionary[key];
                            this.tabControl1.TabPages[key.ToString()].Controls.Add(this.viewModel.GetResultControl(v2.ToArray()));
                        }
                    }
                    break;

                case "DrawFigure":
                    this.canvasPanel.WorkingFigures.Clear();
                    this.canvasPanel.WorkingFigures.AddFigure(this.viewModel.DrawFigure);
                    this.canvasPanel.Invalidate();
                    break;

                case "ViewPort":
                    RectangleF viewport = this.viewModel.ViewPort;
                    viewport.Inflate(Math.Max(200 - viewport.Width, 0) / 2, Math.Max(200 - viewport.Height, 0) / 2);
                    this.canvasPanel.ZoomRange(viewport);
                    break;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command.ExcuteCommand(this, this.saveCommand);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command.ExcuteCommand(this, this.loadCommand);
        }
    }
}
