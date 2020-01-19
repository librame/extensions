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
    /// 抽象引导程序。
    /// </summary>
    public abstract class AbstractBootstrapper : AbstractSortable, IBootstrapper
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractBootstrapper"/>。
        /// </summary>
        /// <param name="priority">给定的优先级（可选；默认为 <see cref="AbstractSortable.DefaultPriority"/>）。</param>
        protected AbstractBootstrapper(float? priority = null)
            : base(priority)
        {
        }


        /// <summary>
        /// 运行引导程序。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public abstract IServiceCollection Run(IServiceCollection services);
    }
}
