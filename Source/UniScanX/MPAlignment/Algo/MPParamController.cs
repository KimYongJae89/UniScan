using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Data;

namespace UniScanX.MPAlignment.Algo
{
    public partial class MPParamController : UserControl
    {
        public MPParamController()
        {
            InitializeComponent();

            this.propertyGrid.SelectedObject = null;
        }

        public delegate void UpdateValueDalegate(object obj);
        public void UpdateValue(object obj)
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateValueDalegate(UpdateValue), obj);
                return;
            }

            this.propertyGrid.SelectedObject = obj;
            this.propertyGrid.ExpandAllGridItems();
        }
    }
}
