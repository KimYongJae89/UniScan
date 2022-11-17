using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.IO;
using System.Xml;

namespace DataCollector
{
    public class Settings
    {
        bool userChecked = false;

        bool fix = false;
        string bin;
        string config;
        string log;
        string model;
        string remoteResult;
        string result;
        string state;
        string temp;
        string update;
        string virtualImage;

        bool useNetworkFolder = false;
        string companyLogo;
        string productLogo;

        public bool Fix
        {
            get { return fix; }
            set { fix = value; }
        }

        public string Bin
        {
            get { return bin; }
            set { bin = value; }
        }

        public string Config
        {
            get { return config; }
            set { config = value; }
        }

        public string Log
        {
            get { return log; }
            set { log = value; }
        }

        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        public string RemoteResult
        {
            get { return remoteResult; }
            set { remoteResult = value; }
        }

        public string Result
        {
            get { return result; }
            set { result = value; }
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        public string Temp
        {
            get { return temp; }
            set { temp = value; }
        }

        public string Update
        {
            get { return update; }
            set { update = value; }
        }

        public string VirtualImage
        {
            get { return virtualImage; }
            set { virtualImage = value; }
        }

        public bool UseNetworkFolder
        {
            get { return useNetworkFolder; }
            set { useNetworkFolder = value; }
        }

        public string CompanyLogo
        {
            get { return companyLogo; }
            set { companyLogo = value; }
        }

        public string ProductLogo
        {
            get { return productLogo; }
            set { productLogo = value; }
        }

        static Settings _instance;
        public static Settings Instance()
        {
            if (_instance == null)
            {
                _instance = new Settings();
            }

            return _instance;
        }

        private Settings()
        {
            SetDefault();
        }

        private void SetDefault()
        {
            userChecked = false;

            fix = false;

            bin = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Bin"));
            config = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Config"));
            log = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Log"));
            model = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Model"));
            remoteResult = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "RemoteResult"));
            result = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Result"));
            state = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "State"));
            temp = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Temp"));
            update = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Update"));
            virtualImage = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "VirtualImage"));

            useNetworkFolder = false;
            companyLogo = "";
            productLogo = "";
        }

        public void Load()
        {
            string fileName = Path.Combine(config, "path.xml");
            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
            {
                CreateDirectory();
                Save();
                return;
            }

            XmlElement pathElement = xmlDocument["Path"];
            if (pathElement == null)
                return;

            fix = Convert.ToBoolean(XmlHelper.GetValue(pathElement, "Fix", "false"));
            
            string binPath = XmlHelper.GetValue(pathElement, "Bin", bin);
            string configPath = XmlHelper.GetValue(pathElement, "Config", config);
            string logPath = XmlHelper.GetValue(pathElement, "Log", log);
            string modelPath = XmlHelper.GetValue(pathElement, "Model", model);
            string remoteResultPath = XmlHelper.GetValue(pathElement, "RemoteResult", remoteResult);
            string resultPath = XmlHelper.GetValue(pathElement, "Result", result);
            string statePath = XmlHelper.GetValue(pathElement, "State", state);
            string tempPath = XmlHelper.GetValue(pathElement, "Temp", temp);
            string updatePath = XmlHelper.GetValue(pathElement, "Update", update);
            string virtualImagePath = XmlHelper.GetValue(pathElement, "VirtualImage", virtualImage);

            if (fix == false &&
                (bin != binPath || config != configPath || log != logPath || model != modelPath || remoteResult != remoteResultPath || result != resultPath || state != statePath || temp != tempPath || update != updatePath || virtualImage != virtualImagePath))
            {
                string message = "Path is changed. Update Path Setting?";
                if (MessageForm.Show(null, message, MessageFormType.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    fix = false;
                }
                else
                {
                    fix = true;
                    bin = binPath;
                    config = configPath;
                    log = logPath;
                    model = modelPath;
                    remoteResult = remoteResultPath;
                    result = resultPath;
                    state = statePath;
                    temp = tempPath;
                    update = updatePath;
                    virtualImage = virtualImagePath;
                }
            }
            
            useNetworkFolder = Convert.ToBoolean(XmlHelper.GetValue(pathElement, "UseNetworkFolder", useNetworkFolder.ToString()));
            companyLogo = XmlHelper.GetValue(pathElement, "CompanyLogo", companyLogo);
            productLogo = XmlHelper.GetValue(pathElement, "ProductLogo", productLogo);

            CreateDirectory();
            this.Save();
        }

        private void CreateDirectory()
        {
            try
            {
                Directory.CreateDirectory(bin);
                Directory.CreateDirectory(config);
                Directory.CreateDirectory(log);
                Directory.CreateDirectory(model);
                Directory.CreateDirectory(remoteResult);
                Directory.CreateDirectory(result);
                Directory.CreateDirectory(state);
                Directory.CreateDirectory(temp);
                Directory.CreateDirectory(update);
                Directory.CreateDirectory(virtualImage);
            }
            catch (DirectoryNotFoundException)
            {
                MessageForm.Show(null, "Some Path is not exist!");
                SetDefault();
            }
        }
        
        public void Save()
        {

            //fix = Convert.ToBoolean(XmlHelper.GetValue(pathElement, "", "false"));

            //string binPath = XmlHelper.GetValue(pathElement, "", bin);
            //string configPath = XmlHelper.GetValue(pathElement, "", config);
            //string logPath = XmlHelper.GetValue(pathElement, "Log", log);
            //string modelPath = XmlHelper.GetValue(pathElement, "Model", model);
            //string remoteResultPath = XmlHelper.GetValue(pathElement, "RemoteResult", remoteResult);
            //string resultPath = XmlHelper.GetValue(pathElement, "Result", result);
            //string statePath = XmlHelper.GetValue(pathElement, "State", state);
            //string tempPath = XmlHelper.GetValue(pathElement, "Temp", temp);
            //string updatePath = XmlHelper.GetValue(pathElement, "Update", update);
            //string virtualImagePath = XmlHelper.GetValue(pathElement, "Virtual", );

            string fileName = String.Format(@"{0}\path.xml", config);

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement pathElement = xmlDocument.CreateElement("", "Path", "");
            xmlDocument.AppendChild(pathElement);

            XmlHelper.SetValue(pathElement, "Fix", fix.ToString());
            XmlHelper.SetValue(pathElement, "Bin", bin);
            XmlHelper.SetValue(pathElement, "Config", config);
            XmlHelper.SetValue(pathElement, "Log", log);
            XmlHelper.SetValue(pathElement, "Model", model);
            XmlHelper.SetValue(pathElement, "RemoteResult", remoteResult);
            XmlHelper.SetValue(pathElement, "Result", result);
            XmlHelper.SetValue(pathElement, "State", state);
            XmlHelper.SetValue(pathElement, "Temp", temp);
            XmlHelper.SetValue(pathElement, "Update", update);
            XmlHelper.SetValue(pathElement, "VirtualImage", virtualImage);

            XmlHelper.SetValue(pathElement, "UseNetworkFolder", useNetworkFolder.ToString());
            XmlHelper.SetValue(pathElement, "CompanyLogo", companyLogo);
            XmlHelper.SetValue(pathElement, "ProductLogo", productLogo);

            XmlHelper.Save(xmlDocument, fileName);
        }

        public void ClearTempDirectory()
        {
            string[] files = Directory.GetFiles(temp);
            foreach(string file in files)
            {
                File.Delete(file);
            }
        }
    }
}
