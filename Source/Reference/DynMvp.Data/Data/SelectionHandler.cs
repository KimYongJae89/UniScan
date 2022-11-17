using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Data
{
    public class SelectionHandler
    {
        private List<Target> targetList = new List<Target>();
        public List<Target> TargetList
        {
            get { return targetList; }
            set { targetList = value; }
        }

        private List<Probe> probeList = new List<Probe>();
        public List<Probe> ProbeList
        {
            get { return probeList; }
            set { probeList = value; }
        }

        public void AddTarget(Target target)
        {
            targetList.Add(target);
            probeList.Clear();
        }

        public void ClearTargetList()
        {
            targetList.Clear();
            probeList.Clear();
        }

        public bool IsTargetSelected()
        {
            return targetList.Count > 0;
        }

        public bool IsTargetSingleSelected()
        {
            return targetList.Count == 1;
        }

        public Target GetSingleTarget()
        {
            if (targetList.Count == 1)
                return targetList[0];

            return null;
        }

        public void AddProbe(Probe probe)
        {
            probeList.Add(probe);
        }

        public void ClearProbeList()
        {
            probeList.Clear();
        }

        public bool IsProbeSelected()
        {
            return probeList.Count > 0;
        }

        public bool IsProbeSingleSelected()
        {
            return probeList.Count == 1;
        }

        public Probe GetSingleProbe()
        {
            if (probeList.Count == 1)
                return probeList[0];

            return null;
        }
    }
}
