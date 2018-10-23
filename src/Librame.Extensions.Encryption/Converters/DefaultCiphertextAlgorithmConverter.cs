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

namespace Librame.Extensions.Encryption
{
    using Buffers;
    using Converters;

    /// <summary>
    /// 默认密文算法转换器。
    /// </summary>
    public class DefaultCiphertextAlgorithmConverter : AbstractConverter<string, IBuffer<byte>>, ICiphertextAlgorithmConverter
    {
        /// <summary>
        /// 记录器。
        /// </summary>
        protected ILogger Logger;


        /// <summary>
        /// 构造一个 <see cref="DefaultCiphertextAlgorithmConverter"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{DefaultCiphertextAlgorithmConverter}"/>。</param>
        public DefaultCiphertextAlgorithmConverter(ILogger<DefaultCiphertextAlgorithmConverter> logger)
        {
            Logger = logger;
        }


        /// <summary>
        /// 转换为字节缓冲区。
        /// </summary>
        /// <param name="source">给定的密文字符串。</param>
        /// <returns>返回缓冲区。</returns>
        public override IBuffer<byte> ToResult(string source)
        {
            var buffer = source.FromBase64StringAsBuffer();
            Logger.LogDebug($"From BASE64 String: {source}");

            return buffer;
        }

        /// <summary>
        /// 转换为密文。
        /// </summary>
        /// <param name="result">给定的缓冲区。</param>
        /// <returns>返回字符串。</returns>
        public override string ToSource(IBuffer<byte> result)
        {
            string str = result.AsBase64String();
            Logger.LogDebug($"Convert to BASE64 String: {str}");

            return str;
        }

    }
}
