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
using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 服务接口。
    /// </summary>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public interface IService<out TBuilderOptions> : IService
        where TBuilderOptions : IBuilderOptions
    {
        /// <summary>
        /// 构建器选项。
        /// </summary>
        /// <value>返回 <typeparamref name="TBuilderOptions"/>。</value>
        TBuilderOptions Options { get; }
    }


    /// <summary>
    /// 服务接口。
    /// </summary>
    public interface IService : IDisposable
    {
        /// <summary>
        /// 记录器工厂。
        /// </summary>
        /// <value>返回 <see cref="ILoggerFactory"/>。</value>
        ILoggerFactory LoggerFactory { get; }
    }
}
