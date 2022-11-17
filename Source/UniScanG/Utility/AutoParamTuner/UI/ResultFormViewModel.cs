using AutoParamTuner.Base;
using AutoParamTuner.Model;
using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using UniScanG.Data.Inspect;

namespace AutoParamTuner.UI
{
    internal class ResultFormViewModel : ViewModel
    {
        public Image2D ModelImage { get => base.Model.Get<Image2D>("ModelImage"); }
        public Dictionary<ParamName, Dictionary<object, TunerResult>> ResultDictionary => base.Model.Get<Dictionary<ParamName, Dictionary<object, TunerResult>>>("ResultDictionary");

        public Figure DrawFigure { get; private set; }
        public RectangleF ViewPort { get; private set; }

        public ResultFormViewModel(Base.Model model) : base(model) { }

        public ResultControl GetResultControl(KeyValuePair<object, TunerResult>[] pairs)
        {
            ResultControl resultControl = new ResultControl(new ResultControlViewModel(base.Model, this, pairs));
            resultControl.Dock = System.Windows.Forms.DockStyle.Fill;
            return resultControl;
        }

        internal void SaveCommand(Control parent, object parameter)
        {
            // 경로 선택
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Xml File|*.xml";
            dialog.AddExtension = true;
            dialog.CreatePrompt = true;
            dialog.DefaultExt = ".xml";
            dialog.FileName = "TuneResult.xml";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            string directory = Path.GetDirectoryName(dialog.FileName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            this.Model.Save(dialog.FileName);
            MessageBox.Show(parent, "Done.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal void LoadCommand(Control parent, object parameter)
        {
            // 경로 선택
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Xml File|*.xml";
            dialog.AddExtension = true;
            dialog.CheckFileExists = true;
            dialog.DefaultExt = ".xml";
            dialog.FileName = "TuneResult.xml";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            this.Model.Load(dialog.FileName);
            MessageBox.Show(parent, "Done.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
