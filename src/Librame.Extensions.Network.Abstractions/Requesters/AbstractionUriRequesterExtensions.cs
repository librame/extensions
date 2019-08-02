#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    /// <summary>
    /// 抽象 <see cref="IUriRequester"/> 静态扩展。
    /// </summary>
    public static class AbstractionUriRequesterExtensions
    {
        /// <summary>
        /// 异步获取响应字节集合。
        /// </summary>
        /// <param name="requester">给定的 <see cref="IUriRequester"/>。</param>
        /// <param name="url">给定的 URL。</param>
        /// <param name="postData">给定要提交的数据（可选）。</param>
        /// <param name="enableCodec">启用字节编解码传输（可选）。</param>
        /// <param name="parameters">给定的自定义参数（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字节数组的异步操作。</returns>
        public static Task<byte[]> GetResponseBytesAsync(this IUriRequester requester, string url, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
        {
            requester.NotNull(nameof(requester));

            return requester.GetResponseBytesAsync(url.AsAbsoluteUri(), postData, enableCodec, parameters, cancellationToken);
        }

        /// <summary>
        /// 异步获取响应字符串。
        /// </summary>
        /// <param name="requester">给定的 <see cref="IUriRequester"/>。</param>
        /// <param name="url">给定的 URL。</param>
        /// <param name="postData">给定要提交的数据（可选）。</param>
        /// <param name="enableCodec">启用字节编解码传输（可选）。</param>
        /// <param name="parameters">给定的自定义参数（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public static Task<string> GetResponseStringAsync(this IUriRequester requester, string url, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
        {
            requester.NotNull(nameof(requester));

            return requester.GetResponseStringAsync(url.AsAbsoluteUri(), postData, enableCodec, parameters, cancellationToken);
        }

        /// <summary>
        /// 异步获取响应流。
        /// </summary>
        /// <param name="requester">给定的 <see cref="IUriRequester"/>。</param>
        /// <param name="url">给定的 URL。</param>
        /// <param name="postData">给定要提交的数据（可选）。</param>
        /// <param name="enableCodec">启用字节编解码传输（可选）。</param>
        /// <param name="parameters">给定的自定义参数（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="Stream"/> 的异步操作。</returns>
        public static Task<Stream> GetResponseStreamAsync(this IUriRequester requester, string url, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
        {
            requester.NotNull(nameof(requester));

            return requester.GetResponseStreamAsync(url.AsAbsoluteUri(), postData, enableCodec, parameters, cancellationToken);
        }

    }
}
