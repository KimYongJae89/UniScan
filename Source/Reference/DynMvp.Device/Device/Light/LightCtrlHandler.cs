using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Light
{
    public class LightCtrlHandler
    {
        protected List<LightCtrl> lightCtrlList = new List<LightCtrl>();

        public IEnumerator<LightCtrl> GetEnumerator()
        {
            return lightCtrlList.GetEnumerator();
        }

        public int Count
        {
            get { return lightCtrlList.Count; }
        }

        public int NumLightCtrl
        {
            get { return lightCtrlList.Count; }
        }

        public int NumLight
        {
            get
            {
                int sumLight = 0;
                foreach (LightCtrl lightCtrl in lightCtrlList)
                    sumLight += lightCtrl.NumChannel;

                return sumLight;
            }
        }

        public void AddLightCtrl(LightCtrl lightCtrl)
        {
            if (lightCtrlList.Count > 0)
            {
                lightCtrl.StartChannelIndex = lightCtrlList.Last().StartChannelIndex + lightCtrlList.Last().NumChannel;
            }

            lightCtrlList.Add(lightCtrl);
        }

        public LightCtrl GetLightCtrl(int lightCtrlIndex)
        {
            if (lightCtrlList.Count <= lightCtrlIndex)
                return null;

            return lightCtrlList[(int)lightCtrlIndex];
        }

        public LightCtrl GetLightCtrlByIndex(int lightCtrlIndex)
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                if (lightCtrl.StartChannelIndex <= lightCtrlIndex && lightCtrl.EndChannelIndex > lightCtrlIndex)
                    return lightCtrl;
            }

            return null;
        }

        public void SetLightStableTiemsMs(int lightStableTimeMs)
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.LightStableTimeMs = lightStableTimeMs;
            }
        }

        public void Release()
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                bool isReady = lightCtrl.IsReady();
                if(isReady)
                    lightCtrl.Release();
            }
            lightCtrlList.Clear();
        }

        public LightParamSet GetLastLightParamSet()
        {
            LightParamSet lightParamSet = new LightParamSet();
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                LightParam lightParam = new LightParam(lightCtrl.NumChannel);
                if(lightCtrl.LastLightValue!=null)
                    Array.Copy(lightCtrl.LastLightValue.Value, lightParam.LightValue.Value, lightCtrl.NumChannel);
                //lightParam.LightValue = lightCtrl.LastLightValue;
                lightParam.LightStableTimeMs = lightCtrl.LightStableTimeMs;
                lightParamSet.LightParamList.Add(lightParam);
            }

            return lightParamSet;
        }

        public void TurnOn()
        {
            foreach(LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.TurnOn();
            }
        }

        public void TurnOn(LightValue lightValue)
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.TurnOn(lightValue);
            }
        }

        public void TurnOn(LightParam lightParam)
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.LightStableTimeMs = lightParam.LightStableTimeMs;
                lightCtrl.TurnOn(lightParam.LightValue);
            }
        }

        public void TurnOff()
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.TurnOff();
            }
        }

        public void GetLightList()
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.TurnOff();
            }
        }
    }
}
