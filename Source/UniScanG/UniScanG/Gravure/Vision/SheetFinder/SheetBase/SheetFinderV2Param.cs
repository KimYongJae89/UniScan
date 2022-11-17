using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Globalization;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System.IO;
using UniEye.Base.Settings;
using UniScanG.Inspect;
using UniScanG.Gravure.Inspect;

namespace UniScanG.Gravure.Vision.SheetFinder.SheetBase
{
    public class SheetFinderV2Param : SheetFinderBaseParam
    {
        float projectionBinalizeMul = 1.1f;
        public float ProjectionBinalizeMul
        {
            get { return projectionBinalizeMul; }
            set { projectionBinalizeMul = value; }
        }

        public SheetFinderV2Param() : base()
        {
        }

        ~SheetFinderV2Param()
        {
            Clear();
        }

        #region Abstract
        public override SheetFinderBase CreateFinder()
        {
            return new SheetFinderV2();
        }
        #endregion

        #region Override
        #endregion

        public override AlgorithmParam Clone()
        {
            SheetFinderV2Param param = new SheetFinderV2Param();
            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            SheetFinderV2Param srcParam = srcAlgorithmParam as SheetFinderV2Param;
            if (srcParam != null)
            {
                this.projectionBinalizeMul = srcParam.projectionBinalizeMul;
            }
        }

        public override void SyncParam(AlgorithmParam srcAlgorithmParam)
        {
            CopyFrom(srcAlgorithmParam);
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            this.projectionBinalizeMul = XmlHelper.GetValue(paramElement, "ProjectionBinalizeMul", this.projectionBinalizeMul);

        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "ProjectionBinalizeMul", projectionBinalizeMul.ToString());
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
    }
}
