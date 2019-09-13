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
    /// 16 进制算法转换器。
    /// </summary>
    [Serializable]
    public class HexAlgorithmConverter : IAlgorithmConverter
    {
        /// <summary>
        /// 获取默认只读实例。
        /// </summary>
        [NonSerialized]
        public static readonly HexAlgorithmConverter Default
            = LazySingleton.GetInstance<HexAlgorithmConverter>();


        /// <summary>
        /// 还原 <see cref="ReadOnlyMemory{Byte}"/>。
        /// </summary>
        /// <param name="from">给定的 16 进制字符串。</param>
        /// <returns>返回 <see cref="ReadOnlyMemory{Byte}"/>。</returns>
        public ReadOnlyMemory<byte> From(string from)
            => from.FromHexString();

        /// <summary>
        /// 转换 <see cref="ReadOnlyMemory{Byte}"/>。
        /// </summary>
        /// <param name="to">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        /// <returns>返回 16 进制字符串。</returns>
        public string To(ReadOnlyMemory<byte> to)
            => to.ToArray().AsHexString();
    }
}
