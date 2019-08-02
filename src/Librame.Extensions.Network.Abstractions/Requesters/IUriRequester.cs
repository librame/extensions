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
    /// URI 请求程序接口。
    /// </summary>
    public interface IUriRequester : INetworkService
    {
        /// <summary>
        /// 字节编解码器。
        /// </summary>
        /// <value>返回 <see cref="IByteCodecService"/>。</value>
        IByteCodecService ByteCodec { get; }


        /// <summary>
        /// 异步获取响应字节集合。
        /// </summary>
        /// <param name="uri">给定的 <see cref="Uri"/>。</param>
        /// <param name="postData">给定要提交的数据（可选）。</param>
        /// <param name="enableCodec">启用字节编解码传输（可选）。</param>
        /// <param name="parameters">给定的自定义参数（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字节数组的异步操作。</returns>
        Task<byte[]> GetResponseBytesAsync(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取响应字符串。
        /// </summary>
        /// <param name="uri">给定的 <see cref="Uri"/>。</param>
        /// <param name="postData">给定要提交的数据（可选）。</param>
        /// <param name="enableCodec">启用字节编解码传输（可选）。</param>
        /// <param name="parameters">给定的自定义参数（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<string> GetResponseStringAsync(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取响应流。
        /// </summary>
        /// <param name="uri">给定的 <see cref="Uri"/>。</param>
        /// <param name="postData">给定要提交的数据（可选）。</param>
        /// <param name="enableCodec">启用字节编解码传输（可选）。</param>
        /// <param name="parameters">给定的自定义参数（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="Stream"/> 的异步操作。</returns>
        Task<Stream> GetResponseStreamAsync(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default);
    }
}
