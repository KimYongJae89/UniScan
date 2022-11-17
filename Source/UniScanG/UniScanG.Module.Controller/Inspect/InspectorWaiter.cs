using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniScanG.Common;
using UniScanG.Common.Data;

namespace UniScanG.Module.Controller.Inspect
{
    public class WaitException : Exception
    {
        public string Name { get; private set; }
        public object[] Argument { get; private set; }

        public WaitException(string name, string message, object[] argument) : base(message)
        {
            this.Name = name;
            this.Argument = argument;
        }
    }

    static class InspectorWaiter
    {
        public static void WaitAll(InspectorObj[] inspectorObjs, string message, OpState waitUntil, int waitTimeMs)
        {
            WaitUntil(ConfigHelper.Instance().MainForm, message, inspectorObjs, waitUntil, waitTimeMs, true);
        }

        public static void WaitAll(InspectorObj[] inspectorObjs, string message, OpState waitUntil, int waitTimeMs, AlarmException alarmException)
        {
            try
            {
                WaitUntil(ConfigHelper.Instance().MainForm, message, inspectorObjs, waitUntil, waitTimeMs, true);
            }
            catch (WaitException ex)
            {
                if (alarmException != null)
                    throw new AlarmException(alarmException.ErrorCode, alarmException.ErrorLevel, ex.Name, ex.Message, ex.Argument, "");
            }
        }

        private static void WaitUntil(Control parent, string message, InspectorObj[] inspectorObjs, OpState waitUntil, int waitTimeMs, bool forAll)
        {
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm(message);
            Exception exception = null;
            simpleProgressForm.Show(parent, () =>
            {
                try
                {
                    TimeOutTimer tot = new TimeOutTimer();
                    if (waitTimeMs >= 0)
                        tot.Start(waitTimeMs);

                    bool waitDone = false;
                    while (!waitDone)
                    {
                        InspectorObj[] connectedInspObjs = Array.FindAll(inspectorObjs, f => f.CommState == CommState.CONNECTED);
                        if (connectedInspObjs.Length == 0)
                            break;

                        Thread.Sleep(100);

                        // CM 알람 발생시 종료
                        // 종료 외의 경우만 해당함 (알람떠서 종료하는데 알람때문에 종료가 안되면 이상하니까..)
                        if (waitUntil != OpState.Idle && ErrorManager.Instance().IsAlarmed())
                        {
                            ErrorItem ei = ErrorManager.Instance().GetLastAlarmItem();
                            throw new WaitException("CM", ei.Message, ei.Argument);
                        }

                        // 타임아웃 발생시 종료
                        if (tot.TimeOut)
                            throw new WaitException("CM", "Wait Timeout", null);

                        // IM 알람 발생시 종료
                        InspectorObj alaramObj = Array.Find(connectedInspObjs, f => f.OpState == OpState.Alarm);
                        if (alaramObj != null)
                            throw new WaitException(alaramObj.Info.GetName(), alaramObj.OpMessage, null);

                        // IM 정상일 경우 종료
                        if (forAll)
                            waitDone = Array.TrueForAll(connectedInspObjs, f => f.OpState == waitUntil);
                        else
                            waitDone = Array.Exists(connectedInspObjs, f => f.OpState == waitUntil);
                    };
                }
                catch (WaitException ex)
                {
                    exception = ex;
                }
            });

            if (exception != null)
                throw exception;
        }
    }
}
