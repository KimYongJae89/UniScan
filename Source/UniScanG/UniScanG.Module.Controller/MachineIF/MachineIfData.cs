using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Common;
using UniScanG.Gravure.Data;

namespace UniScanG.Module.Controller.MachineIF
{
    enum UniScanGMelsecProtocolSet { SET_VISION_STATE, GET_ERASER_STATE, SET_ERASER_STATE };

    public class MachineIfData: UniScanG.MachineIF.MachineIfData, IModelListener
    {
        public float SET_VISION_GRAVURE_INSP_INFO_SHTLEN_REAL
        {
            get => SET_VISION_GRAVURE_INSP_INFO_SHTLEN / 100;
            set => SET_VISION_GRAVURE_INSP_INFO_SHTLEN = value * 100;
        }

        public float SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE_REAL
        {
            get => SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE / 100;
            set => SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE = value * 100;
        }

        // Controller -> Printer
        public bool SET_VISION_GRAVURE_INSP_READY;
        public bool SET_VISION_GRAVURE_INSP_RUNNING;
        public bool SET_VISION_GRAVURE_INSP_RESULT;

        public bool SET_VISION_GRAVURE_INSP_NG_REPDEF_P;
        public bool SET_VISION_GRAVURE_INSP_NG_REPDEF_N;
        public bool SET_VISION_GRAVURE_INSP_NG_REPDEF_B;
        public bool SET_VISION_GRAVURE_INSP_NG_REPDEF;
        public bool SET_VISION_GRAVURE_INSP_NG_NORDEF;
        public bool SET_VISION_GRAVURE_INSP_NG_SHTLEN;

        public int SET_VISION_GRAVURE_INSP_CNT_ALL;
        public int SET_VISION_GRAVURE_INSP_CNT_NG;
        public int SET_VISION_GRAVURE_INSP_CNT_PINHOLE;
        public int SET_VISION_GRAVURE_INSP_CNT_COATING;
        public int SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK;
        public int SET_VISION_GRAVURE_INSP_CNT_NOPRINT;

        public bool SET_VISION_GRAVURE_INSP_NG_DEFCNT;
        public bool SET_VISION_GRAVURE_INSP_NG_MARGIN;
        public bool SET_VISION_GRAVURE_INSP_NG_STRIPE;
        public bool SET_VISION_GRAVURE_INSP_NG_CRITICAL;

        public float SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE;
        public float SET_VISION_GRAVURE_INSP_INFO_SHTLEN;

        // Printer -> Laser
        public bool GET_FORCE_GRAVURE_ERASER;

        // Laser -> Printer
        public bool SET_VISION_GRAVURE_ERASER_READY;
        public bool SET_VISION_GRAVURE_ERASER_RUNNING;

        public int SET_VISION_GRAVURE_ERASER_CNT_ERASE;

        public MachineIfData() : base()
        {
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        public void SetInspCnt(ProductionG productionG)
        {
            if (productionG == null)
            {
#if !DEBUG
                this.SET_VISION_GRAVURE_INSP_CNT_ALL = 0;
                this.SET_VISION_GRAVURE_INSP_CNT_NG = 0;
                this.SET_VISION_GRAVURE_INSP_CNT_PINHOLE = 0;
                this.SET_VISION_GRAVURE_INSP_CNT_COATING = 0;
                this.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK = 0;
                this.SET_VISION_GRAVURE_INSP_CNT_NOPRINT = 0;
#endif
                return;
            }

            this.SET_VISION_GRAVURE_INSP_CNT_ALL = productionG.Done;
            this.SET_VISION_GRAVURE_INSP_CNT_NG = productionG.Ng;
            this.SET_VISION_GRAVURE_INSP_CNT_PINHOLE = productionG.Patterns.PinHole;
            this.SET_VISION_GRAVURE_INSP_CNT_COATING = productionG.Patterns.Dielectric;
            this.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK = productionG.Patterns.SheetAttack;
            this.SET_VISION_GRAVURE_INSP_CNT_NOPRINT = productionG.Patterns.NoPrint;
        }

        public override void Reset()
        {
            base.Reset();

            // Controller -> Printer
            this.SET_VISION_GRAVURE_INSP_READY = false;
            this.SET_VISION_GRAVURE_INSP_RUNNING = false;
            this.SET_VISION_GRAVURE_INSP_RESULT = false;

            this.SET_VISION_GRAVURE_INSP_NG_REPDEF_P = false;
            this.SET_VISION_GRAVURE_INSP_NG_REPDEF_N = false;
            this.SET_VISION_GRAVURE_INSP_NG_REPDEF_B = false;
            this.SET_VISION_GRAVURE_INSP_NG_REPDEF = false;
            this.SET_VISION_GRAVURE_INSP_NG_NORDEF = false;
            this.SET_VISION_GRAVURE_INSP_NG_SHTLEN = false;

            this.SET_VISION_GRAVURE_INSP_CNT_ALL = 0;
            this.SET_VISION_GRAVURE_INSP_CNT_NG = 0;
            this.SET_VISION_GRAVURE_INSP_CNT_PINHOLE = 0;
            this.SET_VISION_GRAVURE_INSP_CNT_COATING = 0;
            this.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK = 0;
            this.SET_VISION_GRAVURE_INSP_CNT_NOPRINT = 0;

            this.SET_VISION_GRAVURE_INSP_NG_DEFCNT = false;
            this.SET_VISION_GRAVURE_INSP_NG_MARGIN = false;
            this.SET_VISION_GRAVURE_INSP_NG_STRIPE = false;
            this.SET_VISION_GRAVURE_INSP_NG_CRITICAL = false;

            this.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE = 0;
            this.SET_VISION_GRAVURE_INSP_INFO_SHTLEN = 0;

            // Printer -> Laser
            this.GET_FORCE_GRAVURE_ERASER = false;

            // Laser -> Printer
            this.SET_VISION_GRAVURE_ERASER_READY = false;
            this.SET_VISION_GRAVURE_ERASER_RUNNING = false;
            this.SET_VISION_GRAVURE_ERASER_CNT_ERASE = 0;
        }

        public void ClearVisionNG()
        {
            this.SET_VISION_GRAVURE_INSP_RESULT = false;  // 0(false)이면 정상. 1(true)이면 불량.
            this.SET_VISION_GRAVURE_INSP_NG_SHTLEN = false;
            this.SET_VISION_GRAVURE_INSP_NG_NORDEF = false;
            this.SET_VISION_GRAVURE_INSP_NG_DEFCNT = false;
            this.SET_VISION_GRAVURE_INSP_NG_MARGIN = false;
            this.SET_VISION_GRAVURE_INSP_NG_STRIPE = false;
            this.SET_VISION_GRAVURE_INSP_NG_CRITICAL = false;
        }

        public void ModelChanged()
        {
            if (SystemManager.Instance().CurrentModel == null)
                this.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE = 0;
            else
                this.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE = SystemManager.Instance().CurrentModel.ChipShare100p;
        }

        public void ModelRefreshed() { }
        public void ModelTeachDone(int camId)
        {
            this.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE = SystemManager.Instance().CurrentModel.ChipShare100p;
        }
    }
}
