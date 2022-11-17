using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniView.Client.Core
{
    public class RabbitMqRoutingKey
    {
        #region 속성
        /// <summary>
        /// 채널 방 
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 클라언트 단에서는 지정할 필요는 없으나 필요시 추가
        /// 커맨드 받을때엔 반드시 지정해 줘야 합니다.
        /// </summary>
        public string RecvTopicKey { get; set; }

        /// <summary>
        /// 보내는 라우팅 키
        /// </summary>
        public string SendTopicKey { get; set; }
        #endregion
    }
}
