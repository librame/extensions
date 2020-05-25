#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Core.Starters
{
    /// <summary>
    /// 表示可自动注册的预启动器接口。如果不需要自动注册，请在实现类型中标记 <see cref="NonRegisteredAttribute"/>。
    /// </summary>
    public interface IPreStarter : ISortable
    {
        /// <summary>
        /// 启动预启动器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        IServiceCollection Start(IServiceCollection services);
    }
}
