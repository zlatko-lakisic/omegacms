using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MD.Tools.Helpers.Core.Net
{
    /// <summary>
    /// Useful Extension Methods for the Net namespace
    /// </summary>
    public static class NetCoreExtensions
    {

        #region IPAddress - http://www.codeproject.com/KB/cs/IPAddressExtension2.aspx

        private static void CheckIPVersion(IPAddress ipAddress, IPAddress mask, out byte[] addressBytes, out byte[] maskBytes)
        {
            if (mask == null) throw new ArgumentNullException(nameof(mask));
            addressBytes = ipAddress.GetAddressBytes();
            maskBytes = mask.GetAddressBytes();
            if (addressBytes.Length != maskBytes.Length)
            {
                throw new ArgumentException("The address and mask don't use the same IP standard");
            }
        }

        /// <summary>
        /// Ands the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="mask">The mask.</param>
        /// <returns></returns>
        public static IPAddress And(this IPAddress ipAddress, IPAddress mask)
        {
            if(ipAddress == null)
            {
                throw new ArgumentNullException(nameof(ipAddress));
            }

            byte[] addressBytes;
            byte[] maskBytes;
            CheckIPVersion(ipAddress, mask, out addressBytes, out maskBytes);

            byte[] resultBytes = new byte[addressBytes.Length];
            for (int i = 0; i < addressBytes.Length; ++i)
            {
                resultBytes[i] = (byte)(addressBytes[i] & maskBytes[i]);
            }

            return new IPAddress(resultBytes);
        }

        private static IPAddress empty = IPAddress.Parse("0.0.0.0");
        private static IPAddress[] IntranetMasks = MD.Tools.Helpers.Core.Properties.HelperSettings.Default.IntranetMasks.OfType<string>().Select(sip => IPAddress.Parse(sip)).ToArray();
               

        /// <summary>
        /// Retuns true if the ip address is one of the following
        /// IANA-reserved private IPv4 network ranges (from http://en.wikipedia.org/wiki/IP_address)
        /// Start 	      End
        /// 10.0.0.0 	    10.255.255.255
        /// 192.168.0.0   192.168.255.255
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>
        /// 	<c>true</c> if [is on intranet] [the specified ip address]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOnIntranet(this IPAddress ipAddress)
        {
            if (empty.Equals(ipAddress))
            {
                return false;
            }
            bool onIntranet = IPAddress.IsLoopback(ipAddress);
            onIntranet = onIntranet || IntranetMasks.Where(im => im.Equals(ipAddress.And(im))).Any();
          
            return onIntranet;
        }

        #endregion

    }
}
