#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;

namespace Librame.Extensions
{
    using Core;

    /// <summary>
    /// 转换器静态扩展。
    /// </summary>
    public static class ConverterExtensions
    {

        /// <summary>
        /// 转换为字符编码转换器。
        /// </summary>
        /// <param name="encoding">给定的 <see cref="Encoding"/>。</param>
        /// <returns>返回 <see cref="IEncodingConverter"/>。</returns>
        public static IEncodingConverter AsEncodingConverter(this Encoding encoding)
        {
            return new EncodingConverter(encoding);
        }

    }
}
