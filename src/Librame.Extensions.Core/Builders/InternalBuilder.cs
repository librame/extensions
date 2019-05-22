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
    /// 内部构建器。
    /// </summary>
    internal class InternalBuilder : AbstractBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalBuilder"/> 实例。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        public InternalBuilder(IServiceCollection services)
            : base(services)
        {
            Services.AddSingleton<IBuilder>(this);
        }

    }
}
