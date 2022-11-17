using AutoParamTuner.Base;
using AutoParamTuner.Model;
using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG;
using UniScanG.Data.Inspect;

namespace AutoParamTuner.UI
{
    internal class MainFormViewModel : ViewModel
    {
        public UniScanG.Data.Model.Model ModelG { get => base.Model.Get<UniScanG.Data.Model.Model>("ModelG"); }
        public string ModelPath { get => base.Model.Get<string>("ModelPath"); set => base.Model.Set<string>("ModelPath", value); }
        public Image2D Image { get => base.Model.Get<Image2D>("Image"); }
        public List<SwapParamItem> ParamItemList { get => base.Model.Get<List<SwapParamItem>>("ParamItemList");}

        public double Progress100 { get; private set; }

        public MainFormViewModel(TunerModel tunerModel) : base(tunerModel) { }

        public void ModelPathCommand(Control sender, object parameter)
        {
            string defaultPath = (string)parameter;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.AddExtension = true;
            openFileDialog.Filter = "XML Files|*.xml";
            openFileDialog.FileName = "Model.xml";
            openFileDialog.InitialDirectory = defaultPath;

            if (openFileDialog.ShowDialog(sender) == DialogResult.OK)
            {
                base.Model.Set<string>("ModelPath", openFileDialog.FileName);
                OpenModel(openFileDialog.FileName);
            }
        }

        private void OpenModel(string modelXmlFileName)
        {
            string dirName = Path.GetDirectoryName(modelXmlFileName);
            string modelXml = modelXmlFileName;
            if (!File.Exists(modelXml))
                throw new FileNotFoundException("File is not exist");

            string modelDecXml = Path.Combine(dirName, "ModelDescription.xml");
            DynMvp.Data.ModelDescription md = new UniScanG.Data.Model.ModelDescription();
            md.Load(modelDecXml);

            DynMvp.Data.ModelManager modelManager = new UniScanG.Data.Model.ModelManager();
            string modelPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..", @"Model"));
            modelManager.Refresh(modelPath);
            UniScanG.Data.Model.Model modelG = (UniScanG.Data.Model.Model)modelManager.LoadModel(md, null);
            SystemManager.Instance().CurrentModel = modelG;
            base.Model.Set<UniScanG.Data.Model.Model>("ModelG", modelG);

            string imagePath = modelG.GetImagePath();
            string imageName = modelG.GetImageName(0, 0, 0);
            string image = Path.Combine(imagePath, imageName);
            Image2D image2D = new Image2D(image);
            base.Model.Set<Image2D>("Image", image2D);
        }

        public void StartCommand(Control sender, object parameter)
        {
            if (ModelG == null)
                OpenModel(ModelPath);

            Image2D modelImage = base.Model.Get<Image2D>("Image");
            if (modelImage == null)
                throw new Exception("Image is not loaded");

            Tuner.Inspecter inspector = new Tuner.Inspecter(sender);
            DynMvp.Vision.ScaledCalibration calibration = new DynMvp.Vision.ScaledCalibration(new SizeF(14, 14));
            calibration.Calibrate(14, 14);

            Dictionary<ISwapParamItem, TunerResult> resultDic = new Dictionary<ISwapParamItem, TunerResult>();
            List<ISwapParamItem> list = new List<ISwapParamItem>(this.ParamItemList);
            list.RemoveAll(f => !f.Use);
            double step = 100.0 / Math.Max(list.Count, 1);
            Set("Progress100", 0.0);

            list.ForEach(f =>
            {
                f.Apply(ModelG);
                TunerResult result = inspector.Inspect(ModelG, modelImage, calibration);
                resultDic.Add(f, result);
                Set("Progress100", this.Progress100 + step);
            });
            //this.TunerModel.Set<Dictionary<ISwapParamItem, TunerResult>>("ResultDictionary", resultDic);

            Set("Progress100", 100.0);

            ResultModel resultModel = new ResultModel(Image, resultDic);
            (new ResultForm(new ResultFormViewModel(resultModel))).Show();
        }
        public void ShowCommand(Control sender, object parameter)
        {
            ResultModel resultModel = new ResultModel(Image);
            (new ResultForm(new ResultFormViewModel(resultModel))).Show();
        }
    }
}
