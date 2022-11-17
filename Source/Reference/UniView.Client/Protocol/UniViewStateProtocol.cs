using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniView.Client.Protocol
{
    [Serializable()]
    public class UniViewStateProtocol : UniViewProtocolBase
    {
        #region 생성자
        public UniViewStateProtocol()
        {
            SetDefault(UniViewProtocolType.State);
        }

        public UniViewStateProtocol(MachineStateType machinestate)
        {
            MachineState = machinestate;
            SetDefault(UniViewProtocolType.State);
        }
        #endregion


        #region 속성
        public MachineStateType MachineState { get; set; } = MachineStateType.Idle;
        #endregion


        #region 메서드
        public override bool Equals(object obj)
        {
            var tagetObj = obj as UniViewStateProtocol;
            return tagetObj.MachineState == MachineState;
        }

        public override int GetHashCode()
        {
            return 1272613547 + MachineState.GetHashCode();
        }
        #endregion
    }
}
