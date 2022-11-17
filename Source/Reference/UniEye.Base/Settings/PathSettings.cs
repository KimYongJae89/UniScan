using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms.Design;
using System.Xml;

using DynMvp.Base;
using DynMvp.UI.Touch;

using DynMvp.UI.EditorAttribute;

namespace UniEye.Base.Settings
{
    public class PathSettings
    {
        public bool UseManualSetting { get => this.useManualSetting; set => this.useManualSetting = value; }
        bool useManualSetting = false;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string Bin { get => this.bin; set => this.bin = value; }
        string bin;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string Config { get => this.config; set => this.config = value; }
        string config;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string Log { get => this.log; set => this.log = value; }
        string log;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string Model { get => this.model; set => this.model = value; }
        string model;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string RemoteResult { get => this.remoteResult; set => this.remoteResult = value; }
        string remoteResult;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string Result { get => this.result; set => this.result = value; }
        string result;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string State { get => this.state; set => this.state = value; }
        string state;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string Temp { get => this.temp; set => this.temp = value; }
        string temp;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string Update { get => this.update; set => this.update = value; }
        string update;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string VirtualImage { get => this.virtualImage; set => this.virtualImage = value; }
        string virtualImage;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string HWMonitor { get => this.hWMonitor; set => this.hWMonitor = value; }
        string hWMonitor;

        public bool UseNetworkFolder { get => this.useNetworkFolder; set => this.useNetworkFolder = value; }
        bool useNetworkFolder = false;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string CompanyLogo { get => this.companyLogo; set => this.companyLogo = value; }
        string companyLogo;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string ProductLogo { get => this.productLogo; set => this.productLogo = value; }
        string productLogo;

        static PathSettings _instance;
        public static PathSettings Instance()
        {
            if (_instance == null)
            {
                _instance = new PathSettings();
                _instance.Load();
            }

            return _instance;
        }

        private PathSettings()
        {
            SetDefault();
        }

        private void SetDefault()
        {
            this.useManualSetting = false;

            this.bin = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Bin"));
            this.config = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Config"));
            this.log = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Log"));
            this.model = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Model"));
            this.remoteResult = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "RemoteResult"));
            this.result = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Result"));
            this.state = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "State"));
            this.temp = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Temp"));
            this.update = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Update"));
            this.virtualImage = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "VirtualImage"));
            this.hWMonitor = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "HWMonitor"));

            this.useNetworkFolder = false;
            this.companyLogo = "";
            this.productLogo = "";
        }

        public void Load()
        {
            string fileName = Path.Combine(config, "path.xml");
            try
            {
                string basePath = Path.GetFullPath(Environment.CurrentDirectory);
                string[] defaultPaths = new string[]
                {
                    this.bin,
                    this.config,
                    this.log,
                    this.model,
                    this.result,
                    this.remoteResult,
                    this.state,
                    this.temp,
                    this.update,
                    this.virtualImage,
                    this.hWMonitor,
                }.Select(f => f.ToLower()).ToArray();

                if (!File.Exists(fileName))
                    return;

                XmlDocument xmlDocument = XmlHelper.Load(fileName);
                XmlElement pathElement = xmlDocument["Path"];
                if (pathElement == null)
                    return;

                this.useManualSetting = Convert.ToBoolean(XmlHelper.GetValue(pathElement, "UseManualSetting", "false"));
                string[] loadPaths = new string[]
                {
                    XmlHelper.GetValue(pathElement, "Bin", this.bin),
                    XmlHelper.GetValue(pathElement, "Config", this.config),
                    XmlHelper.GetValue(pathElement, "Log", this.log),
                    XmlHelper.GetValue(pathElement, "Model", this.model),
                    XmlHelper.GetValue(pathElement, "Result", this.result),
                    XmlHelper.GetValue(pathElement, "RemoteResult", this.remoteResult),
                    XmlHelper.GetValue(pathElement, "State", this.state),
                    XmlHelper.GetValue(pathElement, "Temp", this.temp),
                    XmlHelper.GetValue(pathElement, "Update", this.update),
                    XmlHelper.GetValue(pathElement, "VirtualImage", this.virtualImage),
                    XmlHelper.GetValue(pathElement, "HWMonitor", this.hWMonitor),
                };

                if (!defaultPaths.SequenceEqual(loadPaths.Select(f => f.ToLower())) && !this.useManualSetting)
                {
                    string message = "Path is changed. Update Path Setting?";
                    if (MessageForm.Show(null, message, MessageFormType.YesNo) == System.Windows.Forms.DialogResult.No)
                        this.useManualSetting = true;
                }

                if (this.useManualSetting)
                {
                    this.bin = loadPaths[0];
                    this.config = loadPaths[1];
                    this.log = loadPaths[2];
                    this.model = loadPaths[3];
                    this.result = loadPaths[4];
                    this.remoteResult = loadPaths[5];
                    this.state = loadPaths[6];
                    this.temp = loadPaths[7];
                    this.update = loadPaths[8];
                    this.virtualImage = loadPaths[9];
                    this.hWMonitor = loadPaths[10];
                }

                this.useNetworkFolder = Convert.ToBoolean(XmlHelper.GetValue(pathElement, "UseNetworkFolder", this.useNetworkFolder));
                this.companyLogo = XmlHelper.GetValue(pathElement, "CompanyLogo", this.companyLogo);
                this.productLogo = XmlHelper.GetValue(pathElement, "ProductLogo", this.productLogo);
            }
            catch { }
            finally
            {
                Save();
                CreateDirectory();
            }
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
                Directory.CreateDirectory(hWMonitor);
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

            XmlHelper.SetValue(pathElement, "UseManualSetting", this.useManualSetting);
            XmlHelper.SetValue(pathElement, "Bin", this.bin);
            XmlHelper.SetValue(pathElement, "Config", this.config);
            XmlHelper.SetValue(pathElement, "Log", this.log);
            XmlHelper.SetValue(pathElement, "Model", this.model);
            XmlHelper.SetValue(pathElement, "Result", this.result);
            XmlHelper.SetValue(pathElement, "RemoteResult", this.remoteResult);
            XmlHelper.SetValue(pathElement, "State", this.state);
            XmlHelper.SetValue(pathElement, "Temp", this.temp);
            XmlHelper.SetValue(pathElement, "Update", this.update);
            XmlHelper.SetValue(pathElement, "VirtualImage", this.virtualImage);

            XmlHelper.SetValue(pathElement, "UseNetworkFolder", this.useNetworkFolder);
            XmlHelper.SetValue(pathElement, "CompanyLogo", this.companyLogo);
            XmlHelper.SetValue(pathElement, "ProductLogo", this.productLogo);

            XmlHelper.Save(xmlDocument, fileName);
        }

        public void ClearTempDirectory()
        {
            string[] files = Directory.GetFiles(temp);
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
    }
}
