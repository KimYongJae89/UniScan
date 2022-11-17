using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using DynMvp.Data.UI;

using UniEye.Base;
using UniEye.Base.UI;
using UniEye.Base.Data;
using System.IO;

using UniScanM.ColorSens.Data;
using UniScanM.ColorSens.Operation;

namespace UniScanM.ColorSens.UI
{
    public partial class InspectionPanelLeft : UserControl, IInspectionPanel, IMultiLanguageSupport
    {
        CanvasPanel canvasPanel;
        Bitmap displayBitmap = null;
        object LockDisplayBitmap = new object();

        public InspectionPanelLeft()//생성자
        {
            InitializeComponent();
            
            this.Dock = DockStyle.Fill;

            //1.Main Image View(Left View)
           
            this.canvasPanel = new CanvasPanel(true);
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.ReadOnly = true;
            this.canvasPanel.SetPanMode();
            this.canvasPanel.BackColor = Color.Black;
            this.canvasPanel.SizeChanged += DrawBox_SizeChanged;
            this.Controls.Add(canvasPanel);

            //  Color2048.png
            string initImagePath = ".\\Color2048.png";
            if (File.Exists(initImagePath) == true)
            {
                Bitmap bitmap = (Bitmap)Image.FromFile(initImagePath);
                this.canvasPanel.UpdateImage(bitmap);
                this.canvasPanel.ZoomFit();
            }
            StringManager.AddListener(this);
        }

        private void DrawBox_SizeChanged(object sender, EventArgs e)
        {
            canvasPanel?.ZoomFit();
        }

        private void InspectPage_Load(object sender, EventArgs e)
        {
            canvasPanel?.ZoomFit();
        }
        
        //IInspectionPanel
        public void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ProductInspectedDelegate(ProductInspected), inspectionResult);
                return;
            }

            ColorSens.Data.InspectionResult myInspectionResult = (ColorSens.Data.InspectionResult)inspectionResult;
            lock (LockDisplayBitmap)
            {
                canvasPanel?.UpdateImage(myInspectionResult.DisplayBitmap);
                displayBitmap?.Dispose();
                displayBitmap = myInspectionResult.DisplayBitmap;
                canvasPanel?.ZoomFit();
                canvasPanel.Invalidate();
            }
        }


        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void ClearPanel()
        {
            canvasPanel.UpdateImage(null);
            canvasPanel.Invalidate();
        }

        public void OnPreInspection()
        {
            throw new NotImplementedException();
        }

        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, DynMvp.InspData.InspectionResult inspectionResult)
        {
            throw new NotImplementedException();
        }

        public void TargetGroupInspected(TargetGroup targetGroup, DynMvp.InspData.InspectionResult inspectionResult, DynMvp.InspData.InspectionResult objectInspectionResult)
        {
            throw new NotImplementedException();
        }

        public void TargetInspected(Target target, DynMvp.InspData.InspectionResult targetInspectionResult)
        {
            throw new NotImplementedException();
        }

        public void UpdateImage(DeviceImageSet deviceImageSet, int groupId, DynMvp.InspData.InspectionResult inspectionResult)
        {
            Bitmap bitmap = deviceImageSet.ImageList2D[0]?.ToBitmap();
            canvasPanel.UpdateImage(bitmap);
            canvasPanel?.ZoomFit();
            //throw new NotImplementedException();
        }

        public void OnPostInspection()
        {
            throw new NotImplementedException();
        }

        public void ModelChanged(DynMvp.Data.Model model = null)
        {
            throw new NotImplementedException();
        }

        public void InfomationChanged(object obj = null)
        {
            //throw new NotImplementedException();
        }

        public void EnterWaitInspection()
        {

            //throw new NotImplementedException();
        }

        public void ExitWaitInspection()
        {
            //throw new NotImplementedException();
        }
    }
}
