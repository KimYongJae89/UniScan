using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data;
using UniScanG.Data.Inspect;

namespace AutoParamTuner.Base
{
    internal class TunerResult
    {
        public Figure Figure { get; private set; }
        public int TotalCount => this.UnknownList.Count + this.TrueNgList.Count + this.FalseNgList.Count;

        public List<FoundedObjInPattern> UnknownList { get; } = new List<FoundedObjInPattern>();
        public List<FoundedObjInPattern> TrueNgList { get; } = new List<FoundedObjInPattern>();
        public List<FoundedObjInPattern> FalseNgList { get; } = new List<FoundedObjInPattern>();

        public static TunerResult Load(XmlElement xmlElement, string key)
        {
            TunerResult tunerResult = new TunerResult();
            tunerResult.LoadXml(xmlElement, key);
            return tunerResult;
        }

        private TunerResult() { }
        public TunerResult(InspectionResult inspectionResult)
        {
            this.Figure = inspectionResult.GetDefectFigure();

            this.UnknownList.AddRange(inspectionResult.GetSubResultList());
            this.TrueNgList.Clear();
            this.FalseNgList.Clear();
        }

        public void SaveXml(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = XmlHelper.CreateAndAppendChild(xmlElement, key);
                SaveXml(subElement, "");
                return;
            }

            XmlElement elementTn = XmlHelper.CreateAndAppendChild(xmlElement, "TrueNg");
            SetDefectXmlData(elementTn, TrueNgList);

            XmlElement elementFn = XmlHelper.CreateAndAppendChild(xmlElement, "FalseNg");
            SetDefectXmlData(elementFn, FalseNgList);

            XmlElement elementNc = XmlHelper.CreateAndAppendChild(xmlElement, "Unknown");
            SetDefectXmlData(elementNc, UnknownList);
        }

        private void SetDefectXmlData(XmlElement xmlElement, List<FoundedObjInPattern> list)
        {
            list.ForEach(f =>
            {
                XmlElement defectElement = XmlHelper.CreateAndAppendChild(xmlElement, "Defect");
                XmlHelper.SetValue(defectElement, "Data", f.ToExportData());
                XmlHelper.SetValue(defectElement, "Bitmap", ImageHelper.BitmapToBase64String(f.Image));

            //    using (System.IO.MemoryStream mStream = new System.IO.MemoryStream())
            //    {
            //        using (System.IO.Compression.GZipStream gZipStream = new System.IO.Compression.GZipStream(mStream, System.IO.Compression.CompressionMode.Compress))
            //        {
            //            string base64 = ImageHelper.BitmapToBase64String(f.Image);
            //            byte[] bytes = Encoding.Default.GetBytes(base64);
            //            gZipStream.Write(bytes, 0, bytes.Length);
            //            gZipStream.Flush();
            //        }

            //        byte[] buffers = mStream.GetBuffer();
            //        string str = Encoding.Default.GetString(buffers);
            //        XmlHelper.SetValue(defectElement, "Bitmap", str);
            //    }
            });
        }

        public void LoadXml(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                LoadXml(xmlElement[key], "");
                return;
            }

            XmlElement elementTn = xmlElement["TrueNg"];
            GetDefectXmlData(elementTn, TrueNgList);

            XmlElement elementFn = xmlElement["FalseNg"];
            GetDefectXmlData(elementFn, FalseNgList);

            XmlElement elementNc = xmlElement["Unknown"];
            GetDefectXmlData(elementNc, UnknownList);

            FigureGroup figureGroup = new FigureGroup();
            figureGroup.AddFigure(TrueNgList.Select(f => f.GetFigure()).ToArray());
            figureGroup.AddFigure(FalseNgList.Select(f => f.GetFigure()).ToArray());
            figureGroup.AddFigure(UnknownList.Select(f => f.GetFigure()).ToArray());
            this.Figure = figureGroup;
        }

        private void GetDefectXmlData(XmlElement xmlElement, List<FoundedObjInPattern> list)
        {
            XmlNodeList xmlDefectNodeList = xmlElement.GetElementsByTagName("Defect");
            foreach (XmlElement xmlDefectElement in xmlDefectNodeList)
            {
                string dataStr = XmlHelper.GetValue(xmlDefectElement, "Data", "");
                string typeString = dataStr.Split(',').FirstOrDefault();

                FoundedObjInPattern data = FoundedObjInPattern.Create(typeString);
                data.FromExportData(dataStr);

                string bitmapStr = XmlHelper.GetValue(xmlDefectElement, "Bitmap", "");
                data.Image = ImageHelper.Base64StringToBitmap(bitmapStr);

                list.Add(data);
            }
        }
    }
}
