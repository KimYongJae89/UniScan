using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using DynMvp.Base;
using System.Globalization;

namespace DynMvp.Data
{
    public class ModelDescription
    {
        public virtual bool IsDefaultModel => this.name == "Default";

        protected string name = "";
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string category = "";
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        private string productName = "";
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        private string productCode = "";
        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        private string description = "";
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        bool useByMasterModel;
        public bool UseByMasterModel
        {
            get { return useByMasterModel; }
            set { useByMasterModel = value; }
        }

        string masterModelName = "";
        public string MasterModelName
        {
            get { return masterModelName; }
            set { masterModelName = value; }
        }

        ExportPacketFormat exportPacketFormat = new ExportPacketFormat();
        public ExportPacketFormat ExportPacketFormat
        {
            get { return exportPacketFormat; }
            set { exportPacketFormat = value; }
        }

        ExportPacketFormat reportPacketFormat = new ExportPacketFormat();
        public ExportPacketFormat ReportPacketFormat
        {
            get { return reportPacketFormat; }
            set { reportPacketFormat = value; }
        }

        float fiducialDistanceUm;
        public float FiducialDistanceUm
        {
            get { return fiducialDistanceUm; }
            set { fiducialDistanceUm = value; }
        }

        DateTime registrationDate;
        public DateTime RegistrationDate
        {
            get { return registrationDate; }
            set { registrationDate = value; }
        }

        DateTime lastModifiedDate;
        public DateTime LastModifiedDate
        {
            get { return lastModifiedDate; }
            set { lastModifiedDate = value; }
        }

        string registrant;
        public string Registrant
        {
            get { return registrant; }
            set { registrant = value; }
        }

        public ModelDescription()
        {
            productName = "";
            productCode = "";

            fiducialDistanceUm = 0;

            reportPacketFormat.PacketStart = "";
            reportPacketFormat.PacketEnd = "";
            reportPacketFormat.Separator = ",";

            this.registrationDate = DateTime.Now;
            this.lastModifiedDate = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            ModelDescription md = obj as ModelDescription;
            if (md == null)
                return false;

            return name == md.name &&
                category == md.category &&
                productName == md.productName &&
                productCode == md.productCode;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode()
                ^ category.GetHashCode()
                ^ productName.GetHashCode()
                ^ productCode.GetHashCode();
        }


        public virtual ModelDescription Clone()
        {
            ModelDescription discription = new ModelDescription();

            discription.Copy(this);

            return discription;
        }

        public virtual void Copy(ModelDescription srcDesc)
        {
            name = srcDesc.name;
            category = srcDesc.category;
            productName = srcDesc.productName;
            productCode = srcDesc.productCode;
            description = srcDesc.description;
            useByMasterModel = srcDesc.useByMasterModel;
            masterModelName = srcDesc.masterModelName;

            exportPacketFormat = srcDesc.exportPacketFormat.Clone();
            ReportPacketFormat = srcDesc.ReportPacketFormat.Clone();

            fiducialDistanceUm = srcDesc.fiducialDistanceUm;
        }

        public virtual string[] GetArgs()
        {
            return new string[] { Name };
        }

        public virtual void FromArgs(string[] args)
        {
            this.name = args[0];
        }

        public void Load(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                MessageBox.Show(String.Format("Can't find model description file. {0}", fileName));
                return;
            }

            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(fileName);

