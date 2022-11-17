using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.UI;
using System.Drawing;
using DynMvp.Devices.MotionController;
using DynMvp.Devices;
using System.Windows.Media.Imaging;
using UniScanWPF.Screen.PinHoleColor.Device;
using UniScanWPF.Screen.PinHoleColor.Properties;
using System.Windows.Media;
using UniScanWPF.Helper;

namespace UniScanWPF.Screen.PinHoleColor.Data
{
    public class ModelDescription : DynMvp.Data.ModelDescription
    {
        static BitmapSource onImage;
        static BitmapSource offImage;

        BitmapSource[] images;
        public BitmapSource[] Images
        {
            get { return images; }
        }

        public override string Name
        {
            get { return name; }
            set
            {
                name = value;
                for (int i = 0; i < name.Length; i++)
                {
                    if (name[i] == '0')
                        images[i] = offImage;
                    else
                        images[i] = onImage;
                }
            }
        }

        public ModelDescription() : base()
        {
            if (onImage == null)
                onImage = WPFImageHelper.ConvertImage(Resources.Model_Circle);

            if (offImage == null)
                offImage = WPFImageHelper.ConvertImage(Resources.Model_Circle_Empty);

            images = new BitmapSource[Enum.GetNames(typeof(PortMap.ModelPortName)).Length];
        }
    }
}
