using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanS.Common.Exchange
{
    public static class AddressManager
    {
        public static string GetMonitorAddress()
        {

            return string.Format("192.168.0.100");
        }

        public static string GetInspectorAddress(int camIndex, int clientIndex)
        {
            return string.Format("192.168.0.{0}", ((camIndex + 1) * 10) + clientIndex + 100);
        }
    }
}
