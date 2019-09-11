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
    using Core;

    /// <summary>
    /// 密文转换器。
    /// </summary>
    public class CiphertextConverter : ICiphertextConverter
    {
        /// <summary>
        /// 构造一个 <see cref="CiphertextConverter"/>。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{CiphertextConverter}"/>。</param>
        public CiphertextConverter(ILogger<CiphertextConverter> logger)
        {
            Logger = logger;
        }


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
        public IByteMemoryBuffer From(string to)
        {
            var buffer = to.AsByteBufferFromBase64String();
            Logger.LogDebug($"From BASE64 String: {to}");

            return buffer;
        }

        /// <summary>
        /// 转换 <see cref="IByteMemoryBuffer"/>。
        /// </summary>
        /// <param name="from">给定的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <returns>返回字符串。</returns>
        public string To(IByteMemoryBuffer from)
        {
            string str = from.AsBase64String();
            Logger.LogDebug($"Convert to BASE64 String: {str}");

            return str;
        }

    }
}
