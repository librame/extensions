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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// BASE32 算法转换器。
    /// </summary>
    [Serializable]
    public class Base32AlgorithmConverter : IAlgorithmConverter
    {
        /// <summary>
        /// 获取默认只读实例。
        /// </summary>
        [NonSerialized]
        public static readonly Base32AlgorithmConverter Default
            = LazySingleton.GetInstance<Base32AlgorithmConverter>();


        /// <summary>
        /// 还原 <see cref="ReadOnlyMemory{Byte}"/>。
        /// </summary>
        /// <param name="from">给定的 BASE32 字符串。</param>
        /// <returns>返回 <see cref="ReadOnlyMemory{Byte}"/>。</returns>
        public ReadOnlyMemory<byte> ConvertFrom(string from)
            => from.FromBase32String();

        /// <summary>
        /// 转换 <see cref="ReadOnlyMemory{Byte}"/>。
        /// </summary>
        /// <param name="to">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        /// <returns>返回 BASE32 字符串。</returns>
        public string ConvertTo(ReadOnlyMemory<byte> to)
            => to.ToArray().AsBase32String();
    }
}
