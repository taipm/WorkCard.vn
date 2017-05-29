using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CafeT.Text
{
    public static class IpOnText
    {
        public static IPAddress ToIp(this string text)
        {
            System.Net.IPAddress _ipAddress = System.Net.IPAddress.Parse(text);
            return _ipAddress;
        }
    }
}
