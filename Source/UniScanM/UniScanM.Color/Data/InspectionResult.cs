using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.InspData;

namespace UniScanM.ColorSens.Data
{
    //그라비아 칼라센서 시트 밝기 값..검사
    public class InspectionResult : UniScanM.Data.InspectionResult
    {
        public int InspectionCount { get; set; }
        
        float sheetBrightness =0.1234f;  //최종 결과, 한시트길이의 평균 밝기
        public float SheetBrightness
        {
            get { return sheetBrightness; }
            set { sheetBrightness = value; }
        }

        float deltaBrightness = 0.001f;  //
        public float DeltaBrightness
        {
            get { return deltaBrightness; }
            set { deltaBrightness = value; }
        }

        float upperlimit = 0.002f;  //
        public float Uppperlimit
        {
            get { return upperlimit; }
            set { upperlimit = value; }
        }
        float lowerlimit = 0.004f;  //
        public float Lowerlimit
        {
            get { return lowerlimit; }
            set { lowerlimit = value; }
        }


        float referenceBrightness = 0.001f;  //
        public float ReferenceBrightness
        {
            get { return referenceBrightness; }
            set { referenceBrightness = value; }
        }

        //double processedSheetLength;
        //public double ProcessedSheetLength
        //{
        //    get { return processedSheetLength; }
        //    set { processedSheetLength = value; }
        //}


        float[] arrLocalBrightness = null;
        public void SetLocalBrightness(float[] data)
        {
            arrLocalBrightness = data;
        }

        private ColorSensorParam colorSensorParameter = null;
        public void SetInspectionParameter(ref ColorSensorParam param)
        {
            colorSensorParameter = param; 
        }

        public void Judge()
        {
            if (colorSensorParameter != null)
            {
                if (deltaBrightness <  colorSensorParameter.SpecLimit &&
                    deltaBrightness > -colorSensorParameter.SpecLimit)
                    Judgment = Judgment.Accept;
                else
                    Judgment = Judgment.Reject;
                //if (sheetBrightness > 90) return true;
            }
        }

        public override bool IsGood()
        {
            if (Judgment == Judgment.Accept)
                return true;           

            return false;
        }
        
        public override bool IsPass()
        {
            return IsGood();
        }
    }
}


