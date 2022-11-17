using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Data;
using System.Xml;
using DynMvp.UI;
using System.IO;
using System.Drawing;
using DynMvp.InspData;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.Light;
using DynMvp.Devices;

namespace UniScanX.MPAlignment.Data
{
    public class MpProbeResult : VisionProbeResult
    {

    }

    public class MpProbe : VisionProbe
    {

    }

    public class ConnerProbeResult :VisionProbeResult
    {

    }

    public class ConnerProbe : VisionProbe
    {

    }

    public class Target : DynMvp.Data.Target
    {
        // target Image
        // Probe List 는 이미 부모에 있음.
        private AxisPosition basePosition = null; //모션 좌표 MC 좌표임. FOV 좌 상단? 아님 중심? 최초 티칭시 설정 좌료
        public AxisPosition BasePosition //metric(mm) MC00
        {
            get { return basePosition; }
            set
            {
                basePosition = value;
                alignedPosition = basePosition.Clone();
            }
        }

        // 모델 피듀셜 보정값으로 보정된 위치. ㅡ
        // 모델의 좌상단에서 타겟 BasePosition 차이값 및 회전량 적용된 AlignedPosition 
        private AxisPosition alignedPosition = null; 
        public AxisPosition AlignedPosition
        {
            get { return alignedPosition; }
            set { alignedPosition = value; }
        } //metric(mm) MC00
        private LightParamSet lightParamSet = new LightParamSet();
        public LightParamSet LightParamSet
        {
            get { return lightParamSet; }
            set { lightParamSet = value; }
        }

        PositionAligner aliner;
        public PositionAligner Aliner
        {
            get { return aliner; }
            set { aliner = value; }
        }
    }

    public class MPModel : DynMvp.Data.Model
    {
        public MPModel()
        {
           
        }
        string resultPath;
        public string ResultPath { get => resultPath; set => resultPath = value; }
        public SizeF PatternSize  // metric (mm)
        {
            get
            {
                if (base.modelDescription == null) return SizeF.Empty;
                var Mdescirp  = base.modelDescription as ModelDescription;
                return Mdescirp.PatternSize;
            }
            set
            {
                if (base.modelDescription != null)
                {
                    var Mdsc = base.modelDescription as ModelDescription;
                    Mdsc.PatternSize = value;
                }
            }
        }
        //전체 패턴 이미지 .

        private List<DynMvp.Data.Target> targetList = new List<DynMvp.Data.Target>();
        public List<DynMvp.Data.Target> TargetList
        {
            get { return targetList; }
        }

        private int modelFiducialId = -1;
        public int ModelFiducialId
        {
            get { return modelFiducialId; }
            set { modelFiducialId = value; }
        }

        private FiducialSet fiducialSet = new FiducialSet();
        public FiducialSet FiducialSet
        {
            get { return fiducialSet; }
        }

        public override bool IsTaught()
        {
            return IsTrained;
        }

        public bool IsTrained
        {
            get { return ((ModelDescription)ModelDescription).IsTrained; }
            set { ((ModelDescription)ModelDescription).IsTrained = value; }
        }


        new public void GetProbes(List<Probe> probeList)
        {
           // probeList = listMpProbe.ToArray().ToList<Probe>();
        }

        public FigureGroup AppendFigures(FigureGroup figureGroup, Pen pen, bool includeProbe = false)
        {
            foreach (Target target in targetList)
            {
                target.AppendFigures(figureGroup, pen, includeProbe);
            }
            return figureGroup;
        }

        public override void SaveModel(XmlElement xmlElement)
        {
            //PatternSize  //save load 할까 말까?
            //fiducialSet
            //listMpProbe

        }

        public override void LoadModel(XmlElement xmlElement)
        {

        }
        public void SaveDefaultImage(Bitmap image)
        {
            string dirPath = String.Format(@"{0}\DefaultImage\", ModelPath);
            if (Directory.Exists(dirPath) == false)
            {
                Directory.CreateDirectory(dirPath);
            }
            string filePath = Path.Combine(dirPath, "DefaultImage.bmp");
            //Task.Run(() => image.Save(filePath));
            image.Save(filePath);
        }

        public Bitmap LoadDefaultImage()
        {
            string dirPath = String.Format(@"{0}\DefaultImage\", ModelPath);
            string filePath = Path.Combine(dirPath, "DefaultImage.bmp");
            if (File.Exists(filePath) == false)
            {
                return null;
            }
            return (Bitmap)ImageHelper.LoadImage(filePath);
        }


    }
}
