using DynMvp.Authentication;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniView.Client.Core
{
    public class UserAccessInfo // 나중에 Auth 쪽으로 가야되는게 맞기는 한데 TcpIpInfo 때문에 좀 고민중
    {
        #region 속성
        public User UserAccount { get; set; } = new User();

        public TcpIpInfo HostInfo { get; set; } = new TcpIpInfo();
        #endregion
    }
}
