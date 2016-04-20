/* SocketUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Net;
using System.Net.Sockets;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Net
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class SocketUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Gets IP address from hostname.
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <returns>Resolved IP address of the host.</returns>
        public static IPAddress IPAddressFromHostname(string hostname)
        {
            Code.NotNull(hostname, "hostname");
            if (StringUtility.CompareNoCase(hostname, "localhost")
                 || StringUtility.CompareNoCase(hostname, "local")
                 || StringUtility.CompareNoCase(hostname, "(local)")
                )
            {
                return IPAddress.Loopback;
            }
            IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
            if (hostEntry.AddressList.Length == 0)
            {
                throw new SocketException();
            }
            return
                hostEntry.AddressList
                    [new Random().Next(hostEntry.AddressList.Length)];
        }

        #endregion
    }
}