#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System.Text;

namespace Librame.Extensions
{
    using Converters;

    /// <summary>
    /// 转换器静态扩展。
    /// </summary>
    public static class ConverterExtensions
    {

        /// <summary>
        /// 转换为默认字符编码转换器。
        /// </summary>
        /// <param name="encoding">给定的 <see cref="Encoding"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        /// <returns>返回 <see cref="IEncodingConverter"/>。</returns>
        public static IEncodingConverter AsDefaultEncodingConverter(this Encoding encoding, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<DefaultEncodingConverter>();

            return new DefaultEncodingConverter(logger)
            {
                Encoding = encoding.NotDefault(nameof(encoding))
            };
        }

    }
}
