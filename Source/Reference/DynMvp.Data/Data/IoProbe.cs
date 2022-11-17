using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.Devices.Dio;

namespace DynMvp.Data
{
    public class IoProbe : Probe
    {
        string digitalIoName;
        public string DigitalIoName
        {
            get { return digitalIoName; }
            set { digitalIoName = value; }
        }

        int portNo;
        public int PortNo
        {
            get { return portNo; }
            set { portNo = value; }
        }

        public override Object Clone()
        {
            IoProbe ioProbe = new IoProbe();
            ioProbe.Copy(this);

            return ioProbe;
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);

            IoProbe ioProbe = (IoProbe)probe;

            digitalIoName = ioProbe.digitalIoName;
            portNo = ioProbe.portNo;
        }

        public override void OnPreInspection()
        {

        }

        public override void OnPostInspection()
        {

        }

        public override bool IsControllable()
        {
            return false;
        }

        public override List<ProbeResultValue> GetResultValues()
        {
            List<ProbeResultValue> resultValues = new List<ProbeResultValue>();
            resultValues.Add(new ProbeResultValue("Result", true));

            return resultValues;
        }

        public override ProbeResult Inspect(InspParam inspParam)
        {
            bool value = false;

            int deviceIndex = inspParam.DigitalIoHandler.GetIndex(digitalIoName);
            if (deviceIndex > -1)
            {
                value = inspParam.DigitalIoHandler.ReadInput(deviceIndex, 0, portNo);

                LogHelper.Debug(LoggerType.Inspection, String.Format("IO Probe [{0}] Inspected. Result : {1}", FullId, value));
            }
            else
            {
                LogHelper.Debug(LoggerType.Inspection, String.Format("IO Probe [{0}] is failed : Can't find Digital I/O : {1}", FullId, digitalIoName));
            }

            IoProbeResult ioProbeResult = new IoProbeResult(this, value == true);
            ioProbeResult.ResultValueList.Add(new ProbeResultValue("Result", value));

            return ioProbeResult;
        }

        public override ProbeResult CreateDefaultResult()
        {
            return new IoProbeResult(this, false);
        }
    }

    public class IoProbeResult : ProbeResult
    {
        public IoProbeResult()
        {

        }

        public IoProbeResult(Probe probe, bool good)
        {
            if (good == true)
                this.Judgment = Judgment.Accept;
            else
                this.Judgment = Judgment.Reject;

            this.Probe = probe;
        }

        public override void BuildResultMessage(DynMvp.UI.MessageBuilder totalResultMessage)
        {

        }

        public override string ToString()
        {
            if (ResultValueList.Count > 0)
                return String.Format("result value = {0}", ResultValueList[0].Value);

            return "( No Result )";
        }
    }
}
