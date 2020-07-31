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
    /// 抽象预启动器。
    /// </summary>
    public abstract class AbstractPreStarter : AbstractSortable, IPreStarter
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractPreStarter"/>。
        /// </summary>
        /// <param name="priority">给定的优先级（可选；默认为 <see cref="AbstractSortable.DefaultPriority"/>）。</param>
        protected AbstractPreStarter(float? priority = null)
            : base(priority)
        {
        }


        /// <summary>
        /// 是否启动。
        /// </summary>
        public bool IsStarting { get; protected set; }


        /// <summary>
        /// 启动。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public virtual IServiceCollection Start(IServiceCollection services)
        {
            if (!IsStarting)
            {
                StartCore(services);
            }

            IsStarting = true;

            return services;
        }

        /// <summary>
        /// 启动核心。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public abstract IServiceCollection StartCore(IServiceCollection services);
    }
}
