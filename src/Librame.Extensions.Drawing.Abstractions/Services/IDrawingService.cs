#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Drawing.Services
{
    using Core.Builders;
    using Core.Services;

    /// <summary>
    /// 图画服务接口。
    /// </summary>
    public interface IDrawingService : IService
    {
        /// <summary>
        /// 构建器依赖。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilderDependency"/>。
        /// </value>
        IExtensionBuilderDependency Dependency { get; }
    }
}
