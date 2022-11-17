using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Inspect;

namespace UniScanG.Gravure.Vision.Calculator
{
    public abstract class CalculatorBase: UniScanG.Vision.AlgorithmG
    {
        public static CalculatorParam CalculatorParam => AlgorithmSetting.Instance().CalculatorParam;

        public abstract ProcessBufferSetG CreateProcessingBuffer(float scaleFactor, bool isMultiLayer, int width, int height);

        public static string TypeName { get { return "Calculator"; } }

        #region Abstract
        public override string GetAlgorithmType()
        {
            return TypeName;
        }
        #endregion

        #region Override
        public override void PrepareInspection()
        {
            for (int i = 0; i < CalculatorParam.ModelParam.RegionInfoCollection.Count; i++)
            {
                RegionInfo info = CalculatorParam.ModelParam.RegionInfoCollection[i];
                LogHelper.Debug(LoggerType.Inspection, $"CalculatorBase::PrepareInspection - RegionInfo[{i}].Use = {info.Use}");
            };
        }
        #endregion
    }
}
