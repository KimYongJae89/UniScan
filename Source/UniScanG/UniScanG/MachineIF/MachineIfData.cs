using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;

namespace UniScanG.MachineIF
{
    public abstract class MachineIfData : UniEye.Base.MachineInterface.MachineIfDataBase
    {
        public float GET_TARGET_SPEED_REAL { get => GET_TARGET_SPEED / 10; set => GET_TARGET_SPEED = value * 10; }
        public float GET_PRESENT_SPEED_REAL { get => GET_PRESENT_SPEED / 10; set => GET_PRESENT_SPEED = value * 10; }
        public float GET_ROLL_DIAMETER_REAL { get => GET_ROLL_DIAMETER / 100; set => GET_ROLL_DIAMETER = value * 100; }

        // Printer -> Controller
        public bool GET_START_STILLIMAGE;
        public bool GET_START_COLORSENSOR;
        public bool GET_START_EDMS;
        public bool GET_START_PINHOLE;
        public bool GET_START_RVMS;

        public float GET_TARGET_SPEED;
        public float GET_PRESENT_SPEED;
        public float GET_PRESENT_POSITION;


        public string GET_LOT;
        public string GET_MODEL;
        public string GET_WORKER;
        public string GET_PASTE;

        public float GET_ROLL_DIAMETER;
        public bool GET_REWINDER_CUT;
        public bool GET_START_GRAVURE_INSPECTOR;
        public bool GET_START_GRAVURE_ERASER;

        public Data.RewinderZone RewinderZone => !GET_REWINDER_CUT ? Data.RewinderZone.ZoneA : Data.RewinderZone.ZoneB;

        public MachineIfData() 
        {
            Reset();

            MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            if (machineIf != null && machineIf.IsVirtual)
            {
                this.GET_ROLL_DIAMETER = 14642.3f; // x100
                this.GET_TARGET_SPEED = 240; // x10
            }
        }

        public virtual void Reset()
        {
            // Printer -> Controller
            this.GET_START_STILLIMAGE = false;
            this.GET_START_COLORSENSOR = false;
            this.GET_START_EDMS = false; ;
            this.GET_START_PINHOLE = false;
            this.GET_START_RVMS = false;
            this.GET_TARGET_SPEED = 0;
            this.GET_PRESENT_SPEED = 0;
            this.GET_PRESENT_POSITION = 0;
            this.GET_LOT = "";
            this.GET_MODEL = "";
            this.GET_WORKER = "";
            this.GET_PASTE = "";

            this.GET_ROLL_DIAMETER = 0;
            this.GET_REWINDER_CUT = false;
            this.GET_START_GRAVURE_INSPECTOR = false;
            this.GET_START_GRAVURE_ERASER = false;
        }
    }
}