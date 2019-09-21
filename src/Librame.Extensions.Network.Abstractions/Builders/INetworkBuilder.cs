#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network
{
    using Core;

    /// <summary>
    /// 网络构建器接口。
    /// </summary>
    public interface INetworkBuilder : IExtensionBuilder
    {
        /// <summary>
        /// DotNetty 依赖选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilderDependencyOptions"/>。
        /// </value>
        IExtensionBuilderDependencyOptions DotNettyDependencyOptions { get; }


        /// <summary>
        /// 添加 DotNetty 依赖选项。
        /// </summary>
        /// <param name="dependencyOptions">给定的 <see cref="IExtensionBuilderDependencyOptions"/>。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        INetworkBuilder AddDotNettyDependencyOptions(IExtensionBuilderDependencyOptions dependencyOptions);
    }
}
