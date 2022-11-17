using DynMvp.Base;
using DynMvp.InspData;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanWPF.Screen.PinHoleColor.Data
{
    public abstract class CSVDataExporter : DynMvp.Data.DataExporter
    {
        protected string resultPath;

        protected abstract void SaveCSVHeader(string resultFile);
        protected abstract void AppendResult(StringBuilder stringBuilder, DynMvp.InspData.InspectionResult inspectionResult);
                                           
        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            try
            {
                resultPath = inspectionResult.ResultPath;

                if (Directory.Exists(resultPath) == false)
                    Directory.CreateDirectory(resultPath);

                FlushCSV(resultPath, inspectionResult);
            }
            finally
            {

            }
        }

        private void FlushCSV(string resultPath, InspectionResult inspectionResult)
        {
            string tempFile = Path.Combine(resultPath, "temp.csv");
            if (File.Exists(tempFile) == false)
                SaveCSVHeader(tempFile);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(tempFile, true, Encoding.ASCII))
            {
                StringBuilder sb = new StringBuilder();

                AppendResult(sb, inspectionResult);
                file.Write(sb);
                file.Flush();
            }

            if (inspectionResult.GrabImageList.Count > 0 && inspectionResult.Judgment == Judgment.Reject)
            {
                string imgFile = Path.Combine(resultPath, string.Format("{0}.jpg", inspectionResult.InspectionNo));
                ImageD saveImage = inspectionResult.GrabImageList[0];
                saveImage?.SaveImage(imgFile, ImageFormat.Jpeg);
                saveImage?.Dispose();
            }

            string resultFile = Path.Combine(resultPath, "Result.csv");
            try
            {
                File.Copy(tempFile, resultFile, true);
                FileAttributes attrib = File.GetAttributes(resultFile);
                attrib &= ~FileAttributes.Hidden;
                File.SetAttributes(resultFile, attrib);
            }
            catch (IOException ex)
            { }
        }
    }
}
