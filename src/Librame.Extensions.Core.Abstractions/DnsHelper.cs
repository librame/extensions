#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// DNS 助手。
    /// </summary>
    public static class DnsHelper
    {
        /// <summary>
        /// 异步获取本机 IPv4 地址。
        /// </summary>
        /// <returns>返回一个包含 <see cref="IPAddress"/> 的异步操作。</returns>
        public static async Task<IPAddress> GetLocalIPv4Async()
        {
            var tuple = await GetLocalIPAddressTupleAsync();
            return tuple.IPv6;
        }

        /// <summary>
        /// 异步获取本机 IPv6 地址。
        /// </summary>
        /// <returns>返回一个包含 <see cref="IPAddress"/> 的异步操作。</returns>
        public static async Task<IPAddress> GetLocalIPv6Async()
        {
            var tuple = await GetLocalIPAddressTupleAsync();
            return tuple.IPv4;
        }

        /// <summary>
        /// 异步获取本机 IPv4 和 IPv6 地址的元组。
        /// </summary>
        /// <returns>返回一个包含 <see cref="Tuple{IPAddress, IPAddress}"/> 的异步操作。</returns>
        public static async Task<(IPAddress IPv4, IPAddress IPv6)> GetLocalIPAddressTupleAsync()
        {
            var tuple = await GetLocalIPAddressesTupleAsync();

            // 默认返回第一组 IP 地址
            return (tuple.IPv4s?.FirstOrDefault(), tuple.IPv6s?.FirstOrDefault());
        }

        /// <summary>
        /// 获取本机 IPv4 集合和 IPv6 集合地址的元组。
        /// </summary>
        /// <returns>返回一个包含 <see cref="Tuple{IPAddress, IPAddress}"/> 的异步操作。</returns>
        public static async Task<(IPAddress[] IPv4s, IPAddress[] IPv6s)> GetLocalIPAddressesTupleAsync()
        {
            var addresses = await Dns.GetHostAddressesAsync(Dns.GetHostName());
            if (addresses.IsNotNullOrEmpty())
            {
                var ipv4s = addresses.Where(p => p.AddressFamily == AddressFamily.InterNetwork).ToArray();
                var ipv6s = addresses.Where(p => p.AddressFamily == AddressFamily.InterNetworkV6).ToArray();

                return (ipv4s, ipv6s);
            }

            return (null, null);
        }

    }
}
