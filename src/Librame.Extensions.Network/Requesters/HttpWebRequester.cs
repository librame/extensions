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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    /// <summary>
    /// <see cref="HttpWebRequest"/> 请求程序。
    /// </summary>
    public class HttpWebRequester : UriRequesterBase, IUriRequester
    {
        /// <summary>
        /// 构造一个 <see cref="HttpWebRequester"/>。
        /// </summary>
        /// <param name="byteCodec">给定的 <see cref="IByteCodecService"/>。</param>
        public HttpWebRequester(IByteCodecService byteCodec)
            : base(byteCodec)
        {
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
        public override Task<Stream> GetResponseStreamAsync(Uri uri, string postData = null,
            bool enableCodec = false, RequestParameters parameters = default, CancellationToken cancellationToken = default)
        {
            uri.NotNull(nameof(uri));

            HttpWebRequest request;

            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var opts = Options.Requester;

                request = WebRequest.CreateHttp(uri.ToString());
                request.Timeout = (int)opts.Timeout.TotalMilliseconds;
                request.UserAgent = opts.UserAgent;

                if (parameters.Accept.IsNotNullOrEmpty())
                    request.Accept = parameters.Accept;

                if (parameters.ContentType.IsNotNullOrEmpty())
                    request.ContentType = parameters.ContentType;

                if (parameters.Headers.IsNotNullOrEmpty())
                {
                    parameters.Headers.ForEach(pair =>
                    {
                        request.Headers.Add(pair.Key, pair.Value);
                    });
                }

                if (postData.IsNotNullOrEmpty())
                {
                    request.ContentType = $"application/x-www-form-urlencoded; charset={Encoding.AsName()}";
                    request.Method = "POST";
                }

                return Policy
                    .Handle<IOException>(exception =>
                    {
                        Logger.LogError(exception, exception.AsInnerMessage());
                        return true;
                    })
                    .OrResult<Stream>(r => r.IsNull())
                    .WaitAndRetry(opts.RetryCount, opts.SleepDurationFactory, (exception, sleepDuration, retryCount, context) =>
                    {
                        Logger.LogDebug($"Retry {retryCount} times from {sleepDuration.TotalMilliseconds} milliseconds later");
                    })
                    .Execute(GetResponseStream);
            });

            Stream GetResponseStream()
            {
                if (postData.IsNotNullOrEmpty())
                {
                    var buffer = ByteCodec.EncodeStringAsBytes(postData, enableCodec);
                    request.ContentLength = buffer.Length;

                    var stream = request.GetRequestStream();
                    stream.Write(buffer, 0, buffer.Length);

                    Logger.LogDebug($"Send data: {postData}");
                    Logger.LogDebug($"Enable codec: {enableCodec}");
                }

                return request.GetResponse().GetResponseStream();
            }
        }

    }
}
