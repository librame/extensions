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
        /// 记录器。
        /// </summary>
        /// <value>返回 <see cref="ILogger"/>。</value>
        protected ILogger Logger { get; }


        /// <summary>
        /// 转换为密文。
        /// </summary>
        /// <param name="output">给定的缓冲区。</param>
        /// <returns>返回字符串。</returns>
        public string From(IByteBuffer output)
        {
            string str = output.AsBase64String();
            Logger.LogDebug($"Convert to BASE64 String: {str}");

            return str;
        }

        /// <summary>
        /// 转换为字节缓冲区。
        /// </summary>
        /// <param name="input">给定的密文字符串。</param>
        /// <returns>返回缓冲区。</returns>
        public IByteBuffer To(string input)
        {
            var buffer = input.AsByteBufferFromBase64String();
            Logger.LogDebug($"From BASE64 String: {input}");

            return buffer;
        }

    }
}
