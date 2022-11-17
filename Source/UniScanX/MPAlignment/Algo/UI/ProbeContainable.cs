using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Data;


namespace UniScanX.MPAlignment.Algo.UI
{
    public interface ProbeContainable
    {
        void SetSelectedProbe(Probe probe);
        void UpdateProbeImage();
    }



    //public delegate void ValueChangedDelegate(ValueChangedType valueChangedType, bool modified);
}
