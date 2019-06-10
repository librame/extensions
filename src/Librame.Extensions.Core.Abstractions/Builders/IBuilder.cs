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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 构建器接口。
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// 父构建器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IBuilder"/>。
        /// </value>
        IBuilder ParentBuilder { get; }

        /// <summary>
        /// 服务集合。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IServiceCollection"/>。
        /// </value>
        IServiceCollection Services { get; }

        /// <summary>
        /// 构建器选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IBuilderOptions"/>。
        /// </value>
        IBuilderOptions Options { get; }
    }
}
