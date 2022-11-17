using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;

namespace UniScanM.UI.MenuPage.AutoTune
{
    public partial class AutoTunePanel : UserControl
    {
        private int deviceIndex;
        public int DeviceIndex { get => deviceIndex; }
        
        public AutoTunePanel(int deviceIndex)
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            this.deviceIndex = deviceIndex;
            labelTitle.Text = string.Format("Cam {0}", deviceIndex);
        }
        
        public void UpdateData(int lightValue, float std)
        {
            valueList.Rows.Add(lightValue, std);
            valueList.Sort(valueList.Columns[1], System.ComponentModel.ListSortDirection.Descending);
        }

        public void Clear()
        {
            valueList.Rows.Clear();
        }
    }
}
