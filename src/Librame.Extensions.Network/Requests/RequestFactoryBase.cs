﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Options;
using System;

namespace Librame.Extensions.Network
{
    using Core;

    /// <summary>
    /// 请求工厂基类。
    /// </summary>
    /// <typeparam name="TRequest">指定的请求类型。</typeparam>
    public class RequestFactoryBase<TRequest> : CoreBuilderOptionsEncodingBase, IRequestFactory<TRequest>
        where TRequest : class
    {
        /// <summary>
        /// 构造一个 <see cref="RequestFactoryBase{TRequest}"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        protected RequestFactoryBase(IOptions<NetworkBuilderOptions> options,
            IOptions<CoreBuilderOptions> coreOptions)
            : base(coreOptions)
        {
            Options = options.NotNull(nameof(options)).Value;
        }


        /// <summary>
        /// 网络构建器选项。
        /// </summary>
        public NetworkBuilderOptions Options { get; }


        /// <summary>
        /// 创建请求。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="method">给定的请求方法（可选；默认 POST）。</param>
        /// <returns>返回 <typeparamref name="TRequest"/>。</returns>
        public virtual TRequest CreateRequest(string url, string method = "POST")
        {
            throw new NotImplementedException();
        }
    }
}
