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
using Microsoft.Extensions.Options;
using System.Text;

namespace Librame.Extensions.Encryption
{
    using Core;

    /// <summary>
    /// 密文转换器。
    /// </summary>
    public class PlaintextConverter : IPlaintextConverter
    {
        /// <summary>
        /// 构造一个 <see cref="PlaintextConverter"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{PlaintextConverter}"/>。</param>
        public PlaintextConverter(IOptions<CoreBuilderOptions> options,
            ILogger<PlaintextConverter> logger)
        {
            Encoding = options.NotNull(nameof(options)).Value.Encoding;
            Logger = logger.NotNull(nameof(logger));
        }


        /// <summary>
        /// 字符编码。
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 日志。
        /// </summary>
        /// <value>返回 <see cref="ILogger"/>。</value>
        protected ILogger Logger { get; }


        /// <summary>
        /// 还原 <see cref="IByteMemoryBuffer"/>。
        /// </summary>
        /// <param name="to">给定的密文字符串。</param>
        /// <returns>返回缓冲区。</returns>
        public IByteMemoryBuffer ConvertFrom(string to)
        {
            var buffer = to.AsByteBufferFromEncodingString(Encoding);
            Logger.LogDebug($"From encoding string: {to}");

            return buffer;
        }

        /// <summary>
        /// 转换 <see cref="IByteMemoryBuffer"/>。
        /// </summary>
        /// <param name="from">给定的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <returns>返回字符串。</returns>
        public string ConvertTo(IByteMemoryBuffer from)
        {
            var str = from.AsEncodingString(Encoding);
            Logger.LogDebug($"To encoding string: {str}");

            return str;
        }

    }
}
