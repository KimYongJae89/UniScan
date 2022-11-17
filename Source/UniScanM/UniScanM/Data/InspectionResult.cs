using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.InspData;

namespace UniScanM.Data
{
    public class InspectionResult : DynMvp.InspData.InspectionResult
    {
        string worker;
        public string Wroker { get => worker; set => worker = value; }

        Bitmap displayBitmap;
        public Bitmap DisplayBitmap { get => displayBitmap; set => displayBitmap = value; }

        Bitmap saveBitmap;
        public Bitmap SaveBitmap { get => saveBitmap; set => saveBitmap = value; }

        int rollDistance;   // From PLC
        public int RollDistance { get => rollDistance; set => rollDistance = value; }

        string reportPath;
        public string ReportPath { get => reportPath; set => reportPath = value; }

        string displayBitmapFilename= "";
        public string DisplayBitmapSaved { get => displayBitmapFilename; set => displayBitmapFilename = value; }
    }
}


