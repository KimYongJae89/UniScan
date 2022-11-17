using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniView.Client.Protocol
{
    [Serializable()]
    public class UniViewProtocolBase
    {
        #region 속성
        public string DeviceId { get; set; }

        public UniViewProtocolType PrtocolType { get; set; }

        public DateTime PostedOn { get; set; } = DateTime.Now;

        public bool RequestRemoteControl { get; set; } = false;
        #endregion


        #region 메소드
        protected void SetDefault(UniViewProtocolType protocol)
        {
            PrtocolType = protocol;
            PostedOn = DateTime.Now;
        }

        public void SetDeviceId(string deviceId)
        {
            DeviceId = deviceId;
        }

        public string GetDeviceId()
        {
            return DeviceId;
        }
        #endregion
    }
}
