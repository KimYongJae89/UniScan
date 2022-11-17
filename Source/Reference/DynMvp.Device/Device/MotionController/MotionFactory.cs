﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DynMvp.Base;
using DynMvp.Device.Device.MotionController;

namespace DynMvp.Devices.MotionController
{
    //public class MotionManager
    //{
    //    private static List<Motion> motionList = new List<Motion>();

    //    public static void Clear()
    //    {
    //        motionList.Clear();
    //    }

    //    public static void AddMotion(Motion motion)
    //    {
    //        Motion foundMotion = GetMotion(motion.Name);
    //        if (foundMotion != null)
    //            motionList.Remove(foundMotion);

    //        motionList.Add(motion);
    //    }

    //    public static void RemoveMotion(string name)
    //    {
    //        Motion foundMotion = GetMotion(name);
    //        if (foundMotion != null)
    //            motionList.Remove(foundMotion);
    //    }

    //    public static Motion GetMotion(string name)
    //    {
    //        foreach (Motion motion in motionList)
    //        {
    //            if (motion.Name == name)
    //                return motion;
    //        }

    //        return null;
    //    }
    //}

    public class MotionFactory
    {
        public static Motion Create(MotionInfo motionInfo, bool isVirtual = false)
        {
            LogHelper.Debug(LoggerType.StartUp, "Create Motion");

            Motion motion = null;
            if (isVirtual)
            {
                motion = new MotionVirtual(motionInfo.Name);
            }
            else
            {
                switch (motionInfo.Type)
                {
                    case MotionType.AlphaMotion302:
                        motion = new MotionAlphaMotion302(motionInfo.Name);
                        break;
                    case MotionType.AlphaMotion304:
                        motion = new MotionAlphaMotion304(motionInfo.Name);
                        break;
                    case MotionType.AlphaMotion314:
                        motion = new MotionAlphaMotion314(motionInfo.Name);
                        break;
                    case MotionType.AlphaMotionBx:
                        motion = new MotionAlphaMotionBx(motionInfo.Name);
                        break;
                    case MotionType.FastechEziMotionPlusR:
                        motion = new MotionEziMotionPlusR(motionInfo.Name);
                        break;
                    case MotionType.PowerPmac:
                        motion = new MotionPowerPmac(motionInfo.Name);
                        break;
                    case MotionType.Ajin:
                        motion = new MotionAjin(motionInfo.Name);
                        break;
                    case MotionType.Virtual:
                        motion = new MotionVirtual(motionInfo.Name);
                        break;

                    case MotionType.AlphaMotionBBx:
                        motion = new MotionAlphaMotionBBx(motionInfo.Name);
                        break;
                }
            }

            if (motion == null)
                throw new AlarmException(ErrorCodeMotion.Instance.InvalidType, ErrorLevel.Fatal,
                    motion.Name, "Can't create motion.", null, "");


            if (motion.Initialize(motionInfo) == false)
            {
                throw new AlarmException(ErrorCodeMotion.Instance.FailToInitialize, ErrorLevel.Fatal,
                    motion.Name, "Can't Initialize motion.", null, "");
                //ErrorManager.Instance().Report(ErrorCodeMotion.Instance.FailToInitialize, ErrorLevel.Error,
                //    motionInfo.Name, String.Format("Can't initialize motion. {0}", motionInfo.Type.ToString()));

                //motion = new MotionVirtual(motionInfo.Type, motionInfo.Name);
                //motion.Initialize(motionInfo);
                //motion.UpdateState(DeviceState.Error, "Motion is invalid.");
            }

            motion.UpdateState(DeviceState.Ready, "Motion initialization succeeded.");

            DeviceManager.Instance().AddDevice(motion);
            return motion;
        }
    }
}
