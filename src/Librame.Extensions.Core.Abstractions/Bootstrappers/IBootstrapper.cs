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
    /// 引导程序接口。
    /// </summary>
    public interface IBootstrapper : ISortable
    {
        /// <summary>
        /// 运行引导程序。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        IServiceCollection Run(IServiceCollection services);
    }
}
