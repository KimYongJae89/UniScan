using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Inspection;
using DynMvp.InspData;
using DynMvp.Base;

namespace UniEye.Base.Data
{
    public enum OpState
    {
        Idle, Wait, Inspect, Review, Teach, Align, Alarm
    }

    public enum InspectState
    {
        Done, Align, Run, Pause, Wait, Scan, Tune, Stop, Ready
    }

    public delegate void InspectStateChangedDelegate(InspectState curInspectState);
    public delegate void OpStateChangedDelegate(OpState curOpState, OpState prevOpState);
    public interface IOpStateListener
    {
        void OpStateChanged(OpState curOpState, OpState prevOpState);
    }

    public interface IInspectStateListener
    {
        void InspectStateChanged(InspectState curInspectState);
    }

    public class SystemState
    {
        public OpState OpState => this.opState;
        OpState opState;

        public string OpMessage => this.opMessage;
        string opMessage = "";

        public InspectState InspectState => inspectState;
        InspectState inspectState;

        Judgment inspectionResult;
        public Judgment InspectionResult
        {
            get { return inspectionResult; }
            set { inspectionResult = value; }
        }

        // 검사 중지 신호를 보낸 후, 대기 중인지
        bool onWaitStop;
        public bool IsWaitStop
        {
            get { return onWaitStop; }
            set { onWaitStop = value; }
        }

        bool alarmed;
        public bool Alarmed
        {
            get { return alarmed; }
            set { alarmed = value; }
        }

        List<IOpStateListener> opListenerList = new List<IOpStateListener>();
        List<IInspectStateListener> inspectListenerList = new List<IInspectStateListener>();

        static SystemState _instance;
        public static SystemState Instance()
        {
            if (_instance == null)
                _instance = new SystemState();

            return _instance;
        }

        public void AddOpListener(IOpStateListener listener)
        {
            opListenerList.Add(listener);
            listener.OpStateChanged(opState, opState);
        }

        public void AddInspectListener(IInspectStateListener listener)
        {
            inspectListenerList.Add(listener);
            listener.InspectStateChanged(inspectState);
        }

        public bool IsInspection
        {
            get { return opState == OpState.Inspect; }
        }

        public bool IsInspectOrWait
        {
            get { return opState == OpState.Inspect || opState == OpState.Wait; }
        }

        public bool IsIdle
        {
            get { return opState == OpState.Idle; }
        }

        private SystemState()
        {
            DynMvp.Base.ErrorManager.Instance().OnStartAlarmState += ErrorManager_OnStartAlarmState;
            DynMvp.Base.ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmState;
        }

        private void ErrorManager_OnStartAlarmState()
        {
            SetAlarm(ErrorManager.Instance().GetLastAlarmItem().FormattedMessage);
        }

        private void ErrorManager_OnResetAlarmState()
        {
            if(opState == OpState.Alarm)
                SetIdle();
        }

        private void SetOpState(OpState opState, string opMessage)
        {
            if (this.opState != opState)
            {
                LogHelper.Debug(LoggerType.Operation, string.Format("SystemState::SetOpState({0} -> {1})", this.opState, opState));
                this.opMessage = opMessage;
                OpState oldOpState = this.opState;
                this.opState = opState;
                OpStateNotifyChanged(oldOpState, opState);
                SetInspectState(opState == OpState.Inspect ? InspectState.Wait : InspectState.Stop);
            }
        }

        /// <summary>
        /// 유휴 상태. Stop 버튼을 눌렀을 때.
        /// </summary>
        public void SetIdle()
        {
            if (opState != OpState.Idle)
            {
                LogHelper.Debug(LoggerType.Operation, "SystemState::SetIdle");
                SetOpState(OpState.Idle, "");
            }
        }

        /// <summary>
        /// 검사 준비 중. Start 버튼을 눌렀을 때.
        /// </summary>
        public void SetWait()
        {
            if (opState != OpState.Wait)
            {
                LogHelper.Debug(LoggerType.Operation, "SystemState::SetWait");
                SetOpState(OpState.Wait, "");
            }
        }

        /// <summary>
        /// 오류 발생.
        /// </summary>
        public void SetAlarm(string reason)
        {
            if (opState != OpState.Alarm)
            {
                LogHelper.Debug(LoggerType.Operation, "SystemState::SetAlarm");
                SetOpState(OpState.Alarm, reason);
            }
        }

        public void SetAlign()
        {
            if (opState != OpState.Align)
            {
                LogHelper.Debug(LoggerType.Operation, "SystemState::SetAlign");
                SetOpState(OpState.Align, "");
            }
        }

        public void SetTeach()
        {
            if (opState != OpState.Teach)
            {
                LogHelper.Debug(LoggerType.Operation, "SystemState::SetTeach");
                SetOpState(OpState.Teach, "");
            }
        }

        /// <summary>
        /// 검사 준비 완료. Trigger 대기 상태.
        /// </summary>
        public void SetInspect()
        {
            if (opState != OpState.Inspect)
            {
                LogHelper.Debug(LoggerType.Operation, "SystemState::SetInspect");
                SetOpState(OpState.Inspect, "");
                SetInspectState(InspectState.Wait);
            }
        }


        /// <summary>
        /// Inspect 상태일 때, Trigger 등으로 상태가 변화하면 이 함수를 사용한다.
        /// Idle / Wait 상태이면 함수 실행을 무시한다.
        /// </summary>
        /// <param name="inspectState"></param>
        public void SetInspectState(InspectState inspectState)
        {   
            if (this.inspectState != inspectState)
            {
                LogHelper.Debug(LoggerType.Operation, string.Format("SystemState::SetInspectState - {0}", inspectState));
                this.inspectState = inspectState;
                InspectStateNotifyChanged(inspectState);
            }
        }

        public void OpStateNotifyChanged(OpState prevOpState, OpState curOpState)
        {
            foreach (IOpStateListener listener in opListenerList)
                listener.OpStateChanged(curOpState, prevOpState);
        }

        public void InspectStateNotifyChanged(InspectState inspectState)
        {
            foreach (IInspectStateListener listener in inspectListenerList)
                listener.InspectStateChanged(inspectState);
        }
    }
}
