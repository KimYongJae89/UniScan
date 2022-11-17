using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniView.Client.Protocol
{
    public class InspectResultExtendedInfo
    {
        //나중에 Type 관련만 좀 넣으면 될듯 지금은 당장 안씀
    }

    public class R2RInspectResultExtendedInfo : InspectResultExtendedInfo
    {
        #region 속성
        public double CurrentInspectPosition { get; set; } = 0;
        public string Unit = "m";
        #endregion

        #region 생성자
        public R2RInspectResultExtendedInfo(double currentInspectPosition)
        {
            CurrentInspectPosition = currentInspectPosition;
        }
        #endregion
    }
}
