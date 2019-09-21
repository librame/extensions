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
    /// 扩展构建器接口。
    /// </summary>
    public interface IExtensionBuilder
    {
        /// <summary>
        /// 父构建器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilder"/>。
        /// </value>
        IExtensionBuilder ParentBuilder { get; }


        /// <summary>
        /// 服务集合。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IServiceCollection"/>。
        /// </value>
        IServiceCollection Services { get; }

        /// <summary>
        /// 依赖选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilderDependencyOptions"/>。
        /// </value>
        IExtensionBuilderDependencyOptions DependencyOptions { get; }
    }
}
