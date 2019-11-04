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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    /// <summary>
    /// <see cref="HttpClient"/> 请求程序。
    /// </summary>
    public class HttpClientRequester : UriRequesterBase, IUriRequester
    {
        /// <summary>
        /// 构造一个 <see cref="HttpClientRequester"/>。
        /// </summary>
        /// <param name="factory">给定的 <see cref="IHttpClientFactory"/>。</param>
        /// <param name="byteCodec">给定的 <see cref="IByteCodecService"/>。</param>
        public HttpClientRequester(IHttpClientFactory factory, IByteCodecService byteCodec)
            : base(byteCodec, priority: 1)
        {
            Factory = factory.NotNull(nameof(factory));
        }


        /// <summary>
        /// <see cref="HttpClient"/> 工厂。
        /// </summary>
        public IHttpClientFactory Factory { get; }


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
            var message = await GetResponseMessage(uri, postData, enableCodec, parameters, cancellationToken).ConfigureAndResultAsync();

            var buffer = await message.Content.ReadAsByteArrayAsync().ConfigureAndResultAsync();
            if (buffer.IsNotEmpty())
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
        public override async Task<Stream> GetResponseStreamAsync(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
        {
            var message = await GetResponseMessage(uri, postData, enableCodec, parameters, cancellationToken).ConfigureAndResultAsync();
            return await message.Content.ReadAsStreamAsync().ConfigureAndResultAsync();
        }


        /// <summary>
        /// 获取响应消息。
        /// </summary>
        /// <param name="uri">给定的 <see cref="Uri"/>。</param>
        /// <param name="postData">给定要提交的数据（可选）。</param>
        /// <param name="enableCodec">启用字节编解码传输（可选）。</param>
        /// <param name="parameters">给定的自定义参数（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="HttpResponseMessage"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        protected virtual Task<HttpResponseMessage> GetResponseMessage(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
        {
            var opts = Options.Requester;

            var client = Factory.CreateClient();
            client.Timeout = opts.Timeout;
            client.DefaultRequestHeaders.Add("User-Agent", opts.UserAgent);

            if (parameters.Accept.IsNotEmpty())
                client.DefaultRequestHeaders.Add("Accept", parameters.Accept);

            if (parameters.ContentType.IsNotEmpty())
                client.DefaultRequestHeaders.Add("Content-Type", parameters.ContentType);

            if (parameters.Headers.IsNotEmpty())
            {
                parameters.Headers.ForEach(pair =>
                {
                    client.DefaultRequestHeaders.Add(pair.Key, pair.Value);
                });
            }

            return Policy
                .Handle<HttpRequestException>(exception =>
                {
                    Logger.LogError(exception, exception.AsInnerMessage());
                    return true;
                })
                .OrResult<HttpResponseMessage>(result =>
                {
                    Logger.LogTrace(result.StatusCode.ToString());
                    return result.StatusCode == HttpStatusCode.BadGateway;
                })
                .WaitAndRetryAsync(opts.RetryCount, opts.SleepDurationFactory, (exception, sleepDuration, retryCount, context) =>
                {
                    Logger.LogDebug($"Retry {retryCount} times from {sleepDuration.TotalMilliseconds} milliseconds later");
                })
                .ExecuteAsync(GetResponseMessageAsync);

            async Task<HttpResponseMessage> GetResponseMessageAsync()
            {
                HttpResponseMessage responseMessage;

                if (postData.IsNotEmpty())
                {
                    var buffer = ByteCodec.EncodeStringAsBytes(postData, enableCodec);
                    var content = new ByteArrayContent(buffer);

                    responseMessage = await client.PostAsync(uri, content, cancellationToken).ConfigureAndResultAsync();

                    Logger.LogDebug($"Send data: {postData}");
                    Logger.LogDebug($"Enable codec: {enableCodec}");
                }
                else
                {
                    responseMessage = await client.GetAsync(uri, cancellationToken).ConfigureAndResultAsync();
                }

                return responseMessage;
            }
        }

    }
}
