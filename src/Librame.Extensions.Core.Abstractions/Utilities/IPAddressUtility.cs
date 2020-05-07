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

namespace Librame.Extensions.Core.Utilities
{
    /// <summary>
    /// <see cref="IPAddress"/> 实用工具。
    /// </summary>
    public static class IPAddressUtility
    {
        /// <summary>
        /// 异步获取本机 IPv4 地址。
        /// </summary>
        /// <returns>返回一个包含 <see cref="IPAddress"/> 的异步操作。</returns>
        public static async Task<IPAddress> GetLocalIPv4AddressAsync()
        {
            (var v4, var _) = await GetLocalIPv4AndIPv6AddressAsync().ConfigureAndResultAsync();
            return v4;
        }

        /// <summary>
        /// 异步获取本机 IPv6 地址。
        /// </summary>
        /// <returns>返回一个包含 <see cref="IPAddress"/> 的异步操作。</returns>
        public static async Task<IPAddress> GetLocalIPv6AddressAsync()
        {
            (var _, var v6) = await GetLocalIPv4AndIPv6AddressAsync().ConfigureAndResultAsync();
            return v6;
        }

        /// <summary>
        /// 异步获取本机 IPv4 和 IPv6 地址的元组。
        /// </summary>
        /// <returns>返回一个包含 <see cref="Tuple{IPAddress, IPAddress}"/> 的异步操作。</returns>
        public static async Task<(IPAddress v4, IPAddress v6)> GetLocalIPv4AndIPv6AddressAsync()
        {
            (var v4s, var v6s) = await GetLocalIPv4AndIPv6AddressesAsync().ConfigureAndResultAsync();

            // 默认返回第一组 IP 地址
            return (v4s?.FirstOrDefault(), v6s?.FirstOrDefault());
        }

        /// <summary>
        /// 异步获取本机 IPv4 和 IPv6 地址集合的元组。
        /// </summary>
        /// <returns>返回一个包含 <see cref="Tuple{IPAddress, IPAddress}"/> 的异步操作。</returns>
        public static async Task<(IPAddress[] v4s, IPAddress[] v6s)> GetLocalIPv4AndIPv6AddressesAsync()
        {
            var addresses = await GetLocalAddressesAsync().ConfigureAndResultAsync();
            if (addresses.IsNotEmpty())
            {
                var v4s = addresses.Where(p => p.AddressFamily == AddressFamily.InterNetwork).ToArray();
                var v6s = addresses.Where(p => p.AddressFamily == AddressFamily.InterNetworkV6).ToArray();

                return (v4s, v6s);
            }

            return (null, null);
        }

        /// <summary>
        /// 异步获取本机地址集合。
        /// </summary>
        /// <returns>返回数组。</returns>
        public static Task<IPAddress[]> GetLocalAddressesAsync()
            => Dns.GetHostAddressesAsync(Dns.GetHostName());

    }
}
