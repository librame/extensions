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
    /// BASE64 算法转换器。
    /// </summary>
    [Serializable]
    public class Base64AlgorithmConverter : IAlgorithmConverter
    {
        /// <summary>
        /// 获取默认只读实例。
        /// </summary>
        [NonSerialized]
        public static readonly Base64AlgorithmConverter Default
            = LazySingleton.GetInstance<Base64AlgorithmConverter>();


        /// <summary>
        /// 还原 <see cref="ReadOnlyMemory{Byte}"/>。
        /// </summary>
        /// <param name="from">给定的 BASE64 字符串。</param>
        /// <returns>返回 <see cref="ReadOnlyMemory{Byte}"/>。</returns>
        public ReadOnlyMemory<byte> From(string from)
            => from.FromBase64String();

        /// <summary>
        /// 转换 <see cref="ReadOnlyMemory{Byte}"/>。
        /// </summary>
        /// <param name="to">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        /// <returns>返回 BASE64 字符串。</returns>
        public string To(ReadOnlyMemory<byte> to)
            => to.ToArray().AsBase64String();
    }
}
