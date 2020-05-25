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
    /// 预启动器工厂接口。
    /// </summary>
    public interface IPreStarterFactory
    {
        /// <summary>
        /// 创建预启动器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        IServiceCollection Create(IServiceCollection services);
    }
}
