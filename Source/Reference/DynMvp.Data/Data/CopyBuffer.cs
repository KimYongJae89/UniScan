using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Data
{
    public class CopyBuffer
    {
        static List<ICloneable> copyData = new List<ICloneable>();

        public static void SetData(List<ICloneable> copyData)
        {
            CopyBuffer.copyData.Clear();

            foreach(ICloneable clonable in copyData)
            {
                CopyBuffer.copyData.Add((ICloneable)clonable.Clone());
            }
        }

        public static List<ICloneable> GetData()
        {
            return copyData;
        }

        public static List<Target> GetTargetList()
        {
            List<Target> targetList = new List<Target>();

            foreach (ICloneable clonable in copyData)
            {
                if (clonable is Probe)
                {
                    Probe probe = (Probe)clonable;
                    if (targetList.IndexOf(probe.Target) == -1)
                        targetList.Add(probe.Target);
                }
                else if (clonable is Target)
                {
                    if (targetList.IndexOf((Target)clonable) == -1)
                        targetList.Add((Target)clonable);
                }
            }

            return targetList;
        }

        public static bool IsTypeValid(Type type)
        {
            foreach (ICloneable clonable in copyData)
            {
                if (clonable is Probe)
                {
                    if (type.Name.IndexOf("Probe") != -1)
                        return true;
                }
                else if (clonable is Target)
                {
                    if (type.Name.IndexOf("Target") != -1)
                        return true;
                }
            }

            return false;
        }
    }
}
