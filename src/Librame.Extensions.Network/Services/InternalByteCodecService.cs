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
using System;

namespace Librame.Extensions.Network
{
    using Core;

    /// <summary>
    /// 内部字节编解码服务。
    /// </summary>
    internal class InternalByteCodecService : NetworkServiceBase, IByteCodecService
    {
        private readonly IServiceProvider _serviceProvider;


        /// <summary>
        /// 构造一个 <see cref="InternalByteCodecService"/> 实例。
        /// </summary>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalByteCodecService(IServiceProvider serviceProvider,
            IOptions<CoreBuilderOptions> coreOptions, IOptions<NetworkBuilderOptions> options,
            ILoggerFactory loggerFactory)
            : base(coreOptions, options, loggerFactory)
        {
            _serviceProvider = serviceProvider.NotNull(nameof(serviceProvider));
        }


        /// <summary>
        /// 从字节数组解码字符串。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="enableCodec">启用编解码。</param>
        /// <returns>返回原始字符串。</returns>
        public string DecodeStringFromBytes(byte[] buffer, bool enableCodec)
        {
            buffer = enableCodec ? Decode(buffer) : buffer;

            return buffer.FromEncodingBytes(Encoding);
        }

        /// <summary>
        /// 解码字符串。
        /// </summary>
        /// <param name="encode">给定的编码字符串。</param>
        /// <param name="enableCodec">启用编解码。</param>
        /// <returns>返回原始字符串。</returns>
        public string DecodeString(string encode, bool enableCodec)
        {
            var buffer = encode.FromBase64String();
            buffer = enableCodec ? Decode(buffer) : buffer;

            return buffer.FromEncodingBytes(Encoding);
        }

        /// <summary>
        /// 解码字节数组。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回经过解码的字节数组。</returns>
        public byte[] Decode(byte[] buffer)
        {
            return Options.ByteCodec.DecodeFactory?.Invoke(_serviceProvider, buffer) ?? buffer;
        }


        /// <summary>
        /// 编码字符串为字节数组。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="enableCodec">启用编解码。</param>
        /// <returns>返回经过编码的字节数组。</returns>
        public byte[] EncodeStringAsBytes(string str, bool enableCodec)
        {
            var buffer = str.AsEncodingBytes(Encoding);

            return enableCodec ? Encode(buffer) : buffer;
        }

        /// <summary>
        /// 编码字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="enableCodec">启用编解码。</param>
        /// <returns>返回经过编码的字符串。</returns>
        public string EncodeString(string str, bool enableCodec)
        {
            var buffer = str.AsEncodingBytes(Encoding);
            buffer = enableCodec ? Encode(buffer) : buffer;

            return buffer.AsBase64String();
        }

        /// <summary>
        /// 编码字节数组。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回经过编码的字节数组。</returns>
        public byte[] Encode(byte[] buffer)
        {
            return Options.ByteCodec.EncodeFactory?.Invoke(_serviceProvider, buffer) ?? buffer;
        }

    }
}
