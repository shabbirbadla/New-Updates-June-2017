using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Udyog.Application.License
{
    public static class NetworkHelper
    {
        public static IPAddress FindIPAddress(bool localPreference)
        {
            return FindIPAddress(Dns.GetHostEntry(Dns.GetHostName()), localPreference);
        }

        public static IPAddress FindIPAddress(IPHostEntry host, bool localPreference)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            if (host.AddressList.Length == 1)
                return host.AddressList[0];
            else
            {
                foreach (System.Net.IPAddress address in host.AddressList)
                {
                    bool local = IsLocal(address);

                    if (local && localPreference)
                        return address;
                    else if (!local && !localPreference)
                        return address;
                }

                return host.AddressList[0];
            }
        }

        public static bool IsLocal(IPAddress address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            byte[] addr = address.GetAddressBytes();

            return addr[0] == 10
            || (addr[0] == 192 && addr[1] == 168)
            || (addr[0] == 172 && addr[1] >= 16 && addr[1] <= 31);
        }
    }
}
