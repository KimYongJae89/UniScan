using DynMvp.Base;
using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.Gravure.Vision.Trainer;

namespace UniScanG.Data.Model
{
    public class Model : UniScanG.Common.Data.Model
    {
        public bool ImageModified { get => this.imageModified; set => this.imageModified = value; }
        bool imageModified = false;

        public float ScaleFactorF { get => 1f / this.scaleFactor;}
        public int ScaleFactor { get => this.scaleFactor; set => this.scaleFactor = value; }
        private int scaleFactor = 1;

        // for RCI---------------------
        public Gravure.Vision.RCI.RCIOptions RCIOptions { get; set; } 
        public Gravure.Vision.RCI.Trainer.RCITrainResult RCITrainResult { get; set; }
        // ----------------------------

        // 전극 점유율
        public float ChipShare100p { get => this.chipShare100p; set => this.chipShare100p = value; }
        float chipShare100p;

        // Trainer
        public TrainerModelParam TrainerModelParam => this.trainerModelParam;
        TrainerModelParam trainerModelParam;

        // Calculator
        public CalculatorModelParam CalculatorModelParam => this.calculatorModelParam;
        CalculatorModelParam calculatorModelParam;

        // Detector
        public DetectorModelParam DetectorModelParam => this.detectorModelParam;
        DetectorModelParam detectorModelParam;

        // Extender
        public WatcherModelParam WatcherModelParam => this.watcherModelParam;
        WatcherModelParam watcherModelParam;
        
        public Model()
        {
            this.scaleFactor = 1;
            this.chipShare100p = 0;

            this.trainerModelParam = new TrainerModelParam();
            this.calculatorModelParam = new CalculatorModelParam();
            this.detectorModelParam = new DetectorModelParam();
            this.watcherModelParam = new WatcherModelParam();

            this.RCIOptions = new Gravure.Vision.RCI.RCIOptions();
            this.RCITrainResult = new Gravure.Vision.RCI.Trainer.RCITrainResult();
        }

        public override bool IsTaught()
        {
            //Size sheetSizePx = this.calculatorModelParam.SheetSizePx;
            //return base.IsTaught() && (sheetSizePx.Width * sheetSizePx.Height > 0);
            return base.IsTaught();
        }

        public override void Setup(int numCamera, int numLight, int numLightType)
        {
            base.Setup(numCamera, numLight, numLightType);

            LightParamSet.Initialize(numLight, 0);
        }

        public override void SaveModel(XmlElement xmlElement)
        {
            base.SaveModel(xmlElement);

            XmlHelper.SetValue(xmlElement, "ScaleFactor", this.scaleFactor);
            XmlHelper.SetValue(xmlElement, "ChipShare100p", this.chipShare100p);

            this.trainerModelParam.Save(xmlElement, "TrainerModelParam");

            this.calculatorModelParam.Save(xmlElement, "CalculatorModelParam");
            this.detectorModelParam.Save(xmlElement, "DetectorModelParam");
            this.watcherModelParam.Save(xmlElement, "WatcherModelParam");

            this.RCIOptions.Save(xmlElement, "RCIOptions");
            this.RCITrainResult.Save(xmlElement, "RCITrainResult");
        }

        public override void LoadModel(XmlElement xmlElement)
        {
            base.LoadModel(xmlElement);

            // 조명값 초기화
            this.LightParamSet.LightParamList.RemoveAll(f =>
            {
                float ff;
                return !float.TryParse(f.Name, out ff);
            });
            this.LightParamSet.LightParamList.Sort(new LightParamComparer());

            this.scaleFactor = XmlHelper.GetValue(xmlElement, "ScaleFactor", this.scaleFactor);
            this.chipShare100p = XmlHelper.GetValue(xmlElement, "ChipShare100p", this.chipShare100p);

            this.trainerModelParam.Load(xmlElement, "TrainerModelParam");
            this.calculatorModelParam.Load(xmlElement, "CalculatorModelParam");
            this.detectorModelParam.Load(xmlElement, "DetectorModelParam");
            this.watcherModelParam.Load(xmlElement, "WatcherModelParam");

            this.RCIOptions.CopyFrom(Gravure.Vision.RCI.RCIOptions.Load(xmlElement, "RCIOptions"));
            this.RCITrainResult = Gravure.Vision.RCI.Trainer.RCITrainResult.Load(xmlElement, "RCITrainResult");

            RenewModel();
        }

        private void RenewModel()
        {
            // 조명값 프리셋이 3개 이하이면 기본조명으로 재설정
            if (this.LightParamSet.LightParamList.Count < 3)
            {
                this.LightParamSet.LightParamList.Clear();
                AdditionalSettings additionalSettings = AdditionalSettings.Instance();
                if (additionalSettings != null)
                    this.LightParamSet.LightParamList.AddRange(additionalSettings.DefaultLightParamList.ToArray());
            }
        }

        public new ModelDescription ModelDescription
        {
            get { return (ModelDescription)modelDescription; }
        }

        public Bitmap GetPreviewImage(string postfix)
        {
            ModelManager modelManager = SystemManager.Instance().ModelManager as ModelManager;
            if (modelManager == null)
                return null;

            return modelManager.GetPreviewImage((ModelDescription)modelDescription, postfix);
        }

        public string GetPreviewImagePath(string postfix)
        {
            ModelManager modelManager = SystemManager.Instance().ModelManager as ModelManager;
            if (modelManager == null)
                return null;

            return modelManager.GetPreviewImagePath((ModelDescription)modelDescription, postfix);
        }
    }

    public class LightParamComparer : Comparer<DynMvp.Devices.LightParam>
    {
        public override int Compare(LightParam x, LightParam y)
        {
            float xx, yy;

            bool xxx = float.TryParse(x.Name, out xx);
            bool yyy = float.TryParse(y.Name, out yy);
            if (xxx && yyy)
                return xx.CompareTo(yy);

            return x.Name.CompareTo(y.Name);
        }
    }
}