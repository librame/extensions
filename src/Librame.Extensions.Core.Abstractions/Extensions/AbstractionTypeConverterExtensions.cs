#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace System.ComponentModel
{
    /// <summary>
    /// 抽象类型转换器静态扩展。
    /// </summary>
    public static class AbstractionTypeConverterExtensions
    {
        /// <summary>
        /// 还原字符串为来源。
        /// </summary>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="converter">给定的 <see cref="TypeConverter"/>。</param>
        /// <param name="value">给定的字符串。</param>
        /// <returns>返回 <typeparamref name="TSource"/>。</returns>
        public static TSource ConvertFromString<TSource>(this TypeConverter converter, string value)
            => (TSource)converter?.ConvertFromString(value);
    }
}
