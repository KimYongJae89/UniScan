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

namespace UniScanG.Gravure.Vision.SheetFinder.FiducialMarkBase
{
    public class SheetFinderPJParam : SheetFinderBaseParam
    {
        float fidSearchLBound;
        public float FidSearchLBound
        {
            get { return fidSearchLBound; }
            set { fidSearchLBound = value; }
        }

        float fidSearchRBound;
        public float FidSearchRBound
        {
            get { return fidSearchRBound; }
            set { fidSearchRBound = value; }
        }

        public Size FidSize { get => this.fidSize; set => this.fidSize = value; }
        protected System.Drawing.Size fidSize = new System.Drawing.Size(500, 150);

        public SheetFinderPJParam() : base()
        {
            this.fidSize = new System.Drawing.Size(500, 150);

            this.fidSearchLBound = 0.00f;
            this.fidSearchRBound = 0.92f;
        }

        ~SheetFinderPJParam()
        {
            Clear();
        }

        #region Abstract
        public override SheetFinderBase CreateFinder()
        {
            return new SheetFinderPJ();
        }
        #endregion

        #region Override
        #endregion

        public override AlgorithmParam Clone()
        {
            SheetFinderPJParam param = new SheetFinderPJParam();
            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            SheetFinderPJParam sheetFinderPJParam = srcAlgorithmParam as SheetFinderPJParam;
            if (sheetFinderPJParam != null)
            {
                fidSize = sheetFinderPJParam.fidSize;

                fidSearchLBound = sheetFinderPJParam.fidSearchLBound;
                fidSearchRBound = sheetFinderPJParam.fidSearchRBound;
            }
        }

        public override void SyncParam(AlgorithmParam srcAlgorithmParam)
        {
            CopyFrom(srcAlgorithmParam);
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            this.fidSearchLBound = Convert.ToSingle(XmlHelper.GetValue(paramElement, "FidSearchLBound", fidSearchLBound.ToString()));
            this.fidSearchRBound = Convert.ToSingle(XmlHelper.GetValue(paramElement, "FidSearchRBound", fidSearchRBound.ToString()));

            this.fidSize = XmlHelper.GetValue(paramElement, "FidSizeWidth", Size.Empty);
            if (this.fidSize.IsEmpty)
            {
                try
                {
                    int fidSizeWidth = Convert.ToInt32(XmlHelper.GetValue(paramElement, "FidSizeWidth", 500));
                    int fidSizeHeight = Convert.ToInt32(XmlHelper.GetValue(paramElement, "FidSizeHeight", 150));
                    this.fidSize = new System.Drawing.Size(fidSizeWidth, fidSizeHeight);
                }
                catch
                {
                    this.fidSize = new System.Drawing.Size(500, 150);
                }
            }

        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "FidSearchLBound", this.fidSearchLBound.ToString());
            XmlHelper.SetValue(paramElement, "FidSearchRBound", this.fidSearchRBound.ToString());
            XmlHelper.SetValue(paramElement, "FidSize", this.fidSize);
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
    }
}
