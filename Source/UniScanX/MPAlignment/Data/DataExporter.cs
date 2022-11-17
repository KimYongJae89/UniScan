using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Devices.Comm;
using DynMvp.InspData;
using System.Threading;
using System.Xml;
using DynMvp.Base;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using UniEye.Base;
using System.Diagnostics;
using DynMvp.Vision;
using UniEye.Base.Settings;
//using Infragistics.Documents.Excel;
using DynMvp.Data;

namespace UniScanX.MPAlignment.Data
{
    internal enum ExcelHeader { Index, Date, Time, RollPos, InspZone, Width, Height, MarginW, MarginH, BlotW, BlotH, DefectW, DefectH, Result, FileName }
    internal enum ResultHeader { Index, Date, Time, RollPos, InspPos, InspZone, pelSizeX, pelSizeY,
        SheetX, SheetY, SheetW, SheetH,
        InspX, InspY, InspW, InspH,
        RoiX, RoiY, RoiW, RoiH,
        BlotRectX, BlotRectY, BlotRectW, BlotRectH,
        MarginRectX, MarginRectY, MarginRectW, MarginRectH,
        InspArea, InspMarginW, InspMarginL, InspBlotW, InspBlotL,
        OffsArea, OffsMarginW, OffsMarginL, OffsBlotW, OffsBlotL,
        DefectW, DefectH, DefectC, Result, PrintingPeriod, PrintingLength,
        MAX_COUNT }

    public class DataExporter : DynMvp.Data.DataExporter
    {
        public DataExporter() : base()
        {

        }

        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected string GetTemplateName()
        {
            return "MPAlignment.xlsx";
        }

        protected  void WriteCsvHeader(StringBuilder stringBuilder)
        {
            //WriteCsvHeader(stringBuilder, typeof(ResultHeader));
        }

  
    }

    public class DataImporter 
    {
        public DataImporter()
        {
        }
    }
}
