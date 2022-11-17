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

namespace UniScanG.Gravure.Vision.SheetFinder.ReversOffset
{
    public class SheetFinderRVOSParam : SheetFinderBaseParam
    {
        public SheetFinderRVOSParam() : base()
        {
        }

        ~SheetFinderRVOSParam()
        {
            Clear();
        }

        #region Abstract
        public override SheetFinderBase CreateFinder()
        {
            return new SheetFinderRVOS();
        }
        #endregion

        #region Override
        #endregion

        public override AlgorithmParam Clone()
        {
            SheetFinderRVOSParam param = new SheetFinderRVOSParam();
            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            SheetFinderRVOSParam srcParam = srcAlgorithmParam as SheetFinderRVOSParam;
            if (srcParam != null)
            {
        
            }
        }

        public override void SyncParam(AlgorithmParam srcAlgorithmParam)
        {
            CopyFrom(srcAlgorithmParam);
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
    }
}
