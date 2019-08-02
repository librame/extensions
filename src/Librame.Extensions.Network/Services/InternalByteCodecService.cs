#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
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
        /// 构造一个 <see cref="InternalByteCodecService"/>。
        /// </summary>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        public InternalByteCodecService(IServiceProvider serviceProvider)
            : base(serviceProvider?.GetService<IOptions<CoreBuilderOptions>>(),
                  serviceProvider?.GetService<IOptions<NetworkBuilderOptions>>(),
                  serviceProvider?.GetService<ILoggerFactory>())
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// 从字节数组解码字符串。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="enableCodec">启用编解码。</param>
        /// <returns>返回原始字符串。</returns>
        public string DecodeStringFromBytes(byte[] buffer, bool enableCodec)
        {
            buffer = Decode(buffer, enableCodec);

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
            buffer = Decode(buffer, enableCodec);

            return buffer.FromEncodingBytes(Encoding);
        }

        /// <summary>
        /// 解码字节数组。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="enableCodec">启用编解码。</param>
        /// <returns>返回经过解码的字节数组。</returns>
        public byte[] Decode(byte[] buffer, bool enableCodec)
        {
            if (enableCodec)
                return Options.ByteCodec.DecodeFactory?.Invoke(_serviceProvider, buffer) ?? buffer;

            return buffer;
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

            return Encode(buffer, enableCodec);
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
            buffer = Encode(buffer, enableCodec);

            return buffer.AsBase64String();
        }

        /// <summary>
        /// 编码字节数组。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="enableCodec">启用编解码。</param>
        /// <returns>返回经过编码的字节数组。</returns>
        public byte[] Encode(byte[] buffer, bool enableCodec)
        {
            if (enableCodec)
                return Options.ByteCodec.EncodeFactory?.Invoke(_serviceProvider, buffer) ?? buffer;

            return buffer;
        }

    }
}
