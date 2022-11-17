using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanS.Screen.Vision
{
    public class ProcessBufferSetS : UniScanS.Inspect.ProcessBufferSet
    {
        AlgoImage poleMask;
        public AlgoImage PoleMask
        {
            get { return poleMask; }
            set { poleMask = value; }
        }

        AlgoImage poleInspect;
        public AlgoImage PoleInspect
        {
            get { return poleInspect; }
            set { poleInspect = value; }
        }

        AlgoImage dielectricMask;
        public AlgoImage DielectricMask
        {
            get { return dielectricMask; }
            set { dielectricMask = value; }
        }

        AlgoImage dielectricInspect;
        public AlgoImage DielectricInspect
        {
            get { return dielectricInspect; }
            set { dielectricInspect = value; }
        }
        
        AlgoImage interestBin;
        public AlgoImage InterestBin
        {
            get { return interestBin; }
            set { interestBin = value; }
        }

        AlgoImage mask;
        public AlgoImage Mask
        {
            get { return mask; }
            set { mask = value; }
        }

        public ProcessBufferSetS(string algorithmTypeName, int width, int height) : base(algorithmTypeName, width, height)
        {
        }

        protected override void BuildBuffers(string algorithmTypeName, int width, int height)
        {
            base.BuildBuffers(algorithmTypeName, width, height);

            AllocImage(algorithmTypeName, width, height);
        }

        private void AllocImage(string algorithmTypeName, int width, int height)
        {
            bufferList.Add(poleMask = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height));
            bufferList.Add(poleInspect = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height));
            bufferList.Add(dielectricMask = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height));
            bufferList.Add(dielectricInspect = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height));
            bufferList.Add(interestBin = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height));
            bufferList.Add(mask = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height));
        }
    }
}
