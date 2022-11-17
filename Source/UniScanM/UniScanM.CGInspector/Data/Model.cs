using DynMvp.Base;
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
using UniScanM.Data;
using System.Diagnostics;
using System.IO;
using DynMvp.Data;

namespace UniScanM.CGInspector.Data
{
    public class Model: UniScanM.Data.Model
    {
        public ImageBuffer ImageBuffer => this.imageBuffer;
        ImageBuffer imageBuffer = new ImageBuffer();

        public Model() : base()
        {
            this.inspectParam = new InspectParam();
        }

        public string GetImageName(string extension = "Bmp")
        {
            return string.Format("Prev.{0}", extension.ToLower());
        }

        public override bool IsTaught()
        {
            // PLC에서 모델이 입력되지 않으면 항상 티칭함.
            if (this.Name == "Unknown")
                return false;

            return base.IsTaught();
        }

        public override void SaveModel(XmlElement xmlElement)
        {
            base.SaveModel(xmlElement);

            this.imageBuffer.Save(GetImagePath(), 0, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        public override void LoadModel(XmlElement xmlElement)
        {
            base.LoadModel(xmlElement);

            InspectionStep inspectionStep = GetInspectionStep(0);
            if (inspectionStep == null)
                inspectionStep = CreateInspectionStep();

            TargetGroup targetGroup = inspectionStep.GetTargetGroup(0);
            if (targetGroup == null)
                targetGroup = inspectionStep.CreateTargetGroup();

            Target target = targetGroup.GetTarget(0);
            if (target == null)
                target = targetGroup.CreateTarget();

            this.imageBuffer.Initialize(1, 1);
            this.imageBuffer.Load(this.GetImagePath(), 0, 0, 0);
        }

        public void UpdateFullImage()
        {
        }
    }

    public class ModelDescription : UniScanM.Data.ModelDescription
    {
        public ModelDescription() : base() { }

        public override void Load(XmlElement modelDescElement)
        {
            base.Load(modelDescElement);

            string imageString = XmlHelper.GetValue(modelDescElement, "Image", "");
            Bitmap bitmap = ImageHelper.Base64StringToBitmap(imageString);
        }

        public override void Save(XmlElement modelDescElement)
        {
            base.Save(modelDescElement);
        }
        
        public override DynMvp.Data.ModelDescription Clone()
        {
            ModelDescription discription = new ModelDescription();
            discription.Copy(this);

            return discription;
        }

        public override void Copy(DynMvp.Data.ModelDescription srcDesc)
        {
            base.Copy(srcDesc);
        }
    }
    
}
