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

namespace Librame.Converters
{
    using Buffers;
    using Extensions;

    /// <summary>
    /// 字符编码转换器。
    /// </summary>
    public class EncodingConverter : AbstractConverter<string, IByteBuffer>, IEncodingConverter
    {
        /// <summary>
        /// 构造一个 <see cref="EncodingConverter"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{EncodingConverter}"/>。</param>
        public EncodingConverter(ILogger<EncodingConverter> logger)
        {
            Logger = logger;
        }


        /// <summary>
        /// 记录器。
        /// </summary>
        /// <value>返回 <see cref="ILogger"/>。</value>
        protected ILogger Logger { get; }

        /// <summary>
        /// 字符编码（默认使用 <see cref="Encoding.UTF8"/>）。
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;


        /// <summary>
        /// 转换为字节缓冲区。
        /// </summary>
        /// <param name="source">给定的明文字符串。</param>
        /// <returns>返回缓冲区。</returns>
        public override IByteBuffer ToResult(string source)
        {
            var buffer = source.AsEncodingBytes(Encoding).AsByteBuffer();
            Logger.LogDebug($"Encoding to bytes: {source}");

            return buffer;
        }

        /// <summary>
        /// 转换为明文。
        /// </summary>
        /// <param name="result">给定的缓冲区。</param>
        /// <returns>返回字符串。</returns>
        public override string ToSource(IByteBuffer result)
        {
            var str = result.Memory.ToArray().FromEncodingBytes(Encoding);
            Logger.LogDebug($"Encoding to string: {str}");

            return str;
        }

    }
}
