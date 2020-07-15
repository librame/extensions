#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.Requesters
{
    using Core;
    using Network.Services;

    /// <summary>
    /// URI 请求程序基类。
    /// </summary>
    public class UriRequesterBase : NetworkServiceBase, IUriRequester
    {
        /// <summary>
        /// 构造一个 <see cref="UriRequesterBase"/>。
        /// </summary>
        /// <param name="byteCodec">给定的 <see cref="IByteCodecService"/>。</param>
        /// <param name="priority">给定的服务优先级（数值越小越优先）。</param>
        protected UriRequesterBase(IByteCodecService byteCodec, float priority)
            : base(byteCodec.CastTo<IByteCodecService, NetworkServiceBase>(nameof(byteCodec)))
        {
            ByteCodec = byteCodec;
            Priority = priority;
        }


        /// <summary>
        /// 字节编解码器。
        /// </summary>
        /// <value>返回 <see cref="IByteCodecService"/>。</value>
        public IByteCodecService ByteCodec { get; }

        /// <summary>
        /// 服务优先级（数值越小越优先）。
        /// </summary>
        public float Priority { get; set; }


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
            var stream = await GetResponseStreamAsync(uri, postData, enableCodec, parameters, cancellationToken).ConfigureAwait();
            if (stream.IsNotNull())
            {
                var bytes = Array.Empty<byte>();

                using (var ms = new MemoryStream())
                {
                    var buffer = new byte[64 * Options.BufferSize];

                    int length;
                    while ((length = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, length);
                    }

                    bytes = ms.ToArray();
                }

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
            var buffer = await GetResponseBytesAsync(uri, postData, enableCodec, parameters, cancellationToken).ConfigureAwait();
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


        /// <summary>
        /// 比较优先级。
        /// </summary>
        /// <param name="other">给定的 <see cref="ISortable"/>。</param>
        /// <returns>返回整数。</returns>
        public virtual int CompareTo(ISortable other)
            => Priority.CompareTo((float)other?.Priority);


        /// <summary>
        /// 优先级相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is UriRequesterBase sortable && Priority == sortable?.Priority;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
            => Priority.GetHashCode();


        /// <summary>
        /// 相等比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="UriRequesterBase"/>。</param>
        /// <param name="right">给定的 <see cref="UriRequesterBase"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(UriRequesterBase left, UriRequesterBase right)
            => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.Equals(right);

        /// <summary>
        /// 不等比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="UriRequesterBase"/>。</param>
        /// <param name="right">给定的 <see cref="UriRequesterBase"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(UriRequesterBase left, UriRequesterBase right)
            => !(left == right);

        /// <summary>
        /// 小于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="UriRequesterBase"/>。</param>
        /// <param name="right">给定的 <see cref="UriRequesterBase"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <(UriRequesterBase left, UriRequesterBase right)
            => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        /// <summary>
        /// 小于等于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="UriRequesterBase"/>。</param>
        /// <param name="right">给定的 <see cref="UriRequesterBase"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <=(UriRequesterBase left, UriRequesterBase right)
            => ReferenceEquals(left, null) || left.CompareTo(right) <= 0;

        /// <summary>
        /// 大于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="UriRequesterBase"/>。</param>
        /// <param name="right">给定的 <see cref="UriRequesterBase"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >(UriRequesterBase left, UriRequesterBase right)
            => !ReferenceEquals(left, null) && left.CompareTo(right) > 0;

        /// <summary>
        /// 大于等于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="UriRequesterBase"/>。</param>
        /// <param name="right">给定的 <see cref="UriRequesterBase"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >=(UriRequesterBase left, UriRequesterBase right)
            => ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
}
