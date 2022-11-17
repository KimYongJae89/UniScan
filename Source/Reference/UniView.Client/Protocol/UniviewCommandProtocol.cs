using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniView.Client.Protocol
{
    [Serializable()]
    public class UniViewVisionCommandProtocol : UniViewProtocolBase
    {
        #region 생성자
        public UniViewVisionCommandProtocol()
        {
            SetDefault(UniViewProtocolType.Command);
        }

        public UniViewVisionCommandProtocol(VisionCommandType visionCommand)
        {
            SetDefault(UniViewProtocolType.Command);
        }
        #endregion


        #region 속성
        public VisionCommandType VisionCommand { get; set; } = VisionCommandType.Unknown;
        #endregion

    }
}
