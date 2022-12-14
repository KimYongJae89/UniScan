using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.UI;
using DynMvp.Data.UI;
using DynMvp.UI;
using System.IO;
using UniEye.Base.Settings;
using DynMvp.UI.Touch;
using DynMvp.Base;
using UniEye.Base;
using UniScanM.CGInspector.Data;

namespace UniScanM.CGInspector.UI.MenuPage.SettingPanel
{
    public partial class SettingPageMonitoringPanel : UserControl, UniEye.Base.UI.IPage,IMultiLanguageSupport
    {
        Image fullImage = null;
        DrawBox dBox = null;

        public SettingPageMonitoringPanel()
        {
            InitializeComponent();

            dBox = new DrawBox();
            dBox.BackColor = Color.Gray;
            dBox.Dock = DockStyle.Fill;
            dBox.AutoFitStyle = AutoFitStyle.FitAll;
            dBox.pictureBox.Click += PictureBox_Click;
            dBox.SizeChanged += DBoxBig_SizeChanged;
            panelDrawBox.Controls.Add(dBox);

            StringManager.AddListener(this);
        }

        private void SettingPageMonitoringPanel_Load(object sender, EventArgs e)
        {

        }

        private void UpdateImage(Image image = null)
        {
            if (image == null)
                image = this.fullImage;

            
            dBox.ZoomFit();
            dBox.Invalidate();
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            CoordTransformer coordTransformer = dBox.GetCoordTransformer();

            MouseEventArgs me = (MouseEventArgs)e;
            Point clickedPt = me.Location;
            Point imagePt = coordTransformer.InverseTransform(clickedPt);
            //Point scaledPt = new Point(imagePt.X * 10, imagePt.Y * 10);
            float newYPos = imagePt.Y * 1.0f / this.fullImage.Height;

            SimpleProgressForm form = new SimpleProgressForm();
            form.Show(() => SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel));

            UpdateImage();
        }

        private void DBoxBig_SizeChanged(object sender, EventArgs e)
        {
            dBox.ZoomFit();
        }

        public void UpdateControl(string item, object value)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateControlDelegate(UpdateControl), item, value);
                return;
            }

            switch (item)
            {
                case "FullImage":
                    fullImage = (Image)value;
                    UpdateImage();
                    this.dBox.ZoomFit();
                    break;
            }
        }


        public void PageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag)
            {
                Model model = SystemManager.Instance().CurrentModel as Model;
                string imagePath = model.GetImagePath();
                string imageName = model.GetImageName("bmp");
                string imageFile = Path.Combine(imagePath, imageName);
                Image image = null;
                if (File.Exists(imageFile))
                {
                  image =  ImageHelper.LoadImage(imageFile);
                    //imageD = new Image2D(imageFile);
                }
                else
                {
                    image = new Bitmap(100, 3200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                }
                this.UpdateImage(image);
                image?.Dispose();
                //this.fullImage = model?.FullImage as Image2D;
                //UpdateImage(this.fullImage);
                //Bitmap bitmap = model?.FullImage?.ToBitmap();
                //dBox.UpdateImage(bitmap);
                //bitmap?.Dispose();
                //dBox.ZoomFit();
            }
        }
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
