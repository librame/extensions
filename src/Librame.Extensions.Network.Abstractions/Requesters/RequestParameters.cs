#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Network.Requesters
{
    /// <summary>
    /// 请求参数集。
    /// </summary>
    public struct RequestParameters : IEquatable<RequestParameters>
    {
        /// <summary>
        /// 接受的内容类型。
        /// </summary>
        public string Accept { get; set; }

        /// <summary>
        /// 内容类型。
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 头部信息。
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public Dictionary<string, string> Headers { get; set; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="RequestParameters"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(RequestParameters other)
        {
            if (!Accept.Equals(other.Accept, StringComparison.OrdinalIgnoreCase))
                return false;

            return ContentType.Equals(other.ContentType, StringComparison.OrdinalIgnoreCase);
            //return Headers.SequenceEqual(other.Headers);
        }

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is RequestParameters other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
            => Accept.CompatibleGetHashCode() ^ ContentType.CompatibleGetHashCode();


        /// <summary>
        /// 比较相等。
        /// </summary>
        /// <param name="left">给定的 <see cref="RequestParameters"/>。</param>
        /// <param name="right">给定的 <see cref="RequestParameters"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(RequestParameters left, RequestParameters right)
            => left.Equals(right);

        /// <summary>
        /// 比较不等。
        /// </summary>
        /// <param name="left">给定的 <see cref="RequestParameters"/>。</param>
        /// <param name="right">给定的 <see cref="RequestParameters"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(RequestParameters left, RequestParameters right)
            => !left.Equals(right);
    }
}
