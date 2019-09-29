#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    /// <summary>
    /// URI 请求程序基类。
    /// </summary>
    public class UriRequesterBase : NetworkServiceBase, IUriRequester
    {
        /// <summary>
        /// 构造一个 <see cref="UriRequesterBase"/>。
        /// </summary>
        /// <param name="byteCodec">给定的 <see cref="IByteCodecService"/>。</param>
        protected UriRequesterBase(IByteCodecService byteCodec)
            : base(byteCodec.CastTo<IByteCodecService, NetworkServiceBase>(nameof(byteCodec)))
        {
            ByteCodec = byteCodec;
        }


        /// <summary>
        /// 字节编解码器。
        /// </summary>
        /// <value>返回 <see cref="IByteCodecService"/>。</value>
        public IByteCodecService ByteCodec { get; }


        /// <summary>
        /// 异步获取响应字节集合。
        /// </summary>
        /// <param name="uri">给定的 <see cref="Uri"/>。</param>
        /// <param name="postData">给定要提交的数据（可选）。</param>
        /// <param name="enableCodec">启用字节编解码传输（可选）。</param>
        /// <param name="parameters">给定的自定义参数（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字节数组的异步操作。</returns>
        public virtual async Task<byte[]> GetResponseBytesAsync(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
        {
            var stream = await GetResponseStreamAsync(uri, postData, enableCodec, parameters, cancellationToken).ConfigureAndResultAsync();
            if (stream.IsNotNull())
            {
                var bytes = new byte[stream.Length];

                stream.Read(bytes, 0, bytes.Length);

                // 重置流定位到开始处
                stream.Seek(0, SeekOrigin.Begin);

                return ByteCodec.Decode(bytes, enableCodec);
            }

            return null;
        }

        /// <summary>
        /// 异步获取响应字符串。
        /// </summary>
        /// <param name="uri">给定的 <see cref="Uri"/>。</param>
        /// <param name="postData">给定要提交的数据（可选）。</param>
        /// <param name="enableCodec">启用字节编解码传输（可选）。</param>
        /// <param name="parameters">给定的自定义参数（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual async Task<string> GetResponseStringAsync(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
        {
            var buffer = await GetResponseBytesAsync(uri, postData, enableCodec, parameters, cancellationToken).ConfigureAndResultAsync();
            if (buffer.IsNotEmpty())
                return buffer.AsEncodingString(Encoding);

            return null;
        }

        /// <summary>
        /// 异步获取响应流。
        /// </summary>
        /// <param name="uri">给定的 <see cref="Uri"/>。</param>
        /// <param name="postData">给定要提交的数据（可选）。</param>
        /// <param name="enableCodec">启用字节编解码传输（可选）。</param>
        /// <param name="parameters">给定的自定义参数（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="Stream"/> 的异步操作。</returns>
        public virtual Task<Stream> GetResponseStreamAsync(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
    }
}
