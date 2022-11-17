using AutoParamTuner.Base;
using AutoParamTuner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Data;
using UniScanG.Data.Inspect;

namespace AutoParamTuner.UI
{
    internal class ResultControlViewModel : ViewModel
    {
        public ResultFormViewModel ResultFormViewModel { get; private set; }

        public List<FoundedObjInPattern> TrueNgList { get; private set; }
        public List<FoundedObjInPattern> UnknownList { get; private set; }
        public List<FoundedObjInPattern> FalseNgList { get; private set; }

        public int GridSelectedIndex { get; private set; }
        public KeyValuePair<object, TunerResult>[] Pairs { get; private set; }

        public ResultControlViewModel(Base.Model model, ResultFormViewModel resultFormViewModel, KeyValuePair<object, TunerResult>[] pairs) : base(model)
        {
            this.ResultFormViewModel = resultFormViewModel;
            this.ResultFormViewModel.PropertyChanged += this.OnPropertyChanged;

            this.Pairs = pairs;
        }

        public Tuple<object, int, int, int>[] GetDataSource()
        {
            List<Tuple<object, int, int, int>> list = this.Pairs.Select(f => new Tuple<object, int, int, int>(f.Key, f.Value.TotalCount, f.Value.TrueNgList.Count, f.Value.FalseNgList.Count)).ToList();
            return list.OrderBy(f => f.Item1).ToArray();

            return this.Pairs.Select(f => new Tuple<object, int, int, int>(f.Key, f.Value.TotalCount, f.Value.TrueNgList.Count, f.Value.FalseNgList.Count)).ToArray();
        }

        internal void GridSelectionChangedCommand(Control parent, object parameter)
        {
            object key = ((Tuple<Object, int, int, int>)parameter).Item1;
            int selectedIndex = Array.FindIndex(this.Pairs, f => f.Key == key);
            Set<int>("GridSelectedIndex", selectedIndex);

            Set("TrueNgList", this.Pairs.ElementAt(GridSelectedIndex).Value.TrueNgList);
            Set("UnknownList", this.Pairs.ElementAt(GridSelectedIndex).Value.UnknownList);
            Set("FalseNgList", this.Pairs.ElementAt(GridSelectedIndex).Value.FalseNgList);

            DynMvp.UI.Figure drawFigure = this.Pairs[GridSelectedIndex].Value.Figure;
            this.ResultFormViewModel.Set<DynMvp.UI.Figure>("DrawFigure", drawFigure);
        }

        internal void ListViewSelectionChangedCommand(Control parent, object parameter)
        {
            UniScanG.Data.FoundedObjInPattern item = parameter as UniScanG.Data.FoundedObjInPattern;
            ResultFormViewModel.Set<System.Drawing.RectangleF>("ViewPort", item.Region);
        }

        internal void ListViewDragDropCommand(Control parent, object parameter)
        {
            Tuple<ListViewItem[], string, string> param = (Tuple<ListViewItem[], string, string>)parameter;

            ListViewItem[] items = param.Item1;
            string sourcePropName = param.Item2;
            string targetPropName = param.Item3;
            List<FoundedObjInPattern> source = Get<List<FoundedObjInPattern>>(param.Item2);
            List<FoundedObjInPattern> target = Get<List<FoundedObjInPattern>>(param.Item3);

            Array.ForEach(items, f =>
            {
                source.Remove((FoundedObjInPattern)f.Tag);
                target.Add((FoundedObjInPattern)f.Tag);
            });

            OnPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(sourcePropName));
            OnPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(targetPropName));
            OnPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("DataGrid"));
            OnPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("Chart"));
            //Set(sourcePropName, source);
            //Set(targetPropName, target);
        }
    }
}