                Load(xmlDocument.DocumentElement);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Can't load model description file. {0}", fileName));
                return;
            }
        }

        public virtual void Load(XmlElement modelDescElement)
        {
            description = XmlHelper.GetValue(modelDescElement, "Description", "");

            category = XmlHelper.GetValue(modelDescElement, "Category", "");
            name = XmlHelper.GetValue(modelDescElement, "Name", "");

            productName = XmlHelper.GetValue(modelDescElement, "ProductName", "");
            productCode = XmlHelper.GetValue(modelDescElement, "ProductCode", "");
            masterModelName = XmlHelper.GetValue(modelDescElement, "MasterModelName", "");

            fiducialDistanceUm = Convert.ToSingle(XmlHelper.GetValue(modelDescElement, "FiducialDistanceUm", "0"));

            useByMasterModel = Convert.ToBoolean(XmlHelper.GetValue(modelDescElement, "UseByMasterModel", "False"));

            XmlElement exportPacketFormatElement = modelDescElement["ExportPacketFormat"];
            if (exportPacketFormatElement != null)
                exportPacketFormat.Load(exportPacketFormatElement);

            XmlElement reportPacketFormatElement = modelDescElement["ReportPacketFormat"];
            if (reportPacketFormatElement != null)
            {
                reportPacketFormat.Load(reportPacketFormatElement);
                reportPacketFormat.PacketStart = "";
                reportPacketFormat.PacketEnd = "";
                reportPacketFormat.Separator = ",";
            }

            string registrationDateStr = XmlHelper.GetValue(modelDescElement, "RegistrationDate", "");
            if (registrationDateStr == "")
            {
                int year = Convert.ToInt32(XmlHelper.GetValue(modelDescElement, "RegistrationDateYear", "2017"));
                int month = Convert.ToInt32(XmlHelper.GetValue(modelDescElement, "RegistrationDateMonth", "4"));
                int day = Convert.ToInt32(XmlHelper.GetValue(modelDescElement, "RegistrationDateDay", "27"));

                registrationDate = new DateTime(year, month, day);
            }
            else
            {
                if (DateTime.TryParseExact(registrationDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out registrationDate) == false)
                {
                    registrationDate = DateTime.Now;
                }
            }

            string lastModifiedDateStr = XmlHelper.GetValue(modelDescElement, "LastModifiedDate", "");
            if (DateTime.TryParseExact(lastModifiedDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out lastModifiedDate) == false)
            {
                lastModifiedDate = registrationDate;
            }

            registrant = XmlHelper.GetValue(modelDescElement, "Registrant", "");
        }

        public bool Save(string fileName)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();

                XmlElement modelDescElement = xmlDocument.CreateElement("", "ModelDescription", "");
                xmlDocument.AppendChild(modelDescElement);

                Save(modelDescElement);

                //xmlDocument.Save(fileName);
                XmlHelper.Save(xmlDocument, fileName);
                //using (FileStream fs = new FileStream(fileName, FileMode.Create))
                //{
                //    xmlDocument.Save(fs);
                //    fs.Flush(true);
                //    fs.Close();
                //}
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("ModelDescription::Save Fail : {0}", ex.Message));
                return false;
            }
        }

        public virtual void Save(XmlElement modelDescElement)
        {
            XmlHelper.SetValue(modelDescElement, "Description", description);

            XmlHelper.SetValue(modelDescElement, "Name", name);
            XmlHelper.SetValue(modelDescElement, "Category", category);
            XmlHelper.SetValue(modelDescElement, "ProductName", productName);
            XmlHelper.SetValue(modelDescElement, "ProductCode", productCode);
            XmlHelper.SetValue(modelDescElement, "MasterModelName", masterModelName);

            XmlHelper.SetValue(modelDescElement, "FiducialDistanceUm", fiducialDistanceUm.ToString());

            XmlHelper.SetValue(modelDescElement, "UseByMasterModel", useByMasterModel.ToString());

            XmlElement exportPacketFormatElement = modelDescElement.OwnerDocument.CreateElement("", "ExportPacketFormat", "");
            modelDescElement.AppendChild(exportPacketFormatElement);

            exportPacketFormat.Save(exportPacketFormatElement);

            XmlElement reportPacketFormatElement = modelDescElement.OwnerDocument.CreateElement("", "ReportPacketFormat", "");
            modelDescElement.AppendChild(reportPacketFormatElement);

            XmlHelper.SetValue(modelDescElement, "RegistrationDate", registrationDate.ToString("yyyy-MM-dd HH:mm:ss"));

            lastModifiedDate = DateTime.Now;
            XmlHelper.SetValue(modelDescElement, "LastModifiedDate", lastModifiedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            XmlHelper.SetValue(modelDescElement, "Registrant", registrant);

            
            reportPacketFormat.Save(reportPacketFormatElement);
        }
    }
}
