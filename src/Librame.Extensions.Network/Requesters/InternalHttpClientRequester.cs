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
using Polly;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    /// <summary>
    /// 内部 <see cref="HttpClient"/> 请求程序。
    /// </summary>
    internal class InternalHttpClientRequester : UriRequesterBase, IUriRequester
    {
        private readonly IHttpClientFactory _factory;


        /// <summary>
        /// 构造一个 <see cref="InternalHttpClientRequester"/> 实例。
        /// </summary>
        /// <param name="factory">给定的 <see cref="IHttpClientFactory"/>。</param>
        /// <param name="byteCodec">给定的 <see cref="IByteCodecService"/>。</param>
        public InternalHttpClientRequester(IHttpClientFactory factory, IByteCodecService byteCodec)
            : base(byteCodec)
        {
            _factory = factory.NotNull(nameof(factory));
        }


        /// <summary>
        /// 异步获取响应字节数组。
        /// </summary>
        /// <param name="uri">给定的 <see cref="Uri"/>。</param>
        /// <param name="postData">给定要提交的数据（可选）。</param>
        /// <param name="enableCodec">启用字节编解码传输（可选）。</param>
        /// <param name="parameters">给定的自定义参数（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字节数组的异步操作。</returns>
        public override async Task<byte[]> GetResponseBytesAsync(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
        {
            var message = await GetResponseMessage(uri, postData, enableCodec, parameters, cancellationToken);

            var buffer = await message.Content.ReadAsByteArrayAsync();
            if (buffer.IsNotNullOrEmpty())
                return ByteCodec.Decode(buffer, enableCodec);

            return buffer;
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
        public override async Task<string> GetResponseStringAsync(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
        {
            var buffer = await GetResponseBytesAsync(uri, postData, enableCodec, parameters, cancellationToken);
            if (buffer.IsNotNullOrEmpty())
                return buffer.FromEncodingBytes(Encoding);

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
        public override async Task<Stream> GetResponseStreamAsync(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
        {
            var message = await GetResponseMessage(uri, postData, enableCodec, parameters, cancellationToken);
            
            return await message.Content.ReadAsStreamAsync();
        }

        private Task<HttpResponseMessage> GetResponseMessage(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
        {
            var opts = Options.Requester;

            var client = _factory.CreateClient();
            client.Timeout = TimeSpan.FromMilliseconds(opts.Timeout);
            client.DefaultRequestHeaders.Add("User-Agent", opts.UserAgent);

            if (parameters.Accept.IsNotNullOrEmpty())
                client.DefaultRequestHeaders.Add("Accept", parameters.Accept);

            if (parameters.ContentType.IsNotNullOrEmpty())
                client.DefaultRequestHeaders.Add("Content-Type", parameters.ContentType);

            if (parameters.Headers.IsNotNullOrEmpty())
            {
                parameters.Headers.ForEach(pair =>
                {
                    client.DefaultRequestHeaders.Add(pair.Key, pair.Value);
                });
            }

            return Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(p => p.StatusCode == HttpStatusCode.BadGateway)
                .RetryAsync(opts.RetryCount, (exception, retryCount, context) =>
                {
                    Logger.LogDebug($"Start the {retryCount} retry: ");
                })
                .ExecuteAsync(GetResponseMessageAsync);

            async Task<HttpResponseMessage> GetResponseMessageAsync()
            {
                HttpResponseMessage responseMessage;

                if (postData.IsNotNullOrEmpty())
                {
                    var buffer = ByteCodec.EncodeStringAsBytes(postData, enableCodec);
                    var content = new ByteArrayContent(buffer);

                    responseMessage = await client.PostAsync(uri, content, cancellationToken);

                    Logger.LogDebug($"Send data: {postData}");
                    Logger.LogDebug($"Enable codec: {enableCodec}");
                }
                else
                {
                    responseMessage = await client.GetAsync(uri, cancellationToken);
                }

                return responseMessage;
            }
        }

    }
}
