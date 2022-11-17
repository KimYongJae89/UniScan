using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.Light
{
    public class LightCtrlVirtual : LightCtrl
    {
        public LightCtrlVirtual(LightCtrlInfo lightCtrlInfo) : base(lightCtrlInfo)
        {
        }

        public override int GetMaxLightLevel()
        {
            return 255;
        }

        public override bool Initialize()
        {
            return true;
        }

        //public override void TurnOff()
        //{
            
        //}

        //public override void TurnOn()
        //{
            
        //}

        protected override bool SetLightValue(LightValue lightValue)
        {
            Thread.Sleep(lightStableTimeMs);
            return true;
        }

        public override LightValue GetLightValue()
        {
            return this.curLightValue.Clone();
        }
    }
}
