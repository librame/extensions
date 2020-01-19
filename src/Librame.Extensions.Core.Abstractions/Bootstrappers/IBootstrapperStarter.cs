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

namespace Librame.Extensions.Core.Bootstrappers
{
    /// <summary>
    /// 引导程序启动器接口。
    /// </summary>
    public interface IBootstrapperStarter
    {
        /// <summary>
        /// 启动启动器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        IServiceCollection Start(IServiceCollection services);
    }
}
