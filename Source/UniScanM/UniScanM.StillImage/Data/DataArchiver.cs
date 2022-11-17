using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace UniScanM.StillImage.Data
{
    //public abstract class DataArchiver 
    //{
    //    private static DataArchiver instance = null;
    //    public static void SetInstance(DataArchiver dataArchiver)
    //    {
    //        instance = dataArchiver;
    //    }
    //    public static DataArchiver Instance()
    //    {
    //        return instance;
    //    }

    //    public abstract bool Save(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken);
    //    public abstract List<DynMvp.InspData.InspectionResult> Search(string resultPath, DateTime start, DateTime end, string modelName);
    //    public abstract List<string[]> Load(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken);
    //}
}
