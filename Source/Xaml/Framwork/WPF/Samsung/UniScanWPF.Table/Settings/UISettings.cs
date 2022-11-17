using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;
using UniEye.Base.Settings;

namespace UniScanWPF.Table.Settings
{
    public class UISettings
    {
        static UISettings _instance;

        public ScaleTransform ScaleTransform { get => this.scaleTransform; set => this.scaleTransform = value.Clone(); }
        ScaleTransform scaleTransform;

        private UISettings()
        {
            this.scaleTransform = new ScaleTransform();
        }

        public static UISettings Instance()
        {
            if (_instance == null)
                UISettings.Load();

            return _instance;
        }

        public void Save()
        {
            string file = Path.Combine(PathSettings.Instance().Config, "UiSettings.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(UISettings));
            using (var writer = new StreamWriter(file))
                serializer.Serialize(writer, this);
        }

        private static void Load()
        {
            string file = Path.Combine(PathSettings.Instance().Config, "UiSettings.xml");
            if (!File.Exists(file))
            {
                _instance = new UISettings();
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(UISettings));
            using (var reader = new StreamReader(file))
            {
                _instance = (UISettings)serializer.Deserialize(reader);
            }
        }
    }
}
